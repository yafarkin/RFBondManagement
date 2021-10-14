using RfFondPortfolio.Common.Dtos;
using System;
using System.Linq;
using System.Windows.Forms;
using RfBondManagement.WinForm.Forms;
using RfFondPortfolio.Common.Interfaces;
using Unity;

namespace RfBondManagement.WinForm.Controls
{
    public partial class PortfolioTreeUC : UserControl
    {
        public Portfolio Portfolio { get; set; }

        [Dependency]
        public IPortfolioRepository PortfolioRepository { get; set; }

        [Dependency]
        public IUnityContainer DiContainer { get; set; }

        public PortfolioStructureLeaf SelectedLeaf
        {
            get
            {
                var selectedItem = tvPortfolio.SelectedNode?.Tag;
                if (null == selectedItem)
                {
                    return null;
                }

                if (selectedItem is PortfolioStructureLeaf leaf)
                {
                    return leaf;
                }
                
                if (selectedItem is PortfolioStructureLeafPaper paperLeaf)
                {
                    return tvPortfolio.SelectedNode.Parent?.Tag as PortfolioStructureLeaf;
                }

                return null;
            }
        }

        public PortfolioStructureLeafPaper SelectedLeafPaper
        {
            get
            {
                var selectedItem = tvPortfolio.SelectedNode?.Tag;
                if (selectedItem is PortfolioStructureLeafPaper paperLeaf)
                {
                    return tvPortfolio.SelectedNode.Tag as PortfolioStructureLeafPaper;
                }

                return null;
            }
        }

        public PortfolioTreeUC()
        {
            InitializeComponent();
        }

        private void PortfolioTreeUC_Load(object sender, EventArgs e)
        {
            if (null == Portfolio)
            {
                return;
            }

            DataBind();
        }

        public void DataBind()
        {
            var selectedItem = tvPortfolio.SelectedNode?.Tag;

            tvPortfolio.Nodes.Clear();

            BindNode(tvPortfolio.Nodes, Portfolio.RootLeaf, tvPortfolio, selectedItem);

            tvPortfolio.ExpandAll();
        }

        protected TreeNode BindNode(TreeNodeCollection nc, PortfolioStructureLeaf leaf, TreeView tv, object selectedItem)
        {
            if (null == leaf)
            {
                return null;
            }

            var totalVolume = leaf.Parent?.Children.Sum(x => x.Volume) ?? leaf.Volume;

            var node = new TreeNode
            {
                Text = $"{leaf.Volume / totalVolume:P} ({leaf.Volume:N}), {leaf.Name}",
                Tag = leaf,
                ImageKey = "group",
                SelectedImageKey = "group"
            };
            nc.Add(node);

            TreeNode selectedNode = null;

            if (selectedItem == leaf)
            {
                selectedNode = node;
            }

            if (leaf.Papers != null)
            {
                totalVolume = leaf.Papers.Sum(x => x.Volume);

                foreach (var leafPaper in leaf.Papers)
                {
                    var paperNode = new TreeNode
                    {
                        Text = $"{leafPaper.Volume/totalVolume:P} ({leafPaper.Volume:N}), {leafPaper.Paper.SecId} ({leafPaper.Paper.ShortName})",
                        Tag = leafPaper,
                        ImageKey = "paper",
                        SelectedImageKey = "paper"
                    };
                    node.Nodes.Add(paperNode);

                    if (selectedItem == leafPaper)
                    {
                        selectedNode = paperNode;
                    }
                }
            }

            if (leaf.Children != null)
            {
                foreach (var leafChild in leaf.Children)
                {
                    BindNode(node.Nodes, leafChild, tv, selectedItem);
                }
            }

            if (selectedNode != null)
            {
                tv.SelectedNode = selectedNode;
            }

            return node;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var f = DiContainer.Resolve<StructureLeafEditForm>())
            {
                if (null == SelectedLeaf && tvPortfolio.HasChildren)
                {
                    MessageBox.Show("Добавлять можно только в дочерние записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                f.RootLeaf = Portfolio.RootLeaf;
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                if (null == SelectedLeaf)
                {
                    Portfolio.RootLeaf = f.Leaf;
                }
                else
                {
                    f.Leaf.Parent = SelectedLeaf;
                    SelectedLeaf.Children.Add(f.Leaf);
                }

                PortfolioRepository.Update(Portfolio);

                DataBind();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            using (var f = DiContainer.Resolve<StructureLeafEditForm>())
            {
                f.RootLeaf = Portfolio.RootLeaf;
                f.Leaf = SelectedLeaf;
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                PortfolioRepository.Update(Portfolio);

                DataBind();
            }
        }

        private bool DeleteLeafOrPaper(PortfolioStructureLeaf leaf, PortfolioStructureLeaf leafToDelete, PortfolioStructureLeafPaper leafPaperToDelete)
        {
            if (leafPaperToDelete != null)
            {
                var paperItem = leaf.Papers?.FirstOrDefault(x => x == leafPaperToDelete);
                if (paperItem != null)
                {
                    leaf.Papers.Remove(paperItem);
                    return true;
                }
            }
            else
            {
                var childLeaf = leaf.Children.FirstOrDefault(x => x == leafToDelete);
                if (childLeaf != null)
                {
                    leaf.Children.Remove(childLeaf);
                    return true;
                }
            }

            foreach (var child in leaf.Children)
            {
                var isDeleted = DeleteLeafOrPaper(child, leafToDelete, leafPaperToDelete);
                if (isDeleted)
                {
                    return true;
                }
            }

            return false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (null == SelectedLeaf)
            {
                return;
            }

            if (MessageBox.Show("Подтвердите удаление записи.", "Удаление бумаги", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            if (Portfolio.RootLeaf == SelectedLeaf)
            {
                Portfolio.RootLeaf = null;
            }
            else if(SelectedLeaf != null || SelectedLeafPaper != null)
            {
                DeleteLeafOrPaper(Portfolio.RootLeaf, SelectedLeaf, SelectedLeafPaper);
            }

            PortfolioRepository.Update(Portfolio);
            DataBind();
        }
    }
}

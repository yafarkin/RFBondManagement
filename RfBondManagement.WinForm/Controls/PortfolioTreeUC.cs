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
        public IUnityContainer Container { get; set; }

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
                else if (selectedItem is PortfolioStructureLeafPaper paperLeaf)
                {
                    return tvPortfolio.SelectedNode.Parent?.Tag as PortfolioStructureLeaf;
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

            var rootLeaf = Portfolio.RootLeaf;
            var rootNode = BindNode(rootLeaf, tvPortfolio, selectedItem);

            if (rootNode != null)
            {
                tvPortfolio.Nodes.Add(rootNode);
            }
        }

        protected TreeNode BindNode(PortfolioStructureLeaf leaf, TreeView tv, object selectedItem)
        {
            if (null == leaf)
            {
                return null;
            }

            var totalVolume = leaf.Parent?.Children.Sum(x => x.Volume) ?? leaf.Volume;

            var node = new TreeNode
            {
                Text = $"{leaf.Volume/totalVolume:P} ({leaf.Volume:N}), {leaf.Name}",
                Tag = leaf,
                ImageKey = "group"
            };

            if (selectedItem == leaf)
            {
                tv.SelectedNode = node;
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
                        ImageKey = "paper"
                    };
                    node.Nodes.Add(paperNode);

                    if (selectedItem == leafPaper)
                    {
                        tv.SelectedNode = paperNode;
                    }
                }
            }

            if (leaf.Children != null)
            {
                foreach (var leafChild in leaf.Children)
                {
                    node.Nodes.Add(BindNode(leafChild, tv, selectedItem));
                }
            }

            return node;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var f = Container.Resolve<StructureLeafEditForm>())
            {
                if (null == SelectedLeaf && tvPortfolio.HasChildren)
                {
                    MessageBox.Show("Добавлять можно только в дочерние записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
                    SelectedLeaf.Children.Add(f.Leaf);
                }

                DataBind();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            using (var f = Container.Resolve<StructureLeafEditForm>())
            {
                f.Leaf = SelectedLeaf;
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                DataBind();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}

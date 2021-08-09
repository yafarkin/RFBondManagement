﻿using RfFondPortfolio.Common.Dtos;
using System;
using System.Linq;
using System.Windows.Forms;

namespace RfBondManagement.WinForm.Controls
{
    public partial class PortfolioTreeUC : UserControl
    {
        public Portfolio Portfolio { get; set; }

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
            var selectedItem = tvPortfolio.SelectedNode.Tag;

            tvPortfolio.Nodes.Clear();

            var rootLeaf = Portfolio.RootLeaf;
            var rootNode = BindNode(rootLeaf, tvPortfolio, selectedItem);
            tvPortfolio.Nodes.Add(rootNode);
        }

        protected TreeNode BindNode(PortfolioStructureLeaf leaf, TreeView tv, object selectedItem)
        {
            var totalVolume = null == leaf.Parent ? leaf.Volume : leaf.Parent.Children.Sum(x => x.Volume);

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
    }
}

using RfFondPortfolio.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RfBondManagement.WinForm.Forms
{
    public partial class StructureLeafEditForm : Form
    {
        public PortfolioStructureLeaf Leaf { get; set; }

        public StructureLeafEditForm()
        {
            InitializeComponent();
        }

        private void StructureLeafEditForm_Load(object sender, EventArgs e)
        {
            if (null == Leaf)
            {
                return;
            }

            tbName.Text = Leaf.Name;
            tbPercent.Text = Leaf.PercentLimit.ToString();

            if (Leaf.Papers != null)
            {
                foreach (var leafPaper in Leaf.Papers)
                {
                    var lvi = new ListViewItem(new[]
                    {
                        leafPaper.PaperPercent.ToString(),
                        leafPaper.Paper.SecId,
                        leafPaper.Paper.ShortName
                    })
                    {
                        Tag = leafPaper.Paper
                    };

                    lvPapers.Items.Add(lvi);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Leaf ??= new PortfolioStructureLeaf();

            Leaf.Name = tbName.Text.Trim();
            Leaf.PercentLimit = Convert.ToDecimal(tbPercent.Text.Trim());

            if (Leaf.Papers == null)
            {
                Leaf.Papers = new List<PortfolioStructureLeafPaper>();
            }

            var cloneList = new List<PortfolioStructureLeafPaper>(Leaf.Papers);
            foreach (var leafPaper in cloneList)
            {
                var lvi = lvPapers.FindItemWithText(leafPaper.Paper.SecId);
                if (null == lvi)
                {
                    Leaf.Papers.Remove(leafPaper);
                }
                else
                {
                    leafPaper.PaperPercent = Convert.ToDecimal(lvi.SubItems[0].Text);
                }
            }

            foreach (ListViewItem lvi in lvPapers.Items)
            {
                var leafPaper = Leaf.Papers.FirstOrDefault(x => x.Paper.SecId == lvi.SubItems[0].Text);
                if (null == leafPaper)
                {
                    Leaf.Papers.Add(new PortfolioStructureLeafPaper
                    {
                        Paper = lvi.Tag as AbstractPaper,
                        PaperPercent = Convert.ToDecimal(lvi.SubItems[0].Text)
                    });
                }
                else
                {
                    leafPaper.PaperPercent = Convert.ToDecimal(lvi.SubItems[0].Text);
                }
            }
        }
    }
}

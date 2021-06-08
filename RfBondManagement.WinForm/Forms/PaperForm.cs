using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Integration.Moex;

namespace RfBondManagement.WinForm.Forms
{
    public partial class PaperForm : Form
    {
        public BaseStockPaper Paper;

        public PaperForm()
        {
            InitializeComponent();

            DataBind();
        }

        private void DataBind()
        {
            tbSecId.Text = Paper?.SecId;
            tbName.Text = Paper?.Name;
            tbShortName.Text = Paper?.ShortName;
            tbIsin.Text = Paper?.Isin;
            tbFaceValue.Text = Paper?.FaceValue?.ToString();
            tbIssueDate.Text = Paper?.IssueDate?.ToShortDateString();
            tbTypeName.Text = Paper?.TypeName;
            tbGroup.Text = Paper?.Group;
            tbType.Text = Paper?.Type;
            tbGroupName.Text = Paper?.GroupName;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSearch.Text))
            {
                return;
            }

            var request = new MoexPaperDefinitionRequest(tbSearch.Text.Trim());
            var response = request.Read();
            Paper = StockPaperConverter.Map(response);

            DataBind();
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }
    }
}

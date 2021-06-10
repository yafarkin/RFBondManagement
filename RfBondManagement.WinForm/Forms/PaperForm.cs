using System;
using System.Collections.Generic;
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
        }

        private void DataBind()
        {
            tbSecId.Text = Paper?.SecId;
            tbName.Text = Paper?.Name;
            tbShortName.Text = Paper?.ShortName;
            tbIsin.Text = Paper?.Isin;
            tbFaceValue.Text = Paper?.FaceValue.ToString();
            tbIssueDate.Text = Paper?.IssueDate?.ToShortDateString();
            tbTypeName.Text = Paper?.TypeName;
            tbGroup.Text = Paper?.Group;
            tbType.Text = Paper?.Type;
            tbGroupName.Text = Paper?.GroupName;
            tbPrimaryMarket.Text = Paper?.PrimaryBoard?.Market;
            tbPrimaryBoardId.Text = Paper?.PrimaryBoard?.BoardId;
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSearch.Text))
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            var request = new MoexPaperDefinitionRequest(tbSearch.Text.Trim());
            var response = await request.Read();
            Paper = StockPaperConverter.Map(response);

            if (Paper.IsBond)
            {
                var couponsRequest = new MoexBondCouponsRequest(Paper.SecId);
                var couponsResponse = await couponsRequest.Read();
                StockPaperConverter.MapBond(Paper, couponsResponse);
            }
            else if (Paper.IsShare)
            {
                //TODO: Add dividends
            }


            DataBind();

            lblLastPrice.Text = "---";
            if (Paper?.PrimaryBoard != null)
            {
                var priceRequest = new MoexLastPriceRequest(Paper.PrimaryBoard.Market, Paper.PrimaryBoard.BoardId, Paper.SecId);
                var priceResponse = await priceRequest.Read();

                var lastPrice = priceResponse.Securities.GetDataForDecimal("SECID", Paper.SecId, "PREVADMITTEDQUOTE");

                if (lastPrice.HasValue)
                {
                    lblLastPrice.Text = lastPrice.ToString();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (null == Paper)
            {
                Paper = new BaseStockPaper {Boards = new List<PaperBoard> {new PaperBoard {IsPrimary = true}}};
            }

            Paper.SecId = tbSecId.Text;
            Paper.Name = tbName.Text;
            Paper.ShortName = tbShortName.Text;
            Paper.Isin = tbIsin.Text;
            Paper.FaceValue = string.IsNullOrWhiteSpace(tbFaceValue.Text) ? 0 : Convert.ToDecimal(tbFaceValue.Text);
            Paper.IssueDate = string.IsNullOrWhiteSpace(tbIssueDate.Text) ? null : Convert.ToDateTime(tbIssueDate.Text);
            Paper.TypeName = tbTypeName.Text;
            Paper.Group = tbGroup.Text;
            Paper.TypeName = tbType.Text;
            Paper.GroupName = tbGroupName.Text;

            Paper.PrimaryBoard.Market = tbPrimaryMarket.Text;
            Paper.PrimaryBoard.BoardId = tbPrimaryBoardId.Text;
        }

        private void PaperForm_Load(object sender, EventArgs e)
        {
            DataBind();
        }
    }
}

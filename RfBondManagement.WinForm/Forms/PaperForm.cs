using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NLog;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.WinForm.Forms
{
    public partial class PaperForm : Form
    {
        public AbstractPaper Paper;

        protected IExternalImport _import;
        protected ILogger _logger;

        protected bool IsNewPaper;

        public PaperForm(IExternalImport import, ILogger logger)
        {
            InitializeComponent();

            _import = import;
            _logger = logger;
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
            cbFavorite.Checked = Paper?.IsFavorite ?? false;

            tbSecId.Enabled = IsNewPaper;
            tbIsin.Enabled = tbSecId.Enabled;
            tbType.Enabled = tbSecId.Enabled;
            tbTypeName.Enabled = tbSecId.Enabled;
            tbGroup.Enabled = tbSecId.Enabled;
            tbGroupName.Enabled = tbSecId.Enabled;
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSearch.Text))
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                Paper = await _import.ImportPaper(_logger, tbSearch.Text.Trim());
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Ошибка при импорте бумаги");
                return;
            }

            DataBind();

            lblLastPrice.Text = "---";
            if (Paper?.PrimaryBoard != null)
            {
                var lastPrice = await _import.LastPrice(_logger, Paper);
                if (lastPrice != null)
                {
                    lblLastPrice.Text = lastPrice.Price.ToString();
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
            Paper ??= new SharePaper {Boards = new List<PaperBoard> {new PaperBoard {IsPrimary = true}}};

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
            Paper.IsFavorite = cbFavorite.Checked;

            Paper.PrimaryBoard.Market = tbPrimaryMarket.Text;
            Paper.PrimaryBoard.BoardId = tbPrimaryBoardId.Text;
        }

        private void PaperForm_Load(object sender, EventArgs e)
        {
            IsNewPaper = null == Paper;

            DataBind();
        }
    }
}

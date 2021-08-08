
namespace RfBondManagement.WinForm.Controls
{
    partial class PaperListUC
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.pnlData = new System.Windows.Forms.Panel();
            this.dgvPapers = new System.Windows.Forms.DataGridView();
            this.dgvPapersSecId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPapersIsin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPapersShortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPapersName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPapersIsFavorite = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvPapersPaperType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPapersGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPapersTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPapersIssueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPapersIssueSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPapersFaceValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlActions.SuspendLayout();
            this.pnlData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPapers)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnSearch);
            this.pnlActions.Controls.Add(this.tbSearch);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActions.Location = new System.Drawing.Point(0, 0);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(829, 31);
            this.pnlActions.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(784, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(42, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = ">>>";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tbSearch
            // 
            this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearch.Location = new System.Drawing.Point(0, 3);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(778, 23);
            this.tbSearch.TabIndex = 3;
            this.tbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyUp);
            // 
            // pnlData
            // 
            this.pnlData.Controls.Add(this.dgvPapers);
            this.pnlData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlData.Location = new System.Drawing.Point(0, 31);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(829, 402);
            this.pnlData.TabIndex = 4;
            // 
            // dgvPapers
            // 
            this.dgvPapers.AllowUserToAddRows = false;
            this.dgvPapers.AllowUserToDeleteRows = false;
            this.dgvPapers.AllowUserToOrderColumns = true;
            this.dgvPapers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvPapers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvPapers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPapers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPapers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvPapersSecId,
            this.dgvPapersIsin,
            this.dgvPapersShortName,
            this.dgvPapersName,
            this.dgvPapersIsFavorite,
            this.dgvPapersPaperType,
            this.dgvPapersGroupName,
            this.dgvPapersTypeName,
            this.dgvPapersIssueDate,
            this.dgvPapersIssueSize,
            this.dgvPapersFaceValue});
            this.dgvPapers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPapers.Location = new System.Drawing.Point(0, 0);
            this.dgvPapers.MultiSelect = false;
            this.dgvPapers.Name = "dgvPapers";
            this.dgvPapers.ReadOnly = true;
            this.dgvPapers.RowTemplate.Height = 25;
            this.dgvPapers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPapers.Size = new System.Drawing.Size(829, 402);
            this.dgvPapers.TabIndex = 0;
            // 
            // dgvPapersSecId
            // 
            this.dgvPapersSecId.DataPropertyName = "SecId";
            this.dgvPapersSecId.HeaderText = "SecID";
            this.dgvPapersSecId.Name = "dgvPapersSecId";
            this.dgvPapersSecId.ReadOnly = true;
            this.dgvPapersSecId.Width = 61;
            // 
            // dgvPapersIsin
            // 
            this.dgvPapersIsin.DataPropertyName = "Isin";
            this.dgvPapersIsin.HeaderText = "ISIN";
            this.dgvPapersIsin.Name = "dgvPapersIsin";
            this.dgvPapersIsin.ReadOnly = true;
            this.dgvPapersIsin.Width = 53;
            // 
            // dgvPapersShortName
            // 
            this.dgvPapersShortName.DataPropertyName = "ShortName";
            this.dgvPapersShortName.HeaderText = "Короткое имя";
            this.dgvPapersShortName.Name = "dgvPapersShortName";
            this.dgvPapersShortName.ReadOnly = true;
            this.dgvPapersShortName.Width = 109;
            // 
            // dgvPapersName
            // 
            this.dgvPapersName.DataPropertyName = "Name";
            this.dgvPapersName.HeaderText = "Name";
            this.dgvPapersName.Name = "dgvPapersName";
            this.dgvPapersName.ReadOnly = true;
            this.dgvPapersName.Width = 64;
            // 
            // dgvPapersIsFavorite
            // 
            this.dgvPapersIsFavorite.DataPropertyName = "IsFavorite";
            this.dgvPapersIsFavorite.HeaderText = "Избранное";
            this.dgvPapersIsFavorite.Name = "dgvPapersIsFavorite";
            this.dgvPapersIsFavorite.ReadOnly = true;
            this.dgvPapersIsFavorite.Width = 74;
            // 
            // dgvPapersPaperType
            // 
            this.dgvPapersPaperType.DataPropertyName = "PaperType";
            this.dgvPapersPaperType.HeaderText = "Тип бумаги";
            this.dgvPapersPaperType.Name = "dgvPapersPaperType";
            this.dgvPapersPaperType.ReadOnly = true;
            this.dgvPapersPaperType.Width = 95;
            // 
            // dgvPapersGroupName
            // 
            this.dgvPapersGroupName.DataPropertyName = "GroupName";
            this.dgvPapersGroupName.HeaderText = "Группа";
            this.dgvPapersGroupName.Name = "dgvPapersGroupName";
            this.dgvPapersGroupName.ReadOnly = true;
            this.dgvPapersGroupName.Width = 71;
            // 
            // dgvPapersTypeName
            // 
            this.dgvPapersTypeName.DataPropertyName = "TypeName";
            this.dgvPapersTypeName.HeaderText = "Тип";
            this.dgvPapersTypeName.Name = "dgvPapersTypeName";
            this.dgvPapersTypeName.ReadOnly = true;
            this.dgvPapersTypeName.Width = 52;
            // 
            // dgvPapersIssueDate
            // 
            this.dgvPapersIssueDate.DataPropertyName = "IssueDate";
            this.dgvPapersIssueDate.HeaderText = "Дата выпуска";
            this.dgvPapersIssueDate.Name = "dgvPapersIssueDate";
            this.dgvPapersIssueDate.ReadOnly = true;
            this.dgvPapersIssueDate.Width = 106;
            // 
            // dgvPapersIssueSize
            // 
            this.dgvPapersIssueSize.DataPropertyName = "IssueSize";
            this.dgvPapersIssueSize.HeaderText = "Объем выпуска";
            this.dgvPapersIssueSize.Name = "dgvPapersIssueSize";
            this.dgvPapersIssueSize.ReadOnly = true;
            this.dgvPapersIssueSize.Width = 109;
            // 
            // dgvPapersFaceValue
            // 
            this.dgvPapersFaceValue.DataPropertyName = "FaceValue";
            this.dgvPapersFaceValue.HeaderText = "Нач. стоимость";
            this.dgvPapersFaceValue.Name = "dgvPapersFaceValue";
            this.dgvPapersFaceValue.ReadOnly = true;
            this.dgvPapersFaceValue.Width = 108;
            // 
            // PaperListUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlData);
            this.Controls.Add(this.pnlActions);
            this.Name = "PaperListUC";
            this.Size = new System.Drawing.Size(829, 433);
            this.Load += new System.EventHandler(this.PaperListUC_Load);
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.pnlData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPapers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Panel pnlData;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.DataGridView dgvPapers;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPapersSecId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPapersIsin;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPapersShortName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPapersName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvPapersIsFavorite;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPapersPaperType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPapersGroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPapersTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPapersIssueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPapersIssueSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPapersFaceValue;
    }
}

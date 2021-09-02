
namespace RfBondManagement.WinForm.Controls
{
    partial class PortfolioActionsUC
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
            this.dgvActions = new System.Windows.Forms.DataGridView();
            this.colWhen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGlobalType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocalTYpe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSecId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvActions)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvActions
            // 
            this.dgvActions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvActions.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvActions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvActions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWhen,
            this.colGlobalType,
            this.colLocalTYpe,
            this.colSecId,
            this.colCount,
            this.colPrice,
            this.colSum,
            this.colComment});
            this.dgvActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvActions.Location = new System.Drawing.Point(0, 0);
            this.dgvActions.Name = "dgvActions";
            this.dgvActions.ReadOnly = true;
            this.dgvActions.RowTemplate.Height = 25;
            this.dgvActions.Size = new System.Drawing.Size(465, 239);
            this.dgvActions.TabIndex = 0;
            // 
            // colWhen
            // 
            this.colWhen.DataPropertyName = "When";
            this.colWhen.HeaderText = "Когда";
            this.colWhen.Name = "colWhen";
            this.colWhen.ReadOnly = true;
            this.colWhen.Width = 63;
            // 
            // colGlobalType
            // 
            this.colGlobalType.DataPropertyName = "GlobalType";
            this.colGlobalType.HeaderText = "Тип";
            this.colGlobalType.Name = "colGlobalType";
            this.colGlobalType.ReadOnly = true;
            this.colGlobalType.Width = 52;
            // 
            // colLocalTYpe
            // 
            this.colLocalTYpe.DataPropertyName = "LocalType";
            this.colLocalTYpe.HeaderText = "Подтип";
            this.colLocalTYpe.Name = "colLocalTYpe";
            this.colLocalTYpe.ReadOnly = true;
            this.colLocalTYpe.Width = 73;
            // 
            // colSecId
            // 
            this.colSecId.DataPropertyName = "SecId";
            this.colSecId.HeaderText = "Бумага";
            this.colSecId.Name = "colSecId";
            this.colSecId.ReadOnly = true;
            this.colSecId.Width = 71;
            // 
            // colCount
            // 
            this.colCount.DataPropertyName = "Count";
            this.colCount.HeaderText = "Количество";
            this.colCount.Name = "colCount";
            this.colCount.ReadOnly = true;
            this.colCount.Width = 97;
            // 
            // colPrice
            // 
            this.colPrice.DataPropertyName = "Price";
            this.colPrice.HeaderText = "Цена";
            this.colPrice.Name = "colPrice";
            this.colPrice.ReadOnly = true;
            this.colPrice.Width = 60;
            // 
            // colSum
            // 
            this.colSum.DataPropertyName = "Sum";
            this.colSum.HeaderText = "Сумма";
            this.colSum.Name = "colSum";
            this.colSum.ReadOnly = true;
            this.colSum.Width = 70;
            // 
            // colComment
            // 
            this.colComment.DataPropertyName = "Comment";
            this.colComment.HeaderText = "Комментарий";
            this.colComment.Name = "colComment";
            this.colComment.ReadOnly = true;
            this.colComment.Width = 109;
            // 
            // PortfolioActionsUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvActions);
            this.Name = "PortfolioActionsUC";
            this.Size = new System.Drawing.Size(465, 239);
            this.Load += new System.EventHandler(this.PortfolioActionsUC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvActions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvActions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWhen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGlobalType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocalTYpe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSecId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
    }
}

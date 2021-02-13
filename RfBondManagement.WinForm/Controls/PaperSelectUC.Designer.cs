
namespace RfBondManagement.WinForm.Controls
{
    partial class PaperSelectUC
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
            this.lblSearch = new System.Windows.Forms.Label();
            this.cbbPaper = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(0, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(106, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Выберите бумагу:";
            // 
            // cbbPaper
            // 
            this.cbbPaper.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbPaper.FormattingEnabled = true;
            this.cbbPaper.Location = new System.Drawing.Point(0, 18);
            this.cbbPaper.MaxDropDownItems = 25;
            this.cbbPaper.Name = "cbbPaper";
            this.cbbPaper.Size = new System.Drawing.Size(245, 23);
            this.cbbPaper.Sorted = true;
            this.cbbPaper.TabIndex = 1;
            this.cbbPaper.SelectionChangeCommitted += new System.EventHandler(this.cbPaper_SelectionChangeCommitted);
            this.cbbPaper.TextChanged += new System.EventHandler(this.cbPaper_TextChanged);
            // 
            // PaperSelectUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbbPaper);
            this.Controls.Add(this.lblSearch);
            this.Name = "PaperSelectUC";
            this.Size = new System.Drawing.Size(248, 45);
            this.Load += new System.EventHandler(this.PaperSelectUC_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.ComboBox cbbPaper;
    }
}


namespace RfBondManagement.WinForm.Controls
{
    partial class PaperSelectControl
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
            this.lblOption = new System.Windows.Forms.Label();
            this.cbPaper = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblOption
            // 
            this.lblOption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOption.AutoSize = true;
            this.lblOption.Location = new System.Drawing.Point(0, 0);
            this.lblOption.Name = "lblOption";
            this.lblOption.Size = new System.Drawing.Size(106, 15);
            this.lblOption.TabIndex = 0;
            this.lblOption.Text = "Выберите бумагу:";
            // 
            // cbPaper
            // 
            this.cbPaper.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPaper.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbPaper.FormattingEnabled = true;
            this.cbPaper.Location = new System.Drawing.Point(0, 18);
            this.cbPaper.Name = "cbPaper";
            this.cbPaper.Size = new System.Drawing.Size(185, 23);
            this.cbPaper.TabIndex = 1;
            this.cbPaper.TextChanged += new System.EventHandler(this.cbPaper_TextChanged);
            // 
            // PaperSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.cbPaper);
            this.Controls.Add(this.lblOption);
            this.Name = "PaperSelectControl";
            this.Size = new System.Drawing.Size(188, 49);
            this.Load += new System.EventHandler(this.PaperSelectControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOption;
        private System.Windows.Forms.ComboBox cbPaper;
    }
}

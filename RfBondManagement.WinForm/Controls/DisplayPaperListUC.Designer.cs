
namespace RfBondManagement.WinForm.Controls
{
    partial class DisplayPaperListUC
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lvPapers = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Стоимость:";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(87, 15);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(22, 15);
            this.lblPrice.TabIndex = 1;
            this.lblPrice.Text = "---";
            // 
            // lvPapers
            // 
            this.lvPapers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPapers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvPapers.FullRowSelect = true;
            this.lvPapers.GridLines = true;
            this.lvPapers.HideSelection = false;
            this.lvPapers.Location = new System.Drawing.Point(11, 33);
            this.lvPapers.MultiSelect = false;
            this.lvPapers.Name = "lvPapers";
            this.lvPapers.Size = new System.Drawing.Size(761, 84);
            this.lvPapers.TabIndex = 2;
            this.lvPapers.UseCompatibleStateImageBehavior = false;
            // 
            // DisplayPaperListUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvPapers);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.label1);
            this.Name = "DisplayPaperListUC";
            this.Size = new System.Drawing.Size(788, 132);
            this.Load += new System.EventHandler(this.DisplayPaperListUC_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.ListView lvPapers;
    }
}

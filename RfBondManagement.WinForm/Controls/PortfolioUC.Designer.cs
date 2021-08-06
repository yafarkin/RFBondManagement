﻿
namespace RfBondManagement.WinForm.Controls
{
    partial class PortfolioUC
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
            this.plBond = new RfBondManagement.WinForm.Controls.DisplayPaperListUC();
            this.label2 = new System.Windows.Forms.Label();
            this.plFond = new RfBondManagement.WinForm.Controls.DisplayPaperListUC();
            this.label3 = new System.Windows.Forms.Label();
            this.plShare = new RfBondManagement.WinForm.Controls.DisplayPaperListUC();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Облигации:";
            // 
            // plBond
            // 
            this.plBond.Location = new System.Drawing.Point(12, 44);
            this.plBond.Name = "plBond";
            this.plBond.Portfolio = null;
            this.plBond.Size = new System.Drawing.Size(789, 132);
            this.plBond.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Фонды:";
            // 
            // plFond
            // 
            this.plFond.Location = new System.Drawing.Point(12, 197);
            this.plFond.Name = "plFond";
            this.plFond.Portfolio = null;
            this.plFond.Size = new System.Drawing.Size(788, 132);
            this.plFond.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 332);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Акции:";
            // 
            // plShare
            // 
            this.plShare.Location = new System.Drawing.Point(12, 350);
            this.plShare.Name = "plShare";
            this.plShare.Portfolio = null;
            this.plShare.Size = new System.Drawing.Size(788, 132);
            this.plShare.TabIndex = 5;
            // 
            // PortfolioUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.plShare);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.plFond);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.plBond);
            this.Controls.Add(this.label1);
            this.Name = "PortfolioUC";
            this.Size = new System.Drawing.Size(804, 493);
            this.Load += new System.EventHandler(this.PortfolioUC_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DisplayPaperListUC plBond;
        private System.Windows.Forms.Label label2;
        private DisplayPaperListUC plFond;
        private System.Windows.Forms.Label label3;
        private DisplayPaperListUC plShare;
    }
}
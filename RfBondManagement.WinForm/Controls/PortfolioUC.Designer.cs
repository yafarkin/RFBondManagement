
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
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblPortfolio = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlData = new System.Windows.Forms.Panel();
            this.tcPortfolio = new System.Windows.Forms.TabControl();
            this.tpStructure = new System.Windows.Forms.TabPage();
            this.portfolioTree = new RfBondManagement.WinForm.Controls.PortfolioTreeUC();
            this.tpPapers = new System.Windows.Forms.TabPage();
            this.tpActions = new System.Windows.Forms.TabPage();
            this.portfolioActions = new RfBondManagement.WinForm.Controls.PortfolioActionsUC();
            this.pnlInfo.SuspendLayout();
            this.pnlData.SuspendLayout();
            this.tcPortfolio.SuspendLayout();
            this.tpStructure.SuspendLayout();
            this.tpActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.lblPortfolio);
            this.pnlInfo.Controls.Add(this.label1);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(804, 37);
            this.pnlInfo.TabIndex = 0;
            // 
            // lblPortfolio
            // 
            this.lblPortfolio.AutoSize = true;
            this.lblPortfolio.Location = new System.Drawing.Point(79, 9);
            this.lblPortfolio.Name = "lblPortfolio";
            this.lblPortfolio.Size = new System.Drawing.Size(22, 15);
            this.lblPortfolio.TabIndex = 1;
            this.lblPortfolio.Text = "---";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Портфель:";
            // 
            // pnlData
            // 
            this.pnlData.Controls.Add(this.tcPortfolio);
            this.pnlData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlData.Location = new System.Drawing.Point(0, 37);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(804, 456);
            this.pnlData.TabIndex = 1;
            // 
            // tcPortfolio
            // 
            this.tcPortfolio.Controls.Add(this.tpStructure);
            this.tcPortfolio.Controls.Add(this.tpPapers);
            this.tcPortfolio.Controls.Add(this.tpActions);
            this.tcPortfolio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcPortfolio.Location = new System.Drawing.Point(0, 0);
            this.tcPortfolio.Name = "tcPortfolio";
            this.tcPortfolio.SelectedIndex = 0;
            this.tcPortfolio.Size = new System.Drawing.Size(804, 456);
            this.tcPortfolio.TabIndex = 0;
            // 
            // tpStructure
            // 
            this.tpStructure.Controls.Add(this.portfolioTree);
            this.tpStructure.Location = new System.Drawing.Point(4, 24);
            this.tpStructure.Name = "tpStructure";
            this.tpStructure.Padding = new System.Windows.Forms.Padding(3);
            this.tpStructure.Size = new System.Drawing.Size(796, 428);
            this.tpStructure.TabIndex = 0;
            this.tpStructure.Text = "Структура";
            this.tpStructure.UseVisualStyleBackColor = true;
            // 
            // portfolioTree
            // 
            this.portfolioTree.Container = null;
            this.portfolioTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.portfolioTree.Location = new System.Drawing.Point(3, 3);
            this.portfolioTree.Name = "portfolioTree";
            this.portfolioTree.Portfolio = null;
            this.portfolioTree.PortfolioRepository = null;
            this.portfolioTree.Size = new System.Drawing.Size(790, 422);
            this.portfolioTree.TabIndex = 0;
            // 
            // tpPapers
            // 
            this.tpPapers.Location = new System.Drawing.Point(4, 24);
            this.tpPapers.Name = "tpPapers";
            this.tpPapers.Padding = new System.Windows.Forms.Padding(3);
            this.tpPapers.Size = new System.Drawing.Size(796, 428);
            this.tpPapers.TabIndex = 1;
            this.tpPapers.Text = "Бумаги";
            this.tpPapers.UseVisualStyleBackColor = true;
            // 
            // tpActions
            // 
            this.tpActions.Controls.Add(this.portfolioActions);
            this.tpActions.Location = new System.Drawing.Point(4, 24);
            this.tpActions.Name = "tpActions";
            this.tpActions.Size = new System.Drawing.Size(796, 428);
            this.tpActions.TabIndex = 2;
            this.tpActions.Text = "Действия";
            this.tpActions.UseVisualStyleBackColor = true;
            // 
            // portfolioActions
            // 
            this.portfolioActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.portfolioActions.Location = new System.Drawing.Point(0, 0);
            this.portfolioActions.MoneyActionRepository = null;
            this.portfolioActions.Name = "portfolioActions";
            this.portfolioActions.PaperActionRepository = null;
            this.portfolioActions.Portfolio = null;
            this.portfolioActions.Size = new System.Drawing.Size(796, 428);
            this.portfolioActions.TabIndex = 0;
            // 
            // PortfolioUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlData);
            this.Controls.Add(this.pnlInfo);
            this.Name = "PortfolioUC";
            this.Size = new System.Drawing.Size(804, 493);
            this.Load += new System.EventHandler(this.PortfolioUC_Load);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.pnlData.ResumeLayout(false);
            this.tcPortfolio.ResumeLayout(false);
            this.tpStructure.ResumeLayout(false);
            this.tpActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Panel pnlData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tcPortfolio;
        private System.Windows.Forms.TabPage tpStructure;
        private System.Windows.Forms.TabPage tpPapers;
        private System.Windows.Forms.TabPage tpActions;
        private PortfolioTreeUC portfolioTree;
        private System.Windows.Forms.Label lblPortfolio;
        private PortfolioActionsUC portfolioActions;
    }
}

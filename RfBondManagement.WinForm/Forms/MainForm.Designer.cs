using RfBondManagement.WinForm.Controls;

namespace RfBondManagement.WinForm.Forms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemBondCalculator = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGeneralSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDictionary = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPapers = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlData = new System.Windows.Forms.Panel();
            this.tcData = new System.Windows.Forms.TabControl();
            this.tpWatchList = new System.Windows.Forms.TabPage();
            this.watchList = new RfBondManagement.WinForm.Controls.WatchListUC();
            this.menuItemAddPortfolio = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEditPortfolio = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.pnlData.SuspendLayout();
            this.tcData.SuspendLayout();
            this.tpWatchList.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemSettings,
            this.menuItemDictionary});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(844, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "Main Menu";
            // 
            // menuItemFile
            // 
            this.menuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemBondCalculator,
            this.menuItemExit});
            this.menuItemFile.Name = "menuItemFile";
            this.menuItemFile.Size = new System.Drawing.Size(48, 20);
            this.menuItemFile.Text = "Файл";
            // 
            // menuItemBondCalculator
            // 
            this.menuItemBondCalculator.Name = "menuItemBondCalculator";
            this.menuItemBondCalculator.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.menuItemBondCalculator.Size = new System.Drawing.Size(226, 22);
            this.menuItemBondCalculator.Text = "Калькулятор облигации";
            this.menuItemBondCalculator.Click += new System.EventHandler(this.menuItemBondCalculator_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(226, 22);
            this.menuItemExit.Text = "Выход";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemSettings
            // 
            this.menuItemSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemGeneralSettings,
            this.menuItemAddPortfolio,
            this.menuItemEditPortfolio});
            this.menuItemSettings.Name = "menuItemSettings";
            this.menuItemSettings.Size = new System.Drawing.Size(79, 20);
            this.menuItemSettings.Text = "Настройки";
            // 
            // menuItemGeneralSettings
            // 
            this.menuItemGeneralSettings.Name = "menuItemGeneralSettings";
            this.menuItemGeneralSettings.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.menuItemGeneralSettings.Size = new System.Drawing.Size(211, 22);
            this.menuItemGeneralSettings.Text = "Общие настройки";
            this.menuItemGeneralSettings.Click += new System.EventHandler(this.menuItemGeneralSettings_Click);
            // 
            // menuItemDictionary
            // 
            this.menuItemDictionary.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemPapers});
            this.menuItemDictionary.Name = "menuItemDictionary";
            this.menuItemDictionary.Size = new System.Drawing.Size(94, 20);
            this.menuItemDictionary.Text = "Справочники";
            // 
            // menuItemPapers
            // 
            this.menuItemPapers.Name = "menuItemPapers";
            this.menuItemPapers.Size = new System.Drawing.Size(114, 22);
            this.menuItemPapers.Text = "Бумаги";
            this.menuItemPapers.Click += new System.EventHandler(this.menuItemPapers_Click);
            // 
            // pnlData
            // 
            this.pnlData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlData.Controls.Add(this.tcData);
            this.pnlData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlData.Location = new System.Drawing.Point(0, 24);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(844, 502);
            this.pnlData.TabIndex = 4;
            // 
            // tcData
            // 
            this.tcData.Controls.Add(this.tpWatchList);
            this.tcData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcData.Location = new System.Drawing.Point(0, 0);
            this.tcData.Name = "tcData";
            this.tcData.SelectedIndex = 0;
            this.tcData.Size = new System.Drawing.Size(842, 500);
            this.tcData.TabIndex = 4;
            // 
            // tpWatchList
            // 
            this.tpWatchList.Controls.Add(this.watchList);
            this.tpWatchList.Location = new System.Drawing.Point(4, 24);
            this.tpWatchList.Name = "tpWatchList";
            this.tpWatchList.Padding = new System.Windows.Forms.Padding(3);
            this.tpWatchList.Size = new System.Drawing.Size(834, 472);
            this.tpWatchList.TabIndex = 0;
            this.tpWatchList.Text = "Watch list";
            this.tpWatchList.UseVisualStyleBackColor = true;
            // 
            // watchList
            // 
            this.watchList.Container = null;
            this.watchList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.watchList.ExternalImport = null;
            this.watchList.HistoryEngine = null;
            this.watchList.Location = new System.Drawing.Point(3, 3);
            this.watchList.Logger = null;
            this.watchList.Name = "watchList";
            this.watchList.PaperRepository = null;
            this.watchList.Size = new System.Drawing.Size(828, 466);
            this.watchList.TabIndex = 0;
            // 
            // menuItemAddPortfolio
            // 
            this.menuItemAddPortfolio.Name = "menuItemAddPortfolio";
            this.menuItemAddPortfolio.Size = new System.Drawing.Size(211, 22);
            this.menuItemAddPortfolio.Text = "Новый портфель";
            this.menuItemAddPortfolio.Click += new System.EventHandler(this.menuItemAddPortfolio_Click);
            // 
            // menuItemEditPortfolio
            // 
            this.menuItemEditPortfolio.Name = "menuItemEditPortfolio";
            this.menuItemEditPortfolio.Size = new System.Drawing.Size(211, 22);
            this.menuItemEditPortfolio.Text = "Редактировать портфель";
            this.menuItemEditPortfolio.Click += new System.EventHandler(this.menuItemEditPortfolio_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 526);
            this.Controls.Add(this.pnlData);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "Портфель";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.pnlData.ResumeLayout(false);
            this.tcData.ResumeLayout(false);
            this.tpWatchList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItemFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuItemSettings;
        private System.Windows.Forms.ToolStripMenuItem menuItemGeneralSettings;
        private System.Windows.Forms.Panel pnlData;
        private System.Windows.Forms.ColumnHeader chFaceValue;
        private System.Windows.Forms.ToolStripMenuItem menuItemBondCalculator;
        private System.Windows.Forms.ToolStripMenuItem menuItemDictionary;
        private System.Windows.Forms.ToolStripMenuItem menuItemPapers;
        private System.Windows.Forms.TabControl tcData;
        private System.Windows.Forms.TabPage tpWatchList;
        private WatchListUC watchList;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddPortfolio;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditPortfolio;
    }
}


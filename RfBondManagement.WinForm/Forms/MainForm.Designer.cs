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
            this.pnlPaperDetails = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlPapers = new System.Windows.Forms.Panel();
            this.lvPapers = new System.Windows.Forms.ListView();
            this.chPaperName = new System.Windows.Forms.ColumnHeader();
            this.chCurrency = new System.Windows.Forms.ColumnHeader();
            this.chBondPar = new System.Windows.Forms.ColumnHeader();
            this.chCount = new System.Windows.Forms.ColumnHeader();
            this.chSummToClose = new System.Windows.Forms.ColumnHeader();
            this.chIncome = new System.Windows.Forms.ColumnHeader();
            this.chPercent = new System.Windows.Forms.ColumnHeader();
            this.chPositiveDate = new System.Windows.Forms.ColumnHeader();
            this.chDaysToClose = new System.Windows.Forms.ColumnHeader();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnDeletePaper = new System.Windows.Forms.Button();
            this.btnEditPaper = new System.Windows.Forms.Button();
            this.btnAddPaper = new System.Windows.Forms.Button();
            this.lblPaperList = new System.Windows.Forms.Label();
            this.mainMenu.SuspendLayout();
            this.pnlPaperDetails.SuspendLayout();
            this.pnlPapers.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemSettings});
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
            this.menuItemGeneralSettings});
            this.menuItemSettings.Name = "menuItemSettings";
            this.menuItemSettings.Size = new System.Drawing.Size(79, 20);
            this.menuItemSettings.Text = "Настройки";
            // 
            // menuItemGeneralSettings
            // 
            this.menuItemGeneralSettings.Name = "menuItemGeneralSettings";
            this.menuItemGeneralSettings.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.menuItemGeneralSettings.Size = new System.Drawing.Size(194, 22);
            this.menuItemGeneralSettings.Text = "Общие настройки";
            this.menuItemGeneralSettings.Click += new System.EventHandler(this.menuItemGeneralSettings_Click);
            // 
            // pnlPaperDetails
            // 
            this.pnlPaperDetails.Controls.Add(this.label1);
            this.pnlPaperDetails.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlPaperDetails.Location = new System.Drawing.Point(574, 24);
            this.pnlPaperDetails.Name = "pnlPaperDetails";
            this.pnlPaperDetails.Size = new System.Drawing.Size(270, 502);
            this.pnlPaperDetails.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Детали по выбранной бумаге";
            // 
            // pnlPapers
            // 
            this.pnlPapers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPapers.Controls.Add(this.lvPapers);
            this.pnlPapers.Controls.Add(this.pnlActions);
            this.pnlPapers.Controls.Add(this.lblPaperList);
            this.pnlPapers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPapers.Location = new System.Drawing.Point(0, 24);
            this.pnlPapers.Name = "pnlPapers";
            this.pnlPapers.Size = new System.Drawing.Size(574, 502);
            this.pnlPapers.TabIndex = 4;
            // 
            // lvPapers
            // 
            this.lvPapers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPapers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvPapers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPaperName,
            this.chCurrency,
            this.chBondPar,
            this.chCount,
            this.chSummToClose,
            this.chIncome,
            this.chPercent,
            this.chPositiveDate,
            this.chDaysToClose});
            this.lvPapers.FullRowSelect = true;
            this.lvPapers.GridLines = true;
            this.lvPapers.HideSelection = false;
            this.lvPapers.Location = new System.Drawing.Point(12, 62);
            this.lvPapers.MultiSelect = false;
            this.lvPapers.Name = "lvPapers";
            this.lvPapers.ShowGroups = false;
            this.lvPapers.Size = new System.Drawing.Size(545, 420);
            this.lvPapers.TabIndex = 4;
            this.lvPapers.UseCompatibleStateImageBehavior = false;
            this.lvPapers.View = System.Windows.Forms.View.Details;
            // 
            // chPaperName
            // 
            this.chPaperName.Name = "chPaperName";
            this.chPaperName.Text = "Бумага";
            // 
            // chCurrency
            // 
            this.chCurrency.Name = "chCurrency";
            this.chCurrency.Text = "Валюта";
            // 
            // chBondPar
            // 
            this.chBondPar.Name = "chBondPar";
            this.chBondPar.Text = "Номинал";
            // 
            // chCount
            // 
            this.chCount.Name = "chCount";
            this.chCount.Text = "Количество";
            // 
            // chSummToClose
            // 
            this.chSummToClose.Name = "chSummToClose";
            this.chSummToClose.Text = "Сумма к закр.";
            // 
            // chIncome
            // 
            this.chIncome.Name = "chIncome";
            this.chIncome.Text = "Доход";
            // 
            // chPercent
            // 
            this.chPercent.Name = "chPercent";
            this.chPercent.Text = "% доход";
            // 
            // chPositiveDate
            // 
            this.chPositiveDate.Name = "chPositiveDate";
            this.chPositiveDate.Text = "В плюс после";
            // 
            // chDaysToClose
            // 
            this.chDaysToClose.Name = "chDaysToClose";
            this.chDaysToClose.Text = "Дней до закрытия";
            // 
            // pnlActions
            // 
            this.pnlActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlActions.Controls.Add(this.btnDeletePaper);
            this.pnlActions.Controls.Add(this.btnEditPaper);
            this.pnlActions.Controls.Add(this.btnAddPaper);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActions.Location = new System.Drawing.Point(0, 0);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(572, 41);
            this.pnlActions.TabIndex = 3;
            // 
            // btnDeletePaper
            // 
            this.btnDeletePaper.Enabled = false;
            this.btnDeletePaper.Location = new System.Drawing.Point(175, 8);
            this.btnDeletePaper.Name = "btnDeletePaper";
            this.btnDeletePaper.Size = new System.Drawing.Size(75, 23);
            this.btnDeletePaper.TabIndex = 2;
            this.btnDeletePaper.Text = "Удалить";
            this.btnDeletePaper.UseVisualStyleBackColor = true;
            // 
            // btnEditPaper
            // 
            this.btnEditPaper.Enabled = false;
            this.btnEditPaper.Location = new System.Drawing.Point(93, 8);
            this.btnEditPaper.Name = "btnEditPaper";
            this.btnEditPaper.Size = new System.Drawing.Size(75, 23);
            this.btnEditPaper.TabIndex = 1;
            this.btnEditPaper.Text = "Изменить";
            this.btnEditPaper.UseVisualStyleBackColor = true;
            // 
            // btnAddPaper
            // 
            this.btnAddPaper.Enabled = false;
            this.btnAddPaper.Location = new System.Drawing.Point(12, 8);
            this.btnAddPaper.Name = "btnAddPaper";
            this.btnAddPaper.Size = new System.Drawing.Size(75, 23);
            this.btnAddPaper.TabIndex = 0;
            this.btnAddPaper.Text = "Добавить";
            this.btnAddPaper.UseVisualStyleBackColor = true;
            // 
            // lblPaperList
            // 
            this.lblPaperList.AutoSize = true;
            this.lblPaperList.Location = new System.Drawing.Point(12, 44);
            this.lblPaperList.Name = "lblPaperList";
            this.lblPaperList.Size = new System.Drawing.Size(84, 15);
            this.lblPaperList.TabIndex = 0;
            this.lblPaperList.Text = "Список бумаг";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 526);
            this.Controls.Add(this.pnlPapers);
            this.Controls.Add(this.pnlPaperDetails);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "Портфель облигаций";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.pnlPaperDetails.ResumeLayout(false);
            this.pnlPaperDetails.PerformLayout();
            this.pnlPapers.ResumeLayout(false);
            this.pnlPapers.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItemFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuItemSettings;
        private System.Windows.Forms.ToolStripMenuItem menuItemGeneralSettings;
        private System.Windows.Forms.Panel pnlPaperDetails;
        private System.Windows.Forms.Panel pnlPapers;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Label lblPaperList;
        private System.Windows.Forms.Button btnEditPaper;
        private System.Windows.Forms.Button btnAddPaper;
        private System.Windows.Forms.Button btnDeletePaper;
        private System.Windows.Forms.ListView lvPapers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader chPaperName;
        private System.Windows.Forms.ColumnHeader chCurrency;
        private System.Windows.Forms.ColumnHeader chSummToClose;
        private System.Windows.Forms.ColumnHeader chPercent;
        private System.Windows.Forms.ColumnHeader chDaysToClose;
        private System.Windows.Forms.ColumnHeader chBondPar;
        private System.Windows.Forms.ColumnHeader chCount;
        private System.Windows.Forms.ColumnHeader chIncome;
        private System.Windows.Forms.ColumnHeader chPositiveDate;
        private System.Windows.Forms.ToolStripMenuItem menuItemBondCalculator;
    }
}


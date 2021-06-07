
namespace RfBondManagement.WinForm.Forms
{
    partial class PaperListForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuPapers = new System.Windows.Forms.MenuStrip();
            this.menuItemClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.pnlList = new System.Windows.Forms.Panel();
            this.lvPapers = new System.Windows.Forms.ListView();
            this.chIsin = new System.Windows.Forms.ColumnHeader();
            this.chType = new System.Windows.Forms.ColumnHeader();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.menuPapers.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlList.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuPapers
            // 
            this.menuPapers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemClose,
            this.menuItemNew,
            this.menuItemEdit,
            this.menuItemDelete});
            this.menuPapers.Location = new System.Drawing.Point(0, 0);
            this.menuPapers.Name = "menuPapers";
            this.menuPapers.Size = new System.Drawing.Size(800, 24);
            this.menuPapers.TabIndex = 0;
            this.menuPapers.Text = "menuStrip1";
            // 
            // menuItemClose
            // 
            this.menuItemClose.Name = "menuItemClose";
            this.menuItemClose.Size = new System.Drawing.Size(65, 20);
            this.menuItemClose.Text = "Закрыть";
            this.menuItemClose.Click += new System.EventHandler(this.menuItemClose_Click);
            // 
            // menuItemNew
            // 
            this.menuItemNew.Name = "menuItemNew";
            this.menuItemNew.Size = new System.Drawing.Size(53, 20);
            this.menuItemNew.Text = "Новая";
            this.menuItemNew.Click += new System.EventHandler(this.menuItemNew_Click);
            // 
            // menuItemEdit
            // 
            this.menuItemEdit.Name = "menuItemEdit";
            this.menuItemEdit.Size = new System.Drawing.Size(65, 20);
            this.menuItemEdit.Text = "Править";
            this.menuItemEdit.Click += new System.EventHandler(this.menuItemEdit_Click);
            // 
            // menuItemDelete
            // 
            this.menuItemDelete.Name = "menuItemDelete";
            this.menuItemDelete.Size = new System.Drawing.Size(63, 20);
            this.menuItemDelete.Text = "Удалить";
            this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.cbType);
            this.pnlSearch.Controls.Add(this.lblType);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.tbSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 24);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(800, 31);
            this.pnlSearch.TabIndex = 1;
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(323, 3);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(187, 23);
            this.cbType.TabIndex = 3;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(287, 7);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(30, 15);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "Тип:";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(252, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(29, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "->";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(12, 3);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(234, 23);
            this.tbSearch.TabIndex = 0;
            // 
            // pnlList
            // 
            this.pnlList.Controls.Add(this.lvPapers);
            this.pnlList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlList.Location = new System.Drawing.Point(0, 55);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(800, 395);
            this.pnlList.TabIndex = 2;
            // 
            // lvPapers
            // 
            this.lvPapers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chIsin,
            this.chType,
            this.chName});
            this.lvPapers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPapers.FullRowSelect = true;
            this.lvPapers.GridLines = true;
            this.lvPapers.HideSelection = false;
            this.lvPapers.Location = new System.Drawing.Point(0, 0);
            this.lvPapers.MultiSelect = false;
            this.lvPapers.Name = "lvPapers";
            this.lvPapers.Size = new System.Drawing.Size(800, 395);
            this.lvPapers.TabIndex = 0;
            this.lvPapers.UseCompatibleStateImageBehavior = false;
            this.lvPapers.View = System.Windows.Forms.View.Details;
            // 
            // chIsin
            // 
            this.chIsin.Text = "ISIN";
            // 
            // chType
            // 
            this.chType.Text = "Тип";
            // 
            // chName
            // 
            this.chName.Text = "Имя";
            // 
            // PaperListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.menuPapers);
            this.ForeColor = System.Drawing.Color.Crimson;
            this.MainMenuStrip = this.menuPapers;
            this.MinimizeBox = false;
            this.Name = "PaperListForm";
            this.Text = "Список бумаг";
            this.Load += new System.EventHandler(this.PaperListForm_Load);
            this.menuPapers.ResumeLayout(false);
            this.menuPapers.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuPapers;
        private System.Windows.Forms.ToolStripMenuItem menuItemClose;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.ToolStripMenuItem menuItemNew;
        private System.Windows.Forms.ToolStripMenuItem menuItemEdit;
        private System.Windows.Forms.ToolStripMenuItem menuItemDelete;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.ListView lvPapers;
        private System.Windows.Forms.ColumnHeader chIsin;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.ColumnHeader chName;
    }
}

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
            this.menuItemSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.paperList = new RfBondManagement.WinForm.Controls.PaperListUC();
            this.menuPapers.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuPapers
            // 
            this.menuPapers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemClose,
            this.menuItemNew,
            this.menuItemEdit,
            this.menuItemDelete,
            this.menuItemSelect});
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
            // menuItemSelect
            // 
            this.menuItemSelect.Name = "menuItemSelect";
            this.menuItemSelect.Size = new System.Drawing.Size(66, 20);
            this.menuItemSelect.Text = "Выбрать";
            this.menuItemSelect.Click += new System.EventHandler(this.menuItemSelect_Click);
            // 
            // paperList
            // 
            this.paperList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paperList.Location = new System.Drawing.Point(0, 24);
            this.paperList.Name = "paperList";
            this.paperList.PaperRepository = null;
            this.paperList.Size = new System.Drawing.Size(800, 426);
            this.paperList.TabIndex = 1;
            // 
            // PaperListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.paperList);
            this.Controls.Add(this.menuPapers);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MainMenuStrip = this.menuPapers;
            this.MinimizeBox = false;
            this.Name = "PaperListForm";
            this.Text = "Список бумаг";
            this.Load += new System.EventHandler(this.PaperListForm_Load);
            this.menuPapers.ResumeLayout(false);
            this.menuPapers.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuPapers;
        private System.Windows.Forms.ToolStripMenuItem menuItemClose;
        private System.Windows.Forms.ToolStripMenuItem menuItemNew;
        private System.Windows.Forms.ToolStripMenuItem menuItemEdit;
        private System.Windows.Forms.ToolStripMenuItem menuItemDelete;
        private Controls.PaperListUC paperList;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelect;
    }
}
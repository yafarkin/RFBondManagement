
namespace RfBondManagement.WinForm.Forms
{
    partial class StructureLeafEditForm
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
            this.pnlLeaf = new System.Windows.Forms.Panel();
            this.tbVolume = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlPapers = new System.Windows.Forms.Panel();
            this.lvPapers = new System.Windows.Forms.ListView();
            this.chPercent = new System.Windows.Forms.ColumnHeader();
            this.chSecId = new System.Windows.Forms.ColumnHeader();
            this.chShortName = new System.Windows.Forms.ColumnHeader();
            this.pnlPaperActions = new System.Windows.Forms.Panel();
            this.btnDeletePaper = new System.Windows.Forms.Button();
            this.btnEditPaper = new System.Windows.Forms.Button();
            this.btnAddPaper = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.chVolume = new System.Windows.Forms.ColumnHeader();
            this.pnlLeaf.SuspendLayout();
            this.pnlPapers.SuspendLayout();
            this.pnlPaperActions.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeaf
            // 
            this.pnlLeaf.Controls.Add(this.tbVolume);
            this.pnlLeaf.Controls.Add(this.label2);
            this.pnlLeaf.Controls.Add(this.tbName);
            this.pnlLeaf.Controls.Add(this.label1);
            this.pnlLeaf.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLeaf.Location = new System.Drawing.Point(0, 0);
            this.pnlLeaf.Name = "pnlLeaf";
            this.pnlLeaf.Size = new System.Drawing.Size(301, 68);
            this.pnlLeaf.TabIndex = 5;
            // 
            // tbVolume
            // 
            this.tbVolume.Location = new System.Drawing.Point(142, 35);
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Size = new System.Drawing.Size(144, 23);
            this.tbVolume.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Объём в портфеле:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(80, 6);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(206, 23);
            this.tbName.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Название:";
            // 
            // pnlPapers
            // 
            this.pnlPapers.Controls.Add(this.lvPapers);
            this.pnlPapers.Controls.Add(this.pnlPaperActions);
            this.pnlPapers.Controls.Add(this.label3);
            this.pnlPapers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPapers.Location = new System.Drawing.Point(0, 68);
            this.pnlPapers.Name = "pnlPapers";
            this.pnlPapers.Size = new System.Drawing.Size(301, 382);
            this.pnlPapers.TabIndex = 6;
            // 
            // lvPapers
            // 
            this.lvPapers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPercent,
            this.chVolume,
            this.chSecId,
            this.chShortName});
            this.lvPapers.FullRowSelect = true;
            this.lvPapers.GridLines = true;
            this.lvPapers.HideSelection = false;
            this.lvPapers.Location = new System.Drawing.Point(15, 30);
            this.lvPapers.MultiSelect = false;
            this.lvPapers.Name = "lvPapers";
            this.lvPapers.Size = new System.Drawing.Size(271, 282);
            this.lvPapers.TabIndex = 7;
            this.lvPapers.UseCompatibleStateImageBehavior = false;
            this.lvPapers.View = System.Windows.Forms.View.Details;
            // 
            // chPercent
            // 
            this.chPercent.Text = "% в блоке";
            // 
            // chSecId
            // 
            this.chSecId.Text = "SecID";
            // 
            // chShortName
            // 
            this.chShortName.Text = "Краткое имя";
            // 
            // pnlPaperActions
            // 
            this.pnlPaperActions.Controls.Add(this.btnDeletePaper);
            this.pnlPaperActions.Controls.Add(this.btnEditPaper);
            this.pnlPaperActions.Controls.Add(this.btnAddPaper);
            this.pnlPaperActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPaperActions.Location = new System.Drawing.Point(0, 318);
            this.pnlPaperActions.Name = "pnlPaperActions";
            this.pnlPaperActions.Size = new System.Drawing.Size(301, 64);
            this.pnlPaperActions.TabIndex = 6;
            // 
            // btnDeletePaper
            // 
            this.btnDeletePaper.Location = new System.Drawing.Point(177, 3);
            this.btnDeletePaper.Name = "btnDeletePaper";
            this.btnDeletePaper.Size = new System.Drawing.Size(75, 23);
            this.btnDeletePaper.TabIndex = 2;
            this.btnDeletePaper.Text = "Удалить";
            this.btnDeletePaper.UseVisualStyleBackColor = true;
            this.btnDeletePaper.Click += new System.EventHandler(this.btnDeletePaper_Click);
            // 
            // btnEditPaper
            // 
            this.btnEditPaper.Location = new System.Drawing.Point(96, 3);
            this.btnEditPaper.Name = "btnEditPaper";
            this.btnEditPaper.Size = new System.Drawing.Size(75, 23);
            this.btnEditPaper.TabIndex = 1;
            this.btnEditPaper.Text = "Изменить";
            this.btnEditPaper.UseVisualStyleBackColor = true;
            this.btnEditPaper.Click += new System.EventHandler(this.btnEditPaper_Click);
            // 
            // btnAddPaper
            // 
            this.btnAddPaper.Location = new System.Drawing.Point(15, 3);
            this.btnAddPaper.Name = "btnAddPaper";
            this.btnAddPaper.Size = new System.Drawing.Size(75, 23);
            this.btnAddPaper.TabIndex = 0;
            this.btnAddPaper.Text = "Добавить";
            this.btnAddPaper.UseVisualStyleBackColor = true;
            this.btnAddPaper.Click += new System.EventHandler(this.btnAddPaper_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Список бумаг:";
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnCancel);
            this.pnlActions.Controls.Add(this.btnSave);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Location = new System.Drawing.Point(0, 418);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(301, 32);
            this.pnlActions.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(84, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chVolume
            // 
            this.chVolume.Text = "Объём";
            // 
            // StructureLeafEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 450);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlPapers);
            this.Controls.Add(this.pnlLeaf);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StructureLeafEditForm";
            this.ShowIcon = false;
            this.Text = "Редактирование блока";
            this.Load += new System.EventHandler(this.StructureLeafEditForm_Load);
            this.pnlLeaf.ResumeLayout(false);
            this.pnlLeaf.PerformLayout();
            this.pnlPapers.ResumeLayout(false);
            this.pnlPapers.PerformLayout();
            this.pnlPaperActions.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlLeaf;
        private System.Windows.Forms.TextBox tbVolume;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlPapers;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Panel pnlPaperActions;
        private System.Windows.Forms.ListView lvPapers;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEditPaper;
        private System.Windows.Forms.Button btnAddPaper;
        private System.Windows.Forms.Button btnDeletePaper;
        private System.Windows.Forms.ColumnHeader chPercent;
        private System.Windows.Forms.ColumnHeader chSecId;
        private System.Windows.Forms.ColumnHeader chShortName;
        private System.Windows.Forms.ColumnHeader chVolume;
    }
}
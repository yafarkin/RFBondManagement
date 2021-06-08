
namespace RfBondManagement.WinForm.Forms
{
    partial class PaperForm
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
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.pnlEdit = new System.Windows.Forms.Panel();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSecId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbShortName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbIsin = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbFaceValue = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbIssueDate = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbTypeName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbGroup = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbType = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbGroupName = new System.Windows.Forms.TextBox();
            this.pnlSearch.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.tbSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(378, 47);
            this.pnlSearch.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(323, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(37, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "->";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(12, 12);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(305, 23);
            this.tbSearch.TabIndex = 0;
            this.tbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyUp);
            // 
            // pnlEdit
            // 
            this.pnlEdit.Controls.Add(this.tbGroupName);
            this.pnlEdit.Controls.Add(this.label10);
            this.pnlEdit.Controls.Add(this.tbType);
            this.pnlEdit.Controls.Add(this.label9);
            this.pnlEdit.Controls.Add(this.tbGroup);
            this.pnlEdit.Controls.Add(this.label8);
            this.pnlEdit.Controls.Add(this.tbTypeName);
            this.pnlEdit.Controls.Add(this.label7);
            this.pnlEdit.Controls.Add(this.tbIssueDate);
            this.pnlEdit.Controls.Add(this.label6);
            this.pnlEdit.Controls.Add(this.tbFaceValue);
            this.pnlEdit.Controls.Add(this.label5);
            this.pnlEdit.Controls.Add(this.tbIsin);
            this.pnlEdit.Controls.Add(this.label4);
            this.pnlEdit.Controls.Add(this.tbShortName);
            this.pnlEdit.Controls.Add(this.label3);
            this.pnlEdit.Controls.Add(this.tbName);
            this.pnlEdit.Controls.Add(this.label2);
            this.pnlEdit.Controls.Add(this.tbSecId);
            this.pnlEdit.Controls.Add(this.label1);
            this.pnlEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEdit.Location = new System.Drawing.Point(0, 47);
            this.pnlEdit.Name = "pnlEdit";
            this.pnlEdit.Size = new System.Drawing.Size(378, 504);
            this.pnlEdit.TabIndex = 1;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(117, 42);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(243, 23);
            this.tbName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Имя:";
            // 
            // tbSecId
            // 
            this.tbSecId.Location = new System.Drawing.Point(117, 13);
            this.tbSecId.Name = "tbSecId";
            this.tbSecId.Size = new System.Drawing.Size(243, 23);
            this.tbSecId.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "SecId:";
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnCancel);
            this.pnlActions.Controls.Add(this.btnSave);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Location = new System.Drawing.Point(0, 500);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(378, 51);
            this.pnlActions.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(93, 16);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(12, 16);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Краткое имя:";
            // 
            // tbShortName
            // 
            this.tbShortName.Location = new System.Drawing.Point(117, 72);
            this.tbShortName.Name = "tbShortName";
            this.tbShortName.Size = new System.Drawing.Size(243, 23);
            this.tbShortName.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 6;
            this.label4.Text = "ISIN:";
            // 
            // tbIsin
            // 
            this.tbIsin.Location = new System.Drawing.Point(117, 104);
            this.tbIsin.Name = "tbIsin";
            this.tbIsin.Size = new System.Drawing.Size(243, 23);
            this.tbIsin.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Ном. стоимость:";
            // 
            // tbFaceValue
            // 
            this.tbFaceValue.Location = new System.Drawing.Point(117, 133);
            this.tbFaceValue.Name = "tbFaceValue";
            this.tbFaceValue.Size = new System.Drawing.Size(243, 23);
            this.tbFaceValue.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "Дата нач. торгов:";
            // 
            // tbIssueDate
            // 
            this.tbIssueDate.Location = new System.Drawing.Point(117, 162);
            this.tbIssueDate.Name = "tbIssueDate";
            this.tbIssueDate.Size = new System.Drawing.Size(243, 23);
            this.tbIssueDate.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 15);
            this.label7.TabIndex = 12;
            this.label7.Text = "Вид бумаги:";
            // 
            // tbTypeName
            // 
            this.tbTypeName.Location = new System.Drawing.Point(117, 191);
            this.tbTypeName.Name = "tbTypeName";
            this.tbTypeName.Size = new System.Drawing.Size(243, 23);
            this.tbTypeName.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 223);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 15);
            this.label8.TabIndex = 14;
            this.label8.Text = "Код типа:";
            // 
            // tbGroup
            // 
            this.tbGroup.Location = new System.Drawing.Point(117, 220);
            this.tbGroup.Name = "tbGroup";
            this.tbGroup.Size = new System.Drawing.Size(243, 23);
            this.tbGroup.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 252);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 15);
            this.label9.TabIndex = 16;
            this.label9.Text = "Тип:";
            // 
            // tbType
            // 
            this.tbType.Location = new System.Drawing.Point(117, 249);
            this.tbType.Name = "tbType";
            this.tbType.Size = new System.Drawing.Size(243, 23);
            this.tbType.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 281);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 15);
            this.label10.TabIndex = 18;
            this.label10.Text = "Тип инструмента:";
            // 
            // tbGroupName
            // 
            this.tbGroupName.Location = new System.Drawing.Point(117, 278);
            this.tbGroupName.Name = "tbGroupName";
            this.tbGroupName.Size = new System.Drawing.Size(243, 23);
            this.tbGroupName.TabIndex = 19;
            // 
            // PaperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 551);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlEdit);
            this.Controls.Add(this.pnlSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaperForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Параметры бумаги";
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlEdit.ResumeLayout(false);
            this.pnlEdit.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Panel pnlEdit;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbSecId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbShortName;
        private System.Windows.Forms.TextBox tbIsin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbFaceValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbIssueDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbTypeName;
        private System.Windows.Forms.TextBox tbGroup;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbGroupName;
    }
}
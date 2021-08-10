
namespace RfBondManagement.WinForm.Forms
{
    partial class SettingsForm
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
            this.lblComission = new System.Windows.Forms.Label();
            this.tbComission = new System.Windows.Forms.TextBox();
            this.lblTax = new System.Windows.Forms.Label();
            this.tbTax = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbActual = new System.Windows.Forms.CheckBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbAccount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbLTB = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblComission
            // 
            this.lblComission.AutoSize = true;
            this.lblComission.Location = new System.Drawing.Point(12, 165);
            this.lblComission.Name = "lblComission";
            this.lblComission.Size = new System.Drawing.Size(82, 15);
            this.lblComission.TabIndex = 0;
            this.lblComission.Text = "Комиссии, %:";
            // 
            // tbComission
            // 
            this.tbComission.Location = new System.Drawing.Point(100, 162);
            this.tbComission.Name = "tbComission";
            this.tbComission.Size = new System.Drawing.Size(57, 23);
            this.tbComission.TabIndex = 4;
            // 
            // lblTax
            // 
            this.lblTax.AutoSize = true;
            this.lblTax.Location = new System.Drawing.Point(12, 192);
            this.lblTax.Name = "lblTax";
            this.lblTax.Size = new System.Drawing.Size(67, 15);
            this.lblTax.TabIndex = 2;
            this.lblTax.Text = "Налоги, %:";
            // 
            // tbTax
            // 
            this.tbTax.Location = new System.Drawing.Point(100, 189);
            this.tbTax.Name = "tbTax";
            this.tbTax.Size = new System.Drawing.Size(57, 23);
            this.tbTax.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(12, 223);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(100, 223);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(57, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbActual
            // 
            this.cbActual.AutoSize = true;
            this.cbActual.Location = new System.Drawing.Point(12, 12);
            this.cbActual.Name = "cbActual";
            this.cbActual.Size = new System.Drawing.Size(150, 19);
            this.cbActual.TabIndex = 0;
            this.cbActual.Text = "Портфель актуальный";
            this.cbActual.UseVisualStyleBackColor = true;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(12, 58);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(145, 23);
            this.tbName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Название:";
            // 
            // tbAccount
            // 
            this.tbAccount.Location = new System.Drawing.Point(12, 108);
            this.tbAccount.Name = "tbAccount";
            this.tbAccount.Size = new System.Drawing.Size(145, 23);
            this.tbAccount.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Счёт:";
            // 
            // cbLTB
            // 
            this.cbLTB.AutoSize = true;
            this.cbLTB.Location = new System.Drawing.Point(11, 137);
            this.cbLTB.Name = "cbLTB";
            this.cbLTB.Size = new System.Drawing.Size(110, 19);
            this.cbLTB.TabIndex = 3;
            this.cbLTB.Text = "Учитывать ЛДВ";
            this.cbLTB.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(177, 262);
            this.Controls.Add(this.cbLTB);
            this.Controls.Add(this.tbAccount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbActual);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbTax);
            this.Controls.Add(this.lblTax);
            this.Controls.Add(this.tbComission);
            this.Controls.Add(this.lblComission);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Общие настройки";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblComission;
        private System.Windows.Forms.TextBox tbComission;
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.TextBox tbTax;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbActual;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAccount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbLTB;
    }
}
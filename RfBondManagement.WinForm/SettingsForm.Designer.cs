
namespace RfBondManagement.WinForm
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
            this.SuspendLayout();
            // 
            // lblComission
            // 
            this.lblComission.AutoSize = true;
            this.lblComission.Location = new System.Drawing.Point(19, 15);
            this.lblComission.Name = "lblComission";
            this.lblComission.Size = new System.Drawing.Size(82, 15);
            this.lblComission.TabIndex = 0;
            this.lblComission.Text = "Комиссии, %:";
            // 
            // tbComission
            // 
            this.tbComission.Location = new System.Drawing.Point(107, 12);
            this.tbComission.Name = "tbComission";
            this.tbComission.Size = new System.Drawing.Size(57, 23);
            this.tbComission.TabIndex = 1;
            // 
            // lblTax
            // 
            this.lblTax.AutoSize = true;
            this.lblTax.Location = new System.Drawing.Point(19, 42);
            this.lblTax.Name = "lblTax";
            this.lblTax.Size = new System.Drawing.Size(67, 15);
            this.lblTax.TabIndex = 2;
            this.lblTax.Text = "Налоги, %:";
            // 
            // tbTax
            // 
            this.tbTax.Location = new System.Drawing.Point(107, 39);
            this.tbTax.Name = "tbTax";
            this.tbTax.Size = new System.Drawing.Size(57, 23);
            this.tbTax.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(19, 73);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(107, 73);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(57, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(181, 103);
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
    }
}
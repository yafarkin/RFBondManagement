
namespace RfBondManagement.WinForm.Controls
{
    partial class WatchListUC
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
            this.pnlActions = new System.Windows.Forms.Panel();
            this.rbMonth = new System.Windows.Forms.RadioButton();
            this.rbWeek = new System.Windows.Forms.RadioButton();
            this.rbDay = new System.Windows.Forms.RadioButton();
            this.btnAdd = new System.Windows.Forms.Button();
            this.pnlPapers = new System.Windows.Forms.Panel();
            this.lvPapers = new System.Windows.Forms.ListView();
            this.chSecId = new System.Windows.Forms.ColumnHeader();
            this.chLastPrice = new System.Windows.Forms.ColumnHeader();
            this.chMinPrice = new System.Windows.Forms.ColumnHeader();
            this.chMaxPrice = new System.Windows.Forms.ColumnHeader();
            this.chDelete = new System.Windows.Forms.ColumnHeader();
            this.pnlGraph = new System.Windows.Forms.Panel();
            this.pnlActions.SuspendLayout();
            this.pnlPapers.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.rbMonth);
            this.pnlActions.Controls.Add(this.rbWeek);
            this.pnlActions.Controls.Add(this.rbDay);
            this.pnlActions.Controls.Add(this.btnAdd);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActions.Location = new System.Drawing.Point(0, 0);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(566, 40);
            this.pnlActions.TabIndex = 0;
            // 
            // rbMonth
            // 
            this.rbMonth.AutoSize = true;
            this.rbMonth.Location = new System.Drawing.Point(215, 8);
            this.rbMonth.Name = "rbMonth";
            this.rbMonth.Size = new System.Drawing.Size(66, 19);
            this.rbMonth.TabIndex = 3;
            this.rbMonth.Text = "30 дней";
            this.rbMonth.UseVisualStyleBackColor = true;
            // 
            // rbWeek
            // 
            this.rbWeek.AutoSize = true;
            this.rbWeek.Location = new System.Drawing.Point(149, 8);
            this.rbWeek.Name = "rbWeek";
            this.rbWeek.Size = new System.Drawing.Size(60, 19);
            this.rbWeek.TabIndex = 2;
            this.rbWeek.Text = "7 дней";
            this.rbWeek.UseVisualStyleBackColor = true;
            // 
            // rbDay
            // 
            this.rbDay.AutoSize = true;
            this.rbDay.Checked = true;
            this.rbDay.Location = new System.Drawing.Point(86, 7);
            this.rbDay.Name = "rbDay";
            this.rbDay.Size = new System.Drawing.Size(57, 19);
            this.rbDay.TabIndex = 1;
            this.rbDay.TabStop = true;
            this.rbDay.Text = "Сутки";
            this.rbDay.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(4, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // pnlPapers
            // 
            this.pnlPapers.Controls.Add(this.lvPapers);
            this.pnlPapers.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlPapers.Location = new System.Drawing.Point(0, 40);
            this.pnlPapers.Name = "pnlPapers";
            this.pnlPapers.Size = new System.Drawing.Size(320, 294);
            this.pnlPapers.TabIndex = 1;
            // 
            // lvPapers
            // 
            this.lvPapers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSecId,
            this.chLastPrice,
            this.chMinPrice,
            this.chMaxPrice,
            this.chDelete});
            this.lvPapers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPapers.FullRowSelect = true;
            this.lvPapers.HideSelection = false;
            this.lvPapers.Location = new System.Drawing.Point(0, 0);
            this.lvPapers.MultiSelect = false;
            this.lvPapers.Name = "lvPapers";
            this.lvPapers.Size = new System.Drawing.Size(320, 294);
            this.lvPapers.TabIndex = 0;
            this.lvPapers.UseCompatibleStateImageBehavior = false;
            this.lvPapers.View = System.Windows.Forms.View.Details;
            this.lvPapers.SelectedIndexChanged += new System.EventHandler(this.lvPapers_SelectedIndexChanged);
            // 
            // chSecId
            // 
            this.chSecId.Text = "SecId";
            // 
            // chLastPrice
            // 
            this.chLastPrice.Text = "Цена";
            // 
            // chMinPrice
            // 
            this.chMinPrice.Text = "Мин. цена";
            // 
            // chMaxPrice
            // 
            this.chMaxPrice.Text = "Макс. цена";
            // 
            // chDelete
            // 
            this.chDelete.Text = "Удалить";
            // 
            // pnlGraph
            // 
            this.pnlGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGraph.Location = new System.Drawing.Point(320, 40);
            this.pnlGraph.Name = "pnlGraph";
            this.pnlGraph.Size = new System.Drawing.Size(246, 294);
            this.pnlGraph.TabIndex = 2;
            // 
            // WatchListUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlGraph);
            this.Controls.Add(this.pnlPapers);
            this.Controls.Add(this.pnlActions);
            this.Name = "WatchListUC";
            this.Size = new System.Drawing.Size(566, 334);
            this.Load += new System.EventHandler(this.WatchListUC_Load);
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.pnlPapers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Panel pnlPapers;
        private System.Windows.Forms.Panel pnlGraph;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.RadioButton rbMonth;
        private System.Windows.Forms.RadioButton rbWeek;
        private System.Windows.Forms.RadioButton rbDay;
        private System.Windows.Forms.ListView lvPapers;
        private System.Windows.Forms.ColumnHeader chSecId;
        private System.Windows.Forms.ColumnHeader chLastPrice;
        private System.Windows.Forms.ColumnHeader chMinPrice;
        private System.Windows.Forms.ColumnHeader chMaxPrice;
        private System.Windows.Forms.ColumnHeader chDelete;
    }
}

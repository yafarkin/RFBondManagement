
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
            this.components = new System.ComponentModel.Container();
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
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
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
            this.pnlActions.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(647, 53);
            this.pnlActions.TabIndex = 0;
            // 
            // rbMonth
            // 
            this.rbMonth.AutoSize = true;
            this.rbMonth.Checked = true;
            this.rbMonth.Location = new System.Drawing.Point(246, 11);
            this.rbMonth.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbMonth.Name = "rbMonth";
            this.rbMonth.Size = new System.Drawing.Size(84, 24);
            this.rbMonth.TabIndex = 3;
            this.rbMonth.TabStop = true;
            this.rbMonth.Text = "30 дней";
            this.rbMonth.UseVisualStyleBackColor = true;
            this.rbMonth.CheckedChanged += new System.EventHandler(this.rbPeriod_CheckedChanged);
            // 
            // rbWeek
            // 
            this.rbWeek.AutoSize = true;
            this.rbWeek.Location = new System.Drawing.Point(170, 11);
            this.rbWeek.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbWeek.Name = "rbWeek";
            this.rbWeek.Size = new System.Drawing.Size(76, 24);
            this.rbWeek.TabIndex = 2;
            this.rbWeek.Text = "7 дней";
            this.rbWeek.UseVisualStyleBackColor = true;
            this.rbWeek.CheckedChanged += new System.EventHandler(this.rbPeriod_CheckedChanged);
            // 
            // rbDay
            // 
            this.rbDay.AutoSize = true;
            this.rbDay.Location = new System.Drawing.Point(98, 9);
            this.rbDay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbDay.Name = "rbDay";
            this.rbDay.Size = new System.Drawing.Size(68, 24);
            this.rbDay.TabIndex = 1;
            this.rbDay.Text = "Сутки";
            this.rbDay.UseVisualStyleBackColor = true;
            this.rbDay.CheckedChanged += new System.EventHandler(this.rbPeriod_CheckedChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(5, 5);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(86, 31);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // pnlPapers
            // 
            this.pnlPapers.Controls.Add(this.lvPapers);
            this.pnlPapers.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlPapers.Location = new System.Drawing.Point(0, 53);
            this.pnlPapers.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlPapers.Name = "pnlPapers";
            this.pnlPapers.Size = new System.Drawing.Size(366, 392);
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
            this.lvPapers.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lvPapers.MultiSelect = false;
            this.lvPapers.Name = "lvPapers";
            this.lvPapers.Size = new System.Drawing.Size(366, 392);
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
            this.pnlGraph.Location = new System.Drawing.Point(366, 53);
            this.pnlGraph.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlGraph.Name = "pnlGraph";
            this.pnlGraph.Size = new System.Drawing.Size(281, 392);
            this.pnlGraph.TabIndex = 2;
            // 
            // timerRefresh
            // 
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Interval = 1200000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // WatchListUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlGraph);
            this.Controls.Add(this.pnlPapers);
            this.Controls.Add(this.pnlActions);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "WatchListUC";
            this.Size = new System.Drawing.Size(647, 445);
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
        private System.Windows.Forms.Timer timerRefresh;
    }
}

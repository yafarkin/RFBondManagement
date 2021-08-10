
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
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rb5Years = new System.Windows.Forms.RadioButton();
            this.rbYTD = new System.Windows.Forms.RadioButton();
            this.rbYear = new System.Windows.Forms.RadioButton();
            this.rb6Months = new System.Windows.Forms.RadioButton();
            this.rb3Months = new System.Windows.Forms.RadioButton();
            this.rbMonth = new System.Windows.Forms.RadioButton();
            this.rbWeek = new System.Windows.Forms.RadioButton();
            this.btnAdd = new System.Windows.Forms.Button();
            this.pnlPapers = new System.Windows.Forms.Panel();
            this.lvPapers = new System.Windows.Forms.ListView();
            this.chSecId = new System.Windows.Forms.ColumnHeader();
            this.chLastPrice = new System.Windows.Forms.ColumnHeader();
            this.chMinPrice = new System.Windows.Forms.ColumnHeader();
            this.chMaxPrice = new System.Windows.Forms.ColumnHeader();
            this.pnlGraph = new System.Windows.Forms.Panel();
            this.pnlGraphData = new System.Windows.Forms.Panel();
            this.pnlGraphType = new System.Windows.Forms.Panel();
            this.rbGraphPoints = new System.Windows.Forms.RadioButton();
            this.rbGraphCandle = new System.Windows.Forms.RadioButton();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.pnlActions.SuspendLayout();
            this.pnlPapers.SuspendLayout();
            this.pnlGraph.SuspendLayout();
            this.pnlGraphType.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.rbAll);
            this.pnlActions.Controls.Add(this.rb5Years);
            this.pnlActions.Controls.Add(this.rbYTD);
            this.pnlActions.Controls.Add(this.rbYear);
            this.pnlActions.Controls.Add(this.rb6Months);
            this.pnlActions.Controls.Add(this.rb3Months);
            this.pnlActions.Controls.Add(this.rbMonth);
            this.pnlActions.Controls.Add(this.rbWeek);
            this.pnlActions.Controls.Add(this.btnAdd);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActions.Location = new System.Drawing.Point(0, 0);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(566, 40);
            this.pnlActions.TabIndex = 0;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(396, 12);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(33, 19);
            this.rbAll.TabIndex = 8;
            this.rbAll.Text = "A";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbPeriod_CheckedChanged);
            // 
            // rb5Years
            // 
            this.rb5Years.AutoSize = true;
            this.rb5Years.Location = new System.Drawing.Point(352, 12);
            this.rb5Years.Name = "rb5Years";
            this.rb5Years.Size = new System.Drawing.Size(38, 19);
            this.rb5Years.TabIndex = 7;
            this.rb5Years.Text = "5Y";
            this.rb5Years.UseVisualStyleBackColor = true;
            this.rb5Years.CheckedChanged += new System.EventHandler(this.rbPeriod_CheckedChanged);
            // 
            // rbYTD
            // 
            this.rbYTD.AutoSize = true;
            this.rbYTD.Location = new System.Drawing.Point(302, 12);
            this.rbYTD.Name = "rbYTD";
            this.rbYTD.Size = new System.Drawing.Size(46, 19);
            this.rbYTD.TabIndex = 6;
            this.rbYTD.Text = "YTD";
            this.rbYTD.UseVisualStyleBackColor = true;
            this.rbYTD.CheckedChanged += new System.EventHandler(this.rbPeriod_CheckedChanged);
            // 
            // rbYear
            // 
            this.rbYear.AutoSize = true;
            this.rbYear.Checked = true;
            this.rbYear.Location = new System.Drawing.Point(264, 12);
            this.rbYear.Name = "rbYear";
            this.rbYear.Size = new System.Drawing.Size(32, 19);
            this.rbYear.TabIndex = 5;
            this.rbYear.TabStop = true;
            this.rbYear.Text = "Y";
            this.rbYear.UseVisualStyleBackColor = true;
            this.rbYear.CheckedChanged += new System.EventHandler(this.rbPeriod_CheckedChanged);
            // 
            // rb6Months
            // 
            this.rb6Months.AutoSize = true;
            this.rb6Months.Location = new System.Drawing.Point(216, 12);
            this.rb6Months.Name = "rb6Months";
            this.rb6Months.Size = new System.Drawing.Size(42, 19);
            this.rb6Months.TabIndex = 4;
            this.rb6Months.Text = "6M";
            this.rb6Months.UseVisualStyleBackColor = true;
            this.rb6Months.CheckedChanged += new System.EventHandler(this.rbPeriod_CheckedChanged);
            // 
            // rb3Months
            // 
            this.rb3Months.AutoSize = true;
            this.rb3Months.Location = new System.Drawing.Point(168, 12);
            this.rb3Months.Name = "rb3Months";
            this.rb3Months.Size = new System.Drawing.Size(42, 19);
            this.rb3Months.TabIndex = 3;
            this.rb3Months.Text = "3M";
            this.rb3Months.UseVisualStyleBackColor = true;
            this.rb3Months.CheckedChanged += new System.EventHandler(this.rbPeriod_CheckedChanged);
            // 
            // rbMonth
            // 
            this.rbMonth.AutoSize = true;
            this.rbMonth.Location = new System.Drawing.Point(126, 12);
            this.rbMonth.Name = "rbMonth";
            this.rbMonth.Size = new System.Drawing.Size(36, 19);
            this.rbMonth.TabIndex = 2;
            this.rbMonth.Text = "M";
            this.rbMonth.UseVisualStyleBackColor = true;
            this.rbMonth.CheckedChanged += new System.EventHandler(this.rbPeriod_CheckedChanged);
            // 
            // rbWeek
            // 
            this.rbWeek.AutoSize = true;
            this.rbWeek.Location = new System.Drawing.Point(84, 12);
            this.rbWeek.Name = "rbWeek";
            this.rbWeek.Size = new System.Drawing.Size(36, 19);
            this.rbWeek.TabIndex = 1;
            this.rbWeek.Text = "W";
            this.rbWeek.UseVisualStyleBackColor = true;
            this.rbWeek.CheckedChanged += new System.EventHandler(this.rbPeriod_CheckedChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 8);
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
            this.chMaxPrice});
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
            // pnlGraph
            // 
            this.pnlGraph.Controls.Add(this.pnlGraphData);
            this.pnlGraph.Controls.Add(this.pnlGraphType);
            this.pnlGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGraph.Location = new System.Drawing.Point(320, 40);
            this.pnlGraph.Name = "pnlGraph";
            this.pnlGraph.Size = new System.Drawing.Size(246, 294);
            this.pnlGraph.TabIndex = 2;
            // 
            // pnlGraphData
            // 
            this.pnlGraphData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGraphData.Location = new System.Drawing.Point(0, 38);
            this.pnlGraphData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlGraphData.Name = "pnlGraphData";
            this.pnlGraphData.Size = new System.Drawing.Size(246, 256);
            this.pnlGraphData.TabIndex = 1;
            // 
            // pnlGraphType
            // 
            this.pnlGraphType.Controls.Add(this.rbGraphPoints);
            this.pnlGraphType.Controls.Add(this.rbGraphCandle);
            this.pnlGraphType.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGraphType.Location = new System.Drawing.Point(0, 0);
            this.pnlGraphType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlGraphType.Name = "pnlGraphType";
            this.pnlGraphType.Size = new System.Drawing.Size(246, 38);
            this.pnlGraphType.TabIndex = 0;
            // 
            // rbGraphPoints
            // 
            this.rbGraphPoints.AutoSize = true;
            this.rbGraphPoints.Location = new System.Drawing.Point(81, 10);
            this.rbGraphPoints.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbGraphPoints.Name = "rbGraphPoints";
            this.rbGraphPoints.Size = new System.Drawing.Size(66, 19);
            this.rbGraphPoints.TabIndex = 1;
            this.rbGraphPoints.Text = "График";
            this.rbGraphPoints.UseVisualStyleBackColor = true;
            this.rbGraphPoints.CheckedChanged += new System.EventHandler(this.rbGraph_CheckedChanged);
            // 
            // rbGraphCandle
            // 
            this.rbGraphCandle.AutoSize = true;
            this.rbGraphCandle.Checked = true;
            this.rbGraphCandle.Location = new System.Drawing.Point(13, 10);
            this.rbGraphCandle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbGraphCandle.Name = "rbGraphCandle";
            this.rbGraphCandle.Size = new System.Drawing.Size(59, 19);
            this.rbGraphCandle.TabIndex = 0;
            this.rbGraphCandle.TabStop = true;
            this.rbGraphCandle.Text = "Свечи";
            this.rbGraphCandle.UseVisualStyleBackColor = true;
            this.rbGraphCandle.CheckedChanged += new System.EventHandler(this.rbGraph_CheckedChanged);
            // 
            // timerRefresh
            // 
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Interval = 1200000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
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
            this.pnlGraph.ResumeLayout(false);
            this.pnlGraphType.ResumeLayout(false);
            this.pnlGraphType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Panel pnlPapers;
        private System.Windows.Forms.Panel pnlGraph;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.RadioButton rb3Months;
        private System.Windows.Forms.RadioButton rbMonth;
        private System.Windows.Forms.RadioButton rbWeek;
        private System.Windows.Forms.ListView lvPapers;
        private System.Windows.Forms.ColumnHeader chSecId;
        private System.Windows.Forms.ColumnHeader chLastPrice;
        private System.Windows.Forms.ColumnHeader chMinPrice;
        private System.Windows.Forms.ColumnHeader chMaxPrice;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Panel pnlGraphType;
        private System.Windows.Forms.RadioButton rbGraphPoints;
        private System.Windows.Forms.RadioButton rbGraphCandle;
        private System.Windows.Forms.Panel pnlGraphData;
        private System.Windows.Forms.RadioButton rb6Months;
        private System.Windows.Forms.RadioButton rbYear;
        private System.Windows.Forms.RadioButton rbYTD;
        private System.Windows.Forms.RadioButton rb5Years;
        private System.Windows.Forms.RadioButton rbAll;
    }
}

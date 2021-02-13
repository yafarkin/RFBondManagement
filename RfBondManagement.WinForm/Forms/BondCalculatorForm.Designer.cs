
namespace RfBondManagement.WinForm.Forms
{
    partial class BondCalculatorForm
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
            this.psBond = new RfBondManagement.WinForm.Controls.PaperSelectUC();
            this.pnlSettings = new System.Windows.Forms.Panel();
            this.dtpSellDate = new System.Windows.Forms.DateTimePicker();
            this.lblSellWhen = new System.Windows.Forms.Label();
            this.tbSellPrice = new System.Windows.Forms.TextBox();
            this.lblSell = new System.Windows.Forms.Label();
            this.cbUntilMaturityDate = new System.Windows.Forms.CheckBox();
            this.dtpBuyDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.tbNKD = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbBuyPrice = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbCount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbInflation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbTax = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbComission = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlResults = new System.Windows.Forms.Panel();
            this.pnlGraph = new System.Windows.Forms.Panel();
            this.pnlBalanceStory = new System.Windows.Forms.Panel();
            this.pnlCommonResults = new System.Windows.Forms.Panel();
            this.lblBreakevenDate = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblTotalBalance = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblExpectedIncome = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblPaymentByCoupons = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblTaxOnSell = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblStartBalance = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblRealIncomePercent = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pnlSettings.SuspendLayout();
            this.pnlResults.SuspendLayout();
            this.pnlCommonResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // psBond
            // 
            this.psBond.Dock = System.Windows.Forms.DockStyle.Top;
            this.psBond.Location = new System.Drawing.Point(0, 0);
            this.psBond.Name = "psBond";
            this.psBond.Size = new System.Drawing.Size(693, 45);
            this.psBond.TabIndex = 0;
            this.psBond.OnSelectStockPaper += new System.Action<RfBondManagement.Engine.Common.BaseStockPaper>(this.psBond_OnSelectStockPaper);
            // 
            // pnlSettings
            // 
            this.pnlSettings.Controls.Add(this.dtpSellDate);
            this.pnlSettings.Controls.Add(this.lblSellWhen);
            this.pnlSettings.Controls.Add(this.tbSellPrice);
            this.pnlSettings.Controls.Add(this.lblSell);
            this.pnlSettings.Controls.Add(this.cbUntilMaturityDate);
            this.pnlSettings.Controls.Add(this.dtpBuyDate);
            this.pnlSettings.Controls.Add(this.label7);
            this.pnlSettings.Controls.Add(this.tbNKD);
            this.pnlSettings.Controls.Add(this.label6);
            this.pnlSettings.Controls.Add(this.tbBuyPrice);
            this.pnlSettings.Controls.Add(this.label5);
            this.pnlSettings.Controls.Add(this.tbCount);
            this.pnlSettings.Controls.Add(this.label4);
            this.pnlSettings.Controls.Add(this.tbInflation);
            this.pnlSettings.Controls.Add(this.label3);
            this.pnlSettings.Controls.Add(this.tbTax);
            this.pnlSettings.Controls.Add(this.label2);
            this.pnlSettings.Controls.Add(this.tbComission);
            this.pnlSettings.Controls.Add(this.label1);
            this.pnlSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSettings.Location = new System.Drawing.Point(0, 45);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new System.Drawing.Size(693, 155);
            this.pnlSettings.TabIndex = 1;
            // 
            // dtpSellDate
            // 
            this.dtpSellDate.Location = new System.Drawing.Point(421, 118);
            this.dtpSellDate.Name = "dtpSellDate";
            this.dtpSellDate.Size = new System.Drawing.Size(129, 23);
            this.dtpSellDate.TabIndex = 18;
            // 
            // lblSellWhen
            // 
            this.lblSellWhen.AutoSize = true;
            this.lblSellWhen.Location = new System.Drawing.Point(374, 124);
            this.lblSellWhen.Name = "lblSellWhen";
            this.lblSellWhen.Size = new System.Drawing.Size(38, 15);
            this.lblSellWhen.TabIndex = 17;
            this.lblSellWhen.Text = "Когда";
            // 
            // tbSellPrice
            // 
            this.tbSellPrice.Location = new System.Drawing.Point(241, 121);
            this.tbSellPrice.Name = "tbSellPrice";
            this.tbSellPrice.Size = new System.Drawing.Size(100, 23);
            this.tbSellPrice.TabIndex = 16;
            // 
            // lblSell
            // 
            this.lblSell.AutoSize = true;
            this.lblSell.Location = new System.Drawing.Point(174, 124);
            this.lblSell.Name = "lblSell";
            this.lblSell.Size = new System.Drawing.Size(57, 15);
            this.lblSell.TabIndex = 15;
            this.lblSell.Text = "Продажа";
            // 
            // cbUntilMaturityDate
            // 
            this.cbUntilMaturityDate.AutoSize = true;
            this.cbUntilMaturityDate.Location = new System.Drawing.Point(12, 125);
            this.cbUntilMaturityDate.Name = "cbUntilMaturityDate";
            this.cbUntilMaturityDate.Size = new System.Drawing.Size(106, 19);
            this.cbUntilMaturityDate.TabIndex = 14;
            this.cbUntilMaturityDate.Text = "До погашения";
            this.cbUntilMaturityDate.UseVisualStyleBackColor = true;
            this.cbUntilMaturityDate.CheckedChanged += new System.EventHandler(this.cbUntilMaturityDate_CheckedChanged);
            // 
            // dtpBuyDate
            // 
            this.dtpBuyDate.Location = new System.Drawing.Point(421, 81);
            this.dtpBuyDate.Name = "dtpBuyDate";
            this.dtpBuyDate.Size = new System.Drawing.Size(129, 23);
            this.dtpBuyDate.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(374, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 15);
            this.label7.TabIndex = 12;
            this.label7.Text = "Когда";
            // 
            // tbNKD
            // 
            this.tbNKD.Location = new System.Drawing.Point(241, 81);
            this.tbNKD.Name = "tbNKD";
            this.tbNKD.Size = new System.Drawing.Size(100, 23);
            this.tbNKD.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(200, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "НКД";
            // 
            // tbBuyPrice
            // 
            this.tbBuyPrice.Location = new System.Drawing.Point(84, 81);
            this.tbBuyPrice.Name = "tbBuyPrice";
            this.tbBuyPrice.Size = new System.Drawing.Size(100, 23);
            this.tbBuyPrice.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Купля";
            // 
            // tbCount
            // 
            this.tbCount.Location = new System.Drawing.Point(84, 42);
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(100, 23);
            this.tbCount.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Количество";
            // 
            // tbInflation
            // 
            this.tbInflation.Enabled = false;
            this.tbInflation.Location = new System.Drawing.Point(421, 12);
            this.tbInflation.Name = "tbInflation";
            this.tbInflation.Size = new System.Drawing.Size(100, 23);
            this.tbInflation.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(347, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Инфляция";
            // 
            // tbTax
            // 
            this.tbTax.Location = new System.Drawing.Point(241, 12);
            this.tbTax.Name = "tbTax";
            this.tbTax.Size = new System.Drawing.Size(100, 23);
            this.tbTax.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(190, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Налог";
            // 
            // tbComission
            // 
            this.tbComission.Location = new System.Drawing.Point(84, 9);
            this.tbComission.Name = "tbComission";
            this.tbComission.Size = new System.Drawing.Size(100, 23);
            this.tbComission.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Комиссии:";
            // 
            // pnlResults
            // 
            this.pnlResults.Controls.Add(this.pnlGraph);
            this.pnlResults.Controls.Add(this.pnlBalanceStory);
            this.pnlResults.Controls.Add(this.pnlCommonResults);
            this.pnlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResults.Location = new System.Drawing.Point(0, 200);
            this.pnlResults.Name = "pnlResults";
            this.pnlResults.Size = new System.Drawing.Size(693, 461);
            this.pnlResults.TabIndex = 2;
            // 
            // pnlGraph
            // 
            this.pnlGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGraph.Location = new System.Drawing.Point(395, 100);
            this.pnlGraph.Name = "pnlGraph";
            this.pnlGraph.Size = new System.Drawing.Size(298, 361);
            this.pnlGraph.TabIndex = 2;
            // 
            // pnlBalanceStory
            // 
            this.pnlBalanceStory.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBalanceStory.Location = new System.Drawing.Point(0, 100);
            this.pnlBalanceStory.Name = "pnlBalanceStory";
            this.pnlBalanceStory.Size = new System.Drawing.Size(395, 361);
            this.pnlBalanceStory.TabIndex = 1;
            // 
            // pnlCommonResults
            // 
            this.pnlCommonResults.Controls.Add(this.lblBreakevenDate);
            this.pnlCommonResults.Controls.Add(this.label14);
            this.pnlCommonResults.Controls.Add(this.lblTotalBalance);
            this.pnlCommonResults.Controls.Add(this.label13);
            this.pnlCommonResults.Controls.Add(this.lblExpectedIncome);
            this.pnlCommonResults.Controls.Add(this.label12);
            this.pnlCommonResults.Controls.Add(this.lblPaymentByCoupons);
            this.pnlCommonResults.Controls.Add(this.label11);
            this.pnlCommonResults.Controls.Add(this.lblTaxOnSell);
            this.pnlCommonResults.Controls.Add(this.label10);
            this.pnlCommonResults.Controls.Add(this.lblStartBalance);
            this.pnlCommonResults.Controls.Add(this.label9);
            this.pnlCommonResults.Controls.Add(this.lblRealIncomePercent);
            this.pnlCommonResults.Controls.Add(this.label8);
            this.pnlCommonResults.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCommonResults.Location = new System.Drawing.Point(0, 0);
            this.pnlCommonResults.Name = "pnlCommonResults";
            this.pnlCommonResults.Size = new System.Drawing.Size(693, 100);
            this.pnlCommonResults.TabIndex = 0;
            // 
            // lblBreakevenDate
            // 
            this.lblBreakevenDate.AutoSize = true;
            this.lblBreakevenDate.Location = new System.Drawing.Point(546, 18);
            this.lblBreakevenDate.Name = "lblBreakevenDate";
            this.lblBreakevenDate.Size = new System.Drawing.Size(61, 15);
            this.lblBreakevenDate.TabIndex = 13;
            this.lblBreakevenDate.Text = "00.00.0000";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(417, 18);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(129, 15);
            this.label14.TabIndex = 12;
            this.label14.Text = "Дата безубыточности:";
            // 
            // lblTotalBalance
            // 
            this.lblTotalBalance.AutoSize = true;
            this.lblTotalBalance.Location = new System.Drawing.Point(367, 74);
            this.lblTotalBalance.Name = "lblTotalBalance";
            this.lblTotalBalance.Size = new System.Drawing.Size(28, 15);
            this.lblTotalBalance.TabIndex = 11;
            this.lblTotalBalance.Text = "0,00";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(241, 74);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(113, 15);
            this.label13.TabIndex = 10;
            this.label13.Text = "Сумма погашения:";
            // 
            // lblExpectedIncome
            // 
            this.lblExpectedIncome.AutoSize = true;
            this.lblExpectedIncome.Location = new System.Drawing.Point(367, 17);
            this.lblExpectedIncome.Name = "lblExpectedIncome";
            this.lblExpectedIncome.Size = new System.Drawing.Size(28, 15);
            this.lblExpectedIncome.TabIndex = 9;
            this.lblExpectedIncome.Text = "0,00";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(239, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 15);
            this.label12.TabIndex = 8;
            this.label12.Text = "Ожидаемый доход:";
            // 
            // lblPaymentByCoupons
            // 
            this.lblPaymentByCoupons.AutoSize = true;
            this.lblPaymentByCoupons.Location = new System.Drawing.Point(367, 45);
            this.lblPaymentByCoupons.Name = "lblPaymentByCoupons";
            this.lblPaymentByCoupons.Size = new System.Drawing.Size(28, 15);
            this.lblPaymentByCoupons.TabIndex = 7;
            this.lblPaymentByCoupons.Text = "0,00";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(239, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(119, 15);
            this.label11.TabIndex = 6;
            this.label11.Text = "Выплат по купонам:";
            // 
            // lblTaxOnSell
            // 
            this.lblTaxOnSell.AutoSize = true;
            this.lblTaxOnSell.Location = new System.Drawing.Point(179, 74);
            this.lblTaxOnSell.Name = "lblTaxOnSell";
            this.lblTaxOnSell.Size = new System.Drawing.Size(28, 15);
            this.lblTaxOnSell.TabIndex = 5;
            this.lblTaxOnSell.Text = "0,00";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(119, 15);
            this.label10.TabIndex = 4;
            this.label10.Text = "Налог при продаже:";
            // 
            // lblStartBalance
            // 
            this.lblStartBalance.AutoSize = true;
            this.lblStartBalance.Location = new System.Drawing.Point(179, 45);
            this.lblStartBalance.Name = "lblStartBalance";
            this.lblStartBalance.Size = new System.Drawing.Size(28, 15);
            this.lblStartBalance.TabIndex = 3;
            this.lblStartBalance.Text = "0,00";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 15);
            this.label9.TabIndex = 2;
            this.label9.Text = "Сумма на начало:";
            // 
            // lblRealIncomePercent
            // 
            this.lblRealIncomePercent.AutoSize = true;
            this.lblRealIncomePercent.Location = new System.Drawing.Point(179, 17);
            this.lblRealIncomePercent.Name = "lblRealIncomePercent";
            this.lblRealIncomePercent.Size = new System.Drawing.Size(41, 15);
            this.lblRealIncomePercent.TabIndex = 1;
            this.lblRealIncomePercent.Text = "0,00 %";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(153, 15);
            this.label8.TabIndex = 0;
            this.label8.Text = "Доходность к погашению:";
            // 
            // BondCalculatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 661);
            this.Controls.Add(this.pnlResults);
            this.Controls.Add(this.pnlSettings);
            this.Controls.Add(this.psBond);
            this.MinimumSize = new System.Drawing.Size(700, 700);
            this.Name = "BondCalculatorForm";
            this.Text = "Калькулятор облигации";
            this.Load += new System.EventHandler(this.BondCalculatorForm_Load);
            this.pnlSettings.ResumeLayout(false);
            this.pnlSettings.PerformLayout();
            this.pnlResults.ResumeLayout(false);
            this.pnlCommonResults.ResumeLayout(false);
            this.pnlCommonResults.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.PaperSelectUC psBond;
        private System.Windows.Forms.Panel pnlSettings;
        private System.Windows.Forms.TextBox tbComission;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbTax;
        private System.Windows.Forms.TextBox tbInflation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbBuyPrice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbNKD;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpBuyDate;
        private System.Windows.Forms.CheckBox cbUntilMaturityDate;
        private System.Windows.Forms.Label lblSell;
        private System.Windows.Forms.TextBox tbSellPrice;
        private System.Windows.Forms.DateTimePicker dtpSellDate;
        private System.Windows.Forms.Label lblSellWhen;
        private System.Windows.Forms.Panel pnlResults;
        private System.Windows.Forms.Panel pnlCommonResults;
        private System.Windows.Forms.Label lblRealIncomePercent;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblStartBalance;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblTaxOnSell;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblPaymentByCoupons;
        private System.Windows.Forms.Label lblExpectedIncome;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblTotalBalance;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblBreakevenDate;
        private System.Windows.Forms.Panel pnlBalanceStory;
        private System.Windows.Forms.Panel pnlGraph;
    }
}
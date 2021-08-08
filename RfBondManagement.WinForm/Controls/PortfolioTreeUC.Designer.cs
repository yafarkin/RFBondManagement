
namespace RfBondManagement.WinForm.Controls
{
    partial class PortfolioTreeUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PortfolioTreeUC));
            this.pnlAction = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.pnlData = new System.Windows.Forms.Panel();
            this.tvPortfolio = new System.Windows.Forms.TreeView();
            this.ilStructure = new System.Windows.Forms.ImageList(this.components);
            this.pnlAction.SuspendLayout();
            this.pnlData.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAction
            // 
            this.pnlAction.Controls.Add(this.btnDelete);
            this.pnlAction.Controls.Add(this.btnEdit);
            this.pnlAction.Controls.Add(this.btnAdd);
            this.pnlAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAction.Location = new System.Drawing.Point(0, 0);
            this.pnlAction.Name = "pnlAction";
            this.pnlAction.Size = new System.Drawing.Size(665, 29);
            this.pnlAction.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(165, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(84, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Изменить";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // pnlData
            // 
            this.pnlData.Controls.Add(this.tvPortfolio);
            this.pnlData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlData.Location = new System.Drawing.Point(0, 29);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(665, 421);
            this.pnlData.TabIndex = 1;
            // 
            // tvPortfolio
            // 
            this.tvPortfolio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvPortfolio.ImageIndex = 0;
            this.tvPortfolio.ImageList = this.ilStructure;
            this.tvPortfolio.Location = new System.Drawing.Point(0, 0);
            this.tvPortfolio.Name = "tvPortfolio";
            this.tvPortfolio.SelectedImageIndex = 0;
            this.tvPortfolio.Size = new System.Drawing.Size(665, 421);
            this.tvPortfolio.TabIndex = 0;
            // 
            // ilStructure
            // 
            this.ilStructure.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.ilStructure.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilStructure.ImageStream")));
            this.ilStructure.TransparentColor = System.Drawing.Color.Transparent;
            this.ilStructure.Images.SetKeyName(0, "group");
            this.ilStructure.Images.SetKeyName(1, "paper");
            // 
            // PortfolioTreeUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlData);
            this.Controls.Add(this.pnlAction);
            this.Name = "PortfolioTreeUC";
            this.Size = new System.Drawing.Size(665, 450);
            this.Load += new System.EventHandler(this.PortfolioTreeUC_Load);
            this.pnlAction.ResumeLayout(false);
            this.pnlData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlAction;
        private System.Windows.Forms.Panel pnlData;
        private System.Windows.Forms.TreeView tvPortfolio;
        private System.Windows.Forms.ImageList ilStructure;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
    }
}

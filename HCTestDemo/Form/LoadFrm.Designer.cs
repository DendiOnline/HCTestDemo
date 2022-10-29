namespace HCTestDemo
{
    partial class LoadFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadFrm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbx_TestForm = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_OpenTest = new Sunny.UI.UISymbolButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel1.Location = new System.Drawing.Point(3, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(409, 211);
            this.panel1.TabIndex = 0;
            // 
            // lbx_TestForm
            // 
            this.lbx_TestForm.FormattingEnabled = true;
            this.lbx_TestForm.ItemHeight = 18;
            this.lbx_TestForm.Location = new System.Drawing.Point(441, 55);
            this.lbx_TestForm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lbx_TestForm.Name = "lbx_TestForm";
            this.lbx_TestForm.Size = new System.Drawing.Size(311, 454);
            this.lbx_TestForm.TabIndex = 1;
            this.lbx_TestForm.SelectedIndexChanged += new System.EventHandler(this.lbx_TestForm_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(436, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "测试项目集合";
            // 
            // bt_OpenTest
            // 
            this.bt_OpenTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bt_OpenTest.FillColor = System.Drawing.Color.Teal;
            this.bt_OpenTest.FillHoverColor = System.Drawing.Color.DarkSlateGray;
            this.bt_OpenTest.FillPressColor = System.Drawing.Color.DarkSlateGray;
            this.bt_OpenTest.FillSelectedColor = System.Drawing.Color.DarkSlateGray;
            this.bt_OpenTest.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_OpenTest.Location = new System.Drawing.Point(37, 431);
            this.bt_OpenTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bt_OpenTest.MinimumSize = new System.Drawing.Size(1, 1);
            this.bt_OpenTest.Name = "bt_OpenTest";
            this.bt_OpenTest.RectColor = System.Drawing.Color.Teal;
            this.bt_OpenTest.RectHoverColor = System.Drawing.Color.DarkSlateGray;
            this.bt_OpenTest.RectPressColor = System.Drawing.Color.DarkSlateGray;
            this.bt_OpenTest.RectSelectedColor = System.Drawing.Color.DarkSlateGray;
            this.bt_OpenTest.Size = new System.Drawing.Size(141, 47);
            this.bt_OpenTest.Style = Sunny.UI.UIStyle.Custom;
            this.bt_OpenTest.StyleCustomMode = true;
            this.bt_OpenTest.Symbol = 61912;
            this.bt_OpenTest.SymbolSize = 28;
            this.bt_OpenTest.TabIndex = 32;
            this.bt_OpenTest.Text = "打开测试项";
            this.bt_OpenTest.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_OpenTest.Click += new System.EventHandler(this.bt_OpenTest_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(31, 292);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 30);
            this.label2.TabIndex = 33;
            this.label2.Text = "选择项目:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(51, 359);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 30);
            this.label3.TabIndex = 34;
            this.label3.Text = "选择项目:";
            // 
            // LoadFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(783, 545);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bt_OpenTest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbx_TestForm);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "LoadFrm";
            this.Text = "测试项目选择";
            this.Load += new System.EventHandler(this.LoadFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lbx_TestForm;
        private System.Windows.Forms.Label label1;
        private Sunny.UI.UISymbolButton bt_OpenTest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
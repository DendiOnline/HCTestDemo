namespace HCTestDemo
{
    partial class ExcelPathNameInit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExcelPathNameInit));
            this.bt_Confirm = new Sunny.UI.UISymbolButton();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.fileSaveName = new System.Windows.Forms.TextBox();
            this.SaveInterval = new System.Windows.Forms.TextBox();
            this.label55 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bt_Cancel = new Sunny.UI.UISymbolButton();
            this.SelectFiles = new Sunny.UI.UICheckBoxGroup();
            this.SuspendLayout();
            // 
            // bt_Confirm
            // 
            this.bt_Confirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bt_Confirm.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Confirm.Location = new System.Drawing.Point(89, 137);
            this.bt_Confirm.Margin = new System.Windows.Forms.Padding(2);
            this.bt_Confirm.MinimumSize = new System.Drawing.Size(1, 1);
            this.bt_Confirm.Name = "bt_Confirm";
            this.bt_Confirm.Size = new System.Drawing.Size(94, 31);
            this.bt_Confirm.Style = Sunny.UI.UIStyle.Custom;
            this.bt_Confirm.SymbolSize = 30;
            this.bt_Confirm.TabIndex = 0;
            this.bt_Confirm.Text = "确认";
            this.bt_Confirm.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Confirm.Click += new System.EventHandler(this.bt_Confirm_Click);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(11, 20);
            this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(65, 12);
            this.label43.TabIndex = 71;
            this.label43.Text = "保存间隔：";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(10, 57);
            this.label44.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(77, 12);
            this.label44.TabIndex = 73;
            this.label44.Text = "保存文件名：";
            // 
            // fileSaveName
            // 
            this.fileSaveName.Location = new System.Drawing.Point(89, 54);
            this.fileSaveName.Margin = new System.Windows.Forms.Padding(2);
            this.fileSaveName.Name = "fileSaveName";
            this.fileSaveName.Size = new System.Drawing.Size(157, 21);
            this.fileSaveName.TabIndex = 68;
            // 
            // SaveInterval
            // 
            this.SaveInterval.Location = new System.Drawing.Point(89, 17);
            this.SaveInterval.Margin = new System.Windows.Forms.Padding(2);
            this.SaveInterval.Name = "SaveInterval";
            this.SaveInterval.Size = new System.Drawing.Size(46, 21);
            this.SaveInterval.TabIndex = 72;
            this.SaveInterval.Text = "100";
            this.SaveInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label55.Location = new System.Drawing.Point(137, 20);
            this.label55.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(21, 14);
            this.label55.TabIndex = 74;
            this.label55.Text = "ms";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(11, 96);
            this.label42.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(65, 12);
            this.label42.TabIndex = 69;
            this.label42.Text = "保存路径：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(89, 93);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(324, 21);
            this.textBox1.TabIndex = 70;
            // 
            // bt_Cancel
            // 
            this.bt_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bt_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Cancel.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Cancel.Location = new System.Drawing.Point(230, 137);
            this.bt_Cancel.Margin = new System.Windows.Forms.Padding(2);
            this.bt_Cancel.MinimumSize = new System.Drawing.Size(1, 1);
            this.bt_Cancel.Name = "bt_Cancel";
            this.bt_Cancel.Size = new System.Drawing.Size(94, 31);
            this.bt_Cancel.Style = Sunny.UI.UIStyle.Custom;
            this.bt_Cancel.Symbol = 61453;
            this.bt_Cancel.SymbolSize = 30;
            this.bt_Cancel.TabIndex = 75;
            this.bt_Cancel.Text = "取消";
            this.bt_Cancel.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Cancel.Click += new System.EventHandler(this.bt_Cancel_Click);
            // 
            // SelectFiles
            // 
            this.SelectFiles.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SelectFiles.Items.AddRange(new object[] {
            "调试数据",
            "整车数据",
            "功率分析仪"});
            this.SelectFiles.Location = new System.Drawing.Point(431, 1);
            this.SelectFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SelectFiles.MinimumSize = new System.Drawing.Size(1, 1);
            this.SelectFiles.Name = "SelectFiles";
            this.SelectFiles.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.SelectFiles.SelectedIndexes = ((System.Collections.Generic.List<int>)(resources.GetObject("SelectFiles.SelectedIndexes")));
            this.SelectFiles.Size = new System.Drawing.Size(135, 167);
            this.SelectFiles.TabIndex = 76;
            this.SelectFiles.Text = "保存内容";
            this.SelectFiles.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExcelPathNameInit
            // 
            this.AcceptButton = this.bt_Confirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.CancelButton = this.bt_Cancel;
            this.ClientSize = new System.Drawing.Size(594, 180);
            this.Controls.Add(this.SelectFiles);
            this.Controls.Add(this.bt_Cancel);
            this.Controls.Add(this.bt_Confirm);
            this.Controls.Add(this.label43);
            this.Controls.Add(this.label44);
            this.Controls.Add(this.fileSaveName);
            this.Controls.Add(this.SaveInterval);
            this.Controls.Add(this.label55);
            this.Controls.Add(this.label42);
            this.Controls.Add(this.textBox1);
            this.Name = "ExcelPathNameInit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EXCEL保存初始化";
            this.Load += new System.EventHandler(this.ExcelPathNameInit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunny.UI.UISymbolButton bt_Confirm;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox fileSaveName;
        private System.Windows.Forms.TextBox SaveInterval;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox textBox1;
        private Sunny.UI.UISymbolButton bt_Cancel;
        private Sunny.UI.UICheckBoxGroup SelectFiles;
    }
}
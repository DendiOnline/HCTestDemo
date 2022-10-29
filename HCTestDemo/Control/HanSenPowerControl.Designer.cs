namespace HCTestDemo
{
    partial class HanSenPowerControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bt_PwrInit = new Sunny.UI.UISymbolButton();
            this.label94 = new System.Windows.Forms.Label();
            this.cbx_Local = new Sunny.UI.UIComboBox();
            this.cbxPramChange = new Sunny.UI.UISymbolButton();
            this.btPowerStart = new Sunny.UI.UISymbolButton();
            this.label93 = new System.Windows.Forms.Label();
            this.cbxModeSelect = new Sunny.UI.UIComboBox();
            this.label92 = new System.Windows.Forms.Label();
            this.tex_SET_P = new System.Windows.Forms.TextBox();
            this.label88 = new System.Windows.Forms.Label();
            this.tex_SET_I = new System.Windows.Forms.TextBox();
            this.label87 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.tex_SET_V = new System.Windows.Forms.TextBox();
            this.tex_Data_P = new System.Windows.Forms.TextBox();
            this.label55 = new System.Windows.Forms.Label();
            this.tex_Data_I = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.tex_Data_V = new System.Windows.Forms.TextBox();
            this.timer_recMsg = new System.Windows.Forms.Timer(this.components);
            this.pwrLed = new Sunny.UI.UILight();
            this.label1 = new System.Windows.Forms.Label();
            this.cbx_Bot = new Sunny.UI.UIComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_PwrInit
            // 
            this.bt_PwrInit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bt_PwrInit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.bt_PwrInit.FillHoverColor = System.Drawing.Color.Fuchsia;
            this.bt_PwrInit.FillPressColor = System.Drawing.Color.Fuchsia;
            this.bt_PwrInit.FillSelectedColor = System.Drawing.Color.Fuchsia;
            this.bt_PwrInit.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_PwrInit.Location = new System.Drawing.Point(14, 160);
            this.bt_PwrInit.Margin = new System.Windows.Forms.Padding(2);
            this.bt_PwrInit.MinimumSize = new System.Drawing.Size(1, 1);
            this.bt_PwrInit.Name = "bt_PwrInit";
            this.bt_PwrInit.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.bt_PwrInit.RectHoverColor = System.Drawing.Color.Fuchsia;
            this.bt_PwrInit.RectPressColor = System.Drawing.Color.Fuchsia;
            this.bt_PwrInit.RectSelectedColor = System.Drawing.Color.Fuchsia;
            this.bt_PwrInit.Size = new System.Drawing.Size(94, 31);
            this.bt_PwrInit.Style = Sunny.UI.UIStyle.Custom;
            this.bt_PwrInit.Symbol = 61475;
            this.bt_PwrInit.SymbolSize = 28;
            this.bt_PwrInit.TabIndex = 94;
            this.bt_PwrInit.Text = "连接";
            this.bt_PwrInit.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_PwrInit.Click += new System.EventHandler(this.bt_PwrInit_Click);
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.Location = new System.Drawing.Point(22, 62);
            this.label94.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(53, 12);
            this.label94.TabIndex = 93;
            this.label94.Text = "控制模式";
            // 
            // cbx_Local
            // 
            this.cbx_Local.DataSource = null;
            this.cbx_Local.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cbx_Local.FillColor = System.Drawing.Color.White;
            this.cbx_Local.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbx_Local.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cbx_Local.Items.AddRange(new object[] {
            "本地",
            "远程"});
            this.cbx_Local.Location = new System.Drawing.Point(14, 80);
            this.cbx_Local.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbx_Local.MinimumSize = new System.Drawing.Size(62, 0);
            this.cbx_Local.Name = "cbx_Local";
            this.cbx_Local.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbx_Local.Size = new System.Drawing.Size(94, 21);
            this.cbx_Local.Style = Sunny.UI.UIStyle.Custom;
            this.cbx_Local.TabIndex = 92;
            this.cbx_Local.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxPramChange
            // 
            this.cbxPramChange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbxPramChange.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.cbxPramChange.FillHoverColor = System.Drawing.Color.Fuchsia;
            this.cbxPramChange.FillPressColor = System.Drawing.Color.Fuchsia;
            this.cbxPramChange.FillSelectedColor = System.Drawing.Color.Fuchsia;
            this.cbxPramChange.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbxPramChange.Location = new System.Drawing.Point(20, 161);
            this.cbxPramChange.Margin = new System.Windows.Forms.Padding(2);
            this.cbxPramChange.MinimumSize = new System.Drawing.Size(1, 1);
            this.cbxPramChange.Name = "cbxPramChange";
            this.cbxPramChange.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.cbxPramChange.RectHoverColor = System.Drawing.Color.Fuchsia;
            this.cbxPramChange.RectPressColor = System.Drawing.Color.Fuchsia;
            this.cbxPramChange.RectSelectedColor = System.Drawing.Color.Fuchsia;
            this.cbxPramChange.Size = new System.Drawing.Size(94, 31);
            this.cbxPramChange.Style = Sunny.UI.UIStyle.Custom;
            this.cbxPramChange.Symbol = 61573;
            this.cbxPramChange.SymbolSize = 28;
            this.cbxPramChange.TabIndex = 91;
            this.cbxPramChange.Text = "参数修改";
            this.cbxPramChange.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbxPramChange.Click += new System.EventHandler(this.cbxPramChange_Click);
            // 
            // btPowerStart
            // 
            this.btPowerStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btPowerStart.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btPowerStart.FillHoverColor = System.Drawing.Color.Fuchsia;
            this.btPowerStart.FillPressColor = System.Drawing.Color.Fuchsia;
            this.btPowerStart.FillSelectedColor = System.Drawing.Color.Fuchsia;
            this.btPowerStart.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btPowerStart.Location = new System.Drawing.Point(21, 160);
            this.btPowerStart.Margin = new System.Windows.Forms.Padding(2);
            this.btPowerStart.MinimumSize = new System.Drawing.Size(1, 1);
            this.btPowerStart.Name = "btPowerStart";
            this.btPowerStart.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btPowerStart.RectHoverColor = System.Drawing.Color.Fuchsia;
            this.btPowerStart.RectPressColor = System.Drawing.Color.Fuchsia;
            this.btPowerStart.RectSelectedColor = System.Drawing.Color.Fuchsia;
            this.btPowerStart.Size = new System.Drawing.Size(94, 31);
            this.btPowerStart.Style = Sunny.UI.UIStyle.Custom;
            this.btPowerStart.Symbol = 61764;
            this.btPowerStart.SymbolSize = 28;
            this.btPowerStart.TabIndex = 90;
            this.btPowerStart.Text = "电源开机";
            this.btPowerStart.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btPowerStart.Click += new System.EventHandler(this.btPowerStart_Click);
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Location = new System.Drawing.Point(22, 18);
            this.label93.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(53, 12);
            this.label93.TabIndex = 89;
            this.label93.Text = "运行模式";
            // 
            // cbxModeSelect
            // 
            this.cbxModeSelect.DataSource = null;
            this.cbxModeSelect.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cbxModeSelect.FillColor = System.Drawing.Color.White;
            this.cbxModeSelect.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbxModeSelect.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cbxModeSelect.Items.AddRange(new object[] {
            "恒压模式",
            "恒流模式",
            "恒功率模式",
            "恒电阻模式"});
            this.cbxModeSelect.Location = new System.Drawing.Point(14, 36);
            this.cbxModeSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbxModeSelect.MinimumSize = new System.Drawing.Size(62, 0);
            this.cbxModeSelect.Name = "cbxModeSelect";
            this.cbxModeSelect.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbxModeSelect.Size = new System.Drawing.Size(94, 21);
            this.cbxModeSelect.Style = Sunny.UI.UIStyle.Custom;
            this.cbxModeSelect.TabIndex = 88;
            this.cbxModeSelect.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Location = new System.Drawing.Point(36, 105);
            this.label92.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(53, 12);
            this.label92.TabIndex = 87;
            this.label92.Text = "限制功率";
            // 
            // tex_SET_P
            // 
            this.tex_SET_P.Location = new System.Drawing.Point(20, 120);
            this.tex_SET_P.Margin = new System.Windows.Forms.Padding(2);
            this.tex_SET_P.Name = "tex_SET_P";
            this.tex_SET_P.Size = new System.Drawing.Size(76, 21);
            this.tex_SET_P.TabIndex = 86;
            this.tex_SET_P.Tag = "直流电压";
            this.tex_SET_P.Text = "10";
            this.tex_SET_P.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label88
            // 
            this.label88.AutoSize = true;
            this.label88.Location = new System.Drawing.Point(36, 65);
            this.label88.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(53, 12);
            this.label88.TabIndex = 85;
            this.label88.Text = "限制电流";
            // 
            // tex_SET_I
            // 
            this.tex_SET_I.Location = new System.Drawing.Point(20, 80);
            this.tex_SET_I.Margin = new System.Windows.Forms.Padding(2);
            this.tex_SET_I.Name = "tex_SET_I";
            this.tex_SET_I.Size = new System.Drawing.Size(76, 21);
            this.tex_SET_I.TabIndex = 84;
            this.tex_SET_I.Tag = "直流电压";
            this.tex_SET_I.Text = "20";
            this.tex_SET_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label87
            // 
            this.label87.AutoSize = true;
            this.label87.Location = new System.Drawing.Point(36, 21);
            this.label87.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(53, 12);
            this.label87.TabIndex = 83;
            this.label87.Text = "设定电压";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(38, 104);
            this.label56.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(53, 12);
            this.label56.TabIndex = 82;
            this.label56.Text = "直流功率";
            // 
            // tex_SET_V
            // 
            this.tex_SET_V.Location = new System.Drawing.Point(20, 36);
            this.tex_SET_V.Margin = new System.Windows.Forms.Padding(2);
            this.tex_SET_V.Name = "tex_SET_V";
            this.tex_SET_V.Size = new System.Drawing.Size(76, 21);
            this.tex_SET_V.TabIndex = 81;
            this.tex_SET_V.Tag = "直流电压";
            this.tex_SET_V.Text = "280";
            this.tex_SET_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tex_Data_P
            // 
            this.tex_Data_P.Location = new System.Drawing.Point(22, 119);
            this.tex_Data_P.Margin = new System.Windows.Forms.Padding(2);
            this.tex_Data_P.Name = "tex_Data_P";
            this.tex_Data_P.Size = new System.Drawing.Size(76, 21);
            this.tex_Data_P.TabIndex = 80;
            this.tex_Data_P.Tag = "直流电压";
            this.tex_Data_P.Text = "0";
            this.tex_Data_P.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(37, 64);
            this.label55.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(53, 12);
            this.label55.TabIndex = 79;
            this.label55.Text = "直流电流";
            // 
            // tex_Data_I
            // 
            this.tex_Data_I.Location = new System.Drawing.Point(21, 79);
            this.tex_Data_I.Margin = new System.Windows.Forms.Padding(2);
            this.tex_Data_I.Name = "tex_Data_I";
            this.tex_Data_I.Size = new System.Drawing.Size(76, 21);
            this.tex_Data_I.TabIndex = 78;
            this.tex_Data_I.Tag = "直流电压";
            this.tex_Data_I.Text = "0";
            this.tex_Data_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(37, 20);
            this.label44.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(53, 12);
            this.label44.TabIndex = 77;
            this.label44.Text = "直流电压";
            // 
            // tex_Data_V
            // 
            this.tex_Data_V.Location = new System.Drawing.Point(21, 35);
            this.tex_Data_V.Margin = new System.Windows.Forms.Padding(2);
            this.tex_Data_V.Name = "tex_Data_V";
            this.tex_Data_V.Size = new System.Drawing.Size(76, 21);
            this.tex_Data_V.TabIndex = 76;
            this.tex_Data_V.Tag = "直流电压";
            this.tex_Data_V.Text = "0";
            this.tex_Data_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // timer_recMsg
            // 
            this.timer_recMsg.Interval = 10;
            this.timer_recMsg.Tick += new System.EventHandler(this.timer_recMsg_Tick);
            // 
            // pwrLed
            // 
            this.pwrLed.CenterColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(248)))), ((int)(((byte)(232)))));
            this.pwrLed.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.pwrLed.Location = new System.Drawing.Point(112, 159);
            this.pwrLed.Margin = new System.Windows.Forms.Padding(2);
            this.pwrLed.MinimumSize = new System.Drawing.Size(1, 1);
            this.pwrLed.Name = "pwrLed";
            this.pwrLed.OffColor = System.Drawing.Color.Transparent;
            this.pwrLed.OnCenterColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(248)))), ((int)(((byte)(232)))));
            this.pwrLed.OnColor = System.Drawing.Color.YellowGreen;
            this.pwrLed.Radius = 32;
            this.pwrLed.Size = new System.Drawing.Size(32, 32);
            this.pwrLed.State = Sunny.UI.UILightState.Off;
            this.pwrLed.Style = Sunny.UI.UIStyle.Custom;
            this.pwrLed.TabIndex = 95;
            this.pwrLed.Text = "pwr_ConditionLed";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 102);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 97;
            this.label1.Text = "波特率";
            // 
            // cbx_Bot
            // 
            this.cbx_Bot.DataSource = null;
            this.cbx_Bot.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cbx_Bot.FillColor = System.Drawing.Color.White;
            this.cbx_Bot.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbx_Bot.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cbx_Bot.Items.AddRange(new object[] {
            "125Kbps",
            "250Kbps",
            "500Kbps"});
            this.cbx_Bot.Location = new System.Drawing.Point(14, 120);
            this.cbx_Bot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbx_Bot.MinimumSize = new System.Drawing.Size(62, 0);
            this.cbx_Bot.Name = "cbx_Bot";
            this.cbx_Bot.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbx_Bot.Size = new System.Drawing.Size(94, 21);
            this.cbx_Bot.Style = Sunny.UI.UIStyle.Custom;
            this.cbx_Bot.TabIndex = 96;
            this.cbx_Bot.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxPramChange);
            this.groupBox1.Controls.Add(this.tex_SET_V);
            this.groupBox1.Controls.Add(this.label87);
            this.groupBox1.Controls.Add(this.tex_SET_I);
            this.groupBox1.Controls.Add(this.label88);
            this.groupBox1.Controls.Add(this.tex_SET_P);
            this.groupBox1.Controls.Add(this.label92);
            this.groupBox1.Location = new System.Drawing.Point(153, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(134, 210);
            this.groupBox1.TabIndex = 98;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "电参数设置";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btPowerStart);
            this.groupBox2.Controls.Add(this.tex_Data_V);
            this.groupBox2.Controls.Add(this.label44);
            this.groupBox2.Controls.Add(this.tex_Data_I);
            this.groupBox2.Controls.Add(this.label55);
            this.groupBox2.Controls.Add(this.tex_Data_P);
            this.groupBox2.Controls.Add(this.label56);
            this.groupBox2.Location = new System.Drawing.Point(13, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(134, 210);
            this.groupBox2.TabIndex = 99;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "实时信息";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.bt_PwrInit);
            this.groupBox3.Controls.Add(this.cbxModeSelect);
            this.groupBox3.Controls.Add(this.label93);
            this.groupBox3.Controls.Add(this.pwrLed);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cbx_Local);
            this.groupBox3.Controls.Add(this.cbx_Bot);
            this.groupBox3.Controls.Add(this.label94);
            this.groupBox3.Location = new System.Drawing.Point(293, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(162, 210);
            this.groupBox3.TabIndex = 100;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "电源操作";
            // 
            // HanSenPowerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "HanSenPowerControl";
            this.Size = new System.Drawing.Size(458, 253);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UISymbolButton bt_PwrInit;
        private System.Windows.Forms.Label label94;
        private Sunny.UI.UIComboBox cbx_Local;
        private Sunny.UI.UISymbolButton cbxPramChange;
        private Sunny.UI.UISymbolButton btPowerStart;
        private System.Windows.Forms.Label label93;
        private Sunny.UI.UIComboBox cbxModeSelect;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.TextBox tex_SET_P;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.TextBox tex_SET_I;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.TextBox tex_SET_V;
        private System.Windows.Forms.TextBox tex_Data_P;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.TextBox tex_Data_I;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox tex_Data_V;
        private System.Windows.Forms.Timer timer_recMsg;
        private Sunny.UI.UILight pwrLed;
        private System.Windows.Forms.Label label1;
        private Sunny.UI.UIComboBox cbx_Bot;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}

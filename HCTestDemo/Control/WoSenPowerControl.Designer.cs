namespace HCTestDemo
{
    partial class WoSenPowerControl
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
            this.bt_OpenSerial = new Sunny.UI.UISymbolButton();
            this.bt_ParaChange = new Sunny.UI.UISymbolButton();
            this.bt_PowerOn = new Sunny.UI.UISymbolButton();
            this.label100 = new System.Windows.Forms.Label();
            this.set_P = new System.Windows.Forms.TextBox();
            this.label101 = new System.Windows.Forms.Label();
            this.set_I = new System.Windows.Forms.TextBox();
            this.label102 = new System.Windows.Forms.Label();
            this.label103 = new System.Windows.Forms.Label();
            this.set_V = new System.Windows.Forms.TextBox();
            this.dc_P = new System.Windows.Forms.TextBox();
            this.label104 = new System.Windows.Forms.Label();
            this.dc_I = new System.Windows.Forms.TextBox();
            this.label105 = new System.Windows.Forms.Label();
            this.dc_V = new System.Windows.Forms.TextBox();
            this.Led_Device = new Sunny.UI.UILight();
            this.cb_StopBitSel = new Sunny.UI.UIComboBox();
            this.label95 = new System.Windows.Forms.Label();
            this.cb_CheckBitSel = new Sunny.UI.UIComboBox();
            this.label96 = new System.Windows.Forms.Label();
            this.cb_DataBitSel = new Sunny.UI.UIComboBox();
            this.label97 = new System.Windows.Forms.Label();
            this.label98 = new System.Windows.Forms.Label();
            this.cb_BaudrateSel = new Sunny.UI.UIComboBox();
            this.label99 = new System.Windows.Forms.Label();
            this.cb_SerialNum = new Sunny.UI.UIComboBox();
            this.sp_Com = new System.IO.Ports.SerialPort(this.components);
            this.timer_sendReq = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_OpenSerial
            // 
            this.bt_OpenSerial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bt_OpenSerial.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.bt_OpenSerial.FillHoverColor = System.Drawing.Color.Fuchsia;
            this.bt_OpenSerial.FillPressColor = System.Drawing.Color.Fuchsia;
            this.bt_OpenSerial.FillSelectedColor = System.Drawing.Color.Fuchsia;
            this.bt_OpenSerial.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_OpenSerial.Location = new System.Drawing.Point(5, 169);
            this.bt_OpenSerial.Margin = new System.Windows.Forms.Padding(2);
            this.bt_OpenSerial.MinimumSize = new System.Drawing.Size(1, 1);
            this.bt_OpenSerial.Name = "bt_OpenSerial";
            this.bt_OpenSerial.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.bt_OpenSerial.RectHoverColor = System.Drawing.Color.Fuchsia;
            this.bt_OpenSerial.RectPressColor = System.Drawing.Color.Fuchsia;
            this.bt_OpenSerial.RectSelectedColor = System.Drawing.Color.Fuchsia;
            this.bt_OpenSerial.Size = new System.Drawing.Size(94, 31);
            this.bt_OpenSerial.Style = Sunny.UI.UIStyle.Custom;
            this.bt_OpenSerial.Symbol = 61475;
            this.bt_OpenSerial.SymbolSize = 28;
            this.bt_OpenSerial.TabIndex = 152;
            this.bt_OpenSerial.Text = "打开串口";
            this.bt_OpenSerial.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_OpenSerial.Click += new System.EventHandler(this.bt_OpenSerial_Click);
            // 
            // bt_ParaChange
            // 
            this.bt_ParaChange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bt_ParaChange.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.bt_ParaChange.FillHoverColor = System.Drawing.Color.Fuchsia;
            this.bt_ParaChange.FillPressColor = System.Drawing.Color.Fuchsia;
            this.bt_ParaChange.FillSelectedColor = System.Drawing.Color.Fuchsia;
            this.bt_ParaChange.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_ParaChange.Location = new System.Drawing.Point(5, 169);
            this.bt_ParaChange.Margin = new System.Windows.Forms.Padding(2);
            this.bt_ParaChange.MinimumSize = new System.Drawing.Size(1, 1);
            this.bt_ParaChange.Name = "bt_ParaChange";
            this.bt_ParaChange.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.bt_ParaChange.RectHoverColor = System.Drawing.Color.Fuchsia;
            this.bt_ParaChange.RectPressColor = System.Drawing.Color.Fuchsia;
            this.bt_ParaChange.RectSelectedColor = System.Drawing.Color.Fuchsia;
            this.bt_ParaChange.Size = new System.Drawing.Size(94, 31);
            this.bt_ParaChange.Style = Sunny.UI.UIStyle.Custom;
            this.bt_ParaChange.Symbol = 61573;
            this.bt_ParaChange.SymbolSize = 28;
            this.bt_ParaChange.TabIndex = 151;
            this.bt_ParaChange.Text = "参数修改";
            this.bt_ParaChange.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_ParaChange.Click += new System.EventHandler(this.bt_ParaChange_Click);
            // 
            // bt_PowerOn
            // 
            this.bt_PowerOn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bt_PowerOn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.bt_PowerOn.FillHoverColor = System.Drawing.Color.Fuchsia;
            this.bt_PowerOn.FillPressColor = System.Drawing.Color.Fuchsia;
            this.bt_PowerOn.FillSelectedColor = System.Drawing.Color.Fuchsia;
            this.bt_PowerOn.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_PowerOn.Location = new System.Drawing.Point(13, 169);
            this.bt_PowerOn.Margin = new System.Windows.Forms.Padding(2);
            this.bt_PowerOn.MinimumSize = new System.Drawing.Size(1, 1);
            this.bt_PowerOn.Name = "bt_PowerOn";
            this.bt_PowerOn.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.bt_PowerOn.RectHoverColor = System.Drawing.Color.Fuchsia;
            this.bt_PowerOn.RectPressColor = System.Drawing.Color.Fuchsia;
            this.bt_PowerOn.RectSelectedColor = System.Drawing.Color.Fuchsia;
            this.bt_PowerOn.Size = new System.Drawing.Size(94, 31);
            this.bt_PowerOn.Style = Sunny.UI.UIStyle.Custom;
            this.bt_PowerOn.Symbol = 61764;
            this.bt_PowerOn.SymbolSize = 28;
            this.bt_PowerOn.TabIndex = 150;
            this.bt_PowerOn.Text = "电源开机";
            this.bt_PowerOn.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_PowerOn.Click += new System.EventHandler(this.bt_PowerOn_Click);
            // 
            // label100
            // 
            this.label100.AutoSize = true;
            this.label100.Location = new System.Drawing.Point(21, 102);
            this.label100.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label100.Name = "label100";
            this.label100.Size = new System.Drawing.Size(53, 12);
            this.label100.TabIndex = 149;
            this.label100.Text = "限制功率";
            // 
            // set_P
            // 
            this.set_P.Location = new System.Drawing.Point(5, 117);
            this.set_P.Margin = new System.Windows.Forms.Padding(2);
            this.set_P.Name = "set_P";
            this.set_P.Size = new System.Drawing.Size(76, 21);
            this.set_P.TabIndex = 148;
            this.set_P.Tag = "直流电压";
            this.set_P.Text = "5";
            this.set_P.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label101
            // 
            this.label101.AutoSize = true;
            this.label101.Location = new System.Drawing.Point(21, 62);
            this.label101.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label101.Name = "label101";
            this.label101.Size = new System.Drawing.Size(53, 12);
            this.label101.TabIndex = 147;
            this.label101.Text = "限制电流";
            // 
            // set_I
            // 
            this.set_I.Location = new System.Drawing.Point(5, 77);
            this.set_I.Margin = new System.Windows.Forms.Padding(2);
            this.set_I.Name = "set_I";
            this.set_I.Size = new System.Drawing.Size(76, 21);
            this.set_I.TabIndex = 146;
            this.set_I.Tag = "直流电压";
            this.set_I.Text = "10";
            this.set_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label102
            // 
            this.label102.AutoSize = true;
            this.label102.Location = new System.Drawing.Point(21, 18);
            this.label102.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label102.Name = "label102";
            this.label102.Size = new System.Drawing.Size(53, 12);
            this.label102.TabIndex = 144;
            this.label102.Text = "设定电压";
            // 
            // label103
            // 
            this.label103.AutoSize = true;
            this.label103.Location = new System.Drawing.Point(30, 104);
            this.label103.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label103.Name = "label103";
            this.label103.Size = new System.Drawing.Size(53, 12);
            this.label103.TabIndex = 145;
            this.label103.Text = "直流功率";
            // 
            // set_V
            // 
            this.set_V.Location = new System.Drawing.Point(5, 33);
            this.set_V.Margin = new System.Windows.Forms.Padding(2);
            this.set_V.Name = "set_V";
            this.set_V.Size = new System.Drawing.Size(76, 21);
            this.set_V.TabIndex = 142;
            this.set_V.Tag = "直流电压";
            this.set_V.Text = "280";
            this.set_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dc_P
            // 
            this.dc_P.Location = new System.Drawing.Point(14, 119);
            this.dc_P.Margin = new System.Windows.Forms.Padding(2);
            this.dc_P.Name = "dc_P";
            this.dc_P.Size = new System.Drawing.Size(76, 21);
            this.dc_P.TabIndex = 143;
            this.dc_P.Tag = "直流电压";
            this.dc_P.Text = "0";
            this.dc_P.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label104
            // 
            this.label104.AutoSize = true;
            this.label104.Location = new System.Drawing.Point(29, 64);
            this.label104.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label104.Name = "label104";
            this.label104.Size = new System.Drawing.Size(53, 12);
            this.label104.TabIndex = 141;
            this.label104.Text = "直流电流";
            // 
            // dc_I
            // 
            this.dc_I.Location = new System.Drawing.Point(13, 79);
            this.dc_I.Margin = new System.Windows.Forms.Padding(2);
            this.dc_I.Name = "dc_I";
            this.dc_I.Size = new System.Drawing.Size(76, 21);
            this.dc_I.TabIndex = 140;
            this.dc_I.Tag = "直流电压";
            this.dc_I.Text = "0";
            this.dc_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label105
            // 
            this.label105.AutoSize = true;
            this.label105.Location = new System.Drawing.Point(25, 20);
            this.label105.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label105.Name = "label105";
            this.label105.Size = new System.Drawing.Size(53, 12);
            this.label105.TabIndex = 139;
            this.label105.Text = "直流电压";
            // 
            // dc_V
            // 
            this.dc_V.Location = new System.Drawing.Point(14, 34);
            this.dc_V.Margin = new System.Windows.Forms.Padding(2);
            this.dc_V.Name = "dc_V";
            this.dc_V.Size = new System.Drawing.Size(76, 21);
            this.dc_V.TabIndex = 138;
            this.dc_V.Tag = "直流电压";
            this.dc_V.Text = "0";
            this.dc_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Led_Device
            // 
            this.Led_Device.CenterColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(248)))), ((int)(((byte)(232)))));
            this.Led_Device.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Led_Device.Location = new System.Drawing.Point(127, 170);
            this.Led_Device.Margin = new System.Windows.Forms.Padding(2);
            this.Led_Device.MinimumSize = new System.Drawing.Size(1, 1);
            this.Led_Device.Name = "Led_Device";
            this.Led_Device.OffColor = System.Drawing.Color.Transparent;
            this.Led_Device.OnCenterColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(248)))), ((int)(((byte)(232)))));
            this.Led_Device.OnColor = System.Drawing.Color.YellowGreen;
            this.Led_Device.Radius = 32;
            this.Led_Device.Size = new System.Drawing.Size(32, 32);
            this.Led_Device.State = Sunny.UI.UILightState.Off;
            this.Led_Device.Style = Sunny.UI.UIStyle.Custom;
            this.Led_Device.TabIndex = 137;
            this.Led_Device.Text = "uiLight1";
            // 
            // cb_StopBitSel
            // 
            this.cb_StopBitSel.DataSource = null;
            this.cb_StopBitSel.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cb_StopBitSel.FillColor = System.Drawing.Color.White;
            this.cb_StopBitSel.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_StopBitSel.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cb_StopBitSel.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.cb_StopBitSel.Location = new System.Drawing.Point(65, 140);
            this.cb_StopBitSel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_StopBitSel.MinimumSize = new System.Drawing.Size(63, 0);
            this.cb_StopBitSel.Name = "cb_StopBitSel";
            this.cb_StopBitSel.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cb_StopBitSel.Size = new System.Drawing.Size(94, 21);
            this.cb_StopBitSel.TabIndex = 131;
            this.cb_StopBitSel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label95
            // 
            this.label95.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label95.AutoSize = true;
            this.label95.Location = new System.Drawing.Point(5, 142);
            this.label95.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(41, 12);
            this.label95.TabIndex = 136;
            this.label95.Text = "停止位";
            // 
            // cb_CheckBitSel
            // 
            this.cb_CheckBitSel.DataSource = null;
            this.cb_CheckBitSel.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cb_CheckBitSel.FillColor = System.Drawing.Color.White;
            this.cb_CheckBitSel.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_CheckBitSel.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cb_CheckBitSel.Items.AddRange(new object[] {
            "无",
            "奇校验",
            "偶校验",
            "Mark",
            "空格校验"});
            this.cb_CheckBitSel.Location = new System.Drawing.Point(65, 109);
            this.cb_CheckBitSel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_CheckBitSel.MinimumSize = new System.Drawing.Size(63, 0);
            this.cb_CheckBitSel.Name = "cb_CheckBitSel";
            this.cb_CheckBitSel.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cb_CheckBitSel.Size = new System.Drawing.Size(94, 21);
            this.cb_CheckBitSel.TabIndex = 130;
            this.cb_CheckBitSel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label96
            // 
            this.label96.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label96.AutoSize = true;
            this.label96.Location = new System.Drawing.Point(5, 111);
            this.label96.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(41, 12);
            this.label96.TabIndex = 135;
            this.label96.Text = "校验位";
            // 
            // cb_DataBitSel
            // 
            this.cb_DataBitSel.DataSource = null;
            this.cb_DataBitSel.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cb_DataBitSel.FillColor = System.Drawing.Color.White;
            this.cb_DataBitSel.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_DataBitSel.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cb_DataBitSel.Items.AddRange(new object[] {
            "6",
            "7",
            "8"});
            this.cb_DataBitSel.Location = new System.Drawing.Point(65, 78);
            this.cb_DataBitSel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_DataBitSel.MinimumSize = new System.Drawing.Size(63, 0);
            this.cb_DataBitSel.Name = "cb_DataBitSel";
            this.cb_DataBitSel.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cb_DataBitSel.Size = new System.Drawing.Size(94, 21);
            this.cb_DataBitSel.TabIndex = 129;
            this.cb_DataBitSel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label97
            // 
            this.label97.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label97.AutoSize = true;
            this.label97.Location = new System.Drawing.Point(5, 80);
            this.label97.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label97.Name = "label97";
            this.label97.Size = new System.Drawing.Size(41, 12);
            this.label97.TabIndex = 134;
            this.label97.Text = "数据位";
            // 
            // label98
            // 
            this.label98.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label98.AutoSize = true;
            this.label98.Location = new System.Drawing.Point(5, 49);
            this.label98.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label98.Name = "label98";
            this.label98.Size = new System.Drawing.Size(41, 12);
            this.label98.TabIndex = 133;
            this.label98.Text = "波特率";
            // 
            // cb_BaudrateSel
            // 
            this.cb_BaudrateSel.DataSource = null;
            this.cb_BaudrateSel.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cb_BaudrateSel.FillColor = System.Drawing.Color.White;
            this.cb_BaudrateSel.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_BaudrateSel.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cb_BaudrateSel.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "56000",
            "57600",
            "115200"});
            this.cb_BaudrateSel.Location = new System.Drawing.Point(65, 46);
            this.cb_BaudrateSel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_BaudrateSel.MinimumSize = new System.Drawing.Size(63, 0);
            this.cb_BaudrateSel.Name = "cb_BaudrateSel";
            this.cb_BaudrateSel.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cb_BaudrateSel.Size = new System.Drawing.Size(94, 21);
            this.cb_BaudrateSel.TabIndex = 128;
            this.cb_BaudrateSel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label99
            // 
            this.label99.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label99.AutoSize = true;
            this.label99.Location = new System.Drawing.Point(5, 18);
            this.label99.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label99.Name = "label99";
            this.label99.Size = new System.Drawing.Size(29, 12);
            this.label99.TabIndex = 132;
            this.label99.Text = "端口";
            // 
            // cb_SerialNum
            // 
            this.cb_SerialNum.DataSource = null;
            this.cb_SerialNum.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cb_SerialNum.FillColor = System.Drawing.Color.White;
            this.cb_SerialNum.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_SerialNum.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cb_SerialNum.Location = new System.Drawing.Point(65, 15);
            this.cb_SerialNum.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_SerialNum.MinimumSize = new System.Drawing.Size(63, 0);
            this.cb_SerialNum.Name = "cb_SerialNum";
            this.cb_SerialNum.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cb_SerialNum.Size = new System.Drawing.Size(94, 21);
            this.cb_SerialNum.TabIndex = 127;
            this.cb_SerialNum.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sp_Com
            // 
            this.sp_Com.ReadTimeout = 20;
            this.sp_Com.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.sp_Com_DataReceived);
            // 
            // timer_sendReq
            // 
            this.timer_sendReq.Tick += new System.EventHandler(this.timer_sendReq_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bt_PowerOn);
            this.groupBox1.Controls.Add(this.dc_V);
            this.groupBox1.Controls.Add(this.label105);
            this.groupBox1.Controls.Add(this.dc_I);
            this.groupBox1.Controls.Add(this.label104);
            this.groupBox1.Controls.Add(this.dc_P);
            this.groupBox1.Controls.Add(this.label103);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(119, 210);
            this.groupBox1.TabIndex = 153;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "实时数据";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bt_ParaChange);
            this.groupBox2.Controls.Add(this.set_V);
            this.groupBox2.Controls.Add(this.label102);
            this.groupBox2.Controls.Add(this.set_I);
            this.groupBox2.Controls.Add(this.label100);
            this.groupBox2.Controls.Add(this.label101);
            this.groupBox2.Controls.Add(this.set_P);
            this.groupBox2.Location = new System.Drawing.Point(128, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(116, 210);
            this.groupBox2.TabIndex = 154;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "电参数设定";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label98);
            this.groupBox3.Controls.Add(this.cb_SerialNum);
            this.groupBox3.Controls.Add(this.bt_OpenSerial);
            this.groupBox3.Controls.Add(this.label99);
            this.groupBox3.Controls.Add(this.Led_Device);
            this.groupBox3.Controls.Add(this.cb_BaudrateSel);
            this.groupBox3.Controls.Add(this.cb_StopBitSel);
            this.groupBox3.Controls.Add(this.label97);
            this.groupBox3.Controls.Add(this.label95);
            this.groupBox3.Controls.Add(this.cb_DataBitSel);
            this.groupBox3.Controls.Add(this.cb_CheckBitSel);
            this.groupBox3.Controls.Add(this.label96);
            this.groupBox3.Location = new System.Drawing.Point(250, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(174, 210);
            this.groupBox3.TabIndex = 155;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "电源操作";
            // 
            // WoSenPowerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "WoSenPowerControl";
            this.Size = new System.Drawing.Size(458, 253);
            this.Load += new System.EventHandler(this.WoSenPowerControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UISymbolButton bt_OpenSerial;
        private Sunny.UI.UISymbolButton bt_ParaChange;
        private Sunny.UI.UISymbolButton bt_PowerOn;
        private System.Windows.Forms.Label label100;
        private System.Windows.Forms.TextBox set_P;
        private System.Windows.Forms.Label label101;
        private System.Windows.Forms.TextBox set_I;
        private System.Windows.Forms.Label label102;
        private System.Windows.Forms.Label label103;
        private System.Windows.Forms.TextBox set_V;
        private System.Windows.Forms.TextBox dc_P;
        private System.Windows.Forms.Label label104;
        private System.Windows.Forms.TextBox dc_I;
        private System.Windows.Forms.Label label105;
        private System.Windows.Forms.TextBox dc_V;
        private Sunny.UI.UILight Led_Device;
        private Sunny.UI.UIComboBox cb_StopBitSel;
        private System.Windows.Forms.Label label95;
        private Sunny.UI.UIComboBox cb_CheckBitSel;
        private System.Windows.Forms.Label label96;
        private Sunny.UI.UIComboBox cb_DataBitSel;
        private System.Windows.Forms.Label label97;
        private System.Windows.Forms.Label label98;
        private Sunny.UI.UIComboBox cb_BaudrateSel;
        private System.Windows.Forms.Label label99;
        private Sunny.UI.UIComboBox cb_SerialNum;
        private System.IO.Ports.SerialPort sp_Com;
        private System.Windows.Forms.Timer timer_sendReq;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}

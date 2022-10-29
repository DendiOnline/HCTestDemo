namespace HCTestDemo
{
    partial class HC_800w
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HC_800w));
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.LedSend = new Sunny.UI.UILight();
            this.LedRec = new Sunny.UI.UILight();
            this.Led_Device = new Sunny.UI.UILight();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_Rec = new System.Windows.Forms.Label();
            this.cb_StopBitSel = new Sunny.UI.UIComboBox();
            this.cb_CheckBitSel = new Sunny.UI.UIComboBox();
            this.cb_DataBitSel = new Sunny.UI.UIComboBox();
            this.cb_BaudrateSel = new Sunny.UI.UIComboBox();
            this.cb_SerialNum = new Sunny.UI.UIComboBox();
            this.bt_OpenSerial = new Sunny.UI.UISymbolButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.desRecovery = new Sunny.UI.UISymbolButton();
            this.label20 = new System.Windows.Forms.Label();
            this.controlMode = new Sunny.UI.UIComboBox();
            this.loopsend = new System.Windows.Forms.CheckBox();
            this.EmergencyStop = new Sunny.UI.UISymbolButton();
            this.label12 = new System.Windows.Forms.Label();
            this.spdIncre = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.SetSpdSend = new System.Windows.Forms.NumericUpDown();
            this.timer_send = new System.Windows.Forms.Timer(this.components);
            this.sp_Com = new System.IO.Ports.SerialPort(this.components);
            this.label35 = new System.Windows.Forms.Label();
            this.WarningDescribe = new System.Windows.Forms.TextBox();
            this.gbxControlMsg = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.ANGLE = new System.Windows.Forms.TextBox();
            this.FAULT_WORD5 = new System.Windows.Forms.Label();
            this.FAULT_WORD4 = new System.Windows.Forms.Label();
            this.FAULT_WORD3 = new System.Windows.Forms.Label();
            this.FAULT_WORD2 = new System.Windows.Forms.Label();
            this.FAULT_WORD1 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.COMPILE_DATE = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.HW_VER = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.FAULT_LEVEL = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.PWM_STS = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SYS_STS = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.CONTROL_POWER_VOLT = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.motorTemp = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.motorSpd = new System.Windows.Forms.TextBox();
            this.lbl_Spd = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.SW_Ver = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.MotorTorqueFB = new System.Windows.Forms.TextBox();
            this.label56 = new System.Windows.Forms.Label();
            this.Phase_Curr_RMS = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.ControlTemp = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.IDC = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.UDC = new System.Windows.Forms.TextBox();
            this.spdMeter = new Sunny.UI.UIAnalogMeter();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SetSpdSend)).BeginInit();
            this.gbxControlMsg.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 234);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 18);
            this.label5.TabIndex = 96;
            this.label5.Text = "停止位";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 18);
            this.label4.TabIndex = 94;
            this.label4.Text = "校验位";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 18);
            this.label3.TabIndex = 92;
            this.label3.Text = "数据位";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 90;
            this.label2.Text = "波特率";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 89;
            this.label1.Text = "端口";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.LedSend);
            this.groupBox5.Controls.Add(this.LedRec);
            this.groupBox5.Controls.Add(this.Led_Device);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.lbl_Rec);
            this.groupBox5.Controls.Add(this.cb_StopBitSel);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.cb_CheckBitSel);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.cb_DataBitSel);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.cb_BaudrateSel);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.cb_SerialNum);
            this.groupBox5.Controls.Add(this.bt_OpenSerial);
            this.groupBox5.Location = new System.Drawing.Point(4, 4);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(242, 416);
            this.groupBox5.TabIndex = 98;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "通讯设置";
            // 
            // LedSend
            // 
            this.LedSend.CenterColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(248)))), ((int)(((byte)(232)))));
            this.LedSend.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.LedSend.Location = new System.Drawing.Point(170, 350);
            this.LedSend.MinimumSize = new System.Drawing.Size(2, 2);
            this.LedSend.Name = "LedSend";
            this.LedSend.OffColor = System.Drawing.Color.Transparent;
            this.LedSend.OnCenterColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(248)))), ((int)(((byte)(232)))));
            this.LedSend.OnColor = System.Drawing.Color.YellowGreen;
            this.LedSend.Radius = 48;
            this.LedSend.Size = new System.Drawing.Size(48, 48);
            this.LedSend.Style = Sunny.UI.UIStyle.Custom;
            this.LedSend.TabIndex = 101;
            this.LedSend.Text = "uiLight2";
            // 
            // LedRec
            // 
            this.LedRec.CenterColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(248)))), ((int)(((byte)(232)))));
            this.LedRec.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.LedRec.Location = new System.Drawing.Point(57, 350);
            this.LedRec.MinimumSize = new System.Drawing.Size(2, 2);
            this.LedRec.Name = "LedRec";
            this.LedRec.OffColor = System.Drawing.Color.Transparent;
            this.LedRec.OnCenterColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(248)))), ((int)(((byte)(232)))));
            this.LedRec.OnColor = System.Drawing.Color.YellowGreen;
            this.LedRec.Radius = 48;
            this.LedRec.Size = new System.Drawing.Size(48, 48);
            this.LedRec.Style = Sunny.UI.UIStyle.Custom;
            this.LedRec.TabIndex = 100;
            this.LedRec.Text = "uiLight1";
            // 
            // Led_Device
            // 
            this.Led_Device.CenterColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(248)))), ((int)(((byte)(232)))));
            this.Led_Device.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Led_Device.Location = new System.Drawing.Point(170, 266);
            this.Led_Device.MinimumSize = new System.Drawing.Size(2, 2);
            this.Led_Device.Name = "Led_Device";
            this.Led_Device.OffColor = System.Drawing.Color.Transparent;
            this.Led_Device.OnCenterColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(248)))), ((int)(((byte)(232)))));
            this.Led_Device.OnColor = System.Drawing.Color.YellowGreen;
            this.Led_Device.Radius = 48;
            this.Led_Device.Size = new System.Drawing.Size(48, 48);
            this.Led_Device.State = Sunny.UI.UILightState.Off;
            this.Led_Device.Style = Sunny.UI.UIStyle.Custom;
            this.Led_Device.TabIndex = 99;
            this.Led_Device.Text = "uiLight1";
            this.Led_Device.Click += new System.EventHandler(this.Led_Device_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(120, 363);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 18);
            this.label7.TabIndex = 98;
            this.label7.Text = "发送";
            // 
            // lbl_Rec
            // 
            this.lbl_Rec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Rec.AutoSize = true;
            this.lbl_Rec.Location = new System.Drawing.Point(8, 363);
            this.lbl_Rec.Name = "lbl_Rec";
            this.lbl_Rec.Size = new System.Drawing.Size(44, 18);
            this.lbl_Rec.TabIndex = 97;
            this.lbl_Rec.Text = "接收";
            this.lbl_Rec.DoubleClick += new System.EventHandler(this.lbl_Rec_DoubleClick);
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
            this.cb_StopBitSel.Location = new System.Drawing.Point(90, 220);
            this.cb_StopBitSel.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.cb_StopBitSel.MinimumSize = new System.Drawing.Size(94, 0);
            this.cb_StopBitSel.Name = "cb_StopBitSel";
            this.cb_StopBitSel.Padding = new System.Windows.Forms.Padding(0, 0, 45, 3);
            this.cb_StopBitSel.Size = new System.Drawing.Size(141, 32);
            this.cb_StopBitSel.TabIndex = 36;
            this.cb_StopBitSel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.cb_CheckBitSel.Location = new System.Drawing.Point(90, 174);
            this.cb_CheckBitSel.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.cb_CheckBitSel.MinimumSize = new System.Drawing.Size(94, 0);
            this.cb_CheckBitSel.Name = "cb_CheckBitSel";
            this.cb_CheckBitSel.Padding = new System.Windows.Forms.Padding(0, 0, 45, 3);
            this.cb_CheckBitSel.Size = new System.Drawing.Size(141, 32);
            this.cb_CheckBitSel.TabIndex = 35;
            this.cb_CheckBitSel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.cb_DataBitSel.Location = new System.Drawing.Point(90, 128);
            this.cb_DataBitSel.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.cb_DataBitSel.MinimumSize = new System.Drawing.Size(94, 0);
            this.cb_DataBitSel.Name = "cb_DataBitSel";
            this.cb_DataBitSel.Padding = new System.Windows.Forms.Padding(0, 0, 45, 3);
            this.cb_DataBitSel.Size = new System.Drawing.Size(141, 32);
            this.cb_DataBitSel.TabIndex = 34;
            this.cb_DataBitSel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.cb_BaudrateSel.Location = new System.Drawing.Point(90, 80);
            this.cb_BaudrateSel.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.cb_BaudrateSel.MinimumSize = new System.Drawing.Size(94, 0);
            this.cb_BaudrateSel.Name = "cb_BaudrateSel";
            this.cb_BaudrateSel.Padding = new System.Windows.Forms.Padding(0, 0, 45, 3);
            this.cb_BaudrateSel.Size = new System.Drawing.Size(141, 32);
            this.cb_BaudrateSel.TabIndex = 33;
            this.cb_BaudrateSel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cb_SerialNum
            // 
            this.cb_SerialNum.DataSource = null;
            this.cb_SerialNum.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cb_SerialNum.FillColor = System.Drawing.Color.White;
            this.cb_SerialNum.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_SerialNum.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cb_SerialNum.Location = new System.Drawing.Point(90, 33);
            this.cb_SerialNum.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.cb_SerialNum.MinimumSize = new System.Drawing.Size(94, 0);
            this.cb_SerialNum.Name = "cb_SerialNum";
            this.cb_SerialNum.Padding = new System.Windows.Forms.Padding(0, 0, 45, 3);
            this.cb_SerialNum.Size = new System.Drawing.Size(141, 32);
            this.cb_SerialNum.TabIndex = 32;
            this.cb_SerialNum.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bt_OpenSerial
            // 
            this.bt_OpenSerial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bt_OpenSerial.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_OpenSerial.Location = new System.Drawing.Point(9, 279);
            this.bt_OpenSerial.Margin = new System.Windows.Forms.Padding(4);
            this.bt_OpenSerial.MinimumSize = new System.Drawing.Size(2, 2);
            this.bt_OpenSerial.Name = "bt_OpenSerial";
            this.bt_OpenSerial.Size = new System.Drawing.Size(141, 46);
            this.bt_OpenSerial.Style = Sunny.UI.UIStyle.Custom;
            this.bt_OpenSerial.Symbol = 61475;
            this.bt_OpenSerial.SymbolSize = 28;
            this.bt_OpenSerial.TabIndex = 31;
            this.bt_OpenSerial.Text = "打开串口";
            this.bt_OpenSerial.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_OpenSerial.Click += new System.EventHandler(this.bt_OpenSerial_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.desRecovery);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.controlMode);
            this.groupBox4.Controls.Add(this.loopsend);
            this.groupBox4.Controls.Add(this.EmergencyStop);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.spdIncre);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.SetSpdSend);
            this.groupBox4.Location = new System.Drawing.Point(246, 4);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(310, 416);
            this.groupBox4.TabIndex = 105;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "主控";
            // 
            // desRecovery
            // 
            this.desRecovery.Cursor = System.Windows.Forms.Cursors.Hand;
            this.desRecovery.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.desRecovery.Location = new System.Drawing.Point(160, 128);
            this.desRecovery.Margin = new System.Windows.Forms.Padding(4);
            this.desRecovery.MinimumSize = new System.Drawing.Size(2, 2);
            this.desRecovery.Name = "desRecovery";
            this.desRecovery.Size = new System.Drawing.Size(141, 46);
            this.desRecovery.Style = Sunny.UI.UIStyle.Custom;
            this.desRecovery.Symbol = 61473;
            this.desRecovery.SymbolSize = 28;
            this.desRecovery.TabIndex = 99;
            this.desRecovery.Text = "故障恢复";
            this.desRecovery.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.desRecovery.TipsText = "恢复状态反馈页面的显示颜色";
            this.desRecovery.Click += new System.EventHandler(this.desRecovery_Click);
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(18, 32);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(80, 18);
            this.label20.TabIndex = 98;
            this.label20.Text = "控制模式";
            // 
            // controlMode
            // 
            this.controlMode.DataSource = null;
            this.controlMode.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.controlMode.FillColor = System.Drawing.Color.White;
            this.controlMode.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.controlMode.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.controlMode.Items.AddRange(new object[] {
            "停机",
            "转速模式"});
            this.controlMode.Location = new System.Drawing.Point(9, 57);
            this.controlMode.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.controlMode.MinimumSize = new System.Drawing.Size(94, 0);
            this.controlMode.Name = "controlMode";
            this.controlMode.Padding = new System.Windows.Forms.Padding(0, 0, 45, 3);
            this.controlMode.Size = new System.Drawing.Size(141, 32);
            this.controlMode.TabIndex = 33;
            this.controlMode.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.controlMode.SelectedIndexChanged += new System.EventHandler(this.controlMode_SelectedIndexChanged);
            // 
            // loopsend
            // 
            this.loopsend.AutoSize = true;
            this.loopsend.Location = new System.Drawing.Point(8, 302);
            this.loopsend.Margin = new System.Windows.Forms.Padding(4);
            this.loopsend.Name = "loopsend";
            this.loopsend.Size = new System.Drawing.Size(106, 22);
            this.loopsend.TabIndex = 43;
            this.loopsend.Text = "循环发送";
            this.loopsend.UseVisualStyleBackColor = true;
            this.loopsend.CheckedChanged += new System.EventHandler(this.loopsend_CheckedChanged);
            // 
            // EmergencyStop
            // 
            this.EmergencyStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EmergencyStop.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EmergencyStop.Location = new System.Drawing.Point(160, 52);
            this.EmergencyStop.Margin = new System.Windows.Forms.Padding(4);
            this.EmergencyStop.MinimumSize = new System.Drawing.Size(2, 2);
            this.EmergencyStop.Name = "EmergencyStop";
            this.EmergencyStop.Size = new System.Drawing.Size(141, 46);
            this.EmergencyStop.Style = Sunny.UI.UIStyle.Custom;
            this.EmergencyStop.Symbol = 61912;
            this.EmergencyStop.SymbolSize = 28;
            this.EmergencyStop.TabIndex = 34;
            this.EmergencyStop.Text = "启动";
            this.EmergencyStop.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EmergencyStop.Click += new System.EventHandler(this.EmergencyStop_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 189);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 18);
            this.label12.TabIndex = 20;
            this.label12.Text = "转速增量";
            // 
            // spdIncre
            // 
            this.spdIncre.Location = new System.Drawing.Point(4, 212);
            this.spdIncre.Margin = new System.Windows.Forms.Padding(4);
            this.spdIncre.Name = "spdIncre";
            this.spdIncre.Size = new System.Drawing.Size(128, 28);
            this.spdIncre.TabIndex = 19;
            this.spdIncre.TextChanged += new System.EventHandler(this.spdIncre_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(18, 110);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(80, 18);
            this.label18.TabIndex = 16;
            this.label18.Text = "设定转速";
            // 
            // SetSpdSend
            // 
            this.SetSpdSend.Location = new System.Drawing.Point(8, 132);
            this.SetSpdSend.Margin = new System.Windows.Forms.Padding(4);
            this.SetSpdSend.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.SetSpdSend.Name = "SetSpdSend";
            this.SetSpdSend.Size = new System.Drawing.Size(130, 28);
            this.SetSpdSend.TabIndex = 15;
            this.SetSpdSend.ValueChanged += new System.EventHandler(this.SetSpdSend_ValueChanged);
            // 
            // timer_send
            // 
            this.timer_send.Tick += new System.EventHandler(this.timer_send_Tick);
            // 
            // sp_Com
            // 
            this.sp_Com.ReadTimeout = 20;
            this.sp_Com.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.sp_Com_DataReceived);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(2, 424);
            this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(80, 18);
            this.label35.TabIndex = 108;
            this.label35.Text = "状态反馈";
            // 
            // WarningDescribe
            // 
            this.WarningDescribe.BackColor = System.Drawing.Color.SpringGreen;
            this.WarningDescribe.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WarningDescribe.Location = new System.Drawing.Point(4, 447);
            this.WarningDescribe.Margin = new System.Windows.Forms.Padding(4);
            this.WarningDescribe.Multiline = true;
            this.WarningDescribe.Name = "WarningDescribe";
            this.WarningDescribe.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.WarningDescribe.Size = new System.Drawing.Size(1279, 385);
            this.WarningDescribe.TabIndex = 107;
            // 
            // gbxControlMsg
            // 
            this.gbxControlMsg.Controls.Add(this.label19);
            this.gbxControlMsg.Controls.Add(this.ANGLE);
            this.gbxControlMsg.Controls.Add(this.FAULT_WORD5);
            this.gbxControlMsg.Controls.Add(this.FAULT_WORD4);
            this.gbxControlMsg.Controls.Add(this.FAULT_WORD3);
            this.gbxControlMsg.Controls.Add(this.FAULT_WORD2);
            this.gbxControlMsg.Controls.Add(this.FAULT_WORD1);
            this.gbxControlMsg.Controls.Add(this.label17);
            this.gbxControlMsg.Controls.Add(this.COMPILE_DATE);
            this.gbxControlMsg.Controls.Add(this.label15);
            this.gbxControlMsg.Controls.Add(this.HW_VER);
            this.gbxControlMsg.Controls.Add(this.label14);
            this.gbxControlMsg.Controls.Add(this.FAULT_LEVEL);
            this.gbxControlMsg.Controls.Add(this.label13);
            this.gbxControlMsg.Controls.Add(this.PWM_STS);
            this.gbxControlMsg.Controls.Add(this.label11);
            this.gbxControlMsg.Controls.Add(this.SYS_STS);
            this.gbxControlMsg.Controls.Add(this.label10);
            this.gbxControlMsg.Controls.Add(this.CONTROL_POWER_VOLT);
            this.gbxControlMsg.Controls.Add(this.label9);
            this.gbxControlMsg.Controls.Add(this.motorTemp);
            this.gbxControlMsg.Controls.Add(this.label6);
            this.gbxControlMsg.Controls.Add(this.motorSpd);
            this.gbxControlMsg.Controls.Add(this.lbl_Spd);
            this.gbxControlMsg.Controls.Add(this.label61);
            this.gbxControlMsg.Controls.Add(this.SW_Ver);
            this.gbxControlMsg.Controls.Add(this.label16);
            this.gbxControlMsg.Controls.Add(this.MotorTorqueFB);
            this.gbxControlMsg.Controls.Add(this.label56);
            this.gbxControlMsg.Controls.Add(this.Phase_Curr_RMS);
            this.gbxControlMsg.Controls.Add(this.label26);
            this.gbxControlMsg.Controls.Add(this.ControlTemp);
            this.gbxControlMsg.Controls.Add(this.label33);
            this.gbxControlMsg.Controls.Add(this.IDC);
            this.gbxControlMsg.Controls.Add(this.label32);
            this.gbxControlMsg.Controls.Add(this.UDC);
            this.gbxControlMsg.Controls.Add(this.spdMeter);
            this.gbxControlMsg.Location = new System.Drawing.Point(558, 4);
            this.gbxControlMsg.Margin = new System.Windows.Forms.Padding(4);
            this.gbxControlMsg.Name = "gbxControlMsg";
            this.gbxControlMsg.Padding = new System.Windows.Forms.Padding(4);
            this.gbxControlMsg.Size = new System.Drawing.Size(728, 416);
            this.gbxControlMsg.TabIndex = 100;
            this.gbxControlMsg.TabStop = false;
            this.gbxControlMsg.Text = "数据";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(153, 108);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 18);
            this.label19.TabIndex = 147;
            this.label19.Text = "转子位置";
            // 
            // ANGLE
            // 
            this.ANGLE.Location = new System.Drawing.Point(132, 130);
            this.ANGLE.Margin = new System.Windows.Forms.Padding(4);
            this.ANGLE.Name = "ANGLE";
            this.ANGLE.Size = new System.Drawing.Size(112, 28);
            this.ANGLE.TabIndex = 146;
            this.ANGLE.Text = "0";
            // 
            // FAULT_WORD5
            // 
            this.FAULT_WORD5.AutoSize = true;
            this.FAULT_WORD5.Location = new System.Drawing.Point(606, 380);
            this.FAULT_WORD5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FAULT_WORD5.Name = "FAULT_WORD5";
            this.FAULT_WORD5.Size = new System.Drawing.Size(17, 18);
            this.FAULT_WORD5.TabIndex = 145;
            this.FAULT_WORD5.Text = "0";
            // 
            // FAULT_WORD4
            // 
            this.FAULT_WORD4.AutoSize = true;
            this.FAULT_WORD4.Location = new System.Drawing.Point(458, 380);
            this.FAULT_WORD4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FAULT_WORD4.Name = "FAULT_WORD4";
            this.FAULT_WORD4.Size = new System.Drawing.Size(17, 18);
            this.FAULT_WORD4.TabIndex = 144;
            this.FAULT_WORD4.Text = "0";
            // 
            // FAULT_WORD3
            // 
            this.FAULT_WORD3.AutoSize = true;
            this.FAULT_WORD3.Location = new System.Drawing.Point(309, 380);
            this.FAULT_WORD3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FAULT_WORD3.Name = "FAULT_WORD3";
            this.FAULT_WORD3.Size = new System.Drawing.Size(17, 18);
            this.FAULT_WORD3.TabIndex = 143;
            this.FAULT_WORD3.Text = "0";
            // 
            // FAULT_WORD2
            // 
            this.FAULT_WORD2.AutoSize = true;
            this.FAULT_WORD2.Location = new System.Drawing.Point(160, 380);
            this.FAULT_WORD2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FAULT_WORD2.Name = "FAULT_WORD2";
            this.FAULT_WORD2.Size = new System.Drawing.Size(17, 18);
            this.FAULT_WORD2.TabIndex = 142;
            this.FAULT_WORD2.Text = "0";
            // 
            // FAULT_WORD1
            // 
            this.FAULT_WORD1.AutoSize = true;
            this.FAULT_WORD1.Location = new System.Drawing.Point(12, 380);
            this.FAULT_WORD1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FAULT_WORD1.Name = "FAULT_WORD1";
            this.FAULT_WORD1.Size = new System.Drawing.Size(17, 18);
            this.FAULT_WORD1.TabIndex = 141;
            this.FAULT_WORD1.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(276, 176);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(80, 18);
            this.label17.TabIndex = 140;
            this.label17.Text = "编译日期";
            // 
            // COMPILE_DATE
            // 
            this.COMPILE_DATE.Location = new System.Drawing.Point(255, 198);
            this.COMPILE_DATE.Margin = new System.Windows.Forms.Padding(4);
            this.COMPILE_DATE.Name = "COMPILE_DATE";
            this.COMPILE_DATE.Size = new System.Drawing.Size(112, 28);
            this.COMPILE_DATE.TabIndex = 139;
            this.COMPILE_DATE.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(30, 243);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(80, 18);
            this.label15.TabIndex = 138;
            this.label15.Text = "硬件版本";
            // 
            // HW_VER
            // 
            this.HW_VER.Location = new System.Drawing.Point(9, 266);
            this.HW_VER.Margin = new System.Windows.Forms.Padding(4);
            this.HW_VER.Name = "HW_VER";
            this.HW_VER.Size = new System.Drawing.Size(112, 28);
            this.HW_VER.TabIndex = 137;
            this.HW_VER.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(255, 304);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(116, 18);
            this.label14.TabIndex = 136;
            this.label14.Text = "最大故障等级";
            // 
            // FAULT_LEVEL
            // 
            this.FAULT_LEVEL.Location = new System.Drawing.Point(258, 327);
            this.FAULT_LEVEL.Margin = new System.Windows.Forms.Padding(4);
            this.FAULT_LEVEL.Name = "FAULT_LEVEL";
            this.FAULT_LEVEL.Size = new System.Drawing.Size(112, 28);
            this.FAULT_LEVEL.TabIndex = 135;
            this.FAULT_LEVEL.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(273, 243);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(71, 18);
            this.label13.TabIndex = 134;
            this.label13.Text = "PWM状态";
            // 
            // PWM_STS
            // 
            this.PWM_STS.Location = new System.Drawing.Point(258, 266);
            this.PWM_STS.Margin = new System.Windows.Forms.Padding(4);
            this.PWM_STS.Name = "PWM_STS";
            this.PWM_STS.Size = new System.Drawing.Size(112, 28);
            this.PWM_STS.TabIndex = 133;
            this.PWM_STS.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(147, 308);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 18);
            this.label11.TabIndex = 132;
            this.label11.Text = "系统状态";
            // 
            // SYS_STS
            // 
            this.SYS_STS.Location = new System.Drawing.Point(132, 330);
            this.SYS_STS.Margin = new System.Windows.Forms.Padding(4);
            this.SYS_STS.Name = "SYS_STS";
            this.SYS_STS.Size = new System.Drawing.Size(112, 28);
            this.SYS_STS.TabIndex = 131;
            this.SYS_STS.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(129, 243);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(134, 18);
            this.label10.TabIndex = 130;
            this.label10.Text = "控制器电源电压";
            // 
            // CONTROL_POWER_VOLT
            // 
            this.CONTROL_POWER_VOLT.Location = new System.Drawing.Point(132, 266);
            this.CONTROL_POWER_VOLT.Margin = new System.Windows.Forms.Padding(4);
            this.CONTROL_POWER_VOLT.Name = "CONTROL_POWER_VOLT";
            this.CONTROL_POWER_VOLT.Size = new System.Drawing.Size(112, 28);
            this.CONTROL_POWER_VOLT.TabIndex = 129;
            this.CONTROL_POWER_VOLT.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(273, 108);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 18);
            this.label9.TabIndex = 128;
            this.label9.Text = "电机温度";
            // 
            // motorTemp
            // 
            this.motorTemp.Location = new System.Drawing.Point(255, 130);
            this.motorTemp.Margin = new System.Windows.Forms.Padding(4);
            this.motorTemp.Name = "motorTemp";
            this.motorTemp.Size = new System.Drawing.Size(112, 28);
            this.motorTemp.TabIndex = 127;
            this.motorTemp.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(153, 176);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 18);
            this.label6.TabIndex = 126;
            this.label6.Text = "电机转速";
            // 
            // motorSpd
            // 
            this.motorSpd.Location = new System.Drawing.Point(132, 198);
            this.motorSpd.Margin = new System.Windows.Forms.Padding(4);
            this.motorSpd.Name = "motorSpd";
            this.motorSpd.Size = new System.Drawing.Size(112, 28);
            this.motorSpd.TabIndex = 125;
            this.motorSpd.Text = "0";
            // 
            // lbl_Spd
            // 
            this.lbl_Spd.AutoSize = true;
            this.lbl_Spd.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Spd.Location = new System.Drawing.Point(483, 300);
            this.lbl_Spd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Spd.MaximumSize = new System.Drawing.Size(250, 60);
            this.lbl_Spd.MinimumSize = new System.Drawing.Size(165, 45);
            this.lbl_Spd.Name = "lbl_Spd";
            this.lbl_Spd.Size = new System.Drawing.Size(165, 46);
            this.lbl_Spd.TabIndex = 124;
            this.lbl_Spd.Text = "0";
            this.lbl_Spd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(30, 312);
            this.label61.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(80, 18);
            this.label61.TabIndex = 123;
            this.label61.Text = "软件版本";
            // 
            // SW_Ver
            // 
            this.SW_Ver.Location = new System.Drawing.Point(9, 333);
            this.SW_Ver.Margin = new System.Windows.Forms.Padding(4);
            this.SW_Ver.Name = "SW_Ver";
            this.SW_Ver.Size = new System.Drawing.Size(112, 28);
            this.SW_Ver.TabIndex = 122;
            this.SW_Ver.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(144, 40);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 18);
            this.label16.TabIndex = 119;
            this.label16.Text = "电机转矩";
            // 
            // MotorTorqueFB
            // 
            this.MotorTorqueFB.Location = new System.Drawing.Point(132, 63);
            this.MotorTorqueFB.Margin = new System.Windows.Forms.Padding(4);
            this.MotorTorqueFB.Name = "MotorTorqueFB";
            this.MotorTorqueFB.Size = new System.Drawing.Size(112, 28);
            this.MotorTorqueFB.TabIndex = 118;
            this.MotorTorqueFB.Text = "0";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(30, 176);
            this.label56.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(80, 18);
            this.label56.TabIndex = 117;
            this.label56.Text = "电机电流";
            // 
            // Phase_Curr_RMS
            // 
            this.Phase_Curr_RMS.Location = new System.Drawing.Point(9, 198);
            this.Phase_Curr_RMS.Margin = new System.Windows.Forms.Padding(4);
            this.Phase_Curr_RMS.Name = "Phase_Curr_RMS";
            this.Phase_Curr_RMS.Size = new System.Drawing.Size(112, 28);
            this.Phase_Curr_RMS.TabIndex = 116;
            this.Phase_Curr_RMS.Text = "0";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(255, 40);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(98, 18);
            this.label26.TabIndex = 115;
            this.label26.Text = "控制器温度";
            // 
            // ControlTemp
            // 
            this.ControlTemp.Location = new System.Drawing.Point(255, 63);
            this.ControlTemp.Margin = new System.Windows.Forms.Padding(4);
            this.ControlTemp.Name = "ControlTemp";
            this.ControlTemp.Size = new System.Drawing.Size(112, 28);
            this.ControlTemp.TabIndex = 114;
            this.ControlTemp.Text = "0";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(30, 108);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(80, 18);
            this.label33.TabIndex = 113;
            this.label33.Text = "母线电流";
            // 
            // IDC
            // 
            this.IDC.Location = new System.Drawing.Point(9, 130);
            this.IDC.Margin = new System.Windows.Forms.Padding(4);
            this.IDC.Name = "IDC";
            this.IDC.Size = new System.Drawing.Size(112, 28);
            this.IDC.TabIndex = 112;
            this.IDC.Text = "0";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(36, 40);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(80, 18);
            this.label32.TabIndex = 111;
            this.label32.Text = "母线电压";
            // 
            // UDC
            // 
            this.UDC.Location = new System.Drawing.Point(9, 63);
            this.UDC.Margin = new System.Windows.Forms.Padding(4);
            this.UDC.Name = "UDC";
            this.UDC.Size = new System.Drawing.Size(112, 28);
            this.UDC.TabIndex = 110;
            this.UDC.Text = "0";
            // 
            // spdMeter
            // 
            this.spdMeter.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.spdMeter.Location = new System.Drawing.Point(430, 30);
            this.spdMeter.Margin = new System.Windows.Forms.Padding(4);
            this.spdMeter.MaxValue = 21000D;
            this.spdMeter.MinimumSize = new System.Drawing.Size(2, 2);
            this.spdMeter.MinValue = 0D;
            this.spdMeter.Name = "spdMeter";
            this.spdMeter.Renderer = null;
            this.spdMeter.ScaleSubDivisions = 5;
            this.spdMeter.Size = new System.Drawing.Size(270, 270);
            this.spdMeter.TabIndex = 102;
            this.spdMeter.Text = "uiAnalogMeter1";
            this.spdMeter.Value = 0D;
            // 
            // HC_800w
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 852);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.WarningDescribe);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.gbxControlMsg);
            this.Controls.Add(this.groupBox5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "HC_800w";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "800W电机";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HC_800w_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SetSpdSend)).EndInit();
            this.gbxControlMsg.ResumeLayout(false);
            this.gbxControlMsg.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_Rec;
        private Sunny.UI.UIComboBox cb_StopBitSel;
        private Sunny.UI.UIComboBox cb_CheckBitSel;
        private Sunny.UI.UIComboBox cb_DataBitSel;
        private Sunny.UI.UIComboBox cb_BaudrateSel;
        private Sunny.UI.UIComboBox cb_SerialNum;
        private Sunny.UI.UISymbolButton bt_OpenSerial;
        private System.Windows.Forms.GroupBox groupBox4;
        private Sunny.UI.UISymbolButton EmergencyStop;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox spdIncre;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown SetSpdSend;
        private System.Windows.Forms.Timer timer_send;
        private System.Windows.Forms.CheckBox loopsend;
        private System.IO.Ports.SerialPort sp_Com;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.TextBox WarningDescribe;
        private System.Windows.Forms.GroupBox gbxControlMsg;
        private Sunny.UI.UIAnalogMeter spdMeter;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox MotorTorqueFB;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.TextBox Phase_Curr_RMS;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox ControlTemp;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox IDC;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox UDC;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.TextBox SW_Ver;
        private System.Windows.Forms.Label lbl_Spd;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox PWM_STS;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox SYS_STS;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox CONTROL_POWER_VOLT;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox motorTemp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox motorSpd;
        private System.Windows.Forms.Label FAULT_WORD5;
        private System.Windows.Forms.Label FAULT_WORD4;
        private System.Windows.Forms.Label FAULT_WORD3;
        private System.Windows.Forms.Label FAULT_WORD2;
        private System.Windows.Forms.Label FAULT_WORD1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox COMPILE_DATE;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox HW_VER;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox FAULT_LEVEL;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox ANGLE;
        private System.Windows.Forms.Label label20;
        private Sunny.UI.UIComboBox controlMode;
        private Sunny.UI.UILight LedSend;
        private Sunny.UI.UILight LedRec;
        private Sunny.UI.UILight Led_Device;
        private Sunny.UI.UISymbolButton desRecovery;
    }
}


namespace HCTestDemo
{
    partial class HC_L1
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
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.Led_Device = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_Rec = new System.Windows.Forms.Label();
            this.cb_StopBitSel = new Sunny.UI.UIComboBox();
            this.cb_CheckBitSel = new Sunny.UI.UIComboBox();
            this.cb_DataBitSel = new Sunny.UI.UIComboBox();
            this.cb_BaudrateSel = new Sunny.UI.UIComboBox();
            this.cb_SerialNum = new Sunny.UI.UIComboBox();
            this.bt_OpenSerial = new Sunny.UI.UISymbolButton();
            this.LedSend = new System.Windows.Forms.Button();
            this.LedRec = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
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
            this.lblSpd = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.SW_Ver = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.MotorVol = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.MotorTorqueFB = new System.Windows.Forms.TextBox();
            this.label56 = new System.Windows.Forms.Label();
            this.Phase_Curr_RMS = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.ControlTemp = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.CtrlCurrentDebug = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.BatteryVolDebug = new System.Windows.Forms.TextBox();
            this.spdMeter = new Sunny.UI.UIAnalogMeter();
            this.label51 = new System.Windows.Forms.Label();
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
            this.label5.Location = new System.Drawing.Point(7, 195);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 96;
            this.label5.Text = "停止位";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 94;
            this.label4.Text = "校验位";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 92;
            this.label3.Text = "数据位";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 90;
            this.label2.Text = "波特率";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 89;
            this.label1.Text = "端口";
            // 
            // groupBox5
            // 
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
            this.groupBox5.Controls.Add(this.LedSend);
            this.groupBox5.Controls.Add(this.LedRec);
            this.groupBox5.Location = new System.Drawing.Point(4, 4);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(215, 306);
            this.groupBox5.TabIndex = 98;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "通讯设置";
            // 
            // Led_Device
            // 
            this.Led_Device.Location = new System.Drawing.Point(151, 224);
            this.Led_Device.Margin = new System.Windows.Forms.Padding(4);
            this.Led_Device.Name = "Led_Device";
            this.Led_Device.Size = new System.Drawing.Size(39, 25);
            this.Led_Device.TabIndex = 99;
            this.Led_Device.UseVisualStyleBackColor = true;
            this.Led_Device.Click += new System.EventHandler(this.Led_Device_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(105, 279);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 15);
            this.label7.TabIndex = 98;
            this.label7.Text = "发送";
            // 
            // lbl_Rec
            // 
            this.lbl_Rec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Rec.AutoSize = true;
            this.lbl_Rec.Location = new System.Drawing.Point(7, 279);
            this.lbl_Rec.Name = "lbl_Rec";
            this.lbl_Rec.Size = new System.Drawing.Size(37, 15);
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
            this.cb_StopBitSel.Location = new System.Drawing.Point(80, 184);
            this.cb_StopBitSel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cb_StopBitSel.MinimumSize = new System.Drawing.Size(84, 0);
            this.cb_StopBitSel.Name = "cb_StopBitSel";
            this.cb_StopBitSel.Padding = new System.Windows.Forms.Padding(0, 0, 40, 2);
            this.cb_StopBitSel.Size = new System.Drawing.Size(125, 26);
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
            this.cb_CheckBitSel.Location = new System.Drawing.Point(80, 145);
            this.cb_CheckBitSel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cb_CheckBitSel.MinimumSize = new System.Drawing.Size(84, 0);
            this.cb_CheckBitSel.Name = "cb_CheckBitSel";
            this.cb_CheckBitSel.Padding = new System.Windows.Forms.Padding(0, 0, 40, 2);
            this.cb_CheckBitSel.Size = new System.Drawing.Size(125, 26);
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
            this.cb_DataBitSel.Location = new System.Drawing.Point(80, 106);
            this.cb_DataBitSel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cb_DataBitSel.MinimumSize = new System.Drawing.Size(84, 0);
            this.cb_DataBitSel.Name = "cb_DataBitSel";
            this.cb_DataBitSel.Padding = new System.Windows.Forms.Padding(0, 0, 40, 2);
            this.cb_DataBitSel.Size = new System.Drawing.Size(125, 26);
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
            this.cb_BaudrateSel.Location = new System.Drawing.Point(80, 66);
            this.cb_BaudrateSel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cb_BaudrateSel.MinimumSize = new System.Drawing.Size(84, 0);
            this.cb_BaudrateSel.Name = "cb_BaudrateSel";
            this.cb_BaudrateSel.Padding = new System.Windows.Forms.Padding(0, 0, 40, 2);
            this.cb_BaudrateSel.Size = new System.Drawing.Size(125, 26);
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
            this.cb_SerialNum.Location = new System.Drawing.Point(80, 28);
            this.cb_SerialNum.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cb_SerialNum.MinimumSize = new System.Drawing.Size(84, 0);
            this.cb_SerialNum.Name = "cb_SerialNum";
            this.cb_SerialNum.Padding = new System.Windows.Forms.Padding(0, 0, 40, 2);
            this.cb_SerialNum.Size = new System.Drawing.Size(125, 26);
            this.cb_SerialNum.TabIndex = 32;
            this.cb_SerialNum.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bt_OpenSerial
            // 
            this.bt_OpenSerial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bt_OpenSerial.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_OpenSerial.Location = new System.Drawing.Point(9, 220);
            this.bt_OpenSerial.Margin = new System.Windows.Forms.Padding(4);
            this.bt_OpenSerial.MinimumSize = new System.Drawing.Size(1, 1);
            this.bt_OpenSerial.Name = "bt_OpenSerial";
            this.bt_OpenSerial.Size = new System.Drawing.Size(125, 39);
            this.bt_OpenSerial.Style = Sunny.UI.UIStyle.Custom;
            this.bt_OpenSerial.Symbol = 61475;
            this.bt_OpenSerial.SymbolSize = 28;
            this.bt_OpenSerial.TabIndex = 31;
            this.bt_OpenSerial.Text = "打开串口";
            this.bt_OpenSerial.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_OpenSerial.Click += new System.EventHandler(this.bt_OpenSerial_Click);
            // 
            // LedSend
            // 
            this.LedSend.Location = new System.Drawing.Point(151, 274);
            this.LedSend.Margin = new System.Windows.Forms.Padding(4);
            this.LedSend.Name = "LedSend";
            this.LedSend.Size = new System.Drawing.Size(39, 25);
            this.LedSend.TabIndex = 10;
            this.LedSend.UseVisualStyleBackColor = true;
            // 
            // LedRec
            // 
            this.LedRec.Location = new System.Drawing.Point(51, 274);
            this.LedRec.Margin = new System.Windows.Forms.Padding(4);
            this.LedRec.Name = "LedRec";
            this.LedRec.Size = new System.Drawing.Size(39, 25);
            this.LedRec.TabIndex = 9;
            this.LedRec.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.loopsend);
            this.groupBox4.Controls.Add(this.EmergencyStop);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.spdIncre);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.SetSpdSend);
            this.groupBox4.Location = new System.Drawing.Point(219, 4);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(276, 306);
            this.groupBox4.TabIndex = 105;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "主控";
            // 
            // loopsend
            // 
            this.loopsend.AutoSize = true;
            this.loopsend.Location = new System.Drawing.Point(8, 195);
            this.loopsend.Margin = new System.Windows.Forms.Padding(4);
            this.loopsend.Name = "loopsend";
            this.loopsend.Size = new System.Drawing.Size(89, 19);
            this.loopsend.TabIndex = 43;
            this.loopsend.Text = "循环发送";
            this.loopsend.UseVisualStyleBackColor = true;
            this.loopsend.CheckedChanged += new System.EventHandler(this.loopsend_CheckedChanged);
            // 
            // EmergencyStop
            // 
            this.EmergencyStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EmergencyStop.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EmergencyStop.Location = new System.Drawing.Point(143, 54);
            this.EmergencyStop.Margin = new System.Windows.Forms.Padding(4);
            this.EmergencyStop.MinimumSize = new System.Drawing.Size(1, 1);
            this.EmergencyStop.Name = "EmergencyStop";
            this.EmergencyStop.Size = new System.Drawing.Size(125, 118);
            this.EmergencyStop.Style = Sunny.UI.UIStyle.Custom;
            this.EmergencyStop.Symbol = 61912;
            this.EmergencyStop.SymbolSize = 40;
            this.EmergencyStop.TabIndex = 34;
            this.EmergencyStop.Text = "启动";
            this.EmergencyStop.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EmergencyStop.Click += new System.EventHandler(this.EmergencyStop_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 101);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 15);
            this.label12.TabIndex = 20;
            this.label12.Text = "转速增量";
            // 
            // spdIncre
            // 
            this.spdIncre.Location = new System.Drawing.Point(5, 120);
            this.spdIncre.Margin = new System.Windows.Forms.Padding(4);
            this.spdIncre.Name = "spdIncre";
            this.spdIncre.Size = new System.Drawing.Size(115, 25);
            this.spdIncre.TabIndex = 19;
            this.spdIncre.TextChanged += new System.EventHandler(this.spdIncre_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(17, 35);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(67, 15);
            this.label18.TabIndex = 16;
            this.label18.Text = "设定转速";
            // 
            // SetSpdSend
            // 
            this.SetSpdSend.Location = new System.Drawing.Point(8, 54);
            this.SetSpdSend.Margin = new System.Windows.Forms.Padding(4);
            this.SetSpdSend.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.SetSpdSend.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
            this.SetSpdSend.Name = "SetSpdSend";
            this.SetSpdSend.Size = new System.Drawing.Size(116, 25);
            this.SetSpdSend.TabIndex = 15;
            this.SetSpdSend.ValueChanged += new System.EventHandler(this.SetSpdSend_ValueChanged);
            // 
            // timer_send
            // 
            this.timer_send.Interval = 50;
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
            this.label35.Location = new System.Drawing.Point(1, 321);
            this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(67, 15);
            this.label35.TabIndex = 108;
            this.label35.Text = "状态反馈";
            // 
            // WarningDescribe
            // 
            this.WarningDescribe.BackColor = System.Drawing.Color.SpringGreen;
            this.WarningDescribe.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WarningDescribe.Location = new System.Drawing.Point(4, 340);
            this.WarningDescribe.Margin = new System.Windows.Forms.Padding(4);
            this.WarningDescribe.Multiline = true;
            this.WarningDescribe.Name = "WarningDescribe";
            this.WarningDescribe.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.WarningDescribe.Size = new System.Drawing.Size(1044, 270);
            this.WarningDescribe.TabIndex = 107;
            // 
            // gbxControlMsg
            // 
            this.gbxControlMsg.Controls.Add(this.lblSpd);
            this.gbxControlMsg.Controls.Add(this.label61);
            this.gbxControlMsg.Controls.Add(this.SW_Ver);
            this.gbxControlMsg.Controls.Add(this.label8);
            this.gbxControlMsg.Controls.Add(this.MotorVol);
            this.gbxControlMsg.Controls.Add(this.label16);
            this.gbxControlMsg.Controls.Add(this.MotorTorqueFB);
            this.gbxControlMsg.Controls.Add(this.label56);
            this.gbxControlMsg.Controls.Add(this.Phase_Curr_RMS);
            this.gbxControlMsg.Controls.Add(this.label26);
            this.gbxControlMsg.Controls.Add(this.ControlTemp);
            this.gbxControlMsg.Controls.Add(this.label33);
            this.gbxControlMsg.Controls.Add(this.CtrlCurrentDebug);
            this.gbxControlMsg.Controls.Add(this.label32);
            this.gbxControlMsg.Controls.Add(this.BatteryVolDebug);
            this.gbxControlMsg.Controls.Add(this.spdMeter);
            this.gbxControlMsg.Controls.Add(this.label51);
            this.gbxControlMsg.Location = new System.Drawing.Point(505, 4);
            this.gbxControlMsg.Margin = new System.Windows.Forms.Padding(4);
            this.gbxControlMsg.Name = "gbxControlMsg";
            this.gbxControlMsg.Padding = new System.Windows.Forms.Padding(4);
            this.gbxControlMsg.Size = new System.Drawing.Size(544, 306);
            this.gbxControlMsg.TabIndex = 100;
            this.gbxControlMsg.TabStop = false;
            this.gbxControlMsg.Text = "数据";
            // 
            // lblSpd
            // 
            this.lblSpd.AutoSize = true;
            this.lblSpd.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSpd.Location = new System.Drawing.Point(321, 266);
            this.lblSpd.MaximumSize = new System.Drawing.Size(171, 38);
            this.lblSpd.MinimumSize = new System.Drawing.Size(168, 36);
            this.lblSpd.Name = "lblSpd";
            this.lblSpd.Size = new System.Drawing.Size(168, 36);
            this.lblSpd.TabIndex = 134;
            this.lblSpd.Text = "0";
            this.lblSpd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(164, 154);
            this.label61.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(67, 15);
            this.label61.TabIndex = 123;
            this.label61.Text = "软件版本";
            // 
            // SW_Ver
            // 
            this.SW_Ver.Location = new System.Drawing.Point(145, 172);
            this.SW_Ver.Margin = new System.Windows.Forms.Padding(4);
            this.SW_Ver.Name = "SW_Ver";
            this.SW_Ver.Size = new System.Drawing.Size(100, 25);
            this.SW_Ver.TabIndex = 122;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(156, 95);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 15);
            this.label8.TabIndex = 121;
            this.label8.Text = "电机电压";
            // 
            // MotorVol
            // 
            this.MotorVol.Location = new System.Drawing.Point(145, 114);
            this.MotorVol.Margin = new System.Windows.Forms.Padding(4);
            this.MotorVol.Name = "MotorVol";
            this.MotorVol.Size = new System.Drawing.Size(100, 25);
            this.MotorVol.TabIndex = 120;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(156, 35);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(67, 15);
            this.label16.TabIndex = 119;
            this.label16.Text = "电机转矩";
            // 
            // MotorTorqueFB
            // 
            this.MotorTorqueFB.Location = new System.Drawing.Point(145, 54);
            this.MotorTorqueFB.Margin = new System.Windows.Forms.Padding(4);
            this.MotorTorqueFB.Name = "MotorTorqueFB";
            this.MotorTorqueFB.Size = new System.Drawing.Size(100, 25);
            this.MotorTorqueFB.TabIndex = 118;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(27, 214);
            this.label56.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(67, 15);
            this.label56.TabIndex = 117;
            this.label56.Text = "电机电流";
            // 
            // Phase_Curr_RMS
            // 
            this.Phase_Curr_RMS.Location = new System.Drawing.Point(8, 232);
            this.Phase_Curr_RMS.Margin = new System.Windows.Forms.Padding(4);
            this.Phase_Curr_RMS.Name = "Phase_Curr_RMS";
            this.Phase_Curr_RMS.Size = new System.Drawing.Size(100, 25);
            this.Phase_Curr_RMS.TabIndex = 116;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(8, 154);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(82, 15);
            this.label26.TabIndex = 115;
            this.label26.Text = "控制器温度";
            // 
            // ControlTemp
            // 
            this.ControlTemp.Location = new System.Drawing.Point(8, 172);
            this.ControlTemp.Margin = new System.Windows.Forms.Padding(4);
            this.ControlTemp.Name = "ControlTemp";
            this.ControlTemp.Size = new System.Drawing.Size(100, 25);
            this.ControlTemp.TabIndex = 114;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(19, 95);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(67, 15);
            this.label33.TabIndex = 113;
            this.label33.Text = "母线电流";
            // 
            // CtrlCurrentDebug
            // 
            this.CtrlCurrentDebug.Location = new System.Drawing.Point(8, 114);
            this.CtrlCurrentDebug.Margin = new System.Windows.Forms.Padding(4);
            this.CtrlCurrentDebug.Name = "CtrlCurrentDebug";
            this.CtrlCurrentDebug.Size = new System.Drawing.Size(100, 25);
            this.CtrlCurrentDebug.TabIndex = 112;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(32, 34);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(67, 15);
            this.label32.TabIndex = 111;
            this.label32.Text = "母线电压";
            // 
            // BatteryVolDebug
            // 
            this.BatteryVolDebug.Location = new System.Drawing.Point(8, 52);
            this.BatteryVolDebug.Margin = new System.Windows.Forms.Padding(4);
            this.BatteryVolDebug.Name = "BatteryVolDebug";
            this.BatteryVolDebug.Size = new System.Drawing.Size(100, 25);
            this.BatteryVolDebug.TabIndex = 110;
            // 
            // spdMeter
            // 
            this.spdMeter.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.spdMeter.Location = new System.Drawing.Point(285, 35);
            this.spdMeter.Margin = new System.Windows.Forms.Padding(4);
            this.spdMeter.MaxValue = 2100D;
            this.spdMeter.MinimumSize = new System.Drawing.Size(1, 1);
            this.spdMeter.MinValue = -2100D;
            this.spdMeter.Name = "spdMeter";
            this.spdMeter.Renderer = null;
            this.spdMeter.ScaleSubDivisions = 5;
            this.spdMeter.Size = new System.Drawing.Size(240, 225);
            this.spdMeter.TabIndex = 102;
            this.spdMeter.Text = "uiAnalogMeter1";
            this.spdMeter.Value = 0D;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(361, 16);
            this.label51.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(67, 15);
            this.label51.TabIndex = 104;
            this.label51.Text = "电机转速";
            // 
            // HC_L1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 626);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.WarningDescribe);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.gbxControlMsg);
            this.Controls.Add(this.groupBox5);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "HC_L1";
            this.Text = "L1垂推电机";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HC_L1_FormClosing);
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
        private System.Windows.Forms.Button LedSend;
        private System.Windows.Forms.Button LedRec;
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
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Button Led_Device;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox MotorVol;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox MotorTorqueFB;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.TextBox Phase_Curr_RMS;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox ControlTemp;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox CtrlCurrentDebug;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox BatteryVolDebug;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.TextBox SW_Ver;
        private System.Windows.Forms.Label lblSpd;
    }
}


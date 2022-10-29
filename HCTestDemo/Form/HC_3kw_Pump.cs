using ControlCAN;
using IDBCManager;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Yokogawa.Tm.WT1800CommSample.cs;
using WinFormsChartSamples;


namespace HCTestDemo
{
    public partial class HC_3kw_Pump : Form
    {
        #region 全局变量


        //定义USBCAN盒变量
        public UInt32 m_devind = 0;//CAN盒索引号
        public UInt32 ledcount_can = 0;//CAN接收数据计数
        VCI_CAN_OBJ[] send_can = new VCI_CAN_OBJ[50];//CAN发送数据数据
        Int16 setSpd; //控制转速
        Int16 setTorque;//控制转矩
        double spd;//实时转速
        UInt16 debugSpd = 0;
        Int16 debugSet1 = 0;
        Int16 debugSet2 = 0;
        UInt32 Motor_Fault;//调试报文故障码
        UInt32 Motor_Warning;//调试报文告警码
        Int64 ControlMotorFault;//整车报文故障码
        int[] controlFalut = new int[4];


        //CAN通讯的ID
        public const uint VCU_MCU = 0xCD04E48;
        public const uint Tx_MCU_1 = 0x561;
        public const uint Tx_MCU_2 = 0x562;
        public const uint Tx_MCU_3 = 0x563;
        public const uint Tx_MCU_4 = 0x564;
        public const uint Tx_MCU_5 = 0x565;
        public const uint Tx_MCU_6 = 0x566;
        public const uint Tx_MCU_7 = 0x567;
        public const uint PC_TestCmd = 0x11F;
        public const uint MCU_Debug_Info1 = 0x112;
        public const uint MCU_Debug_Info2 = 0x113;
        public const uint MCU_Debug_Info3 = 0x114;
        public const uint MCU_Debug_Info4 = 0x115;
        public const uint MCU_Debug_Info5 = 0x116;
        public const uint MCU_Debug_Info6 = 0x117;
        public const uint MCU_Debug_Info7 = 0x11D;
        public const uint MCU_Parameters = 0x11C;
        public const uint PC_Parameters = 0x11E;


        //电机通讯ID
        uint motorID = Tx_MCU_1;
        uint bootLoadID;

        //EXCEL读写部分
        //Thread th;
        IWorkbook wkBook;
        ISheet wkSheet;
        ICellStyle style;//声明style1对象，设置Excel表格的样式
        IFont font;
        IRow irow;
        int row = 0; //excel写入数据的行数
        int msgNum = 0;//记录数据量
        int timerPeriod = 100000; //定时器间隔
        bool autoSaveFlg = false; //保存标志
        string filePathName;//Excel数据保存位置
        string Dirpath = null;//Excel数据保存文件夹位置
        //需要记录的全局变量
        List<TextBox> textMsg = new List<TextBox>(); //所有待记录的数据窗口
        UILedStopwatch uiLedWatch;

        //TCP
        Socket client;
        IPEndPoint point;
        bool recFlag = false;
        bool ConnectFlag = false;
        TextBox[] tb = new TextBox[14];
        Connection connection = new Connection();


        //模块调用的委托和实例
        Action<double, int> paraReadAction; //调用ParaMeModifica模块方法读取参数委托
        Action<VCI_CAN_OBJ> loadFileAction; //调用BootLoadBinFile模块方法下载BIN文件委托
        Action<uint, VCI_CAN_OBJ> checkIDInfoAction;//调用BootLoadBinFile模块方法获取控制器ID委托
        BootLoadBinFile bl; //BootLoad实例

        //DBC
        uint m_hDBC = IDBCManager.Define.INVALID_DBC_HANDLE;
        bool loadDBC = false;
        DbcHelper dbc = new DbcHelper();


        //定时器
        System.Threading.Timer timer_msgSave;
        System.Threading.Timer timer_SendTCP;
        System.Threading.Timer timer_RecTCP;
        System.Threading.Timer timer_SaveTime;


        Action action;
        #endregion
        public HC_3kw_Pump()
        {
            InitializeComponent();
        }

        public HC_3kw_Pump(Action action)
        {
            InitializeComponent();
            this.action = action;
        }


        #region 主通讯界面事件
        private void MainFrm_Load(object sender, EventArgs e)
        {
            //DBC导入初始化
            m_hDBC=dbc.InitialDbcHandler();

            //默认界面设置
            DeviceSlect.SelectedIndex = 3;
            ChannelSelect.SelectedIndex = 1;
            BotSelect.SelectedIndex = 6;
            EnableMotor.SelectedIndex = 0;
            MotorNum.SelectedIndex = 0;
            VehicleCtrMode.SelectedIndex = 2;
            LV_DNR.SelectedIndex = 1;
            textBox5.Text = "5";
            textBox6.Text = "100";
            cbxIP.SelectedIndex = 0;
            cbxPort.SelectedIndex = 0;
            TcpDeviceSelect.SelectedIndex = 0;
            EPTMode.SelectedIndex = 0;
            DebugModeChooseTest.SelectedIndex = 0;
            toolstrip_SaveTime.Alignment = ToolStripItemAlignment.Right;//excel存储动作时间移到右边
            toolStrip_info.Text = "欢迎使用" + this.Text + "上位机~~";
            //定义一个TextBox数组
            tb = new TextBox[] { MotorSpdWT3000, MotorTrqWT3000, InverterPowerWT3000, AlternatePowerWT3000, MotorPowerWT3000, MotorEffWT3000, InverterEffWT3000, SysEffWT3000, InverterVolWT3000, InverterCurrentWT3000, MotorVolWT3000, MotorCurrentWT3000, InverterWT3000Q, InverterWT3000Lamda };
            //遍历TextBox初始化设置并添加到待记录数据List

            foreach (Control item in gbxControlMsg.Controls)
            {
                if (item is System.Windows.Forms.TextBox)
                {
                    item.Text = "0";
                    textMsg.Add((TextBox)item);
                }
            }
            foreach (Control item in gbxDebugMsg.Controls)
            {
                if (item is System.Windows.Forms.TextBox)
                {
                    item.Text = "0.0";
                    textMsg.Add((TextBox)item);
                }

            }
            textMsg.AddRange(tb);
            //添加沃森 汉森电源用户控件
            tb_effTestSave.TabPages.Add("汉森电源");
            tb_effTestSave.TabPages[1].Controls.Add(new HanSenPowerControl(ReturnDBCHandle));
            //tb_effTestSave.TabPages.Add("沃森电源");
            //tb_effTestSave.TabPages[2].Controls.Add(new WoSenPowerControl());
            //tb_effTestSave.TabPages.Add("实时转速");
            //tb_effTestSave.TabPages[3].Controls.Add(new RealTimeSample());

            //BootLoad模块导入
            uiTabControl1.TabPages.Add("BootLoad");
            bl = new BootLoadBinFile();
            uiTabControl1.TabPages[1].Controls.Add(bl);
            loadFileAction = bl.RecLoadInfo;
            checkIDInfoAction = bl.CheckIDInfo;


            //参数读写模块导入
            uiTabControl1.TabPages.Add("参数读写");
            ParameModifica pa = new ParameModifica(ReturnDBCHandle);
            uiTabControl1.TabPages[2].Controls.Add(pa);
            //调用模块读取参数方法
            paraReadAction = pa.ReadPara;

        }
        private void MsgFrm_FormClosing(object sender, FormClosingEventArgs e)
        {


            //在点击右上角关闭按钮或者手动ALT+F4关闭窗口时，做个确定关闭的判断
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //如果关闭窗口时，EXCEL实时记录数据还在进行，应先关闭记录动作
                if (bt_AutoSave.Text == "停止记录")
                {
                    UIPage ui = new UIPage();
                    ui.ShowWarningTip("数据正在实时记录,请关闭EXCEL实时保存后再关闭界面!");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    DialogResult result = MessageBox.Show("确定要退出程序?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        Method.DBC_Release(m_hDBC);
                        File.Delete("dbclog.txt");
                        action();
                    }
                    e.Cancel = result != DialogResult.OK;
                }
            }

        }

        private void connectbutton_Click(object sender, EventArgs e)
        {
            if (loadDBC)
            {
                Device.DevType = Device.SelcetDev(DeviceSlect.SelectedItem.ToString());
                if (Device.m_bOpen == 1)
                {
                    Device.VCI_CloseDevice(Device.DevType, m_devind);//m_devtype设备名称默认USBCAN2,m_devind索引号默认0
                    Device.m_bOpen = 0;
                    LedDebug.State = UILightState.Off;
                    UIPage ui = new UIPage();
                    ui.ShowErrorTip("断开设备连接成功");

                }
                else
                {

                    if (Device.VCI_OpenDevice(Device.DevType, 0, 0) == 1) //打开设备成功
                    {
                        Device.m_bOpen = 1;
                        Device.CanID = (uint)ChannelSelect.SelectedIndex;
                        VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();
                        CanChanelInit(Device.DevType, BotSelect.SelectedItem.ToString(), (uint)ChannelSelect.SelectedIndex, config);
                        UIPage ui = new UIPage();
                        ui.ShowSuccessTip("打开设备连接成功");
                    }
                    else
                    {
                        UIPage ui = new UIPage();
                        ui.ShowWarningTip("打开设备失败,请检查设备类型和CAN盒物理连接是否正确");
                        return;
                    }

                }
                connectbutton.Text = Device.m_bOpen == 1 ? "断开连接" : "连接设备";
                connectbutton.Symbol = Device.m_bOpen == 1 ? 61758 : 61475;
                timer_rec.Enabled = Device.m_bOpen == 1 ? true : false;//根据设备打开状态控制接收定时器的开关
            }
            else
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip("DBC文件未加载！");

            }
        }

        unsafe private void timer_rec_Tick(object sender, EventArgs e)
        {
            UInt32 debugUrecNum = Device.VCI_GetReceiveNum(Device.DevType, 0, (uint)ChannelSelect.SelectedIndex);//尚未被读取的帧数
            if (debugUrecNum != 0) //判断是否有帧需要接收
            {
                UInt32 con_maxlen = 50;//用来接收的帧结构体数组的长度（本次接收的最大帧数，实际返回值小于等于这个值）。
                //用来接收的帧结构体VCI_CAN_OBJ数组的首指针。
                IntPtr pt_debug = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * (Int32)con_maxlen);
                UInt32 debugRecNum = Device.VCI_Receive(Device.DevType, 0, (uint)ChannelSelect.SelectedIndex, pt_debug, con_maxlen, 10);//实际读取到的帧数
                //CAN接收的数据
                if (debugRecNum != 4294967295 && debugRecNum > 0)
                {
                    //构造LED闪烁来显示CAN数据的接收和发送动作正在进行
                    ledcount_can++;
                    if (ledcount_can >= 7)
                    {
                        ledcount_can = 0;
                        LedDebug.State = UILightState.On;
                    }
                    else if (ledcount_can >= 3)
                    {
                        LedDebug.State = UILightState.Off;

                    }

                    for (UInt32 i = 0; i < debugRecNum; i++)
                    {

                        VCI_CAN_OBJ rec_can0 = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((UInt32)pt_debug + i * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));
                        rec_can0.TimeStamp = 0; //设备接收到某一帧的时间标识。 时间标示从CAN卡上电开始计时，计时单位为0.1ms。


                        //将接收到的数据转换成DBCMessage
                        can_frame obj_old = new can_frame();
                        obj_old.data = rec_can0.Data;
                        obj_old.can_dlc = rec_can0.DataLen;
                        obj_old.can_id = rec_can0.ID;
                        IntPtr ptr_msg = Marshal.AllocHGlobal((Marshal.SizeOf(typeof(DBCMessage))));
                        IntPtr ptr_frame = Marshal.AllocHGlobal((Marshal.SizeOf(typeof(can_frame))));
                        Marshal.StructureToPtr(obj_old, ptr_frame, false);
                        Method.DBC_Analyse(m_hDBC, ptr_frame, ptr_msg);
                        DBCMessage msg = (DBCMessage)Marshal.PtrToStructure((IntPtr)((UInt32)ptr_msg), typeof(DBCMessage));

                        #region 实时通讯报文
                        //根据接收的数据ID判断
                        if (rec_can0.ID == motorID)
                        {
                            //内部指令反馈帧
                            if (rec_can0.DataLen == 4)
                            {
                                MotorIDNow.Text = "0x" + ((rec_can0.Data[0] << 8) + (rec_can0.Data[1])).ToString("X");
                                MotorSpdMax.Text = ((Int16)(rec_can0.Data[2] << 8) + (rec_can0.Data[3])).ToString();
                            }
                            //状态指令反馈帧
                            if (rec_can0.DataLen == 8)
                            {
                                lblSpd.Text = MotorSpdFb.Text = ((Int16)(rec_can0.Data[0] << 8) + (rec_can0.Data[1])).ToString();
                                spd = spdMeter.Value = ((Int16)(rec_can0.Data[0] << 8) + (rec_can0.Data[1]));
                                ControlMotorFault = ((Int64)(rec_can0.Data[6] << 32) + (rec_can0.Data[5] << 24) + (rec_can0.Data[4] << 16) + (0 << 8) + (rec_can0.Data[2]));
                                controlFalut[0] = rec_can0.Data[2];
                                controlFalut[1] = rec_can0.Data[4];
                                controlFalut[2] = rec_can0.Data[5];
                                controlFalut[3] = rec_can0.Data[6];
                            }

                        }
                        switch (msg.nID)
                        {
                            #region 调试报文
                            //MCU_Debug_Info1
                            case MCU_Debug_Info1:
                                UdDebug.Text = msg.vSignals[2].nValue.ToString("F1");
                                UqDebug.Text = msg.vSignals[0].nValue.ToString("F1");
                                SpdIndexDebug.Text = msg.vSignals[3].nValue.ToString();
                                TorqueIndexDebug.Text = msg.vSignals[1].nValue.ToString();

                                break;
                            //MCU_Debug_Info2
                            case MCU_Debug_Info2:
                                DAxisCmdCurrent.Text = msg.vSignals[3].nValue.ToString("F1");
                                QAxisCmdCurrent.Text = msg.vSignals[1].nValue.ToString("F1");
                                DAxisFbkCurrent.Text = msg.vSignals[2].nValue.ToString("F1");
                                QAxisFbkCurrent.Text = msg.vSignals[0].nValue.ToString("F1");

                                break;
                            //MCU_Debug_Info3
                            case MCU_Debug_Info3:

                                //判断igbt温度等级
                                double igbtTemp = msg.vSignals[5].nValue;
                                igtbtTempDebug.Text = FanIgbtTemp.Text = igbtTemp.ToString();
                                double igbtTempLevel = igbtTemp >= 0 ? (double)(95 - igbtTemp) / 95 : (double)(40 - Math.Abs(igbtTemp)) / 40;
                                label_IGBTTemp.Text = igbtTemp.ToString() + "℃";
                                label54.Text = string.Format("{0:P1}", igbtTempLevel) + "   Less ....";

                                KL30VolDebug.Text = msg.vSignals[1].nValue.ToString();
                                ResolverOffsetDebug.Text = msg.vSignals[4].nValue.ToString();
                                TrqOutlineDebug.Text = msg.vSignals[2].nValue.ToString();
                                PowerMax.Text = msg.vSignals[3].nValue.ToString();
                                KL15VolDebug.Text = msg.vSignals[0].nValue.ToString();
                                break;
                            //MCU_Debug_Info4
                            case MCU_Debug_Info4:
                                Motor_Fault = (UInt32)msg.vSignals[2].nValue;
                                Motor_Warning = (UInt32)msg.vSignals[1].nValue;
                                SpdFWIndexDebug.Text = msg.vSignals[0].nValue.ToString(); //弱磁索引
                                label35.Text = "故障码：" + Motor_Fault.ToString("F0");
                                label46.Text = "告警码：" + Motor_Warning.ToString("F0");

                                break;
                            //MCU_Debug_Info5
                            case MCU_Debug_Info5:
                                UPhaseCurDebug.Text = msg.vSignals[2].nValue.ToString("F1");
                                VPhaseCurDebug.Text = msg.vSignals[1].nValue.ToString("F1");
                                WPhaseCurDebug.Text = msg.vSignals[0].nValue.ToString("F1");
                                ResolverValueDebug.Text = msg.vSignals[3].nValue.ToString();
                                break;
                            //MCU_Debug_Info6
                            case MCU_Debug_Info6:
                                UaDebug.Text = msg.vSignals[1].nValue.ToString("F1");
                                UbDebug.Text = msg.vSignals[0].nValue.ToString("F1");
                                TorqueCmdDebug.Text = msg.vSignals[2].nValue.ToString();
                                SpdCmdDebug.Text = msg.vSignals[3].nValue.ToString();
                                break;
                            //MCU_Debug_Info7
                            case MCU_Debug_Info7:
                                CtrlCurrentDebug.Text = msg.vSignals[0].nValue.ToString("F1");

                                //判断电机温度等级
                                double motorTemp = msg.vSignals[1].nValue;
                                double motorTempLevel = motorTemp >= 0 ? (double)(160 - motorTemp) / 160 : (double)(40 - Math.Abs(motorTemp)) / 40;
                                label_MotorTemp.Text = motorTemp.ToString() + "℃";
                                label50.Text = string.Format("{0:P1}", motorTempLevel) + "   Less ....";
                                motorTempDebug.Text = Fan_MotorTemp.Text = motorTemp.ToString("F1");
                                ADC_BusVoltage.Text = msg.vSignals[2].nValue.ToString();
                                BatteryVolDebug.Text = BusVoltage.Text = msg.vSignals[3].nValue.ToString("F1");
                                //UInt16 SwVer = (UInt16)msg.vSignals[0].nValue;
                                //UInt16 CompileData = (UInt16)msg.vSignals[3].nValue;
                                //UInt16 C = (UInt16)((SwVer & 0x000F) >> 0);
                                //UInt16 V = (UInt16)((SwVer & 0x00F0) >> 4);
                                //UInt16 B = (UInt16)((SwVer & 0x0F00) >> 8);
                                //UInt16 D = (UInt16)((SwVer & 0xF000) >> 12);
                                //UInt16 Year = (UInt16)(((CompileData & 0xFE00) >> 9) + 2000);
                                //UInt16 Mon = (UInt16)((CompileData & 0x01e0) >> 5);
                                //UInt16 Day = (UInt16)((CompileData & 0x001f) >> 0);
                                //statusStrip.Text = "编译日期:" + Year.ToString() + "年" + Mon.ToString() + "月" + Day.ToString() + "日" + "    " + "软件版本:" + C.ToString() + "." + V.ToString() + "." + B.ToString() + "." + D.ToString();
                                //CompileDate.Text = Year.ToString() + "/" + Mon.ToString() + "/" + Day.ToString();
                                //SW_Ver.Text = "V" + C.ToString() + "." + V.ToString() + "." + B.ToString() + "." + D.ToString();
                                break;



                                #endregion
                                #region 整车报文

                                //// FanSts1  
                                //case VCU_MCU:
                                //    double igbtTemp = msg.vSignals[0].nValue;
                                //    FanIGBTTemp.Text = igbtTemp.ToString();
                                //    //判断igbt温度等级
                                //    double igbtTempLevel = igbtTemp >= 0 ? (double)(160 - igbtTemp) / 160 : (double)(40 - Math.Abs(igbtTemp)) / 40;
                                //    label_IGBTTemp.Text = igbtTemp.ToString() + "℃";
                                //    label54.Text = string.Format("{0:P1}", igbtTempLevel) + "   Less ....";

                                //    double motorTemp = msg.vSignals[1].nValue;
                                //    Fan_MotorTemp.Text = motorTemp.ToString();
                                //    //判断电机温度等级
                                //    double motorTempLevel = motorTemp >= 0 ? (double)(160 - motorTemp) / 160 : (double)(40 - Math.Abs(motorTemp)) / 40;
                                //    label_MotorTemp.Text = motorTemp.ToString() + "℃";
                                //    label50.Text = string.Format("{0:P1}", motorTempLevel) + "   Less ....";

                                //    Phase_Curr_RMS.Text = msg.vSignals[2].nValue.ToString();
                                //    FanSts_MotorLifeSig.Text = msg.vSignals[3].nValue.ToString();

                                //    break;



                                #endregion
                        }
                        #endregion

                        #region BootLoad报文
                        bootLoadID = bl.RecIDChange(); //获取bootload接收ID
                        if (rec_can0.ID == bootLoadID)
                        {
                            loadFileAction(rec_can0);
                        }
                        #region 读取反馈控制器ID
                        //查询设备ID反馈
                        if (rec_can0.ID == 0x18BAB0A1 && rec_can0.Data[1] == 1)
                        {
                            checkIDInfoAction(rec_can0.Data[2], rec_can0);
                        }
                        #endregion
                        #endregion

                        #region 参数读写报文处理

                        if (msg.nID == MCU_Parameters)
                        {
                            paraReadAction(msg.vSignals[0].nValue, (int)msg.vSignals[1].nValue);
                        }
                        #endregion

                        EroCheck();
                        Marshal.FreeHGlobal(ptr_msg);
                        Marshal.FreeHGlobal(ptr_frame);
                    }
                    Marshal.FreeHGlobal(pt_debug);

                }
                else
                {
                    //判断CAN盒是否正常在线
                    VCI_ERR_INFO canerror = new VCI_ERR_INFO();
                    Device.VCI_ReadErrInfo(Device.DevType, 0, (uint)ChannelSelect.SelectedIndex, ref canerror);


                }

            }
        }


        /// <summary>
        /// 发送定时器的启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_send_Tick(object sender, EventArgs e)
        {

            PackPCTestCmd();
        }

        /// <summary>
        /// 勾选自动发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loopsend_CheckedChanged(object sender, EventArgs e)
        {
            //根据循环发送勾选开关发送定时器
            timer_send.Enabled = LoopSend.Checked == true ? true : false;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_AutoSave_Click(object sender, EventArgs e)
        {
            try
            {
                //开始自动保存
                if (bt_AutoSave.Symbol == 61674)
                {
                    autoSaveFlg = true;
                    //根据日期创建数据保存文件夹路径
                    Dirpath = System.IO.Path.Combine((Application.StartupPath + @"\DataSave\3KW_PUMP\"), DateTime.Now.ToString("yyyyMMdd"));
                    //定义记录数据的异步定时器
                    timer_msgSave = new System.Threading.Timer(SaveExcelMsg, null, Timeout.Infinite, 100);
                    //弹出Excel保存路径设置
                    ExcelPathNameInit msgInit = new ExcelPathNameInit(SaveExcelMsgInit, Dirpath);
                    msgInit.Show();

                }
                else  //结束保存动作
                {
                    
                    EndSave();

                }

            }
            catch (Exception ex)
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip(ex.Message);
            }

        }

        private void openDir_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Dirpath);
            }
            catch (Exception e1)
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip(e1.Message);

            }
        }

        private void RecoverFault_Click(object sender, EventArgs e)
        {

            //回复故障页
            WarnningDescribe.Clear();
            FaultDescribe.Clear();
            ControlFaultDescribe.Clear();
            Motor_Warning = 0;
            Motor_Fault = 0;
            WarnningDescribe.BackColor = Color.SpringGreen;
            FaultDescribe.BackColor = Color.SpringGreen;
            ControlFaultDescribe.BackColor = Color.SpringGreen;
            label46.Text = "告警码：0";
            label35.Text = "故障码：0";
            label86.Text = "整车故障码：0";
            timer_send.Enabled = false;
            if (loadDBC)
            {
                //PC_TestCmd
                DBCMessage msg =dbc.GetDBCMessageById(PC_TestCmd,m_hDBC);
                msg.vSignals[1].nValue = debugSpd;
                msg.vSignals[2].nValue = DebugFeedBackTest.Checked == true ? 1 : 0;
                msg.vSignals[5].nValue = 1;
                msg.vSignals[6].nValue = debugSet2;
                msg.vSignals[7].nValue = debugSet1;
                msg.vSignals[8].nValue = DebugModeChooseTest.SelectedIndex;
                dbc.DBCSendMsg(msg, m_hDBC);
            }
            PackPCTestCmd();
            timer_send.Enabled = true;
        }

        private void EmergencyStop_Click(object sender, EventArgs e)
        {
            //转矩转速清零
            SetTrqSend.Value = 0;
            SetSpdSend.Value = 0;
            setSpd = 0;
            TestParameters1.Value = 0;
            TestParameters2.Value = 0;
            VehicleCtrMode.SelectedIndex = 2;
            EnableMotor.SelectedIndex = DebugModeChooseTest.SelectedIndex = EPTMode.SelectedIndex = 0;
            //发送0转速指令
            if (loadDBC)
            {
                timer_send.Enabled = false;
                byte[] msgSend = new byte[8];
                msgSend[0] = 0xA0;
                msgSend[1] = (byte)(setSpd >> 8);
                msgSend[2] = (byte)(setSpd & 0xff);
                Device.sendCanMsg(send_can, Device.DevType, Device.CanID, motorID, msgSend, 3);
                timer_send.Enabled = true;
            }
        }

        private void DebugDeltaTest_TextChanged(object sender, EventArgs e)
        {
            DebugSetPata1Test.Increment = Convert.ToInt16(DebugDeltaTest.Text);
            TestParameters1.Increment = Convert.ToInt16(DebugDeltaTest.Text);
            TestParameters2.Increment = Convert.ToInt16(DebugDeltaTest.Text);
        }

        private void bt_SingleSave_Click(object sender, EventArgs e)
        {
            try
            {
                //创建记录EXCEL
                if (bt_AutoSave.Text == "自动保存") //当前未进行保存动作时
                {
                    autoSaveFlg = false;
                    //根据日期创建数据保存文件夹路径
                    Dirpath = System.IO.Path.Combine((Application.StartupPath + @"\DataSave\3KW_PUMP\"), DateTime.Now.ToString("yyyyMMdd"));
                    //弹出Excel保存路径设置
                    ExcelPathNameInit msgInit = new ExcelPathNameInit(SaveExcelMsgInit,Dirpath);
                    msgInit.Show();

                }
                else   //当excel文件已创立并记录了数据，接着往下记录
                {
                    SaveExcelMsg(0);
                }
            }
            catch (Exception ex)
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip(ex.Message);
            }
        }

        private void DebugModeChooseTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            DebugSetPata1Test.Value = 0;
            TestParameters1.Value = 0;
            TestParameters2.Value = 0;
            switch (DebugModeChooseTest.SelectedIndex)
            {

                case 1:
                    DebugSetPata1Test.Enabled = false;
                    TestParameters1.Enabled = true;
                    TestParameters2.Enabled = true;
                    label38.Text = "Ualfa";
                    label39.Text = "Beta";
                    break;
                case 2:
                    DebugSetPata1Test.Enabled = false;
                    TestParameters1.Enabled = true;
                    TestParameters2.Enabled = true;
                    label38.Text = "Ud";
                    label39.Text = "Uq";
                    break;
                case 3:
                    DebugSetPata1Test.Enabled = false;
                    TestParameters1.Enabled = true;
                    TestParameters2.Enabled = true;
                    label38.Text = "Id";
                    label39.Text = "Iq";
                    break;
                default:
                    DebugSetPata1Test.Enabled = true;
                    TestParameters1.Enabled = true;
                    TestParameters2.Enabled = true;
                    label38.Text = "设定Ua";
                    label39.Text = "设定Ub";
                    break;
            }

        }

        private void btn_connectTCP_Click(object sender, EventArgs e)
        {

            //定义接收数据的异步定时器
            timer_RecTCP = new System.Threading.Timer(ReceiveTcpMsg, null, Timeout.Infinite, 200);
            //定义发送数据的异步定时器
            timer_SendTCP = new System.Threading.Timer(SendTcpMsg, null, Timeout.Infinite, 200);

            try
            {
                #region ZLG功率分析仪
                if (TcpDeviceSelect.SelectedIndex == 0)
                {
                    if (btn_connectTCP.Text == "连接")
                    {

                        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        //连接到的目标IP
                        IPAddress ip = IPAddress.Parse(cbxIP.SelectedText);
                        //IPAddress ip = IPAddress.Any;
                        //连接到目标IP的哪个应用(端口号！)
                        point = new IPEndPoint(ip, int.Parse(cbxPort.SelectedText));
                        //连接到服务器
                        client.Connect(point);
                        UIPage ui = new UIPage();
                        ui.ShowWarningTip("连接ZLG功率分析仪成功");
                        ConnectFlag = true;      //连接标志位
                        //向TCP服务器发送测量数据个数,一共每次测量14项
                        string msg = ":NUMERIC:NORMAL:NUMBER 14;";
                        byte[] buffer = Encoding.UTF8.GetBytes(msg);
                        client.Send(buffer);
                        //向TCP服务器发送测量数据项目   
                        string msgnew = ":NUMERIC:NORMAL:ITEM1 SPEed;ITEM2 TORQue;ITEM3 P,4,Total;ITEM4 P,SIGMA,Total;ITEM5 PM;ITEM6 ETA1;ITEM7 ETA2;ITEM8 ETA3;ITEM9 U,4,Total;ITEM10 I,4,Total;ITEM11 URMS,1;ITEM12 IRMS,1;ITEM13 Q,SIGMA,Total;ITEM14 LAMBda,SIGMA,Total;";
                        byte[] buffernew = Encoding.UTF8.GetBytes(msgnew);
                        client.Send(buffernew);
                        //启动发送异步定时器
                        timer_SendTCP.Change(0, 100);
                        timer_RecTCP.Change(0, 100);
                        btn_connectTCP.Text = "断开";
                        btn_connectTCP.Symbol = 61453;

                    }
                    else
                    {
                        //停止发送异步定时器
                        client.Disconnect(true);
                        recFlag = false;
                        ConnectFlag = false;
                        timer_RecTCP.Change(Timeout.Infinite, 200);
                        timer_SendTCP.Change(Timeout.Infinite, 200);
                        client.Dispose();
                        btn_connectTCP.Text = "连接";
                        btn_connectTCP.Symbol = 61452;
                        UIPage ui = new UIPage();
                        ui.ShowErrorTip("关闭连接");

                    }
                }
                #endregion

                #region 横河功率分析仪
                if (TcpDeviceSelect.SelectedIndex == 2)
                {
                    if (btn_connectTCP.Text == "连接")
                    {
                        connection.devAddress = cbxIP.SelectedItem.ToString();
                        connection.wireType = 8;
                        if (connection.Initialize() == 0)
                        {
                            SendItemSettings();
                            ConnectFlag = true;      //连接标志位
                            recFlag = true;
                            //启动接收异步定时器
                            timer_RecTCP.Change(0, 100);
                            btn_connectTCP.Text = "断开";
                            btn_connectTCP.Symbol = 61453;
                            UIPage ui = new UIPage();
                            ui.ShowWarningTip("横河功率分析仪连接成功！");
                        }

                    }
                    else
                    {
                        //停止发送异步定时器
                        connection.Finish();
                        recFlag = false;
                        ConnectFlag = false;
                        timer_RecTCP.Change(Timeout.Infinite, 200);
                        timer_SendTCP.Change(Timeout.Infinite, 200);
                        btn_connectTCP.Text = "连接";
                        btn_connectTCP.Symbol = 61452;
                        UIPage ui = new UIPage();
                        ui.ShowWarningTip("关闭连接");

                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip(ex.Message);
            }

        }

        private void TcpDeviceSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxIP.SelectedIndex = cbxPort.SelectedIndex = TcpDeviceSelect.SelectedIndex;

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            SetSpdSend.Increment = Convert.ToInt32(textBox6.Text);//更改转速递增值
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            SetTrqSend.Increment = Convert.ToInt32(textBox5.Text);//更改转矩递增值
        }

        private void SetSpdSend_ValueChanged(object sender, EventArgs e)
        {
            setSpd = (Int16)SetSpdSend.Value;

        }

        private void SetTrqSend_ValueChanged(object sender, EventArgs e)
        {
            setTorque = (Int16)SetTrqSend.Value;
        }

        private void ControlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSpdSend.Value = SetTrqSend.Value = DebugSetPata1Test.Value = TestParameters1.Value = TestParameters2.Value = 0;
            switch (EPTMode.SelectedIndex)
            {
                case 0:
                    DebugModeChooseTest.SelectedIndex = 0;
                    textBox6.Enabled = SetSpdSend.Enabled = groupBox2.Enabled = true;
                    textBox5.Enabled = SetTrqSend.Enabled = groupBox2.Enabled = true;
                    groupBox2.Enabled = true;
                    break;
                case 4:
                    textBox5.Enabled = SetTrqSend.Enabled = groupBox2.Enabled = false;
                    break;
                case 5:
                    textBox6.Enabled = SetSpdSend.Enabled = groupBox2.Enabled = false;
                    break;
                case 6:
                    groupBox2.Enabled = true;
                    break;
                default:
                    groupBox2.Enabled = false;
                    break;

            }
        }

        private void DebugSetPata1Test_ValueChanged(object sender, EventArgs e)
        {
            debugSpd = (UInt16)DebugSetPata1Test.Value;
        }

        private void TestParameters1_ValueChanged(object sender, EventArgs e)
        {
            debugSet1 = (Int16)TestParameters1.Value;
        }

        private void TestParameters2_ValueChanged(object sender, EventArgs e)
        {
            debugSet2 = (Int16)TestParameters2.Value;
        }
        private void LoadDBC_Click(object sender, EventArgs e)
        {
            loadDBC = false;
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "DBC Files(*.dbc)|*.dbc";
            fileDlg.RestoreDirectory = true;
            fileDlg.InitialDirectory = Application.StartupPath+@"\DbcFiles\";
            fileDlg.FilterIndex = 1;

            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                UIPage ui = new UIPage();
                ui.ShowSuccessTip(dbc.LoadDbcFile(fileDlg.FileName, m_hDBC));
            }
            loadDBC = true ;
        }


        private void DebugFeedBackTest_CheckedChanged(object sender, EventArgs e)
        {

            if (loadDBC)
            {
                //PC_TestCmd
                DBCMessage msg = dbc.GetDBCMessageById(PC_TestCmd,m_hDBC);
                msg.vSignals[1].nValue = debugSpd;
                msg.vSignals[2].nValue = DebugFeedBackTest.Checked == true ? 1 : 0;
                msg.vSignals[6].nValue = debugSet2;
                msg.vSignals[7].nValue = debugSet1;
                msg.vSignals[8].nValue = DebugModeChooseTest.SelectedIndex;
                dbc.DBCSendMsg(msg, m_hDBC);
            }

        }
        private void ChangeID_Click(object sender, EventArgs e)
        {
            //修改ID
            if (VehicleCtrMode.SelectedIndex == 4)
            {
                timer_send.Enabled = false;
                //电机ID的内容
                List<string> items = new List<string>() { "电机1", "电机2", "电机3", "电机4" };
                int index = 2;
                if (this.ShowSelectDialog(ref index, items, "电机ID选择", "请选择需要修改的电机ID:"))
                {
                    byte[] idChange = new byte[8];
                    idChange[0] = 0xb0;
                    idChange[1] = 0x05;
                    idChange[2] = (byte)(index + 97);
                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, motorID, idChange, 3);
                    UIPage ui = new UIPage();
                    ui.ShowSuccessTip("选择修改的电机ID是：0x" + ((idChange[1] << 8) + idChange[2]).ToString("X3"));
                }
                timer_send.Enabled = true;
            }
            //读取ID
            if (VehicleCtrMode.SelectedIndex == 3)
            {
                timer_send.Enabled = false;
                byte[] msgSend = new byte[8];
                msgSend[0] = 0xE1;
                msgSend[1] = 0x1E;
                Device.sendCanMsg(send_can, Device.DevType, Device.CanID, motorID, msgSend, 2);
                timer_send.Enabled = true;
            }


        }

        private void MotorNum_SelectedIndexChanged(object sender, EventArgs e)
        {

            motorID = (uint)(1377 + MotorNum.SelectedIndex);
        }

        #endregion


        #region 自定义调用的方法

        /// <summary>
        /// 获取横河功率分析仪数据初始化设置
        /// </summary>
        void SendItemSettings()
        {

            int rtn;

            ///----------------------#set ASCII/Float(Binary)#
            rtn = connection.Send(":NUMERIC:FORMAT ASCII");
            if (rtn != 0)
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip("格式设定失败！");
                return;
            }

            ///----------------------#set items number#

            rtn = connection.Send(":NUMERIC:NORMAL:NUMBER 14 ");
            if (rtn != 0)
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip("数量设定失败！");
                return;
            }

            ///----------------------#send message detail#

            string msg = ":NUMERIC:NORMAL:ITEM1 SPEed;ITEM2 TORQue;ITEM3 P,4,Total;ITEM4 P,SIGMA,Total;ITEM5 PM;ITEM6 ETA1;ITEM7 ETA2;ITEM8 ETA3;ITEM9 U,4,Total;ITEM10 I,4,Total;ITEM11 URMS,1;ITEM12 IRMS,1;ITEM13 Q,SIGMA,Total;ITEM14 LAMBda,SIGMA,Total;";

            //SetSendMonitor(msg);
            rtn = connection.Send(msg);
            if (rtn != 0)
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip("项目设定失败！");
                return;
            }
        }

        void GetItemData()
        {
            int rtn;
            string msg;

            ///----------------------#get data#
            msg = ":NUMERIC:NORMAL:VALUE?";
            ///----------------------#send message#
            //###ASCII:TmcSend(); FLOAT:TmcSendBuLength()###
            rtn = connection.Send(msg);
            if (rtn != 0)
            {
                return;
            }

            ///----------------------#receive values#
            int maxLength = 0;
            int realLength = 0;
            string data = "";

            ///----------------------#receive values by ASCII#
            //###ASCII:TmcReceive()###
            maxLength = 15 * 15;
            rtn = connection.Receive(ref data, maxLength, ref realLength);
            if (rtn != 0)
            {
                return;
            }
            //分割提取数据
            string[] dataArray = data.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            BeginInvoke(new Action(() =>
            {

                for (int i = 0; i < dataArray.Length; i++)
                {
                    double recdata;
                    if (double.TryParse(dataArray[i], out recdata))
                    {
                        tb[i].Text = recdata.ToString();
                    }
                    else
                    {
                        tb[i].Text = dataArray[i];
                    }

                }

            }));
        }

        /// <summary>
        /// 将dbc句柄传给子模块
        /// </summary>
        /// <returns></returns>
        uint ReturnDBCHandle()
        {
            return m_hDBC;
        }

        /// <summary>
        /// 根据DBCMessage的single值，读取对应的tab_value的定义
        /// </summary>
        /// <param name="m_hdbc">DBC句柄</param>
        /// <param name="id">Message的id</param>
        /// <param name="sig">single的名称(全程)</param>
        /// <param name="selctnum">DBCMessage的single值，例如是3</param>
        string GetDBCMessageValDescPair(uint m_hdbc, uint id, string sig, int selctnum)
        {
            uint PairCount = IDBCManager.Method.DBC_GetValDescPairCount(m_hdbc, id, sig);
            if (PairCount > 0)
            {
                ValDescPair desc = new ValDescPair();
                ValDescPair[] descPair = new ValDescPair[PairCount];

                int size = Marshal.SizeOf(typeof(ValDescPair));
                IntPtr ptrPair = Marshal.AllocHGlobal((int)PairCount * size);

                Marshal.StructureToPtr(desc, ptrPair, true);
                IDBCManager.Method.DBC_GetValDescPair(m_hDBC, id, sig, ptrPair);
                for (int j = 0; j < PairCount; j++)
                {
                    descPair[PairCount - j - 1] = (ValDescPair)Marshal.PtrToStructure(
                        (IntPtr)((UInt32)ptrPair + j * size), typeof(ValDescPair));

                }
                Marshal.FreeHGlobal(ptrPair);
                return Encoding.GetEncoding("gb2312").GetString(descPair[selctnum].desc).SplitFirst("\0");
            }
            else
            {
                return "读取valu_Table错误";

            }

        }

   

        /// <summary>
        /// 故障检测
        /// </summary>
        public void EroCheck()
        {
            if (Motor_Fault != 0)
            {
                FaultDescribe.BackColor = Color.Red;
                string fault = Convert.ToString(Motor_Fault, 2);
                char[] faultnum = fault.ToCharArray();
                Array.Reverse(faultnum);
                for (int i = 0; i < faultnum.Length; i++)
                {
                    if (faultnum[i] != '0')
                    {
                        switch (i)
                        {
                            case 0:
                                ShowFaultMsg("母线欠压!", FaultDescribe);
                                break;
                            case 1:
                                ShowFaultMsg("母线过压!", FaultDescribe);
                                break;
                            case 2:
                                ShowFaultMsg("弱电欠压!", FaultDescribe);
                                break;
                            case 3:
                                ShowFaultMsg("弱电过压!", FaultDescribe);
                                break;
                            case 4:
                                ShowFaultMsg("硬件相电流过流!", FaultDescribe);
                                break;
                            case 5:
                                ShowFaultMsg("软件相电流过流!", FaultDescribe);
                                break;
                            case 6:
                                ShowFaultMsg("D/Q轴过流故障!", FaultDescribe);
                                break;
                            case 7:
                                ShowFaultMsg("硬件故障!", FaultDescribe);
                                break;
                            case 8:
                                ShowFaultMsg("W相输出故障!", FaultDescribe);
                                break;

                            case 9:
                                ShowFaultMsg("V相输出故障!", FaultDescribe);
                                break;

                            case 10:
                                ShowFaultMsg("U相输出故障!", FaultDescribe);
                                break;
                            case 11:
                                ShowFaultMsg("总IGBT故障!", FaultDescribe);
                                break;
                            case 12:
                                ShowFaultMsg("IGBT软件过热!", FaultDescribe);
                                break;
                            case 13:
                                ShowFaultMsg("电机过热!", FaultDescribe);
                                break;
                            case 14:
                                ShowFaultMsg("母线过流故障!", FaultDescribe);
                                break;
                            case 15:
                                ShowFaultMsg("硬件驱动欠压故障!", FaultDescribe);
                                break;
                            case 16:
                                ShowFaultMsg("电机堵转告警!", FaultDescribe);
                                break;
                            case 17:
                                ShowFaultMsg("电机超速故障!", FaultDescribe);
                                break;
                            case 18:
                                ShowFaultMsg("硬件过流故障!", FaultDescribe);
                                break;
                            case 19:
                                ShowFaultMsg("CAN通讯故障!", FaultDescribe);
                                break;
                            case 20:
                                ShowFaultMsg("旋变信号丢失!", FaultDescribe);
                                break;
                            case 21:
                                ShowFaultMsg("旋变信号降级!", FaultDescribe);
                                break;
                            case 22:
                                ShowFaultMsg("散热器过热!", FaultDescribe);
                                break;
                            case 23:
                                ShowFaultMsg("E2读写异常!", FaultDescribe);
                                break;
                            case 24:
                                ShowFaultMsg("硬件母线过压!", FaultDescribe);
                                break;
                            case 25:
                                ShowFaultMsg("IGBT硬件过热!", FaultDescribe);
                                break;
                            case 26:
                                ShowFaultMsg("CAN总线busoff!", FaultDescribe);
                                break;
                            case 27:
                                ShowFaultMsg("电流传感器偏置故障!", FaultDescribe);
                                break;
                            case 28:
                                ShowFaultMsg("漏水故障!", FaultDescribe);
                                break;
                            case 29:
                                ShowFaultMsg("IGBT_RAD_PTC_ERR_CODE", FaultDescribe);
                                break;
                            case 30:
                                ShowFaultMsg("RectifOver_Temp_ERR_CODE", FaultDescribe);
                                break;
                            case 31:
                                ShowFaultMsg("LACK_PHASE_CODE", FaultDescribe);
                                break;

                        }

                    }
                }

            }
            if (Motor_Warning != 0)
            {
                WarnningDescribe.BackColor = Color.Red;
                string warn = Convert.ToString(Motor_Warning, 2);
                char[] warnnum = warn.ToCharArray();
                Array.Reverse(warnnum);
                for (int i = 0; i < warnnum.Length; i++)
                {
                    if (warnnum[i] != '0')
                    {
                        switch (i)
                        {
                            case 0:
                                ShowFaultMsg("母线过压告警!", WarnningDescribe);
                                break;
                            case 1:
                                ShowFaultMsg("驱动板过温告警!", WarnningDescribe);
                                break;
                            case 2:
                                ShowFaultMsg("弱电欠压告警!", WarnningDescribe);
                                break;
                            case 3:
                                ShowFaultMsg("弱电过压告警!", WarnningDescribe);
                                break;
                            case 4:
                                ShowFaultMsg("CAN通讯告警!", WarnningDescribe);
                                break;
                            case 5:
                                ShowFaultMsg("超速告警!", WarnningDescribe);
                                break;
                            case 6:
                                ShowFaultMsg("母线欠压告警!", WarnningDescribe);
                                break;
                            case 7:
                                ShowFaultMsg("IGBT过温告警!", WarnningDescribe);
                                break;
                            case 8:
                                ShowFaultMsg("电机绕组温度过温告警!", WarnningDescribe);
                                break;

                            case 9:
                                ShowFaultMsg("电机堵转告警!", WarnningDescribe);
                                break;

                            case 10:
                                ShowFaultMsg("PTC_1_WARNING_CODE", WarnningDescribe);
                                break;

                        }
                    }

                }
            }
            if (ControlMotorFault != 0)
            {
                label86.Text = "整车故障码：" + ControlMotorFault.ToString();
                ControlFaultDescribe.BackColor = Color.Red;
                //根据不同的故障位判断
                for (int j = 0; j < controlFalut.Length; j++)
                {
                    string warn = Convert.ToString(controlFalut[j], 2);
                    char[] warnnum = warn.ToCharArray();
                    Array.Reverse(warnnum);
                    for (int i = 0; i < warnnum.Length; i++)
                    {
                        switch (j)
                        {
                            case 0:
                                if (warnnum[i] != '0')
                                {
                                    switch (i)
                                    {
                                        case 1:
                                            ShowFaultMsg("内部超速", ControlFaultDescribe);
                                            break;
                                        case 3:
                                            ShowFaultMsg("过流故障", ControlFaultDescribe);
                                            break;
                                        case 4:
                                            ShowFaultMsg("过压故障", ControlFaultDescribe);
                                            break;
                                        case 5:
                                            ShowFaultMsg("过热故障", ControlFaultDescribe);
                                            break;
                                        case 6:
                                            ShowFaultMsg("位置传感器故障", ControlFaultDescribe);
                                            break;
                                        case 7:
                                            ShowFaultMsg("漏水报警", ControlFaultDescribe);
                                            break;
                                    }

                                }
                                break;
                            case 1:
                                if (warnnum[i] != '0')
                                {
                                    switch (i)
                                    {

                                        case 6:
                                            ShowFaultMsg("Eeprom故障", ControlFaultDescribe);
                                            break;
                                        case 7:
                                            ShowFaultMsg("堵转", ControlFaultDescribe);
                                            break;
                                    }

                                }
                                break;
                            case 2:
                                if (warnnum[i] != '0')
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            ShowFaultMsg("弱电欠压", ControlFaultDescribe);
                                            break;
                                        case 1:
                                            ShowFaultMsg("弱电过压", ControlFaultDescribe);
                                            break;
                                        case 4:
                                            ShowFaultMsg("电机过热", ControlFaultDescribe);
                                            break;
                                        case 5:
                                            ShowFaultMsg("Igbt过热", ControlFaultDescribe);
                                            break;
                                        case 6:
                                            ShowFaultMsg("旋变信号降级", ControlFaultDescribe);
                                            break;
                                        case 7:
                                            ShowFaultMsg("旋变信号丢失", ControlFaultDescribe);
                                            break;
                                    }

                                }
                                break;
                            case 3:
                                if (warnnum[i] != '0')
                                {
                                    switch (i)
                                    {
                                        case 3:
                                            ShowFaultMsg("电机超速", ControlFaultDescribe);
                                            break;
                                        case 6:
                                            ShowFaultMsg("硬件相电流过流", ControlFaultDescribe);
                                            break;
                                        case 7:
                                            ShowFaultMsg("软件相电流过流", ControlFaultDescribe);
                                            break;
                                    }
                                }
                                break;

                        }


                    }
                }

            }

        }

        private void ShowFaultMsg(string msg, TextBox tb)
        {
            if (!tb.Text.Contains(msg))
            {
                tb.AppendText(msg + "\r\n");

            }
        }

        /// <summary>
        /// 打包发送PC端请求帧
        /// </summary>
        public void PackPCTestCmd()
        {
            if (loadDBC)
            {
                //状态查询
                byte[] conditionCheck = new byte[8];
                conditionCheck[0] = 0xf0;
                conditionCheck[1] = 0x0f;
                Device.sendCanMsg(send_can, Device.DevType, Device.CanID, motorID, conditionCheck, 2);

                //速度控制
                if (VehicleCtrMode.SelectedIndex == 1 && EnableMotor.SelectedIndex == 1)
                {
                    byte[] msgSend = new byte[8];
                    msgSend[0] = 0xA0;
                    msgSend[1] = (byte)(setSpd >> 8);
                    msgSend[2] = (byte)(setSpd & 0xff);
                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, motorID, msgSend, 3);
                }
                //PC_TestCmd
                if (DebugFeedBackTest.Checked)
                {
                    DBCMessage msg = dbc.GetDBCMessageById(PC_TestCmd, m_hDBC);
                    msg.vSignals[1].nValue = debugSpd;
                    msg.vSignals[2].nValue = DebugFeedBackTest.Checked == true ? 1 : 0;
                    msg.vSignals[6].nValue = debugSet2;
                    msg.vSignals[7].nValue = debugSet1;
                    msg.vSignals[8].nValue = DebugModeChooseTest.SelectedIndex;
                    dbc.DBCSendMsg(msg, m_hDBC);
                }

                //VCU_MCU
                DBCMessage msg_VCU = dbc.GetDBCMessageById(VCU_MCU,m_hDBC);
                msg_VCU.vSignals[0].nValue = MotorNum.SelectedIndex + 1;
                msg_VCU.vSignals[1].nValue = DisCharge.Checked == true ? 1 : 0;
                msg_VCU.vSignals[2].nValue = setSpd;
                msg_VCU.vSignals[3].nValue = LV_DNR.SelectedIndex;
                msg_VCU.vSignals[4].nValue = EnableMotor.SelectedIndex;
                msg_VCU.vSignals[5].nValue = setTorque;
                msg_VCU.vSignals[6].nValue = EPTMode.SelectedIndex;
                dbc.DBCSendMsg(msg_VCU,m_hDBC);

            }
        }


        /// <summary>
        /// 创建Excel并等待实时记录数据动作结束
        /// </summary>
        public void CreatExcel()
        {
            try
            {
                //新建book
                wkBook = new XSSFWorkbook();
                style = wkBook.CreateCellStyle();//声明style1对象，设置Excel表格的样式
                font = wkBook.CreateFont();
                font.Color = IndexedColors.Red.Index;
                style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//两端自动对齐（自动换行）
                //创建Excel工作表 Sheet=实时记录数据
                wkSheet = wkBook.CreateSheet(DateTime.Now.ToString("HHmmss"));
                //给Sheet1(实时记录数据）添加第一行的头部标题
                IRow row1 = wkSheet.CreateRow(0);
                //写入数据标题
                row1.CreateCell(0).SetCellValue("Time");
                for (int i = 0; i < textMsg.Count; i++)
                {
                    row1.CreateCell(i + 1).SetCellValue((string)textMsg[i].Tag);
                    row1.GetCell(i + 1).CellStyle = style;//初始化设置样式

                }
                //添加故障 告警标题
                row1.CreateCell(textMsg.Count).SetCellValue("故障");
                row1.GetCell(textMsg.Count).CellStyle = style;//初始化设置样式
                row1.CreateCell(textMsg.Count + 1).SetCellValue("告警");
                row1.GetCell(textMsg.Count + 1).CellStyle = style;//初始化设置样式

                if (autoSaveFlg)
                {
                    //启动记录异步定时器
                    timer_msgSave.Change(0, timerPeriod);
                }


            }
            catch (Exception ex)
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip(ex.Message);
            }

        }

        private static object locker = new object();
        /// <summary>
        /// 自定义多线程system Timer定时器事件，保存Excel数据
        /// </summary>
        /// <param name="sourec"></param>
        /// <param name="e"></param>
        void SaveExcelMsg(object sender)
        {
            //防止多线程出现安全问题
            lock (locker)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    //出现故障，第一时间先保存数据
                    EndSave();

                    //提示故障原因
                    UIPage ui = new UIPage();   
                    ui.ShowErrorTip(ex.Message);

                }
                row++;
                #region 重新写sheet
                ////如果超过固定行数，重新写入新的sheet
                //if (row > 6000)
                //{
                //    wkSheet = wkBook.CreateSheet(DateTime.Now.ToString("HHmmss"));
                //    //给Sheet1(实时记录数据）添加第一行的头部标题
                //    IRow row_title = wkSheet.CreateRow(0);
                //    //写入数据标题
                //    row_title.CreateCell(0).SetCellValue("Time");
                //    for (int i = 0; i < textMsg.Count; i++)
                //    {
                //        row_title.CreateCell(i + 1).SetCellValue((string)textMsg[i].Tag);
                //        row_title.GetCell(i + 1).CellStyle = style;//初始化设置样式

                //    }
                //    row = 1;
                //    BeginInvoke(new Action(() =>
                //    {
                //        msgProgressBar.Maximum = msgProgressBar.Maximum + 6000;
                //    }));

                //}
                #endregion

                #region 重新建Excel
                if (row > 6000)
                {
                    //结束记录保存数据

                    if (autoSaveFlg == true) //如果处于自动记录状态，执行这条
                    {
                        timer_msgSave.Change(Timeout.Infinite, timerPeriod);
                        //结束写入，保存数据
                        EndSave();

                    }
                    else //如果处于手动记录状态，执行这条
                    {
                        //结束写入，保存数据
                        EndSave();

                    }

                    //重新建立Excel表格

                    //初始化相关参数

                    BeginInvoke(new Action(() =>
                    {

                        filePathName = System.IO.Path.Combine((Application.StartupPath + @"\DataSave\"), (DateTime.Now.ToString("yyyyMMdd")), (DateTime.Now.ToString("HHmmss") + ".xlsx"));
                        //根据日期创建数据保存文件夹路径
                        Dirpath = System.IO.Path.Combine((Application.StartupPath + @"\DataSave\"), DateTime.Now.ToString("yyyyMMdd"));
                        Directory.CreateDirectory(Dirpath);
                        msgProgressBar.Maximum = msgProgressBar.Maximum + 6000;
                    }));

                    if (autoSaveFlg) //自动保存模式下
                    {
                        //新建多线程去执行创建EXCEL指令，并实时记录
                        Thread th = new Thread(CreatExcel);
                        th.IsBackground = true; //自定义线程定义成后台线程
                        th.Start();

                    }
                    else //单笔保存模式下
                    {
                        autoSaveFlg = false;
                        CreatExcel(); //新建book
                        SaveExcelMsg(0); //记录数据

                    }

                    return; //返回不执行下面

                }

                #endregion

                //建立数据记录行，他是一行一行记录的
                irow = wkSheet.CreateRow(row);
                //写入实时数据
                irow.CreateCell(0).SetCellValue(DateTime.Now.ToLocalTime().ToString("HH:mm:ss:fff"));
                for (int i = 1; i <= textMsg.Count; i++)
                {
                    //判断是否是double类型，不然保存会有报错

                    double data;
                    if (double.TryParse(textMsg[i - 1].Text, out data))
                    {
                        ICell cell = irow.CreateCell(i, CellType.Numeric);
                        cell.SetCellValue(data);
                        cell.CellStyle = style;//设置居中  
                    }
                    else
                    {
                        ICell cell = irow.CreateCell(i, CellType.String);
                        string msg = textMsg[i - 1].Text;
                        cell.SetCellValue(textMsg[i - 1].Text);
                        cell.CellStyle = style;//设置居中  
                    }

                }
                irow.CreateCell(textMsg.Count).SetCellValue(Motor_Fault); //记录故障
                irow.GetCell(textMsg.Count).CellStyle = style;//设置居中
                irow.CreateCell(textMsg.Count + 1).SetCellValue(Motor_Warning);
                irow.GetCell(textMsg.Count + 1).CellStyle = style;//设置居中
                msgNum++;
                BeginInvoke(new Action(() =>
                {
                    msgProgressBar.Value = msgNum;
                    toolStrip_info.Text = "正在记录数据，目前已记录" + msgNum + "笔....";
                }));


            }

        }

        void SaveExcelTime(object sender)
        {
            BeginInvoke(new Action(() =>
            {
                toolstrip_SaveTime.Text = "数据记录持续时间：" + uiLedWatch.Text;
            }));
        }

        /// <summary>
        /// 结束记录，保存Excel
        /// </summary>
        private void EndSave()
        {
            //保存间隔拉大，防止下次启动保存时，维持旧的间隔
            timerPeriod = 1000000;

            if (autoSaveFlg) //如果处于自动记录状态，执行这条
            {
                timer_msgSave.Dispose();
                uiLedWatch.Active = false;
               
                UIPage ui = new UIPage();
                ui.ShowSuccessTip("保存完毕,共自动记录数据" + msgNum + "笔！");
                toolStrip_info.Text = " 自动记录完毕，共记录数据" + msgNum.ToString() + "笔!";

                msgProgressBar.Visible = false;
            }
            else //如果处于手动记录状态，执行这条
            {
           
                UIPage ui = new UIPage();
                ui.ShowSuccessTip("保存完毕,共手动记录数据" + msgNum + "笔！");
                msgProgressBar.Visible = false;
                toolStrip_info.Text = " 手动记录完毕，共记录数据" + msgNum.ToString() + "笔";

            }
            uiLedWatch.Dispose();
            timer_SaveTime.Change(Timeout.Infinite, 100);
            timer_SaveTime.Dispose();
            //写入结束，更改按钮和提示
            bt_AutoSave.Symbol = 61674;
            bt_AutoSave.Text = "自动保存";

            //结束写入，保存数据
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(filePathName, FileMode.Create, FileAccess.Write))
                {
                    wkBook.Write(fs);
                }
            }
            wkBook.Close();
            row = 0;

        }


        /// <summary>
        /// 初始化并启动CAN通道
        /// </summary>
        /// <param name="m_devtype">CAN设备代码，例如USBCAN2就是4</param>
        /// <param name="channelbot">选择的CAN通道波特率</param>
        /// <param name="canind">通道索引号</param>
        /// <param name="config">通道参数组名</param>
        public void CanChanelInit(uint m_devtype, string channelbot, UInt32 canind, VCI_INIT_CONFIG config)
        {
            config.AccCode = Convert.ToUInt32("0x00000000", 16);
            config.AccMask = Convert.ToUInt32("0xFFFFFFFF", 16);
            config.Filter = 0;
            config.Mode = 0;
            switch (channelbot)
            {
                case "250Kbps":
                    config.Timing0 = 0x01;//波特率为250时，Timing0=01 Timing1=1C
                    config.Timing1 = 0x1c;
                    break;
                case "500Kbps":
                    config.Timing0 = 0x00;//波特率为500时，Timing0=00 Timing1=1C
                    config.Timing1 = 0x1c;
                    break;
                case "100Kbps":
                    config.Timing0 = 0x04;//波特率为100时，Timing0=04 Timing1=1C
                    config.Timing1 = 0x1c;
                    break;

            }
            switch (canind)
            {
                case 0:
                    if (Device.VCI_InitCAN(m_devtype, 0, 0, ref config) == 0)
                    {
                        UIPage ui = new UIPage();
                        ui.ShowWarningTip("初始化CAN1失败");
                    }
                    if (Device.VCI_StartCAN(m_devtype, 0, 0) == 1)  //启动CAN通道0
                    {
                        LedDebug.State = UILightState.On;
                    }
                    else
                    {
                        UIPage ui = new UIPage();
                        ui.ShowWarningTip("启动CAN1失败");
                    }
                    break;
                case 1:
                    if (Device.VCI_InitCAN(m_devtype, 0, 1, ref config) == 0)
                    {
                        UIPage ui = new UIPage();
                        ui.ShowWarningTip("初始化CAN2失败");
                    }
                    if (Device.VCI_StartCAN(m_devtype, 0, 1) == 1)  //启动CAN通道1
                    {
                        LedDebug.State = UILightState.On;
                    }
                    else
                    {
                        UIPage ui = new UIPage();
                        ui.ShowWarningTip("启动CAN2失败");
                    }
                    break;

            }

        }

        void SaveExcelMsgInit(string filepath, int time, List<int> list, bool cmd)
        {
            if (cmd)
            {
                //根据弹窗选择，添加保存数据
                textMsg.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    switch (list[i])
                    {
                        case 0:

                            foreach (Control item in gbxDebugMsg.Controls)
                            {
                                if (item is System.Windows.Forms.TextBox)
                                {
                                    textMsg.Add((TextBox)item);
                                }

                            }
                            break;
                        case 1:
                            foreach (Control item in gbxControlMsg.Controls)
                            {
                                if (item is System.Windows.Forms.TextBox)
                                {
                                    textMsg.Add((TextBox)item);
                                }
                            }
                            break;
                        case 2:
                            textMsg.AddRange(tb);
                            break;
                    }
                }
                filePathName = filepath;
                timerPeriod = time;
                //新建多线程去执行创建EXCEL指令，并实时记录
                Thread th = new Thread(CreatExcel);
                th.IsBackground = true; //自定义线程定义成后台线程
                th.Start();
                //修改保存按钮界面
                bt_AutoSave.Symbol = 61516;
                bt_AutoSave.Text = "停止记录";
                msgProgressBar.Visible = true;
                toolStrip_info.Text = "正在记录数据，目前已记录0笔....";
                //初始化相关参数
                row = 0;
                msgNum = 0;
                Directory.CreateDirectory(Dirpath);
                uiLedWatch = new UILedStopwatch();
                uiLedWatch.ShowType = UILedStopwatch.TimeShowType.hhmmss;
                uiLedWatch.Active = true;
                timer_SaveTime = new System.Threading.Timer(SaveExcelTime);
                timer_SaveTime.Change(0, 100);
            }
        }


        //接收测功机的消息 
        void ReceiveTcpMsg(object sender)
        {
            try
            {
                if (ConnectFlag && recFlag)
                {
                    byte[] buffer = new byte[1024 * 1024];
                    if (TcpDeviceSelect.SelectedIndex == 0)
                    {
                        int n = client.Receive(buffer);
                        //读取到的数据数组
                        string s = Encoding.UTF8.GetString(buffer, 0, n);
                        //分割提取数据
                        string[] msg = s.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                        BeginInvoke(new Action(() =>
                        {

                            for (int i = 0; i < tb.Length; i++)
                            {
                                tb[i].Text = Convert.ToDouble(msg[i]).ToString("F3");
                            }

                        }));

                        //接收数据标志取false，未接收到数据前不发送数据
                        recFlag = false;

                    }
                    else
                    {
                        GetItemData();
                    }
                }

            }
            catch (Exception ex)
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip(ex.Message);

            }


        }

        //发送给测功机的消息
        void SendTcpMsg(object sender)
        {
            //客户端给服务器发消息
            if (ConnectFlag && !recFlag)
            {
                //发送读取所有数据数据
                byte[] buffer = Encoding.UTF8.GetBytes(":NUMeric:NORMal:VALue?;");
                client.Send(buffer);
                recFlag = true;
            }

        }

        double GetSpsdValue()
        {
            return spd;
        }

        #endregion


        private void spdMeter_Click(object sender, EventArgs e)
        {
            Form fm = new Form();
            fm.Size = new System.Drawing.Size(800, 513);
            fm.Text = "实时曲线";
            fm.Controls.Add(new RealTimeSample());
            fm.Show();

        }

        private void MsgFrm_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}

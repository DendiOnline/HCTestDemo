using ControlCAN;
using IDBCManager;
using NPOI.HSSF.UserModel;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using Yokogawa.Tm.WT1800CommSample.cs;

namespace HCTestDemo
{
    public partial class HC_M2A_Pump : Form
    {
        #region 全局变量

        //接收登录界面方法的委托
        Action action;

        //定义USBCAN盒变量

        public UInt32 m_devind = 0;//CAN盒索引号
        public UInt32 ledcount_can = 0;//CAN接收数据计数
        VCI_CAN_OBJ[] send_can = new VCI_CAN_OBJ[50];//CAN发送数据数据
        Int16 setSpd; //控制转速
        Int16 setTorque;//控制转矩
        UInt16 debugSpd = 0;
        Int16 debugSet1 = 0;
        Int16 debugSet2 = 0;
        UInt32 Motor_Fault;
        int Motor_Warning;
        int ControlMotorFault;
        


        //CAN通讯的ID
        public const uint VCU_MCU = 0xCD04E48;
        public const uint Tx_MCU_1 = 0x561;
        public const uint Tx_MCU_2 = 0x562;
        public const uint Tx_MCU_3 = 0x563;
        public const uint Tx_MCU_4 = 0x564;
        public const uint Tx_MCU_5 = 0x565;
        public const uint Tx_MCU_6 = 0x566;
        public const uint Tx_MCU_7 = 0x567;
        public const uint Tx_MCU_8 = 0x18BAEEAA;
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
        double[] sj = new double[14];
        List<TextBox> textMsg = new List<TextBox>(); //所有待记录的数据窗口
        UILedStopwatch uiLedWatch;


        //TCP
        Socket client;
        IPEndPoint point;
        bool recFlag = false;
        bool ConnectFlag = false;
        TextBox[] tb = new TextBox[14];

        Connection connection = new Connection();

        //参数读写
        List<int> checkIndedx = new List<int>();
        int rownum = 0;
        bool ReadFlg = false;
        bool WriteFlg = false;

        //BootLoad
        int handShakeNum;
        uint sendNum; //发送ID
        uint recNum; //接收ID
        byte[] msg_Send = new byte[8];//发送的字节
        bool sendcmd = false; //发送标志
        private byte btSts = BOOTLOADER_STS_IDLE;
        public const byte BOOTLOADER_STS_IDLE = 0x0;//初始化完成
        public const byte BOOTLOADER_STS_CONNECT = 0x01;//握手
        public const byte BOOTLOADER_STS_ERASE_APP = 0x02;//擦除
        public const byte BOOTLOADER_STS_SEND_BIN = 0x03;//发送BIN指令
        public const byte BOOTLOADER_STS_PROGRAM = 0x04;//等待校验模式
        public const byte BOOTLOADER_STS_FINISH = 0x05;//结束指令

        byte[] binData = null;
        byte[] binDataBuf;

        UInt16 byteStep = 0;
        int loadStep = 0; //当前刷写的数据段数
        int BytePerStep = 0; //一段完整16K数据的字节大小
        int byteindex = 0; //发送的数据在源数组中的index
        private bool connectCmd = false;//发送标志位
        public static bool ComIsReceiving = false;


        //DBC
        uint m_hDBC = IDBCManager.Define.INVALID_DBC_HANDLE;
        bool loadDBC = false;
        DbcHelper dbc = new DbcHelper();


        //定时器
        System.Threading.Timer timer_msgSave;
        System.Threading.Timer timer_SendTCP;
        System.Threading.Timer timer_RecTCP;
        System.Threading.Timer timer_msgLoad; //执行下载数据的定时器
        System.Threading.Timer timer_SaveTime;
        #endregion
        public HC_M2A_Pump(Action action)
        {
            InitializeComponent();
            this.action = action;
        }
        public HC_M2A_Pump()
        {
            InitializeComponent();
           
        }

        #region 主通讯界面事件
        private void MainFrm_Load(object sender, EventArgs e)
        {


            //DBC导入初始化
            m_hDBC = dbc.InitialDbcHandler();

            //默认界面设置
            DeviceSlect.SelectedIndex = 3;
            ChannelSelect.SelectedIndex = 1;
            BotSelect.SelectedIndex = 6;
            EnableMotor.SelectedIndex = 0;
            MotorNum.SelectedIndex = 0;
            VehicleCtrMode.SelectedIndex = 2;
            textBox5.Text = "5";
            textBox6.Text = "100";
            cbxIP.SelectedIndex = 0;
            cbxPort.SelectedIndex = 0;
            TcpDeviceSelect.SelectedIndex = 0;
            ControlMode.SelectedIndex = 0;
            ControlSelcet.SelectedIndex = 0;
            DebugModeChooseTest.SelectedIndex = 0;
            toolStrip_info.Text = "欢迎使用" + this.Text + "测试版上位机~~";
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
                                spdMeter.Value = ((Int16)(rec_can0.Data[0] << 8) + (rec_can0.Data[1]));
                                ControlMotorFault = rec_can0.Data[2];
                                ControlSwVer.Text = rec_can0.Data[7].ToString();
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
                                FanIGBTTemp.Text = igbtTemp.ToString();
                                igtbtTempDebug.Text = igbtTemp.ToString();
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
                                Motor_Fault = (uint)msg.vSignals[2].nValue;
                                Motor_Warning = (int)msg.vSignals[1].nValue;
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

                                BatteryVolDebug.Text = msg.vSignals[2].nValue.ToString("F1");
                                ADC_BusVoltage.Text = msg.vSignals[1].nValue.ToString();
                                UInt16 SwVer = (UInt16)msg.vSignals[0].nValue;
                                UInt16 CompileData = (UInt16)msg.vSignals[3].nValue;
                                UInt16 C = (UInt16)((SwVer & 0x000F) >> 0);
                                UInt16 V = (UInt16)((SwVer & 0x00F0) >> 4);
                                UInt16 B = (UInt16)((SwVer & 0x0F00) >> 8);
                                UInt16 D = (UInt16)((SwVer & 0xF000) >> 12);
                                UInt16 Year = (UInt16)(((CompileData & 0xFE00) >> 9) + 2000);
                                UInt16 Mon = (UInt16)((CompileData & 0x01e0) >> 5);
                                UInt16 Day = (UInt16)((CompileData & 0x001f) >> 0);
                                statusStrip.Text = "编译日期:" + Year.ToString() + "年" + Mon.ToString() + "月" + Day.ToString() + "日" + "    " + "软件版本:" + C.ToString() + "." + V.ToString() + "." + B.ToString() + "." + D.ToString();
                                CompileDate.Text = Year.ToString() + "/" + Mon.ToString() + "/" + Day.ToString();
                                SW_Ver.Text = "V" + C.ToString() + "." + V.ToString() + "." + B.ToString() + "." + D.ToString();
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

                        #region 参数读写报文
                        //写入数据
                        if (dgvMsgList.RowCount != 0 && msg.nID == 0x11c)
                        {
                            if ((bool)(dgvMsgList.Rows[(int)msg.vSignals[1].nValue - 1].Cells["选择"]).Value == true)
                            {
                                double para = msg.vSignals[0].nValue;
                                double num = Convert.ToDouble(dgvMsgList.Rows[(int)msg.vSignals[1].nValue - 1].Cells["系数"].Value);
                                dgvMsgList.Rows[(int)msg.vSignals[1].nValue - 1].Cells["EPPROM存储默认值"].Value = para / num;
                            }
                        }

                        #endregion

                        #region BootLoad报文
                        if (rec_can0.ID == recNum && bt_LoadFile.Text != "下载")
                        {
                            #region 没有检测到强制报错帧
                            //没有强制报错帧
                            #region 判断是否握手状态
                            if (btSts == BOOTLOADER_STS_CONNECT)
                            {
                                //判断下位机回复的报文
                                if (rec_can0.Data[0] == 0xaa && rec_can0.Data[1] == 0x02)  //握手成功
                                {
                                    btSts = BOOTLOADER_STS_ERASE_APP;   //进入擦除请求模式
                                    timer_msgLoad.Change(0, 60000); //结束握手等待,计时器调整为1min一次
                                    processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  握手成功！\r\n");
                                    handShakeNum = 0;
                                }

                            }
                            #endregion
                            #region 判断是否擦除状态
                            else if (btSts == BOOTLOADER_STS_ERASE_APP)
                            {
                                //判断下位机回复的报文

                                if (rec_can0.Data[0] == 0xbc)
                                {
                                    double num = rec_can0.Data[1];
                                    double total = rec_can0.Data[2];
                                    double s = num / total;
                                    processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  第" + num.ToString() + "扇区擦除完成，" + "擦除进度" + s.ToString("p") + "\r\n");
                                }
                                if (rec_can0.Data[0] == 0xba && rec_can0.Data[1] == 0x03) //擦除成功
                                {
                                    processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  擦除完成！\r\n");
                                    processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  开始烧录...\r\n");
                                    btSts = BOOTLOADER_STS_SEND_BIN;   //进入烧录Bin请求模式
                                    timer_msgLoad.Change(0, 1000);  //结束擦除等待，计时器改为2s一次
                                    sendcmd = true;//开始准备发送BIN

                                }
                            }
                            #endregion
                            #region 判断是否烧录数据状态
                            else if (btSts == BOOTLOADER_STS_SEND_BIN)
                            {
                                //发送一段数据结束时
                                if (rec_can0.Data[0] == 0xdb && rec_can0.Data[1] == 0x05)
                                {

                                    btSts = BOOTLOADER_STS_SEND_BIN;   //保持持续烧录模式
                                    processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  第" + loadStep.ToString() + "扇区下载成功!\r\n");
                                    processMsg.AppendText("\r\n");
                                    processBar.Value = byteindex;
                                    label.Text = byteindex.ToString();
                                    //LoadMsg(0);//启动一次
                                    //sendcmd = true;
                                    timer_msgLoad.Change(0, 2500);

                                    sendcmd = true;


                                }
                                //发送最后一次结束<=4个字节时
                                if (rec_can0.Data[0] == 0xde && rec_can0.Data[1] == 0x07)
                                {
                                    processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  下位机数据烧录完成，可以停止BootLoad!\r\n");
                                    btSts = BOOTLOADER_STS_FINISH;   //跳到结束模式
                                    timer_msgLoad.Change(0, 2500);
                                    this.Enabled = true;
                                    bt_LoadBin.Enabled = true;
                                    bt_LoadFile.Enabled = true;
                                    sendcmd = false;
                                }


                                //发送最后一次结束>4个字节时(第二次发送)时
                                if (rec_can0.Data[0] == 0xdf && rec_can0.Data[1] == 0x08)
                                {

                                    processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  下位机数据烧录完成，可以停止BootLoad！\r\n");
                                    btSts = BOOTLOADER_STS_FINISH;   //跳到结束模式
                                    timer_msgLoad.Change(0, 2500);
                                    this.Enabled = true;
                                    bt_LoadBin.Enabled = true;
                                    bt_LoadFile.Enabled = true;
                                    sendcmd = false;
                                }

                            }
                            #endregion
                            #region 判断是否烧录结束状态
                            else if (btSts == BOOTLOADER_STS_FINISH)
                            {
                                //结束bootloader
                                if (rec_can0.Data[0] == 0xcc && rec_can0.Data[1] == 0x10)
                                {

                                    sendcmd = false;//停止传输
                                    processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  烧录结束，本次烧录成功!\r\n");
                                    timer_msgLoad.Change(Timeout.Infinite, 2000);//暂停下载数据的定时器
                                    bt_LoadFile.Text = "下载";
                                    bt_LoadFile.Symbol = 62033;
                                    filePath.Clear();
                                    DialogResult dr = MessageBox.Show("结束烧录,退出程序！", "完成提示", MessageBoxButtons.OK);
                                    if (dr == DialogResult.OK)
                                    {
                                        LoadMSGInit();
                                    }

                                }

                            }
                            #endregion


                            #endregion
                            #region 检测到强制报错帧
                            //数据错误,下位机要求上位机重发数据
                            if (rec_can0.Data[0] == 0xfe && rec_can0.Data[1] == 0x11)
                            {
                                processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  第" + loadStep.ToString() + "笔数据发送错误，退回请求握手状态\r\n");
                                loadStep = 0;   //烧录笔数清零
                                byteindex = 0;
                                processBar.Value = byteindex;
                                timer_msgLoad.Change(0, 3000);
                                btSts = BOOTLOADER_STS_CONNECT;//退回请求握手状态
                                connectCmd = true;

                            }
                            //数据错误,上位机要求下位机重发数据
                            else if (rec_can0.Data[0] == 0xff && rec_can0.Data[1] == 0x06)
                            {
                                processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  第" + loadStep.ToString() + "笔数据发送错误，退回请求握手状态\r\n");
                                loadStep = 0;   //烧录笔数清零
                                byteindex = 0;
                                processBar.Value = byteindex;
                                timer_msgLoad.Change(0, 3000);
                                btSts = BOOTLOADER_STS_CONNECT;//退回请求握手状态
                                connectCmd = true;
                            }
                            //数据传输中中断bin文件传输(上位机下发)
                            else if (rec_can0.Data[0] == 0xf0 && rec_can0.Data[1] == 0x07)
                            {
                                processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  Bin文件传输中断，退回请求握手状态\r\n");
                                loadStep = 0;   //烧录笔数清零
                                byteindex = 0;
                                processBar.Value = byteindex;
                                timer_msgLoad.Change(0, 3000);
                                btSts = BOOTLOADER_STS_CONNECT;//退回请求握手状态
                                connectCmd = true;
                            }
                            //数据传输中中断bin文件传输(上位机下发)
                            else if (rec_can0.Data[0] == 0xf0 && rec_can0.Data[1] == 0x13)
                            {
                                processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  Bin文件传输中断，退回请求握手状态\r\n");
                                loadStep = 0;   //烧录笔数清零
                                byteindex = 0;
                                processBar.Value = byteindex;
                                timer_msgLoad.Change(0, 3000);
                                btSts = BOOTLOADER_STS_CONNECT;//退回请求握手状态
                                connectCmd = true;

                            }
                            //下位机请求重新发送整段数据 !!!!!!
                            else if (rec_can0.Data[0] == 0x11 && rec_can0.Data[1] == 0x11)
                            {
                                processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  下位机要求重新发送整段数据\r\n");
                                loadStep = 0;   //烧录笔数清零
                                byteindex = 0;
                                processBar.Value = byteindex;
                                timer_msgLoad.Change(0, 3000);
                                btSts = BOOTLOADER_STS_CONNECT;//退回请求握手状态
                                connectCmd = true;
                            }

                            #endregion


                        }
                        #endregion

                        #region 读取反馈控制器ID
                        //查询设备ID反馈
                        if (rec_can0.ID == 0x18BAB0A1 && rec_can0.Data[1] == 1)
                        {
                            switch (rec_can0.Data[2])
                            {
                                case 1:
                                    if (uiLight1.State == UILightState.Off)
                                    {
                                        uiLight1.State = UILightState.On;
                                    }
                                    else
                                    {
                                        UIPage ui = new UIPage();
                                        ui.ShowWarningTip("1# ID存在重复！");
                                    }
                                    break;
                                case 2:
                                    if (uiLight2.State == UILightState.Off)
                                    {
                                        uiLight2.State = UILightState.On;
                                    }
                                    else
                                    {
                                        UIPage ui = new UIPage();
                                        ui.ShowWarningTip("2# ID存在重复！");
                                    }
                                    break;
                                case 3:
                                    if (uiLight3.State == UILightState.Off)
                                    {
                                        uiLight3.State = UILightState.On;
                                    }
                                    else
                                    {
                                        UIPage ui = new UIPage();
                                        ui.ShowWarningTip("3# ID存在重复！");
                                    }
                                    break;
                                case 4:
                                    if (uiLight4.State == UILightState.Off)
                                    {
                                        uiLight4.State = UILightState.On;
                                    }
                                    else
                                    {
                                        UIPage ui = new UIPage();
                                        ui.ShowWarningTip("4# ID存在重复！");
                                    }
                                    break;
                                default:
                                    UIPage up = new UIPage();
                                    up.ShowWarningTip("查询设备" + (rec_can0.Data[0] - 160) + "反馈错误，EEPROM值为" + rec_can0.Data[2].ToString());
                                    break;
                            }

                        }

                        #endregion

                        #region 电机零位&&模拟量反馈
                        if (rec_can0.ID == Tx_MCU_8)
                        {
                            //电机零位反馈
                            if (rec_can0.Data[0]==0xE0)
                            {
                                ResolverValue.Text = ((UInt16)(rec_can0.Data[3] << 8) + (rec_can0.Data[2])).ToString();
                                ResolverOffset.Text = ((UInt16)(rec_can0.Data[5] << 8) + (rec_can0.Data[4])).ToString();
                                EncoderMsgWrite.Text = ((UInt16)(rec_can0.Data[7] << 8) + (rec_can0.Data[6])).ToString();
                            }
                            //模拟量系数反馈
                            if (rec_can0.Data[3] == 0xA0)
                            {
                                analogK.Text = (((UInt16)(rec_can0.Data[5] << 8) + (rec_can0.Data[4])) * 0.000001).ToString();
                                analogB.Text = ((((UInt16)(rec_can0.Data[7] << 8) + (rec_can0.Data[6]))-30000) * 0.000001).ToString();
                            }
                            //ADC值反馈
                            if (rec_can0.Data[3] == 0xA1)
                            {
                                analogADC.Text = ((UInt16)((rec_can0.Data[5] << 8) + (rec_can0.Data[4]))).ToString();
                            }

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
            if (loadDBC)
            {
                PackPCTestCmd();
            }
            
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
                    Dirpath = System.IO.Path.Combine((Application.StartupPath + @"\DataSave\M2A_PUMP\"), DateTime.Now.ToString("yyyyMMdd"));
                    //定义记录数据的异步定时器
                    timer_msgSave = new System.Threading.Timer(SaveExcelMsg, null, Timeout.Infinite, 100);
                    //弹出Excel保存路径设置
                    ExcelPathNameInit msgInit = new ExcelPathNameInit(SaveExcelMsgInit, Dirpath);
                    msgInit.Show();

                }

                else  //结束保存动作
                {
                    //保存间隔拉大，防止下次启动保存时，维持旧的间隔
                    timerPeriod = 1000000;
                    if (autoSaveFlg) //如果处于自动记录状态，执行这条
                    {
                        timer_msgSave.Dispose();

                        //结束写入，保存数据
                        EndSave();
                        UIPage ui = new UIPage();
                        ui.ShowSuccessTip("保存完毕,共自动记录数据" + msgNum + "笔！");
                        toolStrip_info.Text = " 自动记录完毕，共记录数据" + msgNum.ToString() + "笔";
                    
                        msgProgressBar.Visible = false;
                    }
                    else //如果处于手动记录状态，执行这条
                    {
                        //结束写入，保存数据
                        EndSave();
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

            if (loadDBC)
            {
                //PC_TestCmd
                DBCMessage msg = dbc.GetDBCMessageById(PC_TestCmd,m_hDBC);
                msg.vSignals[1].nValue = debugSpd;
                msg.vSignals[2].nValue = DebugFeedBackTest.Checked == true ? 1 : 0;
                msg.vSignals[5].nValue = 1;
                msg.vSignals[6].nValue = debugSet2;
                msg.vSignals[7].nValue = debugSet1;
                msg.vSignals[8].nValue = DebugModeChooseTest.SelectedIndex;
                dbc.DBCSendMsg(msg,m_hDBC);
            }

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



        }

        private void EmergencyStop_Click(object sender, EventArgs e)
        {
            
            //转矩转速清零
            SetTrqSend.Value = 0;
            SetSpdSend.Value = 0;
            TestParameters1.Value = 0;
            TestParameters2.Value = 0;
            EnableMotor.SelectedIndex = DebugModeChooseTest.SelectedIndex = ControlMode.SelectedIndex = 0;
            //发送0转速指令
            if (loadDBC)
            {
                byte[] msgSend = new byte[8];
                msgSend[0] = 0xA0;
                msgSend[1] = (byte)(setSpd >> 8);
                msgSend[2] = (byte)(setSpd & 0xff);
                Device.sendCanMsg(send_can, Device.DevType, Device.CanID, motorID, msgSend, 3);
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
                    Dirpath = System.IO.Path.Combine((Application.StartupPath + @"\DataSave\M2A_PUMP\"), DateTime.Now.ToString("yyyyMMdd"));
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
                case 4:
                    DebugSetPata1Test.Enabled = false;
                    TestParameters1.Enabled = true;
                    TestParameters2.Enabled = true;
                    label38.Text = "Uu";
                    label39.Text = "Uv";
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
            switch (ControlMode.SelectedIndex)
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
            fileDlg.InitialDirectory = Application.StartupPath + @"\DbcFiles\";
            fileDlg.FilterIndex = 1;

            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                UIPage ui = new UIPage();
                ui.ShowSuccessTip(dbc.LoadDbcFile(fileDlg.FileName, m_hDBC));
            }
            loadDBC = true;
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
                dbc.DBCSendMsg(msg,m_hDBC);
            }

        }

        private void MotorNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            motorID = MotorNum.SelectedIndex == 3 ? 0x7FF :(uint)(1377 + MotorNum.SelectedIndex);
        }

        private void FaultCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (loadDBC)
            {
                //初始化发送数据
                DBCMessage msg = dbc.GetDBCMessageById(Tx_MCU_8,m_hDBC);
                for (int i = 0; i < msg.vSignals.Length; i++)
                {
                    msg.vSignals[i].nValue = FaultCheck.Checked == true ? 170 : 85;
                }
                Task.Run(() =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        dbc.DBCSendMsg(msg,m_hDBC);
                        Thread.Sleep(50);
                    }
                });

            }

        }

        private void ChangeResolverValue_Click_1(object sender, EventArgs e)
        {

            if (PcControlEnable.Checked&&VehicleCtrMode.SelectedIndex==0)
            {

                //电机零位的内容
                int value = 0;
                if (this.InputIntegerDialog(ref value, true, "修改的电机零位是："))
                {
                    byte[] resolverChange = new byte[8];
                    resolverChange[0] = 0xA1;
                    resolverChange[1] = MotorNum.SelectedIndex == 3 ? (byte)15 : (byte)(MotorNum.SelectedIndex + 1);
                    resolverChange[2] = (byte)(value & 0xff);
                    resolverChange[3] = (byte)(value >> 8);
                    resolverChange[4] = 0xFF;
                    resolverChange[5] = 0xFF;
                    resolverChange[6] = 0xFF;
                    resolverChange[7] = 0xFF;
                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, Tx_MCU_8, resolverChange, 8);
                    UIPage ui = new UIPage();
                    ui.ShowSuccessTip("修改的电机零位是：" + value.ToString());
                }

            }
        }

        private void ChangeID_Click(object sender, EventArgs e)
        {
            //修改ID
            if (PcControlEnable.Checked && VehicleCtrMode.SelectedIndex == 0)
            {
                //电机ID的内容
                List<string> items = new List<string>() { "电机1", "电机2", "电机3", "模拟量" };
                int index = 2;
                if (this.ShowSelectDialog(ref index, items, "电机ID选择", "请选择需要修改的电机ID:"))
                {
                    byte[] idChange = new byte[8];
                    if (index != 3)
                    {
                        idChange[0] = 0xb0;
                        idChange[1] = 0x05;
                        idChange[2] = (byte)(index + 97);
                    }
                    else
                    {
                        idChange[0] = 0xb0;
                        idChange[1] = 0x07;
                        idChange[2] = 0xff;

                    }
                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, motorID, idChange, 3);
                    UIPage ui = new UIPage();
                    ui.ShowSuccessTip("选择修改的电机ID是：0x" + ((idChange[1] << 8) + idChange[2]).ToString("X3"));
                }

            }

        }

        private void SetAnalog_Click(object sender, EventArgs e)
        {


            try
            {
                if (PcControlEnable.Checked)
                {
                    //模拟量设定
                    if (VehicleCtrMode.SelectedIndex != 5)
                    {
                        AnalogKB ana = new AnalogKB(SendKBValue);
                        ana.Show();

                    }
                    else
                    {
                        //模拟量查询
                        byte[] msgSend = new byte[8];
                        msgSend[0] = 0xAB;
                        msgSend[1] = MotorNum.SelectedIndex == 3 ? (byte)15 : (byte)(MotorNum.SelectedIndex + 1);
                        msgSend[2] = 0xE0;
                        msgSend[3] = 0xE0;
                        msgSend[4] = 0xE0;
                        msgSend[5] = 0xE0;
                        msgSend[6] = 0xE0;
                        msgSend[7] = 0xE0;
                        Device.sendCanMsg(send_can, Device.DevType, Device.CanID, Tx_MCU_8, msgSend, 8);
                    }
                }
            }
            catch (Exception ex)
            {

                UIPage ui = new UIPage();
                ui.ShowSuccessTip(ex.Message);
            }
        }

        private void analogK_DoubleClick(object sender, EventArgs e)
        {
            LinearReg li = new LinearReg();
            li.Show();
        }

        #endregion


        #region BootLoad操作事件

        private void ControlSelcet_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (ControlSelcet.SelectedIndex)
            {
                case 0:
                    recNum = 0x18BAB1A1;
                    sendNum = 0x18BAB1A0;
                    break;
                case 1:
                    recNum = 0x18BAB2A1;
                    sendNum = 0x18BAB2A0;
                    break;
                case 2:
                    recNum = 0x18BAB3A1;
                    sendNum = 0x18BAB3A0;
                    break;
                case 3:
                    recNum = 0x18BAB4A1;
                    sendNum = 0x18BAB4A0;
                    break;
            }
        }

        private void CheckID_Click(object sender, EventArgs e)
        {
            UILight[] light = new UILight[4] { uiLight1, uiLight2, uiLight3, uiLight4 };
            foreach (UILight item in light)
            {
                item.State = UILightState.Off;
            }
            byte[] checkid = new byte[8];
            checkid[1] = 0x02;
            checkid[2] = 0xaa;
            checkid[3] = 0xff;
            checkid[4] = 0xff;
            checkid[5] = 0xff;
            checkid[6] = 0xff;
            checkid[7] = 0xff;
            for (int i = 1; i <= 4; i++)
            {
                checkid[0] = (byte)(160 + i);
                Device.sendCanMsg(send_can, Device.DevType, Device.CanID, 0x18BAB0A0, checkid, 8);
            }

        }

        private void WriteID_Click(object sender, EventArgs e)
        {
            //电机ID的内容
            List<string> items = new List<string>() { "电机1", "电机2", "电机3", "电机4" };
            int index = 0;
            //根据选择修改ID
            if (this.ShowSelectDialog(ref index, items, "电机ID选择", "请选择需要修改的电机ID:"))
            {
                byte[] writeID = new byte[8];
                writeID[0] = (byte)(160 + ControlSelcet.SelectedIndex + 1);
                writeID[2] = (byte)(index + 1);
                writeID[1] = 0x01;
                writeID[3] = 0xff;
                writeID[4] = 0xff;
                writeID[5] = 0xff;
                writeID[6] = 0xff;
                writeID[7] = 0xff;
                Device.sendCanMsg(send_can, Device.DevType, Device.CanID, 0x18BAB0A0, writeID, 8);
            }

        }
        private void bt_LoadBin_Click(object sender, EventArgs e)
        {
            processMsg.Clear();
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "Bin文件|*.bin";
            fileDlg.RestoreDirectory = true;
            fileDlg.InitialDirectory = Application.StartupPath + @"\BIN";
            fileDlg.FilterIndex = 1;

            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                filePath.Text = fileDlg.FileName;
                try
                {
                    FileStream fs = new FileStream(fileDlg.FileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    binDataBuf = br.ReadBytes((int)fs.Length);
                    binData = new byte[binDataBuf.Length];
                    Array.Copy(binDataBuf, binData, binDataBuf.Length);
                    fs.Close();
                    processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  导入Bin文件成功\r\n");
                    timer_msgLoad = new System.Threading.Timer(LoadMsg, null, Timeout.Infinite, 100); //定义下载数据的异步定时器
                }
                catch (Exception e1)
                {
                    UIPage ui = new UIPage();
                    ui.ShowWarningTip(e1.Message);
                }

                processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  数据长度为" + binData.Length.ToString() + "字节\r\n");
                processBar.Maximum = binData.Length;
                label.Text = byteindex.ToString();
            }
        }

        private void bt_LoadFile_Click(object sender, EventArgs e)
        {

            if (bt_LoadFile.Symbol == 62033)
            {
                if (Device.m_bOpen == 1)
                {
                    if (binData != null && filePath.Text != "")
                    {
                        //初始化
                        btSts = BOOTLOADER_STS_IDLE;
                        BytePerStep = Convert.ToInt32(BytePerSize.Text);
                        btSts = BOOTLOADER_STS_CONNECT; //更改状态为准备握手
                        processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  准备完成...\r\n");
                        //初始化发送设置
                        connectCmd = true;
                        byteindex = 0;
                        loadStep = 0;
                        //bt_LoadBin.Enabled = false;
                        //bt_LoadFile.Enabled = false;
                        //this.Enabled = false;
                        timer_msgLoad.Change(0, 50);//启动下载定时器
                        bt_LoadFile.Symbol = 62034;
                        bt_LoadFile.Text = "停止下载";

                    }
                    else
                    {
                        UIPage ui = new UIPage();
                        ui.ShowWarningTip("未加载Bin文件");

                    }
                }
                else
                {
                    UIPage ui = new UIPage();
                    ui.ShowWarningTip("CAN盒尚未连接,请返回通讯页连接CAN盒！");

                }
            }
            else
            {
                DialogResult result = MessageBox.Show("确定要停止烧录?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    LoadMSGInit();
                }

            }
            timer_rec.Enabled = connectCmd == true ? true : false;

        }

       


        #endregion

        #region 自定义调用的方法


        void SendKBValue(double k, double b)
        {
            byte[] analogChange = new byte[8];
            analogChange[0] = 0xAB;
            analogChange[1] = MotorNum.SelectedIndex == 3 ? (byte)15 : (byte)(MotorNum.SelectedIndex + 1);
            analogChange[2] = 0xCC;
            analogChange[3] = 0xCC;
            analogChange[4] = (byte)((Int16)(k * 1000000) & 0xff);
            analogChange[5] = (byte)((Int16)(k * 1000000) >> 8);
            analogChange[6] = (byte)((Int16)(b * 1000000 + 30000) & 0xff);
            analogChange[7] = (byte)((Int16)(b * 1000000 + 30000) >> 8);
            Device.sendCanMsg(send_can, Device.DevType, Device.CanID, Tx_MCU_8, analogChange, 8);
            UIPage ui = new UIPage();
            ui.ShowSuccessTip("模拟量设定的是：" + k.ToString("f6") + "和" + b.ToString("f6"));
        }

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
        /// 读取发送参数循环指令帧
        /// </summary>
        /// <param name="sender"></param>
        void ParaReadWriteCmd()
        {
            
            try
            {
                for (int i = 0; i < checkIndedx.Count; i++)
                {
                    if (ReadFlg)
                    {
                        if ((bool)(dgvMsgList.Rows[checkIndedx[i]].Cells["选择"]).Value == true)
                        {
                            DBCMessage msgRead = dbc.GetDBCMessageById(0x11E,m_hDBC);
                            msgRead.vSignals[3].nValue = Convert.ToDouble(dgvMsgList.Rows[checkIndedx[i]].Cells["参数序号"].Value);
                            msgRead.vSignals[1].nValue = 1;
                            dbc.DBCSendMsg(msgRead,m_hDBC);
                            BeginInvoke(new Action(() =>
                            {
                                uiRoundProcess1.Value = i + 1;
                            }));
                        }
                    }
                    else if (WriteFlg)
                    {
                        if ((bool)(dgvMsgList.Rows[checkIndedx[i]].Cells["选择"]).Value == true)
                        {
                            Int32 paracoef = Convert.ToInt32(dgvMsgList.Rows[checkIndedx[i]].Cells["系数"].Value);
                            double paravalue = Convert.ToDouble(dgvMsgList.Rows[checkIndedx[i]].Cells["EPPROM存储默认值"].Value);
                            Int32 para = Convert.ToInt32(paravalue * paracoef);

                            DBCMessage msgWrite = dbc.GetDBCMessageById(0x11E,m_hDBC);
                            msgWrite.vSignals[0].nValue = 1;
                            msgWrite.vSignals[2].nValue = para;
                            msgWrite.vSignals[3].nValue = Convert.ToDouble(dgvMsgList.Rows[checkIndedx[i]].Cells["参数序号"].Value);
                            dbc.DBCSendMsg(msgWrite,m_hDBC);

                            BeginInvoke(new Action(() =>
                            {
                                uiRoundProcess1.Value = i + 1;
                            }));
                        }

                    }
                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip(ex.Message);
            }


        }


        /// <summary>
        /// 初始化下载动作，并清空数据数组
        /// </summary>
        private void LoadMSGInit()
        {
            connectCmd = false;
            sendcmd = false;
            timer_msgLoad.Change(Timeout.Infinite, 3000);//暂停下载定时器
            btSts = BOOTLOADER_STS_CONNECT; //更改状态为准备握手
            byteStep = 0;
            loadStep = 0; //当前刷写的数据段数
            byteindex = 0; //发送的数据在源数组中的index
            //清空并初始化数组
            Array.Clear(binData, 0, binData.Length);
            binData = null;
            Array.Clear(binDataBuf, 0, binDataBuf.Length);
            binDataBuf = null;
            bt_LoadFile.Symbol = 62033;
            bt_LoadFile.Text = "下载";
            processBar.Value = 0;
            label.Text = "0";
            filePath.Clear();
            processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  停止下载...\r\n");
        }


        /// <summary>
        /// 下载数据定时器
        /// </summary>
        /// <param name="sender"></param>
        void LoadMsg(object sender)
        {

            switch (btSts) //判断状态
            {
                #region 开始握手
                case BOOTLOADER_STS_CONNECT:   //开始握手

                    if (connectCmd && handShakeNum < 20)
                    {
                        //握手数据初始化
                        InitMsg();
                        msg_Send[0] = 0xaa;
                        msg_Send[1] = 0x01;
                        Device.sendCanMsg(send_can, Device.DevType, Device.CanID, sendNum, msg_Send, 8);
                        BeginInvoke(new Action(() =>
                        {
                            handShakeNum++;//递增握手次数
                            processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  连接请求中...\r\n");
                        }
                            ));
                        timer_msgLoad.Change(50, 50);//延时50ms等待

                    }
                    else
                    {
                        timer_msgLoad.Change(Timeout.Infinite, 500);//暂停定时器
                        BeginInvoke(new Action(() =>
                        {
                            processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  握手未成功...\r\n");
                        }
                            ));
                        handShakeNum = 0;
                    }
                    break;
                #endregion

                #region 开始擦除
                case BOOTLOADER_STS_ERASE_APP:  //开始擦除

                    if (connectCmd)
                    {
                        //擦除数据初始化
                        InitMsg();
                        msg_Send[0] = 0xba;
                        msg_Send[1] = 0x02;
                        Device.sendCanMsg(send_can, Device.DevType, Device.CanID, sendNum, msg_Send, 8);

                        BeginInvoke(new Action(() =>
                        {
                            processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  擦除请求中...\r\n");
                        }
                            ));
                        timer_msgLoad.Change(60000, 60000);//延时1min等待擦除

                    }
                    else
                    {
                        BeginInvoke(new Action(() =>
                        {
                            processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  擦除动作失败...\r\n");
                        }
                        ));

                    }

                    break;
                #endregion

                #region 发送数据模式

                case BOOTLOADER_STS_SEND_BIN: //发送数据模式

                    if (connectCmd == true && byteindex + 1 <= binData.Length) //发送连接OK且未发送完所有的Bin数据
                    {
                        //数据正好16k倍数
                        if (binData.Length % BytePerStep == 0)
                        {
                            //发送除了最后一段16k的数据
                            if (loadStep < binData.Length / BytePerStep - 1)
                            {
                                LoadStepMsg();
                            }
                            //开始发送最后一段16K数据时
                            else if (loadStep == binData.Length / BytePerStep - 1)
                            {
                                int num = (BytePerStep - 4) / 7;//剩下的最后16K数据发送需要的帧数
                                //先发送前面能完整7个字节的数据
                                for (int i = 1; i <= num; i++)
                                {
                                    LoadbyteMSG(0xdc);
                                    byteStep += 7;
                                }
                                //再发最后4个字节
                                InitMsg();
                                msg_Send[0] = 0xde;
                                msg_Send[1] = binData[byteindex];
                                msg_Send[2] = binData[byteindex + 1];
                                msg_Send[3] = binData[byteindex + 2];
                                msg_Send[4] = binData[byteindex + 3];
                                byteindex += 4;
                                byteStep += 4;

                                //int step =loadStep ; //一共发了多少帧含完整7字节的帧数量
                                msg_Send[5] = (byte)(Convert.ToUInt16(4) & 0x00ff);  //发送最后几个个字节的这帧中，含几个有效字节
                                msg_Send[6] = (byte)(Convert.ToUInt16(byteStep) >> 8); //发送完整7字节的帧数量（高8位）
                                msg_Send[7] = (byte)(Convert.ToUInt16(byteStep) & 0x00ff);//发送完整7字节帧的数量（低8位）

                                Device.sendCanMsg(send_can, Device.DevType, Device.CanID, sendNum, msg_Send, 8);

                                connectCmd = false;
                                BeginInvoke(new Action(() =>
                                {
                                    processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  最终扇区请求..\r\n");
                                }));

                                timer_msgLoad.Change(Timeout.Infinite, 2000);//暂停定时器

                            }

                        }
                        else //数据大小不是16K的整数倍
                        {
                            //数据大于16K
                            if (binData.Length > BytePerStep)
                            {
                                //发送除了最后一段所有的16k的数据
                                if (loadStep < (binData.Length - binData.Length % BytePerStep) / BytePerStep)
                                {
                                    LoadStepMsg();
                                }
                                //发送小于16K的部分
                                else if (loadStep == (binData.Length - binData.Length % BytePerStep) / BytePerStep)
                                {
                                    //897658
                                    int num = ((binData.Length - loadStep * BytePerStep) - (binData.Length - loadStep * BytePerStep) % 7) / 7;//发送完整7字节的帧数量

                                    //发送前面完整的7个字节数据的帧数
                                    for (int i = 1; i <= num; i++)
                                    {
                                        LoadbyteMSG(0xdc);
                                        byteStep += 7;
                                    }
                                    //最后一次结束<=4个字节
                                    if ((binData.Length - loadStep * BytePerStep) % 7 <= 4)
                                    {
                                        int bynum = 0;
                                        InitMsg();
                                        msg_Send[0] = 0xde;
                                        for (int i = 0; i < (binData.Length - loadStep * BytePerStep) % 7; i++) //剩余不足4个字节的数据进行遍历填充
                                        {
                                            msg_Send[i + 1] = binData[byteindex];
                                            byteindex++;
                                            byteStep++;
                                            bynum++;
                                        }
                                        //byteindex--;
                                        for (int i = bynum + 1; i < 5; i++) //无字节数据的地方，用ff替代
                                        {
                                            msg_Send[i + 1] = 0xff;
                                        }
                                        msg_Send[5] = (byte)(Convert.ToUInt16(bynum) & 0x00ff);  //发送最后几个个字节的这帧中，含几个有效字节
                                        msg_Send[6] = (byte)(Convert.ToUInt16(byteStep) >> 8); //发送完整7字节的数据的数量（高8位）
                                        msg_Send[7] = (byte)(Convert.ToUInt16(byteStep) & 0x00ff);//发送完整7k的数据的数量（低8位）
                                        //发送最后一帧
                                        Device.sendCanMsg(send_can, Device.DevType, Device.CanID, sendNum, msg_Send, 8);
                                        connectCmd = false;
                                        BeginInvoke(new Action(() =>
                                        {
                                            processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  最终扇区请求..\r\n");
                                        }));
                                        timer_msgLoad.Change(Timeout.Infinite, 2000);//暂停定时器
                                    }

                                   //最后一次结束>4个字节
                                    else if ((binData.Length - loadStep * BytePerStep) % 7 > 4)
                                    {
                                        //loadStep++;
                                        int bynum2 = 0;
                                        //第一次发送
                                        InitMsg();
                                        msg_Send[0] = 0xdf;
                                        int a = (binData.Length - loadStep * BytePerStep) % 7;
                                        for (int i = 0; i < a; i++) //剩余不足7个字节的数据进行遍历填充
                                        {
                                            msg_Send[i + 1] = binData[byteindex];///
                                            byteindex++;
                                            bynum2++;
                                            byteStep++;
                                        }

                                        for (int i = bynum2 + 1; i < 8; i++) //无字节数据的地方，用ff替代
                                        {
                                            msg_Send[i] = 0xff;
                                        }

                                        //发送一帧
                                        Device.sendCanMsg(send_can, Device.DevType, Device.CanID, sendNum, msg_Send, 8);

                                        connectCmd = false;
                                        timer_msgLoad.Change(Timeout.Infinite, 2000);//暂停定时器


                                        //第二次发送
                                        InitMsg();
                                        msg_Send[0] = 0xdf;
                                        msg_Send[1] = (byte)(Convert.ToUInt16(bynum2) & 0x00ff);  //发送最后几个个字节的这帧中，含几个有效字节;
                                        msg_Send[2] = (byte)(Convert.ToUInt16(byteStep) >> 8); //发送完整7字节的数据的数量（高8位）
                                        msg_Send[3] = (byte)(Convert.ToUInt16(byteStep) & 0x00ff);//发送完整7k的数据的数量（低8位）
                                        //发送最后一帧
                                        Device.sendCanMsg(send_can, Device.DevType, Device.CanID, sendNum, msg_Send, 8);
                                        BeginInvoke(new Action(() =>
                                        {

                                            processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  最终扇区请求..\r\n");
                                        }));
                                        timer_msgLoad.Change(Timeout.Infinite, 2000);//暂停定时器
                                    }

                                }
                            }
                            //数据小于16k
                            else
                            {
                                int num = ((binData.Length - loadStep * BytePerStep) - (binData.Length - loadStep * BytePerStep) % 7) / 7;//发送完整7字节的帧数量

                                //发送前面完整的7个字节数据的帧数
                                for (int i = 1; i <= num; i++)
                                {
                                    LoadbyteMSG(0xdc);
                                    byteStep += 7;
                                }
                                //最后一次结束<=4个字节
                                if ((binData.Length - loadStep * BytePerStep) % 7 <= 4)
                                {
                                    int bynum = 0;
                                    InitMsg();
                                    msg_Send[0] = 0xde;
                                    for (int i = 0; i < (binData.Length - loadStep * BytePerStep) % 7; i++) //剩余不足4个字节的数据进行遍历填充
                                    {
                                        msg_Send[i + 1] = binData[byteindex];
                                        byteindex++;
                                        byteStep++;
                                        bynum++;
                                    }
                                    //byteindex--;
                                    for (int i = bynum + 1; i < 5; i++) //无字节数据的地方，用ff替代
                                    {
                                        msg_Send[i + 1] = 0xff;
                                    }
                                    msg_Send[5] = (byte)(Convert.ToUInt16(bynum) & 0x00ff);  //发送最后几个个字节的这帧中，含几个有效字节
                                    msg_Send[6] = (byte)(Convert.ToUInt16(byteStep) >> 8); //发送完整7字节的数据的数量（高8位）
                                    msg_Send[7] = (byte)(Convert.ToUInt16(byteStep) & 0x00ff);//发送完整7k的数据的数量（低8位）
                                    //发送最后一帧
                                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, sendNum, msg_Send, 8);
                                    connectCmd = false;
                                    BeginInvoke(new Action(() =>
                                    {
                                        processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  最终扇区请求..\r\n");
                                    }));
                                    timer_msgLoad.Change(Timeout.Infinite, 2000);//暂停定时器
                                }

                               //最后一次结束>4个字节
                                else if ((binData.Length - loadStep * BytePerStep) % 7 > 4)
                                {
                                    //loadStep++;
                                    int bynum2 = 0;
                                    //第一次发送
                                    InitMsg();
                                    msg_Send[0] = 0xdf;
                                    int a = (binData.Length - loadStep * BytePerStep) % 7;
                                    for (int i = 0; i < a; i++) //剩余不足7个字节的数据进行遍历填充
                                    {
                                        msg_Send[i + 1] = binData[byteindex];///
                                        byteindex++;
                                        bynum2++;
                                        byteStep++;
                                    }

                                    for (int i = bynum2 + 1; i < 8; i++) //无字节数据的地方，用ff替代
                                    {
                                        msg_Send[i] = 0xff;
                                    }

                                    //发送一帧
                                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, sendNum, msg_Send, 8);

                                    connectCmd = false;
                                    timer_msgLoad.Change(Timeout.Infinite, 2000);//暂停定时器


                                    //第二次发送
                                    InitMsg();
                                    msg_Send[0] = 0xdf;
                                    msg_Send[1] = (byte)(Convert.ToUInt16(bynum2) & 0x00ff);  //发送最后几个个字节的这帧中，含几个有效字节;
                                    msg_Send[2] = (byte)(Convert.ToUInt16(byteStep) >> 8); //发送完整7字节的数据的数量（高8位）
                                    msg_Send[3] = (byte)(Convert.ToUInt16(byteStep) & 0x00ff);//发送完整7k的数据的数量（低8位）
                                    //发送最后一帧
                                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, sendNum, msg_Send, 8);
                                    BeginInvoke(new Action(() =>
                                    {

                                        processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  最终扇区请求..\r\n");
                                    }));
                                    timer_msgLoad.Change(Timeout.Infinite, 2000);//暂停定时器
                                }

                            }

                        }

                    }
                    else
                    {
                        BeginInvoke(new Action(() =>
                        {
                            processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  未连接...\r\n");
                        }
                        ));
                    }
                    break;
                #endregion


                #region 请求结束
                case BOOTLOADER_STS_FINISH: //结束发送
                    InitMsg();
                    msg_Send[0] = 0xcc;
                    msg_Send[1] = 0x05;
                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, sendNum, msg_Send, 8);
                    BeginInvoke(new Action(() =>
                    {
                        processBar.Value = byteindex;//进度条数据
                        label.Text = byteindex.ToString();
                        processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  请求结束BootLoad..\r\n");
                    }));
                    timer_msgLoad.Change(3000, 3000);   //延时3S等待下位机
                    break;

                #endregion

            }

        }


        /// <summary>
        /// 发送完整7个字节的数据帧
        /// </summary>
        /// <param name="msg">发送的数据ID</param>
        private void LoadbyteMSG(byte msg)
        {
            if (sendcmd)
            {
                InitMsg();
                msg_Send[0] = msg;
                msg_Send[1] = binData[byteindex];
                msg_Send[2] = binData[byteindex + 1];
                msg_Send[3] = binData[byteindex + 2];
                msg_Send[4] = binData[byteindex + 3];
                msg_Send[5] = binData[byteindex + 4];
                msg_Send[6] = binData[byteindex + 5];
                msg_Send[7] = binData[byteindex + 6];
                Device.sendCanMsg(send_can, Device.DevType, Device.CanID, sendNum, msg_Send, 8);
                byteindex += 7;//烧录的数据index位步进7
                BeginInvoke(new Action(() =>
                {
                    processBar.Value = byteindex;//进度条数据
                    label.Text = byteindex.ToString();
                }));

            }
        }


        /// <summary>
        /// 发送非最后一段16K数据
        /// </summary>
        /// <param name="msg">根据最后一段和非最后一段选择</param>

        private void LoadStepMsg()
        {
            if (sendcmd)
            {
                loadStep++;
                timer_msgLoad.Change(Timeout.Infinite, 2000);//暂停定时器
                int num = (BytePerStep - 4) / 7; //因为当数据正好为16K整数倍，每次发送7个帧，正好剩余4个数据
                int endstep = 0;//剩余的段数
                if (binData.Length % BytePerStep == 0)
                {
                    endstep = binData.Length / BytePerStep - loadStep;
                }
                else
                {
                    endstep = (binData.Length - binData.Length % BytePerStep) / BytePerStep + 1 - loadStep;
                }
                //每个数据段前面n个7字节发送
                for (int j = 0; j < num; j++)
                {
                    if (connectCmd)
                    {
                        LoadbyteMSG(0xda);
                    }
                    else
                    {
                        loadStep = 0;
                        byteindex = 0;
                        processBar.Value = byteindex;
                        return;
                    }

                }
                //一段数据结束最后4个字节发送
                byte[] head = new byte[8];
                head[0] = 0xdb;
                head[1] = binData[byteindex];
                head[2] = binData[byteindex + 1];
                head[3] = binData[byteindex + 2];
                head[4] = binData[byteindex + 3];
                head[5] = (byte)(Convert.ToUInt16(endstep) & 0x00ff); //剩余的数据段数目
                byteindex += 4;//BIN数组的index递增4
                Device.sendCanMsg(send_can, Device.DevType, Device.CanID, sendNum, head, 8);

                BeginInvoke(new Action(() =>
                {
                    //byteindex--;
                    processBar.Value = byteindex;//进度条数据
                    processMsg.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss ") + "  第" + loadStep.ToString() + "扇区请求..\r\n");
                    label.Text = byteindex.ToString();
                }
            ));

                timer_msgLoad.Change(Timeout.Infinite, 2000);//暂停定时器
                sendcmd = false;

            }

        }

        void InitMsg()
        {
            msg_Send[0] = 0x00;
            msg_Send[1] = 0x00;
            msg_Send[2] = 0x00;
            msg_Send[3] = 0x00;
            msg_Send[4] = 0x00;
            msg_Send[5] = 0x00;
            msg_Send[6] = 0x00;
            msg_Send[7] = 0x00;
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
                                ShowFaultMsg("UDC_UV_CODE", FaultDescribe);
                                break;
                            case 1:
                                ShowFaultMsg("UDC_OV_CODE", FaultDescribe);
                                break;
                            case 2:
                                ShowFaultMsg("V12_UV_CODE", FaultDescribe);
                                break;
                            case 3:
                                ShowFaultMsg("V12_OV_CODE", FaultDescribe);
                                break;
                            case 4:
                                ShowFaultMsg("HW_OC_CODE", FaultDescribe);
                                break;
                            case 5:
                                ShowFaultMsg("SOFT_OC_CODE", FaultDescribe);
                                break;
                            case 6:
                                ShowFaultMsg("IDQ_OC_CODE", FaultDescribe);
                                break;
                            case 7:
                                ShowFaultMsg("HW_ERR_CODE", FaultDescribe);
                                break;
                            case 8:
                                ShowFaultMsg("IGBT_W_ERR_CODE", FaultDescribe);
                                break;

                            case 9:
                                ShowFaultMsg("IGBT_V_ERR_CODE", FaultDescribe);
                                break;

                            case 10:
                                ShowFaultMsg("IGBT_U_ERR_CODE", FaultDescribe);
                                break;
                            case 11:
                                ShowFaultMsg("IGBT_SHORT_ERR_CODE", FaultDescribe);
                                break;
                            case 12:
                                ShowFaultMsg("IGBT_OVER_HEAT_U_CODE", FaultDescribe);
                                break;
                            case 13:
                                ShowFaultMsg("MOTOR_OT_FAULT", FaultDescribe);
                                break;
                            case 14:
                                ShowFaultMsg("IDC_OC_ERR_CODE", FaultDescribe);
                                break;
                            case 15:
                                ShowFaultMsg("IGBT_DRIVE_UV_ERR_CODE", FaultDescribe);
                                break;
                            case 16:
                                ShowFaultMsg("STALL_ERR_CODE", FaultDescribe);
                                break;
                            case 17:
                                ShowFaultMsg("OS_ERR_CODE", FaultDescribe);
                                break;
                            case 18:
                                ShowFaultMsg("HW_OC_ERR_CODE", FaultDescribe);
                                break;
                            case 19:
                                ShowFaultMsg("CAN_CHIP_ERR_CODE", FaultDescribe);
                                break;
                            case 20:
                                ShowFaultMsg("LOT_ERR_CODE", FaultDescribe);
                                break;
                            case 21:
                                ShowFaultMsg("LOS_ERR_CODE", FaultDescribe);
                                break;
                            case 22:
                                ShowFaultMsg("BOARD_TEMP_ERR_CODE", FaultDescribe);
                                break;
                            case 23:
                                ShowFaultMsg("EEPROM_ERR_CODE", FaultDescribe);
                                break;
                            case 24:
                                ShowFaultMsg("UDC_HW_OV_ERR_CODE", FaultDescribe);
                                break;
                            case 25:
                                ShowFaultMsg("IGBT_HW_OT_ERR_CODE", FaultDescribe);
                                break;
                            case 26:
                                ShowFaultMsg("CAN_BUSOFF_ERR_CODE", FaultDescribe);
                                break;
                            case 27:
                                ShowFaultMsg("CURR_OFFSET_ERR_CODE", FaultDescribe);
                                break;
                            case 28:
                                ShowFaultMsg("MOTOR_PTC_ERR_CODE", FaultDescribe);
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
                                ShowFaultMsg("UDC_OV_WARNING_CODE", WarnningDescribe);
                                break;
                            case 1:
                                ShowFaultMsg("BOARD_OT_WARNING_CODE", WarnningDescribe);
                                break;
                            case 2:
                                ShowFaultMsg("V12_UV_WARNING_CODE", WarnningDescribe);
                                break;
                            case 3:
                                ShowFaultMsg("V12_OV_WARNING_CODE", WarnningDescribe);
                                break;
                            case 4:
                                ShowFaultMsg("CAN_COMM_WARNING_CODE", WarnningDescribe);
                                break;
                            case 5:
                                ShowFaultMsg("OS_WARNING_CODE", WarnningDescribe);
                                break;
                            case 6:
                                ShowFaultMsg("UDC_UV_WARNING_CODE", WarnningDescribe);
                                break;
                            case 7:
                                ShowFaultMsg("IGBT_OT_WARNING_CODE", WarnningDescribe);
                                break;
                            case 8:
                                ShowFaultMsg("MOTOR_OT_WARNING_CODE", WarnningDescribe);
                                break;

                            case 9:
                                ShowFaultMsg("STALL_WARNING_CODE", WarnningDescribe);
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
                string warn = Convert.ToString(ControlMotorFault, 2);
                char[] warnnum = warn.ToCharArray();
                Array.Reverse(warnnum);
                for (int i = 0; i < warnnum.Length; i++)
                {
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
            if (PcControlEnable.Checked)
            {
                //状态查询
                byte[] conditionCheck = new byte[8];
                conditionCheck[0] = 0xf0;
                conditionCheck[1] = 0x0f;
                Device.sendCanMsg(send_can, Device.DevType, Device.CanID, motorID, conditionCheck, 2);

                //速度控制
                if ((ControlMode.SelectedIndex == 4 || VehicleCtrMode.SelectedIndex == 1) && EnableMotor.SelectedIndex == 1)
                {
                    byte[] msgSend = new byte[8];
                    msgSend[0] = 0xA0;
                    msgSend[1] = (byte)(setSpd >> 8);
                    msgSend[2] = (byte)(setSpd & 0xff);
                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, motorID, msgSend, 3);
                }
                //查询电机零位
                if (VehicleCtrMode.SelectedIndex == 4)
                {
                    byte[] resolverCheck = new byte[8];
                    resolverCheck[0] = 0xA0;
                    resolverCheck[1] = MotorNum.SelectedIndex == 3 ? (byte)15 : (byte)(MotorNum.SelectedIndex + 1); 
                    resolverCheck[2] = 0xFF;
                    resolverCheck[3] = 0xFF;
                    resolverCheck[4] = 0xFF;
                    resolverCheck[5] = 0xFF;
                    resolverCheck[6] = 0xFF;
                    resolverCheck[7] = 0xFF;
                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, Tx_MCU_8, resolverCheck, 8);
                }

                //读取ID
                if (VehicleCtrMode.SelectedIndex == 3)
                {
                    byte[] msgSend = new byte[8];
                    msgSend[0] = 0xE1;
                    msgSend[1] = 0x1E;
                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, motorID, msgSend, 2);
                   
                }
                //模拟量ADC查询
                byte[] checkanalog = new byte[8];
                checkanalog[0] = 0xAB;
                checkanalog[1] = MotorNum.SelectedIndex == 3 ? (byte)15 : (byte)(MotorNum.SelectedIndex + 1);
                checkanalog[2] = 0xE1;
                checkanalog[3] = 0xE1;
                checkanalog[4] = 0xE1;
                checkanalog[5] = 0xE1;
                checkanalog[6] = 0xE1;
                checkanalog[7] = 0xE1;
                Device.sendCanMsg(send_can, Device.DevType, Device.CanID, Tx_MCU_8, checkanalog, 8);

                //PC_TestCmd
                if (DebugFeedBackTest.Checked)
                {
                    DBCMessage msg = dbc.GetDBCMessageById(PC_TestCmd,m_hDBC);
                    msg.vSignals[1].nValue = debugSpd;
                    msg.vSignals[2].nValue = DebugFeedBackTest.Checked == true ? 1 : 0;
                    msg.vSignals[6].nValue = debugSet2;
                    msg.vSignals[7].nValue = debugSet1;
                    msg.vSignals[8].nValue = DebugModeChooseTest.SelectedIndex;
                    dbc.DBCSendMsg(msg,m_hDBC);

                }
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
            lock (locker)
            {
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
                        irow.CreateCell(i).SetCellValue(data);
                        irow.GetCell(i).CellStyle = style;//设置居中  
                    }
                    else
                    {
                        string msg = textMsg[i - 1].Text;
                        irow.CreateCell(i).SetCellValue(textMsg[i - 1].Text);

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

        /// <summary>
        /// 结束记录，保存Excel
        /// </summary>
        private void EndSave()
        {
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

        void SaveExcelTime(object sender)
        {
            BeginInvoke(new Action(() =>
            {
                toolstrip_SaveTime.Text = "数据记录持续时间：" + uiLedWatch.Text;
            }));
        }
        #endregion


        #region 参数读写界面事件

        private void chooseFile_Click(object sender, EventArgs e)
        {
            if (bt_AutoSave.Text != "停止记录")
            {
                try
                {
                    OpenFileDialog fileDlg = new OpenFileDialog();
                    fileDlg.Filter = "Excel文件|*.xlsx|Excel文件|*.xls";
                    fileDlg.RestoreDirectory = true;
                    fileDlg.InitialDirectory = Application.StartupPath + @"\ParaFile";
                    fileDlg.FilterIndex = 1;
                    int cloumnNum = 5; //读取EXCEL数据的列数
                    if (fileDlg.ShowDialog() == DialogResult.OK)
                    {
                        excelPath.Text = fileDlg.FileName;
                        using (FileStream fs = File.OpenRead(excelPath.Text))
                        {
                            //这里需要根据文件名格式判断一下
                            //HSSF只能读取xls的
                            //XSSF只能读取xlsx格式的
                            if (Path.GetExtension(fs.Name) == ".xls")
                            {
                                wkBook = new HSSFWorkbook(fs);
                            }
                            else if (Path.GetExtension(fs.Name) == ".xlsx")
                            {
                                wkBook = new XSSFWorkbook(fs);
                            }
                            //先清零datagridview的行数
                            dgvMsgList.ClearRows();
                            //得到当前sheet
                            wkSheet = wkBook.GetSheetAt(0);

                            //添加表头,这一段比较通用
                            irow = wkSheet.GetRow(wkSheet.FirstRowNum);
                            for (int k = irow.FirstCellNum; k < cloumnNum; k++)
                            {

                                dgvMsgList.AddColumn(irow.Cells[k].ToString(), irow.Cells[k].ToString()).SetFixedMode(150);

                            }
                            dgvMsgList.AddCheckBoxColumn("选择", "选择").SetFixedMode(80);


                            //也可以通过GetSheet(name)得到
                            //遍历表中所有的行
                            //注意这里加1，这里得到的最后一个单元格的索引默认是从0开始的
                            for (int j = wkSheet.FirstRowNum + 1; j <= 48; j++)
                            {

                                //得到当前的行
                                irow = wkSheet.GetRow(j);
                                //判断是否需要添加不是空的数据行
                                if (irow.Cells[1].ToString() != "")
                                {
                                    dgvMsgList.AddRow();
                                    //遍历每行所有的单元格
                                    //注意这里不用加1，这里得到的最后一个单元格的索引默认是从1开始的
                                    for (int k = irow.FirstCellNum; k < cloumnNum; k++)
                                    {
                                        //得到当前单元格
                                        ICell cell = irow.GetCell(k, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                        if (cell.ToString() != "")
                                        {
                                            dgvMsgList.Rows[j - 1].Cells[k].Value = cell.ToString();
                                            //初始化所有的checkbox状态为true
                                            ((DataGridViewCheckBoxCell)dgvMsgList.Rows[j - 1].Cells["选择"]).Value = true;
                                            //初始化所有的单元格状态的只读模式改为false
                                            dgvMsgList.Rows[j - 1].Cells[k].ReadOnly = false;

                                        }

                                    }
                                }


                            }


                        }
                        //根据内容大小，自动调整列宽
                        dgvMsgList.AutoResizeColumns();
                    }

                }
                catch (Exception ex)
                {
                    UIPage ui = new UIPage();
                    ui.ShowWarningTip(ex.Message);
                }

            }
            else
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip("请先结束数据保存，再进行读取参数表动作！");

            }



        }


        private void SelectAll_Click(object sender, EventArgs e)
        {
            if (dgvMsgList.RowCount != 0)
            {
                if (SelectAll.Text == "全选")
                {
                    SelectAll.Text = "全不选";
                    for (int i = 0; i < dgvMsgList.RowCount - 1; i++)
                    {

                        ((DataGridViewCheckBoxCell)dgvMsgList.Rows[i].Cells["选择"]).Value = true;
                    }

                }
                else
                {
                    SelectAll.Text = "全选";
                    for (int i = 0; i < dgvMsgList.RowCount - 1; i++)
                    {

                        ((DataGridViewCheckBoxCell)dgvMsgList.Rows[i].Cells["选择"]).Value = false;
                    }
                }
            }
            else
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip("请先加载EXCEL参数表！");
            }


        }

        private void btClearMsg_Click(object sender, EventArgs e)
        {
            if (dgvMsgList.RowCount != 0)
            {
                for (int i = 0; i < dgvMsgList.RowCount - 1; i++)
                {
                    dgvMsgList.Rows[i].Cells["EPPROM存储默认值"].Value = 0;
                }
            }
            else
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip("请先加载EXCEL参数表！");
            }


        }

        private void bt_WriteMsg_Click(object sender, EventArgs e)
        {
            if (bt_AutoSave.Text != "停止记录")
            {
                try
                {
                    if (dgvMsgList.RowCount != 0)
                    {
                        if (Device.m_bOpen == 1)
                        {
                            //保存修改到EXCEL,x需要先文件流打开
                            using (FileStream fs = File.Open(excelPath.Text, FileMode.Open, FileAccess.Read))
                            {
                                wkBook = new XSSFWorkbook(fs);
                                wkSheet = wkBook.GetSheetAt(0);
                                //写入数据
                                for (int i = 0; i < 48; i++)
                                {
                                    irow = wkSheet.GetRow(i + 1);

                                    if ((bool)(dgvMsgList.Rows[i].Cells["选择"]).Value == true)
                                    {
                                        irow.GetCell(2).SetCellValue(Convert.ToDouble(dgvMsgList.Rows[i].Cells["EPPROM存储默认值"].Value));

                                    }

                                }
                                //这是NPOI的bug，保存的话必须创建新的excel取代原来的
                                FileStream sw = File.Create(excelPath.Text);
                                wkBook.Write(sw);
                                sw.Close();
                            }
                            //写入请求发送
                            ReadFlg = false;
                            WriteFlg = true;
                            checkIndedx.Clear();
                            
                            for (int i = 0; i < dgvMsgList.RowCount - 1; i++)
                            {
                                if ((bool)(dgvMsgList.Rows[i].Cells["选择"]).Value == true)
                                {
                                    checkIndedx.Add(i);
                                }

                            }
                            uiRoundProcess1.Maximum = checkIndedx.Count;
                            Task.Run(new Action(ParaReadWriteCmd));
                        }
                        else
                        {
                            UIPage ui = new UIPage();
                            ui.ShowWarningTip("请开启CAN盒后再进行写入操作！");

                        }

                    }
                    else
                    {
                        UIPage ui = new UIPage();
                        ui.ShowWarningTip("请加载EXCEL参数表！");
                    }


                }
                catch (Exception ex)
                {
                    UIPage ui = new UIPage();
                    ui.ShowWarningTip(ex.Message);
                }

            }
            else
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip("请先结束数据保存，再进行参数写入动作！");

            }


        }


        private void btnReadMsg_Click(object sender, EventArgs e)
        {

            
            try
            {
                if (dgvMsgList.RowCount != 0)
                {

                    if (Device.m_bOpen == 1)
                    {
                        //rownum = 0;
                        checkIndedx.Clear();
                        ReadFlg = true;
                        WriteFlg = false;
                        for (int i = 0; i < dgvMsgList.RowCount - 1; i++)
                        {
                            if ((bool)(dgvMsgList.Rows[i].Cells["选择"]).Value == true)
                            {
                                checkIndedx.Add(i);
                            }

                        }
                        uiRoundProcess1.Maximum = checkIndedx.Count;
                        Task.Run(new Action(ParaReadWriteCmd));
                    }
                    else
                    {
                        UIPage ui = new UIPage();
                        ui.ShowWarningTip("请开启CAN盒以后再尝试读取操作！");

                    }

                }
                else
                {
                    UIPage ui = new UIPage();
                    ui.ShowWarningTip("请加载EXCEL参数表！");
                }
            }
            catch (Exception ex)
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip(ex.Message);
            }


        }



        #endregion

        #region 修改datagridview单元格内容
        //要修改这个datagridview单元格内容，就得先启动单元格编辑，再编辑，在完成

        private void dgvMsgList_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgvMsgList.CurrentCell = dgvMsgList.Rows[e.RowIndex].Cells[e.ColumnIndex];//获取当前单元格
                dgvMsgList.BeginEdit(true);//将单元格设为编辑状态

            }
        }


        private void dgvMsgList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //判断点击的是不是“选择”的那列CheckBoxCell
            if (e.ColumnIndex == 5)
            {
                //判断状态是选中还是没有选中
                if ((bool)(dgvMsgList.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value == true)
                {

                    ((DataGridViewCheckBoxCell)dgvMsgList.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value = false;
                }
                else
                {
                    ((DataGridViewCheckBoxCell)dgvMsgList.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value = true;
                }
            }

        }
        #endregion

       

      



      




















    }
}

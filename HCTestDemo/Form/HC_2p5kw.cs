using ControlCAN;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;




namespace HCTestDemo
{
    public partial class HC_2p5kw : Form
    {
        //定义USBCAN盒变量
        public UInt32 m_devind = 0;//CAN盒索引号
        int lifecount = 0;//生命信号
        int controlMotor_Station;//故障等级
        UInt16 controlMotor_Fault;//故障等级
        UInt32 Motor_Fault;
        int Motor_Warning;
        Int16 debugSpd = 0;
        int debugSet1 = 0;
        int debugSet2 = 0;
        public UInt32 ledcount_can0 = 0, ledcount_can1 = 0;//CAN接收数据计数
        VCI_CAN_OBJ[] send_can = new VCI_CAN_OBJ[50];//CAN发送数据数据
        byte[] pc_ParaRead = new byte[8];
        byte[] pc_ParaWrite = new byte[8];
        byte[] msg_Send = new byte[8];//发送的字节

        //EXCEL读写部分
        IWorkbook wkBook;
        ISheet wkSheet;
        ICellStyle style;//声明style1对象，设置Excel表格的样式
        IFont font;
        IRow irow;
        int row = 0; //excel写入数据的行数
        int msgNum = 0;//记录数据量
        int endFlag = 0; //结束记录数据标志
        bool saveFlg = false; //保存标志
        string filePathName = null;//Excel数据保存位置
        string Dirpath = null;//Excel数据保存文件夹位置
        //需要记录的全局变量
        double[] sj = new double[14];
        List<TextBox> textMsg = new List<TextBox>(); //所有待记录的数据窗口
        Action action;

        //参数导入
        bool ReadFlg = false;
        bool WriteFlg = false;
        int rownum = 0;

        //定时器
        System.Threading.Timer timer_msgSave;//执行实时保存数据的定时器
        System.Threading.Timer timer_ParaReadWrite;//执行读取参数操作
        public HC_2p5kw(Action action)
        {
            InitializeComponent();
            this.action = action;
        }
        public HC_2p5kw()
        {
            InitializeComponent();

        }
        private void connectbutton_Click(object sender, EventArgs e)
        {
            Device.CanID = (uint)ChannelSelect.SelectedIndex;
            if (Device.m_bOpen==1)
            {
                Device.VCI_CloseDevice(Device.DevType, m_devind);//m_devtype设备名称默认USBCAN2,m_devind索引号默认0
                Device.m_bOpen = 0;
                LedDebug.BackColor = Color.White;

            }
            else
            {

                if (Device.VCI_OpenDevice(Device.DevType, 0, (uint)ChannelSelect.SelectedIndex) == 1) //打开设备成功
                {
                    Device.m_bOpen = 1;
                    VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();
                    
                    CanChanelInit(Device.DevType, CANBotChose.SelectedItem.ToString(), (uint)ChannelSelect.SelectedIndex, config);
                }
                else
                {

                    MessageBox.Show("打开设备失败,请检查设备类型和CAN盒物理连接是否正确", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return;
                }

            }
            connectbutton.Text = Device.m_bOpen == 1 ? "断开连接" : "连接设备";
            connectbutton.Symbol = Device.m_bOpen == 1 ? 61758 : 61475;
            timer_rec.Enabled = Device.m_bOpen == 1 ? true : false;//根据设备打开状态控制接收定时器的开关
            
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
                        MessageBox.Show("初始化CAN1失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    if (Device.VCI_StartCAN(m_devtype, 0, 0) == 1)  //启动CAN通道0
                    {
                        LedDebug.BackColor = Color.YellowGreen;
                    }
                    else
                    {
                        MessageBox.Show("启动CAN1失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case 1:
                    if (Device.VCI_InitCAN(m_devtype, 0, 1, ref config) == 0)
                    {
                        MessageBox.Show("初始化CAN2失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    if (Device.VCI_StartCAN(m_devtype, 0, 1) == 1)  //启动CAN通道1
                    {
                        LedDebug.BackColor = Color.YellowGreen;
                    }
                    else
                    {
                        MessageBox.Show("启动CAN2失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                  break;
            }
           

        }

        /// <summary>
        /// CAN接收到数据解析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        unsafe private void timer_rec_Tick(object sender, EventArgs e)
        {
            
            lifecountFlg.Text = lifecount.ToString();
            UInt32 con_maxlen = 50;//用来接收的帧结构体数组的长度（本次接收的最大帧数，实际返回值小于等于这个值）。
            //CAN0接收的数据
            IntPtr pt_debug = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * (Int32)con_maxlen);//用来接收的帧结构体VCI_CAN_OBJ数组的首指针。
            UInt32 debugUrecNum = Device.VCI_GetReceiveNum(Device.DevType, 0, (uint)ChannelSelect.SelectedIndex);//尚未被读取的帧数
           
            if (debugUrecNum!=0)
            {
                UInt32 debugRecNum = Device.VCI_Receive(Device.DevType, 0, (uint)ChannelSelect.SelectedIndex, pt_debug, con_maxlen, 10);//实际读取到的帧数
                if (debugRecNum != 4294967295 && debugRecNum > 0)
                {
                    //构造LED闪烁来显示CAN数据的接收和发送动作正在进行
                    ledcount_can0++;
                    if (ledcount_can0 >= 7)
                    {
                        ledcount_can0 = 0;
                        LedDebug.BackColor = Color.YellowGreen;
                    }
                    else if (ledcount_can0 >= 3)
                    {
                        LedDebug.BackColor = Color.White;

                    }

                    for (UInt32 i = 0; i < debugRecNum; i++)
                    {

                        VCI_CAN_OBJ rec_can0 = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((UInt32)pt_debug + i * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));
                        rec_can0.TimeStamp = 0; //设备接收到某一帧的时间标识。 时间标示从CAN卡上电开始计时，计时单位为0.1ms。

                        switch (rec_can0.ID)
                        {
                            #region 调试报文
                            //MCU2PC1
                            case 0x112:
                                UdDebug.Text = ((Int16)(((rec_can0.Data[1]) << 8) + (rec_can0.Data[0])) * 0.1).ToString();
                                UqDebug.Text = ((Int16)(((rec_can0.Data[3]) << 8) + (rec_can0.Data[2])) * 0.1).ToString();
                                SpdIndexDebug.Text = ((Int16)(((rec_can0.Data[5]) << 8) + (rec_can0.Data[4]))).ToString();
                                TorqueIndexDebug.Text = ((Int16)(((rec_can0.Data[7]) << 8) + (rec_can0.Data[6])) * 0.1).ToString();

                                break;
                            //MCU2PC2
                            case 0x113:
                                DAxisCmdCurrent.Text = ((Int16)(((rec_can0.Data[1]) << 8) + (rec_can0.Data[0])) * 0.1).ToString();
                                QAxisCmdCurrent.Text = ((Int16)(((rec_can0.Data[3]) << 8) + (rec_can0.Data[2])) * 0.1).ToString();
                                DAxisFbkCurrent.Text = ((Int16)(((rec_can0.Data[5]) << 8) + (rec_can0.Data[4])) * 0.1).ToString();
                                QAxisFbkCurrent.Text = ((Int16)(((rec_can0.Data[7]) << 8) + (rec_can0.Data[6])) * 0.1).ToString();

                                break;
                            //MCU2PC3
                            case 0x114:
                                igtbtTempDebug.Text = (rec_can0.Data[0] - 50).ToString();
                                KL30VolDebug.Text = (rec_can0.Data[1] * 0.1).ToString();
                                ResolverOffsetDebug.Text = (((rec_can0.Data[3]) << 8) + (rec_can0.Data[2])).ToString();
                                TrqOutlineDebug.Text = (((rec_can0.Data[5]) << 8) + (rec_can0.Data[4])).ToString();
                                PowerMax.Text = (rec_can0.Data[6] * 0.1).ToString();
                                KL15VolDebug.Text = (rec_can0.Data[7] * 0.1).ToString();
                                break;
                            //MCU2PC4
                            case 0x115:
                                Motor_Fault = (UInt32)(((rec_can0.Data[3]) << 24) + ((rec_can0.Data[2]) << 16) + ((rec_can0.Data[1]) << 8) + (rec_can0.Data[0]));
                                Motor_Warning = (((rec_can0.Data[5]) << 8) + (rec_can0.Data[4]));
                                SpdFWIndexDebug.Text = (((rec_can0.Data[5]) << 8) + (rec_can0.Data[4])).ToString(); //弱磁索引
                                label58.Text = "调试故障码：" + Motor_Fault.ToString("F0");
                                label59.Text = "调试告警码：" + Motor_Warning.ToString("F0");

                                break;
                            //MCU2PC5
                            case 0x116:
                                UPhaseCurDebug.Text = ((Int16)(((rec_can0.Data[1]) << 8) + (rec_can0.Data[0])) * 0.1).ToString();
                                VPhaseCurDebug.Text = ((Int16)(((rec_can0.Data[3]) << 8) + (rec_can0.Data[2])) * 0.1).ToString();
                                WPhaseCurDebug.Text = ((Int16)(((rec_can0.Data[5]) << 8) + (rec_can0.Data[4])) * 0.1).ToString();
                                ResolverValueDebug.Text = ((Int16)(((rec_can0.Data[7]) << 8) + (rec_can0.Data[6]))).ToString();
                                break;
                            //MCU2PC6
                            case 0x117:
                                UaDebug.Text = ((Int16)(((rec_can0.Data[1]) << 8) + (rec_can0.Data[0])) * 0.1).ToString();
                                UbDebug.Text = ((Int16)(((rec_can0.Data[3]) << 8) + (rec_can0.Data[2])) * 0.1).ToString();
                                TorqueCmdDebug.Text = ((Int16)(((rec_can0.Data[5]) << 8) + (rec_can0.Data[4])) * 0.1).ToString();
                                SpdCmdDebug.Text = ((Int16)(((rec_can0.Data[7]) << 8) + (rec_can0.Data[6]))).ToString();
                                break;
                            //MCU2PC7
                            case 0x11d:

                                BatteryVolDebug.Text = ((((rec_can0.Data[3]) << 8) + (rec_can0.Data[2])) * 0.1).ToString();
                                ADC_BusVoltage.Text = (((rec_can0.Data[5]) << 8) + (rec_can0.Data[4])).ToString();
                                UInt16 SwVer = Convert.ToUInt16(((rec_can0.Data[1]) << 8) + (rec_can0.Data[0]));
                                UInt16 CompileData = Convert.ToUInt16(((rec_can0.Data[7]) << 8) + (rec_can0.Data[6]));
                                UInt16 C = (UInt16)((SwVer & 0x000F) >> 0);
                                UInt16 V = (UInt16)((SwVer & 0x00F0) >> 4);
                                UInt16 B = (UInt16)((SwVer & 0x0F00) >> 8);
                                UInt16 D = (UInt16)((SwVer & 0xF000) >> 12);
                                UInt16 Year = (UInt16)(((CompileData & 0xFE00) >> 9) + 2000);
                                UInt16 Mon = (UInt16)((CompileData & 0x01e0) >> 5);
                                UInt16 Day = (UInt16)((CompileData & 0x001f) >> 0);
                                statusStrip1.Text = "编译日期:" + Year.ToString() + "年" + Mon.ToString() + "月" + Day.ToString() + "日" + "    " + "软件版本:" + C.ToString() + "." + V.ToString() + "." + B.ToString() + "." + D.ToString();
                                CompileDate.Text = Year.ToString() + "/" + Mon.ToString() + "/" + Day.ToString();
                                SW_Ver.Text = "V" + C.ToString() + "." + V.ToString() + "." + B.ToString() + "." + D.ToString();
                                break;
                            #endregion
                            #region 整车控制报文
                            //MCU2VCU2
                            case 0x18f0083f:

                                FanSpeed.Text = (((rec_can0.Data[1]) << 8) + (rec_can0.Data[0])).ToString();
                                ControlTemp.Text = (rec_can0.Data[2] - 50).ToString();
                                Phase_Curr_RMS.Text = ((Int16)(((rec_can0.Data[4]) << 8) + (rec_can0.Data[3])) * 0.1).ToString();
                                controlMotor_Fault = (UInt16)(((rec_can0.Data[6]) << 8) + (rec_can0.Data[5]));
                                controlMotor_Station = rec_can0.Data[7];

                                label46.Text = "整车故障码：" + controlMotor_Fault.ToString();
                                spdMeter.Value = Convert.ToUInt16(FanSpeed.Text);
                                spdTextBox.Text = FanSpeed.Text + " r/min";

                                switch (controlMotor_Station)
                                {
                                    case 0:
                                        MotorStaion.Text = "待机";
                                        break;
                                    case 1:
                                        MotorStaion.Text = "准备好";
                                        break;
                                    case 2:
                                        MotorStaion.Text = "转速运行";
                                        break;
                                    case 3:
                                        MotorStaion.Text = "降额运行";
                                        break;
                                    case 4:
                                        MotorStaion.Text = "故障停机";
                                        break;
                                }


                                break;
                            case 0x18F0084F:
                                MotorTemp.Text = (rec_can0.Data[0] - 50).ToString();
                                MotorLifeCount.Text = (((rec_can0.Data[2]) << 8) + (rec_can0.Data[1])).ToString();
                                MotorVol.Text = ((((rec_can0.Data[4]) << 8) + (rec_can0.Data[3])) * 0.1).ToString();
                                DCVolt.Text = ((((rec_can0.Data[6]) << 8) + (rec_can0.Data[5])) * 0.1).ToString();
                                switch (rec_can0.Data[7])
                                {
                                    case 1:
                                        WorkModeDdescribe.Text = "怠速运行";
                                        break;
                                    case 2:
                                        WorkModeDdescribe.Text = "中速运行";
                                        break;
                                    case 3:
                                        WorkModeDdescribe.Text = "高速运行";
                                        break;


                                }

                                break;

                            #endregion
                            #region 参数读取返回值报文
                            case 0x11c:
                                if (dgvMsgList.RowCount != 0)
                                {
                                    if ((bool)(dgvMsgList.Rows[rec_can0.Data[0]].Cells["选择"]).Value == true)
                                    {
                                        dgvMsgList.Rows[rec_can0.Data[0]].Cells["EPPROM存储默认值"].Value = rec_can0.Data[1];
                                    }
                                }

                                break;

                            #endregion

                        }
                        //错误侦测
                        ErrorCheck();

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
        /// 自动保存数据
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
                    saveFlg = true;
                    //定义记录数据的异步定时器
                    timer_msgSave = new System.Threading.Timer(SaveExcelMsg, null, Timeout.Infinite, 100);
                    //新建多线程去执行创建EXCEL指令，并实时记录
                    Thread th = new Thread(CreatExcel);
                    th.IsBackground = true; //自定义线程定义成后台线程
                    th.Start();
                    SaveMsgInit();//初始化记录
                    msgProgressBar.Visible = true;
                    toolStripStatusLabel1.Text = "正在自动记录数据，目前已记录0笔....";
                }

                else  //结束保存动作
                {
                    if (saveFlg == true) //如果处于自动记录状态，执行这条
                    {
                        timer_msgSave.Dispose();
                        //结束写入，保存数据
                        EndSave();
                        MessageBox.Show("保存完毕,共自动记录数据" + msgNum + "笔！");
                        toolStripStatusLabel1.Text = " 自动记录完毕，共记录数据" + msgNum.ToString() + "笔";
                        msgProgressBar.Visible = false;

                    }
                    else //如果处于手动记录状态，执行这条
                    {
                        //结束写入，保存数据
                        EndSave();
                        MessageBox.Show("保存完毕,共手动记录数据" + msgNum + "笔！");
                        msgProgressBar.Visible = false;
                        toolStripStatusLabel1.Text = " 手动记录完毕，共记录数据" + msgNum.ToString() + "笔";

                    }
                    //写入结束，更改按钮和提示
                    bt_AutoSave.Symbol = 61674;
                    bt_AutoSave.Text = "自动保存";
                    fileSaveName.Text = "";
                    fileSaveName.Enabled = true;
                    saveSpace.Enabled = true;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
            try
            {
                lock (locker)
                {
                    row++;
                    msgNum++;
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

                        if (saveFlg == true) //如果处于自动记录状态，执行这条
                        {
                            timer_msgSave.Change(Timeout.Infinite, Convert.ToInt32(saveSpace.Text));
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
                        row = 0;
                        endFlag = 0;
                        BeginInvoke(new Action(() =>
                        {
                            fileSaveName.Text = DateTime.Now.ToString("HHmmss");
                            filePathName = System.IO.Path.Combine((Application.StartupPath + @"\DataSave\"), (DateTime.Now.ToString("yyyyMMdd")), (DateTime.Now.ToString("HHmmss") + ".xlsx"));
                        //根据日期创建数据保存文件夹路径
                        Dirpath = System.IO.Path.Combine((Application.StartupPath + @"\DataSave\"), DateTime.Now.ToString("yyyyMMdd"));
                            Directory.CreateDirectory(Dirpath);
                            textBox1.Text = Dirpath;
                            msgProgressBar.Maximum = msgProgressBar.Maximum + 6000;
                        }));

                        if (saveFlg) //自动保存模式下
                        {
                            //新建多线程去执行创建EXCEL指令，并实时记录
                            Thread th = new Thread(CreatExcel);
                            th.IsBackground = true; //自定义线程定义成后台线程
                            th.Start();

                        }
                        else //单笔保存模式下
                        {
                            saveFlg = false;
                            CreatExcel(); //新建book
                            SaveExcelMsg(0); //记录数据
                        }


                        return; //返回不执行下面

                    }

                    #endregion

                    //建立数据记录行，他是一行一行记录的
                    irow = wkSheet.CreateRow(row);

                    //写入实时数据
                    irow.CreateCell(0).SetCellValue(DateTime.Now.ToLocalTime().ToString("HH:mm:ss "));
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
                            irow.CreateCell(i).SetCellValue(textMsg[i - 1].Text);

                        }

                    }
                    irow.CreateCell(textMsg.Count).SetCellValue(Motor_Fault); //记录故障
                    irow.GetCell(textMsg.Count).CellStyle = style;//设置居中
                    irow.CreateCell(textMsg.Count + 1).SetCellValue(Motor_Warning);
                    irow.GetCell(textMsg.Count + 1).CellStyle = style;//设置居中
                    BeginInvoke(new Action(() =>
                    {
                        msgProgressBar.Value = msgNum;
                        toolStripStatusLabel1.Text = "正在记录数据，目前已记录" + msgNum + "笔....";
                    }));
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
                if (saveFlg)
                {
                    //启动记录异步定时器
                    timer_msgSave.Change(0, Convert.ToInt32(saveSpace.Text));
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        void SaveMsgInit()
        {
            //初始化相关参数
            row = 0;
            endFlag = 0;
            msgNum = 0;

            //修改保存按钮界面
            bt_AutoSave.Symbol = 61516;
            bt_AutoSave.Text = "停止记录";
            fileSaveName.Enabled = false;
            saveSpace.Enabled = false;
            //显示保存文件名
            if (fileSaveName.Text == "")
            {
                fileSaveName.Text = DateTime.Now.ToString("HHmmss");
                filePathName = System.IO.Path.Combine((Application.StartupPath + @"\DataSave\2.5KW\"), (DateTime.Now.ToString("yyyyMMdd")), (DateTime.Now.ToString("HHmmss") + ".xlsx"));
            }
            else
            {
                filePathName = System.IO.Path.Combine((Application.StartupPath + @"\DataSave\2.5KW\"), (DateTime.Now.ToString("yyyyMMdd")), (fileSaveName.Text + ".xlsx"));

            }
            //根据日期创建数据保存文件夹路径
            Dirpath = System.IO.Path.Combine((Application.StartupPath + @"\DataSave\2.5KW\"), DateTime.Now.ToString("yyyyMMdd"));
            Directory.CreateDirectory(Dirpath);
            textBox1.Text = Dirpath;

        }

        private void bt_SingleSave_Click(object sender, EventArgs e)
        {
           
            try
            {
                //创建记录EXCEL
                if (bt_AutoSave.Text == "自动保存") //当前未进行保存动作时
                {
                    saveFlg = false;
                    SaveMsgInit();//初始化记录
                    CreatExcel(); //新建book
                    SaveExcelMsg(0); //记录数据
                    msgProgressBar.Visible = true;

                }
                else if (row != 0)  //当excel文件已创立并记录了数据，接着往下记录
                {
                    SaveExcelMsg(0);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }

        private void openDir_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("地址栏为空，无法打开保存路径");
                }
                else
                {
                    Process.Start(textBox1.Text);
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }
           
        }


        private void RecoverFault_Click(object sender, EventArgs e)
        {
            label58.Text = "调试故障码：0";
            label59.Text = "调试告警码：0";
            label46.Text = "整车故障码：0";
            FaultDescribe.BackColor = Color.SpringGreen;
            debugFaultDescribe.BackColor = Color.SpringGreen;
            debugWarningDescribe.BackColor = Color.SpringGreen;
            FaultDescribe.Clear();
            debugWarningDescribe.Clear();
            debugFaultDescribe.Clear();
        }

        /// <summary>
        /// 整车数据帧发送
        /// </summary>
        public void PackPCTestCmd()
        {
            //0x18f401ff
            byte[] spdmsg_Send = new byte[8];
            spdmsg_Send[0] = (byte)(Convert.ToUInt16(EPTMode.SelectedIndex + 1));
            spdmsg_Send[1] = (byte)(Convert.ToUInt16(debugSpd) & 0x00ff);
            spdmsg_Send[2] = (byte)(Convert.ToUInt16(debugSpd) >> 8);
            spdmsg_Send[3] = (byte)lifecount;
            Device.sendCanMsg(send_can, Device.DevType, Device.CanID, 0x18f401ff, spdmsg_Send,8);
            lifecount++;
            if (lifecount > 255)
            {
                lifecount = 0;
            }
        }

        private void EmergencyStop_Click(object sender, EventArgs e)
        {
            debugSpd = 0;
            debugSet1 = 0;
            debugSet2 = 0;
            EPTMode.SelectedIndex = 4;
            


        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            //默认界面设置
            DeviceSlect.SelectedIndex = 3;
            CANBotChose.SelectedIndex = 9;
            ChannelSelect.SelectedIndex = 0;
            DebugDeltaTest.Text = "100";
            Device.DevType = Device.SelcetDev(DeviceSlect.SelectedItem.ToString());
            EPTMode.SelectedIndex = 4;
            toolStripStatusLabel1.Text = "欢迎使用测试版上位机~~~";
            //打开双缓存，避免界面出现闪烁异常
            this.DoubleBuffered = true;
           

            //遍历TextBox初始化设置
            foreach (Control j in gbxControlMsg.Controls)
            {
                if (j is System.Windows.Forms.TextBox)
                {
                    j.Text = "0";
                    textMsg.Add((TextBox)j);
                }
            }
            textMsg.Remove(spdTextBox);
            foreach (Control i in gbxDebugMsg.Controls)
            {
                if (i is System.Windows.Forms.TextBox)
                {
                    i.Text = "0.00";
                    i.Tag = i.Name;
                    textMsg.Add((TextBox)i);
                }
            }

        }

        private void DebugDeltaTest_TextChanged(object sender, EventArgs e)
        {
            DebugSetPata1Test.Increment = Convert.ToInt32(DebugDeltaTest.Text);
            TestParameters1.Increment = Convert.ToInt32(DebugDeltaTest.Text);
            TestParameters2.Increment = Convert.ToInt32(DebugDeltaTest.Text);

        }


        private void EPTMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            TestParameters1.Value = TestParameters2.Value=DebugSetPata1Test.Value = 0;
           
            switch (EPTMode.SelectedIndex)
            {
                case 0:
                case 1:
                case 2:
                    groupBox2.Enabled = false;
                    break;
                case 3:  
                case 4:
                    groupBox2.Enabled = true;
                    break;
            }

        }

        private void DebugSetPata1Test_ValueChanged(object sender, EventArgs e)
        {
            debugSpd = (Int16)DebugSetPata1Test.Value;

        }

        private void TestParameters1_ValueChanged(object sender, EventArgs e)
        {
            debugSet1 = (int)TestParameters1.Value;

        }

        private void TestParameters2_ValueChanged(object sender, EventArgs e)
        {
            debugSet2 = (int)TestParameters2.Value;
        }



        private void PcControlEnable_CheckedChanged(object sender, EventArgs e)
        {
            timer_send.Enabled = PcControlEnable.Checked == true ? true : false;//根据设备打开状态控制接收定时器的开关
        }

        void ErrorCheck()
        {
            #region 调试故障
            if (Motor_Fault != 0)
            {
                debugFaultDescribe.BackColor = Color.Red;
                string fault = Convert.ToString(Motor_Fault, 2);
                char[] faultnum = fault.ToCharArray();
                Array.Reverse(faultnum);
                for (int j = 0; j < faultnum.Length; j++)
                {
                    if (faultnum[j] != '0')
                    {
                        switch (j + 1)
                        {
                            case 1:
                                if (!debugFaultDescribe.Text.Contains("软件欠压"))
                                {
                                    debugFaultDescribe.AppendText("软件欠压\r\n");
                                }
                                break;
                            case 2:
                                if (!debugFaultDescribe.Text.Contains("软件过压"))
                                {
                                    debugFaultDescribe.AppendText("软件过压\r\n");
                                }
                                break;
                            case 3:
                                if (!debugFaultDescribe.Text.Contains("K30欠压"))
                                {
                                    debugFaultDescribe.AppendText("K30欠压\r\n");
                                }
                                break;
                            case 4:
                                if (!debugFaultDescribe.Text.Contains("K30过压"))
                                {
                                    debugFaultDescribe.AppendText("K30过压\r\n");
                                }
                                break;
                            case 5:
                                if (!debugFaultDescribe.Text.Contains("硬件过流或硬件封波"))
                                {
                                    debugFaultDescribe.AppendText("硬件过流或硬件封波\r\n");
                                }
                                break;
                            case 6:
                                if (!debugFaultDescribe.Text.Contains("软件过流"))
                                {
                                    debugFaultDescribe.AppendText("软件过流\r\n");
                                }
                                break;
                            case 7:
                                if (!debugFaultDescribe.Text.Contains("??&7"))
                                {
                                    debugFaultDescribe.AppendText("??&7\r\n");
                                }
                                break;
                            case 8:
                                if (!debugFaultDescribe.Text.Contains("硬件故障（无）"))
                                {
                                    debugFaultDescribe.AppendText("硬件故障（无）\r\n");
                                }
                                break;

                            case 9:
                                if (!debugFaultDescribe.Text.Contains("IGBT W相故障"))
                                {
                                    debugFaultDescribe.AppendText("IGBT W相故障\r\n");
                                }
                                break;

                            case 10:
                                if (!debugFaultDescribe.Text.Contains("IGBT V相故障"))
                                {
                                    debugFaultDescribe.AppendText("IGBT V相故障\r\n");
                                }
                                break;
                            case 11:
                                if (!debugFaultDescribe.Text.Contains("IGBT U相故障"))
                                {
                                    debugFaultDescribe.AppendText("IGBT U相故障\r\n");
                                }
                                break;
                            case 12:
                                if (!debugFaultDescribe.Text.Contains("IGBT直通故障（无）"))
                                {
                                    debugFaultDescribe.AppendText("IGBT直通故障（无）\r\n");
                                }
                                break;
                            case 13:
                                if (!debugFaultDescribe.Text.Contains("IGBT过温故障"))
                                {
                                    debugFaultDescribe.AppendText("IGBT过温故障\r\n");
                                }
                                break;
                            case 14:
                                if (!debugFaultDescribe.Text.Contains("电机过温故障"))
                                {
                                    debugFaultDescribe.AppendText("电机过温故障\r\n");
                                }
                                break;
                            case 15:
                                if (!debugFaultDescribe.Text.Contains("直流电流过流"))
                                {
                                    debugFaultDescribe.AppendText("直流电流过流\r\n");
                                }
                                break;
                            case 16:
                                if (!debugFaultDescribe.Text.Contains("驱动板欠压"))
                                {
                                    debugFaultDescribe.AppendText("驱动板欠压\r\n");
                                }
                                break;
                            case 17:
                                if (!debugFaultDescribe.Text.Contains("??&17"))
                                {
                                    debugFaultDescribe.AppendText("??&17\r\n");
                                }
                                break;
                            case 18:
                                if (!debugFaultDescribe.Text.Contains("过速故障"))
                                {
                                    debugFaultDescribe.AppendText("过速故障\r\n");
                                }
                                break;
                            case 19:
                                if (!debugFaultDescribe.Text.Contains("??&19"))
                                {
                                    debugFaultDescribe.AppendText("??&19\r\n");
                                }
                                break;
                            case 20:
                                if (!debugFaultDescribe.Text.Contains("??&20"))
                                {
                                    debugFaultDescribe.AppendText("??&20\r\n");
                                }
                                break;
                            case 21:
                                if (!debugFaultDescribe.Text.Contains("旋变LOT故障（无）"))
                                {
                                    debugFaultDescribe.AppendText("旋变LOT故障（无）\r\n");
                                }
                                break;
                            case 22:
                                if (!debugFaultDescribe.Text.Contains("旋变LOS故障（无）"))
                                {
                                    debugFaultDescribe.AppendText("旋变LOS故障（无）\r\n");
                                }
                                break;
                            case 23:
                                if (!debugFaultDescribe.Text.Contains("驱动板过温"))
                                {
                                    debugFaultDescribe.AppendText("驱动板过温\r\n");
                                }
                                break;
                            case 24:
                                if (!debugFaultDescribe.Text.Contains("EEPROM故障"))
                                {
                                    debugFaultDescribe.AppendText("EEPROM故障\r\n");
                                }
                                break;
                            case 25:
                                if (!debugFaultDescribe.Text.Contains("硬件过压"))
                                {
                                    debugFaultDescribe.AppendText("硬件过压\r\n");
                                }
                                break;
                            case 26:
                                if (!debugFaultDescribe.Text.Contains("硬件过温（无）"))
                                {
                                    debugFaultDescribe.AppendText("硬件过温（无）\r\n");
                                }
                                break;
                            case 27:
                                if (!debugFaultDescribe.Text.Contains("??&27"))
                                {
                                    debugFaultDescribe.AppendText("??&27\r\n");
                                }
                                break;
                            case 28:
                                if (!debugFaultDescribe.Text.Contains("电流采样故障"))
                                {
                                    debugFaultDescribe.AppendText("电流采样故障\r\n");
                                }
                                break;
                            case 29:
                                if (!debugFaultDescribe.Text.Contains("??&29"))
                                {
                                    debugFaultDescribe.AppendText("??&29\r\n");
                                }
                                break;
                            case 30:
                                if (!debugFaultDescribe.Text.Contains("??&30"))
                                {
                                    debugFaultDescribe.AppendText("??&30\r\n");
                                }
                                break;
                            case 31:
                                if (!debugFaultDescribe.Text.Contains("??&31"))
                                {
                                    debugFaultDescribe.AppendText("??&31\r\n");
                                }
                                break;
                            case 32:
                                if (!debugFaultDescribe.Text.Contains("??&32"))
                                {
                                    debugFaultDescribe.AppendText("??&32\r\n");
                                }
                                break;
                            case 33:
                                if (!debugFaultDescribe.Text.Contains("RectifOver_Temp"))
                                {
                                    debugFaultDescribe.AppendText("RectifOver_Temp\r\n");
                                }
                                break;
                            case 34:
                                if (!debugFaultDescribe.Text.Contains("缺相"))
                                {
                                    debugFaultDescribe.AppendText("缺相\r\n");
                                }
                                break;


                        }
                    }
                }
            }
            #endregion
            #region 调试警告
            if (Motor_Warning != 0)
            {
                debugWarningDescribe.BackColor = Color.Red;
            }
            #endregion
            #region 整车故障
            if (controlMotor_Fault != 0)
            {
                FaultDescribe.BackColor = Color.Red;
                string fault = Convert.ToString(controlMotor_Fault, 2);
                char[] faultnum = fault.ToCharArray();
                Array.Reverse(faultnum);
                for (int j = 0; j < faultnum.Length; j++)
                {
                    if (faultnum[j] != '0')
                    {
                        switch (j)
                        {
                            case 0:
                                if (!FaultDescribe.Text.Contains("高压欠压故障"))
                                {
                                    FaultDescribe.AppendText("高压欠压故障\r\n");
                                }
                                break;
                            case 1:
                                if (!FaultDescribe.Text.Contains("高压过压故障"))
                                {
                                    FaultDescribe.AppendText("高压过压故障\r\n");
                                }
                                break;
                            case 2:
                                if (!FaultDescribe.Text.Contains("硬件故障"))
                                {
                                    FaultDescribe.AppendText("硬件故障\r\n");
                                }
                                break;
                            case 3:
                                if (!FaultDescribe.Text.Contains("电机软件过流"))
                                {
                                    FaultDescribe.AppendText("电机软件过流\r\n");
                                }
                                break;
                            case 4:
                                if (!FaultDescribe.Text.Contains("电机堵转"))
                                {
                                    FaultDescribe.AppendText("电机堵转\r\n");
                                }
                                break;
                            case 5:
                                if (!FaultDescribe.Text.Contains("CAN总线故障"))
                                {
                                    FaultDescribe.AppendText("CAN总线故障\r\n");
                                }
                                break;
                            case 6:
                                if (!FaultDescribe.Text.Contains("低压供电欠压"))
                                {
                                    FaultDescribe.AppendText("低压供电欠压\r\n");
                                }
                                break;
                            case 7:
                                if (!FaultDescribe.Text.Contains("电机过速故障"))
                                {
                                    FaultDescribe.AppendText("电机过速故障\r\n");
                                }
                                break;

                            case 8:
                                if (!FaultDescribe.Text.Contains("电机电流传感器故障"))
                                {
                                    FaultDescribe.AppendText("电机电流传感器故障\r\n");
                                }
                                break;

                            case 9:
                                if (!FaultDescribe.Text.Contains("电机缺相"))
                                {
                                    FaultDescribe.AppendText("电机缺相\r\n");
                                }
                                break;
                            case 10:
                                if (!FaultDescribe.Text.Contains("低压供电过压"))
                                {
                                    FaultDescribe.AppendText("低压供电过压\r\n");
                                }
                                break;
                            case 11:
                                if (!FaultDescribe.Text.Contains("igbt驱动供电故障"))
                                {
                                    FaultDescribe.AppendText("igbt驱动供电故障\r\n");
                                }
                                break;
                            case 12:
                                if (!FaultDescribe.Text.Contains("直流电压过压降功率"))
                                {
                                    FaultDescribe.AppendText("直流电压过压降功率\r\n");
                                }
                                break;
                            case 13:
                                if (!FaultDescribe.Text.Contains("直流电压欠压降功率"))
                                {
                                    FaultDescribe.AppendText("直流电压欠压降功率\r\n");
                                }
                                break;
                            case 14:
                                if (!FaultDescribe.Text.Contains("igbt过温降功率"))
                                {
                                    FaultDescribe.AppendText("igbt过温降功率\r\n");
                                }
                                break;
                            case 15:
                                if (!FaultDescribe.Text.Contains("堵转告警"))
                                {
                                    FaultDescribe.AppendText("堵转告警\r\n");
                                }
                                break;

                        }
                    }
                }
            }
            #endregion
           
        }


        private void chooseFile_Click(object sender, EventArgs e)
        {

            try
            {
                OpenFileDialog fileDlg = new OpenFileDialog();
                fileDlg.Filter = "Excel文件|*.xlsx|Excel文件|*.xls";
                fileDlg.RestoreDirectory = true;
                fileDlg.InitialDirectory = Application.StartupPath;
                fileDlg.FilterIndex = 1;

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
                        //得到当前sheet,也可以通过GetSheet(name)得到
                        wkSheet = wkBook.GetSheetAt(0);

                        
                        //遍历表中所有的行//添加表头,这一段比较通用
                        irow = wkSheet.GetRow(wkSheet.FirstRowNum);
                        for (int k = irow.FirstCellNum; k < 7; k++)
                        {
                            dgvMsgList.AddColumn(irow.Cells[k].ToString(), irow.Cells[k].ToString(),200);

                        }
                        dgvMsgList.AddCheckBoxColumn("选择", "选择",100);
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
                                for (int k = irow.FirstCellNum; k < 7; k++)
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
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }

        private void debugFeedBack_CheckedChanged(object sender, EventArgs e)
        {
            //0x11f
            byte[] debugMsgSend = new byte[8];
            debugMsgSend[1] = debugFeedBack.Checked == true ? (byte)8 : (byte)0;
            Device.sendCanMsg(send_can, Device.DevType, Device.CanID, 0x11f, debugMsgSend,8);
        }

        #region 修改datagridview单元格内容
        //要修改这个datagridview单元格内容，就得先启动单元格编辑，再编辑，在完成

        private void dgvMsgList_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvMsgList.CurrentCell = dgvMsgList.Rows[e.RowIndex].Cells[e.ColumnIndex];//获取当前单元格
            dgvMsgList.BeginEdit(true);//将单元格设为编辑状态

        }

        private void dgview_Msg_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string msg = string.Format("编辑表格({0},{1})", e.RowIndex, e.ColumnIndex);
            this.Text = msg;
        }


        private void dgview_Msg_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string msg = String.Format("完成编辑 ({0}, {1})", e.RowIndex, e.ColumnIndex);
            this.Text = msg;
        }

        private void dgvMsgList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //判断点击的是不是“选择”的那列CheckBoxCell
            if (e.ColumnIndex == 7)
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

        private void btClearMsg_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvMsgList.RowCount - 1; i++)
            {
                dgvMsgList.Rows[i].Cells["EPPROM存储默认值"].Value = 0;
            }

        }

        private void bt_WriteMsg_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMsgList.RowCount!=0)
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
                    timer_ParaReadWrite = new System.Threading.Timer(ParaReadWriteCmd, null, Timeout.Infinite, 500);
                    uiRoundProcess1.Maximum = dgvMsgList.RowCount - 1;
                    //读取参数请求帧
                    byte[] pc_ParaWrite = new byte[8];
                    pc_ParaWrite[0] = 0x00;
                    pc_ParaWrite[1] = 0x0C;
                    pc_ParaWrite[6] = 0x10;
                    pc_ParaWrite[7] = 0x27;
                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, 0x11F, pc_ParaWrite,8);
                    //依次查询选择了的参数
                    //启动读写异步定时器
                    timer_ParaReadWrite.Change(0, 500);
                }
                else
                {
                    MessageBox.Show("请加载EXCEL参数表！");
                }
               

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void btnReadMsg_Click(object sender, EventArgs e)
        {

            try
            {
                if (dgvMsgList.RowCount!=0)
                {
                    rownum = 0;
                    ReadFlg = true;
                    WriteFlg = false;
                    timer_ParaReadWrite = new System.Threading.Timer(ParaReadWriteCmd, null, Timeout.Infinite, 500);
                    uiRoundProcess1.Maximum = dgvMsgList.RowCount - 1;
                    //读取参数请求帧
                    byte[] pc_ParaRead = new byte[8];
                    pc_ParaRead[0] = 0x00;
                    pc_ParaRead[1] = 0x0C;
                    pc_ParaRead[6] = 0x10;
                    pc_ParaRead[7] = 0x27;
                    Device.sendCanMsg(send_can, Device.DevType, Device.CanID, 0x11F, pc_ParaRead,8);

                    //依次查询选择了的参数
                    //启动读取异步定时器
                    timer_ParaReadWrite.Change(0, 500); 
                }
                else
                {
                    MessageBox.Show("未加载EXCEL参数表");

                }
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void SelectAll_Click(object sender, EventArgs e)
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


        void ParaReadWriteCmd(object sender)
        {

            if (rownum < dgvMsgList.RowCount - 1)
            {
                if (ReadFlg)
                {
                    if ((bool)(dgvMsgList.Rows[rownum].Cells["选择"]).Value == true)
                    {
                        byte[] pc_ParaRead = new byte[8];
                        pc_ParaRead[0] = Convert.ToByte(dgvMsgList.Rows[rownum].Cells["参数序号"].Value);
                        pc_ParaRead[5] = 0x01;
                        Device.sendCanMsg(send_can, Device.DevType, Device.CanID, 0x11E, pc_ParaRead,8);
                        BeginInvoke(new Action(() =>
                        {
                            uiRoundProcess1.Value = rownum + 1;
                        }));
                    }
                    rownum++; 
                }
                else if (WriteFlg)
                {
                     if ((bool)(dgvMsgList.Rows[rownum].Cells["选择"]).Value == true)
                    {
                        UInt32 paracoef = Convert.ToUInt32(dgvMsgList.Rows[rownum].Cells["系数"].Value);
                        double paravalue = Convert.ToDouble(dgvMsgList.Rows[rownum].Cells["EPPROM存储默认值"].Value );
                        UInt32 para = Convert.ToUInt32(paravalue * paracoef);
                        pc_ParaWrite[0] = Convert.ToByte(dgvMsgList.Rows[rownum].Cells["参数序号"].Value);
                        pc_ParaWrite[1] = (byte)(para & 0xFF);
                        pc_ParaWrite[2] = (byte)(para>>8);
                        pc_ParaWrite[3] = (byte)(para >>16);
                        pc_ParaWrite[4] = (byte)(para>>24);
                        pc_ParaWrite[6] = 0x01; 
                        Device.sendCanMsg(send_can, Device.DevType, Device.CanID, 0x11E, pc_ParaWrite,8);
                        BeginInvoke(new Action(() =>
                        {
                            uiRoundProcess1.Value = rownum + 1;
                        }));
                    }
                    rownum++; 
                }
            }
            else
            {
                timer_ParaReadWrite.Change(Timeout.Infinite, 500);
                timer_ParaReadWrite.Dispose();
                rownum = 0;

            }
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //在点击右上角关闭按钮或者手动ALT+F4关闭窗口时，做个确定关闭的判断
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //如果关闭窗口时，EXCEL实时记录数据还在进行，应先关闭记录动作
                if (bt_AutoSave.Text == "停止记录")
                {
                    MessageBox.Show("数据正在实时记录,请关闭EXCEL实时保存后再关闭界面!");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    DialogResult result = MessageBox.Show("确定要退出程序?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    action();
                    e.Cancel = result != DialogResult.OK;
                    
                }
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

        private void DischargeCMD_Click(object sender, EventArgs e)
        {
            if (PcControlEnable.Checked)
            {
                byte[] dischargeCMD = new byte[8];
                dischargeCMD[0] = 0xaa;
                Device.sendCanMsg(send_can, Device.DevType, Device.CanID, 0x18F501FF, dischargeCMD,8);
            }
           
        }

        

    }
}

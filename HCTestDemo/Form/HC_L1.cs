using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HCTestDemo
{
    public delegate void SpConfigDelegate();
    public partial class HC_L1 : Form
    {
        #region 全局变量


        //定时器
        System.Threading.Timer timer_msgSave;

        //发送变量
        int spd = 0;

        Int16 spdFeedBack=0;

        List<string> buff = new List<string>();

        //Excel实时记录的数据
        double[] sj = new double[5];

        //COM
        public SpConfigDelegate spConfigDelegate;
        public string PortNameSel = "Com1";
        public string CheckBitSel = "无";
        public string BaudrateSel = "无";
        public string DataBitSel = "无";
        public string StopBitSel = "无";
        int checkFlag = 1; //查询标志位
        int recNum = 0; //接收串口消息计数
        int checkStateNum = 0;//发送次数统计
        int errorFlag = 0;
        int oldError = 0;
        #endregion

        public HC_L1()
        {
            InitializeComponent();
        }
        Action action;
        public HC_L1(Action action)
        {
            InitializeComponent();
            this.action = action;

        }

        private void bt_OpenSerial_Click(object sender, EventArgs e)
        {

            if (cb_SerialNum.SelectedItem != null)
            {
                //定义串口参数
                PortNameSel = cb_SerialNum.Text;
                CheckBitSel = cb_CheckBitSel.Text;
                BaudrateSel = cb_BaudrateSel.Text;
                DataBitSel = cb_DataBitSel.Text;
                StopBitSel = cb_StopBitSel.Text;

                if (sp_Com.IsOpen)
                {
                    sp_Com.Close();
                    bt_OpenSerial.Text = "打开串口";
                    bt_OpenSerial.Symbol = 61475;
                    WriteLogFile("控制事件", " 串口关闭");
                    Led_Device.BackColor = Color.White;
                    cb_SerialNum.Enabled = true;
                    cb_CheckBitSel.Enabled = true;
                    cb_BaudrateSel.Enabled = true;
                    cb_DataBitSel.Enabled = true;
                    cb_StopBitSel.Enabled = true;

                    //释放定时器
                    if (timer_msgSave!=null)
                    {
                        timer_msgSave.Change(Timeout.Infinite, 1000);
                        timer_msgSave.Dispose();
                        
                    }
                    
                }
                else
                {
                    cb_SerialNum.Enabled = false;
                    cb_CheckBitSel.Enabled = false;
                    cb_BaudrateSel.Enabled = false;
                    cb_DataBitSel.Enabled = false;
                    cb_StopBitSel.Enabled = false;

                    sp_Com.PortName = PortNameSel;
                    sp_Com.BaudRate = Convert.ToInt32(BaudrateSel);
                    sp_Com.DataBits = Convert.ToInt16(DataBitSel);
                    if (CheckBitSel == "无")
                    {
                        sp_Com.Parity = Parity.None;
                    }
                    else if (CheckBitSel == "奇校验")
                    {
                        sp_Com.Parity = Parity.Odd;
                    }
                    else if (CheckBitSel == "偶校验")
                    {
                        sp_Com.Parity = Parity.Even;
                    }
                    else if (CheckBitSel == "Mark")
                    {
                        sp_Com.Parity = Parity.Mark;
                    }
                    else if (CheckBitSel == "空格校验")
                    {
                        sp_Com.Parity = Parity.Space;
                    }
                    else
                    {
                        sp_Com.Parity = Parity.None;
                    }

                    if (StopBitSel == "1")
                    {
                        sp_Com.StopBits = StopBits.One;
                    }
                    else if (StopBitSel == "1.5")
                    {
                        sp_Com.StopBits = StopBits.OnePointFive;
                    }
                    else if (StopBitSel == "2")
                    {
                        sp_Com.StopBits = StopBits.Two;
                    }
                    else
                    {
                        sp_Com.StopBits = StopBits.None;
                    }
                    sp_Com.Open();
                    bt_OpenSerial.Text = "关闭串口";
                    bt_OpenSerial.Symbol = 61596;
                    WriteLogFile("控制事件", " 串口打开");
                    Led_Device.BackColor = Color.YellowGreen;
                    //定义记录数据的异步定时器
                    timer_msgSave = new System.Threading.Timer(UpdateBuffer, null, Timeout.Infinite, 1000);
                    timer_msgSave.Change(0, 1000);
                }
            }
            else
            {
                MessageBox.Show("还未选择端口号");
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] str = System.IO.Ports.SerialPort.GetPortNames();
                if (str == null)
                {
                    MessageBox.Show("本机没有串口！", "Error");
                    return;
                }
                //添加串口项目
                foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
                {
                    //获取有多少个COM口
                    cb_SerialNum.Items.Add(s);
                }
                spdIncre.Text = "100";
                cb_SerialNum.SelectedIndex = cb_SerialNum.Items.Count-1;
                cb_BaudrateSel.SelectedIndex = 3;
                cb_DataBitSel.SelectedIndex = 2;
                cb_CheckBitSel.SelectedIndex = 0;
                cb_StopBitSel.SelectedIndex = 0;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }


        private void spdIncre_TextChanged(object sender, EventArgs e)
        {
            SetSpdSend.Increment = Convert.ToInt32(spdIncre.Text);
        }

        private void timer_send_Tick(object sender, EventArgs e)
        {

            try
            {
                if (sp_Com.IsOpen)
                {

                    switch (checkFlag)
                    {
                        case 1:
                            //查询故障指令
                            byte[] checkError = new byte[3];
                            checkError[0] = 0x4D;
                            checkError[1] = 0x4C;
                            checkError[2] = 0x4C;
                            sp_Com.Write(checkError, 0, 3);
                            SaveLog(DateTime.Now.ToLocalTime().ToString("HH:mm:ss:fff ") + "     " + "发送：" + ArrayToString(checkError) + "\r\n");
                            checkStateNum++; //查询状态计数
                            break;
                        case 2:
                            //查询转速指令
                            byte[] checkSpd = new byte[3];
                            checkSpd[0] = 0x4D;
                            checkSpd[1] = 0x4C;
                            checkSpd[2] = 0x47;
                            sp_Com.Write(checkSpd, 0, 3);
                            SaveLog(DateTime.Now.ToLocalTime().ToString("HH:mm:ss:fff ") + "     " + "发送：" + ArrayToString(checkSpd) + "\r\n");
                            checkStateNum++; //查询计数
                            break;

                        case 3:
                            //母线电压查询
                            CheckMsg(0xdb);
                            break;
                        case 4:
                            //母线电流查询
                            CheckMsg(0xdc);
                            break;
                        case 5:
                            //控制器温度查询
                            CheckMsg(0xdd);
                            break;
                        case 6:
                            //查询电机电流
                            CheckMsg(0xde);
                            break;
                        case 7:
                            //查询电机扭矩
                            CheckMsg(0xdf);
                            break;
                        case 8:
                            //查询电机电压
                            CheckMsg(0xef);
                            break;
                        case 9:
                            //查询软件版本
                            CheckMsg(0xda);
                            break;
                        case 10:
                            //转速指令
                            byte[] sendSpd = new byte[5];
                            sendSpd[0] = 0x4D;
                            sendSpd[1] = 0x4C;
                            sendSpd[2] = 0x3B;
                            sendSpd[3] = (byte)(Convert.ToInt16(spd * 16) & 0x00ff);
                            sendSpd[4] = (byte)(Convert.ToInt16(spd * 16) >> 8);
                            sp_Com.Write(sendSpd, 0, 5);
                            SaveLog(DateTime.Now.ToLocalTime().ToString("HH:mm:ss:fff ") + "     " + "发送：" + ArrayToString(sendSpd) + "\r\n");
                            checkFlag++; //查询计数，用于区分开转速、故障查询转速控制、启停
                            break;

                        case 11:
                            //启停指令
                            StartOrStop();
                            checkFlag++;
                            break;

                    }
                    LedSend.BackColor = checkFlag % 2 == 0 ? Color.YellowGreen : Color.White;
                }
                //连续发送超过10次
                if (checkStateNum >= 10)
                {
                    string error = null;
                    loopsend.Checked = timer_send.Enabled = false;
                    SetSpdSend.Value = 0;
                    switch (checkFlag)
                    {
                        case 1:
                            error = "控制器状态无反馈";
                            break;
                        case 2:

                            error = "电机转速无反馈";
                            break;
                        case 3:
                            error = "直流电压无反馈";
                            break;
                        case 4:
                            error = "直流电流无反馈";
                            break;
                        case 5:
                            error = "控制器温度无反馈";
                            break;
                        case 6:
                            error = "电机电流无反馈";
                            break;
                        case 7:
                            error = "电机扭矩无反馈";
                            break;
                        case 8:
                            error = "电机电压无反馈";
                            break;
                        case 9:
                            error = "软件版本无反馈";
                            break;
                    }
                    WriteLogFile("故障事件", "串口掉线 " + error);
                    DialogResult dr = MessageBox.Show("串口掉线！");
                    errorFlag = 1;//故障位更新
                    if (dr == DialogResult.OK)
                    {
                        EmergencyStop.Text = "启动";
                        EmergencyStop.Symbol = 61912;
                        checkStateNum = 0;
                    }
                }
                if (checkFlag >= 12)
                {
                    checkFlag = 1;
                }
            }
            catch (Exception ex)
            {
                loopsend.Checked = timer_send.Enabled = false;
                SetSpdSend.Value = 0;
                DialogResult dr = MessageBox.Show(ex.Message);
                bt_OpenSerial.Text = "打开串口";
                bt_OpenSerial.Symbol = 61475;
                EmergencyStop.Text = "启动";
                EmergencyStop.Symbol = 61912;
                checkStateNum = 0;
            }

        }

        private void sp_Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                recNum++;
                LedRec.BackColor = recNum % 2 == 0 ? Color.White : Color.YellowGreen;
                int bufflen = sp_Com.BytesToRead;
                //不解析不完整的帧
                if (bufflen < 2)
                {
                    return;

                }
                byte[] buff = new byte[bufflen];
                sp_Com.Read(buff, 0, bufflen);
                SaveLog(DateTime.Now.ToLocalTime().ToString("HH:mm:ss:fff ") + "     " + "接收：" + ArrayToString(buff) + "\r\n");
                if (loopsend.Checked)
                {
                    #region 接收反馈状态
                    switch (checkFlag)
                    {
                        //反馈状态
                        case 1:
                           
                            oldError = errorFlag;
                            errorFlag = buff[0];
                            if (oldError != errorFlag)
                            {
                                switch (buff[0])
                                {
                                    case 0:
                                        WriteLogFile("故障清除", "无故障");
                                        BeginInvoke(new Action(() =>
                                        {
                                            WarningDescribe.BackColor = Color.SpringGreen;
                                        }));
                                        break;
                                    case 4:
                                        WriteLogFile("故障事件", "IPM过流");

                                        break;
                                    case 7:
                                        WriteLogFile("故障事件", "超速");

                                        break;
                                    case 8:
                                        WriteLogFile("故障事件", "旋变断线");

                                        break;
                                    default:
                                        WriteLogFile("故障事件", "其他故障");
                                        break;
                                }

                            }
                            checkStateNum = 0;
                            checkFlag++; //查询计数，用于区分开转速、故障查询转速控制、启停
                            break;

                        //转速反馈
                        case 2:
                            BeginInvoke(new Action(() =>
                            {
                                spdMeter.Value = (Int16)(((buff[1]) << 8) + (buff[0])) / 4;
                                lblSpd.Text = spdMeter.Value.ToString();
                                spdFeedBack = (Int16)spdMeter.Value;
                            }));
                            checkStateNum = 0;
                            checkFlag++; //查询计数，用于区分开转速、故障查询转速控制、启停
                            break;

                        //直流电压反馈
                        case 3:
                            BeginInvoke(new Action(() =>
                            {
                                BatteryVolDebug.Text = ((Int16)(((buff[1]) << 8) + (buff[0])) * 0.1).ToString();
                            }));
                            checkStateNum = 0;
                            checkFlag++; //查询计数，用于区分开转速、故障查询转速控制、启停
                            break;

                        //直流电流反馈
                        case 4:
                            BeginInvoke(new Action(() =>
                            {
                                CtrlCurrentDebug.Text = (((Int16)(((buff[1]) << 8) + (buff[0])) - 1000) * 0.1).ToString();
                            }));
                            checkStateNum = 0;
                            checkFlag++; //查询计数，用于区分开转速、故障查询转速控制、启停
                            break;

                        //控制器温度反馈
                        case 5:
                            BeginInvoke(new Action(() =>
                            {
                                ControlTemp.Text = ((Int16)(((buff[1]) << 8) + (buff[0])) - 50).ToString();
                            }));
                            checkStateNum = 0;
                            checkFlag++; //查询计数，用于区分开转速、故障查询转速控制、启停
                            break;

                        //电机电流反馈
                        case 6:
                            BeginInvoke(new Action(() =>
                            {
                                Phase_Curr_RMS.Text = ((Int16)(((buff[1]) << 8) + (buff[0])) * 0.1).ToString();
                            }));
                            checkStateNum = 0;
                            checkFlag++; //查询计数，用于区分开转速、故障查询转速控制、启停
                            break;

                        //电机扭矩反馈
                        case 7:
                            BeginInvoke(new Action(() =>
                            {
                                MotorTorqueFB.Text = (((Int16)(((buff[1]) << 8) + (buff[0])) - 1000) * 0.1).ToString();
                            }));
                            checkStateNum = 0;
                            checkFlag++; //查询计数，用于区分开转速、故障查询转速控制、启停
                            break;

                        //电机电压反馈
                        case 8:
                            BeginInvoke(new Action(() =>
                            {
                                MotorVol.Text = ((Int16)(((buff[1]) << 8) + (buff[0])) * 0.1).ToString();
                            }));
                            checkStateNum = 0;
                            checkFlag++; //查询计数，用于区分开转速、故障查询转速控制、启停
                            break;
                        //软件版本反馈
                        case 9:
                            UInt16 SwVer = Convert.ToUInt16(((buff[1]) << 8) + (buff[0]));
                            UInt16 C = (UInt16)((SwVer & 0x000F) >> 0);
                            UInt16 V = (UInt16)((SwVer & 0x00F0) >> 4);
                            UInt16 B = (UInt16)((SwVer & 0x0F00) >> 8);
                            UInt16 D = (UInt16)((SwVer & 0xF000) >> 12);
                            BeginInvoke(new Action(() =>
                            {
                                SW_Ver.Text = "V" + C.ToString() + "." + V.ToString() + "." + B.ToString() + "." + D.ToString();
                            }));
                            checkStateNum = 0;
                            checkFlag++; //查询计数，用于区分开转速、故障查询转速控制、启停
                            break;

                    }


                }
                    #endregion

                if (recNum == 1000)
                {
                    recNum = 0;
                }

            }
            catch (Exception ex)
            {


                loopsend.Checked = timer_send.Enabled = false;
                SetSpdSend.Value = 0;
                DialogResult dr = MessageBox.Show(ex.Message);
                bt_OpenSerial.Text = "打开串口";
                bt_OpenSerial.Symbol = 61475;
                EmergencyStop.Text = "启动";
                EmergencyStop.Symbol = 61912;
                checkStateNum = 0;
            }

        }

        private void SetSpdSend_ValueChanged(object sender, EventArgs e)
        {
            spd =(int)SetSpdSend.Value;

        }

        private void loopsend_CheckedChanged(object sender, EventArgs e)
        {
            timer_send.Enabled = loopsend.Checked == true ? true : false;
        }

        private void EmergencyStop_Click(object sender, EventArgs e)
        {
            if (EmergencyStop.Text == "启动")
            {
                //启动
                EmergencyStop.Text = "停机";
                EmergencyStop.Symbol = 61457;
                WriteLogFile("控制事件", "电机启动");

            }
            else
            {
                //停止
                SetSpdSend.Value = 0;
                EmergencyStop.Text = "启动";
                EmergencyStop.Symbol = 61912;
                WriteLogFile("控制事件", "电机停止");
            }
            if (!loopsend.Checked)
            {
                StartOrStop();
            }

        }

        private void Led_Device_Click(object sender, EventArgs e)
        {

            try
            {
                string[] str = System.IO.Ports.SerialPort.GetPortNames();
                if (str == null)
                {
                    MessageBox.Show("本机没有串口！", "Error");
                    return;
                }
                //添加串口项目
                foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
                {
                    //获取有多少个COM口
                    cb_SerialNum.Items.Add(s);
                }
                cb_SerialNum.SelectedIndex = cb_SerialNum.Items.Count - 1;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void lbl_Rec_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Application.StartupPath + @"\LogSave");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }


        /// <summary>
        /// 配合传输转速值给绘图界面
        /// </summary>
        /// <returns></returns>
        public Int16 TraMsg()
        {
            return spdFeedBack;

        }

        #region 自定义方法

        /// <summary>
        /// 将数据存入缓存list
        /// </summary>
        /// <param name="msg"></param>
        void SaveLog(string msg)
        {
            //往队列增加数据点位
            buff.Add(msg);
            buff.Add(System.Environment.NewLine);

        }

        /// <summary>
        /// 启停控制
        /// </summary>
        void StartOrStop()
        {
            if (sp_Com.IsOpen)
            {
                byte[] sendMsg = new byte[5];
                sendMsg[0] = 0x4D;
                sendMsg[1] = 0x4C;
                sendMsg[2] = 0x33;
                sendMsg[3] = EmergencyStop.Text == "启动" ? (byte)0 : (byte)1;

                sendMsg[4] = 0x00;
                sp_Com.Write(sendMsg, 0, 5);
                SaveLog(DateTime.Now.ToLocalTime().ToString("HH:mm:ss:fff ") + "     " + "发送：" + ArrayToString(sendMsg) + "\r\n");

            }

        }

        /// <summary>
        /// 保存LOG
        /// </summary>
        /// <param name="title">事件名称</param>
        /// <param name="info">事件具体内容</param>
        void WriteLogFile(string title, string info)
        {
            BeginInvoke(new Action(() =>
            {
                WarningDescribe.AppendText(DateTime.Now.ToLocalTime().ToString("HH:mm:ss:fff ") + "     " + title + "：" + info + "\r\n");
            }));
            if (title == "故障事件")
            {
                BeginInvoke(new Action(() =>
                {
                    WarningDescribe.BackColor = Color.Red;
                }));
            }

            SaveLog(DateTime.Now.ToLocalTime().ToString("HH:mm:ss:fff ") + "     " + title + "：" + info + "\r\n");
        }



        /// <summary>
        /// 将需要保存的数据写入队列
        /// </summary>
        void UpdateBuffer(object sender)
        {
            if (buff.Count != 0)
            {
                for (int i = 0; i < buff.Count; i++)
                {
                    string path = Application.StartupPath + @"\DataSave\L1_LogSave\" + DateTime.Now.ToString("yyyyMMdd") + ".dat";
                    using (StreamWriter dout = new StreamWriter(path, true, Encoding.Default))
                    {
                        dout.Write(buff[i]);
                    }

                }
                buff.Clear();
            }
        }



        /// <summary>
        /// 转换Byte数组转成string字符
        /// </summary>
        /// <param name="by">Byte数组</param>
        /// <returns></returns>
        string ArrayToString(byte[] by)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < by.Length; i++)
            {
                s.Append(by[i].ToString("X")+" " );
            }
            return s.ToString();

        }



        /// <summary>
        /// 查询指令
        /// </summary>
        /// <param name="msg">查询指令最后一个字节</param>
        void CheckMsg(byte msg)
        {
            byte[] checkMsg = new byte[4];
            checkMsg[0] = 0x4D;
            checkMsg[1] = 0x4C;
            checkMsg[2] = 0xC4;
            checkMsg[3] = msg;
            sp_Com.Write(checkMsg, 0, 4);
            SaveLog(DateTime.Now.ToLocalTime().ToString("HH:mm:ss:fff ") + "     " + "发送：" + ArrayToString(checkMsg) + "\r\n");
            checkStateNum++; //查询计数
        }















        #endregion

        private void HC_L1_FormClosing(object sender, FormClosingEventArgs e)
        {

            //在点击右上角关闭按钮或者手动ALT+F4关闭窗口时，做个确定关闭的判断
            DialogResult result = MessageBox.Show("确定要退出程序?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                action();
            }
            e.Cancel = result != DialogResult.OK;
        }
    }
}

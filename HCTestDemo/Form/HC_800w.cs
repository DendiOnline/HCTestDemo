using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Diagnostics;
using Modbus.Device;
using Sunny.UI;

namespace HCTestDemo
{
    public partial class HC_800w : Form
    {
        #region 全局变量


        //定时器
        System.Threading.Timer timer_msgSave;

        //发送变量
        int spd = 0;
        bool spdCmd = false;

        Int16 spdFeedBack=0;

        List<string> buff = new List<string>();

        //Excel实时记录的数据
        double[] sj = new double[5];


        private static IModbusMaster master;

        //COM
        public SpConfigDelegate spConfigDelegate;
        public string PortNameSel = "Com1";
        public string CheckBitSel = "无";
        public string BaudrateSel = "无";
        public string DataBitSel = "无";
        public string StopBitSel = "无";
        int checkFlag = 1; //查询标志位
        int sendNum = 0; //发送消息计数
        int recNum = 0; //接收串口消息计数
        #endregion

        public HC_800w()
        {
            InitializeComponent();
        }
        Action action;
        public HC_800w(Action action)
        {
            InitializeComponent();
            this.action = action;

        }

        private void bt_OpenSerial_Click(object sender, EventArgs e)
        {
           
            if (cb_SerialNum.SelectedItem != null)
            {
 
                if (sp_Com.IsOpen)
                {
                    sp_Com.Close();
                    bt_OpenSerial.Text = "打开串口";
                    bt_OpenSerial.Symbol = 61475;
                    WriteLogFile("控制事件", " 串口关闭");
                    Led_Device.State = UILightState.Off;
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
                    //设定串口参数
                    sp_Com.PortName = cb_SerialNum.Text; //串口名称
                    sp_Com.BaudRate = int.Parse(cb_BaudrateSel.Text); //波特率
                    //校验位
                    if (cb_CheckBitSel.Text == "无")
                    {
                        sp_Com.Parity = Parity.None;
                    }
                    else if (cb_CheckBitSel.Text == "奇校验")
                    {
                        sp_Com.Parity = Parity.Odd;
                    }
                    else if (cb_CheckBitSel.Text == "偶校验")
                    {
                        sp_Com.Parity = Parity.Even;
                    }
                    else if (cb_CheckBitSel.Text == "Mark")
                    {
                        sp_Com.Parity = Parity.Mark;
                    }
                    else if (cb_CheckBitSel.Text == "空格校验")
                    {
                        sp_Com.Parity = Parity.Space;
                    }
                    else
                    {
                        sp_Com.Parity = Parity.None;
                    }
                    //数据位
                    sp_Com.DataBits = int.Parse(cb_DataBitSel.Text);
                    //停止位
                    if (cb_StopBitSel.Text == "1")
                    {
                        sp_Com.StopBits = StopBits.One;
                    }
                    else if (cb_StopBitSel.Text == "1.5")
                    {
                        sp_Com.StopBits = StopBits.OnePointFive;
                    }
                    else if (cb_StopBitSel.Text == "2")
                    {
                        sp_Com.StopBits = StopBits.Two;
                    }
                    else
                    {
                        sp_Com.StopBits = StopBits.None;
                    }

                    //创建ModbusRTU主站实例
                    master = ModbusSerialMaster.CreateRtu(sp_Com);
                    master.Transport.ReadTimeout = 2000; //读取从站报文回复的延迟
                    master.Transport.WriteTimeout = 2000; //写入从站报文回复的延迟
                    master.Transport.Retries = 1;
                    //打开串口
                    sp_Com.Open();
                    bt_OpenSerial.Text = "关闭串口";
                    bt_OpenSerial.Symbol = 61596;
                    WriteLogFile("控制事件", " 串口打开");
                    Led_Device.State = UILightState.On;
                    //定义记录数据的异步定时器
                    timer_msgSave = new System.Threading.Timer(UpdateBuffer, null, Timeout.Infinite, 1000);
                    timer_msgSave.Change(0, 1000);
                    Directory.CreateDirectory(Path.Combine(Application.StartupPath + @"\LogSave\"));
                    
                }
            }
            else
            {
                MessageBox.Show("端口号为空!");
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
                controlMode.SelectedIndex = 0;

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
            string msgTitle="";
            if (sp_Com.IsOpen)
            {
                //读取状态
                try
                {
                    msgTitle = " 读取状态错误";
                    var returnData = master.ReadHoldingRegisters(4, 5001, 20);
                    UDC.Text = (returnData[0] * 0.1).ToString();
                    IDC.Text = (returnData[1] *0.1 - 1000).ToString();
                    motorSpd.Text = lbl_Spd.Text = (returnData[2]).ToString();
                    Phase_Curr_RMS.Text = (returnData[3] * 0.1).ToString();
                    motorTemp.Text = (returnData[4] * 0.1 - 1000).ToString();
                    MotorTorqueFB.Text = (returnData[5] * 0.01 - 500).ToString("f2");
                    ANGLE.Text = (returnData[6]).ToString();
                    CONTROL_POWER_VOLT.Text = (returnData[7] * 0.1).ToString();
                    ControlTemp.Text = (returnData[8] * 0.1 - 1000).ToString("f1");
                    SYS_STS.Text = (returnData[9]).ToString();
                    PWM_STS.Text = (returnData[10]).ToString();
                    FAULT_LEVEL.Text = (returnData[11]).ToString("X4");
                    FAULT_WORD1.Text = "Fault1：" + (returnData[12]).ToString("X4");
                    FAULT_WORD2.Text = "Fault2：" + (returnData[13]).ToString("X4");
                    FAULT_WORD3.Text = "Fault3：" + (returnData[14]).ToString("X4");
                    FAULT_WORD4.Text = "Fault4：" + (returnData[15]).ToString("X4");
                    FAULT_WORD5.Text = "Fault5：" + (returnData[16]).ToString("X4");
                    SW_Ver.Text = string.Join(".",(returnData[17]).ToString("X4").ToCharArray());
                    HW_VER.Text = string.Join(".",(returnData[18]).ToString("X4").ToCharArray());
                    UInt16 Year = (UInt16)(((returnData[19] & 0xFE00) >> 9) + 2000);
                    UInt16 Mon = (UInt16)((returnData[19] & 0x01e0) >> 5);
                    UInt16 Day = (UInt16)((returnData[19] & 0x001f) >> 0);
                    COMPILE_DATE.Text = Year.ToString() + "/" + Mon.ToString() + "/" + Day.ToString();
                    sendNum++;
                    //转速模式
                    if (controlMode.SelectedIndex == 1&&spdCmd&&EmergencyStop.Text=="停机")
                    {
                        msgTitle = " 控制转速错误";
                        master.WriteSingleRegister(4, 8003, (ushort)spd);
                        spdCmd = false;
                        sendNum++;
                    }
                    LedSend.State = sendNum % 2 == 0 ? UILightState.On : UILightState.Off; 
                }
                catch (Exception ex)
                {
                    loopsend.Checked = false;
                    WriteLogFile("故障事件", msgTitle+ex.Message);
                }
                if (sendNum == int.MaxValue)
                {
                    sendNum = 0;
                }

            }

        }

     
        private void sp_Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            recNum++;
            LedRec.State = recNum % 2 == 0 ? UILightState.On : UILightState.Off;
            //int bufflen = sp_Com.BytesToRead;
            //不解析不完整的帧
            //if (bufflen < 2)
            //{
            //    return;

            //}
            //byte[] buff = new byte[bufflen];
            //sp_Com.Read(buff, 0, bufflen);
            //发生错误
            //if (buff.Length != 0 && buff[1] > 128)
            //{
            //    CheckError(buff);
            //    //转速回复错误，需要重新发送
            //    if (buff[1] == 134)
            //    {
            //        spdCmd = true;
            //    }
            //}
            //WriteLogFile("接收报文", "  " + ArrayToString(buff));
            if (recNum==int.MaxValue)
            {
                recNum = 0;
            }
        }

        private void CheckError(byte[] buff)
        {
            if (buff[1] > 128 && buff.Length==5)
            {
                switch (buff[2])
                {
                    case 0x01:
                        WriteLogFile("非法功能码", "变频器中没有该编号功能码  " + buff.ToString());
                        break;
                    case 0x02:
                        WriteLogFile("非法地址", "错误产生同上  " + ArrayToString(buff));
                        break;
                    case 0x03:
                        WriteLogFile("非法数据", "写入数据超出上下限  " + ArrayToString(buff));
                        break;
                    case 0x04:
                        WriteLogFile("非法数据", "写入数据无效  " + ArrayToString(buff));
                        break;
                    case 0x05:
                        WriteLogFile("非法数据", "数据帧错误  " + ArrayToString(buff));
                        break;
                    case 0x30:
                        WriteLogFile("非法数据", "数据为实际值，不可修改  " + ArrayToString(buff));
                        break;
                    case 0x31:
                        WriteLogFile("非法数据", "数据只能在停机时修改  " + ArrayToString(buff));
                        break;
                    case 0x32: 
                        WriteLogFile("非法数据", "密码保护错误  " + ArrayToString(buff));
                        break;
                }
                BeginInvoke(new Action(() =>
                {
                    WarningDescribe.BackColor = Color.Red;
                }));
            }
        }

        private void SetSpdSend_ValueChanged(object sender, EventArgs e)
        {
            spd =(int)SetSpdSend.Value;
            spdCmd = true;
        }

        private void loopsend_CheckedChanged(object sender, EventArgs e)
        {
            timer_send.Enabled = loopsend.Checked == true ? true : false;
        }

        private void EmergencyStop_Click(object sender, EventArgs e)
        {

            if (master!=null)
            {

                try
                {
                    if (EmergencyStop.Text == "启动")
                    {
                        //启动
                        master.WriteSingleRegister(4, 8001, 0);
                        EmergencyStop.Text = "停机";
                        EmergencyStop.Symbol = 61457;
                        WriteLogFile("控制事件", " 电机启动");
                        
                    }
                    else
                    {
                        //停止
                        SetSpdSend.Value = 0;
                        controlMode.SelectedIndex = 0;
                        master.WriteSingleRegister(4, 8001, 1);
                        EmergencyStop.Text = "启动";
                        EmergencyStop.Symbol = 61912;
                        WriteLogFile("控制事件", " 电机停止");
                    }
                }
                catch (Exception ex)
                {
                    
                    WriteLogFile("故障事件"," 启停故障"+ ex.Message);
                }
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
                
                cb_SerialNum.Items.Clear();//清空串口集合
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
            return Convert.ToInt16(motorSpd.Text);

        }
        private void controlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (master!=null)
            {
                //停机模式
                if (controlMode.SelectedIndex == 0)
                {
                    SetSpdSend.Value = 0;
                    //master.WriteSingleRegister(4, 0xfe01, (byte)spd);
                    spdCmd = false;
                }
                
            }
        }
        private void desRecovery_Click(object sender, EventArgs e)
        {
            
            try
            {
                master.WriteSingleRegister(4, 8002, 0);
                master.WriteSingleRegister(4, 8002, 1);
                WriteLogFile("故障恢复", " 手动恢复系统故障");
                WarningDescribe.BackColor = Color.SpringGreen;

               
            }
            catch (Exception ex)
            {
                
                WriteLogFile("系统故障", " 故障恢复错误"+ex.Message);
                WarningDescribe.BackColor = Color.Red;
            }
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
            byte[] sendMsg = new byte[5];
            sendMsg[0] = 0x4D;
            sendMsg[1] = 0x4C;
            sendMsg[2] = 0x33;
            sendMsg[3] = EmergencyStop.Text == "启动" ? (byte)0 : (byte)1;
            sendMsg[4] = 0x00;
            sp_Com.Write(sendMsg, 0, 5);
            SaveLog(DateTime.Now.ToLocalTime().ToString("HH:mm:ss:fff ") + "     " + "发送：" + ArrayToString(sendMsg) + "\r\n");
           
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
                    string path = Application.StartupPath + @"\LogSave\" + DateTime.Now.ToString("yyyyMMdd") + ".dat";
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
                s.Append(by[i].ToString("X2")+" " );
            }
            return s.ToString();

        }





















        #endregion

        private void HC_800w_FormClosing(object sender, FormClosingEventArgs e)
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

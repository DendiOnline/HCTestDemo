using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Modbus.Device;
using Sunny.UI;
using System.IO.Ports;

namespace HCTestDemo
{
    public partial class WoSenPowerControl : UserControl
    {
        private  IModbusMaster master;
        int SendNum, recNum;
        public WoSenPowerControl()
        {
            InitializeComponent();
        }

        private void WoSenPowerControl_Load(object sender, EventArgs e)
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
            cb_BaudrateSel.SelectedIndex = 3;
            cb_DataBitSel.SelectedIndex = 2;
            cb_CheckBitSel.SelectedIndex = 0;
            cb_StopBitSel.SelectedIndex = 0;
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
                    Led_Device.State = UILightState.Off;
                    cb_SerialNum.Enabled = true;
                    cb_CheckBitSel.Enabled = true;
                    cb_BaudrateSel.Enabled = true;
                    cb_DataBitSel.Enabled = true;
                    cb_StopBitSel.Enabled = true;
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
                    Led_Device.State = UILightState.On;


                }
            }
            else
            {
                MessageBox.Show("端口号为空!");
            }
        }

        private void bt_PowerOn_Click(object sender, EventArgs e)
        {
            if (sp_Com.IsOpen)
            {
                //电源开机
                if (bt_PowerOn.Text == "电源开机")
                {
                    ushort[] data = { 65280 };

                    master.WriteSingleRegister(1, 770, 3);
                    SendNum ++;
                    bt_PowerOn.Text = "电源关机";
                    bt_PowerOn.Symbol = 62091;
                }
                //电源关机
                else
                {

                    master.WriteSingleRegister(1, 770, 1);
                    SendNum ++;
                    bt_PowerOn.Text = "电源开机";
                    bt_PowerOn.Symbol = 61764;
                }
            }
        }

        private void timer_sendReq_Tick(object sender, EventArgs e)
        {
            if (sp_Com.IsOpen)
            {
                var retunMsg = master.ReadHoldingRegisters(1, 518, 4);//查询电源状态
                dc_V.Text = retunMsg[0].ToString();
                dc_I.Text = retunMsg[2].ToString();
                SendNum ++;
            }
        }

        private void bt_ParaChange_Click(object sender, EventArgs e)
        {
            if (sp_Com.IsOpen)
            {
                ushort[] data=new ushort[7];
                data[0] = Convert.ToUInt16(set_V.Text);
                data[3] = Convert.ToUInt16(set_I.Text);
                data[6] = Convert.ToUInt16(set_P.Text);
                master.WriteMultipleRegisters(1, 898, data);
                SendNum ++;
                UIPage ui = new UIPage();
                ui.ShowSuccessTip("设定电压:" + set_V.Text + "  限定电流:" + set_I.Text + "  限定功率:" + set_P.Text);
            }
        }

        private void sp_Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            recNum+=1;
            if (recNum>int.MaxValue)
            {
                recNum = 0;
            }
        }


    }
}

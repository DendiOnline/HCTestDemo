using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;
using ControlCAN;
using IDBCManager;
using System.Runtime.InteropServices;

namespace HCTestDemo
{
    public partial class HanSenPowerControl : UserControl
    {
        bool getParamSet = false;
        int ledcount_pwr = 0;//直流电源CAN接收数据计数
        bool pwrConnect = false;
        public const uint Pwr_Mode = 0x181AF410;
        public const uint Pwr_Para = 0x1813F410;

        Func<uint> func; //用以接收DBC句柄的委托
        public HanSenPowerControl(Func<uint> fc)
        {
            func = fc;
            InitializeComponent();
            cbxModeSelect.SelectedIndex = 0;
            cbx_Local.SelectedIndex = 1;
            cbx_Bot.SelectedIndex = 1;
        }
        #region 汉升电源

        private void btPowerStart_Click(object sender, EventArgs e)
        {
            if (pwrConnect)
            {
                Device.CanID = 0;
                DBCMessage msg_Pwr_Mode = GetDBCMessageById(Pwr_Mode);
                msg_Pwr_Mode.vSignals[0].nValue = cbxModeSelect.SelectedIndex + 1;
                msg_Pwr_Mode.vSignals[1].nValue = cbx_Local.SelectedIndex + 1;
                msg_Pwr_Mode.vSignals[3].nValue = 0;
                //电源开机
                if (btPowerStart.Text == "电源开机")
                {
                    msg_Pwr_Mode.vSignals[2].nValue = 1;
                    DBCSendMsg(msg_Pwr_Mode);
                    btPowerStart.Text = "电源关机";
                    btPowerStart.Symbol = 62091;
                }
                //电源关机
                else
                {
                    msg_Pwr_Mode.vSignals[2].nValue = 0;
                    DBCSendMsg(msg_Pwr_Mode);
                    btPowerStart.Text = "电源开机";
                    btPowerStart.Symbol = 61764;
                }
                Device.CanID = 1;
            }
        }

        private void cbxPramChange_Click(object sender, EventArgs e)
        {
            if (pwrConnect)
            {
                Device.CanID = 0;
                //参数修改
                DBCMessage msg_Pwr_Para = GetDBCMessageById(Pwr_Para);
                msg_Pwr_Para.vSignals[0].nValue = 0;
                msg_Pwr_Para.vSignals[1].nValue = Convert.ToUInt16(tex_SET_P.Text);
                msg_Pwr_Para.vSignals[2].nValue = Convert.ToUInt16(tex_SET_I.Text);
                msg_Pwr_Para.vSignals[3].nValue = Convert.ToUInt16(tex_SET_V.Text);
                DBCSendMsg(msg_Pwr_Para);
                Device.CanID = 1;
                UIPage ui = new UIPage();
                ui.ShowSuccessTip("设定电压:" + tex_SET_V.Text + "  限定电流:" + tex_SET_I.Text + "  限定功率:" + tex_SET_P.Text);
            }

        }
        private void bt_PwrInit_Click(object sender, EventArgs e)
        {
           

            if (Device.m_bOpen != 1)
            {
                UIPage ui = new UIPage();
                ui.ShowErrorTip("CAN盒未连接！");
                pwrConnect = false;
            }
            else
            {
                if (!pwrConnect)
                {
                    VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();
                    CanChanelInit(Device.DevType, cbx_Bot.SelectedItem.ToString(), 0, config);
                    
                    if (tex_Data_V.Text=="0")
                    {
                        Device.CanID = 0;
                        DBCMessage msg_Pwr_Mode = GetDBCMessageById(Pwr_Mode);
                        msg_Pwr_Mode.vSignals[0].nValue = cbxModeSelect.SelectedIndex + 1;
                        msg_Pwr_Mode.vSignals[1].nValue = cbx_Local.SelectedIndex + 1;
                        msg_Pwr_Mode.vSignals[2].nValue = 0;
                        msg_Pwr_Mode.vSignals[3].nValue = 0;
                        DBCSendMsg(msg_Pwr_Mode);
                        Device.CanID = 1;
                    }
                    getParamSet = true;
                    pwrConnect = true;
                    pwrLed.State = UILightState.On;
                    UIPage ui = new UIPage();
                    ui.ShowSuccessTip("连接直流电源通讯成功");
                    
                }
                else
                {
                    pwrConnect = false;
                    pwrLed.State = UILightState.Off;
                    UIPage ui = new UIPage();
                    ui.ShowErrorTip("断开直流电源通讯成功");

                }
                

            }
            timer_recMsg.Enabled = pwrConnect == true ? true : false;//根据设备打开状态控制接收定时器的开关
            bt_PwrInit.Text = pwrConnect == true ? "断开连接" : "连接设备";
            bt_PwrInit.Symbol = pwrConnect == true ? 61758 : 61475;
          
        }

        private void timer_recMsg_Tick(object sender, EventArgs e)
        {
            //电源柜接收报文，因为他的波特率与整车不符，只能将他放在第一路
            UInt32 pwrUrecNum = Device.VCI_GetReceiveNum(Device.DevType, 0, 0);//尚未被读取的帧数
            if (pwrUrecNum != 0) //判断是否有帧需要接收
            {
                UInt32 con_maxlen = 50;//用来接收的帧结构体数组的长度（本次接收的最大帧数，实际返回值小于等于这个值）。
                //用来接收的帧结构体VCI_CAN_OBJ数组的首指针。
                IntPtr pt_Pwr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * (Int32)con_maxlen);
                UInt32 pwrRecNum = Device.VCI_Receive(Device.DevType, 0, 0, pt_Pwr, con_maxlen, 10);//实际读取到的帧数

                //CAN接收的数据
                if (pwrRecNum != 4294967295 && pwrRecNum > 0)
                {
                    //构造LED闪烁来显示CAN数据的接收和发送动作正在进行
                    ledcount_pwr++;
                    if (ledcount_pwr >= 7)
                    {
                        ledcount_pwr = 0;
                        pwrLed.State = UILightState.On;
                    }
                    else if (ledcount_pwr >= 3)
                    {
                        pwrLed.State = UILightState.Off;

                    }

                    for (UInt32 i = 0; i < pwrRecNum; i++)
                    {

                        VCI_CAN_OBJ rec_can0 = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((UInt32)pt_Pwr + i * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));
                        rec_can0.TimeStamp = 0; //设备接收到某一帧的时间标识。 时间标示从CAN卡上电开始计时，计时单位为0.1ms。

                        //将接收到的数据转换成DBCMessage
                        can_frame obj_old = new can_frame();
                        obj_old.data = rec_can0.Data;
                        obj_old.can_dlc = rec_can0.DataLen;
                        obj_old.can_id = rec_can0.ID;
                        IntPtr ptr_msg = Marshal.AllocHGlobal((Marshal.SizeOf(typeof(DBCMessage))));
                        IntPtr ptr_frame = Marshal.AllocHGlobal((Marshal.SizeOf(typeof(can_frame))));
                        Marshal.StructureToPtr(obj_old, ptr_frame, false);
                        Method.DBC_Analyse(func(), ptr_frame, ptr_msg);
                        DBCMessage msg = (DBCMessage)Marshal.PtrToStructure((IntPtr)((UInt32)ptr_msg), typeof(DBCMessage));

                        if (msg.nID == 0x180310F4)
                        {
                            tex_Data_P.Text = msg.vSignals[1].nValue.ToString();
                            tex_Data_I.Text = msg.vSignals[2].nValue.ToString();
                            tex_Data_V.Text = msg.vSignals[3].nValue.ToString();

                        }

                        if (msg.nID == 0x180610F4 && getParamSet)
                        {
                            tex_SET_P.Text = msg.vSignals[1].nValue.ToString();
                            tex_SET_I.Text = msg.vSignals[2].nValue.ToString();
                            tex_SET_V.Text = msg.vSignals[3].nValue.ToString();
                            getParamSet = false;
                        }
                        Marshal.FreeHGlobal(ptr_msg);
                        Marshal.FreeHGlobal(ptr_frame);
                    }
                    Marshal.FreeHGlobal(pt_Pwr);
                }
                else
                {
                    //判断CAN盒是否正常在线
                    VCI_ERR_INFO canerror = new VCI_ERR_INFO();
                    Device.VCI_ReadErrInfo(Device.DevType, 0, 0, ref canerror);


                }
            }
        }

        #endregion

        DBCMessage GetDBCMessageById(uint id)
        {
            IntPtr ptr_msg = Marshal.AllocHGlobal((Marshal.SizeOf(typeof(DBCMessage))));
            Method.DBC_GetMessageById(func(), id, ptr_msg);
            DBCMessage msg = (DBCMessage)Marshal.PtrToStructure((IntPtr)((UInt32)ptr_msg), typeof(DBCMessage));
            Marshal.FreeHGlobal(ptr_msg);
            return msg;
        }

        void DBCSendMsg(DBCMessage msg)
        {
            IntPtr ptrpTransmit_data = Marshal.AllocHGlobal((Marshal.SizeOf(typeof(DBCMessage))));
            Marshal.StructureToPtr(msg, ptrpTransmit_data, false);
            Method.DBC_Send(func(), ptrpTransmit_data);
            Marshal.FreeHGlobal(ptrpTransmit_data);

        }

        /// <summary>
        /// 初始化并启动CAN通道
        /// </summary>
        /// <param name="m_devtype">CAN设备代码，例如USBCAN2就是4</param>
        /// <param name="channelbot">选择的CAN通道波特率</param>
        /// <param name="canind">通道索引号</param>
        /// <param name="config">通道参数组名</param>
        void CanChanelInit(uint m_devtype, string channelbot, UInt32 canind, VCI_INIT_CONFIG config)
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
                        pwrLed.State = UILightState.On;
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
                        pwrLed.State = UILightState.On;
                    }
                    else
                    {
                        UIPage ui = new UIPage();
                        ui.ShowWarningTip("启动CAN2失败");
                    }
                    break;

            }

        }
      


    }
}

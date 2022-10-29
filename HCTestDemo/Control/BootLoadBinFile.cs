using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ControlCAN;
using Sunny.UI;
using System.IO;

namespace HCTestDemo
{
    public partial class BootLoadBinFile : UserControl
    {
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

        System.Threading.Timer timer_msgLoad; //执行下载数据的定时器

        VCI_CAN_OBJ[] send_can = new VCI_CAN_OBJ[50];//CAN发送数据数据

        public BootLoadBinFile()
        {
            InitializeComponent();
        }


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
            UIPage ui = new UIPage();
            //根据选择修改ID
            if (ui.ShowSelectDialog(ref index, items, "电机ID选择", "请选择需要修改的电机ID:"))
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
        }


        #endregion


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

        private void BootLoadBinFile_Load(object sender, EventArgs e)
        {
            ControlSelcet.SelectedIndex = 0;
        }

        public void RecLoadInfo(VCI_CAN_OBJ rec_can0)
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

        public uint RecIDChange()
        {
            return recNum;
        }

        public void CheckIDInfo(uint idIndex,VCI_CAN_OBJ rec_can0)
        {
            #region 读取反馈控制器ID
            //查询设备ID反馈
            switch (idIndex)
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


            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ControlCAN
{
    //1.ZLGCAN系列接口卡信息的数据类型。
    [StructLayout(LayoutKind.Sequential)]
    public struct VCI_BOARD_INFO
    {
        public UInt16 hw_Version;
        public UInt16 fw_Version;
        public UInt16 dr_Version;
        public UInt16 in_Version;
        public UInt16 irq_Num;
        public byte can_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] str_Serial_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] str_hw_Type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Reserved;
    }


    /////////////////////////////////////////////////////
    //2.定义CAN信息帧的数据类型。
    [StructLayout(LayoutKind.Sequential)]
    public struct VCI_CAN_OBJ
    {
        public uint ID;
        public uint TimeStamp;
        public byte TimeFlag;
        public byte SendType;
        public byte RemoteFlag;//是否是远程帧
        public byte ExternFlag;//是否是扩展帧
        public byte DataLen;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Data;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Reserved;

    }
    ////2.定义CAN信息帧的数据类型。
    //[StructLayout(LayoutKind.Sequential)]
    //public struct VCI_CAN_OBJ 
    //{
    //    public UInt32 ID;
    //    public UInt32 TimeStamp;
    //    public byte TimeFlag;
    //    public byte SendType;
    //    public byte RemoteFlag;//是否是远程帧
    //    public byte ExternFlag;//是否是扩展帧
    //    public byte DataLen;
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    //    public byte[] Data;
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    //    public byte[] Reserved;

    //    public void Init()
    //    {
    //        Data = new byte[8];
    //        Reserved = new byte[3];
    //    }
    //}

    //3.定义CAN控制器状态的数据类型。
    [StructLayout(LayoutKind.Sequential)]
    public struct VCI_CAN_STATUS
    {
        public byte ErrInterrupt;
        public byte regMode;
        public byte regStatus;
        public byte regALCapture;
        public byte regECCapture;
        public byte regEWLimit;
        public byte regRECounter;
        public byte regTECounter;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved;
    }

    //4.定义错误信息的数据类型。
    [StructLayout(LayoutKind.Sequential)]
    public struct VCI_ERR_INFO
    {
        public UInt32 ErrCode;
        public byte Passive_ErrData1;
        public byte Passive_ErrData2;
        public byte Passive_ErrData3;
        public byte ArLost_ErrData;
    }

    //5.定义初始化CAN的数据类型
    [StructLayout(LayoutKind.Sequential)]
    public struct VCI_INIT_CONFIG
    {
        public UInt32 AccCode;
        public UInt32 AccMask;
        public UInt32 Reserved;
        public byte Filter;
        public byte Timing0;
        public byte Timing1;
        public byte Mode;

        
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CHGDESIPANDPORT
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] szpwd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] szdesip;
        public Int32 desport;

        public void Init()
        {
            szpwd = new byte[10];
            szdesip = new byte[20];
        }
    }

    public struct Device
    {
       
        //public const int VCI_PCI5121 = 1;
        //public const int VCI_PCI9810 = 2;
        //public const int VCI_USBCAN1 = 3;
        //public const int VCI_USBCAN2 = 4;
        //public const int VCI_USBCAN2A = 4;
        //public const int VCI_PCI9820 = 5;
        //public const int VCI_CAN232 = 6;
        //public const int VCI_PCI5110 = 7;
        //public const int VCI_CANLITE = 8;
        //public const int VCI_ISA9620 = 9;
        //public const int VCI_ISA5420 = 10;
        //public const int VCI_PC104CAN = 11;
        //public const int VCI_CANETUDP = 12;
        //public const int VCI_CANETE = 12;
        //public const int VCI_DNP9810 = 13;
        //public const int VCI_PCI9840 = 14;
        //public const int VCI_PC104CAN2 = 15;
        //public const int VCI_PCI9820I = 16;
        //public const int VCI_CANETTCP = 17;
        //public const int VCI_PEC9920 = 18;
        //public const int VCI_PCI5010U = 19;
        //public const int VCI_USBCAN_E_U = 20;
        //public const int VCI_USBCAN_2E_U = 21;
        //public const int VCI_PCI5020U = 22;
        //public const int VCI_EG20T_CAN = 23;
        //public const int VCI_PCIE9221 = 24;
        //public const int VCI_CANDTU200 = 32;
        public static uint DevType;
        public static uint CanID;
        public static UInt32 m_bOpen = 0; //设备打开状态
        public static VCI_CAN_OBJ rec_CAN; //接收对象
        public static uint SelcetDev(string devicename)
        {
            
            switch (devicename)
            {
                case "VCI_PCI5121":
                    DevType = 1;
                    break;

                case "VCI_PCI9810":
                    DevType = 2;
                    break;
                case "VCI_USBCAN1":
                    DevType = 3;
                    break;

                case "VCI_USBCAN2":
                    DevType = 4;
                    break;
                case "VCI_USBCAN2A":
                    DevType = 4;
                    break;

                case "VCI_PCI9820":
                    DevType = 5;
                    break;
                case "VCI_CAN232":
                    DevType = 6;
                    break;

                case "VCI_PCI5110":
                    DevType = 7;
                    break;
                case "VCI_CANLITE":
                    DevType = 8;
                    break;

                case "VCI_ISA9620":
                    DevType = 9;
                    break;
                case "VCI_ISA5420":
                    DevType = 10;
                    break;

                case "VCI_PC104CAN":
                    DevType = 11;
                    break;
                case "VCI_CANETUDP":
                    DevType = 12;
                    break;

                case "VCI_CANETE":
                    DevType = 12;
                    break;
                case "VCI_DNP9810":
                    DevType = 13;
                    break;

                case "VCI_PCI9840":
                    DevType = 14;
                    break;
                case "VCI_PC104CAN2":
                    DevType = 15;
                    break;

                case "VCI_PCI9820I":
                    DevType = 16;
                    break;
                case "VCI_CANETTCP":
                    DevType = 17;
                    break;

                case "VCI_PEC9920":
                    DevType = 18;
                    break;
                case "VCI_PCI5010U":
                    DevType = 19;
                    break;
                case "VCI_USBCAN_E_U":
                    DevType = 20;
                    break;

                case "VCI_USBCAN_2E_U":
                    DevType = 21;
                    break;
                case "VCI_PCI5020U":
                    DevType = 22;
                    break;

                case "VCI_EG20T_CAN":
                    DevType = 23;
                    break;
                case "VCI_PCIE9221":
                    DevType = 24;
                    break;

                case "VCI_CANDTU200":
                    DevType = 32;
                    break;
            }
            return DevType;
            

        }

        /// <summary>
        /// 初始化CAN通道发送数据
        /// </summary>
        /// <param name="can">CAN要发送的数据帧数组组名称，例如send_can0</param>
        /// <param name="m_devtype">CAN设备代码，例如USBCAN2就是4</param>
        /// <param name="canind">CAN设备索引号，例如USBCAN第一路就是0，第二路是1</param>l
        /// <param name="id">要发送的数据ID，例如0x000001</param>
        /// <param name="msg">要发送的数据内容</param>
        /// <param name="datalen">数据的长度，标准的未8位，也可以非标准例如3位</param>
        /// 
        public static string sendCanMsg(VCI_CAN_OBJ[] can, uint m_devtype, UInt32 canind, uint id, byte[] msg,byte datalen)
        {
            string result="发送成功";
            for (int j = 0; j < can.Length; j++)
            {

                can[j].SendType = (byte)0; //发送方式 0正常发送，1单次发送，2自发自收，3单次自发自收
                can[j].RemoteFlag = (byte)0;//帧格式 0数据帧，1远程帧
                if (id>4095)  //超过0xfff
                {
                    can[j].ExternFlag = 1;//帧类型 0标准帧，1扩展帧 ，目前调试CAN数据ID都为标准帧，整车CAN数据为扩展帧   
                }
                else
                {
                    can[j].ExternFlag = 0;//帧类型 0标准帧，1扩展帧 ，目前调试CAN数据ID都为标准帧，整车CAN数据为扩展帧 

                }
               
                can[j].ID = id;
                can[j].DataLen = datalen;
                can[j].Data = msg;

            }
            if (VCI_Transmit(m_devtype, 0, canind, can, 1) == 0)
            {
                switch (canind)
                {
                    case 0:
                        result = "CAN0数据发送错误";
                        break;
                    case 1:
                        result = "CAN1数据发送错误";
                        break;
                }

            }
            return result;
        }

        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_OpenDevice(UInt32 DeviceType, UInt32 DeviceInd, UInt32 Reserved);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_CloseDevice(UInt32 DeviceType, UInt32 DeviceInd);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_InitCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_INIT_CONFIG pInitConfig);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ReadBoardInfo(UInt32 DeviceType, UInt32 DeviceInd, ref VCI_BOARD_INFO pInfo);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ReadErrInfo(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_ERR_INFO pErrInfo);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ReadCANStatus(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_STATUS pCANStatus);

        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_GetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_SetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);

        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_GetReceiveNum(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ClearBuffer(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);

        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_StartCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ResetCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);

        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_Transmit(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, VCI_CAN_OBJ[] pSend, UInt32 Len);

        //[DllImport("controlcan.dll")]
        //static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pReceive, UInt32 Len, Int32 WaitTime);
        [DllImport("controlcan.dll", CharSet = CharSet.Ansi)]
        public static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, IntPtr pReceive, UInt32 Len, Int32 WaitTime);
    };
    // 函数调用返回状态值
    public enum STATUS
    {
        STATUS_OK = 1,
        STATUS_ERR = 0,
    };

    // 函数调用返回状态值
    public enum CMD
    {
        CMD_DESIP = 0,
        CMD_DESPORT = 1,
        CMD_SRCPORT = 2,
        CMD_CHGDESIPANDPORT = 2,
        CMD_TCP_TYPE = 4,//tcp 工作方式，服务器:1 或是客户端:0
    };

    public enum TCPTYPE
    {
        TCP_CLIENT = 0,
        TCP_SERVER = 1,
    }

    public enum REF
    {
        REFERENCE_BAUD = 1,
        REFERENCE_SET_TRANSMIT_TIMEOUT = 2,
        REFERENCE_ADD_FILTER = 3,
        REFERENCE_SET_FILTER = 4,
    };
}

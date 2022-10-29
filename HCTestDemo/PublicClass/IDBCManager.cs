using ControlCAN;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace IDBCManager
{
    public class Define
    {
        public const int _MAX_FILE_PATH_ = 260;//最长文件路径
        public const int _DBC_NAME_LENGTH_ = 127;//名称最长长度

        public const int _DBC_COMMENT_MAX_LENGTH_ = 200;//注释最长长度
        public const int _DBC_UNIT_MAX_LENGTH_ = 10;//单位最长长度
        public const int _DBC_SIGNAL_MAX_COUNT_ = 128;//一个消息含有的信号的最大数目
        public const int PROTOCOL_J1939 = 0;
        public const int PROTOCOL_OTHER = 1;
        public const uint INVALID_DBC_HANDLE = 0xffffffff; // 无效的DBC句柄
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DBCSignal
    {
        public UInt32 nStartBit; // 起始位
        public UInt32 nLen;	// 位长度
        public double nFactor; // 转换因子
        public double nOffset;	// 转换偏移 实际值=原始值*nFactor+nOffset
        public double nMin;    // 最小值
        public double nMax;	// 最大值
        public double nValue;  //实际值
        public UInt64 nRawValue;//原始值
        public byte is_signed; //1:有符号数据, 0:无符号
        public byte is_motorola;//是否摩托罗拉格式
        public byte multiplexer_type;//see 'multiplexer type' above
        public byte val_type;//0:integer, 1:float, 2:double
        public UInt32 multiplexer_value;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_UNIT_MAX_LENGTH_ + 1)]
        public byte[] unit;//单位
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_NAME_LENGTH_ + 1)]
        public byte[] strName;  //名称
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_COMMENT_MAX_LENGTH_ + 1)]
        public byte[] strComment;  //注释
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_NAME_LENGTH_ + 1)]
        public byte[] strValDesc;  //值描述
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DBCMessage
    {
        public UInt32 nSignalCount; //信号数量
        public UInt32 nID;
        public UInt32 nSize;	//消息占的字节数目
        double nCycleTime;//发送周期
        public byte nExtend; //1:扩展帧, 0:标准帧
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_SIGNAL_MAX_COUNT_)]
        public DBCSignal[] vSignals; //信号集合
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_NAME_LENGTH_ + 1)]
        public byte[] strName;  //名称
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_COMMENT_MAX_LENGTH_ + 1)]
        public byte[] strComment;    //注释
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct FileInfoMsg
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._MAX_FILE_PATH_ + 1)]
        public byte[] strFilePath; //dbc文件路径
        public byte type; //dbc的协议类型, j1939选择PROTOCOL_J1939, 其他协议选择PROTOCOL_OTHER
        public byte merge;//1:不清除现有的数据, 即支持加载多个文件;0：清除原来的数据
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct DevInfo
    {
        public UInt32 nDevType;
        public UInt32 nDevIndex;
        public UInt32 nChIndex; //通道号
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct Ctx
    {
        public UInt32 owner;
        public DevInfo devinfo;
        public UInt32 can0_handle;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN
    {
        public uint acc_code;
        public uint acc_mask;
        public uint reserved;
        public byte filter;
        public byte timing0;
        public byte timing1;
        public byte mode;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct CANFD
    {
        public uint acc_code;
        public uint acc_mask;
        public uint abit_timing;
        public uint dbit_timing;
        public uint brp;
        public byte filter;
        public byte mode;
        public UInt16 pad;
        public uint reserved;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct can_frame
    {
        public uint can_id;  /* 32 bit MAKE_CAN_ID + EFF/RTR/ERR flags */
        public byte can_dlc; /* frame payload length in byte (0 .. CAN_MAX_DLEN) */
        public byte __pad;   /* padding */
        public byte __res0;  /* reserved / padding */
        public byte __res1;  /* reserved / padding */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] data/* __attribute__((aligned(8)))*/;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct canfd_frame
    {
        public uint can_id;  /* 32 bit MAKE_CAN_ID + EFF/RTR/ERR flags */
        public byte len;     /* frame payload length in byte */
        public byte flags;   /* additional flags for CAN FD,i.e error code */
        public byte __res0;  /* reserved / padding */
        public byte __res1;  /* reserved / padding */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] data/* __attribute__((aligned(8)))*/;
    };

    [StructLayout(LayoutKind.Explicit)]
    public struct ZCAN_CHANNEL_INIT_CONFIG
    {
        [FieldOffset(0)]
        public uint can_type; //type:TYPE_CAN TYPE_CANFD

        [FieldOffset(4)]
        public ZCAN can;

        [FieldOffset(4)]
        public CANFD canfd;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_Receive_Data
    {
        public can_frame frame;
        public UInt64 timestamp;//us
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_Transmit_Data
    {
        public can_frame frame;
        public UInt32 transmit_type;
    };

    public class Method
    {
        /* special address description flags for the MAKE_CAN_ID */
        public static UInt32 CAN_EFF_FLAG = 0x80000000U; /* EFF/SFF is set in the MSB */
        public static UInt32 CAN_RTR_FLAG = 0x40000000U; /* remote transmission request */
        public static UInt32 CAN_ERR_FLAG = 0x20000000U; /* error message frame */
        public static UInt32 CAN_ID_FLAG = 0x1FFFFFFFU; /* id */

        // make id
        public static UInt32 MAKE_CAN_ID(UInt32 id, UInt32 eff, UInt32 rtr, UInt32 err) { return (id | (eff << 31) | (rtr << 30) | (err << 29)); }
        public static UInt32 IS_EFF(UInt32 id) { return ((id & CAN_EFF_FLAG)); } //非0:extend frame 0:standard frame
        public static UInt32 IS_RTR(UInt32 id) { return ((id & CAN_RTR_FLAG)); } //非0:remote frame 0:data frame
        public static UInt32 IS_ERR(UInt32 id) { return ((id & CAN_ERR_FLAG)); } //非0:error frame 0:normal frame
        public static UInt32 GET_ID(UInt32 id) { return (id & CAN_ID_FLAG); }

        // LibDBCManager.dll
        public delegate bool OnSend(IntPtr ctx, IntPtr pObj);
        public delegate void OnMultiTransDone(IntPtr ctx, IntPtr pMsg, IntPtr data, UInt16 nLen, byte nDirection);
        public static OnSend onSend;
        public static OnMultiTransDone onMultiTransDone;

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint DBC_Init();

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint DBC_Release(uint hDBC);

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DBC_LoadFile(uint hDBC, IntPtr pMsg);

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint DBC_GetMessageCount(uint hDBC);

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DBC_GetFirstMessage(uint hDBC, IntPtr pMsg);

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DBC_GetNextMessage(uint hDBC, IntPtr pMsg);

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void DBC_SetSender(uint hDBC, OnSend sender, IntPtr ctx);

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void DBC_SetOnMultiTransDoneFunc(uint hDBC, OnMultiTransDone func, IntPtr ctx);

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void DBC_OnReceive(uint hDBC, IntPtr pObj);

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DBC_Analyse(uint hDBC, IntPtr pObj, IntPtr pMsg);

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern byte DBC_Send(uint hDBC, IntPtr pMsg);

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DBC_GetMessageById(uint hDBC, uint id, IntPtr pMsg);

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 DBC_GetValDescPairCount(uint hDBC, UInt32 id, string name);

        [DllImport("LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern byte DBC_GetValDescPair(uint hDBC, uint msg_id, string name, IntPtr pair);




    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetValueFunc(string path, string value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate string GetValueFunc(string path, string value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetPropertysFunc(string path, string value);

    public struct IProperty
    {
        public SetValueFunc SetValue;
        public GetValueFunc GetValue;
        public GetPropertysFunc GetPropertys;
    };

    [StructLayout(LayoutKind.Sequential)]
    //值与含义对, 例如3 "Not Supported" 2 "Error" 1 "Not Defined"
    public struct ValDescPair
    {
        public double value;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_NAME_LENGTH_ + 1)]
        public byte[] desc;
    };

    public class DbcHelper
    {
        IntPtr m_dev_handle;
        SynchronizationContext mainThreadSynContext;
        public uint InitialDbcHandler()
        {
            
            DevInfo m_devInfo;
            Ctx m_ctx;//回调上下文, 作为回调函数的参数
            uint m_hDBC = Define.INVALID_DBC_HANDLE;
            //List<can_frame> m_frmBuffer = new List<can_frame>();//存放接收到的帧
            //List<VCI_CAN_OBJ> m_frmBuffer = new List<VCI_CAN_OBJ>();//存放接收到的帧
            List<DBCMessage> m_vMsg = new List<DBCMessage>();//存放dbc文件包含的消息
            Dictionary<int, DBCMessage> m_vMultiMsg = new Dictionary<int, DBCMessage>();//存放收发的多帧消息


            //DBC导入初始化
            m_hDBC = Method.DBC_Init();
            if (m_hDBC == Define.INVALID_DBC_HANDLE)
            {
                throw new Exception("生成DBC句柄失败!");
            }
            m_ctx = new Ctx();
            m_devInfo = new DevInfo();
            m_devInfo.nDevType = 4;//根据实际设备自行修改
            m_devInfo.nDevIndex = 0;
            m_devInfo.nChIndex = 0;

            m_ctx.devinfo.nDevType = m_devInfo.nDevType;
            m_ctx.devinfo.nDevIndex = m_devInfo.nDevIndex;
            m_ctx.devinfo.nChIndex = m_devInfo.nChIndex;
            m_ctx.owner = 1234; //用于判断是不是自己要处理的数据
            m_ctx.can0_handle = (UInt32)m_dev_handle;
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(m_ctx));
            Marshal.StructureToPtr(m_ctx, ptr, false);

            Method.onSend = OnSendFunc;
            //Method.onMultiTransDone = OnMultiTransDoneFunc;
            Method.DBC_SetSender(m_hDBC, Method.onSend, ptr);
            Method.DBC_SetOnMultiTransDoneFunc(m_hDBC, Method.onMultiTransDone, ptr);
            Marshal.FreeHGlobal(ptr);

            mainThreadSynContext = SynchronizationContext.Current; //记录主线程上下文

            return m_hDBC;
        }
        // 处理帧发送
        bool OnSendFunc(IntPtr ctx, IntPtr pObj)
        {
            Ctx info = (Ctx)Marshal.PtrToStructure(ctx, typeof(Ctx));
            can_frame obj = (can_frame)Marshal.PtrToStructure(pObj, typeof(can_frame));
            VCI_CAN_OBJ[] data = new VCI_CAN_OBJ[1];
            data[0].SendType = 0; //发送方式 0正常发送，1单次发送，2自发自收，3单次自发自收
            data[0].RemoteFlag = 0;//帧格式 0数据帧，1远程帧
            if (obj.can_id > 4095)  //超过0xfff
            {
                data[0].ExternFlag = 1;//帧类型 0标准帧，1扩展帧 ，目前调试CAN数据ID都为标准帧，整车CAN数据为扩展帧   
            }
            else
            {
                data[0].ExternFlag = 0;//帧类型 0标准帧，1扩展帧 ，目前调试CAN数据ID都为标准帧，整车CAN数据为扩展帧 

            }
            data[0].ID = obj.can_id;
            data[0].DataLen = obj.can_dlc;
            data[0].Data = obj.data;

            IntPtr ptrpTransmit_data = Marshal.AllocHGlobal((Marshal.SizeOf(obj)));


            if (Device.VCI_Transmit(Device.DevType, 0, Device.CanID, data, 1) == 1)
            {
                if (ctx != null && info.owner == 1234)
                {
                    // 转发到主线程处理，子线程不能处理界面
                    mainThreadSynContext.Post(new SendOrPostCallback(OnSendOneFrame), obj);
                }

                Marshal.FreeHGlobal(ptrpTransmit_data);
                return true;
            }

            Marshal.FreeHGlobal(ptrpTransmit_data);
            return false;
        }
        void OnSendOneFrame(object state)
        {
            VCI_CAN_OBJ one_frame = (VCI_CAN_OBJ)state;

        }

        /// <summary>
        /// 根据ID获取DBCMessage
        /// </summary>
        /// <param name="id">需要传输的帧ID</param>
        /// <param name="m_hDBC">DBC头文件</param>
        /// <returns></returns>
        public DBCMessage GetDBCMessageById(uint id, uint m_hDBC)
        {
            IntPtr ptr_msg = Marshal.AllocHGlobal((Marshal.SizeOf(typeof(DBCMessage))));
            Method.DBC_GetMessageById(m_hDBC, id, ptr_msg);
            DBCMessage msg = (DBCMessage)Marshal.PtrToStructure((IntPtr)((UInt32)ptr_msg), typeof(DBCMessage));
            Marshal.FreeHGlobal(ptr_msg);
            //DBCMessage msg = new DBCMessage();
            //foreach (DBCMessage item in m_vMsg)
            //{
            //    if (item.nID == id)
            //    {
            //        msg = item;
            //        for (int i = 0; i < msg.vSignals.Length; i++)
            //        {
            //            msg.vSignals[i].nValue = 0;

            //        }

            //    }
            //}
            return msg;
        }

        /// <summary>
        /// 根据DBC协议发送数据
        /// </summary>
        /// <param name="msg">需要传送的数据</param>
        /// <param name="m_hDBC">DBC头文件</param>
        public void DBCSendMsg(DBCMessage msg, uint m_hDBC)
        {
            IntPtr ptrpTransmit_data = Marshal.AllocHGlobal((Marshal.SizeOf(typeof(DBCMessage))));
            Marshal.StructureToPtr(msg, ptrpTransmit_data, false);
            Method.DBC_Send(m_hDBC, ptrpTransmit_data);
            Marshal.FreeHGlobal(ptrpTransmit_data);

        }

        public string  LoadDbcFile(string fileName, uint m_hDBC)
        {

            if (fileName.Length == 0)
            {
                MessageBox.Show("路径为空!");
            }

            FileInfoMsg fileinfo = new FileInfoMsg();
            fileinfo.strFilePath = new byte[261];
            fileinfo.type = Define.PROTOCOL_OTHER;
            fileinfo.merge = 0;
            byte[] temp = Encoding.GetEncoding("gb2312").GetBytes(fileName);
            for (int i = 0; i < temp.Length; i++)
            {
                fileinfo.strFilePath[i] = temp[i];
            }
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FileInfoMsg)));
            Marshal.StructureToPtr(fileinfo, ptr, true);

            if (!Method.DBC_LoadFile(m_hDBC, ptr))
            {

              
                return "加载文件失败!";
            }

            if (0 == Method.DBC_GetMessageCount(m_hDBC))
            {

               
                return "文件中不含有消息!";
            }

            Marshal.FreeHGlobal(ptr);
            ReadAllMessage(m_hDBC);
            return "DBC文件: " + Path.GetFileNameWithoutExtension(fileName) + " 导入成功！";

        }

        public void ReadAllMessage(uint m_hDBC)
        {
            DBCMessage msg = new DBCMessage();
            uint msg_count = Method.DBC_GetMessageCount(m_hDBC);
            IntPtr ptrFirstMsg = Marshal.AllocHGlobal(Marshal.SizeOf(msg));

            if (Method.DBC_GetFirstMessage(m_hDBC, ptrFirstMsg))
            {
                DBCMessage first_msg = (DBCMessage)Marshal.PtrToStructure((IntPtr)((UInt32)ptrFirstMsg), typeof(DBCMessage));
                DBCMessage msg_next = new DBCMessage();
                IntPtr ptrMsg = Marshal.AllocHGlobal(Marshal.SizeOf(msg_next));
                while (Method.DBC_GetNextMessage(m_hDBC, ptrMsg))
                {
                    DBCMessage next_msg = (DBCMessage)Marshal.PtrToStructure((IntPtr)((UInt32)ptrMsg), typeof(DBCMessage));
                }

                Marshal.FreeHGlobal(ptrMsg);
            }

            Marshal.FreeHGlobal(ptrFirstMsg);
        }


    }
}

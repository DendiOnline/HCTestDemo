using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControlCAN;
using Sunny.UI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using IDBCManager;
using System.Runtime.InteropServices;
using System.Threading;

namespace HCTestDemo
{


    public partial class ParameModifica : UserControl
    {
        Func<uint> func; //用以接收DBC句柄的委托
        //Action<double, int> ac;

        //参数读写
        List<int> checkIndedx = new List<int>();
        //int rownum = 0;
        bool ReadFlg = false;
        bool WriteFlg = false;

        //EXCEL读写部分
        //Thread th;
        IWorkbook iBook;
        ISheet iSheet;
        //ICellStyle iStyle;//声明style1对象，设置Excel表格的样式
        //IFont IFont;
        IRow irow;

        public const uint MCU_Parameters = 0x11C;
        public const uint PC_Parameters = 0x11E;

        public ParameModifica(Func<uint> fc)
        {
            func = fc;
            InitializeComponent();
        }
        #region 参数读写界面事件

        private void chooseFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDlg = new OpenFileDialog();
                fileDlg.Filter = "Excel文件|*.xlsx|Excel文件|*.xls";
                fileDlg.RestoreDirectory = true;
                fileDlg.InitialDirectory = Application.StartupPath + @"\ParaFile";
                fileDlg.FilterIndex = 1;
                int cloumnNum = 6; //读取EXCEL数据的列数
                if (fileDlg.ShowDialog() == DialogResult.OK)
                {
                    dgvMsgList.ClearAll();
                    excelPath.Text = fileDlg.FileName;
                    using (FileStream fs = File.OpenRead(excelPath.Text))
                    {
                        //这里需要根据文件名格式判断一下
                        //HSSF只能读取xls的
                        //XSSF只能读取xlsx格式的
                        if (Path.GetExtension(fs.Name) == ".xls")
                        {
                            iBook = new HSSFWorkbook(fs);
                        }
                        else if (Path.GetExtension(fs.Name) == ".xlsx")
                        {
                            iBook = new XSSFWorkbook(fs);
                        }
                        //先清零datagridview的行数
                        dgvMsgList.ClearRows();
                        //得到当前sheet
                        iSheet = iBook.GetSheetAt(0);

                        //添加表头,这一段比较通用
                        irow = iSheet.GetRow(iSheet.FirstRowNum);
                        for (int k = irow.FirstCellNum; k < cloumnNum; k++)
                        {

                            dgvMsgList.AddColumn(irow.Cells[k].ToString(), irow.Cells[k].ToString()).SetFixedMode(150);

                        }
                        dgvMsgList.AddCheckBoxColumn("选择", "选择").SetFixedMode(80);


                        //也可以通过GetSheet(name)得到
                        //遍历表中所有的行
                        //注意这里加1，这里得到的最后一个单元格的索引默认是从0开始的
                        for (int j = iSheet.FirstRowNum + 1; j <= iSheet.LastRowNum; j++)
                        {

                            //得到当前的行
                            irow = iSheet.GetRow(j);
                            //判断是否需要添加不是空的数据行
                            if (irow.Cells[1].ToString() != "")
                            {
                                dgvMsgList.AddRow();
                                //遍历每行所有的单元格
                                //注意这里不用加1，这里得到的最后一个单元格的索引默认是从1开始的
                                for (int k = irow.FirstCellNum; k < irow.LastCellNum; k++)
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
                    //避免最下面产生空行
                    dgvMsgList.AllowUserToAddRows = false;
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


        private void SelectAll_Click(object sender, EventArgs e)
        {
            if (dgvMsgList.RowCount != 0)
            {
                if (SelectAll.Text == "全选")
                {
                    SelectAll.Text = "全不选";
                    for (int i = 0; i < dgvMsgList.RowCount; i++)
                    {

                        ((DataGridViewCheckBoxCell)dgvMsgList.Rows[i].Cells["选择"]).Value = true;
                    }

                }
                else
                {
                    SelectAll.Text = "全选";
                    for (int i = 0; i < dgvMsgList.RowCount ; i++)
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
                for (int i = 0; i < dgvMsgList.RowCount; i++)
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
            try
            {
                if (dgvMsgList.RowCount != 0)
                {
                    if (Device.m_bOpen == 1)
                    {
                        DialogResult result = MessageBox.Show("是否需要覆盖参数源文件?", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        //避免误修改源参数文件
                        if (result == DialogResult.Yes)
                        {
                            //保存修改到EXCEL,x需要先文件流打开
                            using (FileStream fs = File.Open(excelPath.Text, FileMode.Open, FileAccess.Read))
                            {
                                iBook = new XSSFWorkbook(fs);
                                iSheet = iBook.GetSheetAt(0);
                                //写入数据
                                for (int i = 0; i < dgvMsgList.RowCount; i++)
                                {
                                    irow = iSheet.GetRow(i + 1);

                                    if ((bool)(dgvMsgList.Rows[i].Cells["选择"]).Value == true)
                                    {
                                        irow.GetCell(2).SetCellValue(Convert.ToDouble(dgvMsgList.Rows[i].Cells["EPPROM存储默认值"].Value));

                                    }

                                }
                                //这是NPOI的bug，保存的话必须创建新的excel取代原来的
                                FileStream sw = File.Create(excelPath.Text);
                                iBook.Write(sw);
                                sw.Close();
                            }
                        }
                        //写入请求发送
                        ReadFlg = false;
                        WriteFlg = true;
                        checkIndedx.Clear();

                        for (int i = 0; i < dgvMsgList.RowCount; i++)
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
                        for (int i = 0; i < dgvMsgList.RowCount; i++)
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
            if (e.ColumnIndex == 6)
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

        /// <summary>
        /// 读取发送参数循环指令帧
        /// </summary>
        /// <param name="sender"></param>
        void ParaReadWriteCmd()
        {
            try
            {
                if (Device.m_bOpen==1)
                {
                    for (int i = 0; i < checkIndedx.Count; i++)
                    {
                        if (ReadFlg)
                        {
                            if ((bool)(dgvMsgList.Rows[checkIndedx[i]].Cells["选择"]).Value == true)
                            {
                                DBCMessage msgRead = GetDBCMessageById(PC_Parameters);
                                msgRead.vSignals[3].nValue = Convert.ToDouble(dgvMsgList.Rows[checkIndedx[i]].Cells["参数序号"].Value);
                                msgRead.vSignals[1].nValue = 1;
                                DBCSendMsg(msgRead);
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

                                DBCMessage msgWrite = GetDBCMessageById(PC_Parameters);
                                msgWrite.vSignals[0].nValue = 1;
                                msgWrite.vSignals[2].nValue = para;
                                msgWrite.vSignals[3].nValue = Convert.ToDouble(dgvMsgList.Rows[checkIndedx[i]].Cells["参数序号"].Value);
                                DBCSendMsg(msgWrite);

                                BeginInvoke(new Action(() =>
                                {
                                    uiRoundProcess1.Value = i + 1;
                                }));
                            }

                        }
                        Thread.Sleep(50);
                    }
                    
                }
                else
                {
                    UIPage ui = new UIPage();
                    ui.ShowErrorTip("CAN盒未连接！");
                }
               
            }
            catch (Exception ex)
            {
                UIPage ui = new UIPage();
                ui.ShowWarningTip(ex.Message);
            }


        }


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
        /// 将主窗体传来的数据写入datagrid中
        /// </summary>
        /// <param name="value">数据值</param>
        /// <param name="index">写入的行下标</param>
       public void ReadPara(double value,int index)
        {
            //写入数据
            if (dgvMsgList.RowCount != 0 )
            {
                if ((bool)(dgvMsgList.Rows[index - 1].Cells["选择"]).Value == true)
                {
                    double para = value;
                    double num = Convert.ToDouble(dgvMsgList.Rows[index - 1].Cells["系数"].Value);
                    dgvMsgList.Rows[index - 1].Cells["EPPROM存储默认值"].Value = para / num;
                }
            }
        }
    }


}

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HCTestDemo
{
    public partial class LinearReg : Form
    {
        public LinearReg()
        {
            InitializeComponent();
        }

        private void LinearReg_Load(object sender, EventArgs e)
        {
             IWorkbook readBook;
            ISheet readSheet;
            IRow readIrow;
            ICell cellY;
            ICell cellX;
            List<double> xYvalue = new List<double>();
            List<double> yYvalue = new List<double>();

            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "Excel文件|*.xlsx";
            fileDlg.RestoreDirectory = true;
            fileDlg.InitialDirectory = Application.StartupPath;
            fileDlg.FilterIndex = 1;

            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = File.OpenRead(fileDlg.FileName))
                {
                    //这里需要根据文件名格式判断一下
                    //HSSF只能读取xls的
                    //XSSF只能读取xlsx格式的
                    readBook = new XSSFWorkbook(fs);
                    if (Path.GetExtension(fs.Name) == ".xls")
                    {
                        readBook = new HSSFWorkbook(fs);
                    }
                    readSheet = readBook.GetSheetAt(0);
                    for (int j = 1; j <= readSheet.LastRowNum; j++)
                    {
                        //得到当前的行
                        readIrow = readSheet.GetRow(j);
                        //将每行的第一和第二列赋值给两个list
                        cellX = readIrow.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        cellY = readIrow.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        if (readIrow.Cells[1].ToString() != "")
                        {
                            xYvalue.Add(Convert.ToDouble(cellX.ToString()));
                            yYvalue.Add(Convert.ToDouble(cellY.ToString()));
                        }
                    }

                }
                double molecule = 0;//分子
                double denominator = 0;//分母
                if (xYvalue.Count == yYvalue.Count)
                {
                   
                    for (int i = 0; i < xYvalue.Count; i++)
                    {
                        molecule += (xYvalue[i] - xYvalue.Average()) * (yYvalue[i] - yYvalue.Average());
                        denominator += (xYvalue[i] - xYvalue.Average()) * (xYvalue[i] - xYvalue.Average());
                    }
                    Slope.Text = (molecule / denominator).ToString("f6");//斜率
                    Intercept.Text = (yYvalue.Average() - (molecule / denominator) * xYvalue.Average()).ToString("f6");//截距
                }
                else
                {
                    UIPage ui = new UIPage();
                    ui.ShowWarningTip("数据输入有误，个数不对应！");
                    return;
                    
                }
                #region 画点
                chartLabTrend.ChartAreas[0].AxisX.CustomLabels.Clear();
                chartLabTrend.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chartLabTrend.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                List<double> lx = new List<double>();
                List<double> l1 = new List<double>();
                for (int i = 1; i <= xYvalue.Count; i++)
                {
                    CustomLabel label1 = new CustomLabel();
                    label1.Text = xYvalue[i - 1].ToString();
                    label1.ToPosition = i * 2;
                    label1.GridTicks = GridTickTypes.Gridline;
                    chartLabTrend.ChartAreas[0].AxisX.CustomLabels.Add(label1);
                    lx.Add(i);
                    l1.Add(yYvalue[i - 1]);
                }
                chartLabTrend.Series[0].ChartType = SeriesChartType.Point; //设置为点位
                chartLabTrend.Series[0].Name = "线性回归图";
                chartLabTrend.Series[0].Points.DataBindXY(lx, l1);
                #endregion

               
            }

        
        }
    }
}

﻿using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Region = NPOI.SS.Util.Region;

namespace HCTestDemo
{
    public class NpoiHelper
    {
        #region 从datatable中将数据导出到excel
        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        static MemoryStream ExportDT(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;

            #region 右击文件 属性信息


            #endregion

            HSSFCellStyle dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet() as HSSFSheet;
                    }

                    #region 表头及样式

                    {
                        HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        HSSFFont font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;



                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                        //headerRow.Dispose();
                    }

                    #endregion


                    #region 列头及样式

                    {
                        HSSFRow headerRow = sheet.CreateRow(1) as HSSFRow;


                        HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        HSSFFont font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);


                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);

                        }
                        //headerRow.Dispose();
                    }

                    #endregion

                    rowIndex = 2;
                }

                #endregion

                #region 填充内容

                HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                                              //double result;
                                              //if (isNumeric(drValue, out result))
                                              //{

                            //    double.TryParse(drValue, out result);
                            //    newCell.SetCellValue(result);
                            //    break;
                            //}
                            //else
                            //{
                            //    newCell.SetCellValue(drValue);
                            //    break;
                            //}

                            //字符串直出
                            newCell.SetCellValue(drValue);
                            break;



                        case "System.DateTime": //日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);

                            if (dateV.Year == 1)
                            { newCell.SetCellValue(""); }
                            else
                            { newCell.SetCellValue(dateV); }


                            newCell.CellStyle = dateStyle; //格式化显示
                            break;
                        case "System.Boolean": //布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }

                #endregion

                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                //sheet.Dispose();
                //workbook.Dispose();

                return ms;
            }
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        static void ExportDTI(DataTable dtSource, string strHeaderText, FileStream fs)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            XSSFSheet sheet = workbook.CreateSheet() as XSSFSheet;

            #region 右击文件 属性信息

            {
                //DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                //dsi.Company = "Wilson";
                //Workbook.DocumentSummaryInformation = dsi;

                //SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                //si.Author = "Wilson ERP"; //填加xls文件作者信息
                //si.ApplicationName = "Wilson ERP"; //填加xls文件创建程序信息
                //si.LastAuthor = "Wilson ERP"; //填加xls文件最后保存者信息
                //si.Comments = "人事文档"; //填加xls文件作者信息
                //si.Title = "人事资料导出"; //填加xls文件标题信息
                //si.Subject = "人事资料导出"; //填加文件主题信息
                //si.CreateDateTime = DateTime.Now;
                //Workbook.SummaryInformation = si;
            }

            #endregion

            XSSFCellStyle dateStyle = workbook.CreateCellStyle() as XSSFCellStyle;
            XSSFDataFormat format = workbook.CreateDataFormat() as XSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 0)
                {
                    #region 表头及样式
                    //{
                    //    XSSFRow headerRow = sheet.CreateRow(0) as XSSFRow;
                    //    headerRow.HeightInPoints = 25;
                    //    headerRow.CreateCell(0).SetCellValue(strHeaderText);

                    //    XSSFCellStyle headStyle = workbook.CreateCellStyle() as XSSFCellStyle;
                    //    headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                    //    XSSFFont font = workbook.CreateFont() as XSSFFont;
                    //    font.FontHeightInPoints = 20;
                    //    font.Boldweight = 700;
                    //    headStyle.SetFont(font);

                    //    headerRow.GetCell(0).CellStyle = headStyle;

                    //    //sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                    //    //headerRow.Dispose();
                    //}

                    #endregion


                    #region 列头及样式

                    {
                        XSSFRow headerRow = sheet.CreateRow(0) as XSSFRow;


                        XSSFCellStyle headStyle = workbook.CreateCellStyle() as XSSFCellStyle;
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        XSSFFont font = workbook.CreateFont() as XSSFFont;
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);


                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);

                        }
                        //headerRow.Dispose();
                    }

                    #endregion

                    rowIndex = 1;
                }

                #endregion

                #region 填充内容

                XSSFRow dataRow = sheet.CreateRow(rowIndex) as XSSFRow;
                foreach (DataColumn column in dtSource.Columns)
                {
                    XSSFCell newCell = dataRow.CreateCell(column.Ordinal) as XSSFCell;

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                            double result;
                            if (isNumeric(drValue, out result))
                            {

                                double.TryParse(drValue, out result);
                                newCell.SetCellValue(result);
                                break;
                            }
                            else
                            {
                                newCell.SetCellValue(drValue);
                                break;
                            }

                        case "System.DateTime": //日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle; //格式化显示
                            break;
                        case "System.Boolean": //布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }

                #endregion

                rowIndex++;
            }
            workbook.Write(fs);
            fs.Close();
        }

        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        public static void ExportDTtoExcel(DataTable dtSource, string strHeaderText, string strFileName)
        {
            string[] temp = strFileName.Split('.');

            if (temp[temp.Length - 1] == "xls" && dtSource.Columns.Count < 256 && dtSource.Rows.Count < 65536)
            {
                using (MemoryStream ms = ExportDT(dtSource, strHeaderText))
                {
                    using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                }
            }
            else
            {
                if (temp[temp.Length - 1] == "xls")
                    strFileName = strFileName + "x";

                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    ExportDTI(dtSource, strHeaderText, fs);
                }
            }
        }
        #endregion

        #region 从excel中将数据导出到datatable
        /// <summary>
        /// 读取excel 默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName)
        {
            DataTable dt = new DataTable();
            IWorkbook wb;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            ISheet sheet = wb.GetSheetAt(0);
            dt = ImportDt(sheet, 0, true);
            return dt;
        }

        /// <summary>
        /// 读取Excel流到DataTable
        /// </summary>
        /// <param name="stream">Excel流</param>
        /// <returns>第一个sheet中的数据</returns>
        public static DataTable ImportExceltoDt(Stream stream)
        {
            try
            {
                DataTable dt = new DataTable();
                IWorkbook wb;
                using (stream)
                {
                    wb = WorkbookFactory.Create(stream);
                }
                ISheet sheet = wb.GetSheetAt(0);
                dt = ImportDt(sheet, 0, true);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 读取Excel流到DataTable
        /// </summary>
        /// <param name="stream">Excel流</param>
        /// <param name="sheetName">表单名</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns>指定sheet中的数据</returns>
        public static DataTable ImportExceltoDt(Stream stream, string sheetName, int HeaderRowIndex)
        {
            try
            {
                DataTable dt = new DataTable();
                IWorkbook wb;
                using (stream)
                {
                    wb = WorkbookFactory.Create(stream);
                }
                ISheet sheet = wb.GetSheet(sheetName);
                dt = ImportDt(sheet, HeaderRowIndex, true);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 读取Excel流到DataSet
        /// </summary>
        /// <param name="stream">Excel流</param>
        /// <returns>Excel中的数据</returns>
        public static DataSet ImportExceltoDs(Stream stream)
        {
            try
            {
                DataSet ds = new DataSet();
                IWorkbook wb;
                using (stream)
                {
                    wb = WorkbookFactory.Create(stream);
                }
                for (int i = 0; i < wb.NumberOfSheets; i++)
                {
                    DataTable dt = new DataTable();
                    ISheet sheet = wb.GetSheetAt(i);
                    dt = ImportDt(sheet, 0, true);
                    ds.Tables.Add(dt);
                }
                return ds;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 读取Excel流到DataSet
        /// </summary>
        /// <param name="stream">Excel流</param>
        /// <param name="dict">字典参数，key：sheet名，value：列头所在行号，-1表示没有列头</param>
        /// <returns>Excel中的数据</returns>
        public static DataSet ImportExceltoDs(Stream stream, Dictionary<string, int> dict)
        {
            try
            {
                DataSet ds = new DataSet();
                IWorkbook wb;
                using (stream)
                {
                    wb = WorkbookFactory.Create(stream);
                }
                foreach (string key in dict.Keys)
                {
                    DataTable dt = new DataTable();
                    ISheet sheet = wb.GetSheet(key);
                    dt = ImportDt(sheet, dict[key], true);
                    ds.Tables.Add(dt);
                }
                return ds;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="sheet">需要导出的sheet</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, string SheetName, int HeaderRowIndex)
        {
            HSSFWorkbook workbook;
            IWorkbook wb;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = new HSSFWorkbook(file);
            }
            ISheet sheet = wb.GetSheet(SheetName);
            DataTable table = new DataTable();
            table = ImportDt(sheet, HeaderRowIndex, true);
            //ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="sheet">需要导出的sheet序号</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, int SheetIndex, int HeaderRowIndex)
        {
            HSSFWorkbook workbook;
            IWorkbook wb;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            ISheet isheet = wb.GetSheetAt(SheetIndex);
            DataTable table = new DataTable();
            table = ImportDt(isheet, HeaderRowIndex, true);
            //ExcelFileStream.Close();
            workbook = null;
            isheet = null;
            return table;
        }

        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="sheet">需要导出的sheet序号</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, int SheetIndex, string EndColIndex)
        {
            if (EndColIndex == "") EndColIndex = "0";

            HSSFWorkbook workbook;
            IWorkbook wb;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            ISheet isheet = wb.GetSheetAt(SheetIndex);
            DataTable table = new DataTable();
            table = ImportDt(isheet, int.Parse(EndColIndex));
            //ExcelFileStream.Close();
            workbook = null;
            isheet = null;
            return table;
        }


        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="sheet">需要导出的sheet</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, string SheetName, int HeaderRowIndex, bool needHeader)
        {
            HSSFWorkbook workbook;
            IWorkbook wb;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            ISheet sheet = wb.GetSheet(SheetName);
            DataTable table = new DataTable();
            table = ImportDt(sheet, HeaderRowIndex, needHeader);
            //ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="sheet">需要导出的sheet序号</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, int SheetIndex, int HeaderRowIndex, bool needHeader)
        {
            HSSFWorkbook workbook;
            IWorkbook wb;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            ISheet sheet = wb.GetSheetAt(SheetIndex);
            DataTable table = new DataTable();
            table = ImportDt(sheet, HeaderRowIndex, needHeader);
            //ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }




        /// <summary>
        /// 将指定列区间 sheet中的数据导入到datatable中
        /// </summary>
        /// <param name="sheet">需要导入的sheet</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        static DataTable ImportDt(ISheet sheet, int EndColIndex)
        {
            DataTable table = new DataTable();
            IRow headerRow;
            int cellCount;
            try
            {



                for (int i = 0; i <= EndColIndex; i++)
                {
                    DataColumn column = new DataColumn(Convert.ToString(i));
                    table.Columns.Add(column);
                }


                int rowCount = sheet.LastRowNum;
                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    try
                    {
                        IRow row;
                        if (sheet.GetRow(i) == null)
                        {
                            row = sheet.CreateRow(i);
                        }
                        else
                        {
                            row = sheet.GetRow(i);
                        }

                        DataRow dataRow = table.NewRow();

                        for (int j = 0; j <= EndColIndex; j++)
                        {
                            try
                            {
                                if (row.GetCell(j) != null)
                                {
                                    switch (row.GetCell(j).CellType)
                                    {
                                        case CellType.String:
                                            string str = row.GetCell(j).StringCellValue;
                                            if (str != null && str.Length > 0)
                                            {
                                                dataRow[j] = str.ToString();
                                            }
                                            else
                                            {
                                                dataRow[j] = null;
                                            }
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                            {
                                                dataRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                                            }
                                            else
                                            {
                                                dataRow[j] = Convert.ToDouble(row.GetCell(j).NumericCellValue);
                                            }
                                            break;
                                        case CellType.Boolean:
                                            dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                            break;
                                        case CellType.Error:
                                            dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                            break;
                                        case CellType.Formula:
                                            switch (row.GetCell(j).CachedFormulaResultType)
                                            {
                                                case CellType.String:
                                                    string strFORMULA = row.GetCell(j).StringCellValue;
                                                    if (strFORMULA != null && strFORMULA.Length > 0)
                                                    {
                                                        dataRow[j] = strFORMULA.ToString();
                                                    }
                                                    else
                                                    {
                                                        dataRow[j] = null;
                                                    }
                                                    break;
                                                case CellType.Numeric:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);
                                                    break;
                                                case CellType.Boolean:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                                    break;
                                                case CellType.Error:
                                                    dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                                    break;
                                                default:
                                                    dataRow[j] = "";
                                                    break;
                                            }
                                            break;
                                        default:
                                            dataRow[j] = "";
                                            break;
                                    }
                                }
                            }
                            catch (Exception exception)
                            {

                            }
                        }
                        table.Rows.Add(dataRow);
                    }
                    catch (Exception exception)
                    {

                    }
                }
            }
            catch (Exception exception)
            {

            }
            return table;
        }

        /// <summary>
        /// 将制定sheet中的数据导出到datatable中
        /// </summary>
        /// <param name="sheet">需要导出的sheet</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        static DataTable ImportDt(ISheet sheet, int HeaderRowIndex, bool needHeader)
        {
            DataTable table = new DataTable();
            IRow headerRow;
            int cellCount;
            try
            {
                if (HeaderRowIndex < 0 || !needHeader)
                {
                    headerRow = sheet.GetRow(0);
                    cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        DataColumn column = new DataColumn(Convert.ToString(i));
                        table.Columns.Add(column);
                    }
                }
                else
                {
                    headerRow = sheet.GetRow(HeaderRowIndex);
                    cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        if (headerRow.GetCell(i) == null)
                        {
                            if (table.Columns.IndexOf(Convert.ToString(i)) > 0)
                            {
                                DataColumn column = new DataColumn(Convert.ToString("重复列名" + i));
                                table.Columns.Add(column);
                            }
                            else
                            {
                                DataColumn column = new DataColumn(Convert.ToString(i));
                                table.Columns.Add(column);
                            }

                        }
                        else if (table.Columns.IndexOf(headerRow.GetCell(i).ToString()) > 0)
                        {
                            DataColumn column = new DataColumn(Convert.ToString("重复列名" + i));
                            table.Columns.Add(column);
                        }
                        else
                        {
                            DataColumn column = new DataColumn(headerRow.GetCell(i).ToString());
                            table.Columns.Add(column);
                        }
                    }
                }
                int rowCount = sheet.LastRowNum;
                for (int i = (HeaderRowIndex + 1); i <= sheet.LastRowNum; i++)
                {
                    try
                    {
                        IRow row;
                        if (sheet.GetRow(i) == null)
                        {
                            row = sheet.CreateRow(i);
                        }
                        else
                        {
                            row = sheet.GetRow(i);
                        }

                        DataRow dataRow = table.NewRow();

                        for (int j = row.FirstCellNum; j <= cellCount; j++)
                        {
                            try
                            {
                                if (row.GetCell(j) != null)
                                {
                                    switch (row.GetCell(j).CellType)
                                    {
                                        case CellType.String:
                                            string str = row.GetCell(j).StringCellValue;
                                            if (str != null && str.Length > 0)
                                            {
                                                dataRow[j] = str.ToString();
                                            }
                                            else
                                            {
                                                dataRow[j] = null;
                                            }
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                            {
                                                dataRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                                            }
                                            else
                                            {
                                                dataRow[j] = Convert.ToDouble(row.GetCell(j).NumericCellValue);
                                            }
                                            break;
                                        case CellType.Boolean:
                                            dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                            break;
                                        case CellType.Error:
                                            dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                            break;
                                        case CellType.Formula:
                                            switch (row.GetCell(j).CachedFormulaResultType)
                                            {
                                                case CellType.String:
                                                    string strFORMULA = row.GetCell(j).StringCellValue;
                                                    if (strFORMULA != null && strFORMULA.Length > 0)
                                                    {
                                                        dataRow[j] = strFORMULA.ToString();
                                                    }
                                                    else
                                                    {
                                                        dataRow[j] = null;
                                                    }
                                                    break;
                                                case CellType.Numeric:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);
                                                    break;
                                                case CellType.Boolean:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                                    break;
                                                case CellType.Error:
                                                    dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                                    break;
                                                default:
                                                    dataRow[j] = "";
                                                    break;
                                            }
                                            break;
                                        default:
                                            dataRow[j] = "";
                                            break;
                                    }
                                }
                            }
                            catch (Exception exception)
                            {

                            }
                        }
                        table.Rows.Add(dataRow);
                    }
                    catch (Exception exception)
                    {

                    }
                }
            }
            catch (Exception exception)
            {

            }
            return table;
        }

        #endregion


        public static void InsertSheet(string outputFile, string sheetname, DataTable dt)
        {
            FileStream readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);
            IWorkbook hssfworkbook = WorkbookFactory.Create(readfile);
            //HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
            int num = hssfworkbook.GetSheetIndex(sheetname);
            ISheet sheet1;
            if (num >= 0)
                sheet1 = hssfworkbook.GetSheet(sheetname);
            else
            {
                sheet1 = hssfworkbook.CreateSheet(sheetname);
            }


            try
            {
                if (sheet1.GetRow(0) == null)
                {
                    sheet1.CreateRow(0);
                }
                for (int coluid = 0; coluid < dt.Columns.Count; coluid++)
                {
                    if (sheet1.GetRow(0).GetCell(coluid) == null)
                    {
                        sheet1.GetRow(0).CreateCell(coluid);
                    }

                    sheet1.GetRow(0).GetCell(coluid).SetCellValue(dt.Columns[coluid].ColumnName);
                }
            }
            catch
            {

                throw;
            }


            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                try
                {
                    if (sheet1.GetRow(i) == null)
                    {
                        sheet1.CreateRow(i);
                    }
                    for (int coluid = 0; coluid < dt.Columns.Count; coluid++)
                    {
                        if (sheet1.GetRow(i).GetCell(coluid) == null)
                        {
                            sheet1.GetRow(i).CreateCell(coluid);
                        }

                        sheet1.GetRow(i).GetCell(coluid).SetCellValue(dt.Rows[i - 1][coluid].ToString());
                    }
                }
                catch
                {

                    //throw;
                }
            }
            try
            {
                readfile.Close();

                FileStream writefile = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch
            {

            }
        }

        #region 更新excel中的数据
        /// <summary>
        /// 更新Excel表格
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetname">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="coluid">需更新的列号</param>
        /// <param name="rowid">需更新的开始行号</param>
        public static void UpdateExcel(string outputFile, string sheetname, string[] updateData, int coluid, int rowid)
        {
            //FileStream readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);
            IWorkbook hssfworkbook = null;// WorkbookFactory.Create(outputFile);
                                          //HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
            ISheet sheet1 = hssfworkbook.GetSheet(sheetname);
            for (int i = 0; i < updateData.Length; i++)
            {
                try
                {
                    if (sheet1.GetRow(i + rowid) == null)
                    {
                        sheet1.CreateRow(i + rowid);
                    }
                    if (sheet1.GetRow(i + rowid).GetCell(coluid) == null)
                    {
                        sheet1.GetRow(i + rowid).CreateCell(coluid);
                    }

                    sheet1.GetRow(i + rowid).GetCell(coluid).SetCellValue(updateData[i]);
                }
                catch
                {

                    throw;
                }
            }
            try
            {
                //readfile.Close();
                FileStream writefile = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch
            {

            }

        }

        /// <summary>
        /// 更新Excel表格
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetname">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="coluids">需更新的列号</param>
        /// <param name="rowid">需更新的开始行号</param>
        public static void UpdateExcel(string outputFile, string sheetname, string[][] updateData, int[] coluids, int rowid)
        {
            FileStream readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
            readfile.Close();
            ISheet sheet1 = hssfworkbook.GetSheet(sheetname);
            for (int j = 0; j < coluids.Length; j++)
            {
                for (int i = 0; i < updateData[j].Length; i++)
                {
                    try
                    {
                        if (sheet1.GetRow(i + rowid) == null)
                        {
                            sheet1.CreateRow(i + rowid);
                        }
                        if (sheet1.GetRow(i + rowid).GetCell(coluids[j]) == null)
                        {
                            sheet1.GetRow(i + rowid).CreateCell(coluids[j]);
                        }
                        sheet1.GetRow(i + rowid).GetCell(coluids[j]).SetCellValue(updateData[j][i]);
                    }
                    catch
                    {

                    }
                }
            }
            try
            {
                FileStream writefile = new FileStream(outputFile, FileMode.Create);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 更新Excel表格
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetname">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="coluid">需更新的列号</param>
        /// <param name="rowid">需更新的开始行号</param>
        public static void UpdateExcel(string outputFile, string sheetname, double[] updateData, int coluid, int rowid)
        {
            FileStream readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
            ISheet sheet1 = hssfworkbook.GetSheet(sheetname);
            for (int i = 0; i < updateData.Length; i++)
            {
                try
                {
                    if (sheet1.GetRow(i + rowid) == null)
                    {
                        sheet1.CreateRow(i + rowid);
                    }
                    if (sheet1.GetRow(i + rowid).GetCell(coluid) == null)
                    {
                        sheet1.GetRow(i + rowid).CreateCell(coluid);
                    }

                    sheet1.GetRow(i + rowid).GetCell(coluid).SetCellValue(updateData[i]);
                }
                catch
                {

                    throw;
                }
            }
            try
            {
                readfile.Close();
                FileStream writefile = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch
            {

            }

        }

        /// <summary>
        /// 更新Excel表格
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetname">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="coluids">需更新的列号</param>
        /// <param name="rowid">需更新的开始行号</param>
        public static void UpdateExcel(string outputFile, string sheetname, double[][] updateData, int[] coluids, int rowid)
        {
            FileStream readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
            readfile.Close();
            ISheet sheet1 = hssfworkbook.GetSheet(sheetname);
            for (int j = 0; j < coluids.Length; j++)
            {
                for (int i = 0; i < updateData[j].Length; i++)
                {
                    try
                    {
                        if (sheet1.GetRow(i + rowid) == null)
                        {
                            sheet1.CreateRow(i + rowid);
                        }
                        if (sheet1.GetRow(i + rowid).GetCell(coluids[j]) == null)
                        {
                            sheet1.GetRow(i + rowid).CreateCell(coluids[j]);
                        }
                        sheet1.GetRow(i + rowid).GetCell(coluids[j]).SetCellValue(updateData[j][i]);
                    }
                    catch
                    {

                    }
                }
            }
            try
            {
                FileStream writefile = new FileStream(outputFile, FileMode.Create);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch
            {

            }
        }

        #endregion

        public static int GetSheetNumber(string outputFile)
        {
            int number = 0;
            try
            {
                FileStream readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

                HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
                number = hssfworkbook.NumberOfSheets;

            }
            catch (Exception exception)
            {

            }
            return number;
        }

        public static ArrayList GetSheetName(string outputFile)
        {
            ArrayList arrayList = new ArrayList();
            try
            {
                FileStream readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

                HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
                for (int i = 0; i < hssfworkbook.NumberOfSheets; i++)
                {
                    arrayList.Add(hssfworkbook.GetSheetName(i));
                }
            }
            catch (Exception exception)
            {

            }
            return arrayList;
        }

        public static bool isNumeric(String message, out double result)
        {
            Regex rex = new Regex(@"^[-]?\d+[.]?\d*$");
            result = -1;
            if (rex.IsMatch(message))
            {
                result = double.Parse(message);
                return true;
            }
            else
                return false;

        }


        /// <summary>
        /// DataTable导出到Excel的MemoryStream                                                                      第二步
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "文件作者信息"; //填加xls文件作者信息
                si.ApplicationName = "创建程序信息"; //填加xls文件创建程序信息
                si.LastAuthor = "最后保存者信息"; //填加xls文件最后保存者信息
                si.Comments = "作者信息"; //填加xls文件作者信息
                si.Title = "标题信息"; //填加xls文件标题信息
                si.Subject = "主题信息";//填加文件主题信息

                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            HSSFCellStyle dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet() as HSSFSheet;
                    }

                    #region 表头及样式
                    {
                        if (string.IsNullOrEmpty(strHeaderText))
                        {
                            HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                            headerRow.HeightInPoints = 25;
                            headerRow.CreateCell(0).SetCellValue(strHeaderText);
                            HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                            //headStyle.Alignment = CellHorizontalAlignment.CENTER;
                            HSSFFont font = workbook.CreateFont() as HSSFFont;
                            font.FontHeightInPoints = 20;
                            font.Boldweight = 700;
                            headStyle.SetFont(font);
                            headerRow.GetCell(0).CellStyle = headStyle;
                            sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                            //headerRow.Dispose();
                        }
                    }
                    #endregion

                    #region 列头及样式
                    {
                        HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                        HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                        //headStyle.Alignment = CellHorizontalAlignment.CENTER;
                        HSSFFont font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        //headerRow.Dispose();
                    }
                    #endregion

                    rowIndex = 1;
                }
                #endregion


                #region 填充内容
                HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }
                #endregion

                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                //sheet.Dispose();
                //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet
                return ms;
            }
        }



        /// <summary>
        /// 由DataSet导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="sheetName">工作表名称</param>
        /// <returns>Excel工作表</returns>
        private static MemoryStream ExportDataSetToExcel(DataSet sourceDs, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            string[] sheetNames = sheetName.Split(',');
            for (int i = 0; i < sheetNames.Length; i++)
            {
                ISheet sheet = workbook.CreateSheet(sheetNames[i]);

                #region 列头
                IRow headerRow = sheet.CreateRow(0);
                HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                HSSFFont font = workbook.CreateFont() as HSSFFont;
                font.FontHeightInPoints = 10;
                font.Boldweight = 700;
                headStyle.SetFont(font);

                //取得列宽
                int[] arrColWidth = new int[sourceDs.Tables[i].Columns.Count];
                foreach (DataColumn item in sourceDs.Tables[i].Columns)
                {
                    arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                }

                // 处理列头
                foreach (DataColumn column in sourceDs.Tables[i].Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                    headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                    //设置列宽
                    sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);

                }
                #endregion

                #region 填充值
                int rowIndex = 1;
                foreach (DataRow row in sourceDs.Tables[i].Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
                #endregion
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            workbook = null;
            return ms;
        }


        /// <summary>
        /// 验证导入的Excel是否有数据
        /// </summary>
        /// <param name="excelFileStream"></param>
        /// <returns></returns>
        public static bool HasData(Stream excelFileStream)
        {
            using (excelFileStream)
            {
                IWorkbook workBook = new HSSFWorkbook(excelFileStream);
                if (workBook.NumberOfSheets > 0)
                {
                    ISheet sheet = workBook.GetSheetAt(0);
                    return sheet.PhysicalNumberOfRows > 0;
                }
            }
            return false;
        }


        //参数说明
        //第一个:指定操作的Sheet。
        //第二个:指定在第几行指入（插入行的位置）
        //第三个:指定要插入多少行
        //第四个:源单元格格式的行， IRow mySourceStyleRow = mySheet.GetRow(InsertRowIndex - 1);//获取源格式行
        private void InsertRow(ISheet sheet, int InsertRowIndex, int InsertRowCount, IRow SourceStyleRow)
        {
            #region 批量移动行
            sheet.ShiftRows(
                InsertRowIndex,                                 //--开始行
                sheet.LastRowNum,                      //--结束行
                InsertRowCount,                             //--移动大小(行数)--往下移动
                true,                                  //是否复制行高
                false//,                               //是否重置行高
                     //true                                 //是否移动批注
            );
            #endregion

            #region 对批量移动后空出的空行插，创建相应的行，并以插入行的上一行为格式源(即：插入行-1的那一行)
            for (int i = InsertRowIndex; i < InsertRowIndex + InsertRowCount - 1; i++)
            {
                IRow targetRow = null;
                ICell sourceCell = null;
                ICell targetCell = null;

                targetRow = sheet.CreateRow(i + 1);

                for (int m = SourceStyleRow.FirstCellNum; m < SourceStyleRow.LastCellNum; m++)
                {
                    sourceCell = SourceStyleRow.GetCell(m);
                    if (sourceCell == null)
                        continue;
                    targetCell = targetRow.CreateCell(m);

                    //targetCell..Encoding = sourceCell.Encoding;
                    targetCell.CellStyle = sourceCell.CellStyle;
                    targetCell.SetCellType(sourceCell.CellType);
                }
                //CopyRow(sourceRow, targetRow);
                //Util.CopyRow(sheet, sourceRow, targetRow);
            }

            IRow firstTargetRow = sheet.GetRow(InsertRowIndex);
            ICell firstSourceCell = null;
            ICell firstTargetCell = null;

            for (int m = SourceStyleRow.FirstCellNum; m < SourceStyleRow.LastCellNum; m++)
            {
                firstSourceCell = SourceStyleRow.GetCell(m);
                if (firstSourceCell == null)
                    continue;
                firstTargetCell = firstTargetRow.CreateCell(m);

                //firstTargetCell.Encoding = firstSourceCell.Encoding;
                firstTargetCell.CellStyle = firstSourceCell.CellStyle;
                firstTargetCell.SetCellType(firstSourceCell.CellType);
            }
            #endregion
        }



        //数据库对单元格按数据类型输出
        public static void outDBtoEXCELcell(ISheet mySheet, int RowIndex, int CowIndex, DataTable mydt, int vrow, string vcol)
        {
            string drValue = mydt.Rows[vrow][vcol].ToString();

            switch (mydt.Columns[vcol].DataType.ToString())
            {
                case "System.String"://字符串类型   
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue(drValue);
                    break;
                case "System.DateTime"://日期类型   
                    DateTime dateV;
                    DateTime.TryParse(drValue, out dateV);
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue(dateV);

                    break;
                case "System.Boolean"://布尔型   
                    bool boolV = false;
                    bool.TryParse(drValue, out boolV);
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue(boolV);
                    break;
                case "System.Int16"://整型   
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(drValue, out intV);
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue(intV);
                    break;
                case "System.Decimal"://浮点型   
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(drValue, out doubV);
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue(doubV);
                    break;
                case "System.DBNull"://空值处理   
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue("");
                    break;
                default:
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue("");
                    break;
            }

        }
        public static void outDBtoEXCELcell(HSSFSheet mySheet, int RowIndex, int CowIndex, DataTable mydt, int vrow, int vcol)
        {
            string drValue = mydt.Rows[vrow][vcol].ToString();

            switch (mydt.Columns[vcol].DataType.ToString())
            {
                case "System.String"://字符串类型   
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue(drValue);
                    break;
                case "System.DateTime"://日期类型   
                    DateTime dateV;
                    DateTime.TryParse(drValue, out dateV);
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue(dateV);

                    break;
                case "System.Boolean"://布尔型   
                    bool boolV = false;
                    bool.TryParse(drValue, out boolV);
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue(boolV);
                    break;
                case "System.Int16"://整型   
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(drValue, out intV);
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue(intV);
                    break;
                case "System.Decimal"://浮点型   
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(drValue, out doubV);
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue(doubV);
                    break;
                case "System.DBNull"://空值处理   
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue("");
                    break;
                default:
                    mySheet.GetRow(RowIndex).GetCell(CowIndex).SetCellValue("");
                    break;
            }

        }



    }
    
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Utility
{
    /// <summary>
    /// Excel操作类
    /// </summary>
    /// <remarks>
    /// FileName: 	ExcelHelper.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/28 9:47:01
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public class ExcelHelper
    {

        #region 日期单元格的显示格式

        private static string _dataType = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 日期单元格的显示格式
        /// </summary>
        private static ICellStyle _dateStyle;
        private static IDataFormat _dataFormat;


        #endregion

        #region 导出到Excel

        #region dataTable

        /// <summary>
        /// 导出数据到Excel
        /// </summary>
        /// <param name="dataTable">要导出的数据</param>
        public static byte[] ExportToArray(DataTable dataTable)
        {
            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            using (var stream = new MemoryStream())
            {
                ExportToStream(dataSet, stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 导出数据到Excel
        /// </summary>
        /// <param name="dataTable">要导出的数据</param>
        /// <param name="fileName">要导出的文件名</param>
        public static void ExportToFile(DataTable dataTable, string fileName)
        {
            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            using (Stream stream = File.OpenWrite(fileName))
            {
                ExportToStream(dataSet, stream);
            }
        }

        /// <summary>
        /// 导出数据到Excel
        /// </summary>
        /// <param name="dataTable">要导出的数据</param>
        /// <param name="stream">流</param>
        public static void ExportToStream(DataTable dataTable, Stream stream)
        {
            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            ExportToStream(dataSet, stream);
        }

        #endregion

        #region DataSet导出到Excel

        /// <summary>
        /// 导出Excel文件到数组
        /// </summary>
        /// <param name="dataSet">要导出的数据</param>
        public static byte[] ExportToArray(DataSet dataSet)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                ExportToStream(dataSet, stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 导出数据到Excel文件
        /// </summary>
        /// <param name="dataSet">要导出的数据</param>
        /// <param name="fileName">要导出的文件名</param>
        /// <exception cref="ArgumentException">参数异常</exception>
        public static void ExportToFile(DataSet dataSet, string fileName)
        {
            using (Stream stream = File.OpenWrite(fileName))
            {
                ExportToStream(dataSet, stream);
            }
        }

        /// <summary>
        /// 导出数据到数据流
        /// </summary>
        /// <param name="dataSet">要导出的数据</param>
        /// <param name="stream">要导出到的数据流</param>
        /// <exception cref="ArgumentException">参数异常</exception>
        public static void ExportToStream(DataSet dataSet, Stream stream)
        {
            if (dataSet == null) throw new ArgumentException("无效的导入数据", "dataSet");
            if (stream == null) throw new ArgumentException("目标数据流无效", "stream");

            //创建工作簿
            IWorkbook workbook = new HSSFWorkbook();
            //创建日期的显示样式
            _dateStyle = workbook.CreateCellStyle();
            _dataFormat = workbook.CreateDataFormat();
            _dateStyle.DataFormat = _dataFormat.GetFormat(_dataType);
            DataTableCollection tables = dataSet.Tables;
            for (int i = 0; i < tables.Count; i++)  //每张表
            {
                //当前表的总行数
                DataRowCollection rows = tables[i].Rows;
                //如果表里面没有数据,忽略这个表
                if (rows.Count < 1) continue;
                //当前表的列集合
                var columns = tables[i].Columns;
                //创建表
                string tableName = string.IsNullOrEmpty(tables[i].TableName) ? "Sheet" + i : tables[i].TableName;
                var sheet = workbook.CreateSheet(tableName);
                //创建表头
                var titleRow = sheet.CreateRow(0);
                for (int j = 0; j < columns.Count; j++)
                {
                    string columnName = string.IsNullOrEmpty(columns[j].ColumnName) ? "Cell" + j : columns[j].ColumnName;
                    var cell = titleRow.CreateCell(j);
                    cell.SetCellValue(columnName);
                }
                //定义行开始索引
                int rowIndex = 1;
                foreach (DataRow dataRow in tables[i].Rows)     //每一行
                {
                    var row = sheet.CreateRow(rowIndex);
                    rowIndex++;
                    for (int j = 0; j < columns.Count; j++)     //每一个格子
                    {
                        //var cellValue = dataRow[j] == DBNull.Value ? string.Empty : dataRow[j].ToString();
                        var cell = row.CreateCell(j);
                        //如果该属性为null,设置单元格为空格
                        if (dataRow[j] == null)
                        {
                            cell.SetCellType(CellType.Blank);
                            return;
                        }
                        //判断单元格类型
                        switch (columns[j].DataType.Name.ToLower())
                        {
                            case "char":
                            case "string":
                                cell.SetCellValue(Convert.ToString(dataRow[j]));
                                break;
                            case "double":
                            case "single":
                            case "int32":
                                cell.SetCellValue(Convert.ToDouble(dataRow[j]));
                                break;
                            case "boolean":
                                cell.SetCellValue(Convert.ToBoolean(dataRow[j]));
                                break;
                            case "datetime":
                                cell.SetCellValue(Convert.ToDateTime(dataRow[j]));
                                cell.CellStyle = _dateStyle;
                                break;
                            default:
                                cell.SetCellValue(dataRow[j].ToString());
                                break;
                        }

                    }
                }

            }
            workbook.Write(stream);
        }

        #endregion

        #region IEnumerable导出到Excel

        /// <summary>
        /// 导出Excel文件到数组
        /// </summary>
        /// <param name="dataTable">要导出的数据</param>
        public static byte[] ExportToArray(IEnumerable<object> dataTable)
        {
            List<IEnumerable<object>> dataSet = new List<IEnumerable<object>>();
            dataSet.Add(dataTable);
            using (MemoryStream stream = new MemoryStream())
            {
                ExportToStream(dataSet, stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 导出Excel文件到数组
        /// </summary>
        /// <param name="dataSet">要导出的数据</param>
        public static byte[] ExportToArray(IEnumerable<IEnumerable<object>> dataSet)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                ExportToStream(dataSet, stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 导出数据到Excel文件
        /// </summary>
        /// <param name="dataTable">要导出的数据</param>
        /// <param name="fileName">要导出的文件名</param>
        /// <exception cref="ArgumentException">参数异常</exception>
        public static void ExportToFile(IEnumerable<object> dataTable, string fileName)
        {
            List<IEnumerable<object>> dataSet = new List<IEnumerable<object>>();
            dataSet.Add(dataTable);
            ExportToFile(dataSet, fileName);
        }

        /// <summary>
        /// 导出数据到Excel文件
        /// </summary>
        /// <param name="dataSet">要导出的数据</param>
        /// <param name="fileName">要导出的文件名</param>
        /// <exception cref="ArgumentException">参数异常</exception>
        public static void ExportToFile(IEnumerable<IEnumerable<object>> dataSet, string fileName)
        {
            using (Stream stream = File.OpenWrite(fileName))
            {
                ExportToStream(dataSet, stream);
            }
        }

        /// <summary>
        /// 导出数据到数据流
        /// </summary>
        /// <param name="dataTable">要导出的数据</param>
        /// <param name="stream">要导出到的数据流</param>
        /// <exception cref="ArgumentException">参数异常</exception>
        public static void ExportToStream(IEnumerable<object> dataTable, Stream stream)
        {
            List<IEnumerable<object>> dataSet = new List<IEnumerable<object>>();
            dataSet.Add(dataTable);
            ExportToStream(dataSet, stream);
        }

        /// <summary>
        /// 导出数据到数据流
        /// </summary>
        /// <param name="dataSet">要导出的数据</param>
        /// <param name="stream">要导出到的数据流</param>
        /// <exception cref="ArgumentException">参数异常</exception>
        public static void ExportToStream(IEnumerable<IEnumerable<object>> dataSet, Stream stream)
        {
            if (dataSet == null) throw new ArgumentException("无效的导入数据", "dataSet");
            if (stream == null) throw new ArgumentException("目标数据流无效", "stream");

            //创建工作簿
            IWorkbook workbook = new HSSFWorkbook();
            //创建日期的显示样式
            _dateStyle = workbook.CreateCellStyle();
            _dataFormat = workbook.CreateDataFormat();
            _dateStyle.DataFormat = _dataFormat.GetFormat(_dataType);
            //遍历每张表
            foreach (IEnumerable<object> dataTable in dataSet) //每张表
            {
                if (dataTable == null || !dataTable.Any()) continue;
                //获取表中第一个不为空的元素
                var obj = dataTable.FirstOrDefault(o => o != null);
                //如果没有不为空的元素,跳过这张表
                if (obj == null) continue;
                //获取该表中的对象的属性
                var columns = obj.GetType().GetProperties();
                //创建表
                ISheet sheet = workbook.CreateSheet();
                SetTable(sheet, dataTable);
            }
            workbook.Write(stream);
        }

        /// <summary>
        /// 设置单元格
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="property"></param>
        /// <param name="obj"></param>
        private static void SetCell(ICell cell, PropertyInfo property, object obj)
        {
            //如果该属性为null,设置单元格为空格
            if (property.GetValue(obj, null) == null)
            {
                cell.SetCellType(CellType.Blank);
                return;
            }
            //判断属性类型
            switch (property.PropertyType.Name.ToLower())
            {
                case "char":
                case "string":
                    cell.SetCellValue(Convert.ToString(property.GetValue(obj, null)));
                    break;
                case "double":
                case "single":
                case "int32":
                    cell.SetCellValue(Convert.ToDouble(property.GetValue(obj, null)));
                    break;
                case "boolean":
                    cell.SetCellValue(Convert.ToBoolean(property.GetValue(obj, null)));
                    break;
                case "datetime":
                    cell.SetCellValue(Convert.ToDateTime(property.GetValue(obj, null)));
                    cell.CellStyle = _dateStyle;
                    break;
                default:
                    cell.SetCellValue(property.GetValue(property, null).ToString());
                    break;
            }
        }

        /// <summary>
        /// 设置行
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        private static void SetRow(IRow row, object obj)
        {
            //获取属性
            var columns = obj.GetType().GetProperties();
            //一个属性一个格
            for (int i = 0; i < obj.GetType().GetProperties().Length; i++)
            {
                //创建格
                ICell cell = row.CreateCell(i);
                //设置单元格的内容
                SetCell(cell, columns[i], obj);
            }
        }

        /// <summary>
        /// 设置表
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="dataTable"></param>
        private static void SetTable(ISheet sheet, IEnumerable<object> dataTable)
        {
            var columns = dataTable.FirstOrDefault(o => o != null).GetType().GetProperties();
            //创建表头
            IRow title = sheet.CreateRow(0);
            for (int i = 0; i < columns.Length; i++)
            {
                ICell cell = title.CreateCell(i);
                cell.SetCellValue(columns[i].Name);
            }
            //定义行的开始索引
            int rowIndex = 1;
            foreach (var dataRow in dataTable) //每一行
            {
                if (dataRow == null)
                {
                    //创建一个空行
                    sheet.CreateRow(rowIndex);
                    rowIndex++;
                    continue;
                }

                //创建行
                IRow row = sheet.CreateRow(rowIndex);
                rowIndex++;
                //设置行
                SetRow(row, dataRow);
            }
        }

        #endregion

        #endregion

        #region 从Excel导入

        /// <summary>
        /// 将Excel的数据导入到DataSet中
        /// </summary>
        /// <param name="fileName">完整文件名</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSetFormFile(string fileName)
        {
            if (!File.Exists(fileName)) throw new FileNotFoundException("未找到文件",fileName);
            using (Stream stream =File.OpenRead(fileName))
            {
                return GetDataSetFormStream(stream);
            }
        }

        /// <summary>
        /// 将Excel的数据导入到DataSet里面
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSetFormStream(Stream stream)
        {
            //如果文件不存在
            if (stream == null) throw new NullReferenceException("stream不可为null");

            //WorkBook和DataSet
            IWorkbook workBook = null;

            workBook = new HSSFWorkbook(stream);
            var set = new DataSet();

            for (var i = 0; i < workBook.NumberOfSheets; i++)  //遍历每个Sheet
            {
                //Sheet和DataTable
                var sheet = workBook.GetSheetAt(i);
                var table = new DataTable(sheet.SheetName);

                //表头
                var headerRow = sheet.GetRow(0);
                for (int j = 0; j < headerRow.LastCellNum; j++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(j).StringCellValue);
                    table.Columns.Add(column);
                }


                //内容
                for (int j = 1; j <= sheet.LastRowNum; j++) //每一行
                {
                    var row = sheet.GetRow(j);
                    if (row == null) continue;
                    var dataRow = table.NewRow();

                    for (int k = 0; k < headerRow.LastCellNum; k++) //每一格
                    {
                        var cell = row.GetCell(k);
                        if (cell == null) continue;
                        switch (cell.CellType)
                        {
                            case CellType.Blank:
                                dataRow[cell.ColumnIndex] = null;
                                break;
                            case CellType.Boolean:
                                dataRow[cell.ColumnIndex] = cell.BooleanCellValue;
                                break;
                            case CellType.Numeric:
                                if (HSSFDateUtil.IsCellDateFormatted(cell))//日期类型
                                {
                                    dataRow[cell.ColumnIndex] = cell.DateCellValue;
                                }
                                else//其他数字类型
                                {
                                    dataRow[cell.ColumnIndex] = cell.NumericCellValue;
                                }
                                break;
                            default:
                                dataRow[cell.ColumnIndex] = cell.StringCellValue;
                                break;
                        }
                    }
                    table.Rows.Add(dataRow);
                }
                set.Tables.Add(table);
            }
            return set;
        }

        #endregion
    }
}

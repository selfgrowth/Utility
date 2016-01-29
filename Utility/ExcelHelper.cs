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

        //public static void ExportToFile(DataSet dataSet,string fileName)
        //{
        //    //1966-800-100 
        //    //2714
        //    dataSet.Tables[0].Rows[0].
        //}

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
                ExportToStream(dataSet,stream);
            }
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
    }
}

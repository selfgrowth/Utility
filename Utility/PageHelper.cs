using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    /// <summary>
    /// 分页帮助类
    /// </summary>
    /// <remarks>
    /// 2016/3/12 16:53:55 Created By Devin
    /// </remarks>
    public static class PageHelper
    {
        public static string RenderPager(string urlFormat, int total, int size, int current = 1, int showCount = 9, string ulContainerClass = "pagination", string liActiveClass = "active")
        {
            StringBuilder result = new StringBuilder();
            //算出总页数
            int totalPage = (int)Math.Ceiling(total * 1.0 / size);
            int start, end;
            #region 计算开始页到结束页
            //如果总页数比想要显示的页数还小,直接循环开始到结束
            if (total <= showCount)
            {
                start = 1;
                end = totalPage;
            }
            else
            {
                //当前页的索引
                var currentIndex = showCount % 2 == 0 ? showCount / 2 - 1 : showCount / 2;
                start = current - currentIndex;
                start = start <= 0 ? 1 : start;
                end = start + showCount - 1;
                if (end > totalPage)
                {
                    end = totalPage;
                    start = end - showCount + 1;
                }
            }
            #endregion

            #region 生成标签
            result.AppendLine("<ul class=\"" + ulContainerClass + "\">");
            //判断要不要生成首页和上一页
            if (current > 1)
            {
                result.AppendLine("<li><a href=\"" + string.Format(urlFormat, 1) + "\">首页</a></li>");
                result.AppendLine("<li><a href=\"" + string.Format(urlFormat, current - 1) + "\">上一页</a></li>");
            }
            if (start > 1)    //判断要不要生成...
            {
                result.AppendLine("<li><a href=\"javascript:void(0);\">...</a></li>");
            }

            //生成标签
            for (int i = start; i <= end; i++)
            {
                if (i == current)
                {
                    result.AppendLine("<li class=\"" + liActiveClass + "\"><a href=\"" + string.Format(urlFormat, current) + "\">" + current + "</a></li>");
                    continue;
                }
                result.AppendLine("<li><a href=\"" + string.Format(urlFormat, i) + "\">" + i + "</a></li>");
            }
            if (end < totalPage)  //判断要不要生成...
            {
                result.AppendLine("<li><a href=\"javascript:void(0);\">...</a></li>");
            }
            if (current < totalPage) //判断要不要生成下一页和末页
            {
                result.AppendLine("<li><a href=\"" + string.Format(urlFormat, current + 1) + "\">下一页</a></li>");
                result.AppendLine("<li><a href=\"" + string.Format(urlFormat, totalPage) + "\">末页</a></li>");
            }
            result.AppendLine("</ul>");
            #endregion

            return result.ToString();
        }
    }
}

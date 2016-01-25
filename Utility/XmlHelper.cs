using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace System.Xml.Linq
{
    /// <summary>
    /// 操作XML的辅助类
    /// </summary>
    /// <remarks>
    /// FileName: 	XmlHelper.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/25 13:17:09
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public class XmlHelper
    {
        private readonly XDocument _doc;

        /// <summary>
        /// 根据磁盘文件创建一个XmlHelper对象
        /// </summary>
        /// <param name="filePath">要操作那个XML文件的完整文件名</param>
        public XmlHelper(string filePath)
        {
            this._doc = XDocument.Load(filePath);
        }

        /// <summary>
        /// 根据流创建一个XmlHelper对象
        /// </summary>
        /// <param name="stream">流</param>
        public XmlHelper(Stream stream)
        {
            this._doc = XDocument.Load(stream);
        }

        /// <summary>
        /// 创建一个新Xml文档,根据该文档创建一个XmlHelper对象
        /// </summary>
        /// <param name="version"></param>
        /// <param name="encoding"></param>
        /// <param name="standalone"></param>
        /// <param name="rootName"></param>
        public XmlHelper(string version, string encoding, string standalone,string rootName)
        {
            this._doc = new XDocument();
            XDeclaration declaration = new XDeclaration(version,encoding,standalone);
            this._doc.Declaration = declaration;
            this._doc.Add(new XElement(rootName)); 
        }

        #region 查询

        /// <summary>
        /// 获取xPath匹配到的第一个元素
        /// </summary>
        /// <param name="xPath">xPath表达式</param>
        /// <returns>匹配到的XElement对象</returns>
        public XElement GetXElement(string xPath)
        {
            return _doc.XPathSelectElement(xPath);
        }

        /// <summary>
        /// 获取xPath匹配到的第一个元素的文本
        /// </summary>
        /// <param name="xPath">xPath表达式</param>
        /// <returns>匹配到的节点的值</returns>
        public string GetXElementText(string xPath)
        {
            XElement element = _doc.XPathSelectElement(xPath);
            if (element == null) return null;
            return element.ToString();
        }

        /// <summary>
        /// 获取xpath匹配所有元素
        /// </summary>
        /// <param name="xPath">xPath表达式</param>
        /// <returns>存放匹配到的XElement的集合</returns>
        public IEnumerable<XElement> GetXElements(string xPath)
        {
            return _doc.XPathSelectElements(xPath);
        }

        /// <summary>
        /// 获取第一个被xPath匹配到的元素的属性,如果属性不存在,返回""
        /// </summary>
        /// <param name="xPath">xPath表达式</param>
        /// <param name="attributeName">要获取的属性</param>
        /// <returns>属性内容</returns>
        public string GetAttributeValue(string xPath, string attributeName)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(attributeName))
            {
                XElement element = _doc.XPathSelectElement(xPath);
                if (element == null) return result;
                if (!element.HasAttributes) return result;
                XAttribute attr = element.Attribute(attributeName);
                if (attr == null) return result;
                result = attr.Value;
            }
            return result;
        }


        #endregion

        #region 增加

        /// <summary>
        /// 在xPath匹配到的第一个节点下追加一个子节点
        /// </summary>
        /// <param name="xPath">xPath表达式</param>
        /// <param name="name">节点名称</param>
        /// <param name="innerText">节点内的文本</param>
        /// <param name="attributes">属性集合</param>
        /// <returns>true表示追加成功,false表示追加失败</returns>
        public bool AppendXElement(string xPath, string name, string innerText, IDictionary<string, string> attributes)
        {
            XElement element = GetXElement(xPath);
            if (element == null) return false;
            XElement childElement = new XElement(name);
            childElement.Value = innerText;
            if (attributes != null && attributes.Count > 0)
            {
                foreach (var attr in attributes)
                {
                    XAttribute attribute = new XAttribute(attr.Key, attr.Value);
                    childElement.Add(attribute);
                }
            }
            element.Add(childElement);
            return true;
        }
        /// <summary>
        /// 在xPath匹配到的第一个节点下追加一个子节点
        /// </summary>
        /// <param name="xPath">xPath表达式</param>
        /// <param name="childElement">要追加的节点</param>
        /// <returns>true表示追加成功,false表示追加失败</returns>
        public bool AppendXElement(string xPath, XElement childElement)
        {
            XElement element = GetXElement(xPath);
            if (element == null) return false;
            element.Add(childElement);
            return true;
        }

        /// <summary>
        /// 在xPath匹配到的第一个节点下追加多个子节点
        /// </summary>
        /// <param name="xPath">xPath表达式</param>
        /// <param name="elements">要追加的节点集合</param>
        /// <returns>true表示追加成功,false表示追加失败</returns>
        public bool AppendRange(string xPath, IEnumerable<XElement> elements)
        {
            XElement element = GetXElement(xPath);
            if (element == null) return false;
            if (elements != null && elements.Count() > 0)
            {
                foreach (var xElement in elements)
                {
                    element.Add(xElement);
                }
            }
            return true;
        }

        /// <summary>
        /// 给xPath匹配到的第一个节点追加属性.
        /// 对于该节点已存在的属性,将会覆盖这个属性的值
        /// </summary>
        /// <param name="xPath">xPath表达式</param>
        /// <param name="attributes">属性集合</param>
        /// <returns>true表示成功,false表示失败</returns>
        public bool SetAttribute(string xPath, IDictionary<string, string> attributes)
        {
            var element = GetXElement(xPath);
            if (element == null) return false;
            if (attributes == null || attributes.Count <= 0)return false;
            foreach (var attr in attributes)
            {
                //XAttribute attribute = new XAttribute(attr.Key, attr.Value);
                //childElement.Add(attribute);
                XAttribute attribute = element.Attribute(attr.Key);
                if (attribute==null)    //表示不存在该属性
                {
                    attribute = new XAttribute(attr.Key, attr.Value);
                }
                else        //存在该属性
                {
                    attribute.Value = attr.Value;
                }
            }
            return true;
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改xPath匹配到的节点的值
        /// </summary>
        /// <param name="xPath">xPath表达式</param>
        /// <param name="value">要修改的值</param>
        /// <returns>true表示修改成功,false表示修改失败</returns>
        public bool Update(string xPath,string value)
        {
            XElement element= GetXElement(xPath);
            if (element == null) return false;
            element.Value = value;
            return true;
        }

        /// <summary>
        /// 修改xPath匹配到的节点的属性
        /// </summary>
        /// <param name="xPath">xPath表达式</param>
        /// <param name="name">属性名</param>
        /// <param name="value">要修改的值</param>
        /// <returns>true表示修改成功,false表示修改失败</returns>
        public bool UpdateAttribute(string xPath,string name, string value)
        {
            XElement element = GetXElement(xPath);
            if (element == null) return false;
            if (!element.HasAttributes) return false;
            XAttribute attribute = element.Attribute(name);
            if (attribute == null) return false;
            attribute.Value = value;
            return true;
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除被xPath匹配的节点
        /// </summary>
        /// <param name="xPath"></param>
        public void Remove(string xPath)
        {
            var elements = GetXElements(xPath);
            if (elements == null || elements.Count() <= 0) return;
            foreach (var ele in elements)
            {
                ele.Remove();
            }
        }

        /// <summary>
        /// 删除被xPath匹配的节点下的属性
        /// </summary>
        /// <param name="xPath">xPath表达式</param>
        /// <param name="name">要移除的属性名</param>
        public void RemoveAttribute(string xPath,string name)
        {
            var element = GetXElement(xPath);
            if (element == null) return;
            var attribute = element.Attribute(name);
            if (attribute == null) return;
            attribute.Remove();
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存到磁盘
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            _doc.Save(path);
        }

        /// <summary>
        /// 保存到流
        /// </summary>
        /// <param name="stream">流</param>
        public void Save(Stream stream)
        {
            _doc.Save(stream);
        }

        #endregion
    }
}

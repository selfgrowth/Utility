using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Utility
{
    /// <summary>
    /// Ftp操作类
    /// </summary>
    /// <remarks>
    /// FileName: 	FtpClient.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/27 14:42:04
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public class FtpClient
    {
        /// <summary>
        /// ftp链接
        /// </summary>
        private string _ftpServerIp;

        /// <summary>
        /// 用户名
        /// </summary>
        private string _ftpUserId;

        /// <summary>
        /// 密码
        /// </summary>
        private string _ftpPassword;

        /// <summary>
        /// 超时时间
        /// </summary>
        private int _timeout;

        /// <summary>
        /// 完整链接
        /// </summary>
        private readonly string _ftpUrlFormat = "ftp://{0}/{1}/{2}";



        /// <summary>
        /// 创建FtpClient对象
        /// </summary>
        /// <param name="ftpServerIp">ftp链接地址,不用写协议头.如:127.0.0.1</param>
        /// <param name="ftpUserId">用户名</param>
        /// <param name="ftpPassword">密码</param>
        public FtpClient(string ftpServerIp, string ftpUserId, string ftpPassword)
        {
            if (string.IsNullOrEmpty(ftpServerIp))
            {
                throw new ArgumentException("ftp链接不能为空!", "ftpServerIp");
            }
            this._ftpServerIp = ftpServerIp;
            this._ftpUserId = ftpUserId;
            this._ftpPassword = ftpPassword;
        }

        /// <summary>
        /// 创建FtpClient对象
        /// </summary>
        /// <param name="ftpServerIp">ftp链接,不用写协议头.如:127.0.0.1</param>
        /// <param name="timeout">默认的超时时间</param>
        /// <param name="ftpUserId">用户名</param>
        /// <param name="ftpPassword">密码</param>
        public FtpClient(string ftpServerIp, int timeout, string ftpUserId, string ftpPassword)
        {
            if (string.IsNullOrEmpty(ftpServerIp))
            {
                throw new ArgumentException("ftp链接不能为空!", "ftpServerIp");
            }
            this._ftpServerIp = ftpServerIp;
            this._ftpUserId = ftpUserId;
            this._ftpPassword = ftpPassword;
            this._timeout = timeout;
        }

        /// <summary>         
        /// 建立FTP链接,返回请求对象         
        /// </summary>        
        /// <param name="uri">FTP地址</param>         
        /// <param name="ftpMethod">操作命令</param>         
        private FtpWebRequest Connect(string uri, string ftpMethod)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            //设置ftp的行为,上传还是下载等等
            request.Method = ftpMethod;
            request.UseBinary = true;
            request.KeepAlive = false;
            request.Timeout = _timeout;
            request.Credentials = new NetworkCredential(this._ftpUserId, this._ftpPassword);
            return request;
        }

        #region 上传

        public void Upload(string remotePath, string remoteFileName, Stream stream)
        {
            if (stream == null) return;
            string ftpUrl = string.Format(_ftpUrlFormat, _ftpServerIp, remotePath, remoteFileName);
            FtpWebRequest request = Connect(ftpUrl, WebRequestMethods.Ftp.UploadFile);
        }

        #endregion

        #region 下载



        #endregion

        #region 文件操作



        #endregion
    }
}

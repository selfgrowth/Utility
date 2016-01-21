/*
 源码己托管:https://github.com/v5bep7/Utility
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Utility
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2016/1/15 18:04:59 Created By Devin
    /// </remarks>
    public class MailHelper
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _userName;
        private readonly string _displayName;
        private readonly string _password;
        private readonly bool _enableSsl;

        /// <summary>
        /// 构建一个发送邮件帮助类
        /// </summary>
        /// <param name="smtp">smtp服务器</param>
        /// <param name="port">端口.如果不知道请填-1</param>
        /// <param name="displayName">显示名字</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="enableSsl">是否启用SSL加密</param>
        public MailHelper(string smtp, int port, string displayName, string userName, string password, bool enableSsl)
        {
            this._smtpServer = smtp;
            this._smtpPort = port;
            this._userName = userName;
            this._displayName = displayName;
            this._password = password;
            this._enableSsl = enableSsl;
        }


        /// <summary>
        /// 发送邮件到指定收件人
        /// </summary>
        /// <param name="toAddress">收件人地址列表</param>
        /// <param name="subject">主题</param>
        /// <param name="mailBody">正文内容(支持HTML)</param>
        /// <param name="ccs">抄送地址列表</param>
        /// <param name="bccs">密件抄送地址列表</param>
        /// <param name="priority">此邮件的优先级</param>
        /// <param name="attachments">附件列表</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string[] toAddress, string subject, string mailBody, string[] ccs, string[] bccs, MailPriority priority, params Attachment[] attachments)
        {
            //如果没有目的地
            if (toAddress == null) { throw new ArgumentNullException("toAddress"); }
            if (toAddress.Length == 0) { return false; }
            //创建Email实体
            var message = new MailMessage();
            message.From = new MailAddress(_userName, _displayName);
            message.Subject = subject;
            message.Body = mailBody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Priority = priority;

            //插入收件人地址,抄送地址和密件抄送地址
            foreach (var to in toAddress.Where(c => !string.IsNullOrEmpty(c)))
            {
                message.To.Add(new MailAddress(to));
            }

            //如果有附件
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(attachment);
                }
            }
            //如果有抄送
            if (ccs != null)
            {
                foreach (var cc in ccs.Where(c => !string.IsNullOrEmpty(c)))
                {
                    message.CC.Add(new MailAddress(cc));
                }
            }
            //如果有密送
            if (bccs != null)
            {
                foreach (var bcc in bccs.Where(c => !string.IsNullOrEmpty(c)))
                {
                    message.Bcc.Add(new MailAddress(bcc));
                }
            }
            //创建SMTP客户端
            var client = new SmtpClient
            {
                Host = _smtpServer,
                Credentials = new System.Net.NetworkCredential(_userName, _password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = _enableSsl
            };
            if (_smtpPort != -1)
            {
                client.Port = _smtpPort;
            }
            //client.SendCompleted += Client_SendCompleted;
            try
            {
                //发送邮件
                client.Send(message);
                //client.SendAsync(message,DateTime.Now.ToString());

                message.Dispose();
                client.Dispose();

                return true;
            }
            catch (Exception)
            {
                message.Dispose();
                client.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 发送邮件到指定收件人
        /// </summary>
        /// <param name="toAddress">收件人地址列表</param>
        /// <param name="subject">主题</param>
        /// <param name="mailBody">正文内容(支持HTML)</param>
        /// <param name="ccs">抄送地址列表</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string toAddress, string subject, string mailBody, params string[] ccs)
        {
            return Send(new string[] { toAddress }, subject, mailBody, ccs, null, MailPriority.Normal);
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;



namespace CapturaCognitiva.App_Tools
{
    public class CapturaCognitivaController : Controller
    {
        #region Variables       

        /// <summary>
        /// The page size
        /// </summary>
        protected short PageSize;
        /// <summary>
        /// True when proccess is success or fals when other wase.
        /// </summary>
        protected bool Success;
        /// <summary>
        /// Gets or sets the urls.
        /// </summary>
        /// <value>
        /// The urls.
        /// </value>
        private List<Breadcrumb> Urls = new List<Breadcrumb>();
        /// <summary>
        /// The message list
        /// </summary>
        private List<Msg> MsgList = new List<Msg>();
        #endregion

        #region Builders
        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreController"/> class.
        /// </summary>
        public CapturaCognitivaController(short pageSize = 15)
        {
            PageSize = pageSize;
        }
        #endregion

        #region Messages
        /// <summary>
        /// Messages the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="type">The type.</param>
        protected void Message(string message, MessageType type = MessageType.Success, string title = "")
        {
            if (string.IsNullOrEmpty(title))
            {
                switch (type)
                {
                    case MessageType.Success:
                        title = "Exitoso !!!";
                        break;
                    case MessageType.Info:
                        title = "Información";
                        break;
                    case MessageType.Warning:
                        title = "Alerta !!!";
                        break;
                    case MessageType.Danger:
                        title = "Alerta !!!";
                        break;
                    case MessageType.Smile:
                        title = "Exitoso !!!";
                        break;
                    case MessageType.Sad:
                        title = "Error !!!";
                        break;
                    default:
                        title = "Mensaje";
                        break;
                }
            }

            MsgList.Add(
                new Msg
                {
                    IsNotification = false,
                    Text = message,
                    Title = title,
                    Type = type
                });

            ViewBag.MessageList = MsgList;
        }
        /// <summary>Messages the specified message.</summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        /// <param name="title">The title.</param>
        protected void Message(string message, string type, string title = "")
        {
            MessageType msgType = MessageType.Info;
            if (string.IsNullOrEmpty(title))
            {
                switch (type)
                {
                    case "Success":
                        title = "Exitoso !!!";
                        msgType = MessageType.Success;
                        break;
                    case "Info":
                        title = "Información";
                        msgType = MessageType.Info;
                        break;
                    case "Warning":
                        title = "Alerta !!!";
                        msgType = MessageType.Warning;
                        break;
                    case "Danger":
                        title = "Alerta !!!";
                        msgType = MessageType.Danger;
                        break;
                    case "Smile":
                        title = "Exitoso !!!";
                        msgType = MessageType.Smile;
                        break;
                    case "Sad":
                        title = "Error !!!";
                        msgType = MessageType.Sad;
                        break;
                    default:
                        title = "Mensaje";
                        break;
                }
            }

            MsgList.Add(
                new Msg
                {
                    IsNotification = false,
                    Text = message,
                    Title = title,
                    Type = msgType
                });

            ViewBag.MessageList = MsgList;
        }
        /// <summary>
        /// Messages the specified message.
        /// </summary>
        /// <param name="exception">The exception</param>
        protected void Message(Exception exception)
        {
            MsgList.Add(
                new Msg
                {
                    IsNotification = false,
                    Title = "Alerta !!!",
                    Type = MessageType.Warning
                });

            ViewBag.MessageList = MsgList;
        }

        #endregion
    }
    /// <summary>
    /// List items of breadcrumb 
    /// </summary>
    public class Breadcrumb
    {
        public Breadcrumb() { }
        public bool Active { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public object Parameters { get; set; }
        public string Text { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Msg
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is notification.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is notification; otherwise, <c>false</c>.
        /// </value>
        public bool IsNotification { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public MessageType Type { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }
    }
    /// <summary>
    /// The enum that enunciate the messages types
    /// </summary>
    public enum MessageType
    {
        Success,
        Info,
        Warning,
        Danger,
        Smile,
        Sad
    }

    public class TKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}
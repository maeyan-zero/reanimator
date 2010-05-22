using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Reanimator.Forms;
using System.Xml.Serialization;

namespace Reanimator
{
    public class ExceptionFormat
    {
        [XmlElement("Date")]
        string _date;
        [XmlElement("Time")]
        string _time;
        [XmlElement("Function")]
        string _function;
        [XmlElement("Source")]
        string _source;
        [XmlElement("TargetSite")]
        string _targetSite;
        [XmlElement("InnerException")]
        string _innerException;
        [XmlElement("ExceptionMessage")]
        string _exceptionMessage;
        [XmlElement("StackTrace")]
        string _stackTrace;
        [XmlElement("CustomMessage")]
        string _customMessage;

        #region Properties
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public string TargetSite
        {
            get { return _targetSite; }
            set { _targetSite = value; }
        }

        public string Function
        {
            get { return _function; }
            set { _function = value; }
        }

        public string InnerException
        {
            get { return _innerException; }
            set { _innerException = value; }
        }

        public string ExceptionMessage
        {
            get { return _exceptionMessage; }
            set { _exceptionMessage = value; }
        }

        public string StackTrace
        {
            get { return _stackTrace; }
            set { _stackTrace = value; }
        }

        public string CustomMessage
        {
            get { return _customMessage; }
            set { _customMessage = value; }
        }
        #endregion

        public ExceptionFormat()
        {
        }

        public ExceptionFormat(Exception exception, string functionName, string customMessage)
        {
            _date = DateTime.Now.ToLongDateString();
            _time = DateTime.Now.ToLongTimeString();
            _function = functionName;
            _source = exception.Source;
            _targetSite = exception.TargetSite.Name;
            if (exception.InnerException != null)
            {
                _innerException = exception.InnerException.ToString();
            }
            _exceptionMessage = exception.Message;
            _stackTrace = exception.StackTrace;
            _customMessage = customMessage;
        }

        public ExceptionFormat(Exception exception, string functionName) : this(exception, functionName, "")
        {
        }

        public override string ToString()
        {
            return
                _date + Environment.NewLine +
                _time + Environment.NewLine +
                _function + Environment.NewLine +
                _source + Environment.NewLine +
                _targetSite + Environment.NewLine +
                _innerException + Environment.NewLine +
                _exceptionMessage + Environment.NewLine +
                _stackTrace + Environment.NewLine +
                _customMessage;
        }
    }

    public class ExceptionLogger
    {
        const string FILENAME = "errorLog.xml";
        static ExceptionLogger _logger;

        [XmlArray("Exceptions")]
        static List<ExceptionFormat> _exceptions;

        private ExceptionLogger()
        {
            _exceptions = new List<ExceptionFormat>();
        }

        //public static ExceptionLogger GetInstance()
        //{
        //    if (_logger == null)
        //    {
        //        _logger = new ExceptionLogger();
        //    }

        //    return _logger;
        //}

        public static void LogException(Exception exception)
        {
            LogException(exception, "Not specified", FILENAME, "");
        }

        public static void LogException(Exception exception, string functionName)
        {
            LogException(exception, functionName, FILENAME, "");
        }

        public static void LogException(Exception exception, string functionName, string customMessage)
        {
            LogException(exception, functionName, FILENAME, customMessage);
        }

        public static void LogException(Exception exception, string functionName, string fileName, string customMessage)
        {
            if (_logger == null)
            {
                _logger = new ExceptionLogger();
            }

            _exceptions.Add(new ExceptionFormat(exception, functionName, customMessage));

            XmlUtilities<List<ExceptionFormat>>.Serialize(_exceptions, fileName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Revival.Common
{
    [XmlRoot("Exceptions")]
    public class ExceptionsLog
    {
        private const int MaxExceptions = 150; // maintain a rolling-log of 100-150 exceptions
        private const int TrimExceptions = 50;

        [XmlElement("Exception")]
        public List<ExceptionFormat> Exceptions { get; set; }

        public ExceptionsLog()
        {
            Exceptions = new List<ExceptionFormat>();
        }

        public void Add(ExceptionFormat exceptionFormat)
        {
            Exceptions.Add(exceptionFormat);

            if (Exceptions.Count > MaxExceptions)
            {
                Exceptions.RemoveRange(0, TrimExceptions);
            }
        }
    }

    [XmlRoot("Exception")]
    public class ExceptionFormat
    {
        public String Date { get; set; }
        public String Time { get; set; }
        public String Source { get; set; }
        public String TargetSite { get; set; }
        public String InnerException { get; set; }
        public String ExceptionMessage { get; set; }
        public String StackTrace { get; set; }
        public String CustomMessage { get; set; }

        public ExceptionFormat() { } // needed for XML serialization

        public ExceptionFormat(Exception exception, String customMessage = "")
        {
            Date = DateTime.Now.ToLongDateString();
            Time = DateTime.Now.ToLongTimeString();
            Source = exception.Source;

            if (exception.TargetSite != null)
            {
                TargetSite = exception.TargetSite.Name;
            }
            if (exception.InnerException != null)
            {
                InnerException = exception.InnerException.ToString();
            }

            ExceptionMessage = exception.Message;
            StackTrace = exception.StackTrace;
            CustomMessage = customMessage;
        }

        public override string ToString()
        {
            return
                Date + Environment.NewLine +
                Time + Environment.NewLine +
                Source + Environment.NewLine +
                TargetSite + Environment.NewLine +
                InnerException + Environment.NewLine +
                ExceptionMessage + Environment.NewLine +
                StackTrace + Environment.NewLine +
                CustomMessage;
        }
    }

    public class ExceptionLogger
    {
        private const String FileName = "errorLog.xml";

        private static readonly Object Lock = new Object();
        private static volatile ExceptionLogger _logger;

        private readonly ExceptionsLog _exceptionsLog;
        private readonly XmlSerializer _serializer;
        private readonly bool _isReadOnly;

        /// <summary>
        /// (private-Singleton) Constructor 
        /// Is only called once in _GetInstace();
        /// </summary>
        private ExceptionLogger()
        {
            FileInfo logFileInfo = new FileInfo(FileName);
            if (logFileInfo.IsReadOnly)
            {
                _isReadOnly = true;
                MessageBox.Show("Log file is Read Only!\nNo exception logging will occur for this instance!", "Log Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            _serializer = new XmlSerializer(typeof(ExceptionsLog));
            if (logFileInfo.Exists && logFileInfo.Length > 0)
            {
                try
                {
                    _exceptionsLog = _Deserialize<ExceptionsLog>(FileName);
                    return;
                }
                catch (Exception)
                {
                    MessageBox.Show("Log file load .Deserialize() failed! A new one will be created...");
                }
            }

            _exceptionsLog = new ExceptionsLog();
        }

        public static void LogException(Exception exception, bool logSilent = false, String customMessage = "")
        {
            _logger = _GetInstance();
            if (_logger._isReadOnly) return;

            if (!logSilent)
            {
                MessageBox.Show(exception.Message + "\n\n" + exception.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            ExceptionFormat exceptionFormat = new ExceptionFormat(exception, customMessage);

            lock (Lock)
            {
                _logger._AddExceptionFormat(exceptionFormat);
                _logger._Serialize();
            }
        }

        private static ExceptionLogger _GetInstance()
        {
            if (_logger == null)
            {
                lock (Lock)
                {
                    if (_logger == null)
                    {
                        _logger = new ExceptionLogger();
                    }
                }
            }

            return _logger;
        }

        private void _AddExceptionFormat(ExceptionFormat exceptionFormat)
        {
            _exceptionsLog.Add(exceptionFormat);
        }

        private void _Serialize()
        {
            if (_isReadOnly) return;
            Debug.Assert(_exceptionsLog != null);

            using (TextWriter textWriter = new StreamWriter(FileName))
            {
                _serializer.Serialize(textWriter, _exceptionsLog);
                textWriter.Close();
            }
        }

        private T _Deserialize<T>(String path)
        {
            using (TextReader textReader = new StreamReader(path))
            {
                T obj = (T)_serializer.Deserialize(textReader);
                textReader.Close();

                return obj;
            }
        }
    }
}
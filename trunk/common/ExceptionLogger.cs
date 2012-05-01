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
        private const int AskLessThan = 3;

        private static readonly Object Lock = new Object();
        private static volatile ExceptionLogger _logger;

        private readonly ExceptionsLog _exceptionsLog;
        private readonly XmlSerializer _serializer;
        private readonly bool _isReadOnly;
        private int _errorsShown;

        public bool HideErrors { get; set; }

        /// <summary>
        /// (private-Singleton) Constructor 
        /// Is only called once in _GetInstace();
        /// </summary>
        private ExceptionLogger()
        {
            FileInfo logFileInfo = new FileInfo(FileName);
            if (logFileInfo.Exists && logFileInfo.IsReadOnly)
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

        public static void LogException(Exception exception, String extraDetails)
        {
            LogException(exception, false, extraDetails);
        }

        public static void LogException(Exception exception, bool logSilent = false, String extraDetails = "")
        {
            _logger = _GetInstance();
            if (_logger._isReadOnly) return;

            if (!_logger.HideErrors && !logSilent)
            {
                MessageBox.Show(exception.Message + "\n\n" + exception.StackTrace, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _logger._errorsShown++;

                if (_logger._errorsShown >= AskLessThan)
                {
                    DialogResult dr = MessageBox.Show("Show further errors?", "Exception Messages", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes) _logger._errorsShown = (1 << 31);
                    if (dr == DialogResult.No) _logger.HideErrors = true;
                    if (dr == DialogResult.Cancel) _logger._errorsShown = 0;
                }
            }

            ExceptionFormat exceptionFormat = new ExceptionFormat(exception, extraDetails);

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
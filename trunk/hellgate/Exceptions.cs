using System;

namespace Hellgate
{
    public static class Exceptions
    {
        public class ReanimatorException : Exception
        {
            protected String CustomMessage;

            public override string ToString()
            {
                return String.Format("{0}\n{1}", CustomMessage, StackTrace);
            }
        }

        public class UnexpectedMagicWordException : ReanimatorException
        {
            public UnexpectedMagicWordException()
            {
                CustomMessage = "Unexpected file header Magic Word!";
            }
        }

        public class NotSupportedFileVersionException : ReanimatorException
        {
            public NotSupportedFileVersionException()
            {
                CustomMessage = "Unexpected file header version!";
            }
        }

        public class NotInitializedException : ReanimatorException
        {
            public NotInitializedException()
            {
                CustomMessage = "Class Not Initialized!";
            }
        }

        public class NotSupportedFileDefinitionException : ReanimatorException
        {
            public NotSupportedFileDefinitionException()
            {
                CustomMessage = "File definition type not supported!";
            }
        }

        public class UnexpectedTokenException : ReanimatorException
        {
            public UnexpectedTokenException(String str)
            {
                CustomMessage = str;
            }
        }

        public class UnknownExcelElementException : ReanimatorException
        {
            public UnknownExcelElementException(String excelElement)
            {
                if (String.IsNullOrEmpty(excelElement)) excelElement = String.Empty;
                CustomMessage = "An unknown Excel Element was encountered!\n" + excelElement;
            }
        }

        public class UnknownExcelCodeException : ReanimatorException
        {
            public UnknownExcelCodeException(int code)
            {
                CustomMessage = "Unknown Excel Table code value = " + code;
            }
        }

        public class NotSupportedXmlElementCount : ReanimatorException
        {
            public NotSupportedXmlElementCount(String excelElement)
            {
                if (String.IsNullOrEmpty(excelElement)) excelElement = String.Empty;
                CustomMessage = "The XML Definition for the file has less elements than the file has defined!\n" + excelElement;
            }
        }

        public class DataFileStringIdNotFound : ReanimatorException
        {
            public DataFileStringIdNotFound(String filePath)
            {
                if (String.IsNullOrEmpty(filePath)) filePath = String.Empty;
                CustomMessage = "Unable to find an appropriate StringId for the DataFile: " + filePath + "\n";
            }
        }

        public class InvalidXmlElement : ReanimatorException
        {
            public InvalidXmlElement(String element, String reason)
            {
                CustomMessage = String.Format("The XML Element \"{0}\" is invalid: {1}", element, reason);
            }
        }

        public class InvalidXmlDocument : ReanimatorException
        {
            public InvalidXmlDocument(String reason)
            {
                CustomMessage = "The XML Document is invalid.\n" + reason;
            }
        }

    }
}
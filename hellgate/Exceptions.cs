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

            public NotSupportedFileVersionException(uint expected, uint got)
            {
                CustomMessage = String.Format("File version {0} not supported. Only {1} supported.", got, expected);
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

            public UnexpectedTokenException(uint expected, uint got)
            {
                CustomMessage = String.Format("Expected token {0} but got {1} instead.", expected.ToString("X"), got.ToString("X"));
            }
        }

        public class UnknownExcelStringException : ReanimatorException
        {
            public UnknownExcelStringException(String excelString, int offset=-1)
            {
                String offsetStr = ".";
                if (offset > 0) offsetStr = String.Format(" at offset {0}.", offset);

                CustomMessage = String.Format("An unknown Excel string '{0}' was encountered{1}", excelString, offsetStr);
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

        public class ScriptInvalidStackStateException : ReanimatorException
        {
            public ScriptInvalidStackStateException(String reason)
            {
                CustomMessage = reason;
            }
        }

        public class ScriptUnknownOpCodeException : ReanimatorException
        {
            public ScriptUnknownOpCodeException(String opCode, String stack)
            {
                CustomMessage = String.Format("Unknown OpCode: {0}\n{1}", opCode, stack);
            }
        }

        public class ScriptInfiniteCheckException : ReanimatorException
        {
            public ScriptInfiniteCheckException(String opCode, String stack)
            {
                CustomMessage = String.Format("Warning: Forced loop beak - infinite Loop check override (>= 1000 loops).\nLast Op Code = {0}\n{1}", opCode, stack);
            }
        }

        public class ScriptUnexpectedFunctionIndexException : ReanimatorException
        {
            public ScriptUnexpectedFunctionIndexException(int functionIndex)
            {
                CustomMessage = String.Format("Unexpected function index '{0}'.", functionIndex);
            }
        }

        public class ScriptNotInitialisedException : ReanimatorException
        {
            public ScriptNotInitialisedException(String reason)
            {
                CustomMessage = reason;
            }
        }

        public class ScriptUnknownFunctionException : ReanimatorException
        {
            public ScriptUnknownFunctionException(String functionName)
            {
                CustomMessage = String.Format("The function '{0}' could not be found!", functionName);
            }
        }

        public class ScriptFunctionArgumentCountException : ReanimatorException
        {
            public ScriptFunctionArgumentCountException(String functionName, int requiredCount, String extra=null)
            {
                if (extra == null) extra = String.Empty;
                CustomMessage = String.Format("The function '{0}' did not have the required argument count of '{1}'.{2}", functionName, requiredCount, extra);
            }
        }

        public class ScriptFormatException : ReanimatorException
        {
            public ScriptFormatException(String details, int scriptOffset)
            {
                CustomMessage = String.Format("A script format exception has occured at offset '{0}':\n{1}", scriptOffset, details);
            }
        }

        public class ScriptUnknownVarNameException : ReanimatorException
        {
            public ScriptUnknownVarNameException(String varName, int scriptOffset)
            {
                CustomMessage = String.Format("At offset '{0}', an unknown variable name '{1}' was encountered.", scriptOffset, varName);
            }
        }

        public class ScriptInvalidArgTypeException : ReanimatorException
        {
            public ScriptInvalidArgTypeException(String varName, String argType, int scriptOffset)
            {
                CustomMessage = String.Format("An invalid argument '{0}' was supplied at offset '{1}'. Expecting type '{2}'.", varName, scriptOffset, argType);
            }
        }

        public class ScriptUnexpectedScriptTerminationException : ReanimatorException
        {
            public ScriptUnexpectedScriptTerminationException()
            {
                CustomMessage = "The script string terminates unexpectedly.";
            }
        }

        public class ScriptVariableAlreadyDefinedException : ReanimatorException
        {
            public ScriptVariableAlreadyDefinedException(String varNamem, int scriptOffset)
            {
                CustomMessage = "Attempting to redefineThe variable {0} is already definied";
            }
        }
    }
}
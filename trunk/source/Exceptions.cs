using System;

namespace Reanimator
{
    static class Exceptions
    {
        internal class ReanimatorException : Exception
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
    }
}
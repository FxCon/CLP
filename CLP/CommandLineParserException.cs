using System;

namespace CLP
{
    /// <summary>
    /// The CommandLineParserException class.
    /// </summary>
    public class CommandLineParserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParserException"/> class.
        /// </summary>
        public CommandLineParserException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CommandLineParserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference f no inner exception is specified.</param>
        public CommandLineParserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

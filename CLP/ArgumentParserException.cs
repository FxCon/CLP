using System;

namespace CLP
{
    /// <summary>
    /// The ArgumentParserException class.
    /// </summary>
    public class ArgumentParserException : CommandLineParserException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserException"/> class.
        /// </summary>
        public ArgumentParserException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ArgumentParserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference f no inner exception is specified.</param>
        public ArgumentParserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

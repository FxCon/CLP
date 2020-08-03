using System;

namespace CLP
{
    /// <summary>
    /// The FlagParserException class.
    /// </summary>
    public class FlagParserException : CommandLineParserException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlagParserException"/> class.
        /// </summary>
        public FlagParserException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlagParserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FlagParserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlagParserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference f no inner exception is specified.</param>
        public FlagParserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

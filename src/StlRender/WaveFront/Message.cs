using System;

namespace ModelRenderer.WaveFront
{
    /// <summary>
    /// Represents a message of a specific severity relating to the loading of data from a file.
    /// </summary>
    public class Message
    {
	    /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="details">The message details.</param>
        /// <param name="exception">The exception.</param>
        public Message(MessageType messageType, string fileName, int? lineNumber, string details, Exception exception = null)
        {
            this.MessageType = messageType;
            this.FileName = fileName;
            this.LineNumber = lineNumber;
            this.Details = details;
            this.Exception = exception;
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        public MessageType MessageType { get; }

	    /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string FileName { get; }

	    /// <summary>
        /// Gets the line number.
        /// </summary>
        public int? LineNumber { get; }

	    /// <summary>
        /// Gets the details.
        /// </summary>
        public string Details { get; }

	    /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; }
    }
}
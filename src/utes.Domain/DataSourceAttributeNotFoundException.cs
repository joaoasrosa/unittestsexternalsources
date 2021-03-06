﻿using System;

namespace utes.Domain
{
    /// <summary>
    /// Custom exception to handle the exception when the data source method attribute is not found.
    /// </summary>
    public class DataSourceAttributeNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the utes.Domain.DataSourceAttributeNotFoundException class.
        /// </summary>
        public DataSourceAttributeNotFoundException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the utes.Domain.DataSourceAttributeNotFoundException class 
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DataSourceAttributeNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the utes.Domain.DataSourceAttributeNotFoundException class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        ///  (Nothing in Visual Basic) if no inner exception is specified.</param>
        public DataSourceAttributeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

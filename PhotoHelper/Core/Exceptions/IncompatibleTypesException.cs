﻿using System;

namespace PhotoHelper.Core.Exceptions
{
    /// <summary>
    /// Indicates that a dependency could not be resolved because the resolved type is
    /// not compatible with the injected type.
    /// </summary>
    //[Serializable]
    public class IncompatibleTypesException : Exception
    {
        /// <summary>
        /// Initializes the exception.
        /// </summary>
        public IncompatibleTypesException()
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        /// <param name="message">Error Message</param>
        public IncompatibleTypesException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes the exception.
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="exception">Inner Exception</param>
        public IncompatibleTypesException(string message, Exception exception)
            : base(message, exception)
        {
        }

    }
}

/////////////////////////////////////////////////////////////////////////////////
//
// Soft Proofing Effect Plugin for Paint.NET
//
// This software is provided under the MIT License:
//   Copyright (C) 2016-2017 Nicholas Hayes
// 
// See LICENSE.txt for complete licensing and attribution information.
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;

namespace SoftProofing
{
    /// <summary>
    /// The exception that is thrown when the LCMS API returns an error.
    /// </summary>
    [Serializable()]
    internal sealed class LCMSException : Exception, ISerializable
    {
        // <summary>
        /// Initializes a new instance of the <see cref="LCMSException"/> class.
        /// </summary>
        /// <overloads>Initializes a new instance of the <see cref="LCMSException"/> class.</overloads>
        public LCMSException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LCMSException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public LCMSException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LCMSException"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public LCMSException(string message, Exception inner)
            : base(message, inner)
        {
        }

        // This constructor is needed for serialization.
        private LCMSException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

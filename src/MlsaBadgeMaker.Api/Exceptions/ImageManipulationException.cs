using System;
using System.Collections.Generic;
using System.Text;

namespace MlsaBadgeMaker.Api.Exceptions
{
    public class ImageManipulationException : Exception
    {
        /// <inheritdoc />
        public override string Message { get; }

        public ImageManipulationException(string message)
        {
            Message = message;
        }
    }
}

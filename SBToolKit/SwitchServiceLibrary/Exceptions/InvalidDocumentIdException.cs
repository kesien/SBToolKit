using System;

namespace SwitchServiceLibrary.Exceptions
{
    [Serializable]
    public class InvalidDocumentIdException : Exception
    {
        public InvalidDocumentIdException() { }
        public InvalidDocumentIdException(string message) : base(message) { }
        public InvalidDocumentIdException(string message, Exception inner) : base(message, inner) { }
        protected InvalidDocumentIdException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

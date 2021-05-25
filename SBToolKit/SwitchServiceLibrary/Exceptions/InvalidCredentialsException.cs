using System;

namespace SwitchServiceLibrary.Exceptions
{
    [Serializable]
    public class InvalidUsernameOrPasswordException : Exception
    {
        public InvalidUsernameOrPasswordException() { }
        public InvalidUsernameOrPasswordException(string message) : base(message) { }
        public InvalidUsernameOrPasswordException(string message, Exception inner) : base(message, inner) { }
        protected InvalidUsernameOrPasswordException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

using Mini_HR_app.Models;
using System;
using System.Text.RegularExpressions;

namespace Mini_HR_app.Exceptions
{
    [Serializable]
    public class InvalidCompanyException : Exception
    {
        public InvalidCompanyException() : base() { }
        public InvalidCompanyException(string message) : base(message) { }
        public InvalidCompanyException(string message, Exception inner) : base(message, inner) { }
        protected InvalidCompanyException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }       
    }

    [Serializable]
    public class InvalidPersonException : Exception
    {

    }
}

using System;

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
    public class PutCompanyException : Exception
    {
        public PutCompanyException(string message) : base(message) { }
        protected PutCompanyException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class GetCompanyException : Exception
    {
        public GetCompanyException(string message) : base(message) { }
        protected GetCompanyException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class PostCompanyException : Exception
    {
        public PostCompanyException(string message) : base(message) { }
        protected PostCompanyException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidEmployeeException : Exception
    {
        public InvalidEmployeeException() : base() { }
        public InvalidEmployeeException(string message) : base(message) { }
        public InvalidEmployeeException(string message, Exception inner) : base(message, inner) { }
        protected InvalidEmployeeException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class PutEmployeeException : Exception
    {
        public PutEmployeeException(string message) : base(message) { }
        protected PutEmployeeException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class GetEmployeeException : Exception
    {
        public GetEmployeeException(string message) : base(message) { }
        protected GetEmployeeException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class PostEmployeeException : Exception
    {
        public PostEmployeeException(string message) : base(message) { }
        protected PostEmployeeException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class CloudinaryException : Exception
    {
        public CloudinaryException(string message) : base(message) { }
        protected CloudinaryException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

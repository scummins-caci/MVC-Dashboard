using System;

namespace TED.Dashboard.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message){ }
    }
}

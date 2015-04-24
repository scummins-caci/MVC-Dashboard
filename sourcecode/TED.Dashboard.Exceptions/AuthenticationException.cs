using System;

namespace TED.Dashboard.Authentication
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message) { }
    }
}

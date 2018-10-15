using System;

namespace Todolist.Service.Auth.Exceptions
{
    public class LoginFailException : Exception
    {
        public LoginFailException(string login) : base($"Failed to login: {login}.") { }
    }
}
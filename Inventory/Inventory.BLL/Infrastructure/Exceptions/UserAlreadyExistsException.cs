using System;

namespace Inventory.BLL.Infrastructure
{
    public class UserAlreadyExistsException : Exception
    {
        static string message = "User already exists.";

        public UserAlreadyExistsException() : base(message) { }
    }
}

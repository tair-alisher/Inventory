using System;

namespace Inventory.BLL.Infrastructure
{
    public class OldPasswordIsWrongException : Exception
    {
        static string message = "Old password is incorrect.";

        public OldPasswordIsWrongException() : base(message) { }
    }
}

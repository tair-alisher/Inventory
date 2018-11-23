using System;

namespace Inventory.BLL.Infrastructure
{
    public class InsecurePasswordException : Exception
    {
        static string message = "Password is insecure.";

        public InsecurePasswordException() : base(message) { }
    }
}

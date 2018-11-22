using System;

namespace Inventory.BLL.Infrastructure
{
    public class HasRelationsException : Exception
    {
        static string message = "Item has relations.";

        public HasRelationsException() : base(message) { }
    }
}

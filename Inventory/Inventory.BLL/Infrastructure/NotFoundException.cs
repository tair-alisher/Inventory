using System;

namespace Inventory.BLL.Infrastructure
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string message) : base(message) { }
	}
}

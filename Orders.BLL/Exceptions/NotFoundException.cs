using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.BLL.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }


    }
    public class InvalidOrderException : Exception
    {
        public InvalidOrderException(string message) : base(message) { }
    }
}

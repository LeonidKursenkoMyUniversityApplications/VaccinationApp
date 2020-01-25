using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epidemic.BLL.Exceptions
{
    public class BllException : Exception
    {
        public BllException() : base()
        {
            
        }

        public BllException(string message) : base(message)
        {
            
        }
    }
}

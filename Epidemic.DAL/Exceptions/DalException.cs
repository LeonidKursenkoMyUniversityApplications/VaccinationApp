using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Epidemic.DAL.Exceptions
{
    public class DalException : Exception
    {
        public DalException() : base()
        {
            
        }

        public DalException(string message) : base(message)
        {
            
        }
    }
}

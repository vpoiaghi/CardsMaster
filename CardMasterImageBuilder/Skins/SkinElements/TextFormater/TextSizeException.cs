using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterImageBuilder.Elements.TextFormater
{
    class TextSizeException : Exception
    {
        public TextSizeException(string message) : base(message)
        { }

        public TextSizeException(string message, Exception innerException) : base(message, innerException)
        { }

    }
}

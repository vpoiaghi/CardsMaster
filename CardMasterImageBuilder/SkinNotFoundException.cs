using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterImageBuilder
{
    [Serializable]
    public class SkinNotFoundException : Exception
    {
        public SkinNotFoundException()
        {
        }

        public SkinNotFoundException(string message) : base(message)
        {
        }

        public SkinNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SkinNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

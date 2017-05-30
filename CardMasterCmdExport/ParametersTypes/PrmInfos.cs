using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterCmdExport.ParametersTypes
{
    public enum ParameterKinds
    {
        Optional,
        NotOptional,
        Boolean
    }

    class PrmInfos
    {
        protected ParameterKinds kind = ParameterKinds.NotOptional;

        public string AttibuteName { get; set; } = null;
        public string[] AllowedValues { get; set; } = null;
        public ParameterKinds Kind { get { return this.kind; } }
        public object DefaultValue { get; protected set; }

        protected PrmInfos()
        { }

        public PrmInfos(string attributeName) : this(attributeName, null)
        { }

        public PrmInfos(string attributeName, string[] allowedValues)
        {
            this.AttibuteName = attributeName;
            this.AllowedValues = allowedValues;
        }
    }
}

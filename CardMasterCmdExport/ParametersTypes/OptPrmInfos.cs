namespace CardMasterCmdExport.ParametersTypes
{
    class OptPrmInfos : PrmInfos
    {
        public OptPrmInfos(string attributeName, object defaultValue) : this(attributeName, null, defaultValue)
        { }

        public OptPrmInfos(string attributeName, string[] allowedValues, object defaultValue) : base(attributeName, allowedValues)
        {
            this.kind = ParameterKinds.Optional;
            this.DefaultValue = defaultValue;
        }

    }
}

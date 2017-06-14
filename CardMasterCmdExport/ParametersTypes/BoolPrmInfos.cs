namespace CardMasterCmdExport.ParametersTypes
{
    class BoolPrmInfos : PrmInfos
    {
        public BoolPrmInfos(string attributeName) : base(attributeName)
        {
            this.kind = ParameterKinds.Boolean;
            this.DefaultValue = false;
        }
    }
}

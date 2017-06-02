using System.Collections.Generic;

namespace CardMasterImageBuilder.Skins
{
    public class SkinElements : List<SkinElement>
    {
        public new void Add(SkinElement element)
        {
            if (element.Visible)
            {
                base.Add(element);
            }
        }
    }
}

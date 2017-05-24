using System.Collections.Generic;

namespace CardMasterSkin.Skins
{
    public class SkinElements : List<SkinElement>
    {
        public void Add(SkinElement element)
        {
          
            if (element.Visible)
            {
                base.Add(element);
            }
        }
    }
}

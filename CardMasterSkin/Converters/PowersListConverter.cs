﻿using CardMasterCard.Card;
using CardMasterCommon.Converters;

namespace CardMasterSkin.Converters
{
    public class PowersListConverter : ListConverter<Power>
    {
        protected override Power GetItem(string value)
        {
            var p = new Power();
            p.Description = value;
            return p;
        }
    }
}
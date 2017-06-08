using CardMasterCard.Card;
using CardMasterCommon.Converters;

namespace CardMasterImageBuilder.Converters
{
    public class PowersListConverter : ListConverter<JsonPower>
    {
        protected override JsonPower GetItem(string value)
        {
            var p = new JsonPower();
            p.Description = value;
            return p;
        }
    }
}

using CardMasterCard.Card;
using CardMasterCommon.Converters;

namespace CardMasterManager.Converters
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

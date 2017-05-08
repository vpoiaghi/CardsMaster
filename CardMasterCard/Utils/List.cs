using System;

namespace CardMasterCard.Utils
{
    public class List<T> : System.Collections.Generic.List<T>
    {
        public override String ToString()
        {
            String result = "";
            bool firstLine = true;

            foreach (Object p in this)
            {
                if (firstLine)
                {
                    result += p.ToString();
                    firstLine = false;

                }
                else
                {
                    result += Environment.NewLine + p.ToString();
                }

            }

            if (result.Equals(""))
            {
                result = "Liste vide";
            }

            return result;
        }

    }
}

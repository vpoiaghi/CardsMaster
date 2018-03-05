using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterStat.Engine
{
    public class CustomDot
    {
        public String Label { get; set; }
        public int Cost { get; set; }
        public int Value { get; set; }

        public CustomDot(String label,int cost, int value)
        {
            this.Label = label;
            this.Cost = cost;
            this.Value = value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterImageBuilder.Builders
{
    public class BuilderRegister

    {
        private static BuilderRegister INSTANCE;
        private static Dictionary<String, IBuilder> map;
        private BuilderRegister()
        { }

        public static BuilderRegister getInstance()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new BuilderRegister();
                map = new Dictionary<string, IBuilder>();
            }
            return INSTANCE;
        }

        public void Register()
        {
            Type[] toReturn = Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "CardMasterImageBuilder.Builders.Impl", StringComparison.Ordinal)).ToArray();
            foreach (Type type in toReturn)
            {
                IBuilder instance = (IBuilder)Activator.CreateInstance(type);
                map.Add(instance.TYPE, instance);
            }

        }

        public IBuilder getBuilder(String type)
        {
            return map[type];
        }

    }
}

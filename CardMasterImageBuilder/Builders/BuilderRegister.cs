using CardMasterCommon.Utils;
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
        private static bool loaded = false;
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

        public void Register(BuilderParameter builderParameter)
        {
            if (!loaded)
            {
                string namespacePath = "CardMasterImageBuilder.Builders.Impl";

                if (UtilsTypes.IsNamespaceExists(namespacePath))
                {
                    //Type[] toReturn = Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, namespacePath, StringComparison.Ordinal) && (t is IBuilder)).ToArray();
                    Type[] toReturn = Assembly.GetExecutingAssembly().GetTypes().Where(t => UtilsTypes.IsInNamespace(t, namespacePath) && UtilsTypes.IsImplementInterface(t, typeof(IBuilder))).ToArray();
                    foreach (Type type in toReturn)
                    {
                        IBuilder instance = (IBuilder)Activator.CreateInstance(type, builderParameter);
                        map.Add(instance.TYPE, instance);
                    }
                }
                else
                {
                    throw new Exception("Le namespace " + namespacePath + " n'existe pas ou est inaccessible.");
                }
                loaded = true;
            }
        }

        public IBuilder getBuilder(String type)
        {
            return map[type];
        }

        public void Unregister()
        {
            map.Clear();
        }

    }
}

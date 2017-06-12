﻿using CardMasterCommon.Utils;
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
            string namespacePath = "CardMasterImageBuilder.Builders.Impl";

            if (UtilsTypes.IsNamespaceExists(namespacePath))
            {
                //Type[] toReturn = Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, namespacePath, StringComparison.Ordinal) && (t is IBuilder)).ToArray();
                Type[] toReturn = Assembly.GetExecutingAssembly().GetTypes().Where(t => UtilsTypes.IsInNamespace(t, namespacePath) && UtilsTypes.IsImplementInterface(t, typeof(IBuilder))).ToArray();
                foreach (Type type in toReturn)
                {
                    IBuilder instance = (IBuilder)Activator.CreateInstance(type);
                    map.Add(instance.TYPE, instance);
                }
            }
            else
            {
                throw new Exception("Le namespace " + namespacePath + " n'existe pas ou est inaccessible.");
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

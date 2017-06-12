using System;
using System.Reflection;

namespace CardMasterCommon.Utils
{
    public static class UtilsTypes
    {
        public static bool IsNamespaceExists(string namespacePath)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type[] types;

            foreach(Assembly a in assemblies)
            {
                types = a.GetTypes();

                foreach (Type t in types)
                {
                    if ((t.Namespace != null) && (t.Namespace.Equals(namespacePath)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool IsInNamespace(Type type, string namespacePath)
        {
            return String.Equals(type.Namespace, namespacePath, StringComparison.Ordinal);
        }

        public static bool IsImplementInterface(this Type type, Type @interface)
        {
            return type.FindInterfaces((t, c) => {

                bool result = false;

                Type tt = t as Type;
                Type tc = c as Type;

                if (tc.IsGenericType && tc.ContainsGenericParameters && tt.IsGenericType)
                {
                    result = tc.GetGenericTypeDefinition() == tt.GetGenericTypeDefinition();
                }
                else
                {
                    result = tc == tt;
                }

                return result;

            }, @interface).Length > 0;
        }

    }
}

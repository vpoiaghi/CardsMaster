using CardMasterCmdExport.ParametersTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CardMasterCmdExport
{
    class ParametersReader
    {
        private Dictionary<string, PrmInfos> parameters = new Dictionary<string, PrmInfos>()
        {
            { "backside",  new BoolPrmInfos("WithBackSide") },
            { "f",         new OptPrmInfos("ExportFormat", new string[] {"png", "pdf"}, "png") },
            { "m",         new PrmInfos("ExportMode", new string[] {"all", "board"}) },
            { "s",         new OptPrmInfos("BoardSpace", 0) }
        };

        public Parameters Read(string[] args)
        {
            Parameters prms = new Parameters();

            if (args == null)
            {
                throw new ArgumentException("Nombre de paramètres invalide.");
            }
            else if ((args.Length == 1) && (args[0].Equals("/?")))
            {
                throw new ArgumentException("");
            }
            else if (args.Length < 2)
            {
                throw new ArgumentException("Nombre de paramètres invalide.");
            }
            else
            {
                ReadCardProject(args, prms);
                ReadTarget(args, prms);
                SetOptionsTodParameters(args, prms);
            }

            return prms;
        }

        private void ReadCardProject(string[] args, Parameters prms)
        {
            FileInfo f = new FileInfo(args[0].Trim());

            if (!f.Exists)
            {
                throw new ArgumentException("Le projet json n'existe pas ou n'a pu être trouvé.");
            }

            prms.JsonProjectFile = f;
        }

        private void ReadTarget(string[] args, Parameters prms)
        {
            DirectoryInfo d = new DirectoryInfo(args[1]);

            if (!d.Exists)
            {
                throw new ArgumentException("Le dossier " + args[1] + " cible n'existe pas ou n'a pu être trouvé.");
            }

            prms.ExportTargetFolder = d;
        }

        private void SetOptionsTodParameters(string[] args, Parameters prms)
        {
            ReadOptionsFromCommandLine(args, prms);
            completeOptionsWithDefaultValues(prms);
        }

        private void ReadOptionsFromCommandLine(string[] args, Parameters prms)
        {
            string arg = null;
            string argName = null;
            PrmInfos prmInfo = null;
            MethodInfo setter = null;
            object value = null;
            Type attributeType = null;

            int i = 1;
            while (++i < args.Length)
            {
                arg = args[i].Trim();

                if (args[i].Equals("/?"))
                {
                    // Arrêt de la lecture des paramètres. On remonte une exception sans message. L'usage sera affiché.
                    throw new ArgumentException();
                }

                argName = GetArgName(arg);

                prmInfo = null;
                setter = null;

                try
                {
                    prmInfo = parameters[argName];
                }
                catch (KeyNotFoundException)
                { }

                if (prmInfo != null)
                {
                    setter = prms.GetType().GetMethod("set_" + prmInfo.AttibuteName);
                }
                else
                {
                    // Arrêt de la lecture des paramètres On remonte une exception avec message. Le message et l'usage seront affichés.
                    throw new ArgumentException("Paramètre " + arg + " inconnu.");
                }

                if (setter != null)
                {
                    attributeType = setter.GetParameters()[0].ParameterType;

                    if (attributeType == typeof(string))
                    {
                        value = ArgToString(arg, argName, prmInfo.AllowedValues);
                    }
                    else if (attributeType == typeof(int?))
                    {
                        value = ArgToInt(arg, argName, prmInfo.AllowedValues);
                    }
                    else if (attributeType == typeof(bool?))
                    {
                        value = true;
                    }

                    if (value != null)
                    {
                        setter.Invoke(prms, new object[] { value });
                    }
                }
            }
        }

        private void completeOptionsWithDefaultValues(Parameters prms)
        {
            foreach (PrmInfos pInfo in parameters.Values)
            {
                MethodInfo getter = prms.GetType().GetMethod("get_" + pInfo.AttibuteName);
                MethodInfo setter = prms.GetType().GetMethod("set_" + pInfo.AttibuteName);

                object value = getter.Invoke(prms, null);

                if (value == null)
                {
                    if ((pInfo.Kind == ParameterKinds.Optional) || (pInfo.Kind == ParameterKinds.Boolean))
                    {
                        setter.Invoke(prms, new object[] { ((OptPrmInfos)pInfo).DefaultValue });
                    }
                    else if(pInfo.Kind == ParameterKinds.NotOptional)
                    {
                        foreach (string aName in parameters.Keys)
                        {
                            if (parameters[aName] == pInfo)
                            {
                                // Arrêt de la lecture des paramètres On remonte une exception avec message. Le message et l'usage seront affichés.
                                throw new ArgumentException("Le paramètre " + aName + " est obligatoire.");
                            }
                        }
                    }
                }
            }
        }

        private string GetArgName(string arg)
        {
            string name = null;
            int pos = arg.IndexOf(':');

            if (pos == -1)
            {
                name = arg.Substring(1).Trim().ToLower();
            }
            else
            {
                name = arg.Substring(1, pos - 1).Trim().ToLower();
            }

            return name;
        }

        private string ArgToString(string arg, string argName, string[] allowedValues)
        {
            int pos = arg.IndexOf(':');
            string argValue = arg.Substring(pos + 1).Trim().ToLower();

            if (allowedValues != null)
            {
                bool isAllowed = false;

                foreach (string value in allowedValues)
                {
                    isAllowed |= (value.ToLower().Equals(argValue));
                }

                if (!isAllowed)
                {
                    throw new ArgumentException("La valeur " + argValue + " n'est pas autorisée pour le paramètre " + argName + '.');
                }
            }

            return argValue;
        }

        private int ArgToInt(string arg, string argName, string[] allowedValues)
        {
            string argValue = ArgToString(arg, argName, allowedValues);

            try
            {
                return int.Parse(argValue);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Paramètre " + argName + ": la valeur " + argValue + " n'est pas un entier valide.", ex);
            }

        }
    }
}
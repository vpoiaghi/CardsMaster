using System;

namespace CardMasterCmdExport
{
    class Program
    {

        static void Main(string[] args)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // On passe ici si l'appli est lancée en mode debug
                Start(args);
            }
            else
            {
                // On passe ici si l'appli est lancée en mode release
                try
                {
                    Start(args);
                }
                catch (Exception ex)
                {
                    Console.Write("Une erreur s'est produite lors de l'exportation.");
                    Console.Write(ex.Message);
                }
            }
        }

        private static void Start(string[] args)
        {
            Export e = new Export();
            e.StartExport(args);
        }
    }
}

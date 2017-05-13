using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMasterCmdExport
{
    abstract class Usage
    {
        public static void ShowUsage()
        {
            Console.WriteLine("Exporte une ou plusieurs cartes d'un projet CardMaster.");
            Console.WriteLine("");
            Console.WriteLine("CardMasterCmdExport fichier_json dossier_cible [/all] [/f:format]");
            Console.WriteLine("");
            Console.WriteLine("fichier_json            Chemin complet du fichier json d'un projet CardMaster.");
            Console.WriteLine("dossier_cible           Chemin complet du dossier cible où seront générées les fichiers images.");
            Console.WriteLine("/all                    Toutes les cartes doivent être générées.");
            Console.WriteLine("/f:format               Format d'exportation. Par défaut le format est en fichiers .png.");
            Console.WriteLine("                        PNG   fichiers images .png");
            Console.WriteLine("                        PDF   fichiers .pdf avec 12 cartes par pages (non fonctionnel)");
            Console.WriteLine("");

            Console.ReadKey(true);
        }

        public static void ShowErrUsage(string errorMessage)
        {
            if (! String.IsNullOrEmpty(errorMessage))
            {
                Console.WriteLine(errorMessage);
                Console.WriteLine("");
                Console.WriteLine("Usage :");
            }

            ShowUsage();
        }

    }
}

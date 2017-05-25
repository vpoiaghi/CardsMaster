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
            Console.WriteLine("CardMasterCmdExport fichier_json dossier_cible /m:mode [/s:space] [/f:format]");
            Console.WriteLine("");
            Console.WriteLine("fichier_json            Chemin complet du fichier json d'un projet CardMaster.");
            Console.WriteLine("dossier_cible           Chemin complet du dossier cible où seront générées les fichiers images.");
            Console.WriteLine("/m:mode                 Mode d'exportation.");
            Console.WriteLine("   all                  Génération d'une image pour chaque carte.");
            Console.WriteLine("   board                Génération d'une planche de cartes.");
            Console.WriteLine("/s:space                Optionel. Utilisisée qu'avec le mode board. Indique l'espace entre les cartes lors de la génération de planches. Par défaut la valeur est 0.");
            Console.WriteLine("/f:format               Optionel. Format d'exportation. Par défaut le format est en .png.");
            Console.WriteLine("   PNG                  Fichiers images .png");
            Console.WriteLine("   PDF                  Fichiers .pdf avec 12 cartes par pages (non fonctionnel)");
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

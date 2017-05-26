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
            Console.WriteLine("CardMasterCmdExport fichier_json dossier_cible /m:mode [/s:space] [/backside] [/f:format]");
            Console.WriteLine("");
            Console.WriteLine("fichier_json            Chemin complet du fichier json d'un projet CardMaster.");
            Console.WriteLine("dossier_cible           Chemin complet du dossier cible où seront générées les fichiers images.");
            Console.WriteLine("/m:mode                 Mode d'exportation.");
            Console.WriteLine("   all                  Génération d'une image pour chaque carte.");
            Console.WriteLine("   board                Génération d'une planche de cartes.");
            Console.WriteLine("/s:space                Optionel. Utilisé avec le mode board. Indique l'espace entre les cartes lors de l'exportation des planches. Par défaut la valeur est 0.");
            Console.WriteLine("/backside               Optionel. Utilisé avec le mode board. Indique si le dos des cartes doit être généré lors de l'exportation des planches. Si l'option est présente, le dos sera généré.");
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

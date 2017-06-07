# CardsMaster

## Documentation

* Ajouter au DCG.ppt la liste des libs utilisées, par quels projets et où les trouver.


## Evolutions techniques

### Architecture

* Utiliser des interfaces.
Actuellement il n'y a aucn système de contrat de service entre les projets, chaque projet voit tout ou presque des projets qu'il référencie.

* Organisation
  * [DONE] Passer les images de dos dans Textures (voir autre suggestion ci-dessous)

  * [DONE] Autre suggestion : regrouper toutes les images et textures dans un même dossier racine avec une arborescence de dossiers du genre :
    * Textures
	  * Dos : textures de dos des cartes
      * Elements : les images telles que Atl, défense, manna
	  * Fonds : les textures de fond de la carte ou des zones de texte
	  * Images : les images centrales (personnages, lieux, etc..)
	  * Symboles : les symboles utilisés dans les textes
  * Permet de n'avoir qu'un chemin à gérer dans le C# et surtout évide le code spécifique : si c'est l'image centrale on tappe dans le dossier Images, si c'est un symbole on tappe le dossier Texture, etc..., là on donne le répertoire racine et le nom du fichier et le programme recherche l'image dans l'arborescence (c'est déjà codé comme ça sauf cqu'actuellement on a 2 dossiers racines: Images et Textures)
  
### Exportation

* Revoir le mode de communication entre les projets appelants et le projet Export
Chaque mode d'export (carte par carte, planche) dispose de sa classe d'export spécifique pour traiter l'exportation.
Mais j'ai laissé les projets appelant choisir et instancier ces classes plutôt que laisser le projet Export dispatcher entre ces différentes classes.
Un seul type d'objet de paramétrage doit être envoyé au projet d'export et c'est le Parameters présent dans le projet CmdExport qu'il faudra déplacer dans le projet Export.

* Certains paramètres d'export sont en dur
  Taille des cartes, etc... les faire passer par l'obet Parameters à mettre en place

### Cartes

* [DONE] Faire un BackSideFactory : En attendant un vrai fichier Skin, sortir la construction du skin du drawBackground.

* Pouvoir ajouter / supprimer des lignes dans la grille pour ne pas avoir nécéssairement de Json en entrée

* Pouvoir gérer autant de champs custo que l'on veut avec un AnyJsonGetter et une map d'extrafields sur une Card

* [DONE] Mettre en place le fichier de Skin
Skin de face + Skin de dos.
Voir si plusieurs Skin de face sont nécessaires.

* [DONE] Association d'une carte avec un skin de face (?)
Si un projet peut définir plusieurs Skin (à confirmer), il sera nécessaire d'associer une carte à une des Skin.
A préciser : je ne vois pas ce qu'est un skin de face
--> Ben, le skin de la face de la carte... par opposition au skin du dos de la carte. :)

### Nettoyage
* [DONE] Virer les boutons d’option de qualité de rendu que j’avais ajouté sous le viewer du Manager.

  
## Evolutions fonctionnelles

### Aspect des cartes

* [DONE] Pouvoir définir la couleur d'un texte (par ex sur les icones Atk / def, pouvoir écrire en Gold ou en White)

* Bordures en dégradé

* Bordure en or ou noir
Pouvoir choisir pour une carte donnée quelle sera la couleur de la bordure (choix de customisation ou autre)
--> Je ne sais toujours pas si c'est pour la face ou le dos ou les deux... 
La possibilité de choisir la couleur de bordure pour la face et le dos est prête coté Factory et gestion du dessin des SkinElement, reste :
  * A ajouter un attribut à Card.
  * A ajouter cet attribut au DataGrid.
  * A s'en servir comme valeur dans le Factory.

### IHM

* Cartes
  * Gérer les noms longs de cartes (passe sur 2 lignes mais se décalle vers le haut mais se retrouve hors cadre : ex : Vallée des nuages et de la foudre)

* Boîte de dialogue de paramétrage des exports
  * Pouvoir paramétrer l'exportation par l'ihm via une boîte de dialogue (mode, format, dossier/fichier de destination, etc...). 
  * Pouvoir paramétrer lors de l'export la résolution et la taille des cartes à obtenir.
  * Voir quel projet portera cette boîte de dialogue.

* Revoir le menu du Manager
En particulier la partie exportation, où y aurait désormait un seul item qui ouvre sur une boîte de dialogue de choix du type d'export et des paramètres d'export.

* Viewer de dos
  * [DONE] Ajouter un viewer pour le dos sous le viewer de face du Manager

* Datagrid
  * Ajouter la possibilité de masquer/afficher des colonnes
  * [DONE] Ajouter la possibilité de figer une ou 2 colonnes (à minima le nom)
  * [A TESTER] Rendre triable toutes les colonnes, selon leur valeur visible (je pense en particulier au colonnes liées à des Enum)
  * [A TESTER] Est-il intéressant de trier par la colonne pouvoirs ? tri par n'importe quelle colonne SAUF pouvoir
  * Pouvoir faire des recherches (soit dans une colonne spécifique soit via un champs libre auquel cas c'est une recherche "Full Text" : recherche dans tous les champs -> recherche générique par réflection)

* Impression
Maintenant que la fonctionnalité est validée, voir comment elle sera intégrée déns l'ihm
  * Boîte de dialogue personnalisée ?
  * Impression à considérer comme un export comme les autres ou pas ?
  * Quel projet portera l'IHM de paramétrage (actuellement c'est le projet Export) ?
  
### Application  

* SkinEditor
A conceptualiser.


## Bugs

* Pendant un export par le manager si on clique sur un item de la grille l'application plante. J'ai du mal gérer quelquechose au niveau des threads.


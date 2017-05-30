# CardsMaster

## Documentation

* Mettre ce README.md au format READM.md :) 
--> Je crois que c'est bon. Je te laisse valider.

* Ajouter au DCG.ppt la liste des libs utilisées, par quels projets et où les trouver.


## Evolutions techniques

### Architecture

* Utiliser des interaces.
Actuellement il n'y a aucn système de contrat de service entre les projets, chaque projet voit tout ou presque des projets qu'il référencie.

* Organisation
Passer les images de dos dans Textures

  
### Exportation

* Revoir le mode de communication entre les projets appelants et le projet Export
Chaque mode d'export (carte par carte, planche) dispose de sa classe d'export spécifique pour traiter l'exportation.
Mais j'ai laissé les projets appelant choisir et instancier ces classes plutôt que laisser le projet Export dispatcher entre ces différentes classes.
Un seul type d'objet de paramétrage doit être envoyé au projet d'export et c'est le Parameters présent dans le projet CmdExport qu'il faudra déplacer dans le projet Export.

* Certains paramètres d'export sont en dur
  Taille des cartes, etc... les faire passer par l'obet Parameters à mettre en place

### Cartes

* Faire un BackSideFactory (?)
En attendant un vrai fichier Skin. 
Sortir la construction du skin du crawBackground

* Mettre en place le fichier de Skin
Skin de face + Skin de dos.
Voir si plusieurs Skin de face sont nécessaires.

* Association d'une carte avec un skin de face (?)
Si un projet peut définir plusieurs Skin (à confirmer), il sera nécessaire d'associer une carte à une des Skin.
A préciser : je ne vois pas ce qu'est un skin de face
--> Ben, le skin de la face de la carte... par opposition au skin du dos de la carte. :)

### Nettoyage
* [DONE] [A VALIDER] Virer les boutons d’option de qualité de rendu que j’avais ajouté sous le viewer du Manager.

  
## Evolutions fonctionnelles

### Aspect des cartes

* Pouvoir définir la couleur d'un texte (par ex sur les icones Atk / def, pouvoir écrire en Gold ou en White)

* Bordures en dégradé

* Bordure en or ou noir
Pouvoir choisir pour une carte donnée quelle sera la couleur de la bordure (choix de customisation ou autre)
--> Je ne sais toujours pas si c'est pour la face ou le dos ou les deux... 
La possibilité de choisir la couleur de bordure pour la face et le dos est prête coté Factory et gestion du dessin des SkinElement, reste :
  * A ajouter un attribut à Card.
  * A ajouter cet attribut au DataGrid.
  * A s'en servir comme valeur dans le Factory.

### IHM

* Boîte de dialogue de paramétrage des exports
  * Pouvoir paramétrer l'exportation par l'ihm via une boîte de dialogue (mode, format, dossier/fichier de destination, etc...). 
  * Pouvoir paramétrer lors de l'export la résolution et la taille des cartes à obtenir.
  * Voir quel projet portera cette boîte de dialogue.

* Revoir le menu du Manager
En particulier la partie exportation, où y aurait désormait un seul item qui ouvre sur une boîte de dialogue de choix du type d'export et des paramètres d'export.

* Viewer de dos
  * [DONE] Ajouter un viewer pour le dos sous le viewer de face du Manager

* Datagrid
  * Rendre triable toutes les colonnes, selon leur valeur visible (je pense en particulier au colonnes liées à des Enum)
  * Est-il intéressant de trier par la colonne pouvoirs ? tri par n'importe quelle colonne*
  * Pouvoir faire des recherches (soit dans une colonne spécifique soit via un champs libre auquel cas c'est une recherche "Full Text" : recherche dans tous les champs -> recherche générique par réflection)

* Impression
Maintenant que la fonctionnalité est validée, voir comment elle sera intégrée déns l'ihm
  * Boîte de dialogue personnalisée ?
  * Impression à considérer comme un export comme les autres ou pas ?
  * Quel projet portera l'IHM de paramétrage (actuellement c'est le projet Export) ?
  
### Application  

* SkinEditor
A conceptualiser.

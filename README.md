# CardsMaster

## Documentation

* Ajouter au DCG.ppt la liste des libs utilisées, par quels projets et où les trouver.


## Evolutions techniques

### Architecture

* Utiliser des interfaces.
Actuellement il n'y a aucn système de contrat de service entre les projets, chaque projet voit tout ou presque des projets qu'il référencie.

### Exportation

* Revoir le mode de communication entre les projets appelants et le projet Export
Chaque mode d'export (carte par carte, planche) dispose de sa classe d'export spécifique pour traiter l'exportation.
Mais j'ai laissé les projets appelant choisir et instancier ces classes plutôt que laisser le projet Export dispatcher entre ces différentes classes.
Un seul type d'objet de paramétrage doit être envoyé au projet d'export et c'est le Parameters présent dans le projet CmdExport qu'il faudra déplacer dans le projet Export.

* [EN COURS] Certains paramètres d'export sont en dur
  Taille des cartes, etc... les faire passer par l'obet Parameters à mettre en place

### Cartes

* [EN COURS] Pouvoir ajouter / supprimer des lignes dans la grille pour ne pas avoir nécéssairement de Json en entrée.
Bugs quand création d'un projet from scratch (lien avec un skin devant déja exister), bug de rendu corrigé.

* Pouvoir gérer autant de champs custo que l'on veut avec un AnyJsonGetter et une map d'extrafields sur une Card

  
## Evolutions fonctionnelles

### Aspect des cartes

* [EN COURS] Bordures en dégradé avec un color 1 et color 2 pour les borders (attention probleme de map <String,[String])

### IHM

* Cartes
  * [EN COURS] Gérer les noms longs de cartes (passe sur 2 lignes mais se décalle vers le haut mais se retrouve hors cadre : ex : Vallée des nuages et de la foudre)

* Boîte de dialogue de paramétrage des exports
  * Pouvoir paramétrer l'exportation par l'ihm via une boîte de dialogue (mode, format, dossier/fichier de destination, etc...). 
  * Pouvoir paramétrer lors de l'export la résolution et la taille des cartes à obtenir.
  * Voir quel projet portera cette boîte de dialogue.

* Revoir le menu du Manager
En particulier la partie exportation, où y aurait désormait un seul item qui ouvre sur une boîte de dialogue de choix du type d'export et des paramètres d'export.

* Faire un outil de redimensionnement des images avec une mire, et prévisualisation du résultat. Quand on ouvre une image ex "Image1.jpg", une fois le redimensionnement fait,
cela crée une copie en "Image1-original.jpg" et écrase le fichier "Image1.jpg" avec celle retaillée.

* Datagrid
  * [DONE] Ajouter la possibilité de masquer/afficher des colonnes
  * ![Warning](https://cdn1.iconfinder.com/data/icons/hawcons/32/700144-icon-61-warning-128.png)
  * [A TESTER] Rendre triable toutes les colonnes, selon leur valeur visible (je pense en particulier au colonnes liées à des Enum)
      --> KO, le tri sur le GENRE est bon mais pas sur le NATURE CHAKRA. Sans doute l'enum du GENRE est-il bien trié, contriarement à celui du CHAKRA

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


# CardsMaster

-Bouvoir définir la couleur d'un texte (par ex sur les icones Atk / def, pouvoir écrire en Gold ou en White)

- Bordures en dégradé

- Boîte de dialogue de paramétrage des exports
	Pouvoir paramétrer l'exportation par l'ihm via une boîte de dialogue (mode, format, dossier/fichier de destination, etc...). 
	Pouvoir paramétrer lors de l'export la résolution et la taille des cartes à obtenir.
	Voir quel projet portera cette boîte de dialogue.
	
- Revoir le mode de communication entre les projets appelants et le projet Export
	Chaque mode d'export (carte par carte, planche) dispose de sa classe d'export spécifique pour traiter l'exportation.
	Mais j'ai laisser les projets appelant choisir et instancier ces classes plutôt que laisser le projet Export dispatché entre ces différentes classes.
	Un seul type d'objet de paramétrage doit être envoyé au projet d'export et c'est le Parameters présent dans le projet CmdExport qu'il faudra déplacer dans le projet Export.

- Certains paramètres d'export sont en dur
	Taille des cartes, etc... les faire passer par l'obet Parameters à mettre en place
	
- Utiliser des interaces.
	Actuellement il n'y a aucn système de contrat de service entre les projets, chaque projet voit tout ou presque des projets qu'il référencie.
	
- Revoir le menu du Manager
	En particulier la partie exportation, où y aurait désormait un seul item qui ouvre sur une boîte de dialogue de choix du type d'export et des paramètres d'export.

- Mettre en place le fichier de Skin
	Skin de face + Skin de dos.
	Voir si plusieurs Skin de face sont nécessaires.

- SkinEditor
	A conceptualiser.

- Faire un BackSideFactory (?)
	En attendant un vrai fichier Skin.
	
- Bordure en or ou noir
	Pouvoir choisir pour une carte donnée quelle sera la couleur de la bordure (choix de customisation ou autre)

- Nettoyage
	- Virer les boutons d’option de qualité de rendu que j’avais ajouté sous le viewer du Manager.

- Organisation
	Passer les images de dos dans Textures

- Viewer de dos
	Ajouter un viewer pour le dos sous le viewer de face du Manager
	
- Datagrid
	- Rendre triable toutes les colonnes, seon leur valeur visible (je pense en particulier au colonnes liées à des Enum)
	- Est-il intéressant de trier par la colonne pouvoirs ? tri par n'importe quelle colonne*
	-Pouvoir faire des recherches (soit dans une colonne spécifique soit via un champs libre auquel cas c'est une recherche "Full Text" : recherche dans tous les champs -> recherche générique par réflection)
	
- Association d'une carte avec un skin de face (?)
	Si un projet peut définir plusieurs Skin (à confirmer), il sera nécessaire d'associer une carte à une des Skin.
	A préciser : je ne vois pas ce qu'est un skin de face


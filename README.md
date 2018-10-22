# CardsMaster

## Documentation

* Ajouter au DCG.ppt la liste des libs utilisées, par quels projets et où les trouver.


## Evolutions techniques

### Architecture

* Utiliser des interfaces.
Actuellement il n'y a aucn système de contrat de service entre les projets, chaque projet voit tout ou presque des projets qu'il référencie.

### Exportation

* [EN COURS] Certains paramètres d'export sont en dur
  Taille des cartes, etc... les faire passer par l'obet Parameters à mettre en place
  
## Evolutions fonctionnelles

### IHM

* Boîte de dialogue de paramétrage des exports
  * Pouvoir paramétrer l'exportation par l'ihm via une boîte de dialogue (mode, format, dossier/fichier de destination, etc...). 
  * Pouvoir paramétrer lors de l'export la résolution et la taille des cartes à obtenir.
  * Voir quel projet portera cette boîte de dialogue.

* Revoir le menu du Manager
En particulier la partie exportation, où y aurait désormait un seul item qui ouvre sur une boîte de dialogue de choix du type d'export et des paramètres d'export.

* Impression
Maintenant que la fonctionnalité est validée, voir comment elle sera intégrée déns l'ihm
  * Boîte de dialogue personnalisée ?
  * Impression à considérer comme un export comme les autres ou pas ?
  * Quel projet portera l'IHM de paramétrage (actuellement c'est le projet Export) ?
  
### Application  
:warning: 
```
 SkinEditor A conceptualiser.
```

## Bugs

* Pendant un export par le manager si on clique sur un item de la grille l'application plante. J'ai du mal gérer quelquechose au niveau des threads.

UPD cartes :

http://fr.naruto.wikia.com/wiki/Iggy
http://naruto.wikia.com/wiki/Banna
http://fr.naruto.wikia.com/wiki/Shizuka


Test - 7 cartes départ
Quand on joue un pouvoir activate, on ne peut ataquer le meme tour

Ajouter pouvoirs destruction items

Notice d atak du heros avec arme qui peut pas contrataquer en def

Ajouter booleen carte peut ou pas attaquer

Passer à 50 PV

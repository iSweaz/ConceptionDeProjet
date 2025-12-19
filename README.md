# ConceptionDeProjet

Ce projet a été conçu afin de répondre au sujet de cours **Conception agile de projets informatiques - M1 informatique Lyon 2**.

---

## Description

Ce projet est une application Unity visant à :

- Effectuer une **réunion de Planning Poker** au cours d'une réunion agile.  
- Obtenir un **fichier JSON** contenant les résultats.

Le projet répond au **cahier des charges** présent dans le sujet d’évaluation, rédigé par l’enseignant.

Il utilise :

- **PurrNet** pour la solution réseau  
- **StandaloneFileBrowser** pour l’explorateur de fichiers  
- **ParrelSync** pour des tests en local

Fonctionnement :

- Part d’un **fichier JSON** existant ou créé en interne.  
- Permet, grâce au principe LAN, de créer **un seul serveur par réseau** et d’y connecter des clients selon le nombre de joueurs défini par le créateur de session.

---

## Technologies utilisées

- **Unity** version : 6000.0.60f1  
- **C#**  
- **PurrNet** (transport UDP)  
- **StandaloneFileBrowser**  
- **Git / GitHub**

---

## Fonctionnalités principales

- Création et gestion de sessions Planning Poker en LAN  
- Détection automatique du serveur LAN  
- Génération et lecture de fichiers JSON pour les scores  

---

## Structure du projet

- **Assets/Scripts** : ensemble des scripts utilisés  
- **Assets/Models** : fichiers 3D  
- **Assets/Prefabs** : prefabs Unity permettant d’instancier des objets sauvegardés

---

### Clonage du projet

Avec Git Bash :  

```bash
git clone https://github.com/iSweaz/ConceptionDeProjet.git
```
Ou avec GitHub Desktop, utiliser le lien :

https://github.com/iSweaz/ConceptionDeProjet.git

## Lancer le projet

- Ouvrir la branche `main` (par défaut)
- La branche contient le dossier **Build** et un rapport PDF
- Exécutable : `Build/ConceptionDeProjet.exe`

---

## Utilisation

- Si aucun serveur n’est actif, le bouton affiché sera **Create Session** → ouvre l’interface de création
- Si un serveur existe, le bouton affiché sera **Join Session** → permet de rejoindre la session active

---

## Déroulé d’une partie

- Une fois le deck choisi et la partie commencée, l’hôte se retrouve dans la scène **Meeting_Room**  
- **Uniquement à partir de ce moment, les clients peuvent rejoindre le serveur et donc lancer leur exécutable**  
- Selon le mode choisi, le jeu peut faire tourner plusieurs fois la même User Story jusqu’à obtenir les résultats attendus  
- Entre chaque User Story, le deck est sauvegardé et l’hôte doit cliquer sur le bouton pour passer à la suivante  
- Une fois le deck terminé, le message **“FIN fichier sauvegardé”** est affiché, et le fichier JSON est actualisé

---

## Multijoueur LAN

- Un seul **serveur** peut exister par réseau  
- **Ports utilisés** :  
  - `5000` : communication principale  
  - `5001` : ping / détection serveur LAN  
- Les clients détectent automatiquement le serveur actif et peuvent s’y connecter  
- Le serveur décide de l’ordre des User Stories et du passage à la suivante

---

## Documentation 
https://isweaz.github.io/ConceptionDeProjet/html/index.html


## Autres 
Le mode de jeu Majorité absolue n'est pas fonctionnel.

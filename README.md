# Empire Miniature VR

Projet final - Environnements immersifs  
Hiver 2026 - 420-6B3-VI

Nom : A completer  
DA : A completer

## Genre

RTS / Tower Defense en realite virtuelle pour casque Meta Quest.

## Objectif

Le joueur doit proteger son Hotel de Ville et survivre a plusieurs vagues d'ennemis. Il peut selectionner des unites, les deplacer sur une carte miniature et construire des defenses simples.

## Controles

- Pointer une unite avec le controleur droit.
- Appuyer sur la gachette pour selectionner.
- Pointer le sol et appuyer sur la gachette pour deplacer l'unite.
- Pointer un ennemi et appuyer sur la gachette pour attaquer.
- Utiliser les boutons du menu VR pour commencer, construire ou recommencer.

## Elements du projet

- Menu principal avec titre et bouton pour commencer.
- Environnement medieval miniature adapte au genre RTS.
- Interactions VR avec raycast du controleur.
- Objectif clair affiche au joueur.
- Sons lies aux actions importantes, comme construire ou attaquer.
- Ecran de Game Over avec bouton pour recommencer facilement.

## Bonnes pratiques VR utilisees

- Le joueur reste principalement immobile devant une carte miniature pour limiter le motion sickness.
- Les menus sont places en World Space avec de gros boutons faciles a viser.
- Les controles sont simples et bases sur le pointage naturel du controleur.
- L'objectif du jeu est visible des le debut.
- Les deplacements rapides de camera sont evites.

## Sources utilisees

- Unity Documentation - Develop for Meta Quest workflow : https://docs.unity.cn/6000.2/Documentation/Manual/xr-meta-quest-develop.html
- Unity Documentation - Meta OpenXR package setup : https://docs.unity.cn/Packages/com.unity.xr.meta-openxr%402.1/manual/get-started.html
- Unity Documentation - XR Interaction Toolkit : https://docs.unity.cn/Packages/com.unity.xr.interaction.toolkit%403.1/manual/index.html
- Unity Scripting API - NavMeshAgent : https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent.html
- OpenAI ChatGPT / Codex : utilise pour aider a organiser les grandes etapes du projet, proposer une structure de code simple et adapter le concept RTS/Tower Defense aux criteres du projet VR.

Prompt resume utilise avec ChatGPT / Codex :

```text
Je veux faire un jeu VR simple, style Age of Empires, pour un projet final en environnements immersifs sur Meta Quest. Donne-moi les grandes etapes, une structure de projet et quelques exemples de scripts pour un RTS/Tower Defense avec menu, objectif clair, interactions VR, sons et Game Over.
```

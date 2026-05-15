# Empire Miniature VR

Projet final - Environnements immersifs  
Hiver 2026 - 420-6B3-VI

Nom : Zacharie Nolet
DA : 2012487
---------------------------------------------------------------
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

## Sources d'assets gratuits prevues

Ces assets sont prevus pour garder un style low-poly medieval leger et adapte a Meta Quest. Les assets finaux importes dans Unity doivent etre places dans `Assets/Art`, `Assets/Audio` ou `Assets/Prefabs`.

- Quaternius - Ultimate Fantasy RTS : https://quaternius.com/packs/ultimatefantasyrts.html
  - Utilisation prevue : batiments RTS, hotel de ville, tours, ressources, props de carte miniature.
  - Licence indiquee par Quaternius : CC0.
- Quaternius - Medieval Village MegaKit : https://quaternius.itch.io/medieval-village-megakit
  - Utilisation prevue : maisons medievales, murs, decor de village, elements d'environnement.
  - Licence indiquee par Quaternius : CC0.
- Quaternius - Universal Animation Library 2 : https://quaternius.com/packs/universalanimationlibrary2.html
  - Utilisation prevue : animations idle, marche, attaque et mort pour les personnages.
  - Licence indiquee par Quaternius : CC0.
- Quaternius - Free Game Assets : https://quaternius.com/
  - Utilisation prevue : personnages allies, ennemis, monstres et objets low-poly supplementaires.
  - Licence a verifier sur la page du pack telecharge, generalement CC0 chez Quaternius.
- Kenney - Assets gratuits : https://kenney.nl/assets
  - Utilisation prevue : icones UI, boutons, prompts de controles, elements visuels simples pour les menus VR.
  - Licence indiquee par Kenney : CC0.
- Freesound : https://freesound.org/
  - Utilisation prevue : sons de construction, boutons, attaque, mort d'ennemi, ambiance medievale.
  - Important : filtrer de preference par licence CC0. Si un son utilise une licence avec attribution, ajouter le nom de l'auteur, le lien exact du son et la licence dans ce README.
- Poly Haven : https://polyhaven.com/
  - Utilisation prevue : textures de sol, bois, pierre ou ciel HDRI si necessaire.
  - Licence indiquee par Poly Haven : CC0.

## Simulateur et boutons

Le prototype peut etre tester dans Unity avec `Play`. La scene cree une petite map automatiquement si les prefabs sont pas encore placer.

- `START` : commence la partie.
- `BUILD` : active le mode construction.
- `RESTART` : recommence la scene.
- `Space` : start dans le simulateur.
- `B` : build dans le simulateur.
- `R` : restart dans le simulateur.
- clic gauche : remplace la gachette pour tester selection / deplacement / attaque.

Avec un casque VR, la gachette droite sert normalement a pointer et cliquer. Le clic souris reste la facon la plus simple pour tester dans l'editeur.

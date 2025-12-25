# WebApi-EF : Architecture .NET & Clean Code

Ce projet est une démonstration technique d'une Web API robuste couplée à Entity Framework, mettant l'accent sur la maintenabilité, la testabilité et l'industrialisation du code.

## Architecture et Qualité du Code

L'architecture du projet a été pensée pour respecter les principes SOLID et la séparation des responsabilités :

* **Clean Architecture** : Séparation stricte entre les couches (Domain, Data, API).
    * Entities : Cœur du métier, isolé des frameworks externes.
    * Dto (Data Transfer Objects) : Sécurisation et formatage des données exposées.
    * Services : Logique métier découplée des contrôleurs.
* **Testabilité & Mocking** :
    * Utilisation de Stubs (/Stub) pour simuler les dépendances et isoler les tests.
    * Projet dédié aux Tests unitaires (/Tests) pour garantir la fiabilité des régressions.
* **Code Moderne** : Utilisation des Extensions pour garder un code lisible et concis.

## DevOps & Industrialisation

Le projet intègre nativement les standards de déploiement modernes :

* **Dockerisé** : Configuration complète via Dockerfile et .dockerignore pour un environnement iso-prod.
* **CI/CD Ready** : Pipeline d'intégration continue configuré avec Drone CI (.drone.yml).

## Stack Technique

* .NET Core / ASP.NET Web API
* Entity Framework Core (ORM)
* Docker
* xUnit / NUnit (Testing)

## Installation et Démarrage

1. Cloner le dépôt
   git clone https://github.com/JADS63/WebApi-EF.git

2. Lancer avec Docker
   docker build -t webapi-ef .
   docker run -p 8080:80 webapi-ef

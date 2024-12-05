# 🛠️ Projet - API REST, JWT Bearer et Logging avancé dans ASP.NET Core


## 🌟 Objectifs du projet

1. **Implémentation d'une API REST** :
   - Structurer une API RESTful en suivant les principes de REST pour garantir une communication fluide et intuitive.
   - Rendre l'API modulable, maintenable et performante.
   
2. **Sécurité et conformité** :
   - Intégrer les normes **OWASP** et garantir la conformité avec la **RGPD**.
   - Implémenter une authentification sécurisée avec **JWT (JSON Web Token)**.
   
3. **Performance et qualité du code** :
   - Utiliser **ASP.NET Core**, **Entity Framework**, et des bonnes pratiques de développement pour une API robuste et efficace.
   - Garantir une architecture propre et conforme aux standards.

### 📌 Pourquoi deux approches de logging ?
1. **PostSharp** : Utilisé pour ajouter des comportements de logging sans polluer la logique métier.
   - **Avantage** : Gère efficacement les aspects transversaux comme les logs d'appels d'API.
   - **Limite** : Difficulté à capturer le corps des requêtes HTTP.
2. **Middleware ASP.NET Core** : Complémente PostSharp pour capturer les corps des requêtes et des réponses.
   - **Avantage** : Offre une gestion fine des logs et une flexibilité accrue.

---

## 📝 Objectifs du projet :

1. **Implémentation d'une API REST** :
   - Structurer une API RESTful en suivant les principes de REST pour garantir une communication fluide et intuitive.
   - Rendre l'API modulable, maintenable et performante.
   
2. **Sécurité et conformité** :
   - Intégrer les normes **OWASP** et garantir la conformité avec la **RGPD**.
   - Implémenter une authentification sécurisée avec **JWT (JSON Web Token)**.
   
3. **Performance et qualité du code** :
   - Utiliser **ASP.NET Core**, **Entity Framework**, et des bonnes pratiques de développement pour une API robuste et efficace.
   - Garantir une architecture propre et conforme aux standards.

4. **Logging avancé** :
   - Implémenter un système de **logging centralisé** pour capturer les événements de l’application.
   - Stocker les logs dans des **fichiers avec rotation et archivage automatique**.
   - Permettre une **visualisation en temps réel via la console** pour le suivi en environnement de développement.

---

## ✨ Fonctionnalités principales :

### 🌐 API REST :
- **Endpoints CRUD** pour gérer les entités financières.
- Gestion des transactions avec des DTO (Data Transfer Objects) pour une communication optimisée.
- Validation des entrées et gestion des erreurs avec des réponses standardisées.

### 🔒 Sécurité :
- Authentification sécurisée avec **JWT**.
- Application des normes **OWASP** et RGPD.
- Vérification des permissions et des rôles pour limiter l'accès aux données sensibles.

### 📈 Performance et modularité :
- Intégration d'**Entity Framework** pour gérer les bases de données.
- Structure orientée ressources pour une API maintenable et évolutive.

### 📝 Logging avancé :
- **Systèmes de logs multiples** :
  - Stockage des logs dans des fichiers avec **rotation et archivage automatique** pour conserver un historique des événements.
  - **Affichage en temps réel** dans la console pour un suivi en environnement de développement.
- **Logs contextuels** :
  - Inclure les informations importantes comme l’utilisateur, la requête API, le statut de réponse et les erreurs.
- **Faciliter l'audit et le diagnostic** grâce à des logs détaillés et formatés.

---

## 📚 Mise en œuvre des spécifications REST :
Le projet vise à implémenter une **API RESTful** respectant les standards REST, incluant :
- Une architecture orientée ressources.
- Des méthodes HTTP bien définies pour les opérations CRUD (Create, Read, Update, Delete).
- Une gestion des statuts HTTP pour un retour d'informations précis aux clients.

---

## 🛠️ Instructions d'installation

### Prérequis
- [Visual Studio 2019 ou une version plus récente](https://visualstudio.microsoft.com/).
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).

### Étapes :
1. **Cloner le projet** :
   ```bash
   git clone https://github.com/EveCrystali/AdvancedLogging.git
   ```
2. Configurer le projet :

  - Ouvrez le projet dans Visual Studio.
  - Mettez à jour la chaîne de connexion dans appsettings.json.

3. Exécuter le projet :

  - Lancez l'application en appuyant sur F5.

---

# 🚀 Conclusion :
Ce projet illustre une approche avancée du logging avec des systèmes complémentaires pour le stockage, le suivi en temps réel et la gestion des erreurs. Il démontre également la capacité à implémenter une API RESTful sécurisée et performante, conforme aux standards modernes.

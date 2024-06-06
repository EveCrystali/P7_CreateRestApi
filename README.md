# DotNetFrançaisP7
Dépôt de l’étudiant pour le projet 7 du parcours Développeur back-end.NET

La création d'une base de données avec l’approche Code-first d’Entity Framework est nécessaire à la réalisation de ce projet. 

Créez les entités comme indiqué dans le document PDF associé aux informations du projet 7. Utilisez ensuite Entity Framework Code-First pour créer la base de données ainsi que toutes les tables nécessaires. 

Pour créer correctement la base de données, vous devez satisfaire aux prérequis ci-dessous et modifier les chaînes de connexion pour qu'elles pointent vers le serveur MSSQL fonctionnant sur votre PC local.

**Prérequis** : MSSQL Developer 2019 ou Express 2019 a été installé ainsi que Microsoft SQL Server Management Studio (SSMS).

MSSQL : https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads

SSMS : https://docs.microsoft.com/fr-fr/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16

*Remarque : les versions antérieures de MSSQL Server devraient fonctionner sans problèmes, mais elles n’ont pas été testées.

*Dans le projet P7CreateRestApi, ouvrez le fichier appsettings.json file.*

Vous verrez la section ConnectionStrings qui définit les chaînes de connexion pour la base de données utilisée dans cette application.

      "ConnectionStrings":
      {
        "DefaultConnection": "Server=.;Database=VOTRE BASE DE DONNÉES;Trusted_Connection=True;MultipleActiveResultSets=true"
      }

Il existe différentes versions de MSSQL (veuillez utiliser MSSQL pour ce projet et non une autre base de données). Lors de la configuration du serveur de base de données, certains paramètres peuvent modifier la configuration, de sorte que les chaînes de connexion définies pourraient ne pas fonctionner.

Les chaînes de connexion définies dans le projet sont configurées pour MSSQL Server Standard 2019. L’installation n’ayant pas créé de nom d’instance, le serveur est simplement désigné par « . », ce qui signifie l’instance par défaut du serveur MSSQL en cours d’exécution sur la machine actuelle. Pendant l’installation, c’est l’utilisateur intégré de Windows qui est configuré dans le serveur MSSQL par défaut.

Si vous avez installé MSSQL Express, la valeur à utiliser pour Server est probablement .\SQLEXPRESS. Donc votre chaîne de connexion à la base de données serait : -

    "DefaultConnection": "Server=.\SQLEXPRESS;Database=VOTRE BASE DE DONNÉES;Trusted_Connection=True;MultipleActiveResultSets=true"


Vous devrez implémenter l’authentification et l’autorisation JWT avec **Microsoft Identity**. 

Si vous rencontrez des difficultés de connexion, essayez d’abord de vous connecter avec Microsoft SQL Server Management Studio (assurez-vous que le type d’authentification est « authentification Windows »), ou consultez le site https://sqlserver-help.com/2011/06/19/help-whats-my-sql-server-name/. Si le problème persiste, demandez de l’aide à votre mentor.

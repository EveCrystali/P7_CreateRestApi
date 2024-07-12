using System.Collections.Immutable;

namespace P7CreateRestApi.Tests
{
    public static class SampleTestVariables
    {
        // Chaînes de base
        private static readonly string? stringNull = null;
        private static readonly string stringEmpty = "";
        private static readonly string stringSpace = " ";
        private static readonly string stringNumbers = "1234567890";
        private static readonly string stringSpecialChars = "!@#$%^&*()";

        // Liste des chaînes de base pour les combinaisons
        private static readonly List<string?> baseStrings = new()
        {
            stringNull,
            stringEmpty,
            stringSpace,
            stringNumbers,
            stringSpecialChars
        };

        // Méthode pour générer une chaîne de caractères de longueur spécifiée
        private static string GenerateString(int length, char fillChar = 'x')
        {
            return new string(fillChar, length);
        }

        // Création d'un ensemble de chaînes à utiliser dans les tests
        public static TheoryData<string?, int> GetStringCombinationsTest(int maxLength)
        {
            return GenerateStringCombinationsTest(maxLength);
        }

        private static TheoryData<string?, int> GenerateStringCombinationsTest(int maxLength)
        {
            var combinations = new TheoryData<string?, int>();

            // Ajout des chaînes de base
            int baseCode = 0;
            foreach (var str in baseStrings)
            {
                combinations.Add(str, baseCode++);
            }

            // Longueur maximale parmi les chaînes de base
            int maxBaseLength = baseStrings.Where(s => s != null).Max(s => s.Length);

            // Calcul de la longueur pour stringBaseNotTooLong et stringBaseTooLong
            int lengthForNotTooLong = maxLength - 1 - maxBaseLength;
            int lengthForTooLong = maxLength + 1;

            // Génération des chaînes dynamiques nécessaires
            string stringBaseNotTooLong = GenerateString(lengthForNotTooLong);
            string stringBaseTooLong = GenerateString(lengthForTooLong);

            // Ajout des combinaisons pertinentes
            int combinationCode = 1000;

            // Combinaisons doubles
            foreach (var baseString1 in baseStrings)
            {
                foreach (var baseString2 in baseStrings)
                {
                    // Ajout des combinaisons de baseStrings
                    if (baseString1 != null && baseString2 != null)
                    {
                        var combinedBaseStrings = baseString1 + baseString2;
                        combinations.Add(combinedBaseStrings, combinationCode++);
                    }
                }
            }

            // Combinaisons triples avec stringBaseNotTooLong
            var longestCombinationBelowMax = stringBaseNotTooLong + stringSpecialChars + stringNumbers;
            if (longestCombinationBelowMax.Length == maxLength - 1)
            {
                combinations.Add(longestCombinationBelowMax, combinationCode++);
            }

            // Ajout de stringBaseTooLong seul
            if (stringBaseTooLong.Length == maxLength + 1)
            {
                combinations.Add(stringBaseTooLong, combinationCode++);
            }

            // Ajout des combinaisons triples de baseStrings
            foreach (var baseString1 in baseStrings)
            {
                foreach (var baseString2 in baseStrings)
                {
                    foreach (var baseString3 in baseStrings)
                    {
                        // Ajout des combinaisons de baseStrings
                        if (baseString1 != null && baseString2 != null && baseString3 != null)
                        {
                            var combinedBaseStrings = baseString1 + baseString2 + baseString3;
                            combinations.Add(combinedBaseStrings, combinationCode++);
                        }
                    }
                }
            }

            return combinations;
        }


        // Listes de catégories de chaînes
        private static bool ContainsSpecialChars(string str) => str.Any(ch => !char.IsLetterOrDigit(ch));
        private static bool ContainsSpace(string str) => str.Contains(' ');
        private static bool ContainsNumbers(string str) => str.Any(char.IsDigit);


        // Génération automatique des listes
        public static ImmutableList<int?> GenerateStringMandatory(TheoryData<string?, int> data)
        {
            return data
                .Where(item => item[0] == null || string.IsNullOrEmpty((string?)item[0]) || (string?)item[0] == " ")
                .Select(item => (int?)item[1])
                .ToImmutableList();
        }

        public static ImmutableList<int?> GenerateContainsSpecialChars(TheoryData<string?, int> data)
        {
            return data
            .Where(item => item[0] != null && ContainsSpecialChars((string)item[0]))
            .Select(item => (int?)item[1])
            .ToImmutableList();
        }

        public static ImmutableList<int?> GenerateContainsSpace(TheoryData<string?, int> data)
        {
            return data
                .Where(item => item[0] != null && ContainsSpace((string)item[0]))
                .Select(item => (int?)item[1])
                .ToImmutableList();
        }

        public static ImmutableList<int?> GenerateContainsNumbers(TheoryData<string?, int> data)
        {
            return data
                .Where(item => item[0] != null && ContainsNumbers((string)item[0]))
                .Select(item => (int?)item[1])
                .ToImmutableList();
        }

        // Fonctions utilitaires pour filtrer les chaînes par longueur
        public static ImmutableList<int?> MoreThanNChar(TheoryData<string?, int> data, int maxLength)
        {
            return data
                .Select(item => ((string?)item[0], (int)item[1]))
                .Where(entry => entry.Item1?.Length > maxLength)
                .Select(entry => (int?)entry.Item2)
                .ToImmutableList();
        }

        public static ImmutableList<int?> LessThanNChar(TheoryData<string?, int> data, int maxLength)
        {
            return data
                .Select(item => ((string?)item[0], (int)item[1]))
                .Where(entry => entry.Item1?.Length <= maxLength)
                .Select(entry => (int?)entry.Item2)
                .ToImmutableList();
        }
    }
}

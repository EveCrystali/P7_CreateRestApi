using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;

namespace P7CreateRestApi.Tests
{
    public static class SampleTestVariables
    {
        private static readonly string? stringNull = null;
        private static readonly string stringEmpty = "";
        private static readonly string stringSpace = " ";
        private static readonly string stringNumbers = "1234567890";
        private static readonly string stringSpecialChars = "!@#$%^&*()*/-+<>?{}[]|";

        private static readonly List<string?> baseStrings = new()
        {
            stringNull,
            stringEmpty,
            stringSpace,
            stringNumbers,
            stringSpecialChars
        };

        private static string GenerateString(int length, char fillChar = 'x')
        {
            return new string(fillChar, length);
        }

        public static TheoryData<string?, int> GetStringCombinationsTest(int maxLength)
        {
            var combinations = new HashSet<(string?, int)>();
            var result = new TheoryData<string?, int>();

            // Generate single base string combinations
            for (int i = 0; i < baseStrings.Count; i++)
            {
                string? baseString = baseStrings[i];
                combinations.Add((baseString, i));

                if (baseString != null)
                {
                    combinations.Add((baseString + GenerateString(maxLength - 1 - baseString.Length), combinations.Count));
                    combinations.Add((baseString + GenerateString(maxLength - baseString.Length), combinations.Count));
                    combinations.Add((baseString + GenerateString(maxLength + 1 - baseString.Length), combinations.Count));
                }
            }

            // Generate double combinations
            for (int i = 0; i < baseStrings.Count; i++)
            {
                for (int j = i + 1; j < baseStrings.Count; j++)
                {
                    var base1 = baseStrings[i];
                    var base2 = baseStrings[j];

                    if (base1 != null && base2 != null)
                    {
                        combinations.Add((base1 + base2, combinations.Count));
                        
                        AddPaddedCombination(combinations, base1, base2, maxLength - 1);
                        AddPaddedCombination(combinations, base1, base2, maxLength);
                        AddPaddedCombination(combinations, base1, base2, maxLength + 1);
                    }
                }
            }

            // Generate triple combinations
            for (int i = 0; i < baseStrings.Count; i++)
            {
                for (int j = i + 1; j < baseStrings.Count; j++)
                {
                    for (int k = j + 1; k < baseStrings.Count; k++)
                    {
                        var base1 = baseStrings[i];
                        var base2 = baseStrings[j];
                        var base3 = baseStrings[k];

                        if (base1 != null && base2 != null && base3 != null)
                        {
                            combinations.Add((base1 + base2 + base3, combinations.Count));
                            
                            AddPaddedCombination(combinations, base1, base2 + base3, maxLength - 1);
                            AddPaddedCombination(combinations, base1, base2 + base3, maxLength);
                            AddPaddedCombination(combinations, base1, base2 + base3, maxLength + 1);
                        }
                    }
                }
            }

            // Add combinations to result
            foreach (var combo in combinations)
            {
                result.Add(combo.Item1, combo.Item2);
            }

            return result;
        }

        private static void AddPaddedCombination(HashSet<(string?, int)> combinations, string base1, string base2, int targetLength)
        {
            int paddingLength = targetLength - base1.Length - base2.Length;
            if (paddingLength >= 0)
            {
                combinations.Add((base1 + GenerateString(paddingLength) + base2, combinations.Count));
            }
        }

        // Utility methods remain the same...

        public static ImmutableList<int?> GenerateStringMandatory(TheoryData<string?, int> data)
        {
            return data
                .Where(item => item[0] == null || string.IsNullOrWhiteSpace((string?)item[0]))
                .Select(item => (int?)item[1])
                .ToImmutableList();
        }

        public static ImmutableList<int?> GenerateContainsSpecialChars(TheoryData<string?, int> data)
        {
            return data
                .Where(item => item[0] != null && ((string)item[0]).Any(ch => !char.IsLetterOrDigit(ch)))
                .Select(item => (int?)item[1])
                .ToImmutableList();
        }

        public static ImmutableList<int?> MoreThanNChar(TheoryData<string?, int> data, int maxLength)
        {
            return data
                .Where(item => item[0]?.ToString()?.Length > maxLength)
                .Select(item => (int?)item[1])
                .ToImmutableList();
        }

        public static ImmutableList<int?> LessThanNChar(TheoryData<string?, int> data, int maxLength)
        {
            return data
                .Where(item => item[0]?.ToString()?.Length <= maxLength)
                .Select(item => (int?)item[1])
                .ToImmutableList();
        }
    }
}



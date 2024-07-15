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

        private static string GenerateString(int length, char fillChar = 's')
        {
            return new string(fillChar, length);
        }

         public static TheoryData<string?, string> GetStringCombinationsTest(int maxLength)
        {
            var combinations = new HashSet<(string?, string)>();
            var result = new TheoryData<string?, string>();

            // Generate single base string combinations
            for (int i = 0; i < baseStrings.Count; i++)
            {
                string? baseString = baseStrings[i];
                string description = baseString switch
                {
                    null => "null",
                    "" => "empty",
                    " " => "space",
                    "1234567890" => "numbers",
                    "!@#$%^&*()*/-+<>?{}[]|" => "specialChars",
                    _ => "unknown"
                };
                combinations.Add((baseString, description));

                if (baseString != null)
                {
                    combinations.Add((baseString + GenerateString(maxLength - 1 - baseString.Length), description + $" + s{maxLength - 1 - baseString.Length} |TOTAL chars: {maxLength - 1}"));
                    combinations.Add((baseString + GenerateString(maxLength - baseString.Length), description + $" + s{maxLength - baseString.Length} | TOTAL chars: {maxLength}"));
                    combinations.Add((baseString + GenerateString(maxLength + 1 - baseString.Length), description + $" + s{maxLength + 1 - baseString.Length} | TOTAL chars: {maxLength + 1}"));
                }
            }

            // Generate double combinations
            for (int i = 0; i < baseStrings.Count; i++)
            {
                for (int j = i + 1; j < baseStrings.Count; j++)
                {
                    var base1 = baseStrings[i];
                    var base2 = baseStrings[j];
                    string desc1 = base1 switch
                    {
                        null => "null",
                        "" => "empty",
                        " " => "space",
                        "1234567890" => "numbers",
                        "!@#$%^&*()*/-+<>?{}[]|" => "specialChars",
                        _ => "unknown"
                    };
                    string desc2 = base2 switch
                    {
                        null => "null",
                        "" => "empty",
                        " " => "space",
                        "1234567890" => "numbers",
                        "!@#$%^&*()*/-+<>?{}[]|" => "specialChars",
                        _ => "unknown"
                    };
                    if (base1 != null && base2 != null)
                    {
                        combinations.Add((base1 + base2, desc1 + " + " + desc2));
                        
                        AddPaddedCombination(combinations, base1, base2, maxLength - 1, desc1 + " + " + desc2 + $" + s{maxLength - 1 - base1.Length - base2.Length} | TOTAL chars: {maxLength - 1 }");
                        AddPaddedCombination(combinations, base1, base2, maxLength, desc1 + " + " + desc2 + $" + s{maxLength - base1.Length - base2.Length} | TOTAL chars: {maxLength}");
                        AddPaddedCombination(combinations, base1, base2, maxLength + 1, desc1 + " + " + desc2 + $" + s{maxLength + 1 - base1.Length - base2.Length} | TOTAL chars: {maxLength + 1}");
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
                        string desc1 = base1 switch
                        {
                            null => "null",
                            "" => "empty",
                            " " => "space",
                            "1234567890" => "numbers",
                            "!@#$%^&*()*/-+<>?{}[]|" => "specialChars",
                            _ => "unknown"
                        };
                        string desc2 = base2 switch
                        {
                            null => "null",
                            "" => "empty",
                            " " => "space",
                            "1234567890" => "numbers",
                            "!@#$%^&*()*/-+<>?{}[]|" => "specialChars",
                            _ => "unknown"
                        };
                        string desc3 = base3 switch
                        {
                            null => "null",
                            "" => "empty",
                            " " => "space",
                            "1234567890" => "numbers",
                            "!@#$%^&*()*/-+<>?{}[]|" => "specialChars",
                            _ => "unknown"
                        };
                        if (base1 != null && base2 != null && base3 != null)
                        {
                            combinations.Add((base1 + base2 + base3, desc1 + " + " + desc2 + " + " + desc3));
                            
                            AddPaddedCombination(combinations, base1, base2 + base3, maxLength - 1, desc1 + " + " + desc2  + " + " + desc3 + $" + s{maxLength - 1} | TOTAL chars: {maxLength - 1}");
                            AddPaddedCombination(combinations, base1, base2 + base3, maxLength, desc1 + " + " + desc2 + " + "  + desc3 + $" + s{maxLength} | TOTAL chars: {maxLength}");
                            AddPaddedCombination(combinations, base1, base2 + base3, maxLength + 1, desc1 + " + " + desc2 + " + "  + desc3 + $" + s{maxLength + 1} | TOTAL chars: {maxLength + 1}");
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

         private static void AddPaddedCombination(HashSet<(string?, string)> combinations, string base1, string base2, int targetLength, string description)
        {
            int paddingLength = targetLength - base1.Length - base2.Length;
            if (paddingLength >= 0)
            {
                combinations.Add((base1 + GenerateString(paddingLength) + base2, description));
            }
        }

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



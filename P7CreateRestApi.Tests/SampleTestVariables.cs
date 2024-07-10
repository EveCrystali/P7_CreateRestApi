namespace P7CreateRestApi.Tests;

public static class SampleTestVariables
{
    public const string? stringNull = null;
    public const string? string51 = "50string6666666666666666666666666666666666666666666";
    public const string? string101 = "100string66666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666";

    public const string? string501 = "501string66666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666"
                                        + "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666"
                                        + "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666"
                                        + "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666"
                                        + "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666"
                                        + "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666";

    public const string? stringEmpty = "";
    public const string? stringSpace = " ";
    public const string? stringSmall = "string";
    public const string? stringNumbers = "1234567890";
    public const string? stringSpecialChars = "!@#$%^&*()";

    // Creation of a set of strings to be used in the tests
    public static TheoryData<string?, int> StringCombinationsTest => new()
    {
        // Incrementing the int number by 1 for each new combination, providing a unique code for each combination
        // The int number purpose is to make the test results more readable when a error occurs

        // Simple combinations
        { stringNull, 0},
        { stringEmpty, 1},
        { stringSpace, 2},
        { stringSmall, 3},
        { stringNumbers, 4},
        { stringSpecialChars, 5},
        { string51,51},
        { string101, 101},
        { string501, 501},

        // Double combinations
        {stringSmall + stringNumbers, 34},
        {stringSmall + stringSpace, 32},
        {stringSmall + stringSpecialChars, 35},

        {string51 + stringNumbers, 514},
        {string51 + stringSpace, 512},
        {string51 + stringSpecialChars, 515},

        {string101 + stringNumbers, 1014},
        {string101 + stringSpace, 1012},
        {string101 + stringSpecialChars, 1015},

        {string501 + stringNumbers, 5014},
        {string501 + stringSpace, 5012},
        {string501 + stringSpecialChars, 5015},

        // Triple combinations
        {stringSmall + stringNumbers + stringSpecialChars, 345},
        {stringSmall + stringSpace + stringNumbers, 324},
        {stringSmall + stringSpace + stringSpecialChars, 325},
        {stringSmall + stringSpace + stringSpecialChars + stringNumbers, 3254},

        {string51 + stringNumbers + stringSpecialChars, 5145},
        {string51 + stringSpace + stringNumbers, 5124},
        {string51 + stringSpace + stringSpecialChars, 5125},
        {string51 + stringSpace + stringSpecialChars + stringNumbers, 51254},

        {string101 + stringNumbers + stringSpecialChars, 10145},
        {string101 + stringSpace + stringNumbers, 10124},
        {string101 + stringSpace + stringSpecialChars, 10125},
        {string101 + stringSpace + stringSpecialChars + stringNumbers, 101254},

        {string501 + stringNumbers + stringSpecialChars, 50145},
        {string501 + stringSpace + stringNumbers, 50124},
        {string501 + stringSpace + stringSpecialChars, 50125},
        {string501 + stringSpace + stringSpecialChars + stringNumbers, 501254},
    };

    public static readonly List<int?> stringMandatory = [0, 1, 2];
    public static readonly List<int> moreThan51Char = [51, 101, 501, 514, 515, 512, 1014, 1015, 1012, 5014, 5015, 5012, 5145, 5124, 5125, 51254, 10145, 10124, 10125, 101254, 50145, 50124, 50125, 501254];
    public static readonly List<int> moreThan101Char = [101, 1014, 1015, 1012, 10145, 10124, 10125, 101254, 501, 5014, 5015, 5012, 50145, 50124, 50125, 501254];
    public static readonly List<int> moreThan501Char = [501, 5014, 5015, 5012, 50145, 50124, 50125, 501254];
    public static readonly List<int> containsSpecialChars = [5, 35, 515, 1015, 5015, 345, 325, 3254, 5145, 5125, 51254, 10145, 10125, 101254, 50145, 50125, 501254];
    public static readonly List<int> containsSpace = [2, 32, 512, 1012, 5012, 324, 325, 3254, 5124, 5125, 51254, 10124, 10125, 101254, 50124, 50125, 501254];
    public static readonly List<int> containsNumbers = [4, 34, 514, 1014, 5014, 345, 324, 3254, 5145, 5124, 51254, 10145, 10124, 101254, 50145, 50124, 501254];
    public static readonly List<int> lessThan25Char = [3, 4, 5, 34, 32, 35, 324, 325]; // 345 : is 26 chars !
    public static readonly List<int> lessThan51Char = [3, 4, 5, 34, 32, 35, 345, 324, 325, 3254];
    public static readonly List<int> lessThan101Char = [3, 4, 5, 34, 32, 35, 51, 345, 324, 325, 3254, 514, 512, 515, 3254, 5125, 51254, 5124, 5145, 51245];
    public static readonly List<int> lessThan501Char = [3, 4, 5, 34, 32, 35, 51, 345, 324, 325, 3254, 514, 512, 515, 3254, 5125, 51254, 5124, 5145, 51245, 101, 1014, 1012, 1015, 10145, 10124, 10125, 101254];
}
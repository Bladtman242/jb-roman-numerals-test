using RomanNumerals.Lib;

string[] cliArgs = Environment.GetCommandLineArgs();

string usage = $"Usage: {cliArgs[0]} <positive integer>";

uint number;

if (cliArgs.Length == 1) {
    Console.WriteLine(usage);
    Environment.Exit(1);
}
else if (!uint.TryParse(cliArgs[1], out number))
{
    Console.WriteLine(usage + "boo");
    Environment.Exit(2);
}
else
{
    Console.WriteLine(ArabToRomanNumeralConverter.UIntToRomanNumeral(number));
}



// vim: set ts=4 sw=4 et:

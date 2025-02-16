namespace RomanNumerals.Lib;
using Digits;

public class ArabToRomanNumeralConverter
{
    private static readonly RomanDigits[] allRomanDigits = RomanDigitsExtension.RomanDigitsDescending;

    // helper to find the smallest roman digit larger than number. also returns
    // the index, so we can later take the next, smaller, roman digit if we
    // need to. returns (0, M) if number > 1000
    private static (int, RomanDigits) SmallestDigitLargerThan(uint arab)
    {
        RomanDigits digit = allRomanDigits[0];
        int i = 0;
        while ((uint) allRomanDigits[i+1] > arab)
        {
            digit = allRomanDigits[++i];
        }

        return (i, digit);
    }

    // The over all strategy is to pick the smallest single roman digit, that
    // is larger then number, and then check if adding a legal subtraction
    // takes us to or below number. if it does not, we take the next, smaller,
    // roman single digit d, add it to the output, and then recurse on (number
    // - d) to find the smaller roman numeral we should add (by appending it to
    // d), to get the complete roman numeral for d. If the initial subtraction
    // _does_ take us to or below number, we similarly output the subtraction,
    // followed by d, followed by the roman numeral needed (if any) to increase
    // the value to number, found by recursion. By convention we shall say that
    // the empty string is the roman numeral zero, since zero is not
    // representable by roman numerals
    public static string UIntToRomanNumeral(uint number)
    {
        if(number == 0) {
            return "";
        }

        RomanDigits digit = allRomanDigits[0];

        // When number >= our largest roman single-digit, we cannot find a
        // roman digit with a larger value
        if (number >= (uint) digit)
        {
            return digit.ToRomanNumeralString() + UIntToRomanNumeral(number - (uint) digit);
        }

        (int idx, digit) = SmallestDigitLargerThan(number);

        RomanDigits subtraction = digit.PermissibleSubtraction();

        // digit cannot get to or below number by legal subtraction.
        // bump digit to the next (smaller) roman single-digit, which is guaranteed to be no smaller than number,
        // and recurse to find the needed addition (if any)
        if (digit - subtraction > number)
        {
            digit = allRomanDigits[idx + 1];
            return digit.ToRomanNumeralString() + UIntToRomanNumeral(number - (uint) digit);
        }
        // subtraction takes us to or below number, recurse to find needed addition (if any)
        else
        {
            return
                subtraction.ToRomanNumeralString()
                + digit.ToRomanNumeralString()
                + UIntToRomanNumeral((uint) (number + subtraction - digit));

        }
    }
}

// vim: set ts=4 sw=4 et:

namespace RomanNumerals.Lib.Digits
{
    public enum RomanDigits: uint
    {
        NONE = 0,
        I = 1,
        V = 5,
        X = 10,
        L = 50,
        C = 100,
        D = 500,
        M = 1000
    }

    public static class RomanDigitsExtension
    {
        public static string ToRomanNumeralString(this RomanDigits digit) =>
            digit switch
            {
                RomanDigits.NONE => "",
                var x => x.ToString()
            };

        public static RomanDigits PermissibleSubtraction(this RomanDigits digit)
        {
            switch (digit)
            {
                case RomanDigits.M:
                case RomanDigits.D:
                    return RomanDigits.C;
                case RomanDigits.L:
                case RomanDigits.C:
                    return RomanDigits.X;
                case RomanDigits.X:
                case RomanDigits.V:
                    return RomanDigits.I;
                default:
                    return RomanDigits.NONE;

            }
        }

        public static readonly RomanDigits[] RomanDigitsDescending = Enum.GetValues<RomanDigits>();

        static RomanDigitsExtension ()
        {
            Array.Reverse(RomanDigitsDescending);
        }
    }
}


// vim: set ts=4 sw=4 et:

namespace RomanNumerals.Test;

using static RomanNumerals.Lib.ArabToRomanNumeralConverter;
using RomanNumerals.Lib.Digits;
using FsCheck;
using FsCheck.Fluent;
using System.Linq;

public class ArabToRomanNumeralConverterTest
{
    static Config defaultTestConfig =
        Config
            .QuickThrowOnFailure
            .WithParallelRunConfig(new ParallelRunConfig(Environment.ProcessorCount));
    [Fact]
    public void _0_is_empty()
    {
        Assert.Equal("", UIntToRomanNumeral(0));
    }

    [Fact]
    public void _1_is_I()
    {
        Assert.Equal("I", UIntToRomanNumeral(1));
    }

    [Fact]
    public void _2_is_II()
    {
        Assert.Equal("II", UIntToRomanNumeral(2));
    }

    [Fact]
    public void _4_is_IV()
    {
        Assert.Equal("IV", UIntToRomanNumeral(4));
    }

    [Fact]
    public void _5_is_V()
    {
        Assert.Equal("V", UIntToRomanNumeral(5));
    }

    [Fact]
    public void _6_is_VI()
    {
        Assert.Equal("VI", UIntToRomanNumeral(6));
    }

    [Fact]
    public void _90_is_XC()
    {
        Assert.Equal("XC", UIntToRomanNumeral(90));
    }

    [Fact]
    public void _99_is_XCIX()
    {
        Assert.Equal("XCIX", UIntToRomanNumeral(99));
    }

    [Fact]
    public void _1999_is_MCMXCIX()
    {
        Assert.Equal("MCMXCIX", UIntToRomanNumeral(1999));
    }

    [Fact]
    public void _2444_is_MMCDXLIV()
    {
        Assert.Equal("MMCDXLIV", UIntToRomanNumeral(2444));
    }

    // We only test up to 3999. The spec allows us to make errors for
    // values >= 300, and because we don't use a roman digit for values
    // above M/1000, we will start using more that 3 repeats of M from 4000
    // and onwards
    [Fact]
    public void No_quadruples_for_numbers_less_than_4000()
    {
        for (uint i = 0; i <= 3999; i++)
        {
            Assert.DoesNotContain("IIII", UIntToRomanNumeral(i));
            Assert.DoesNotContain("XXXX", UIntToRomanNumeral(i));
            Assert.DoesNotContain("CCCC", UIntToRomanNumeral(i));
            Assert.DoesNotContain("MMMM", UIntToRomanNumeral(i));
        }
    }

    [Fact]
    public void V_L_D_are_never_repeated()
    {
        Prop.ForAll<uint>(arab =>
        {
            string roman = UIntToRomanNumeral(arab);
            return (!roman.Contains("VV"))
            .Label($"Expected no repeat V's in ({roman})")
            .And(!roman.Contains("LL"))
            .Label($"Expected no repeat L's in ({roman})")
            .And(!roman.Contains("DD"))
            .Label($"Expected no repeat D's in ({roman})");
        }).Check(defaultTestConfig.WithMaxTest(10_000));
    }

    [Fact]
    public void Quads_occur_above_3999()
    {
        Prop.ForAll<uint>(offset =>
        {
            uint arab = 4000 + offset;
            String roman = UIntToRomanNumeral(arab);
            return roman.Contains("MMMM").When(arab >= 4000)
                .Label($"{roman} does not have quad Ms");
        }).Check(defaultTestConfig);
    }

    [Fact]
    public void At_most_1_subtractive_prefix()
    {
        Prop.ForAll<uint>(arab =>
        {
            string roman = UIntToRomanNumeral(arab);
            return roman
               .Zip(roman.Skip(1), roman.Skip(2))
               .All(t =>
               {
                    (char a, char b, char c) = t;
                    RomanDigits romanA = Enum.Parse<RomanDigits>(a.ToString());
                    RomanDigits romanB = Enum.Parse<RomanDigits>(b.ToString());
                    RomanDigits romanC = Enum.Parse<RomanDigits>(c.ToString());
                    return !(romanA < romanC && romanB < romanC);
               })
               .Label($"Found two or more smaller roman digits before a larger one: {roman}");
        }).Check(defaultTestConfig);
    }

    [Fact]
    public void Only_IXC_before_larger_digit()
    {
        Prop.ForAll<uint>(arab =>
        {
            string roman = UIntToRomanNumeral(arab);
            return roman
               .Zip(roman.Skip(1))
               .All(t =>
               {
                    (char a, char b) = t;
                    return a == 'I' || a == 'X' || a == 'C' || Enum.Parse<RomanDigits>(a.ToString()) >= Enum.Parse<RomanDigits>(b.ToString());
               })
               .Label($"{roman}");
        }).Check(defaultTestConfig);
    }

    [Fact]
    public void Mostly_homomorphic_with_addition_and_concatenation()
    {
        Prop.ForAll<uint>(offset =>
        {
            string roman = UIntToRomanNumeral(4000);
            string romanOffset = UIntToRomanNumeral(offset);
            string romanSum = UIntToRomanNumeral(4000 + offset);

            return (romanSum == roman + romanOffset)
                .Label($"{romanSum} != {roman} + {romanOffset}");
        }).Check(defaultTestConfig);
    }
}

// vim: set ts=4 sw=4 et:

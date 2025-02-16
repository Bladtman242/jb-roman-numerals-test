# RomanNumerals

The solution is split into 3 components/projects:

1. **Lib** contains the business logic for converting arabic numbers to roman numerals
2. **Test** contains [property-based]([url](https://fscheck.github.io/FsCheck/)) tests of the code in Lib
3. **App** contains a console application, that uses Lib to turn an arabic numeral, given as a single commandline argument, into a roman numeral, printed on stdout.

From the root directory:
```
dotnet test
```
will execute the test suite.

```
dotnet run --project RomanNumerals.App 9
```
Will output the roman numeral equivalent of the arabic number 9: `IX`

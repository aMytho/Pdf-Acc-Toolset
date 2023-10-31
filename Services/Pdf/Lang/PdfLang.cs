// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
namespace Pdf_Acc_Toolset.Services.Pdf.Lang;

/// <summary>
/// Class that contains a few of the common language strings
/// </summary>
public class PdfLang
{
    public const string EnglishUnitedStates = "en-US"; // English (United States)
    public const string EnglishUnitedKingdom = "en-GB"; // English (United Kingdom)
    public const string FrenchFrance = "fr-FR"; // French (France)
    public const string GermanGermany = "de-DE"; // German (Germany)
    public const string SpanishSpain = "es-ES"; // Spanish (Spain)
    public const string JapaneseJapan = "ja-JP"; // Japanese (Japan)
    public const string ItalianItaly = "it-IT"; // Italian (Italy)
    public const string PortuguesePortugal = "pt-PT"; // Portuguese (Portugal)
    public const string ChineseSimplifiedChina = "zh-CN"; // Chinese (Simplified, China)
    public const string KoreanSouthKorea = "ko-KR"; // Korean (South Korea)
    public const string ArabicSaudiArabia = "ar-SA"; // Arabic (Saudi Arabia)
    public const string RussianRussia = "ru-RU"; // Russian (Russia)
    public const string DutchNetherlands = "nl-NL"; // Dutch (Netherlands)
    public const string SwedishSweden = "sv-SE"; // Swedish (Sweden)

    public static IEnumerable<string> GetLanguageOptions()
    {
        yield return EnglishUnitedStates;
        yield return EnglishUnitedKingdom;
        yield return FrenchFrance;
        yield return GermanGermany;
        yield return SpanishSpain;
        yield return JapaneseJapan;
        yield return ItalianItaly;
        yield return PortuguesePortugal;
        yield return ChineseSimplifiedChina;
        yield return KoreanSouthKorea;
        yield return ArabicSaudiArabia;
        yield return RussianRussia;
        yield return DutchNetherlands;
        yield return SwedishSweden;
    }
}

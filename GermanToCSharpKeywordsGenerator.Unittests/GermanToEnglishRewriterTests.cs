using Shouldly;

namespace GermanToCSharpKeywordsGenerator.Unittests;

class GermanToEnglishRewriterTests
{
    [Test]
    public void ConvertGermanToEnglish_IgnoresNonKeywords()
    {
        // Arrange
        var input = @"
            Namenraum EinNamenraum;

            öffentlich Klasse EineKlasse 
            {
                privat Zeichenkette eineVariable = ""ein kommentar Methode""; // Das ist ein Kommentar: Klasse
                öffentlich int EineMethode() 
                {
                    if (eineVariable == ""BMW"")
                    {
                        return 250;
                    }
                    else
                    {
                        return 200;
                    }
                }
            }";

        var expected = @"
            namespace EinNamenraum;

            public class EineKlasse 
            {
                private string eineVariable = ""ein kommentar Methode""; // Das ist ein Kommentar: Klasse
                public int EineMethode() 
                {
                    if (eineVariable == ""BMW"")
                    {
                        return 250;
                    }
                    else
                    {
                        return 200;
                    }
                }
            }";

        // Act
        var result = GermanToEnglishRewriter.ConvertGermanToEnglish(input);

        // Assert
        result.ShouldBeEquivalentTo(expected);
    }
}

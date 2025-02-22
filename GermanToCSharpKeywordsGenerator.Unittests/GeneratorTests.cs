using GermanToCSharpKeywordsGenerator.Unittests.Setup;
using Microsoft.CodeAnalysis;
using Shouldly;

namespace GermanToCSharpKeywordsGenerator.Unittests;

public class Tests
{
    [Test]
    public void SimpleGeneratorTest_Works()
    {
        // Arrange
        var germanSourceFile = @"
            Namenraum Test;

            öffentlich statisch Klasse TestProgram
            {
                öffentlich statisch Leere Main()
                {
                    Console.WriteLine(""Hallo Welt aus deutschem C#!"");
                }
            }
            ";

        var additionalText = new TestAdditionalText("TestProgram.dcs", germanSourceFile);

        var comp = TestCompilationHandler.CreateCompilation();

        // Act
        var output = TestCompilationHandler.RunGenerators(comp, [additionalText], new GermanToCSharpKeywordsGenerator());

        // Assert
        output.ShouldContainNoErrorDiagnostics();
        output.ShouldContainGeneratedClass("Test.TestProgram");
    }
}
using GermanToCSharpGenerator.Unittests.Setup;
using Microsoft.CodeAnalysis;
using Shouldly;

namespace GermanToCSharpGenerator.Unittests;

public class Tests
{
    [Test]
    public void SimpleGeneratorTest_Works()
    {
        // Arrange
        var germanSourceFile = @"
            Namenraum Test;

            intern statisch Klasse TestProgram
            {
                privat statisch Leere Main()
                {
                    Console.WriteLine(""Hallo Welt aus deutschem C#!"");
                }
            }
            ";

        var additionalText = new TestAdditionalText("TestProgram.dcs", germanSourceFile);

        var comp = TestCompilationHandler.CreateCompilation();

        // Act
        var output = TestCompilationHandler.RunGenerators(comp, [additionalText], new GermanToCSharpGenerator());

        // Assert
        output.ShouldContainNoErrorDiagnostics();
        output.ShouldContainGeneratedClass("Test.TestProgram");
    }
}
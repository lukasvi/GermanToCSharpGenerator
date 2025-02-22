using GermanToCSharpKeywordsGenerator.Unittests.Setup;
using Microsoft.CodeAnalysis;
using Shouldly;

namespace GermanToCSharpKeywordsGenerator.Unittests;

public class Tests
{
    private TestCompilationHandler _compilationHandler;

    [SetUp]
    public void Setup()
    {
        _compilationHandler = new();
    }

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

        var comp = _compilationHandler.CreateCompilation();

        // Act
        (var newCompilation, var generatorDiagnostics) = _compilationHandler.RunGenerators(comp, [additionalText], new GermanToCSharpKeywordsGenerator());

        // Assert
        generatorDiagnostics.Where(x => x.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        newCompilation.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();

        // Assert that class / file has been created
        newCompilation.GetTypeByMetadataName("Test.TestProgram").ShouldNotBeNull();
    }
}
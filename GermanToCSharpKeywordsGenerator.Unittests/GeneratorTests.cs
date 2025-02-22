using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Shouldly;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace GermanToCSharpKeywordsGenerator.Unittests;

public class Tests
{
    [Test]
    public void SimpleGeneratorTest_Works()
    {
        // Arrange
        var germanSourceFile = @"
            Namenraum Test;

            öffentlich statisch Klasse GeneratedProgram
            {
                öffentlich statisch Leere Main()
                {
                    Konsole.SchreibeLinie(""Hallo Welt aus deutschem C#!"");
                }
            }
            ";
        var additionalText = new TestAdditionalText("GeneratedProgram.dcs", germanSourceFile);


        Compilation comp = CreateCompilation(string.Empty);

        // Act
        (var newCompilation, var generatorDiagnostics) = RunGenerators(comp, [additionalText], new GermanToCSharpKeywordsGenerator());

        // Assert
        generatorDiagnostics.Where(x => x.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        newCompilation.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();

        // Assert that class / file has been created
        newCompilation.GetTypeByMetadataName("Test.GeneratedProgram").ShouldNotBeNull();
    }

    private static Compilation CreateCompilation(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview));

        var globalUsings = File.ReadAllText("Microsoft.NET.Sdk.usings.cs");
        var usingsTree = CSharpSyntaxTree.ParseText(globalUsings);

        var coreDir = RuntimeEnvironment.GetRuntimeDirectory();

        var references = new[]
            {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),   // System.Private.CoreLib.dll
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console.dll
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location), // System.Linq.dll

            MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Runtime.dll")),
            MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Console.dll")),
            MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Collections.dll")),
            MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Linq.dll")),
            MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Net.Http.dll"))
            };

        return CSharpCompilation.Create("compilation",
            [usingsTree],
            references,
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    }

    private static GeneratorDriver CreateDriver(Compilation compilation, ImmutableArray<AdditionalText> additionalTexts, params ISourceGenerator[] generators)
        => CSharpGeneratorDriver.Create(
            generators: ImmutableArray.Create(generators),
            additionalTexts: additionalTexts,
            parseOptions: (CSharpParseOptions)compilation.SyntaxTrees.First().Options,
            optionsProvider: null
        );

    private static (Compilation, ImmutableArray<Diagnostic>) RunGenerators(
        Compilation compilation,
        ImmutableArray<AdditionalText> additionalTexts,
        params ISourceGenerator[] generators)
    {
        CreateDriver(compilation, additionalTexts, generators)
                .RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generatorDiagnostics);
        return (outputCompilation, generatorDiagnostics);
    }
}
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace GermanToCSharpKeywordsGenerator.Unittests.Setup;

/// <summary>
/// Class for generating and running a <see cref="CSharpCompilation"/> with Generators for testing purposes.
/// </summary>
public static class TestCompilationHandler
{
    /// <summary>
    /// Creates a <see cref="CSharpCompilation"/> from SDK base dlls. Can be modified with optional <param name="source"></param>.
    /// </summary>
    /// <param name="source"></param>
    public static CSharpCompilation CreateCompilation(string source = "")
    {
        var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp10);

        var syntaxTree = CSharpSyntaxTree.ParseText(source, options);

        var globalUsings = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Setup/Microsoft.NET.Sdk.usings.cs"));
        var usingsTree = CSharpSyntaxTree.ParseText(globalUsings, options);

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
            [syntaxTree, usingsTree],
            references,
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    }

    /// <summary>
    /// Runs several Generators on a specific <see cref="CSharpCompilation"/>
    /// </summary>
    /// <param name="compilation"></param>
    /// <param name="additionalTexts"></param>
    /// <param name="generators"></param>
    public static GeneratorRunOutput RunGenerators(
        CSharpCompilation compilation,
        ImmutableArray<AdditionalText> additionalTexts,
        params ISourceGenerator[] generators)
    {
        CreateDriver(compilation, additionalTexts, generators)
                .RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generatorDiagnostics);

        var castCompulation = outputCompilation as CSharpCompilation
            ?? throw new InvalidOperationException("Output compilation is of invalid type. Only CSharpCompilation is supported");

        return new GeneratorRunOutput() { Compilation = castCompulation, Diagnostics = generatorDiagnostics };
    }

    private static CSharpGeneratorDriver CreateDriver(CSharpCompilation compilation, ImmutableArray<AdditionalText> additionalTexts, params ISourceGenerator[] generators)
        => CSharpGeneratorDriver.Create(
            generators: ImmutableArray.Create(generators),
            additionalTexts: additionalTexts,
            parseOptions: (CSharpParseOptions)compilation.SyntaxTrees.First().Options,
            optionsProvider: null
        );
}

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace GermanToCSharpKeywordsGenerator.Unittests.Setup;

class TestCompilationHandler
{
    /// <summary>
    /// Creates a <see cref="CSharpCompilation"/> from SDK base dlls. Can be modified with optional <param name="source"></param>.
    /// </summary>
    /// <param name="source"></param>
    public CSharpCompilation CreateCompilation(string source = "")
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
    public (Compilation, ImmutableArray<Diagnostic>) RunGenerators(
        CSharpCompilation compilation,
        ImmutableArray<AdditionalText> additionalTexts,
        params ISourceGenerator[] generators)
    {
        CreateDriver(compilation, additionalTexts, generators)
                .RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generatorDiagnostics);
        return (outputCompilation, generatorDiagnostics);
    }

    private CSharpGeneratorDriver CreateDriver(CSharpCompilation compilation, ImmutableArray<AdditionalText> additionalTexts, params ISourceGenerator[] generators)
        => CSharpGeneratorDriver.Create(
            generators: ImmutableArray.Create(generators),
            additionalTexts: additionalTexts,
            parseOptions: (CSharpParseOptions)compilation.SyntaxTrees.First().Options,
            optionsProvider: null
        );
}

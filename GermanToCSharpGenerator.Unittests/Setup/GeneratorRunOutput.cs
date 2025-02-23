using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;

namespace GermanToCSharpGenerator.Unittests.Setup;

public class GeneratorRunOutput()
{
    public required CSharpCompilation Compilation { get; init; }

    public required ImmutableArray<Diagnostic> Diagnostics { get; init; }
}

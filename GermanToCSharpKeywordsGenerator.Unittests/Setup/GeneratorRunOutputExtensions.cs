using Microsoft.CodeAnalysis;
using Shouldly;

namespace GermanToCSharpKeywordsGenerator.Unittests.Setup;

public static class GeneratorRunOutputExtensions
{
    public static void ShouldContainNoErrorDiagnostics(this GeneratorRunOutput output)
    {
        output.Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        output.Compilation.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
    }

    public static void ShouldContainGeneratedClass(this GeneratorRunOutput output, string fullQualifiedClassName)
    {
        // Assert that class / file has been created
        output.Compilation.GetTypeByMetadataName(fullQualifiedClassName).ShouldNotBeNull();
    }
}

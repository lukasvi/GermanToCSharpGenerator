using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;

namespace GermanToCSharpKeywordsGenerator;

public static class DiagnosticsReporter
{
    public static void Report(GeneratorExecutionContext context, SyntaxNode root, string filePath)
    {
        var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

        if (classDeclarations.Count == 0)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                GeneratorDiagnostics.ClassNotFoundErrorDescriptor,
                Location.Create(filePath, new TextSpan(0, 0), new LinePositionSpan(new LinePosition(0, 0), new LinePosition(0, 0))),
                filePath
            ));
            return;
        }

        if (classDeclarations.Count > 1)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                descriptor: GeneratorDiagnostics.MultipleClassesErrorDescriptor,
                location: classDeclarations[0].GetLocation(),
                additionalLocations: classDeclarations.Skip(1).Select(x => x.GetLocation()),
                filePath
            ));
        }

        var expectedClassName = Path.GetFileNameWithoutExtension(filePath);
        var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
        if (!classDeclaration.Identifier.Text.Equals(expectedClassName, StringComparison.OrdinalIgnoreCase))
        {
            context.ReportDiagnostic(Diagnostic.Create(
                GeneratorDiagnostics.ClassNameMismatchErrorDescriptor,
                classDeclaration.Identifier.GetLocation(),
                classDeclaration.Identifier.Text, expectedClassName
            ));
        }

        var namespaceDeclaration = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();

        var fileScopednamespaceDeclaration = root.DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();


        if (namespaceDeclaration == null && fileScopednamespaceDeclaration == null)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                GeneratorDiagnostics.NamespaceNotFoundErrorDescriptor,
                Location.Create(filePath, new TextSpan(), new LinePositionSpan()),
                filePath
            ));
            return;
        }
    }

    public static void ReportInternalError(GeneratorExecutionContext context, Exception ex)
    {
        context.ReportDiagnostic(Diagnostic.Create(GeneratorDiagnostics.GeneralErrorDescriptor, Location.None, ex.Message));
    }
}

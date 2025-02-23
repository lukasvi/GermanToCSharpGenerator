using Microsoft.CodeAnalysis;

namespace GermanToCSharpGenerator;

public static class GeneratorDiagnostics
{

    public static readonly DiagnosticDescriptor GeneralErrorDescriptor = new(
        id: "GEN001",
        title: "General Source Generator Execution Error",
        messageFormat: "An error occurred during source generator execution: {0}",
        category: "Generator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor ClassNotFoundErrorDescriptor = new(
        id: "GEN002",
        title: "Source Generator Execution Error: Class not found",
        messageFormat: "The classname was not found in file: {0}",
        category: "Generator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor NamespaceNotFoundErrorDescriptor = new(
        id: "GEN003",
        title: "Source Generator Execution Error: Namespace not found",
        messageFormat: "No namespace was found in file: {0}",
        category: "Generator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MultipleClassesErrorDescriptor = new(
        id: "GEN004",
        title: "Source Generator Execution Warning: Multiple classes found",
        messageFormat: "More than one class was found in file: {0}",
        category: "Generator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor ClassNameMismatchErrorDescriptor = new(
        id: "GEN005",
        title: "Source Generator Execution Warning: Class name does not match file name",
        messageFormat: "The class name '{0}' does not match the file name '{1}'",
        category: "Generator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
}

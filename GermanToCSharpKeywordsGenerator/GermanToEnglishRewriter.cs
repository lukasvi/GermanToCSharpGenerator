using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace GermanToCSharpKeywordsGenerator;

/// <summary>
/// This class provides functionality to convert German C# keywords into their English equivalents.
/// </summary>
public class GermanToEnglishRewriter
{
    private static readonly Dictionary<string, string> _germanToEnglishKeywords = new()
    {
        { "öffentlich", "public" },
        { "privat", "private" },
        { "geschützt", "protected" },
        { "intern", "internal" },
        { "statisch", "static" },
        { "abstrakt", "abstract" },
        { "versiegelt", "sealed" },
        { "neu", "new" },
        { "virtuell", "virtual" },
        { "überschreibt", "override" },
        { "asynchron", "async" },
        { "Aufgabe", "Task" },
        { "erwarte", "await" },
        { "Klasse", "class" },
        { "Schnittstelle", "interface" },
        { "Struktur", "struct" },
        { "Aufzählung", "enum" },
        { "Delegat", "delegate" },
        { "Leere", "void" },
        { "wenn", "if" },
        { "sonst", "else" },
        { "für", "for" },
        { "jede", "foreach" },
        { "während", "while" },
        { "wechseln", "switch" },
        { "Fall", "case" },
        { "Namenraum", "namespace" },
        { "Zeichenkette", "string"},
    };

    /// <summary>
    /// Converts German code keywords to their corresponding English C# keywords while preserving non-keyword elements.
    /// The method translates the entire code into English first and then reverts specific tokens like comments, strings, and identifiers back to their original German form.
    /// </summary>
    /// <param name="sourceCode">The input source code written in German C# keywords.</param>
    /// <returns>The source code with German keywords translated to their English equivalents, with specific elements like comments and strings unchanged.</returns>
    public static string ConvertGermanToEnglish(string sourceCode)
    {
        // Translate all to english to get compilable code.
        var translatedCode = TranslateAllToEnglish(sourceCode);

        var tree = CSharpSyntaxTree.ParseText(translatedCode);
        var root = tree.GetRoot();

        // Translate comments back
        var rootWithComments = root.ReplaceTrivia(root.DescendantTrivia(), (original, rewritten) =>
        {
            if (original.IsKind(SyntaxKind.SingleLineCommentTrivia) || original.IsKind(SyntaxKind.MultiLineCommentTrivia))
            {
                string restoredComment = RestoreGermanKeywords(original.ToString());
                return SyntaxFactory.Comment(restoredComment);
            }
            return original;
        });

        // Translate strings and identifiers back
        var newRoot = rootWithComments.ReplaceTokens(rootWithComments.DescendantTokens(), (original, rewritten) =>
        {
            var originalText = original.Text;

            if (original.IsKind(SyntaxKind.StringLiteralToken) || original.IsKind(SyntaxKind.CharacterLiteralToken))
            {
                var restoredString = RestoreGermanKeywords(originalText);
                return SyntaxFactory.Token(original.LeadingTrivia, original.Kind(), restoredString, restoredString, original.TrailingTrivia);
            }

            if (original.IsKind(SyntaxKind.IdentifierToken))
            {
                string restoredIdentifier = RestoreGermanKeywords(originalText);
                return SyntaxFactory.Identifier(original.LeadingTrivia, restoredIdentifier, original.TrailingTrivia);
            }

            return original;
        });

        return newRoot.ToFullString();
    }

    private static string RestoreGermanKeywords(string text)
    {
        foreach (var kvp in _germanToEnglishKeywords)
        {
            text = text.Replace(kvp.Value, kvp.Key);
        }
        return text;
    }

    private static string TranslateAllToEnglish(string text)
    {
        foreach (var kvp in _germanToEnglishKeywords)
        {
            text = text.Replace(kvp.Key, kvp.Value);
        }
        return text;
    }
}
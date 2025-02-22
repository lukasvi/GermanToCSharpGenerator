using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace GermanToCSharpKeywordsGenerator;

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

    public static string ConvertGermanToEnglish(string sourceCode)
    {
        var translatedCode = TranslateAllToEnglish(sourceCode);

        var tree = CSharpSyntaxTree.ParseText(translatedCode);
        var root = tree.GetRoot();

        var rootWithComments = root.ReplaceTrivia(root.DescendantTrivia(), (original, rewritten) =>
        {
            if (original.IsKind(SyntaxKind.SingleLineCommentTrivia) || original.IsKind(SyntaxKind.MultiLineCommentTrivia))
            {
                string restoredComment = RestoreGermanKeywords(original.ToString());
                return SyntaxFactory.Comment(restoredComment);
            }
            return original;
        });

        var newRoot = rootWithComments.ReplaceTokens(rootWithComments.DescendantTokens(), (original, rewritten) =>
        {
            var originalText = original.Text;

            // 2. **String- und Char-Literale zurückübersetzen**
            if (original.IsKind(SyntaxKind.StringLiteralToken) || original.IsKind(SyntaxKind.CharacterLiteralToken))
            {
                var restoredString = RestoreGermanKeywords(originalText);
                return SyntaxFactory.Token(original.LeadingTrivia, original.Kind(), restoredString, restoredString, original.TrailingTrivia);
            }

            // 3. **Identifizierer (Variablen, Methoden, Klassen, Namespaces) behandeln**
            if (original.IsKind(SyntaxKind.IdentifierToken))
            {
                // Nur C#-Schlüsselwörter übersetzen
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
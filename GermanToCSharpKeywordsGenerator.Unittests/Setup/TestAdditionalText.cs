using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System.Text;

namespace GermanToCSharpKeywordsGenerator.Unittests.Setup;

/// <summary>
/// Mock implementation of <see cref="AdditionalText"/> for testing purposes.
/// </summary>
/// <remarks>
/// Initializes a new instance of <see cref="TestAdditionalText"/> with the given path and content.
/// </remarks>
public class TestAdditionalText(string path, string text) : AdditionalText
{

    /// <summary>
    /// Gets the file path.
    /// </summary>
    public override string Path { get; } = path;

    /// <summary>
    /// Returns the content as a <see cref="SourceText"/>.
    /// </summary>
    public override SourceText? GetText(CancellationToken cancellationToken = default)
    {
        return SourceText.From(text, Encoding.UTF8);
    }
}

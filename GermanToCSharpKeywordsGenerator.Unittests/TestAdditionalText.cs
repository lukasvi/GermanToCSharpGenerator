using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System.Text;

namespace GermanToCSharpKeywordsGenerator.Unittests;

/// <summary>
/// Mock implementation of <see cref="AdditionalText"/> for testing purposes.
/// </summary>
public class TestAdditionalText : AdditionalText
{
    private readonly string _text;

    /// <summary>
    /// Initializes a new instance of <see cref="TestAdditionalText"/> with the given path and content.
    /// </summary>
    public TestAdditionalText(string path, string text)
    {
        Path = path;
        _text = text;
    }

    /// <summary>
    /// Gets the file path.
    /// </summary>
    public override string Path { get; }

    /// <summary>
    /// Returns the content as a <see cref="SourceText"/>.
    /// </summary>
    public override SourceText? GetText(CancellationToken cancellationToken = default)
    {
        return SourceText.From(_text, Encoding.UTF8);
    }
}

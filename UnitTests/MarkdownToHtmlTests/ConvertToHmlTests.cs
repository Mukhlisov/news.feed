using Tools.MarkdownTextExtensions;

namespace UnitTests.MarkdownToHtmlTests;

[TestFixture]
public class ConvertToHmlTests
{
    public static IEnumerable<TestCaseData> MarkdownText_Headers()
    {
        yield return CreateTestCaseData("# Level1Header", "<h1>Level1Header</h1>").SetName("Level 1 Header Test");
        yield return CreateTestCaseData("## Level2Header", "<h2>Level2Header</h2>").SetName("Level 2 Header Test");
        yield return CreateTestCaseData("### Level3Header", "<h3>Level3Header</h3>").SetName("Level 3 Header Test");
        yield return CreateTestCaseData("#### Level4Header", "<h4>Level4Header</h4>").SetName("Level 4 Header Test");
        yield return CreateTestCaseData("##### Level5Header", "<h5>Level5Header</h5>").SetName("Level 5 Header Test");
        yield return CreateTestCaseData("###### Level6Header", "<h6>Level6Header</h6>").SetName("Level 6 Header Test");
    }

    [TestCaseSource(nameof(MarkdownText_Headers))]
    public void HeaderTests(string markdown, string expectedResult)
    {
        var html = markdown.ToHtml();
        Assert.That(html, Is.EqualTo(expectedResult));
    }

    public static IEnumerable<TestCaseData> MarkdownText_ParagraphsWithStyledText()
    {
        yield return CreateTestCaseData("aaa", "<p>aaa</p>").SetName("Paragraph text without styles");

        yield return CreateTestCaseData("**aaa**", "<p><strong>aaa</strong></p>").SetName("Paragraph with bold text");
        yield return CreateTestCaseData("*aaa*", "<p><em>aaa</em></p>").SetName("Paragraph with bold text");
        yield return CreateTestCaseData("***aaa***", "<p><strong><em>aaa</em></strong></p>").SetName("Paragraph with bold and italic text");

        yield return CreateTestCaseData("__aaa__", "<p><strong>aaa</strong></p>").SetName("Paragraph with bold text v2");
        yield return CreateTestCaseData("_aaa_", "<p><em>aaa</em></p>").SetName("Paragraph with bold text v2");
        yield return CreateTestCaseData("___aaa___", "<p><strong><em>aaa</em></strong></p>").SetName("Paragraph with bold and italic text v2");

        yield return CreateTestCaseData("**_aaa_**", "<p><strong><em>aaa</em></strong></p>").SetName("Paragraph with bold and italic text mixed");
        yield return CreateTestCaseData("__*aaa*__", "<p><strong><em>aaa</em></strong></p>").SetName("Paragraph with bold and italic text mixed");
    }

    [TestCaseSource(nameof(MarkdownText_ParagraphsWithStyledText))]
    public void StyledTextTests(string markdown, string expectedResult)
    {
        var  html = markdown.ToHtml();
        Assert.That(html, Is.EqualTo(expectedResult));
    }

    private static TestCaseData CreateTestCaseData(string markdown, string html)
    {
        return new TestCaseData(markdown, html);
    }
}

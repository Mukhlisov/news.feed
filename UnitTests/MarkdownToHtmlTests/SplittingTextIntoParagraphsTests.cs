using Tools.MarkdownTextExtensions;

namespace UnitTests.MarkdownToHtmlTests;

[TestFixture]
public class SplittingTextIntoParagraphsTests
{
    public static IEnumerable<TestCaseData> TextWithParagraphs_TestCases()
    {
        yield return CreateTestCaseData("abc", new[] { "abc" }).SetName("Проверка текста с одним параграфом");
        yield return CreateTestCaseData("abc\ndef", new[] { "abc\ndef" })
            .SetName("Проверка текста с одним параграфом, имеющим две строки");
        yield return CreateTestCaseData("abc\n\ndef", new[] { "abc", "def" })
            .SetName("Проверка текста с двумя параграфами");
        yield return CreateTestCaseData("abc\n\ndef\n\nghi", new[] { "abc", "def", "ghi" })
            .SetName("Проверка текста с тремя параграфами");
        yield return CreateTestCaseData("abc\n\n\ndef", new[] { "abc", "def" })
            .SetName("Проверка текста, в разделителе параграфов которого больше двух переносов");
        yield return CreateTestCaseData("abc\n \ndef", new[] { "abc", "def" })
            .SetName("Проверка текста, в разделителе параграфов которого есть пробелы");
        yield return CreateTestCaseData("abc\n\n  \n \ndef  \n\n  \nghi", new[] { "abc", "def", "ghi" })
            .SetName("Просто трудный кейс с переносами и пробелами");

        TestCaseData CreateTestCaseData(string testCase, IEnumerable<string> expectedResult)
        {
            return new TestCaseData(testCase, expectedResult);
        }
    }

    [TestCaseSource(nameof(TextWithParagraphs_TestCases))]
    public void CheckNormalSplitting(string markdown, string[] expectedResult)
    {
        var result = markdown.GetParagraphs();
        Assert.That(result, Is.EqualTo(expectedResult), "Полученные параграфы не совпадают с ожидаемым результатом");
    }
}
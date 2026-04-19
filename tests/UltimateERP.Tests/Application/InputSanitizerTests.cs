using UltimateERP.Application.Common.Validators;

namespace UltimateERP.Tests.Application;

public class InputSanitizerTests
{
    [Fact]
    public void Sanitize_NullInput_ReturnsEmpty()
    {
        var result = InputSanitizer.Sanitize(null);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Sanitize_EmptyString_ReturnsEmpty()
    {
        var result = InputSanitizer.Sanitize("");
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Sanitize_PlainText_HtmlEncodes()
    {
        var result = InputSanitizer.Sanitize("Hello World");
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Sanitize_ScriptTag_RemovesScript()
    {
        var result = InputSanitizer.Sanitize("<script>alert('xss')</script>Hello");
        Assert.DoesNotContain("script", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Hello", result);
    }

    [Fact]
    public void Sanitize_IframeTag_RemovesIframe()
    {
        var result = InputSanitizer.Sanitize("<iframe src='evil.com'></iframe>Safe text");
        Assert.DoesNotContain("iframe", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Safe text", result);
    }

    [Fact]
    public void ContainsDangerousContent_WithScript_ReturnsTrue()
    {
        Assert.True(InputSanitizer.ContainsDangerousContent("<script>alert(1)</script>"));
    }

    [Fact]
    public void ContainsDangerousContent_WithJavascript_ReturnsTrue()
    {
        Assert.True(InputSanitizer.ContainsDangerousContent("javascript:void(0)"));
    }

    [Fact]
    public void ContainsDangerousContent_WithEventHandler_ReturnsTrue()
    {
        Assert.True(InputSanitizer.ContainsDangerousContent("onload=\"alert(1)\""));
    }

    [Fact]
    public void ContainsDangerousContent_PlainText_ReturnsFalse()
    {
        Assert.False(InputSanitizer.ContainsDangerousContent("Hello, World! This is normal text."));
    }

    [Fact]
    public void ContainsDangerousContent_NullInput_ReturnsFalse()
    {
        Assert.False(InputSanitizer.ContainsDangerousContent(null));
    }

    [Fact]
    public void ContainsDangerousContent_EmptyInput_ReturnsFalse()
    {
        Assert.False(InputSanitizer.ContainsDangerousContent(""));
    }

    [Fact]
    public void Sanitize_EventHandlerAttribute_Removed()
    {
        var input = "<div onclick=\"alert('xss')\">content</div>";
        var result = InputSanitizer.Sanitize(input);
        Assert.DoesNotContain("onclick", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ContainsDangerousContent_WithEmbed_ReturnsTrue()
    {
        Assert.True(InputSanitizer.ContainsDangerousContent("<embed src='evil.swf'>"));
    }
}

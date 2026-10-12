using System.Net;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;
using Xunit;
using Xunit.Sdk;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.AssertionsTests;

public class ObjectAssertionsTests
{
    [Fact]
    public void Be_EqualValues_Passes()
    {
        HttpStatusCode.OK.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public void Be_DifferentValues_FailsWithExpressionAndValues()
    {
        var statusCode = HttpStatusCode.BadRequest;

        var exception = Assert.ThrowsAny<XunitException>(() => statusCode.Should().Be(HttpStatusCode.OK));

        Assert.Contains("statusCode", exception.Message);
        Assert.Contains("OK", exception.Message);
        Assert.Contains("BadRequest", exception.Message);
    }

    [Fact]
    public void Be_NullSubject_Fails()
    {
        string? subject = null;

        Assert.ThrowsAny<XunitException>(() => subject.Should().Be("value"));
    }

    [Fact]
    public void NotBe_DifferentValues_Passes()
    {
        Guid.NewGuid().Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void NotBe_EqualValues_Fails()
    {
        Assert.ThrowsAny<XunitException>(() => Guid.Empty.Should().NotBe(Guid.Empty));
    }
}

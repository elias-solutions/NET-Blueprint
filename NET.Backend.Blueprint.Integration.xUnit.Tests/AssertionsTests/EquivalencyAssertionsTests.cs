using NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;
using Xunit;
using Xunit.Sdk;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.AssertionsTests;

public class EquivalencyAssertionsTests
{
    private record Line(Guid Id, string Text);

    private record Order(Guid Id, string Customer, DateOnly Date, IEnumerable<Line> Lines);

    private class Problem
    {
        public string? Title { get; init; }
        public IDictionary<string, object?> Extensions { get; } = new Dictionary<string, object?>();
    }

    private static Order CreateOrder(string customer = "Joe", params Line[] lines) =>
        new(Guid.NewGuid(), customer, new DateOnly(2020, 5, 3), lines);

    [Fact]
    public void BeEquivalentTo_SameStructureDifferentInstances_Passes()
    {
        var line = new Line(Guid.NewGuid(), "a");
        var actual = new Order(Guid.Empty, "Joe", new DateOnly(2020, 5, 3), [line]);
        var expected = actual with { Lines = [line with { }] };

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void BeEquivalentTo_DifferentMember_FailsNamingThePath()
    {
        var actual = CreateOrder("Joe");
        var expected = actual with { Customer = "Jane" };

        var exception = Assert.ThrowsAny<XunitException>(() => actual.Should().BeEquivalentTo(expected));

        Assert.Contains("Customer", exception.Message);
        Assert.Contains("\"Jane\"", exception.Message);
        Assert.Contains("\"Joe\"", exception.Message);
    }

    [Fact]
    public void BeEquivalentTo_ExcludedMemberDiffers_Passes()
    {
        var actual = CreateOrder();
        var expected = actual with { Id = Guid.NewGuid() };

        actual.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public void BeEquivalentTo_NestedCollectionMemberDiffers_FailsNamingTheItemPath()
    {
        var actual = CreateOrder("Joe", new Line(Guid.NewGuid(), "a"));
        var expected = actual with { Lines = [new Line(Guid.NewGuid(), "a")] };

        var exception = Assert.ThrowsAny<XunitException>(() => actual.Should().BeEquivalentTo(expected));

        Assert.Contains("Lines[0].Id", exception.Message);
    }

    [Fact]
    public void BeEquivalentTo_ExcludedNestedCollectionMemberDiffers_Passes()
    {
        var actual = CreateOrder("Joe", new Line(Guid.NewGuid(), "a"));
        var expected = actual with { Id = Guid.NewGuid(), Lines = [new Line(Guid.NewGuid(), "a")] };

        actual.Should().BeEquivalentTo(expected, options => options
            .Excluding(x => x.Id)
            .For(x => x.Lines).Exclude(x => x.Id));
    }

    [Fact]
    public void BeEquivalentTo_CollectionInDifferentOrder_Passes()
    {
        var a = new Line(Guid.NewGuid(), "a");
        var b = new Line(Guid.NewGuid(), "b");

        CreateOrder("Joe", a, b).Should().BeEquivalentTo(CreateOrder("Joe", b, a), options => options.Excluding(x => x.Id));
    }

    [Fact]
    public void BeEquivalentTo_CollectionCountDiffers_Fails()
    {
        var a = new Line(Guid.NewGuid(), "a");

        var exception = Assert.ThrowsAny<XunitException>(() =>
            CreateOrder("Joe", a).Should().BeEquivalentTo(CreateOrder("Joe", a, a), options => options.Excluding(x => x.Id)));

        Assert.Contains("Lines", exception.Message);
    }

    [Fact]
    public void BeEquivalentTo_DictionaryWithExtraKey_Fails()
    {
        var actual = new Problem { Title = "t", Extensions = { ["traceId"] = "1" } };
        var expected = new Problem { Title = "t" };

        Assert.ThrowsAny<XunitException>(() => actual.Should().BeEquivalentTo(expected));
    }

    [Fact]
    public void BeEquivalentTo_NullSubject_Fails()
    {
        Order? actual = null;

        Assert.ThrowsAny<XunitException>(() => actual.Should().BeEquivalentTo(CreateOrder()));
    }

    [Fact]
    public void BeEquivalentTo_TopLevelCollectionWithElementExclusions_Passes()
    {
        IEnumerable<Order> actual = [CreateOrder("Joe", new Line(Guid.NewGuid(), "a"))];
        IEnumerable<Order> expected = [CreateOrder("Joe", new Line(Guid.NewGuid(), "a"))];

        actual.Should().BeEquivalentTo(expected, options => options
            .Excluding(x => x.Id)
            .For(x => x.Lines).Exclude(x => x.Id));
    }

    [Fact]
    public void BeEquivalentTo_TopLevelCollectionItemDiffers_Fails()
    {
        IEnumerable<Order> actual = [CreateOrder("Joe")];
        IEnumerable<Order> expected = [CreateOrder("Jane")];

        var exception = Assert.ThrowsAny<XunitException>(() =>
            actual.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.Id)));

        Assert.Contains("[0].Customer", exception.Message);
    }
}

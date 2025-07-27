using Mediary.Core.Results;
using Xunit;

namespace Mediary.Tests.Core;

public class ResultTypesTests
{
    [Fact]
    public void Unit_ShouldBeEqual()
    {
        Assert.Equal(Unit.Value, new Unit());
        Assert.True(Unit.Value == new Unit());
        Assert.False(Unit.Value != new Unit());
    }

    [Fact]
    public void Success_ShouldBeEqual()
    {
        Assert.Equal(Success.Value, new Success());
        Assert.True(Success.Value == new Success());
        Assert.False(Success.Value != new Success());
    }

    [Fact]
    public void Created_ShouldBeEqual()
    {
        Assert.Equal(Created.Value, new Created());
        Assert.True(Created.Value == new Created());
        Assert.False(Created.Value != new Created());
    }

    [Fact]
    public void Updated_ShouldBeEqual()
    {
        Assert.Equal(Updated.Value, new Updated());
        Assert.True(Updated.Value == new Updated());
        Assert.False(Updated.Value != new Updated());
    }

    [Fact]
    public void Deleted_ShouldBeEqual()
    {
        Assert.Equal(Deleted.Value, new Deleted());
        Assert.True(Deleted.Value == new Deleted());
        Assert.False(Deleted.Value != new Deleted());
    }

    [Fact]
    public void Result_StaticAccessors_ShouldReturnSingletons()
    {
        Assert.Equal(Unit.Value, Result.Unit);
        Assert.Equal(Success.Value, Result.Success);
        Assert.Equal(Created.Value, Result.Created);
        Assert.Equal(Updated.Value, Result.Updated);
        Assert.Equal(Deleted.Value, Result.Deleted);
    }

    [Theory]
    [InlineData("()", typeof(Unit))]
    [InlineData("Success", typeof(Success))]
    [InlineData("Created", typeof(Created))]
    [InlineData("Updated", typeof(Updated))]
    [InlineData("Deleted", typeof(Deleted))]
    public void ToString_ShouldReturnExpectedValue(string expected, Type type)
    {
        var value = Activator.CreateInstance(type);
        Assert.Equal(expected, value!.ToString());
    }

    [Theory]
    [InlineData(typeof(Unit))]
    [InlineData(typeof(Success))]
    [InlineData(typeof(Created))]
    [InlineData(typeof(Updated))]
    [InlineData(typeof(Deleted))]
    public void Equals_ShouldReturnTrue_WhenComparingSameType(Type type)
    {
        var value = Activator.CreateInstance(type);
        var other = Activator.CreateInstance(type);

        Assert.True(value!.Equals(other));
        Assert.True(value.Equals((object)other!));
    }

    [Theory]
    [InlineData(typeof(Unit))]
    [InlineData(typeof(Success))]
    [InlineData(typeof(Created))]
    [InlineData(typeof(Updated))]
    [InlineData(typeof(Deleted))]
    public void GetHashCode_ShouldReturnZero(Type type)
    {
        var value = Activator.CreateInstance(type);
        Assert.Equal(0, value!.GetHashCode());
    }
}

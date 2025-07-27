using Mediary.Core.Extensions;
using Mediary.Tests.Helpers;
using Xunit;

namespace Mediary.Tests.Dispatcher.Utils;

public class RequestInfoTests
{
    [Fact]
    public void GetDescription_ShouldReturnDescription_WhenAttributeIsPresent()
    {
        // Arrange
        var request = new SampleRequest();
        var expectedDescription = "Sample request";
        var expectedTags = new[] { "sample" };

        // Act
        var info = request.GetInfo();
        var description = request.GetDescription();
        var tags = request.GetTags();

        // Assert
        Assert.NotNull(info);
        Assert.Equal(expectedDescription, info.Description);
        Assert.Equal(expectedDescription, description);
        Assert.Equal(expectedTags, info.Tags);
        Assert.Equal(expectedTags, tags);
    }

    [Fact]
    public void GetDescription_ShouldReturnNull_WhenAttributeIsMissing()
    {
        // Arrange
        var request = new SampleRequestWithoutInfo();

        // Act
        var info = request.GetInfo();
        var description = request.GetDescription();
        var tags = request.GetTags();

        // Assert
        Assert.Null(info);
        Assert.Null(description);
        Assert.Empty(tags);
    }

    [Fact]
    public void GetDescription_ShouldReturnDescription_WhenOnlyDescriptionIsPresent()
    {
        // Arrange
        var request = new SampleRequestWithoutTags();
        var expectedDescription = "Sample request";

        // Act
        var info = request.GetInfo();
        var description = request.GetDescription();
        var tags = request.GetTags();

        // Assert
        Assert.NotNull(info);
        Assert.Equal(expectedDescription, info.Description);
        Assert.Equal(expectedDescription, description);
        Assert.Empty(tags);
    }
}

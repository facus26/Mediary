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

        // Act
        var description = request.GetDescription();
        var tags = request.GetTags();

        // Assert
        Assert.Equal("Sample request", description);
        Assert.Equal(["sample"], tags);
    }

    [Fact]
    public void GetDescription_ShouldReturnNull_WhenAttributeIsMissing()
    {
        // Arrange
        var request = new SampleRequestWithoutInfo();

        // Act
        var description = request.GetDescription();
        var tags = request.GetTags();

        // Assert
        Assert.Null(description);
        Assert.Empty(tags);
    }

    [Fact]
    public void GetDescription_ShouldReturnDescription_WhenOnlyDescriptionIsPresent()
    {
        // Arrange
        var request = new SampleRequestWithoutTags();

        // Act
        var description = request.GetDescription();
        var tags = request.GetTags();

        // Assert
        Assert.Equal("Sample request", description);
        Assert.Empty(tags);
    }
}

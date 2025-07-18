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
        Assert.Equal(new[] { "sample" }, tags);
    }

    [Fact]
    public void GetInfo_ShouldReturnAttributeInstance_WhenPresent()
    {
        // Arrange
        var request = new SampleRequest();

        // Act
        var info = request.GetInfo();

        // Assert
        Assert.NotNull(info);
        Assert.Equal("Sample request", info!.Description);
        Assert.Contains("sample", info.Tags);
    }
    [Fact]
    public void GetDescription_ShouldReturnDescription_WhenAttributeIsPresent_InRequestWithResponse()
    {
        // Arrange
        var request = new SampleRequestWithResponse();

        // Act
        var description = request.GetDescription();
        var tags = request.GetTags();

        // Assert
        Assert.Equal("Sample request with response", description);
        Assert.Equal(new[] { "sample" }, tags);
    }

    [Fact]
    public void GetInfo_ShouldReturnAttributeInstance_WhenPresent_InRequestWithResponse()
    {
        // Arrange
        var request = new SampleRequestWithResponse();

        // Act
        var info = request.GetInfo();

        // Assert
        Assert.NotNull(info);
        Assert.Equal("Sample request with response", info!.Description);
        Assert.Contains("sample", info.Tags);
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
}

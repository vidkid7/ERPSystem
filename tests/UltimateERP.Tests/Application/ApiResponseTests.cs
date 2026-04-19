using UltimateERP.Application.Common.Models;

namespace UltimateERP.Tests.Application;

public class ApiResponseTests
{
    [Fact]
    public void Success_ShouldReturnCorrectResponse()
    {
        var response = ApiResponse<string>.Success("hello", "OK", 1);

        Assert.True(response.IsSuccess);
        Assert.Equal("hello", response.Data);
        Assert.Equal("OK", response.ResponseMSG);
        Assert.Equal(1, response.TotalCount);
    }

    [Fact]
    public void Failure_ShouldReturnCorrectResponse()
    {
        var response = ApiResponse<string>.Failure("Something went wrong");

        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Something went wrong", response.ResponseMSG);
        Assert.Equal(0, response.TotalCount);
    }

    [Fact]
    public void PaginatedList_ShouldCalculatePages()
    {
        var items = new List<string> { "a", "b", "c" };
        var list = new PaginatedList<string>(items, 10, 2, 3);

        Assert.Equal(3, list.Items.Count);
        Assert.Equal(10, list.TotalCount);
        Assert.Equal(2, list.PageNumber);
        Assert.Equal(4, list.TotalPages);
        Assert.True(list.HasPreviousPage);
        Assert.True(list.HasNextPage);
    }

    [Fact]
    public void PaginatedList_FirstPage_ShouldNotHavePreviousPage()
    {
        var items = new List<string> { "a" };
        var list = new PaginatedList<string>(items, 5, 1, 5);

        Assert.False(list.HasPreviousPage);
        Assert.False(list.HasNextPage);
    }
}

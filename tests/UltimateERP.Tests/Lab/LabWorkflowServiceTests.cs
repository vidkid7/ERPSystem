using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using UltimateERP.Application.Interfaces;
using UltimateERP.Application.Services;
using UltimateERP.Domain.Entities.Lab;
using UltimateERP.Domain.Enums;
using Moq;

namespace UltimateERP.Tests.Lab;

public class LabWorkflowServiceTests
{
    private static (LabWorkflowService Service, Mock<IApplicationDbContext> DbMock) CreateService()
    {
        var dbMock = new Mock<IApplicationDbContext>();
        var service = new LabWorkflowService(dbMock.Object);
        return (service, dbMock);
    }

    private static DbSet<T> CreateMockDbSet<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        return mockSet.Object;
    }

    [Fact]
    public async Task ProcessSample_WhenCollected_ShouldMoveToInProcess()
    {
        var (service, dbMock) = CreateService();
        var sample = new SampleCollection
        {
            Id = 1,
            SampleNumber = "S001",
            CollectionDate = DateTime.UtcNow,
            Status = SampleCollectionStatus.Collected
        };

        dbMock.Setup(d => d.SampleCollections.FindAsync(new object[] { 1 }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sample);
        dbMock.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var (success, message) = await service.ProcessSample(1);

        Assert.True(success);
        Assert.Equal(SampleCollectionStatus.InProcess, sample.Status);
    }

    [Fact]
    public async Task ProcessSample_WhenNotCollected_ShouldFail()
    {
        var (service, dbMock) = CreateService();
        var sample = new SampleCollection
        {
            Id = 1,
            SampleNumber = "S001",
            CollectionDate = DateTime.UtcNow,
            Status = SampleCollectionStatus.Pending
        };

        dbMock.Setup(d => d.SampleCollections.FindAsync(new object[] { 1 }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sample);

        var (success, message) = await service.ProcessSample(1);

        Assert.False(success);
        Assert.Contains("Collected", message);
    }

    [Fact]
    public async Task ProcessSample_WhenNotFound_ShouldFail()
    {
        var (service, dbMock) = CreateService();
        dbMock.Setup(d => d.SampleCollections.FindAsync(new object[] { 99 }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SampleCollection?)null);

        var (success, message) = await service.ProcessSample(99);

        Assert.False(success);
        Assert.Contains("not found", message);
    }

    [Fact]
    public async Task CompleteReport_WhenPending_ShouldMarkCompleted()
    {
        var (service, dbMock) = CreateService();
        var report = new LabReport
        {
            Id = 1,
            SampleCollectionId = 1,
            ReportDate = DateTime.UtcNow,
            Status = LabReportStatus.Pending,
            SampleCollection = new SampleCollection
            {
                Id = 1, SampleNumber = "S001", CollectionDate = DateTime.UtcNow,
                Status = SampleCollectionStatus.InProcess
            }
        };

        var reports = new List<LabReport> { report };
        var mockSet = new Mock<DbSet<LabReport>>();
        mockSet.As<IQueryable<LabReport>>().Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<LabReport>(reports.AsQueryable().Provider));
        mockSet.As<IQueryable<LabReport>>().Setup(m => m.Expression).Returns(reports.AsQueryable().Expression);
        mockSet.As<IQueryable<LabReport>>().Setup(m => m.ElementType).Returns(reports.AsQueryable().ElementType);
        mockSet.As<IQueryable<LabReport>>().Setup(m => m.GetEnumerator()).Returns(reports.AsQueryable().GetEnumerator());
        mockSet.As<IAsyncEnumerable<LabReport>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<LabReport>(reports.GetEnumerator()));

        dbMock.Setup(d => d.LabReports).Returns(mockSet.Object);
        dbMock.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var (success, message) = await service.CompleteReport(1, "Test results data");

        Assert.True(success);
        Assert.Equal(LabReportStatus.Completed, report.Status);
        Assert.Equal("Test results data", report.ReportData);
    }

    [Fact]
    public async Task ValidateReport_WhenCompleted_ShouldMarkValidated()
    {
        var (service, dbMock) = CreateService();
        var report = new LabReport
        {
            Id = 1,
            SampleCollectionId = 1,
            ReportDate = DateTime.UtcNow,
            Status = LabReportStatus.Completed
        };

        dbMock.Setup(d => d.LabReports.FindAsync(new object[] { 1 }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(report);
        dbMock.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var (success, message) = await service.ValidateReport(1, 42);

        Assert.True(success);
        Assert.Equal(LabReportStatus.Validated, report.Status);
        Assert.Equal(42, report.ValidatedBy);
    }

    [Fact]
    public async Task ValidateReport_WhenPending_ShouldFail()
    {
        var (service, dbMock) = CreateService();
        var report = new LabReport
        {
            Id = 1,
            SampleCollectionId = 1,
            ReportDate = DateTime.UtcNow,
            Status = LabReportStatus.Pending
        };

        dbMock.Setup(d => d.LabReports.FindAsync(new object[] { 1 }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(report);

        var (success, message) = await service.ValidateReport(1, 42);

        Assert.False(success);
        Assert.Contains("Completed", message);
    }

    [Fact]
    public async Task ValidateReport_WhenNotFound_ShouldFail()
    {
        var (service, dbMock) = CreateService();
        dbMock.Setup(d => d.LabReports.FindAsync(new object[] { 99 }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LabReport?)null);

        var (success, message) = await service.ValidateReport(99, 42);

        Assert.False(success);
        Assert.Contains("not found", message);
    }
}

internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;
    internal TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;
    public IQueryable CreateQuery(System.Linq.Expressions.Expression expression) => new TestAsyncEnumerable<TEntity>(expression);
    public IQueryable<TElement> CreateQuery<TElement>(System.Linq.Expressions.Expression expression) => new TestAsyncEnumerable<TElement>(expression);
    public object? Execute(System.Linq.Expressions.Expression expression) => _inner.Execute(expression);
    public TResult Execute<TResult>(System.Linq.Expressions.Expression expression) => _inner.Execute<TResult>(expression);
    public TResult ExecuteAsync<TResult>(System.Linq.Expressions.Expression expression, CancellationToken ct = default)
    {
        var resultType = typeof(TResult).GetGenericArguments()[0];
        var execMethod = typeof(IQueryProvider).GetMethod(nameof(IQueryProvider.Execute), 1, new[] { typeof(System.Linq.Expressions.Expression) })!;
        var result = execMethod.MakeGenericMethod(resultType).Invoke(_inner, new object[] { expression });
        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!.MakeGenericMethod(resultType).Invoke(null, new[] { result })!;
    }
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(System.Linq.Expressions.Expression expression) : base(expression) { }
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken ct = default) =>
        new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(((IQueryable)this).Provider);
}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;
    public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;
    public T Current => _inner.Current;
    public ValueTask DisposeAsync() { _inner.Dispose(); return ValueTask.CompletedTask; }
    public ValueTask<bool> MoveNextAsync() => new(_inner.MoveNext());
}

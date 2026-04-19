using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Jobs;

public class ScheduledJobConfig : BaseEntity
{
    public string JobName { get; set; } = string.Empty;
    public ScheduledJobType JobType { get; set; }
    public string? Schedule { get; set; }
    public DateTime? LastRunDate { get; set; }
    public JobRunStatus? LastRunStatus { get; set; }
    public DateTime? NextRunDate { get; set; }
    public string? ErrorMessage { get; set; }

    public ICollection<JobExecutionLog> ExecutionLogs { get; set; } = new List<JobExecutionLog>();
}

public class JobExecutionLog : BaseEntity
{
    public int JobConfigId { get; set; }
    public ScheduledJobConfig JobConfig { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public JobRunStatus Status { get; set; }
    public int RecordsProcessed { get; set; }
    public string? ErrorMessage { get; set; }
}

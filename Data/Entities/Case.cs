namespace MinimalApis.Data.Entities;

public enum CaseStatus
{
  Invalid = 0,
  Open,
  Closed,
  Rejected,
  Referred,
  Scheduled,
  Settled
}

public class Case
{
  public int Id { get; set; }
  public string FileNumber { get; set; } = "";
  public CaseStatus Status { get; set; }

  public Client? Client {get;set;}
}
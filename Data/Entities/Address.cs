﻿namespace MinimalApis.Data.Entities;

public class Address
{
  public int Id { get; set; } = 0;
  public string? Address1 { get; set; }
  public string? Address2 { get; set; }
  public string? Address3 { get; set; }
  public string? CityTown { get; set; }
  public string? StateProvince { get; set; }
  public string? PostalCode { get; set; }
  public string? Country { get; set; }
}

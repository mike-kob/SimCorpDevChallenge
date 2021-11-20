using System;

namespace SC.DevChallenge.Api.Models
{
  public class PriceValues
  {
    public int Id {get; set;}
    public string Portfolio {get; set;}
    public string Owner {get; set;}
    public string Instrument {get; set;}
    public DateTime Date {get; set;}
    public decimal Price {get; set;}
  }
}
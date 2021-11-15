using System;

namespace SC.DevChallenge.Api.Models
{
  public class PriceValues
  {
    public string Portfolio {get; set;}
    public string Owner {get; set;}
    public string Instrument {get; set;}
    public DateTime Date {get; set;}
    public decimal Price {get; set;}

    public static PriceValues FromCsv(string csvLine)
    {
      string[] values = csvLine.Split(',');
      PriceValues dailyValues = new PriceValues();
      dailyValues.Portfolio = Convert.ToString(values[0]).ToLower();
      dailyValues.Owner = Convert.ToString(values[1]).ToLower();
      dailyValues.Instrument = Convert.ToString(values[2]).ToLower();
      dailyValues.Date = DateTime.ParseExact(values[3], "dd/MM/yyyy HH:mm:ss", null);
      dailyValues.Price = Convert.ToDecimal(values[4]);
      return dailyValues;
    }
  }
}
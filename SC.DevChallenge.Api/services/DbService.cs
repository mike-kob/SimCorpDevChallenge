using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using SC.DevChallenge.Api.Models;


namespace SC.DevChallenge.Api.Services
{
  public class DbService : BaseDbService
  {
    public DbService()
    {
      values = File.ReadAllLines("./Input/data.csv")
                                      .Skip(1)
                                      .Select(v => PriceValues.FromCsv(v))
                                      .ToList();
    }
  }
}
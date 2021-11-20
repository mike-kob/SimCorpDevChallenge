using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using SC.DevChallenge.Api.Models;
using SC.DevChallenge.Api.Contexts;

namespace SC.DevChallenge.Api.Services
{
  public class PriceService : BasePriceService
  {
    private readonly IDbContext _dbContext;
    public PriceService(IDbContext dbContext)
    {
      _dbContext = dbContext;
      values = dbContext.PriceSet.ToList();
    }
  }
}
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using SC.DevChallenge.Api.Models;

namespace SC.DevChallenge.Api.Services
{
  public class MockDbService : BaseDbService
  {
    public MockDbService(List<PriceValues> _values)
    {
      values = _values;
    }
  }
}

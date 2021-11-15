using System;
using Xunit;

using System.Collections.Generic;

using SC.DevChallenge.Api.Models;
using SC.DevChallenge.Api.Controllers;
using SC.DevChallenge.Api.Services;
using SC.DevChallenge.Api.Utils;

namespace SC.DevChallenge.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
          var vals = new List<PriceValues> () {
            new PriceValues () {Portfolio="Fannie Mae", Owner="SimCorp", Instrument="Equity", Date=new DateTime(2018, 4, 21, 12, 11, 45), Price=150.99m}
          };
          MockDbService dbService = new MockDbService(vals);
          
          var res = dbService.GetFilteredAvg("SimCorp", "Fannie Mae", "Equity", TimeslotManager.DtToTs(new DateTime(2018, 4, 21, 12, 11, 45)));
          Assert.Equal(res, 150.99m);
        }
    }
}

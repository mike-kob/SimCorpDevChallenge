using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;

namespace SC.DevChallenge.Api.Utils
{
  public static class StatisticsManager
  {
    public static decimal GetBenchmark(decimal[] sortedData)
    {
      var n = sortedData.Length;

      if (n == 1)
        return sortedData[0];

      var qLower = n > 3 ? (int)Math.Ceiling((n - 1) / 4.0) : 0;
      var qUpper = n > 3 ? (int)Math.Ceiling((3 * n - 3) / 4.0) : n - 1;
      var iqr = sortedData[qUpper] - sortedData[qLower];

      var average = sortedData.Where((x) =>
        x < sortedData[qUpper] + 1.5m * iqr
        && x > sortedData[qLower] - 1.5m * iqr
      ).Average();

      return average;
    }
  }

}
using System;

namespace SC.DevChallenge.Api.Utils
{
  public static class TimeslotManager
  {
    private static DateTime intervalStart = new DateTime(2018, 1, 1);
    private static int intervalSeconds = 10000;
    public static int DtToTs(DateTime dt) {
      var s = dt.Subtract(intervalStart).TotalSeconds;
      return (int) Math.Floor(s / intervalSeconds);
    }

    public static DateTime TsToDt(int ts) {
      return intervalStart.AddSeconds(intervalSeconds  * ts);
    }
  }

}
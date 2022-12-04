using System.Diagnostics;
using Util;
using AOC;

Stopwatch sw = new Stopwatch();
sw.Start();
int overlapCnt = 0;
int containCnt = 0;
await foreach (var str in FileUtil.ReadFileLineAsync("./data.txt"))
{
    if (Day4.isContained(str))
    {
        containCnt++;
    }
    if (Day4.isOverlap(str))
    {
        overlapCnt++;
    }
}
Console.WriteLine($"Contained Count: {containCnt}, Overlap Count: {overlapCnt}");
sw.Stop();
Console.WriteLine($"Ellapsed time {sw.ElapsedMilliseconds} ms");

namespace AOC
{
    public static class Day4
    {
        //e.g. 17-75,14-75
        public static bool isContained(string s)
        {
            var pairArr = s.Split(",");
            int[] pairOne = pairArr[0].Split("-").Select(str => Convert.ToInt32(str)).ToArray();
            int[] pairTwo = pairArr[1].Split("-").Select(str => Convert.ToInt32(str)).ToArray();

            if ((pairOne[0] >= pairTwo[0] && pairOne[1] <= pairTwo[1]) || (pairTwo[0] >= pairOne[0] && pairTwo[1] <= pairOne[1]))
            {
                return true;
            }
            return false;
        }
        //e.g. 17-75,14-75

        /*
            ---------
                        -----------

                        -----------
            ------
            sort by start

            ----------
            ------

            ------
               ---------

            ---------
                ----
        */
        public static bool isOverlap(string s)
        {
            var pairArr = s.Split(",");
            int[] pairOne = pairArr[0].Split("-").Select(str => Convert.ToInt32(str)).ToArray();
            int[] pairTwo = pairArr[1].Split("-").Select(str => Convert.ToInt32(str)).ToArray();

            if (pairOne[0] <= pairTwo[0])
            {
                return isOverlapRange(pairOne, pairTwo);
            }
            return isOverlapRange(pairTwo, pairOne);
        }

        /*
        pair one's start comes before or is equal to pairTwos start
        we just have to check whether pairTwo's start is <= pairOne's end
        */
        public static bool isOverlapRange(int[] pairOne, int[] pairTwo)
        {
            if (pairTwo[0] <= pairOne[1])
            {
                return true;
            }
            return false;
        }


    }
}

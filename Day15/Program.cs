using System.Diagnostics;

/*
dist = sum of abs(diff in both co-ordinates)
*/

/*

part1 - count of positions (for a specific y) where a beacon cannot be along a single row
*/
using System.Text.RegularExpressions;

using Util;

// List<string> inputList = FileUtil.ReadFile("./testdata.txt");
List<string> inputList = FileUtil.ReadFile("./data.txt");
Regex rx = new Regex(@"x=([-]?\d+),\s*y=([-]?\d+)");
List<SBPair> pairList = new();
foreach (string input in inputList) {
    var matches = rx.Matches(input);
    (int, int) sensor = new();
    sensor.Item1 = Convert.ToInt32(matches[0].Groups[1].Value);
    sensor.Item2 = Convert.ToInt32(matches[0].Groups[2].Value);
    (int, int) beacon = new();
    beacon.Item1 = Convert.ToInt32(matches[1].Groups[1].Value);
    beacon.Item2 = Convert.ToInt32(matches[1].Groups[2].Value);
    // Console.WriteLine(sensor.Item1 + ":" + sensor.Item2 + " | " + beacon.Item1 + ":" + beacon.Item2);
    pairList.Add(new SBPair { sPoint = sensor, bPoint = beacon });
}

// Stopwatch sw = new Stopwatch();
// sw.Start();
// Console.WriteLine(part1(pairList, 0, 4000000));
// sw.Stop();
// Console.WriteLine($"Elapsed ms: {sw.ElapsedMilliseconds}");


Stopwatch sw = new Stopwatch();
sw.Start();
// Console.WriteLine(part2(pairList, 20));
Console.WriteLine(part2(pairList, 4000000));
sw.Stop();
Console.WriteLine($"Elapsed ms: {sw.ElapsedMilliseconds}");
/*

e.g. 
8,7 | 2,10
mdist = 6+3 = 9
vertical expansion for this sensor
8,7 - 0,9 = 8,-2
8,7 + 0,9 = 8, 16

check whether 8,10 falls on 8,-2 | 8,16 - yes
ydist = -10
ydist = -6

total contribution= 1+(n)*2 = 1+(6)2 = 1+10 = 11
this is a horizontal line on 11/2 on the left of 8,10 and 11/2 or the right of 8,10
foreach pair
    find distance
        
*/
int part1(List<SBPair> pairList, int yTarget, int maxCo) {
    //set containing points on the target row
    HashSet<(int, int)> set = new();
    HashSet<(int, int)> beaconSet = new();
    foreach (var pair in pairList) {
        beaconSet.Add(pair.bPoint);
    }
    foreach (var sbPair in pairList) {
        process(sbPair, yTarget, ref set, beaconSet);
    }
    var arr = set.ToArray();
    Array.Sort(arr);
    var toparr = arr;
    foreach (var a in toparr) {
        Console.WriteLine(a);
    }
    return set.Count;

}
int part2(List<SBPair> pairList, int maxCo) {
    HashSet<(int, int)> beaconSet = new();
    foreach (var pair in pairList) {
        beaconSet.Add(pair.bPoint);
    }
    for (int i = 0; i < maxCo; i++) {
        List<(int, int)> rangeList = new();
        foreach (var sbPair in pairList) {
            processRange(sbPair, i, ref rangeList, beaconSet);
        }
        //sort the list by first point
        //merge the list
        List<(int, int)> mergedList = sortAndMergeList(rangeList);
        if (mergedList.Count > 1) {
            Console.WriteLine($"Row: {i}");
            foreach (var range in mergedList) {
                Console.WriteLine("Range:" + range);
            }
        }
    }

    return -1;
}

List<(int, int)> sortAndMergeList(List<(int, int)> rangeList) {
    rangeList.Sort(Comparer<(int, int)>.Create((a, b) => {
        return a.CompareTo(b);
    }));
    //merge sorted range
    /*
        -------------
            ------
              --------------

    */
    List<(int, int)> result = new List<(int, int)>();
    result.Add(rangeList[0]);
    for (int i = 1; i < rangeList.Count; i++) {
        var prevRange = result[result.Count - 1];
        var currRange = rangeList[i];
        if (currRange.Item1 <= prevRange.Item2) {
            prevRange.Item2 = Math.Max(prevRange.Item2, currRange.Item2);
            result[result.Count - 1] = prevRange;
        } else {
            result.Add(currRange);
        }
    }
    return result;
}

void process(SBPair sbPair, int yTarget, ref HashSet<(int, int)> set, HashSet<(int, int)> beaconSet) {
    int mDist = getDist(sbPair.sPoint, sbPair.bPoint);
    //vertical expansion
    var upPoint = addPoint(sbPair.sPoint, (0, mDist), true);
    var lowPoint = addPoint(sbPair.sPoint, (0, mDist), false);
    // Console.WriteLine($"Sensor: {sbPair.sPoint} Dist: {mDist} Up: {upPoint} Low: {lowPoint}");

    //check if yTarget,sBeacon.X lies on upPoint - lowPoint
    // dist between upPoint, lowPoint = dist between upPoint, targetPoint + dist between targetPoint, lowPoint
    var targetPoint = (sbPair.sPoint.Item1, yTarget);
    int upDist = getDist(upPoint, targetPoint);
    int lowDist = getDist(targetPoint, lowPoint);
    if (getDist(upPoint, lowPoint) != (upDist + lowDist)) {
        return;
    }

    // Console.WriteLine($"Target: {targetPoint}");

    //cover length is going to be minimum of upDist and lowDist
    int distFromTarget = upDist < lowDist ? upDist : lowDist;
    int cont = 1 + ((distFromTarget) * 2);

    //add points on yTarget horizontal line
    if (!beaconSet.Contains(targetPoint))
        set.Add(targetPoint);

    for (int i = 1; i <= (cont / 2); i++) {
        (int, int) newPoint = (targetPoint.Item1 - i, targetPoint.Item2);
        if (!beaconSet.Contains(newPoint)) {
            // Console.WriteLine($"Add Point: {newPoint.Item1}: {newPoint.Item2}");
            set.Add(newPoint);
        }

        (int, int) anotherPoint = (targetPoint.Item1 + i, targetPoint.Item2);
        if (!beaconSet.Contains(anotherPoint)) {
            // Console.WriteLine($"Add Point: {anotherPoint.Item1}: {anotherPoint.Item2}");
            set.Add(anotherPoint);
        }
    }
}

void processRange(SBPair sbPair, int yTarget, ref List<(int, int)> rangeList, HashSet<(int, int)> beaconSet) {

    int mDist = getDist(sbPair.sPoint, sbPair.bPoint);
    //vertical expansion
    var upPoint = addPoint(sbPair.sPoint, (0, mDist), true);
    var lowPoint = addPoint(sbPair.sPoint, (0, mDist), false);
    // Console.WriteLine($"Sensor: {sbPair.sPoint} Dist: {mDist} Up: {upPoint} Low: {lowPoint}");

    //check if yTarget,sBeacon.X lies on upPoint - lowPoint
    // dist between upPoint, lowPoint = dist between upPoint, targetPoint + dist between targetPoint, lowPoint
    var targetPoint = (sbPair.sPoint.Item1, yTarget);
    int upDist = getDist(upPoint, targetPoint);
    int lowDist = getDist(targetPoint, lowPoint);
    if (getDist(upPoint, lowPoint) != (upDist + lowDist)) {
        return;
    }

    //cover length is going to be minimum of upDist and lowDist
    int distFromTarget = upDist < lowDist ? upDist : lowDist;
    int cont = 1 + ((distFromTarget) * 2);

    int leftPt = targetPoint.Item1 - (cont / 2);
    int rightPt = targetPoint.Item1 + (cont / 2);
    rangeList.Add((leftPt, rightPt));
}

(int, int) addPoint((int, int) p1, (int, int) p2, bool isSubtract) {
    if (isSubtract) {
        return (p1.Item1 - p2.Item1, p1.Item2 - p2.Item2);
    }
    return (p1.Item1 + p2.Item1, p1.Item2 + p2.Item2);
}

int getDist((int, int) p1, (int, int) p2) {
    return Math.Abs(p1.Item1 - p2.Item1) + Math.Abs(p1.Item2 - p2.Item2);
}
public class SBPair {
    public (int, int) sPoint { get; set; }
    public (int, int) bPoint { get; set; }

}
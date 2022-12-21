using System.Text;
using System.Text.RegularExpressions;

using Util;

List<string> inputList = FileUtil.ReadFile("./data.txt");

Regex rx = new Regex(@"Valve (\w{2}) has flow rate=(\d*); tunnel[s]* lead[s]* to valve[s]* ([\w{2}[,\s]*)");
List<int> valveRate = new List<int>();
Dictionary<string, int> idxMap = new();
Dictionary<int, string> revIdxMap = new();
Dictionary<string, string[]> adjList = new Dictionary<string, string[]>();
int idx = 0;

foreach (string input in inputList) {
    Match match = rx.Match(input);

    string valveName = match.Groups[1].Value;
    int rate = Convert.ToInt32(match.Groups[2].Value);
    string[] nextValveArr = match.Groups[3].Value.Trim().Split(",");

    idxMap.Add(valveName, idx);
    revIdxMap.Add(idx, valveName);
    valveRate.Add(rate);
    adjList.Add(valveName, nextValveArr);

    idx++;
}

//we want to do floyd warshall 
// create adj matrix
int?[,] adjM = new int?[idx, idx];
foreach (var key in adjList.Keys) {
    foreach (var val in adjList[key]) {
        adjM[idxMap[key], idxMap[val.Trim()]] = 1;
    }
}

allPairShortestPath(adjM);
HashSet<int> visited = new HashSet<int>();
List<string> debug = new List<string>();
Console.WriteLine(DFS(0, 30, debug));
// for (int i = 0; i < idx; i++) {
//     Console.WriteLine();
//     for (int j = 0; j < idx; j++) {
//         if (adjM[i, j] == null) {
//             Console.Write("\t");
//             continue;
//         }
//         Console.Write(adjM[i, j] + "\t");
//     }
// }

//DFS starting from 0 index, neighbors in this case are all neighbors that have a non-zero and non-inf dist. also the flowrate != 0
// we want to maximize sum of steps*mins

int DFS(int idx, int timeLeft, List<string> debug) {
    //exit condition 
    if (visited.Contains(idx) || timeLeft == 0) {
        return 0;
    }

    Console.WriteLine();
    Console.WriteLine($"timeLeft -  {timeLeft}");

    visited.Add(idx);
    debug.Add(revIdxMap[idx]);

    //recurse
    //for each neighbor
    int maxGain = int.MinValue;
    for (int j = 0; j < adjM.GetLength(0); j++) {
        int flowRate = valveRate[j];
        if ((idx == j) || (visited.Contains(j)) || adjM[idx, j] == null || adjM[idx, j] == 0 || flowRate == 0) {
            continue;
        }

        int timeSpent = (int)adjM[idx, j]! + 1;
        if (timeLeft - timeSpent <= 0) {
            continue;
        }
        Console.WriteLine(getDebug() + "\t" + revIdxMap[j]);
        Console.WriteLine($"Pressure Released: {timeLeft} - {timeSpent} * {flowRate} = {(timeLeft - timeSpent) * flowRate}");
        int gain = ((timeLeft - timeSpent) * flowRate) + DFS(j, timeLeft - timeSpent, debug);
        maxGain = Math.Max(maxGain, gain);
    }

    visited.Remove(idx);
    debug.RemoveAt(debug.Count - 1);

    int result = maxGain != int.MinValue ? maxGain : 0;
    Console.WriteLine($"MaxGain for {getDebug()}\t{revIdxMap[idx]} is {result}");
    return result;
}

string getDebug() {
    StringBuilder sb = new();
    foreach (string str in debug) {
        if (sb.ToString() == "") {
            sb.Append(str);
            continue;
        }
        sb.Append("\t" + str);
    }
    return sb.ToString();
}
//floyd warshall
void allPairShortestPath(int?[,] arr) {
    int n = arr.GetLength(0);

    // set arr[i,i] = 1
    for (int i = 0; i < n; i++) {
        arr[i, i] = 0;
    }

    for (int k = 0; k < n; k++) {
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {

                if (arr[i, k] == null || arr[k, j] == null) {
                    continue;
                }
                if (arr[i, j] == null || (arr[i, k] + arr[k, j] < arr[i, j])) {
                    arr[i, j] = arr[i, k] + arr[k, j];
                }
            }
        }
    }
}

using System.ComponentModel;
using System.Text.RegularExpressions;

using Util;

List<string> inputList = FileUtil.ReadFile("./data.txt");

/*
    what is the most pressure you could release?
    valves and tunnels
    1 min to open a valve
    1 min to follow any tunnel from 1 valve to another
*/
Regex rx = new Regex(@"Valve (\w{2}) has flow rate=(\d*); tunnel[s]* lead[s]* to valve[s]* ([\w{2}[,\s]*)");
Dictionary<string, int> flowMap = new();
Dictionary<string, List<string>> adjList = new();
Dictionary<string, int> idxMap = new();
int i = 0;
var startValve = "";
foreach (string input in inputList) {
    Match match = rx.Match(input);
    var valveStr = match.Groups[1].Value;
    if (startValve == "") {
        startValve = valveStr;
    }
    flowMap[valveStr] = Convert.ToInt32(match.Groups[2].Value);

    //tunnel to valve map
    if (!adjList.ContainsKey(valveStr)) {
        adjList[valveStr] = new List<string>();
    }
    var valveMapStr = match.Groups[3].Value;
    var valveArr = valveMapStr.Split(",");
    foreach (string valve in valveArr) {
        string tempValve = valve.Trim();

        adjList[valveStr].Add(tempValve);
    }

    idxMap.Add(valveStr, i++);
}

int?[,] adjMatrix = new int?[inputList.Count, inputList.Count];
int[] flowIdxMap = new int[inputList.Count];
foreach (var key in adjList.Keys) {
    foreach (var val in adjList[key]) {
        adjMatrix[idxMap[key], idxMap[val]] = 1;
    }
    flowIdxMap[idxMap[key]] = flowMap[key];
}

Console.WriteLine(part1(idxMap, adjMatrix, flowIdxMap));

/*
*/
int part1(Dictionary<string, int> idMap, int?[,] adjMatrix, int[] flowIdxMap) {
    //find all pair shortest path
    floydWarshall(adjMatrix);
    // for (int i = 0; i < adjMatrix.GetLength(0); i++) {
    //     for (int j = 0; j < adjMatrix.GetLength(1); j++) {
    //         if (adjMatrix[i, j] == null) {
    //             Console.Write("z\t");
    //             continue;
    //         }
    //         Console.Write($"{adjMatrix[i, j]}\t");
    //     }
    //     Console.WriteLine();
    // }

    //find max product using dfs
    HashSet<int> visited = new();
    List<int> debug = new List<int>();
    return solveRec(adjMatrix, flowIdxMap, 0, 30, visited, debug);
}

int solveRec(int?[,] adjMatrix, int[] flowIdxMap, int currNode, int timeLeft, HashSet<int> visited, List<int> debug) {
    visited.Add(currNode);
    debug.Add(currNode);


    //recurse
    //for every other node 
    int product = 0;
    int maxProduct = int.MinValue;
    int nextNode = -1;
    for (int i = 0; i < adjMatrix.GetLength(0); i++) {
        //if no path then skip
        //if flow rate is 0 no need to got there
        if (i == currNode || adjMatrix[currNode, i] == null || flowIdxMap[i] == 0) {
            continue;
        }
        //exit condition
        if (visited.Contains(i)) {
            continue;
        }

        int tempTimeLeft = timeLeft - (int)adjMatrix[currNode, i] - 1;  //additional 1 for opening
        if (tempTimeLeft <= 0) {
            continue;
        }

        product = (tempTimeLeft * flowIdxMap[i]) + solveRec(adjMatrix, flowIdxMap, i, tempTimeLeft, visited, debug);
        if (product > maxProduct) {
            maxProduct = product;
            nextNode = i;

        }
    }

    visited.Remove(currNode);
    debug.RemoveAt(debug.Count - 1);
    Console.WriteLine();
    foreach (var node in debug) {
        Console.Write($"{node}\t");
    }
    Console.Write(currNode);
    Console.WriteLine();
    Console.WriteLine($"time left : {timeLeft}");
    if (nextNode != -1)
        Console.WriteLine($"{maxProduct}, next node taken {nextNode} at dist {adjMatrix[currNode, nextNode]}");
    else
        Console.WriteLine($"{maxProduct}, no next node");
    return maxProduct != int.MinValue ? maxProduct : 0;
}

void floydWarshall(int?[,] adjMatrix) {
    int cnt = adjMatrix.GetLength(0);
    for (int i = 0; i < cnt; i++) {
        adjMatrix[i, i] = 0;
    }

    for (int k = 0; k < cnt; k++) {
        for (int i = 0; i < cnt; i++) {
            for (int j = 0; j < cnt; j++) {
                if (adjMatrix[i, k] == null || adjMatrix[k, j] == null) {
                    continue;
                }

                if (adjMatrix[i, j] == null || adjMatrix[i, k] + adjMatrix[k, j] < adjMatrix[i, j]) {
                    adjMatrix[i, j] = adjMatrix[i, k] + adjMatrix[k, j];
                }
            }
        }
    }
}

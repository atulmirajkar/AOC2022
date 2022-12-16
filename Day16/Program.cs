
using System.ComponentModel;
using System.Text.RegularExpressions;

using Util;

List<string> inputList = FileUtil.ReadFile("./testdata.txt");

/*
    what is the most pressure you could release?
    valves and tunnels
    1 min to open a valve
    1 min to follow any tunnel from 1 valve to another
*/
Regex rx = new Regex(@"Valve (\w{2}) has flow rate=(\d*); tunnel[s]* lead[s]* to valve[s]* ([\w{2}[,\s]*)");
Dictionary<string, int> frMap = new();
Dictionary<string, List<string>> tvMap = new();
var startValve = "";
foreach (string input in inputList) {
    Match match = rx.Match(input);
    var valveStr = match.Groups[1].Value;
    if (startValve == "") {
        startValve = valveStr;
    }
    frMap[valveStr] = Convert.ToInt32(match.Groups[2].Value);

    //tunnel to valve map
    if (!tvMap.ContainsKey(valveStr)) {
        tvMap[valveStr] = new List<string>();
    }
    var valveMapStr = match.Groups[3].Value;
    var valveArr = valveMapStr.Split(",");
    foreach (string valve in valveArr) {
        string tempValve = valve.Trim();

        tvMap[valveStr].Add(tempValve);
    }
}
Console.WriteLine(part1(startValve, frMap, tvMap));

/*
flowRate(valve, timeLeft)
    if(timeLeft <= 0) 
        return 0;
    
    foreach valve 
        tempTime
        flowRate()
        tempTime
*/
int part1(string startValve, Dictionary<string, int> frMap, Dictionary<string, List<string>> tvMap) {
    int step = 1;
    HashSet<string> visited = new();
    return frRec(startValve, 30, frMap, tvMap, visited, step);
}

int frRec(string valve, int timeLeft, Dictionary<string, int> frMap, Dictionary<string, List<string>> tvMap, HashSet<string> visited, int step) {
    //exit condition
    if (timeLeft <= 0) {
        return 0;
    }
    Console.WriteLine($"At Valve:{valve}, Time left: {timeLeft}");
    //recurse
    //take it
    int maxFlow = int.MinValue;
    if (!visited.Contains(valve)) {
        Console.WriteLine($"{step} - Take : {valve} - {frMap[valve]}");
        visited.Add(valve);
        int takeFlow = frRec(valve, timeLeft - 1, frMap, tvMap, visited, step + 1) + frMap[valve];
        maxFlow = Math.Max(maxFlow, takeFlow);
        visited.Remove(valve);
    }

    //try the rest
    foreach (string nextValve in tvMap[valve]) {
        if (!visited.Contains(nextValve)) {
            Console.WriteLine($"{step} - Take : {nextValve} - {frMap[nextValve]}");
            visited.Add(nextValve);
            int flow = frRec(nextValve, timeLeft - 2, frMap, tvMap, visited, step + 1) + frMap[nextValve];
            maxFlow = Math.Max(maxFlow, flow);
            visited.Remove(nextValve);
        }
    }

    return maxFlow != int.MinValue ? maxFlow : 0;
}
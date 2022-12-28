using Util;
using System.Collections.Generic;


//you have 1 ore collecting robot   
//it takes 1 minute to construct a robot
//each robot can collect 1 of its resource type per minute
//which blueprint would maximize the number of opened geodes after 24 minutes
//return sum of (id * max geodes)
//
List<string> inputList = FileUtil.ReadFile("testdata.txt");
List<RR> rrList = new();
foreach (string str in inputList)
{
    var commaArr = str.Split(",").map(val => Convert.ToInt32(val));
    RR rr = new RR();
    rr.id = commaArr[0];
    rr.oreRobCost = commaArr[0];
    rr.clayRobCost = commaArr[1];
    rr.obRobOreCost = commaArr[2];
    rr.obRobClayCost = commaArr[3];
    rr.gRobOreCost = commaArr[4];
    rr.gRobObCost = commaArr[5];

    rrList.Add(rr);
}

Console.WriteLine($"Part 1: {part1()}");

int part1()
{
    int result = 0;
    for (int i = 0; i < rrList.Count; i++)
    {
        result += (rrList[i].id * getMaxGeode(rrList[i]));
    }
    return result;
}

int getMaxGeode(RR rr)
{
    //initial state
    //1 or
    //0 cr 
    //0 ob_r
    //0 gr
    //
    //0 ores    
    //0 clay    
    //0 obs
    return getMaxGeodeRec(rr, 24, new int[] { 1, 0, 0, 0 }, new int[] { 0, 0, 0 });
}

int getMaxGeodeRec(RR rr, int timeLeft, int[] numRobots, int[] numResources)
{
    //exit condition
    if (timeLeft == 0)
    {
        return 0;
    }
    //recurse
    //do work in this minute
    int maxGeode = int.MinValue;
    //dont create an ore robot
    int maxGeode = Math.Max(maxGeode, getMaxGeodeRec(rr,timeLeft-1, numRobots, numResources));
    //try creating an ore robot
    for (int i = 0; i < 4; i++)
    {
        if (canCreateRobot(rr, i, numResources))
        {
            /* numRobots[i]++; */
            int[] tempNumRobots = new int[];
            for(int j=0; j<4; j++){
                tempNumRobots[j] = numRobots[j];
            }
            tempNumRobots[i]++;
            var tempResources = updateResources(rr, i, numResources);
            maxGeode = Math.Max(maxGeode, getMaxGeodeRec(rr, timeLeft - 1, tempNumRobots, tempResources));
        }
    }
    
    for(int i=0; i<3; i++){
        numResources[i] += numRobots[i];
    }
    
    return maxGeode != int.MinValue ? maxGeode : 0;
}

bool canCreateRobot(RR rr, int robotType, int[] numResource)
{
    if (robotType == 0 && numResource[0] >= rr.oreRobCost)
    {
        return true;
    }

    if(robotType == 1 && numResource[1] >= rr.clayRobCost){
        return true;
    }

    if(robotType == 2 && numResource[0] >= rr.obRobOreCost && numResource[1] >= rr.clayRobCost){
        return true;
    }

    if(robotType == 3 && numResource[0] >= rr.gRobOreCost && numResource[2] >= rr.gRobObCost){
        return true;
    }
}

int[] updateResources(RR rr, int robotType, int[] numResources){
    int[] result = new int[3];
    for(int i=0; i<4; i++){
        result[i] = numResources[i];
    }
    
    if(robotType == 0){
        result[0] -= rr.oreRobCost;
    }else if(robotType == 1){
        result[1] -= rr.clayRobCost;
    }else if(robotType == 2){
        result[0] -= rr.obRobOreCost;
        result[1] -= rr.obRobClayCost;
    }else if(robotType == 3){
        result[0] -= rr.gRobOreCost;
        result[2] -= rr.gRobObCost;
    }
}
//robot requirement
public class RR
{
    public int id { get; set; }
    public int oreRobCost { get; set; }
    public int clayRobCost { get; set; }
    public int obRobOreCost { get; set; }
    public int obRobClayCost { get; set; }
    public int gRobOreCost { get; set; }
    public int gRobObCost { get; set; }
}



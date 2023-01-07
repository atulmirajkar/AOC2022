using Util;
List<string> inputList = FileUtil.ReadFile("testdata.txt");
HashSet<(int, int)> ptSet = new();
for (int i = 0; i < inputList.Count; i++) {
    for (int j = 0; j < inputList[i].Length; j++) {
        if (inputList[i][j] == '#') {
            ptSet.Add((i, j));
        }
    }
}
Console.WriteLine(part1());

int part1() {
    //t1 - look at all 8 directions
    //if there are no neighbors - no need to move
    //else decide on a direction to move
    //n,s,e,w --> s,e,w,n and so on
    //if two pts decide on the same next position, don't move
    char[] dir = new char[] { 'N', 'S', 'E', 'W' };
    int ds = 0; //dirStart
    int bn = int.MaxValue;
    int bs = int.MinValue;
    int bw = int.MaxValue;
    int be = int.MinValue;
    DisplayRound(ptSet, 0, 5, 0, 5);
    while (true) {
        int numMoves = 0;
        int cd = ds % 4;  //currDir 

        Dictionary<(int, int), List<(int, int)>> newPtToPt = new();
        HashSet<(int, int)> newPtSet = new();
        foreach (var pt in ptSet) {
            var pp = getMove(pt, cd); //proposedPt
            if (pp != pt) {
                Console.WriteLine($"{pt} --> {pp}");
                numMoves++;
            }

            List<(int, int)> oldPtList = newPtToPt.GetValueOrDefault(pp, new List<(int, int)>());
            oldPtList.Add(pt);
            newPtToPt[pp]=oldPtList;
        }

        // make the actual movement
        foreach (var newPt in newPtToPt.Keys) {
            //if clash use the old pt
            if (newPtToPt[newPt].Count > 1) {
                foreach (var oldPt in newPtToPt[newPt]) {
                    newPtSet.Add(oldPt);
                    updateBoundary(oldPt, ref bn, ref bs, ref bw, ref be);
                }
            } else {
                newPtSet.Add(newPt);
                updateBoundary(newPt, ref bn, ref bs, ref bw, ref be);
            }
        }

        //reset ptSet
        ptSet = newPtSet;

        ds++;
        Console.WriteLine($"Round {ds} PtSet count {ptSet.Count} numMoves {numMoves}");
        Console.WriteLine($"bn: {bn}, bs: {bs}, bw: {bw}, be:{be}");
        DisplayRound(ptSet, bn, bs, bw, be);
        if (numMoves == 0 || ds == 10) {
            int width = be - bw + 1;
            int height = bs - bn + 1;
            return (width * height) - ptSet.Count;
        }
    }
}

void DisplayRound(HashSet<(int, int)> set, int bn, int bs, int bw, int be){
    for(int i=bn; i<=bs; i++){
        Console.WriteLine();
        for(int j=bw; j<=be; j++){
            if(set.Contains((i,j))){
                Console.Write("#");
            }else{
                Console.Write(".");
            }
        }
    }
    Console.WriteLine();
}

void updateBoundary((int, int) pt, ref int bn, ref int bs, ref int bw, ref int be) {
    bn = Math.Min(bn, pt.Item1);
    bs = Math.Max(bs, pt.Item1);
    bw = Math.Min(bw, pt.Item2);
    be = Math.Max(be, pt.Item2);
}

(int, int) getMove((int, int) pt, int currDir) {
    bool hasN = false;
    //first check if pt alone - if yes we dont need to move
    for(int i=0; i<4; i++){
        if(hasNeigh(pt, currDir +i))
        {
            hasN = true;
        }
    }
    if(!hasN){
        return pt;
    }
    for (int i = 0; i < 4; i++) {
        if (!hasNeigh(pt, currDir + i)) {
            return getDir(pt, currDir+i);
        }
    }
    return pt;
}

(int, int) getDir((int, int) pt, int currDir) {
    char[] allDir = new char[] { 'N', 'S', 'E', 'W' };
    char dir = allDir[currDir%4];
    var pt1 = (-1, -1);
    if (dir == 'N') {
        pt1 = (pt.Item1 - 1, pt.Item2);
    } else if (dir == 'S') {
        pt1 = (pt.Item1 + 1, pt.Item2);
    } else if (dir == 'W') {
        pt1 = (pt.Item1, pt.Item2 - 1);
    } else if (dir == 'E') {
        pt1 = (pt.Item1, pt.Item2 + 1);
    }
    return pt1;
}

bool hasNeigh((int, int) pt, int currDir) {
    char[] allDir = new char[] { 'N', 'S', 'E', 'W' };
    char dir = allDir[currDir%4];
    var pt1 = (-1, -1);
    var pt2 = (-1, -1);
    var pt3 = (-1, -1);
    if (dir == 'N') {
        pt1 = (pt.Item1 - 1, pt.Item2);
        pt2 = (pt.Item1 - 1, pt.Item2 - 1);
        pt3 = (pt.Item1 - 1, pt.Item2 + 1);
    } else if (dir == 'S') {
        pt1 = (pt.Item1 + 1, pt.Item2);
        pt2 = (pt.Item1 + 1, pt.Item2 - 1);
        pt3 = (pt.Item1 + 1, pt.Item2 + 1);
    } else if (dir == 'W') {
        pt1 = (pt.Item1, pt.Item2 - 1);
        pt2 = (pt.Item1 - 1, pt.Item2 - 1);
        pt3 = (pt.Item1 + 1, pt.Item2 - 1);
    } else if (dir == 'E') {
        pt1 = (pt.Item1, pt.Item2 + 1);
        pt2 = (pt.Item1 - 1, pt.Item2 + 1);
        pt3 = (pt.Item1 + 1, pt.Item2 + 1);
    }

    if (ptSet.Contains(pt1) || ptSet.Contains(pt2) || ptSet.Contains(pt3)) {
        return true;
    }
    return false;
}




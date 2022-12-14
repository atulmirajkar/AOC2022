using Util;

List<string> inputList = FileUtil.ReadFile("./data.txt");


Console.WriteLine(part2(inputList));
int part2(List<string> inputList) {
    int minX = int.MaxValue;
    int maxX = int.MinValue;
    int minY = int.MaxValue;
    int maxY = int.MinValue;

    List<List<(int, int)>> allPath = new();
    foreach (string path in inputList) {
        allPath.Add(getPathPoint(path, ref minX, ref maxX, ref minY, ref maxY));
    }
    Console.WriteLine(minX + ":" + maxX + ":" + minY + ":" + maxY);

    //shift the array
    //int subX = minX;
    int subX = 0;
    int subY = 0;
    //int xSize = maxX - minX + 1;
    // int xSize = maxX + 1;

    int xSize = maxX + 500 + 1;

    // int ySize = maxY - minY + 1;

    //add infinite horizontal line
    maxY += 2;
    int ySize = maxY + 1;
    char[,] caveArr = new char[ySize, xSize];

    foreach (var path in allPath) {
        for (int i = 1; i < path.Count; i++) {
            addLine(caveArr, path[i - 1], path[i], subX, subY);
        }
    }

    addLine(caveArr, (subX, maxY), (xSize - 1, maxY), subX, subY);
    PrintCave(caveArr);

    int count = 0;
    bool overFlow = false;

    while (true) {
        simulatePathPart2(caveArr, 500 - subX, 0, ref overFlow);
        if (overFlow) {
            break;
        }
        count++;
    }
    Console.WriteLine();
    PrintCave(caveArr);

    return count + 1;
}
/*
    x - dist to right
    y - down
    line is a single path
    sand is pouring from 500,0
    sand is produced - 1 unit at a time
    if sand is blocked by either rock or sand - sand moves diagonally to down and left.
    if that tile is blocked - sand moves diagonally to down and right.
    Next sand is produced only after previous sand comes to a stand still
    
    498,4 -> 498,6 -> 496,6
    503,4 -> 502,4 -> 502,9 -> 494,9
*/

int part1(List<string> inputList) {
    int minX = int.MaxValue;
    int maxX = int.MinValue;
    int minY = int.MaxValue;
    int maxY = int.MinValue;

    List<List<(int, int)>> allPath = new();
    foreach (string path in inputList) {
        allPath.Add(getPathPoint(path, ref minX, ref maxX, ref minY, ref maxY));
    }
    Console.WriteLine(minX + ":" + maxX + ":" + minY + ":" + maxY);

    //shift the array
    int subX = minX;
    // int subY = minY; //should be 0
    int subY = 0;
    int xSize = maxX - minX + 1;
    // int ySize = maxY - minY + 1;
    int ySize = maxY + 1;
    char[,] caveArr = new char[ySize, xSize];

    foreach (var path in allPath) {
        for (int i = 1; i < path.Count; i++) {
            addLine(caveArr, path[i - 1], path[i], subX, subY);
        }
    }

    PrintCave(caveArr);

    int count = 0;
    bool overFlow = false;

    while (true) {
        simulatePath(caveArr, 500 - subX, 0, ref overFlow);
        if (overFlow) {
            break;
        }
        count++;
    }
    Console.WriteLine();
    PrintCave(caveArr);

    return count;
}
void simulatePathPart2(char[,] c, int startX, int startY, ref bool isOverFlow) {
    int x = startX;
    int y = startY;
    int maxX = c.GetLength(1) - 1;
    int maxY = c.GetLength(0) - 1;
    while (true) {
        while (y + 1 <= maxY) {
            if (c[y + 1, x] == '\0') {
                y++;
            } else {
                break;
            }
        }
        // if (y == 0) {
        //     isOverFlow = true;
        //     break;
        // }
        bool moved = false;
        if (y + 1 <= maxY) {
            if (x - 1 >= 0 && c[y + 1, x - 1] == '\0') {
                moved = true;
                y++;
                x--;
            } else if (x + 1 <= maxX && c[y + 1, x + 1] == '\0') {
                moved = true;
                y++;
                x++;
            }
        }

        if (!moved) {
            c[y, x] = 'O';
            if (y == 0 && x == 500) {
                isOverFlow = true;
            }
            break;
        }
    }
}


void simulatePath(char[,] c, int startX, int startY, ref bool isOverFlow) {
    int x = startX;
    int y = startY;
    int maxX = c.GetLength(1) - 1;
    int maxY = c.GetLength(0) - 1;
    // while (x >= 0 && x <= maxX && y >= 0 && y <= maxY) {
    while (true) {
        while (y + 1 <= maxY) {
            if (c[y + 1, x] == '\0') {
                y++;
            } else {
                break;
            }
        }
        if (y == maxY) {
            isOverFlow = true;
            break;
        }
        bool moved = false;
        if (y + 1 <= maxY) {
            if (x - 1 >= 0 && c[y + 1, x - 1] == '\0') {
                moved = true;
                y++;
                x--;
            } else if (x + 1 <= maxX && c[y + 1, x + 1] == '\0') {
                moved = true;
                y++;
                x++;
            }
        }
        if (x == 0 || x == maxX) {
            isOverFlow = true;
            break;
        }
        if (!moved) {
            c[y, x] = 'O';
            break;
        }
    }
}

void PrintCave(char[,] caveArr) {
    Console.WriteLine();
    for (int i = 0; i < caveArr.GetLength(0); i++) {
        for (int j = 0; j < caveArr.GetLength(1); j++) {
            if (caveArr[i, j] == '\0') {
                Console.Write(".\t");
                continue;
            }
            Console.Write(caveArr[i, j] + "\t");
        }
        Console.WriteLine();
    }
}

void addLine(char[,] caveArr, (int, int) start, (int, int) end, int subX, int subY) {
    //if same x value  - going down
    //|
    //|
    //V
    if (start.Item1 == end.Item1) {
        var yStart = start.Item2 - subY;
        var yEnd = end.Item2 - subY;
        var x = start.Item1 - subX;
        if (yStart < yEnd) {
            for (int i = yStart; i <= yEnd; i++) {
                caveArr[i, x] = '#';
            }

        } else {
            for (int i = yEnd; i <= yStart; i++) {
                caveArr[i, x] = '#';
            }

        }

    }
    //if same y value - going horizontal
    //-->
    if (start.Item2 == end.Item2) {
        var xStart = start.Item1 - subX;
        var xEnd = end.Item1 - subX;
        var y = start.Item2 - subY;
        if (xStart < xEnd) {
            for (int j = xStart; j <= xEnd; j++) {
                caveArr[y, j] = '#';
            }
        } else {
            for (int j = xEnd; j <= xStart; j++) {
                caveArr[y, j] = '#';
            }
        }
    }

}


/*
   j, x, item1-->
i, y,item2
|
|
v

0   1   2   3   4   5
1
2
3       +
4    

+ = (2,3)
*/
//item2 - is y value - columns 
//item1 - is x value - rows
List<(int, int)> getPathPoint(string path, ref int minX, ref int maxX, ref int minY, ref int maxY) {
    string[] pathPointArr = path.Split("->");
    List<(int, int)> result = new();
    foreach (string point in pathPointArr) {
        var pointArr = point.Split(",");
        var tempTuple = (Convert.ToInt32(pointArr[0]), Convert.ToInt32(pointArr[1]));
        minX = Math.Min(minX, tempTuple.Item1);
        maxX = Math.Max(maxX, tempTuple.Item1);
        minY = Math.Min(minY, tempTuple.Item2);
        maxY = Math.Max(maxY, tempTuple.Item2);
        result.Add(tempTuple);
    }
    return result;
}
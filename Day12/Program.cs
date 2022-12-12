/*
input - height map from above
alowest , z highest
S - current
E - best signal
S = a
E = z
fewest possible steps
1 step

Dist(x,y) = min of the 4 distances
we can take a path only if height is at a difference of 1
initially dist is max value
*/


using Util;

List<string> inputList = FileUtil.ReadFile("data.txt");

//create a 2d char array
int[,] heightArr = new int[inputList.Count, inputList[0].Length];
int startX = -1;
int startY = -1;
int endX = -1;
int endY = -1;
for (int i = 0; i < heightArr.GetLength(0); i++)
{
    for (int j = 0; j < heightArr.GetLength(1); j++)
    {
        if (inputList[i][j] == 'S')
        {
            heightArr[i, j] = 0;
            startX = i;
            startY = j;
        }
        else if (inputList[i][j] == 'E')
        {
            heightArr[i, j] = 'z' - 'a';
            endX = i;
            endY = j;
        }
        else
            heightArr[i, j] = inputList[i][j] - 'a';

        Console.Write(heightArr[i, j] + "\t");
    }
    Console.WriteLine();
}
Console.WriteLine(part1(heightArr, (startX, startY), (endX, endY), false));
Console.WriteLine(part2(heightArr, (endX, endY)));

/*
    better starting point?
    trail should start as low as possible && should take min steps to the top

    algo:
    Find all lowest points
    for each lowest point 
        find min steps
*/
int part2(int[,] heightArr, (int, int) endPos)
{
    int dist = part1(heightArr, endPos, (-1, -1), true);
    return dist;
}

/*
Using BFS
*/
int part1(int[,] heightArr, (int, int) startPos, (int, int) endPos, bool isPart2)
{
    //apparently tuples implement IComparable
    HashSet<(int, int)> visited = new();
    Queue<(int, int)> queue = new();
    queue.Enqueue(startPos);
    visited.Add(startPos);

    int[,] helper = new int[heightArr.GetLength(0), heightArr.GetLength(1)];
    int[,] dir = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
    int step = 1;
    bool found = false;
    while (queue.Count > 0)
    {

        int count = queue.Count;
        for (int i = 0; i < count; i++)
        {
            var currPos = queue.Dequeue();
            //foreach neighbor
            for (int j = 0; j < 4; j++)
            {
                var nextPos = (currPos.Item1 + dir[j, 0], currPos.Item2 + dir[j, 1]);
                if (visited.Contains(nextPos) || !canTakeDir(heightArr, nextPos, currPos, isPart2))
                {
                    continue;
                }
                if (isEndFound(heightArr, nextPos, endPos, isPart2))
                {
                    found = true;
                    break;
                }
                Console.WriteLine(currPos.Item1 + ":" + currPos.Item2 + "--> " + nextPos.Item1 + ":" + nextPos.Item2);
                helper[nextPos.Item1, nextPos.Item2] = step;
                queue.Enqueue(nextPos);
                visited.Add(nextPos);
            }
            if (found)
            {
                break;
            }
        }
        if (found)
        {
            break;
        }
        step++;
        Console.WriteLine("\nStep:" + step);
    }
    if (!found)
    {
        Console.WriteLine("not found");
        return int.MaxValue;
    }
    PrintHelper(helper);
    return step;
}

bool isEndFound(int[,] heightArr, (int, int) currPos, (int, int) endPos, bool isPart2)
{
    if (isPart2 && heightArr[currPos.Item1, currPos.Item2] == 0)
    {
        return true;
    }
    if (currPos == endPos)
    {
        return true;
    }
    return false;

}
void PrintHelper(int[,] memo)
{
    Console.WriteLine();
    for (int i = 0; i < memo.GetLength(0); i++)
    {
        for (int j = 0; j < memo.GetLength(1); j++)
        {
            Console.Write(memo[i, j] + "\t");
        }
        Console.WriteLine();
    }
}

bool canTakeDir(int[,] heightArr, (int, int) currPos, (int, int) prevPos, bool isPart2)
{
    //check out of bound
    int nr = heightArr.GetLength(0);
    int nc = heightArr.GetLength(1);
    if (currPos.Item1 < 0 || currPos.Item1 > nr - 1 || currPos.Item2 < 0 || currPos.Item2 > nc - 1)
    {
        return false;
    }


    //check if currPos <= prevPos + 1
    int currVal = heightArr[currPos.Item1, currPos.Item2];
    int prevVal = heightArr[prevPos.Item1, prevPos.Item2];
    if (isPart2 && currVal <= prevVal)
    {
        return true;
    }
    else if (currVal <= (prevVal + 1))
    {
        return true;
    }
    return false;
}
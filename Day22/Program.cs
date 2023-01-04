using Util;
using System.Text.RegularExpressions;

List<string> pathList = new();
string path = "";
bool isPath = false;
int nc = 0;
await foreach (string line in FileUtil.ReadFileLineAsync("data.txt"))
{
    if (string.IsNullOrWhiteSpace(line))
    {
        isPath = true;
        continue;
    }
    if (isPath)
    {
        path = line;
        break;
    }
    Console.WriteLine(line);
    nc = Math.Max(nc, line.Length);
    pathList.Add(line);
}

int nr = pathList.Count;
Console.WriteLine(nr + ":" + nc);
char[,] pathArr = new char[nr, nc];
for (int i = 0; i < nr; i++)
{
    for (int j = 0; j < nc; j++)
    {
        if (j > (pathList[i].Length - 1))
        {
            pathArr[i, j] = 'x';
            continue;
        }

        if (pathList[i][j] == ' ')
        {
            pathArr[i, j] = 'x';
        }
        else
        {
            pathArr[i, j] = pathList[i][j];
        }
    }
}
for(int i=0; i<nr; i++){
    Console.WriteLine();
    for(int j=0; j<nc; j++){
       Console.Write(pathArr[i,j]);
    }
}

//parse the path
Regex rx = new Regex(@"((\d*)([A-Z]))*(\d*)");
Match m = rx.Match(path);
int count = m.Groups[1].Captures.Count;
List<int> distList = new List<int>();
List<char> dirList = new List<char>();
for (int i = 0; i < count; i++)
{
    distList.Add(Convert.ToInt32(m.Groups[2].Captures[i].Value));
    dirList.Add(m.Groups[3].Captures[i].Value[0]);
}
distList.Add(Convert.ToInt32(m.Groups[4].Value));

//traverse the path
//start point? - top row - first .  

var pos = getStart(pathArr);


//you need 4 directions N,S,E,W
//you are initially going E
char currDir = 'E';
for (int i = 0; i < distList.Count; i++)
{
    int dist = distList[i];
    //we have to move dist time in currDir
    for (int j = 0; j < dist; j++)
    {
        if (!evalNextPos(ref pos, currDir, pathArr))
        {
            break;
        }
    }
    Console.WriteLine(pos.Item1 + ":" + pos.Item2);

    if (i != distList.Count - 1)
    {
        //change direction
        currDir = mapDirection(currDir, dirList[i]);
    }
}
Console.WriteLine(pos.Item1 + ":" + pos.Item2);

int row = pos.Item1+1;
int col = pos.Item2+1;
int dirVal=0;
if(currDir=='E'){
    dirVal=0;
}else if(currDir=='S'){
    dirVal=1;
}else if(currDir=='W'){
    dirVal=2;
}else if(currDir=='N'){
    dirVal=3;
}

Int64 result = 1000*row + 4*col + dirVal;
Console.WriteLine($"Result: {result}");


char mapDirection(char dir, char rlDir)
{
    if (dir == 'E')
    {
        if (rlDir == 'R')
        {
            return 'S';
        }
        if (rlDir == 'L')
        {
            return 'N';
        }
    }
    if (dir == 'W')
    {
        if (rlDir == 'R')
        {
            return 'N';
        }
        if (rlDir == 'L')
        {
            return 'S';
        }
    }
    if (dir == 'N')
    {
        if (rlDir == 'R')
        {
            return 'E';
        }
        if (rlDir == 'L')
        {
            return 'W';
        }
    }
    if (dir == 'S')
    {
        if (rlDir == 'R')
        {
            return 'W';
        }
        if (rlDir == 'L')
        {
            return 'E';
        }
    }
    throw new ArgumentException();
}
bool evalNextPos(ref (int, int) pos, char dir, char[,] pathArr)
{
    (int, int) tempPos = pos;
    getNextPos(ref tempPos, dir, pathArr);
    while (pathArr[tempPos.Item1, tempPos.Item2] == 'x')
    {
        getNextPos(ref tempPos, dir, pathArr);
    }
    if (pathArr[tempPos.Item1, tempPos.Item2] == '#')
    {
        return false;
    }
    pos = tempPos;

    return true;
}

void getNextPos(ref (int, int) pos, char dir, char[,] pathArr)
{
    Console.WriteLine($"{pos.Item1}:{pos.Item2} - {pathArr[pos.Item1, pos.Item2]}");
    int r = pos.Item1;
    int c = pos.Item2;
    int nr = pathArr.GetLength(0);
    int nc = pathArr.GetLength(1);

    if (dir == 'E')
    {
        pos.Item1 = r;
        pos.Item2 = (c + 1) % nc;
    }
    else if (dir == 'W')
    {
        pos.Item1 = r;
        pos.Item2 = (c - 1);
        if (pos.Item2 == -1)
        {
            pos.Item2 = nc - 1;
        }
    }
    else if (dir == 'N')
    {
        pos.Item1 = r - 1;
        if (pos.Item1 == -1)
        {
            pos.Item1 = nr - 1;
        }
        pos.Item2 = c;
    }
    else if (dir == 'S')
    {
        pos.Item1 = (r + 1) % nr;
        pos.Item2 = c;
    }

    Console.WriteLine($" Went {dir} - {pos.Item1}:{pos.Item2} - {pathArr[pos.Item1, pos.Item2]}");
}




(int, int) getStart(char[,] arr)
{
    int i = 0;
    for (int j = 0; j < arr.GetLength(1); j++)
    {
        if (arr[i, j] == '.')
        {
            return (i, j);
        }
    }
    throw new ArgumentException();
}

/*
    knot at each end
    head and tail
    Position of knots on a 2 dim grid

    series of motion for the head - you can determine how the tail moves

    output: how many positions does the dail visit atleast once:

    consider starting position as 0,0
    Create a set to keep track of positions for the tail.
    return set size

    Dist between head and tail = 1, we dont need to move the tail.
    E.g. h = 2,0 
    t = 0,0
    dist = sq rt of 2^2 - 0 
    dist>1 that means we need to move tail
    If in the same row - move rowwise
    if in the same col - move colwise
*/
using Util;

List<string> inputList = FileUtil.ReadFile("data.txt");
//part1(inputList);
part2(inputList, 9);

void part2(List<string> inputList, int tailLength){
    Tuple<int, int> head = Tuple.Create(0, 0);
    Tuple<int, int>[] tailArr = new Tuple<int, int>[tailLength];
    for(int i=0; i<tailArr.Length; i++)
    {
        tailArr[i] = Tuple.Create(0, 0);

    }
    HashSet<string> set = new HashSet<string>();
    set.Add("0:0");

    foreach (string move in inputList)
    {
        string[] moveArr = move.Split(" ");
        string dir = moveArr[0];
        int dist = Convert.ToInt32(moveArr[1]);
        Console.WriteLine(move);
        for (int i = 0; i < dist; i++)
        {
            head = moveHead(head, dir);
            tailArr[0] = moveTail(head, tailArr[0]);
            for(int j=1; j<tailArr.Length; j++){
                tailArr[j] = moveTail(tailArr[j-1],tailArr[j]);
            }
            // DisplayMove(head, tail);
            string hash = tailArr[tailArr.Length-1].Item1 + ":" + tailArr[tailArr.Length-1].Item2;
            if (!set.Contains(hash))
            {
                set.Add(hash);
            }
        }
    }
    Console.WriteLine(set.Count);
}
void part1(List<string> inputList)
{
    Tuple<int, int> head = Tuple.Create(0, 0);
    Tuple<int, int> tail = Tuple.Create(0, 0);
    HashSet<string> set = new HashSet<string>();
    set.Add("0:0");
    foreach (string move in inputList)
    {
        string[] moveArr = move.Split(" ");
        string dir = moveArr[0];
        int dist = Convert.ToInt32(moveArr[1]);
        Console.WriteLine(move);
        for (int i = 0; i < dist; i++)
        {
            head = moveHead(head, dir);
            tail = moveTail(head, tail);
            DisplayMove(head, tail);
            string hash = tail.Item1 + ":" + tail.Item2;
            if (!set.Contains(hash))
            {
                set.Add(hash);
            }
        }
    }
    Console.WriteLine(set.Count);
}

void DisplayMove(Tuple<int, int> head, Tuple<int, int> tail)
{
    Console.WriteLine($"({head.Item1}:{head.Item2}) - ({tail.Item1}:{tail.Item2})");
}

Tuple<int, int> moveTail(Tuple<int, int> head, Tuple<int, int> tail)
{
    //if head not far, no need to move
    if (!isMoreThanOne(head, tail))
    {
        return tail;
    }

    //if same x
    if (head.Item1 == tail.Item1)
    {
        if (head.Item2 > tail.Item2)
        {
            return Tuple.Create(tail.Item1, tail.Item2 + 1);
        }
        else
        {
            return Tuple.Create(tail.Item1, tail.Item2 - 1);
        }
    }
    //if same y
    if (head.Item2 == tail.Item2)
    {
        if (head.Item1 > tail.Item1)
        {
            return Tuple.Create(tail.Item1 + 1, tail.Item2);
        }
        else
        {
            return Tuple.Create(tail.Item1 - 1, tail.Item2);
        }
    }

    //diagonals

    int xDist = head.Item1 - tail.Item1;
    int yDist = head.Item2 - tail.Item2;
    if (xDist > 0 && yDist > 0)
    {
        return Tuple.Create(tail.Item1 + 1, tail.Item2 + 1);
    }
    else if (xDist > 0 && yDist < 0)
    {
        return Tuple.Create(tail.Item1 + 1, tail.Item2 - 1);
    }
    else if (xDist < 0 && yDist > 0)
    {
        return Tuple.Create(tail.Item1 - 1, tail.Item2 + 1);
    }
    else if (xDist < 0 && yDist < 0)
    {
        return Tuple.Create(tail.Item1 - 1, tail.Item2 - 1);
    }

    throw new ArgumentException();
}

bool isMoreThanOne(Tuple<int, int> head, Tuple<int, int> tail)
{
    if (Math.Abs(head.Item1 - tail.Item1) > 1)
    {
        return true;
    }
    if (Math.Abs(head.Item2 - tail.Item2) > 1)
    {
        return true;
    }
    return false;
}
Tuple<int, int> moveHead(Tuple<int, int> point, string dir)
{
    dir = dir.ToUpper();
    switch (dir)
    {
        case "R":
            return Tuple.Create(point.Item1 + 1, point.Item2);
        case "L":
            return Tuple.Create(point.Item1 - 1, point.Item2);
        case "U":
            return Tuple.Create(point.Item1, point.Item2 + 1);
        case "D":
            return Tuple.Create(point.Item1, point.Item2 - 1);
    }
    throw new ArgumentException();
}
using Util;

internal class Program
{
    private static void Main(string[] args)
    {
        List<string> inputList = FileUtil.ReadFile("testdata.txt");

        //we are given 1x1x1 cubes
        //we need to find how many surfaces are open
        //cube has 6 sides
        //for every cube check its 6 sides
        //x-- x++ y-- y++ z-- z++

        HashSet<(int, int, int)> ptSet = new();
        foreach (string input in inputList)
        {
            string[] inputArr = input.Split(",");
            int x = Convert.ToInt32(inputArr[0].Trim());
            int y = Convert.ToInt32(inputArr[1].Trim());
            int z = Convert.ToInt32(inputArr[2].Trim());
            ptSet.Add((x, y, z));
        }

        Console.WriteLine(part1());
        Console.WriteLine($"Part2: {part2()}");

        //kind of brute force
        //for every point check whether the point is in a pocket. If not get
        //surface area
        int part2()
        {
            int cnt = 0;
            foreach (var pt in ptSet)
            {
                if (inPocket(pt))
                {
                    continue;
                }
                cnt += getSurface(pt);
            }
            return cnt;

        }

        bool inPocket((int, int, int) pt)
        {
            //Console.WriteLine($"Cur Pt: {pt.Item1}:{pt.Item2}:{pt.Item3}");
            HashSet<(int, int, int)> visited = new();
            return DFS(pt, visited, pt);
        }

        bool DFS((int, int, int) pt, HashSet<(int, int, int)> visited, (int, int, int) origPt)
        {
            //exit condition
            if (pt.Item1 < 0 || pt.Item2 < 0 || pt.Item3 < 0 || pt.Item1 > 25 || pt.Item2 > 25 || pt.Item3 > 25)
            {
                return false;
            }

            if (visited.Contains(pt))
            {
                //Console.WriteLine($"Visited Pt: {pt.Item1}:{pt.Item2}:{pt.Item3}");
                return true;
            }

            if (pt != origPt && ptSet.Contains(pt))
            {
                //Console.WriteLine($"Another Pt: {pt.Item1}:{pt.Item2}:{pt.Item3}");
                return true;
            }

            //Console.WriteLine($"Neigh Pt: {pt.Item1}:{pt.Item2}:{pt.Item3}");

            visited.Add(pt);

            int[,] dir = { { -1, 0, 0 }, { 1, 0, 0 }, { 0, -1, 0 }, { 0, 1, 0 }, { 0, 0, -1 }, { 0, 0, 1 } };
            for (int i = 0; i < 6; i++)
            {
                (int, int, int) newPt = (pt.Item1 + dir[i, 0], pt.Item2 + dir[i, 1], pt.Item3 + dir[i, 2]);
                if (!DFS(newPt, visited, origPt))
                {
                    return false;
                }
            }
            visited.Remove(pt);
            return true;
        }

        int part1()
        {
            int cnt = 0;
            foreach (var pt in ptSet)
            {
                cnt += getSurface(pt);
            }
            return cnt;
        }

        int getSurface((int, int, int) pt)
        {
            int[,] dir = { { -1, 0, 0 }, { 1, 0, 0 }, { 0, -1, 0 }, { 0, 1, 0 }, { 0, 0, -1 }, { 0, 0, 1 } };
            int cnt = 0;
            for (int i = 0; i < 6; i++)
            {
                (int, int, int) newPt = (pt.Item1 + dir[i, 0], pt.Item2 + dir[i, 1], pt.Item3 + dir[i, 2]);
                if (ptSet.Contains(newPt))
                {
                    continue;
                }
                cnt++;
            }
            return cnt;
        }
    }
}

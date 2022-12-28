using Util;

internal class Program
{
    private static void Main(string[] args)
    {
        List<string> inputList = FileUtil.ReadFile("data.txt");

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
        Console.WriteLine($"Part 1: {part1()}");
        Console.WriteLine($"Part 1: {part2()}");

        int part2()
        {
            int cnt = 0;
            foreach (var pt in ptSet)
            {
                cnt += getSurface2(pt);
            }
            return cnt;
        }
        
        int getSurface2((int, int, int) pt)
        {
            int[,] dir = { { -1, 0, 0 }, { 1, 0, 0 }, { 0, -1, 0 }, { 0, 1, 0 }, { 0, 0, -1 }, { 0, 0, 1 } };
            int cnt = 0;
            for (int i = 0; i < 6; i++)
            {
                (int, int, int) newPt = (pt.Item1 + dir[i, 0], pt.Item2 + dir[i, 1], pt.Item3 + dir[i, 2]);
                //if sticking to other cube - surface not open
                if (ptSet.Contains(newPt))
                {
                    continue;
                }

                //if newPt is enclosed - continue
                HashSet<(int,int,int)> visited = new();
                visited.Add(newPt);
                if(!isPtOutside(newPt, visited)){
                    continue;
                }

                //else this pt should be outside
                cnt++;
            }
            return cnt;
        }

        bool isPtOutside((int, int, int) pt, HashSet<(int, int, int)> visited)
        {
            //exit condition
            if (pt.Item1 < 0 || pt.Item1 > 25 || pt.Item2 < 0 || pt.Item2 > 25 || pt.Item3 < 0 || pt.Item3 > 25)
            {
                return true;
            }

            int[,] dir = { { -1, 0, 0 }, { 1, 0, 0 }, { 0, -1, 0 }, { 0, 1, 0 }, { 0, 0, -1 }, { 0, 0, 1 } };
            for (int i = 0; i < 6; i++)
            {
                (int, int, int) newPt = (pt.Item1 + dir[i, 0], pt.Item2 + dir[i, 1], pt.Item3 + dir[i, 2]);
                if (visited.Contains(newPt) || ptSet.Contains(newPt)){
                    if(ptSet.Contains(newPt)){
                    }
                    continue;
                }

                visited.Add(newPt);
                if(isPtOutside(newPt, visited)){
                    return true;
                }
            }
            return false;
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

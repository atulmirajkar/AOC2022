using Util;

namespace AOC2022
{
    public static class FileSystemEntry
    {
        public static void Main()
        {
            List<string> inputList = FileUtil.ReadFile("data.txt");
            FileSystem fs = new FileSystem();
            fs.BuildFileSystem(inputList);
            fs.ReCalcSize();
            fs.PrintFS();
            //Console.WriteLine(fs.GetTotalAtmostSize(100000));

            //total disk space available on the fs = 70000000
            // need unused space = 30000000
            // smallest directory >= 30000000 

            //data.tx
            //size = 42805968
            // 
            fs.GetSmallestDir(70000000, 30000000);
        }
    }
}



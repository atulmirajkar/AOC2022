
using System.Diagnostics;
using Util;

Stopwatch sw = new Stopwatch();
sw.Start();
List<string> inputListAsync = await FileUtil.ReadFileAsync("./data.txt");
foreach(var str in inputListAsync){
    Console.WriteLine(str);
}
sw.Stop();
Console.WriteLine($"Whole file async time: {sw.ElapsedMilliseconds}\n");

sw.Reset();
sw.Start();
List<string> inputList = FileUtil.ReadFile("./data.txt");
foreach(var str in inputList){
    Console.WriteLine(str);
}
sw.Stop();
Console.WriteLine($"sync time: {sw.ElapsedMilliseconds}\n");

sw.Reset();
sw.Start();
await foreach(string str in FileUtil.ReadFileLineAsync("./data.txt")){
    Console.WriteLine(str);
}
sw.Stop();
Console.WriteLine($"async time: {sw.ElapsedMilliseconds}\n");


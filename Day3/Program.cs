using System.Diagnostics;
using Util;

Stopwatch watch = new Stopwatch();
watch.Start();
List<string> inputList = FileUtil.ReadFile("./data.txt");
//part1(inputList);
part2(inputList);
watch.Stop();
Console.WriteLine("Elapsed Time:"+watch.ElapsedMilliseconds+" ms");
/*
ruck 2 comp
1 item type per rucksack

case sensitive - item type

first half - first comp
second half - second comp

there would be a common letter in first half and second half

priorites - a-z 36
A-Z - 52

same number of items in each compartment - safe to assume even chars on each line
*/
void part1(List<string> inputList)
{
    int[] priorityArr = new int[52];
    foreach (string input in inputList)
    {
        char repeatedChar = getRepeatedChar(input);
        //if small letter
        if (repeatedChar >= 97)
        {
            priorityArr[repeatedChar - 'a']++;
        }
        else
        {
            priorityArr[(repeatedChar - 'A') + 26]++;
        }
    }

    int totalPriority = 0;
    for (int i = 0; i < 52; i++)
    {
        totalPriority += ((i + 1) * priorityArr[i]);
    }

}

/*
    elves are divided into groups of 3
    badge is the only item type carried by all 3 elves
    atmost two of the elves will be carrying any other item type?

    1  abc
    2   bc
    3   bc

*/
void part2(List<string> inputList){
    int totalPriority = 0;
    int i=0;
    while(i<inputList.Count){
        totalPriority += getGroupRepeatedPriority(inputList[i], inputList[i+1], inputList[i+2]);
        i=i+3;
    }

    Console.WriteLine(totalPriority);
}
int getGroupRepeatedPriority(string s1, string s2, string s3){
    //create a boolean string using string builder
    UInt64 mapOne = getMap(s1);
    UInt64 mapTwo = getMap(s2);
    UInt64 mapThree = getMap(s3);

    UInt64 final = mapOne & mapTwo & mapThree;
    int logVal = (int)Math.Log2(final)+1;
    return logVal;
}

UInt64 getMap(string s){
    UInt64 map = 0;
    foreach(char sChar in s){
        UInt64 shift = 0;
        int shiftVal = 0;
        if(sChar >=97){
            shiftVal = (int)(sChar - 'a');
            shift = (UInt64)1 << shiftVal; 
        } else{
            shiftVal = (int)((sChar-'A')+26);
            shift = (UInt64)1 << shiftVal;
        }
        map = map | shift;
    }
    return map;
}
char getRepeatedChar(string str)
{
    HashSet<char> charSet = new HashSet<char>();
    for (int i = 0; i < str.Length / 2; i++)
    {
        if (!charSet.Contains(str[i]))
        {
            charSet.Add(str[i]);
        }
    }

    for (int i = (str.Length / 2); i < str.Length; i++)
    {
        if (charSet.Contains(str[i]))
        {
            return str[i];
        }
    }

    Console.WriteLine("Something went wrong");
    return '\0';
}

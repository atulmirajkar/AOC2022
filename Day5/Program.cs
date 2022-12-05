using System.Text.RegularExpressions;
using Util;

/*
split data and stack input into different files
stack.txt
            [J]         [B]     [T]    
        [M] [L]     [Q] [L] [R]    
        [G] [Q]     [W] [S] [B] [L]
[D]     [D] [T]     [M] [G] [V] [P]
[T]     [N] [N] [N] [D] [J] [G] [N]
[W] [H] [H] [S] [C] [N] [R] [W] [D]
[N] [P] [P] [W] [H] [H] [B] [N] [G]
[L] [C] [W] [C] [P] [T] [M] [Z] [W]

clean up data to be the following
data.txt
 6  6  5
 2  5  9
 8  9  1
*/
var stackArr = readStackData();
List<string> dataList = FileUtil.ReadFile("./data.txt");
foreach (var data in dataList)
{
    Regex rx = new Regex(@"(\d+)");
    MatchCollection match = rx.Matches(data);
    if (match.Count == 0)
    {
        Console.WriteLine("Something went wrong");
        break;
    }

    int numMove = Convert.ToInt32(match[0].Groups[0].Value);
    int fromCol = Convert.ToInt32(match[1].Groups[0].Value);
    int toCol = Convert.ToInt32(match[2].Groups[0].Value);

    //modifyStack(stackArr, numMove, fromCol, toCol);
    modifyStackPart2(stackArr, numMove, fromCol, toCol);
}

for(int i=0; i<stackArr?.Length; i++){
    Console.WriteLine(stackArr[i].Peek());
}

void modifyStack(Stack<char>[]? stackArr, int numMove, int fromCol, int toCol)
{
    for (int i = 0; i < numMove; i++)
    {
        if (stackArr == null || stackArr[fromCol - 1].Count == 0)
        {
            throw new ArgumentOutOfRangeException();
        }
        stackArr[toCol-1].Push(stackArr[fromCol-1].Pop());
    }

}


void modifyStackPart2(Stack<char>[]? stackArr, int numMove, int fromCol, int toCol)
{
    Stack<char> tempStack = new Stack<char>();
    for (int i = 0; i < numMove; i++)
    {
        if (stackArr == null || stackArr[fromCol - 1].Count == 0)
        {
            throw new ArgumentOutOfRangeException();
        }
        tempStack.Push(stackArr[fromCol-1].Pop());
    }
    while(tempStack.Count>0)
    {
        stackArr?[toCol-1].Push(tempStack.Pop());
    }

}

Stack<char>[]? readStackData()
{
    //read stack
    List<string> stackRowList = FileUtil.ReadFile("./stack.txt");
    Stack<char>[]? stackArr = null;
    foreach (var stackLine in stackRowList)
    {
        var rowList = getRowFromStackLine(stackLine, 4);
        if (stackArr == null)
        {
            stackArr = new Stack<char>[rowList.Count];
        }

        for (int i = 0; i < rowList.Count; i++)
        {

            if (String.IsNullOrWhiteSpace(rowList[i]))
            {
                continue;
            }

            Regex rx = new Regex(@"([A-Z])");
            Match match = rx.Match(rowList[i].Trim());
            if (stackArr[i] == null)
            {
                stackArr[i] = new Stack<char>();
            }
            stackArr[i].Push(match.Groups[0].Value[0]);
        }
    }

    //reverse each stack
    for (int i = 0; i < stackArr?.Length; i++)
    {
        stackArr[i] = reverseStack(stackArr[i]);
    }

    return stackArr;
}


Stack<T> reverseStack<T>(Stack<T> inputStack)
{
    Stack<T> outputStack = new Stack<T>();
    while (inputStack.Count > 0)
    {
        outputStack.Push(inputStack.Pop());
    }
    return outputStack;
}

List<string> getRowFromStackLine(string line, int chunk)
{
    //break into chunks of 3
    string ln = line;
    List<string> result = new List<string>();
    while (ln.Length > 0)
    {
        if (ln.Length > 4)
        {
            result.Add(ln.Substring(0, chunk));
        }
        else
        {
            result.Add(ln);
            break;
        }

        ln = ln.Substring(chunk);
    }
    return result;
}
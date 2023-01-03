using Util;
using System.Text.RegularExpressions;
// awk '/^\w*:\s*[0-9]*$/{print}' testdata.txt | sort -k 2
// highest number in data.txt 3075 - can go out of bounds easily

Dictionary<string, Node> nodeMap = new();
await foreach (string str in FileUtil.ReadFileLineAsync("data.txt"))
{
    populateDS(str);
}
Console.WriteLine(evalTree(nodeMap["root"]));
/* DisplayTree(nodeMap["root"]); */


Int64 evalTree(Node node)
{
    //exit condition
    if (node.isLeaf)
    {
        return (Int64)node.val;
    }

    Int64 leftVal = evalTree(node.left);
    Int64 rightVal = evalTree(node.right);

    if (node.op == '+')
    {
        return leftVal + rightVal;
    }
    else if (node.op == '-')
    {
        return leftVal - rightVal;
    }
    else if (node.op == '/')
    {
        if (rightVal == 0)
        {
            throw new DivideByZeroException();
        }
        return leftVal / rightVal;
    }
    else if (node.op == '*')
    {
        return leftVal * rightVal;
    }

    throw new ArgumentException();
}
void DisplayTree(Node node)
{
    //exit condition
    if (node == null)
    {
        return;
    }


    DisplayTree(node.left);

    Console.WriteLine();
    Console.Write(node.name + "\t");
    if (node.isLeaf)
    {
        Console.WriteLine(node.val);
    }
    else
    {
        Console.WriteLine(node.op);
    }

    DisplayTree(node.right);
}

void populateDS(string line)
{
    Node? node = null;

    Regex rx = new Regex(@"^(\w*):\s*(\d*)");
    Match m = rx.Match(line);
    //add a leaf node
    if (m.Length == line.Length)
    {
        Console.WriteLine($"{m.Groups[1].Value} - {m.Groups[2].Value}");
        string tempName = m.Groups[1].Value;
        int value = Convert.ToInt32(m.Groups[2].Value);

        if (nodeMap.ContainsKey(tempName))
        {
            node = nodeMap[tempName];
        }
        else
        {
            node = new Node();
        }

        node.name = tempName;
        node.val = value;
        node.isLeaf = true;
        nodeMap[node.name] = node;
        return;
    }

    rx = new Regex(@"(\w*)\s*:\s*(\w*)\s([+-/*]?)\s(\w*)");
    m = rx.Match(line);
    if (m.Length != line.Length)
    {
        throw new ArgumentException();
    }
    string name = m.Groups[1].Value;
    string leftName = m.Groups[2].Value;
    char op = m.Groups[3].Value[0];
    string rightName = m.Groups[4].Value;

    if (nodeMap.ContainsKey(name))
    {
        node = nodeMap[name];
    }
    else
    {
        node = new Node();
    }

    node.name = name;
    node.op = op;
    //set left and right nodes
    if (nodeMap.ContainsKey(leftName))
    {
        node.left = nodeMap[leftName];
    }
    else
    {
        Node leftNode = new Node();
        leftNode.name = leftName;
        nodeMap[leftNode.name] = leftNode;
        node.left = leftNode;
    }
    if (nodeMap.ContainsKey(rightName))
    {
        node.right = nodeMap[rightName];
    }
    else
    {
        Node rightNode = new Node();
        rightNode.name = rightName;
        nodeMap[rightNode.name] = rightNode;
        node.right = rightNode;
    }
    nodeMap[node.name] = node;
}

class Node
{
    public string name { get; set; } = "";
    public char op { get; set; }
    public Node? left { get; set; }
    public Node? right { get; set; }
    public int val { get; set; }
    public bool isLeaf { get; set; }
}

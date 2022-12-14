using System;
using System.Text;

using Util;
/*
input - list of received packets
pairs of packets, pairs are separated by blank line

How many pairs of packets are in the right order?

packet is a list and appears on its own line

Rules:
if ints, left less than right, valid
if same, continue checking the rest
if lists
if the left list runs out of items - valid
if 1 value and list, convert 1 value to list and follow 2

first pair has index 1. Sum of the indices

Datastructure:

Class Item{
List<int>
}
Class Packet{
List<Item> itemList;
}
Tuple<Packet, Packet>

*/
List<string> inputList = FileUtil.ReadFile("./data.txt");
Console.WriteLine(part1(inputList));

int part1(List<string> inputList) {
    int i = 0;
    int count = 0;
    int pairIdx = 1;
    while (i < inputList.Count) {
        Console.WriteLine();
        string firstStr = inputList[i++];
        Packet? p1 = ParseAndBuildPacket(firstStr);  //dont send the brackets 

        string secondStr = inputList[i++];
        Packet? p2 = ParseAndBuildPacket(secondStr);  //dont send the brackets 

        if (String.Compare(DisplayPacket(p1), firstStr) != 0) {
            throw new ArgumentException();
        }
        if (String.Compare(DisplayPacket(p2), secondStr) != 0) {
            throw new ArgumentException();
        }
        Console.WriteLine(firstStr);
        Console.WriteLine(secondStr);

        PacketComparer pc = new PacketComparer();
        if (pc.Compare(p1, p2) < 0) {
            count += pairIdx;
            Console.WriteLine($"IsSmaller - {pairIdx}");
        } else {
            Console.WriteLine($"Not Smaller- {pairIdx}");
        }
        pairIdx++;
        i++;//skip new line
    }
    return count;
}

//recursive parser
Packet? ParseAndBuildPacket(string str) {
    //exit condition
    if (String.IsNullOrEmpty(str)) {
        return null;
    }

    //recurse
    Packet p = new Packet();
    int splitIdx = -1;
    if (str[0] == '[') {
        splitIdx = getMatching(str, 0);
        p.InnerPacket = ParseAndBuildPacket(str.Substring(1, splitIdx - 1));  //dont send the brackets again
        splitIdx++; //points to comma
    } else {
        splitIdx = str.IndexOf(','); //index of comma
        if (splitIdx != -1) {
            p.Val = Convert.ToInt32(str.Substring(0, splitIdx));
        } else {
            //last packet value e.g. 42
            p.Val = Convert.ToInt32(str);
        }
    }
    //split the string for the next iteration
    if (splitIdx == -1 || splitIdx > str.Length - 1) {
        //we are done 
        return p;
    }
    str = str.Substring(splitIdx + 1);
    p.NextPacket = ParseAndBuildPacket(str);
    return p;

}

string DisplayPacket(Packet? p) {
    StringBuilder sb = new StringBuilder();
    DisplayPacketRec(p, true, ref sb);
    return sb.ToString();
}

void DisplayPacketRec(Packet? p, bool isFirst, ref StringBuilder sb) {
    if (p == null) {
        return;
    }
    //either inner packet or val will be populated
    if (p.Val == null) {
        if (isFirst) {
            sb.Append("[");

        } else {
            sb.Append(",[");

        }
        DisplayPacketRec(p.InnerPacket, true, ref sb);
        sb.Append("]");
    } else {
        if (isFirst) {
            sb.Append(p.Val);
        } else {
            sb.Append($",{p.Val}");

        }
    }
    DisplayPacketRec(p.NextPacket, false, ref sb);
}

int getMatching(string str, int startIdx) {

    Stack<char> stack = new();
    for (int i = startIdx; i < str.Length; i++) {
        if (str[i] == '[') {
            stack.Push(str[i]);
        } else if (str[i] == ']') {
            stack.Pop();
            if (stack.Count == 0) {
                return i;
            }
        }
    }
    throw new ArgumentException();
}


class PacketComparer : IComparer<Packet> {

    public int Compare(Packet? x, Packet? y) {
        //if both have finished
        if (x == null && y == null) {
            return 0;
        }

        //if x has finished 
        if (x == null) {
            return -1;
        }

        //if y has finished
        if (y == null) {
            return 1;
        }




        //If both values are integers, the lower integer should come first.
        //the inputs are the same integer; continue checking the next part of the input.
        if (x.Val != null && y.Val != null) {
            if (x.Val < y.Val) {
                return -1;
            }
            if (x.Val == y.Val) {
                return Compare(x.NextPacket, y.NextPacket);
            }
            if (x.Val > y.Val) {
                return 1;
            }
        }

        //If both values are lists, compare the first value of each list, 
        //then the second value, and so on. If the left list runs 
        //out of items first, the inputs are in the right order. If 
        //the right list runs out of items first, the inputs are not in 
        //the right order. If the lists are the same length and no comparison 
        //makes a decision about the order, continue checking the next part of the input.
        if (x.InnerPacket != null && y.InnerPacket != null) {
            //check inner packets and then go to next
            if (Compare(x.InnerPacket, y.InnerPacket) < 0) {
                return -1;
            }
            if (Compare(x.InnerPacket, y.InnerPacket) == 0) {
                return Compare(x.NextPacket, y.NextPacket);
            }
            if (Compare(x.InnerPacket, y.InnerPacket) > 0) {
                return 1;
            }

        }

        //If exactly one value is an integer, convert the integer to a list 
        //which contains that integer as its only value, then retry the comparison. 
        //For example, if comparing [0,0,0] and 2
        //, convert the right value to [2] (a list containing 2); 
        //the result is then found by instead comparing [0,0,0] and [2].
        if (x.Val != null && y.InnerPacket != null) {
            Packet tempPacket = new Packet();
            tempPacket.NextPacket = x.NextPacket;
            tempPacket.InnerPacket = new Packet();
            tempPacket.InnerPacket.Val = x.Val;
            return Compare(tempPacket, y);
        }

        if (x.InnerPacket != null && y.Val != null) {
            Packet tempPacket = new Packet();
            tempPacket.NextPacket = y.NextPacket;
            tempPacket.InnerPacket = new Packet();
            tempPacket.InnerPacket.Val = y.Val;
            return Compare(x, tempPacket);
        }


        //[] - x.val = null and x.innerList = null

        if (x.Val == null && x.InnerPacket == null && y.Val == null && y.InnerPacket == null) {
            return 0;
        }
        if (x.Val == null && x.InnerPacket == null) {
            return -1;
        }
        if (y.Val == null && y.InnerPacket == null) {
            return 1;
        }

        throw new ArgumentException();
    }
}
class Packet {
    public int? Val { get; set; }
    public Packet? NextPacket { get; set; }
    public Packet? InnerPacket { get; set; }

}
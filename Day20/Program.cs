using Util;
/*
 * Problem:
 * Given a list of numbers, you have to shift the number, with a shift value = value of the number 
 * Find positions of 0
 * Find 1000th, 2000th, 3000th numner in the list and add those
 * Possible solution:
 * Create a linked list 
*/
List<Node> inputList = new List<Node>();
Node? head = null;
Node? currNode = null;
await foreach (string str in FileUtil.ReadFileLineAsync("data.txt")) {
    int val = Convert.ToInt32(str);
    Node tempNode = new Node(val);
    inputList.Add(tempNode);
    if (head == null) {
        head = tempNode;
        currNode = tempNode;
    } else {
        currNode!.next = tempNode;
        tempNode.prev = currNode;
        currNode = currNode.next;
    }
}
//make it cyclic
head.prev = currNode;
currNode!.next = head;

Console.WriteLine(part1(inputList));

int part1(List<Node> inputList) {
    Node head = inputList[0];
    Node zeroNode = null;
    foreach (Node node in inputList) {
        if (node.val == 0) {
            zeroNode = node;
        }
        shiftNode(node, inputList.Count);
        Console.WriteLine("shifting node:" + node.val);
    }

    int result = 0;

    /* for (int i = 1; i <= 3; i++) { */
    /*     zeroNode = getValueAtJump(zeroNode, 1000); */
    /*     result += zeroNode.val; */
    /* } */

    //add to list
    List<Node> resultList = new();
    resultList.Add(zeroNode);
    Node iter = zeroNode.next;
    while (iter != zeroNode) {
        resultList.Add(iter);
        iter = iter.next;
    }

    //display
    Console.WriteLine("Final list:" + resultList.Count);
    foreach (Node node in resultList) {
        Console.WriteLine(node.val);
    }

    int jIdx = 0;
    for (int i = 1; i <= 3; i++) {
        jIdx += 1000;
        result += resultList[jIdx % resultList.Count].val;
    }

    return result;
}

void DisplayList(Node head) {
    Console.Write(head.val + "\t");
    Node nNode = head.next;
    while (nNode != head) {
        Console.Write(nNode.val + "\t");
        nNode = nNode.next;
    }
    Console.WriteLine();
}

Node getValueAtJump(Node node, int jVal) {
    for (int i = 1; i <= jVal; i++) {
        node = node.next;
    }
    return node;
}



void shiftNode(Node node, int size) {
    bool isForward = node.val > 0;
    int jumpVal = Math.Abs(node.val);
    //no need to shift if final shift value is 0
    jumpVal = jumpVal % (size - 1);
    if (jumpVal == 0) {
        return;
    }
    Node tNode = node;
    for (int i = 1; i <= jumpVal; i++) {
        if (isForward) {
            tNode = tNode.next;
        } else {
            tNode = tNode.prev;
        }
    }

    removeNode(node);
    if (isForward) {
        addAfter(tNode, node);
    } else {
        //if going back go back one more
        tNode = tNode.prev;
        addAfter(tNode, node);
    }

}

void addAfter(Node tNode, Node node) {
    Node nextNode = tNode.next;

    tNode.next = node;
    node.prev = tNode;

    node.next = nextNode;
    nextNode.prev = node;
}

void removeNode(Node node) {
    Node prevNode = node.prev;
    Node nextNode = node.next;

    node.next = null;
    node.prev = null;
    prevNode.next = nextNode;
    nextNode.prev = prevNode;
}
class Node {
    public int val { get; set; }
    public Node? next { get; set; }
    public Node? prev { get; set; }

    public Node(int val) {
        this.val = val;
    }
}
namespace AOC2022
{
    public enum FileType
    {
        File,
        Dir
    }

    public class FileSystem
    {
        // First we need to build the file structure in memory
        public class Node
        {
            public string name;
            public FileType type;
            public Int64 size;
            public List<Node> nodeList;

            public Node(string name, FileType type, Int64 size)
            {
                this.name = name;
                this.type = type;
                this.size = size;
                nodeList = new List<Node>();
            }

            public void AddNode(Node innerNode)
            {
                if (this.type == FileType.File)
                {
                    throw new ArgumentException();
                }

                nodeList!.Add(innerNode);
            }

            public string getParentDirName(Node node)
            {
                if (node.name == "/")
                {
                    return "";
                }
                //  /c/d/e/f
                //  01234567
                //remove f 
                int lastSlashIdx = node.name.LastIndexOf("/");
                if (lastSlashIdx == 0)
                {
                    return "/";
                }
                return node.name.Substring(0, lastSlashIdx);
            }
        }

        //we will also index the node with the fileName for easy access
        Dictionary<string, Node> nodeIndex = new Dictionary<string, Node>();

        Node? rootNode = null;
        private void getDirNearestToSize(Node node, Int64 minSizeToDelete, ref Node smallestDirFound)
        {
            if (node == null)
            {
                return;
            }

            if (node.type == FileType.File)
            {
                return;
            }

            //no point in exploring folders which have size smaller than minimum size
            if(node.size < minSizeToDelete){
                return;
            }

            if (node.size < smallestDirFound.size)
            {
                smallestDirFound = node;
            }
            foreach (Node innerNode in node.nodeList)
            {
                getDirNearestToSize(innerNode, minSizeToDelete, ref smallestDirFound);
            }
        }
        private bool isCommandLine(string command)
        {
            if (command.Split(" ")[0] == "$")
            {
                return true;
            }
            return false;
        }
        private void printFSRec(Node node, string indent)
        {
            if (node == null)
            {
                return;
            }
            if (node.type == FileType.File)
            {
                Console.WriteLine(indent + node.size + " " + node.name);
                return;
            }

            Console.WriteLine(indent + "dir " + node.name + ", Size: " + node.size);
            foreach (Node innerNode in node.nodeList)
            {
                printFSRec(innerNode, indent + "\t");
            }
        }
        private string getInnerNodeName(string parentName, string currName)
        {
            if (parentName == "/")
            {
                return parentName + currName;
            }
            return parentName + "/" + currName;
        }

        private Int64 calcSizeRec(Node node)
        {
            //exit condition
            if (node == null)
            {
                return 0;
            }

            if (node.type == FileType.File)
            {
                return node.size;
            }
            Int64 size = 0;
            foreach (Node innerNode in node.nodeList)
            {
                size += calcSizeRec(innerNode);
            }
            node.size = size;
            return size;
        }

        private Int64 getTotalAtmostSizeRec(Node node, Int64 size)
        {
            if (node == null)
            {
                return 0;
            }
            if (node.type == FileType.File)
            {
                return 0;
            }

            Int64 result = 0;
            if (node.size <= size)
            {
                result += node.size;
            }
            foreach (Node innerNode in node.nodeList)
            {
                if (innerNode.type == FileType.File)
                {
                    continue;
                }

                result += getTotalAtmostSizeRec(innerNode, size);
            }
            return result;
        }

        //public methods
        public void PrintFS()
        {
            if (rootNode == null)
            {
                Console.WriteLine("FS empty");
                return;
            }
            printFSRec(rootNode, "");
        }
        
        public void GetSmallestDir(int totalDiskSpace, int freeSpaceNeeded)
        {
            if (rootNode == null)
            {
                return;
            }
            if (rootNode.size == 0)
            {
                ReCalcSize();
            }
            Int64 freeSpaceAvail = (totalDiskSpace - rootNode.size);
            if (freeSpaceAvail < 0)
            {
                throw new ArgumentException();
            }
            Int64 minSizeToDelete = freeSpaceNeeded - freeSpaceAvail;
            Node smallestDirFound = rootNode;
            Console.WriteLine("MinSizeToDelete:" + minSizeToDelete);
            getDirNearestToSize(rootNode, minSizeToDelete, ref smallestDirFound);
            printFSRec(smallestDirFound, "");
        }


        public Int64 GetTotalAtmostSize(Int64 size)
        {
            Node? node = rootNode;
            if (node == null)
            {
                return 0;
            }
            return getTotalAtmostSizeRec(node, size);
        }

        /*
            c
                a 11000
                    b 3000
                    d 8000 
        */
        public void ReCalcSize()
        {
            Node? node = rootNode;
            if (node == null)
            {
                return;
            }
            calcSizeRec(node);
        }


        public void BuildFileSystem(List<string> commandList)
        {
            Node? currentNode = null;
            foreach (string command in commandList)
            {
                // Console.WriteLine("CurrentDir:" + currentNode?.name);
                // Console.WriteLine(command);
                if (isCommandLine(command))
                {
                    string[] comArr = command.Split(" ");
                    string commandName = comArr[1];
                    if (commandName == "cd")
                    {

                        string commandValue = comArr[2];
                        if (commandValue == "..")
                        {
                            string? dirStr = currentNode?.getParentDirName(currentNode);
                            if (dirStr == null || !nodeIndex.ContainsKey(dirStr))
                            {
                                throw new ArgumentException();
                            }
                            currentNode = nodeIndex[dirStr];
                        }
                        else
                        {
                            //we have to create the dir if not already created
                            if (currentNode == null && commandValue == "/")
                            {
                                //we have just started parsing
                                currentNode = new Node("/", FileType.Dir, 0);
                                nodeIndex[currentNode.name] = currentNode;
                                rootNode = currentNode;
                            }
                            else if (currentNode != null)
                            {
                                //add the new dir to current dir and set currentNode
                                string newDirStr = getInnerNodeName(currentNode.name, commandValue);
                                Node newInnerNode = new Node(newDirStr, FileType.Dir, 0);
                                currentNode.AddNode(newInnerNode);
                                currentNode = newInnerNode;
                                //update index
                                nodeIndex[currentNode.name] = currentNode;
                            }
                            else
                            {
                                throw new ArgumentException();
                            }

                        }
                    }
                    else
                    {
                        // we dont have to do anything
                    }
                }
                else
                {
                    //it is a file or directory
                    //we have to add this to the current node
                    if (currentNode == null)
                    {
                        throw new ArgumentException();
                    }

                    string[] comArr = command.Split(" ");
                    if (comArr[0] == "dir")
                    {
                        // string dirName = comArr[1];
                        // currentNode.AddNode(new Node(dirName, FileType.Dir, 0));
                        //do nothing - will do work only when cd <dir>
                    }
                    else
                    {

                        string fileName = comArr[1];
                        var size = Convert.ToInt64(comArr[0]);
                        currentNode.AddNode(new Node(fileName, FileType.File, size));
                    }
                }
            }
        }
    }
}
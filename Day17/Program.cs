/*
-

+

ulta L

|
|

||

|   |   |   |   |   |   |   |
        -



-----------------------------

each shape can be a set of points w.r.t its left most bottom edge?
or can be a set of lines?

00-30
01-03, 10-13
00-20, 20-23
00-03
00-10,01-11
            |
        -   -   -
            |            
---------------------------

I guess it is like tetris- if all the positions in a row are occupied we can discard 
everything below.
if a shape hits an obstacle we have to update the height array.
way to find collision


List or rows. Each row of size 7

3068 * 4 = 12,272 max height
12272 * 7 = 84000 chars = 180000 bytes = 180 mb - not that much   
Each rock appears so that its left edge is two units away from the left 
wall and its bottom edge is three units above the highest rock in the room 
(or the floor, if there isn't one).
*/

using Util;

List<string> inputList = FileUtil.ReadFile("./data.txt");
string input = inputList[0];
List<bool> rightList = new List<bool>();  //left false
foreach (char c in input) {
    switch (c) {
        case '<':
            rightList.Add(false);
            break;
        case '>':
            rightList.Add(true);
            break;
    }
}

char[,] arr = new char[13000, 7];
// char[,] arr = new char[20, 7];
List<Shape> shapeList = getShapeList();
Console.WriteLine(part1());

int part1() {
    int sIter = 0;
    int dirIter = 0;
    int h = 0;
    // while (sIter < 2022) {
    while (sIter < 2022) {
        Shape s = shapeList[(sIter++) % 5].getCopy();
        s.addOffset(2, h + 3);
        //todo we need to offset s according to max height
        // foreach (bool r in rightList) {
        while (true) {
            bool r = rightList[(dirIter++) % rightList.Count];
            bool isCol = false;
            if (r) {
                isCol = !s.goDir(dir.right, 0, 6, 0);
            } else {
                isCol = !s.goDir(dir.left, 0, 6, 0);
            }
            if (isCol || isCollisionWithArr(s)) {
                //collision with left or right
                if (r) {
                    s.goDir(dir.left, 0, 6, 0);
                } else {
                    s.goDir(dir.right, 0, 6, 0);

                }
            }

            isCol = !s.goDir(dir.down, 0, 6, 0);
            if (isCol || isCollisionWithArr(s)) {
                //collision with bottom - go up
                s.goDir(dir.up, 0, 6, 0);
                h = Math.Max(h, imprint(s, arr));
                break;
            }
        }
        // if (sIter == 12)
        //     DisplayArr();
    }
    return h;
}

void DisplayArr() {
    for (int i = arr.GetLength(0) - 1; i >= 0; i--) {
        Console.WriteLine();
        for (int j = 0; j < arr.GetLength(1); j++) {
            Console.Write((char)arr[i, j] + "\t");
        }
    }
    Console.WriteLine();
}

bool isCollisionWithArr(Shape s) {
    foreach (var p in s.pointList) {
        if (arr[p.Item2, p.Item1] == '#') {
            return true;
        }
    }
    return false;
}

//returns max height
int imprint(Shape s, char[,] arr) {
    int maxHeight = int.MinValue;
    foreach (var point in s.pointList) {
        arr[point.Item2, point.Item1] = '#';
        maxHeight = Math.Max(maxHeight, point.Item2);
    }
    if (maxHeight == int.MinValue) {
        throw new ArgumentException();
    }
    return maxHeight + 1;
}

List<Shape> getShapeList() {
    List<Shape> shapeList = new();
    //define shapes?
    Shape one = new Shape();
    one.pointList.Add((0, 0));
    one.pointList.Add((1, 0));
    one.pointList.Add((2, 0));
    one.pointList.Add((3, 0));
    shapeList.Add(one);


    Shape two = new Shape();
    two.pointList.Add((1, 0));

    two.pointList.Add((0, 1));
    two.pointList.Add((1, 1));
    two.pointList.Add((2, 1));

    two.pointList.Add((1, 2));
    shapeList.Add(two);

    Shape three = new Shape();
    three.pointList.Add((0, 0));
    three.pointList.Add((1, 0));
    three.pointList.Add((2, 0));

    three.pointList.Add((2, 1));
    three.pointList.Add((2, 2));
    shapeList.Add(three);

    Shape four = new Shape();
    four.pointList.Add((0, 0));
    four.pointList.Add((0, 1));
    four.pointList.Add((0, 2));
    four.pointList.Add((0, 3));
    shapeList.Add(four);

    Shape five = new Shape();
    five.pointList.Add((0, 0));
    five.pointList.Add((1, 0));

    five.pointList.Add((0, 1));
    five.pointList.Add((1, 1));
    shapeList.Add(five);

    return shapeList;
}

public enum dir {
    left,
    right,
    down,
    up
}
public class Shape {
    public List<(int, int)> pointList = new();
    public bool goDir(dir d, int leftX, int rightX, int bottomY) {
        bool isCol = false;
        for (int i = 0; i < pointList.Count; i++) {
            (int, int) point = pointList[i];
            if (d == dir.left) {
                point = (point.Item1 - 1, point.Item2);
            } else if (d == dir.right) {
                point = (point.Item1 + 1, point.Item2);
            } else if (d == dir.down) {
                point = (point.Item1, point.Item2 - 1);
            } else if (d == dir.up) { //not required
                point = (point.Item1, point.Item2 + 1);
            }
            if (point.Item1 < leftX || point.Item1 > rightX) {
                isCol = true;
            } else if (point.Item2 < bottomY) {
                isCol = true;
            }
            pointList[i] = point;
        }
        return !isCol;
    }

    public void addOffset(int x, int y) {
        for (int i = 0; i < pointList.Count; i++) {
            var newP = (pointList[i].Item1 + x, pointList[i].Item2 + y);
            pointList[i] = newP;
        }
    }

    public Shape getCopy() {
        Shape newS = new();
        foreach (var p in this.pointList) {
            newS.pointList.Add(p);
        }
        return newS;
    }
}


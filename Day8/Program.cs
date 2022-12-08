
/*

    0 - 9 height
    look up down left or right
    tree is visible if all the other trees are shorter than it from an edge
    How many trees are visible

    all trees on the edge are visible
*/

using Util;

List<string> inputList = FileUtil.ReadFile("data.txt");
int nr = inputList.Count;
int nc = inputList[0].Length;
//create a 2d array
int[,] arr = new int[nr, nc];
for (int i = 0; i < nr; i++)
{
    for (int j = 0; j < nc; j++)
    {
        //Console.WriteLine(inputList[i][j]);
        arr[i, j] = Convert.ToInt32(inputList[i][j] + "");
    }
}

Console.WriteLine(part2(arr));

int part1(int[,] arr)
{
    int[] maxFromTop = new int[arr.GetLength(0)];
    int[] maxFromBottom = new int[arr.GetLength(0)];
    int[] maxFromLeft = new int[arr.GetLength(1)];
    int[] maxFromRight = new int[arr.GetLength(1)];

    int count = 0;
    int nr = arr.GetLength(0);
    int nc = arr.GetLength(1);
    HashSet<string> set = new HashSet<string>();
    //from Left- compare column wise
    for (int j = 0; j < nc; j++)
    {
        for (int i = 0; i < nr; i++)
        {
            if (j == 0 || arr[i, j] > maxFromLeft[i])
            {
                maxFromLeft[i] = arr[i, j];
                updateCount(i, j, set, ref count);
            }
        }
    }

    //from top
    for (int i = 0; i < nr; i++)
    {
        for (int j = 0; j < nc; j++)
        {
            if (i == 0 || arr[i, j] > maxFromTop[j])
            {
                maxFromTop[j] = arr[i, j];
                updateCount(i, j, set, ref count);
            }
        }
    }

    //from right
    for (int j = nc - 1; j >= 0; j--)
    {
        for (int i = 0; i < nr; i++)
        {

            if (j == nc - 1 || arr[i, j] > maxFromRight[i])
            {
                maxFromRight[i] = arr[i, j];
                updateCount(i, j, set, ref count);
            }
        }
    }

    //from bottom
    for (int i = nr - 1; i >= 0; i--)
    {
        for (int j = 0; j < nc; j++)
        {
            //from bottom
            if (i == nr - 1 || arr[i, j] > maxFromBottom[j])
            {
                maxFromBottom[j] = arr[i, j];
                updateCount(i, j, set, ref count);
            }
        }
    }
    return count;
}

void updateCount(int i, int j, HashSet<string> set, ref int count)
{
    if (!set.Contains(i + ":" + j))
    {
        // Console.WriteLine(i+":"+j);
        set.Add(i + ":" + j);
        count++;
    }
}


//highest scenic score
//for every tree go up, down, bottom, left - not recursively though
Int64 part2(int[,] arr){
    int nr = arr.GetLength(0);
    int nc = arr.GetLength(1);

    Int64 maxScore = Int64.MinValue;
    for(int i=0; i<nr; i++){
        for(int j=0; j<nc; j++){
            Int64 left = GetLeft(arr, i, j);
            Int64 right = GetRight(arr, i, j);
            Int64 bottom = GetBottom(arr, i, j);
            Int64 top = GetTop(arr, i, j);

            Int64 score = left*right*bottom*top;
            Console.WriteLine(i+":"+j+" ("+arr[i,j]+") - "+top+":"+left+":"+right+":"+bottom+" score: "+score);
            maxScore = Math.Max(maxScore, score);
        }
    }
    return maxScore;
}


Int64 GetLeft(int[,] arr, int i, int j){
    if(j==0){
        return 0;
    }
    int dist = 1;
    while(j-dist>0 && arr[i,j] > arr[i,j-dist]){
        dist++;
    }
    return dist;
}

Int64 GetRight(int[,] arr, int i, int j){
    int nc = arr.GetLength(1);
    if(j==nc-1){
        return 0;
    }
    int dist = 1;
    while(j+dist<nc-1 && arr[i,j] > arr[i,j+dist]){
        dist++;
    }
    return dist;
}

Int64 GetTop(int[,] arr, int i, int j){
    if(i==0){
        return 0;
    }
    int dist = 1;
    while(i-dist>0 && arr[i,j] > arr[i-dist,j]){
        dist++;
    }
    return dist;
}

Int64 GetBottom(int[,] arr, int i, int j){
    int nr = arr.GetLength(0);
    if(i==nr-1){
        return 0;
    }
    int dist = 1;
    while(i+dist<nr-1 && arr[i,j] > arr[i+dist,j]){
        dist++;
    }
    return dist;
}
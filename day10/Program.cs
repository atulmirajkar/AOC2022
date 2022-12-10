using Util;
/*
    X 1
    2 instr
    addx V - 2 cycles, C cab be -ive
    noop - 1 cycle
    |
        |
            |
                | 4 |
                        |
                         -1 |
    1   2   3   4   5   6   7

    Signal Strengths
    20
    60
    100 * X
    Find sum upto 220th

    Algo:
    if signalstrenght
    if noop
    if addx
    cycle counter

*/
List<string> inputList = FileUtil.ReadFile("./data.txt");
Console.WriteLine(part1(inputList));
Console.WriteLine("/nPart2");
part2(inputList);

int part1(List<string> inputList){
    int cycle=0;
    int x = 1;
    int ss = 0;
    int interCycleCount = 0;

    foreach(var command in inputList){
        string[] commandArr = command.Split(' ');
        string instr = commandArr[0];
        int? addToX = null;
        if(commandArr.Length>1){
            addToX = Convert.ToInt32(commandArr[1]);
        }
        bool isAdd = false;
        switch(instr){
            case "noop":
                interCycleCount = 1;;
                break;
            case "addx":
                
                interCycleCount = 2;
                isAdd = true;
                break;
        }

        for(int i=0; i<interCycleCount; i++){
            cycle++;
            if(cycle == 20 || ((cycle-20) %40 == 0)){
                ss += cycle * x;
                Console.WriteLine($"Cycle: {cycle}, X: {x}, ss: {ss}");
            }
        }
        if(isAdd){
            if(addToX==null){
                    throw new ArgumentException();
                }
            x+=(int)addToX;
        }
    }
    return ss;
}


/*
    Part 2
    X - horizontal position
    sprite  - 3 px wide
    X sets horizontal position of the middle of the sprite

    CRT: 40 wide and 6 high
    0   1   ... 39

    if the sprite is positioned such that one of its three pixels is the pixed currently being drawn - # else .

    addx 15
    x=1

    ###
    ###

    ##

    x = 1, sprite position 0,1,2
*/


void part2(List<string> inputList){
    int cycle=0;
    int x = 1;
    int interCycleCount = 0;
    char[,]? crt = null;
    foreach(var command in inputList){
        string[] commandArr = command.Split(' ');
        string instr = commandArr[0];
        int? addToX = null;
        if(commandArr.Length>1){
            addToX = Convert.ToInt32(commandArr[1]);
        }
        bool isAdd = false;
        switch(instr){
            case "noop":
                interCycleCount = 1;;
                break;
            case "addx":
                
                interCycleCount = 2;
                isAdd = true;
                break;
        }

        for(int i=0; i<interCycleCount; i++){
            cycle++;
            var crtPos = cycleToCRTPos(cycle);
            char pixelVal = '.';
            if(isSpriteOnPixel(crtPos, x)){
               pixelVal = '#';
            }

            //Console.WriteLine(cycle);
            if(cycle%240==1){
                PrintCRT(crt);
                crt = new char[6,40];
            }

            crt![crtPos.Item1%6,crtPos.Item2] = pixelVal;
        }
        if(isAdd){
            if(addToX==null){
                    throw new ArgumentException();
                }
            x+=(int)addToX;
        }
    }
    PrintCRT(crt);
}

void PrintCRT(char[,]? crt){
    if(crt==null){
        return;
    }
    Console.WriteLine("");

    for(int i=0; i<crt.GetLength(0); i++){
        for(int j=0; j<crt.GetLength(1); j++){
            Console.Write(crt[i,j]);
        }
        Console.WriteLine();
    }
}



bool isSpriteOnPixel(Tuple<int, int> crtPos, int x){

    int currPixel = (crtPos.Item1*40) + crtPos.Item2; //0 based

    if(currPixel%40>=x-1 && currPixel%40<=x+1){
        return true;
    }
    return false;
}

//0 based
//can return row > 6
Tuple<int, int> cycleToCRTPos(int cycle){
    int row = (cycle-1) / 40;
    int col = ((cycle-1) % 40);
    return Tuple.Create(row, col);
}


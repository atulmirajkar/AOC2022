using Util;


/*
r>s
s>p
p>r

a   x   r
b   y   p
c   z   s

r 1
p 2
s 3

3 draw
6 win

my  op
r   r
r   p   lose
r   s   win

p   r   win
p   p   
p   s   lose

s   r   lose
s   p   win
s   s   


part 2
X need to lose
Y need to draw
z need to win

A   Y
r   r 
*/

List<string> inputList = FileUtil.ReadFile("./data.txt");
int score = 0;
foreach(string input in inputList){
    string[] playArr = input.Split(' ');
    score += getPlayScore(playArr[0], playArr[1]);
}
Console.WriteLine(score);

int getPlayScore(string opPlay, string myPlay){
    char op = getOpMapping(opPlay);
    //char my = getMyMapping(myPlay); //part 1
    char my = getMyPlayByGuide(op, myPlay);
    int score = getScoreMapping(my);
    score += getScoreForSinglePlay(op, my);
    return score;
}
char getMyPlayByGuide(char opPlay, string myPlay){
    //need to draw - return the same 
    if(myPlay == "Y")
    {
        return opPlay;
    }

    //need to lose
    if(myPlay == "X"){
        if(opPlay == 'p'){
            return 'r';
        } else if(opPlay == 's'){
            return 'p';
        } else if(opPlay == 'r'){
            return 's';
        }
    }

    //need to win
    if(myPlay == "Z"){
        if(opPlay == 's'){
            return 'r';
        }else if (opPlay == 'r'){
            return 'p';
        } else if( opPlay == 'p'){
            return 's';
        }
    }
    Console.WriteLine("Something went wrong");
    return 'k';
}
int getScoreForSinglePlay(char opPlay, char myPlay){
    if(opPlay == myPlay){
        return 3;
    }

    if((myPlay == 'r' && opPlay =='s') ||(myPlay == 's' && opPlay =='p') ||(myPlay == 'p' && opPlay =='r')){
        return 6;
    }

    return 0;

}

int getScoreMapping(char play){
    if(play == 'r'){
        return 1;
    } else if(play == 'p'){
        return 2;
    } else if(play == 's'){
        return 3;
    }
    return 0;
}
//part 1
// char getMyMapping(string opPlay){
//     if(opPlay == "X"){
//         return 'r';
//     } else if(opPlay == "Y"){
//         return 'p';
//     } else if(opPlay == "Z"){
//         return 's';
//     }

//     Console.WriteLine("Something went wrong");
//     return 'z';
// }

char getOpMapping(string opPlay){
    if(opPlay == "A"){
        return 'r';
    } else if(opPlay == "B"){
        return 'p';
    } else if(opPlay == "C"){
        return 's';
    }
    return 'z';
}
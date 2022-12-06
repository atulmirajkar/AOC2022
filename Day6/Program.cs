using Util;

List<string> inputList = FileUtil.ReadFile("./data.txt");
string input = inputList[0];
//sliding window

int left =0;
int right =0;
HashSet<char> set = new HashSet<char>();
bool found = false;
for(; right<input.Length; right++){    
    while(set.Contains(input[right])){
        set.Remove(input[left]);
        left++;
    }

    set.Add(input[right]);
    //part 1 - 4
    if((right-left+1)>=14){
        found = true;
        break;
    }
}

if(!found){
    Console.WriteLine("no window found");
}

Console.WriteLine(right+1);


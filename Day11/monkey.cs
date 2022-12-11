namespace monkeybusiness;

public class Monkey{
    public Queue<UInt64> itemQueue{get; set;}
    public char operation{get; set;} 
    public UInt64 operationVal{get; set;}
    public bool useSelf{get; set;} 
    public UInt64 testVal{get; set;}
    public int trueVal{get; set;}
    public int falseVal{get; set;}

    public int inspectedCount{get; set;}

    public Monkey(){
        itemQueue = new Queue<UInt64>();
    }

}

public class MonkeyCollection{
    public List<Monkey> monkeyList = new List<Monkey>();
    /*

    */
    public void doRound(){
        foreach(Monkey m in monkeyList){
            while(m.itemQueue.Count>0){
                UInt64 wl = m.itemQueue.Dequeue();
                UInt64 ans=0;
                if(m.useSelf){
                    ans = doOperation(wl,wl, m.operation);
                }
                else{
                    ans = doOperation(wl,m.operationVal, m.operation);
                }

                //divide by 3
                //ans = (int)ans/3;

                if(ans%m.testVal==0){
                    monkeyList[m.trueVal].itemQueue.Enqueue(ans);
                }else{
                    monkeyList[m.falseVal].itemQueue.Enqueue(ans);
                }

                //increment inspect count
                m.inspectedCount++;
            }

        }
    }

    public void PrintQueues(){
        Console.WriteLine("");
        for(int i=0; i<this.monkeyList.Count;i++){
            Console.WriteLine($"Monkey: {i} inspected items {this.monkeyList[i].inspectedCount} times");
            foreach(var item in monkeyList[i].itemQueue){
                Console.Write($"{item}\t");
            }
            Console.WriteLine();
        }
    }

    public UInt64 doOperation(UInt64 leftOp, UInt64 rightOp, char op){
        if(op=='+'){
            return leftOp+rightOp;
        }
        if(op=='*'){
            return leftOp*rightOp;
        }
        throw new ArgumentException();
    }
}
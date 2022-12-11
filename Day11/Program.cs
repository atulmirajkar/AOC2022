
namespace monkeybusiness;

public static class Program
{
    public static void Main()
    {
        MonkeyCollection mc = getTestData();
        for (int i = 0; i < 10_000; i++)
        {
            Console.WriteLine($"Round:{i+1}");
            mc.doRound();
            mc.PrintQueues();
        }

        List<int> mbList = new List<int>();
        for (int i = 0; i < mc.monkeyList.Count; i++)
        {
            mbList.Add(mc.monkeyList[i].inspectedCount);
        }

        mbList.Sort(Comparer<int>.Create((a, b) => b.CompareTo(a)));
        Int64 result =(Int64)mbList[0]*mbList[1];
        Console.WriteLine(result);

    }
    public static MonkeyCollection getData()
    {
        Monkey m0 = new Monkey();
        m0.itemQueue.Enqueue(61);
        m0.operation = '*';
        m0.operationVal = 11;
        m0.testVal = 5;
        m0.trueVal = 7;
        m0.falseVal = 4;

        Monkey m1 = new Monkey();
        m1.itemQueue.Enqueue(76);
        m1.itemQueue.Enqueue(92);
        m1.itemQueue.Enqueue(53);
        m1.itemQueue.Enqueue(93);
        m1.itemQueue.Enqueue(79);
        m1.itemQueue.Enqueue(86);
        m1.itemQueue.Enqueue(81);
        m1.operation = '+';
        m1.operationVal = 4;
        m1.testVal = 2;
        m1.trueVal = 2;
        m1.falseVal = 6;

        Monkey m2 = new Monkey();
        m2.itemQueue.Enqueue(91);
        m2.itemQueue.Enqueue(99);
        m2.operation = '*';
        m2.operationVal = 19;
        m2.testVal = 13;
        m2.trueVal = 5;
        m2.falseVal = 0;

        Monkey m3 = new Monkey();
        m3.itemQueue.Enqueue(58);
        m3.itemQueue.Enqueue(67);
        m3.itemQueue.Enqueue(66);
        m3.operation = '*';
        m3.operationVal = 0;
        m3.useSelf = true;
        m3.testVal = 7;
        m3.trueVal = 6;
        m3.falseVal = 1;

        Monkey m4 = new Monkey();
        m4.itemQueue.Enqueue(94);
        m4.itemQueue.Enqueue(54);
        m4.itemQueue.Enqueue(62);
        m4.itemQueue.Enqueue(73);
        m4.operation = '+';
        m4.operationVal = 1;
        m4.testVal = 19;
        m4.trueVal = 3;
        m4.falseVal = 7;

        Monkey m5 = new Monkey();
        m5.itemQueue.Enqueue(59);
        m5.itemQueue.Enqueue(95);
        m5.itemQueue.Enqueue(51);
        m5.itemQueue.Enqueue(58);
        m5.itemQueue.Enqueue(58);
        m5.operation = '+';
        m5.operationVal = 3;
        m5.testVal = 11;
        m5.trueVal = 0;
        m5.falseVal = 4;

        Monkey m6 = new Monkey();
        m6.itemQueue.Enqueue(87);
        m6.itemQueue.Enqueue(69);
        m6.itemQueue.Enqueue(92);
        m6.itemQueue.Enqueue(56);
        m6.itemQueue.Enqueue(91);
        m6.itemQueue.Enqueue(93);
        m6.itemQueue.Enqueue(88);
        m6.itemQueue.Enqueue(73);
        m6.operation = '+';
        m6.operationVal = 8;
        m6.testVal = 3;
        m6.trueVal = 5;
        m6.falseVal = 2;

        Monkey m7 = new Monkey();
        m7.itemQueue.Enqueue(71);
        m7.itemQueue.Enqueue(57);
        m7.itemQueue.Enqueue(86);
        m7.itemQueue.Enqueue(67);
        m7.itemQueue.Enqueue(96);
        m7.itemQueue.Enqueue(95);
        m7.operation = '+';
        m7.operationVal = 7;
        m7.testVal = 17;
        m7.trueVal = 3;
        m7.falseVal = 1;

        MonkeyCollection mc = new MonkeyCollection();
        mc.monkeyList.Add(m0);
        mc.monkeyList.Add(m1);
        mc.monkeyList.Add(m2);
        mc.monkeyList.Add(m3);

        mc.monkeyList.Add(m4);
        mc.monkeyList.Add(m5);
        mc.monkeyList.Add(m6);
        mc.monkeyList.Add(m7);

        return mc;
    }

    public static MonkeyCollection getTestData()
    {
        Monkey m0 = new Monkey();
        m0.itemQueue.Enqueue(79);
        m0.itemQueue.Enqueue(98);
        m0.operation = '*';
        m0.operationVal = 19;
        m0.testVal = 23;
        m0.trueVal = 2;
        m0.falseVal = 3;

        Monkey m1 = new Monkey();
        m1.itemQueue.Enqueue(54);
        m1.itemQueue.Enqueue(65);
        m1.itemQueue.Enqueue(75);
        m1.itemQueue.Enqueue(74);
        m1.operation = '+';
        m1.operationVal = 6;
        m1.testVal = 19;
        m1.trueVal = 2;
        m1.falseVal = 0;

        Monkey m2 = new Monkey();
        m2.itemQueue.Enqueue(79);
        m2.itemQueue.Enqueue(60);
        m2.itemQueue.Enqueue(97);
        m2.operation = '*';
        m2.operationVal = 0;//
        m2.useSelf = true;
        m2.testVal = 13;
        m2.trueVal = 1;
        m2.falseVal = 3;

        Monkey m3 = new Monkey();
        m3.itemQueue.Enqueue(74);
        m3.operation = '+';
        m3.operationVal = 3;
        m3.testVal = 17;
        m3.trueVal = 0;
        m3.falseVal = 1;

        MonkeyCollection mc = new MonkeyCollection();
        mc.monkeyList.Add(m0);
        mc.monkeyList.Add(m1);
        mc.monkeyList.Add(m2);
        mc.monkeyList.Add(m3);

        return mc;
    }
}
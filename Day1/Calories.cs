using System;
using Util;
using System.Collections.Generic;

namespace AOC 
{
    class Calories
    {
        static void Main(string[] args)
        {
            List<string> calList = FileUtil.ReadFile("./data.txt");
            // foreach(string str in testList){
            //     Console.WriteLine(str);
            // }
            //PrintMaxCalories(calList);
            PrintTopNCalories(calList,3);
        }

        static void PrintMaxCalories(List<string> calList){
            if(calList == null || calList.Count == 0 ){
                Console.WriteLine("Data error");
                return;
            }
            int maxIdx = 1;
            int maxCal = int.MinValue;

            int sumCal =0;
            int idx = 0;
            foreach(string cal in calList){
                if(String.IsNullOrEmpty(cal)){
                    if(sumCal > maxCal){
                        maxCal = sumCal;
                        maxIdx = idx;
                    }
                    sumCal =0;        
                    idx++;
                    continue;
                }
                int calVal = int.Parse(cal);
                sumCal += calVal;
            }
            Console.WriteLine(maxIdx+":"+maxCal);
        }

        static void PrintTopNCalories(List<string> calList, int n){
            if(calList == null || calList.Count == 0 ){
                Console.WriteLine("Data error");
                return;
            }

            //min heap of size n
            PriorityQueue<int, int> pq = new PriorityQueue<int, int>();
            int sumCal =0;
            int idx = 0;
            foreach(string cal in calList){
                if(String.IsNullOrEmpty(cal)){
                    updatePQ(pq,sumCal,n);
                    sumCal =0;        
                    idx++;
                    continue;
                }
                int calVal = int.Parse(cal);
                sumCal += calVal;
            }
            updatePQ(pq,sumCal,n);

            int nSum = 0;
            while(pq.Count>0){
                int val = pq.Dequeue();
                Console.WriteLine(val);
                nSum += val;
            }
            Console.WriteLine("total sum:" + nSum);
        }

        static void updatePQ(PriorityQueue<int, int> pq, int sumCal, int n){
            if(pq.Count == 0){
                pq.Enqueue(sumCal, sumCal);
                return;
            }
            if(sumCal>pq.Peek()){
                pq.Enqueue(sumCal, sumCal);
                if(pq.Count>n){
                    pq.Dequeue();
                }
            }
        }
    }

}

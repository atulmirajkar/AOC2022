using System;
using System.IO;
using System.Collections.Generic;

namespace Util
{
    public static class FileUtil
    {
        public static List<string>  ReadFile(string filePath){
            List<string> strList = null;
            try{
                using (var sr = new StreamReader(filePath)){
                    while(sr.Peek()>=0){
                        if(strList == null){
                            strList = new List<string>();
                        }
                        strList.Add(sr.ReadLine());
                    }
                }
            } catch(Exception e){
                Console.Write(e.ToString());
            }
            return strList;
        }

        public static async IAsyncEnumerable<string> ReadFileLineAsync(string filePath){
            using(var sr= File.OpenText(filePath)){
                string s = null;
                while((s = await sr.ReadLineAsync()) !=null){
                    yield return s;
                }
            }
        }
    }
}

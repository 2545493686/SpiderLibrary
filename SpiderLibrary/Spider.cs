using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanQ.HttpLibrary;
using LanQ;

namespace LanQ.SpiderLibrary
{
    public struct Pattern
    {
        public string pattern;
        public string startTrim;
        public string endTrim;
    }

    public class Spider
    {
        
        public string[] UrlList { get; }
        protected TextFile TextFile { get; }

        public Spider(string[] url, TextFile textFile = null)
        {
            UrlList = url;
            TextFile = textFile;
        }

        public List<string[]>[] GetText(Pattern[] patternes, int sleep = 0, bool logInConsole = true)
        {
            if (logInConsole)
                Console.WriteLine("Spider Start!");

            List<string[]>[] ret = new List<string[]>[UrlList.Length];

            for (int i = 0; i < UrlList.Length; i++)
            {
                if (logInConsole)
                    Console.WriteLine("Spider[{0}/{1}]: {2}", i, UrlList.Length, UrlList[i]);

                string source = Http.Get(UrlList[i], sleep); //获得当前url的源
                ret[i] = new List<string[]>();

                List<MatchCollection> matches = new List<MatchCollection>();

                foreach (Pattern pattern in patternes)
                {
                    matches.Add(Regex.Matches(source, pattern.pattern)); //获得匹配项
                }

                int count = matches.Count > 0 ? matches[0].Count : 0;
                for (int j = 0; j < count; j++)
                {
                    ret[i].Add(new string[matches.Count]);
                    for (int k = 0; k < matches.Count; k++)
                    {
                        
                        ret[i][j][k] = matches[k][j].Value.Trim(patternes[k].startTrim, patternes[k].endTrim);

                        if (TextFile != null) //文本属性存在时写入文件
                        {
                            TextFile.WriteLine(matches[k][j].Value.Trim(patternes[k].startTrim, patternes[k].endTrim));
                        }
                    }
                }

                if (logInConsole)
                    Console.WriteLine("Spider[{0}]: success get {1}", i, count);
            }

            if (logInConsole)
                Console.WriteLine("Spider End!");

            return ret;
        }
    }
}

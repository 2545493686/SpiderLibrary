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
    public class Spider
    {
        public string[] UrlList { get; }
        protected TextFile TextFile { get; }

        public Spider(string[] url, TextFile textFile = null)
        {
            UrlList = url;
            TextFile = textFile;
        }

        public List<string>[] GetText(string[] regexPatternes)
        {
            List<string>[] ret = new List<string>[regexPatternes.Length];

            for (int i = 0; i < regexPatternes.Length; i++)
            {
                string source = Http.Get(UrlList[i]);
                MatchCollection matches = Regex.Matches(source, regexPatternes[i]);

                ret[i] = new List<string>();

                foreach (Match item in matches)
                {
                    if (TextFile == null) //文本属性存在时写入文件
                    {
                        TextFile.WriteLine(item.Value);
                    }

                    ret[i].Add(item.Value);
                }
            }

            return ret;
        }
    }
}

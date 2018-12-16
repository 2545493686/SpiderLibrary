using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanQ.HttpLibrary;

namespace LanQ.SpiderLibrary
{
    public class Spider
    {
        public string[] UrlList { get; }
        public Spider(string[] url)
        {
            UrlList = url;
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
                    ret[i].Add(item.Value);
                }
            }

            return ret;
        }
    }
}

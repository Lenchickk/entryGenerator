using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using HtmlAgilityPack;

namespace entryPointsGenerator
{

    public static class EntryGate
    {

        public static void LoadInitials(String file)
        {
            String str = "";
            char[] delimiterLast = { ';' };
            char[] delimiterHTML = { '"' };
            char[] delimiterA = { '/', '\"', '—' };
            String[] input;

            List<String> domains;
            String lookupage = "";

            StreamReader sr = new StreamReader(file);
            CommonPlace.entrypagesTable = CommonPlace.CreateDataTable();
            sr.ReadLine();

            while (((str = sr.ReadLine()) != null) && (str != ""))
            {
                input = str.Split(delimiterLast);
                Int32 group_ID = Int32.Parse(input[0]);
                string group_Name = input[1];
                string domain = input[2];
                string name = input[3];
                string fullname = input[4];
                //Int32 depth = Int32.Parse(input[5]);
                lookupage = CommonPlace.LookUpPage(domain, name);
                //DateTime created = CommonPlace.ReturnCreationDate(domain, name);
                CommonPlace.entrypagesTable.Rows.Add(name.GetHashCode(), group_ID, group_Name, domain, name, fullname, "", "", 0, 0, 0, "", "", "");//,created.ToString("MMddyyyy"));

                //if (CommonPlace.visited.ContainsKey(name))
                //{
                //  CommonPlace.visited[name]++;
                // continue;
                //}

                //CommonPlace.visited.Add(name, 1);


                domains = new List<string>();


                for (int i = 5; i < input.Length; i++)
                {
                    domains.Add(input[i]);
                }

                lookupage = CommonPlace.LookUpPage(domain, name);
                var Webget = new HtmlWeb();
                var doc = Webget.Load(lookupage);

                HtmlNodeCollection htmlcol = doc.DocumentNode.SelectNodes("//div[@id='p-lang']");
                HtmlDocument doc2 = new HtmlDocument();
                doc2.LoadHtml(htmlcol[0].InnerHtml);

                CommonPlace.visited.AddZero(domain, name);

                foreach (HtmlNode node in doc2.DocumentNode.SelectNodes("//a"))
                {
                    String outerhtml = node.OuterHtml;
                    if (outerhtml.IndexOf("hreflang") < 0) continue;
                    String buf = outerhtml.Substring(outerhtml.IndexOf("hreflang"), 15);
                    String[] bu = buf.Split(delimiterHTML);


                    if (domains.Contains(bu[1]))
                    {
                        String buff = outerhtml.Substring(outerhtml.IndexOf("/wiki/"), outerhtml.Length - outerhtml.IndexOf("/wiki/"));
                        String[] buu = buff.Split(delimiterA); ;
                        //created = CommonPlace.ReturnCreationDate(bu[1], buu[2]);
                        CommonPlace.entrypagesTable.Rows.Add(buu[2].GetHashCode(), group_ID, group_Name, bu[1], buu[2], buu[4], "", "", 0, 0, 0, "", "", "");//, created.ToString("MMddyyyy"));
                        if (!CommonPlace.IsInPair(name, buu[2])) CommonPlace.domainPair.Add(new DomainPair(domain, bu[1], name, buu[2]));
                        CommonPlace.visited.AddZero(bu[1], buu[2]);
                        //CommonPlace.visited.Add(buu[2],1);
                    }

                }

            }

            sr.Close();
        }



    }
}

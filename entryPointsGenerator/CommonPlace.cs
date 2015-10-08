using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Net;
using HtmlAgilityPack;


namespace entryPointsGenerator
{
    static public class CommonPlace
    {

        public static bool IsInPair(String str1, String str2)
        {
            foreach (DomainPair dp in domainPair)
            {
                Boolean b1 = (dp.page1 == str1 && dp.page2 == str2);
                Boolean b2 = (dp.page1 == str2 && dp.page2 == str1);
                if (b1 || b2) return true;
            }
            return false;
        }

        static public Int16 Depth;
        static public void Activate(Int16 d)
        {
            CommonPlace.Depth = d;
            entryEdgesTable = CreateEdgesTable();
            entryVerticesTable = CreateVerticesTable();
            CreateSeeAlsoDict();
            CreateSeeAlsoDict2();
            nodes = new PairVisited();
            domainPair = new List<DomainPair>();
        }
        static public String LookUpPage(String domain, String name)
        {
            return (@"http://" + domain + ".wikipedia.org/wiki/" + name);
        }

        public static DataTable entryEdgesTable;
        public static DataTable entryVerticesTable;
        public static Dictionary<String, String> seeAlso;
        public static Dictionary<String, String> seeAlso2;
        public static PairVisited nodes;
        public static List<DomainPair> domainPair;

        static public DataTable CreateEdgesTable()
        {
            DataTable entrypagesTable = new DataTable();
            entrypagesTable.Columns.Add("ID", typeof(int));
            entrypagesTable.Columns.Add("childID", typeof(int));
            entrypagesTable.Columns.Add("groupID", typeof(int));
            entrypagesTable.Columns.Add("groupName", typeof(string));
            entrypagesTable.Columns.Add("domain", typeof(string));
            entrypagesTable.Columns.Add("name", typeof(string));
            entrypagesTable.Columns.Add("fullname", typeof(string));
            entrypagesTable.Columns.Add("parentname", typeof(string));
            entrypagesTable.Columns.Add("parentfullname", typeof(string));
            entrypagesTable.Columns.Add("parentID", typeof(int));
  
            return entrypagesTable;
        }

        static public DataTable CreateVerticesTable()
        {
            DataTable entrypagesTable = new DataTable();
            entrypagesTable.Columns.Add("ID", typeof(int));
            entrypagesTable.Columns.Add("groupID", typeof(int));
            entrypagesTable.Columns.Add("groupName", typeof(string));
            entrypagesTable.Columns.Add("domain", typeof(string));
            entrypagesTable.Columns.Add("name", typeof(string));
            entrypagesTable.Columns.Add("fullname", typeof(string));
            entrypagesTable.Columns.Add("depth", typeof(int));
            entrypagesTable.Columns.Add("weight", typeof(int));
            entrypagesTable.Columns.Add("ru", typeof(string));
            entrypagesTable.Columns.Add("en", typeof(string));
            entrypagesTable.Columns.Add("uk", typeof(string));
            entrypagesTable.Columns.Add("cyear", typeof(int));
            entrypagesTable.Columns.Add("cday", typeof(int));
            entrypagesTable.Columns.Add("cmonth", typeof(int));
            entrypagesTable.Columns.Add("time", typeof(string));
            return entrypagesTable;
        }

        public static void CreateSeeAlsoDict()
        {
            seeAlso = new Dictionary<string, string>();

            seeAlso.Add("en", "See also");
            seeAlso.Add("ru", "См. также");
            seeAlso.Add("uk", "Див. також");
            seeAlso.Add("de", "Siehe auch");
            seeAlso.Add("sv", "Se även");
        }

        public static void CreateSeeAlsoDict2()
        {
            seeAlso2 = new Dictionary<string, string>();

            seeAlso2.Add("en", "See_also");
            seeAlso2.Add("ru", ".D0.A1.D0.BC._.D1.82.D0.B0.D0.BA.D0.B6.D0.B5");
            seeAlso2.Add("uk", ".D0.94.D0.B8.D0.B2._.D1.82.D0.B0.D0.BA.D0.BE.D0.B6");
            //seeAlso.Add("de", "Siehe auch");
            //seeAlso.Add("sv", "Se_.C3.A4ven");
        }

        public static DataTable CopyEntityTable()
        {
            return entryVerticesTable.Copy();
        }

        public static void TableToFile(String file)
        {
            String file2 = file + "Vertices.csv";
            String file1 = file + "Edges_in.csv";
            StreamWriter sw;
            UpdateRows();

            if (File.Exists(file1))
            {
                File.Delete(file1);
            }
            if (false)
            {
                sw = new StreamWriter(file1, false);
            }
            else
            {
                sw = new StreamWriter(file1, true, System.Text.Encoding.UTF8);
                sw.WriteLine("ID\tgroupID\tgroupName\tdomain\tname\tfullname\tdepth\tweight\trunode\tennode\tuknode\tyear\tday\tmonth\ttime");
               
            }

            foreach (DataRow r in entryVerticesTable.Rows)
            {

                sw.WriteLine(r["ID"] + "\t" +  r["groupID"] + "\t" + r["groupName"] + "\t" + r["domain"] + "\t" + r["name"]
                        + "\t" + r["fullname"] + "\t" + r["depth"] + "\t" + r["weight"] + "\t" + r["ru"] + "\t" + r["en"] + "\t" + r["uk"] + "\t" + r["cyear"] +
                      "\t" + r["cday"] + "\t" + r["cmonth"] + "\t" + r["time"]);
            }


     


            sw.Close();

            if (File.Exists(file2))
            {
                File.Delete(file2);
            }
            if (false)
            {
                sw = new StreamWriter(file2, false);
            }
            else
            {
                sw = new StreamWriter(file2, true, System.Text.Encoding.UTF8);
                sw.WriteLine("ID\tchildID\tgroupID\tgroupName\tname\tparentname\tdomain\tparentfullname\tfullname\tparentID");
            }

            foreach (DataRow r in entryEdgesTable.Rows)
            {
                sw.WriteLine(r["ID"] + "\t" + r["childID"] + "\t" + r["groupID"] + "\t" + r["groupName"] + "\t"  + "\t" + r["name"]
                   + "\t" + r["parentname"] + "\t" + r["domain"] + "\t" + r["parentfullname"] + "\t" + r["fullname"]  + "\t" + r["parentID"]);
             }



        }
        static public DateTime ReturnCreationDate(string domain, string name)
        {
            DateTime buf = new DateTime();
            String page = "http://" + domain + ".wikipedia.org/w/index.php?title=" + name + "&offset=&limit=50000000&action=history";

            if (page.IndexOf("#") > -1)
            {
                int p = page.IndexOf("#");
                String help = page.Substring(p);
                int p2 = help.IndexOf("&");
                help = help.Substring(p2);
                page = page.Substring(0, p);
                page = page + help;
            }

            HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(CommonPlace.GetWebData(page));

            HtmlNodeCollection nodes = htmldoc.DocumentNode.SelectNodes("//a[@class='mw-changeslist-date']");
            HtmlNode node = nodes[nodes.Count - 1];
            buf = RevisionItem.GetRevisionItem(node.ChildNodes[0].InnerHtml, domain);

            return buf;
        }
        public static void UpdateRows()
        {
            foreach (DataRow r in entryVerticesTable.Rows)
            {
    
                r["weight"] = CommonPlace.nodes.GetWeight(r["domain"].ToString(), r["name"].ToString());
    

                foreach (DomainPair dp in domainPair)
                {
                    if (dp.lang1 == dp.lang2) continue;
                    if (dp.page1 == r["name"].ToString())
                    {
                        foreach (DataRow rr in entryVerticesTable.Rows)
                        {
                            if (r == rr) continue;
                            if (dp.page2 == rr["name"].ToString())
                            {
                                r[dp.lang2] = dp.page2.GetHashCode();
                                continue;
                            }
                        }

                    }
                    if (dp.page2 == r["name"].ToString())
                    {
                        foreach (DataRow rr in entryVerticesTable.Rows)
                        {
                            if (r == rr) continue;
                            if (dp.page1 == rr["name"].ToString())
                            {
                                r[dp.lang1] = dp.page1.GetHashCode();
                                continue;
                            }
                        }

                    }
                }
            }
        }
        public static string GetWebData3(string url)
        {
            string html = string.Empty;
            using (WebClient client = new WebClient())
            {
                Uri innUri = null;
                Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out innUri);

                try
                {
                    //client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    //using (StreamReader str = new StreamReader(client.OpenRead(innUri)))
                    client.Encoding = System.Text.Encoding.UTF8;

                    html = client.DownloadString(url);
                }
                catch (WebException we)
                {
                    //throw we;"
                    Console.WriteLine(we.ToString());
                    Console.ReadKey();

                }
                return html;
            }
        }

        public static string GetWebData(string url)
        {
            string html = string.Empty;
            using (WebClient client = new WebClient())
            {
                Uri innUri = null;
                Uri.TryCreate(url, UriKind.Absolute, out innUri);


                try
                {
                    //client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    //using (StreamReader str = new StreamReader(client.OpenRead(innUri)))

                    using (StreamReader str = new StreamReader(client.OpenRead(innUri)))
                    {
                        html = str.ReadToEnd();
                    }
                }
                catch (WebException we)
                {
                    //throw we;"
                    Console.WriteLine(we.ToString());

                }
                return html;
            }
        }



        public static string ReturnLangLink(String domain, String lookupage)
        {
            char[] delimiterHTML = { '"' };
            char[] delimiterA = { '/', '\"', '—' };

            var Webget = new HtmlWeb();
            var doc = Webget.Load(lookupage);

            HtmlNodeCollection htmlcol = doc.DocumentNode.SelectNodes("//div[@id='p-lang']");
            if (htmlcol == null) return "";

            HtmlDocument doc2 = new HtmlDocument();
            doc2.LoadHtml(htmlcol[0].InnerHtml);


            foreach (HtmlNode node in doc2.DocumentNode.SelectNodes("//a"))
            {
                String outerhtml = node.OuterHtml;
                if (outerhtml.IndexOf("hreflang") < 0) continue;
                String buf = outerhtml.Substring(outerhtml.IndexOf("hreflang"), 15);
                String[] bu = buf.Split(delimiterHTML);

                if (domain == bu[1])
                {
                    String buff = outerhtml.Substring(outerhtml.IndexOf("/wiki/"), outerhtml.Length - outerhtml.IndexOf("/wiki/"));
                    String[] buu = buff.Split(delimiterA); ;

                    //CommonPlace.entrypagesTable.Rows.Add(buu[2].GetHashCode(), group_ID, group_Name, bu[1], buu[2], buu[4], "", "", 0, 0, "", "", "");
                    //CommonPlace.visited.Add(buu[2],1);
                    return buu[2];
                }

            }
            return "";
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HtmlAgilityPack;
using System.Net;


namespace entryPointsGenerator
{
    static class Digger
    {
        static public void Wraper()
        {
            DataTable temp = CommonPlace.CopyEntityTable();
            Extractor(temp, CommonPlace.Depth, 1, true);


        }

        static public void Extractor(DataTable table, Int32 depth, Int32 current, Boolean top)
        {
            List<String> l = new List<string>();
            l.Add("ru");
            l.Add("en");
            l.Add("uk");

            foreach (DataRow r in table.Rows)
            {

                if (top)
                {
                    foreach (String la in l)
                    {
                        if (la == r["domain"].ToString()) continue;
                        String lookup1 = CommonPlace.LookUpPage(r["domain"].ToString(), r["name"].ToString());
                        String result = CommonPlace.ReturnLangLink(la, lookup1);
                        if (result == "") continue;
                        if (CommonPlace.IsInPair(r["name"].ToString(), result)) continue;
                        CommonPlace.domainPair.Add(new DomainPair(r["domain"].ToString(), la, r["name"].ToString(), result));
                    }
                }

                if (current == depth + 1) continue;


                DataTable local = CommonPlace.CreateVerticesTable();
                String lookup = CommonPlace.LookUpPage(r["domain"].ToString(), r["name"].ToString());
                       
          
                String htmlCode;

                htmlCode = CommonPlace.GetWebData(lookup);

                String tagstring = @"id=""" + CommonPlace.seeAlso2[r["domain"].ToString()] + @"""";
                Int32 spos = htmlCode.IndexOf(tagstring);

                if (spos < 0) continue;

                htmlCode = htmlCode.Substring(htmlCode.IndexOf(tagstring), htmlCode.Length - htmlCode.IndexOf(tagstring));


                HtmlDocument htmldoc = new HtmlDocument();
                htmldoc.LoadHtml(htmlCode);

                HtmlNodeCollection htmlcollection = htmldoc.DocumentNode.SelectNodes("//ul");
                htmlcollection = htmlcollection[0].ChildNodes;

                if (htmlcollection == null) continue;

                foreach (HtmlNode node in htmlcollection)
                {
                    if (node.Name == "li")
                    {
                        String where = node.InnerHtml;
                        String where2 = node.InnerHtml;
                        if (where.Substring(0, 2) != "<a") continue;
                        int pos = where.IndexOf(@"<a href=""/wiki/");
                        if (pos < 0) continue;
                        where = where.Substring(where.IndexOf(@"<a href=""/wiki/"), where.Length - where.IndexOf(@"<a href=""/wiki/"));
                        where = where.Substring(@"<a href=""/wiki/".Length);
                        where2 = where2.Substring(where2.IndexOf(@"title="""), where2.Length - where2.IndexOf(@"title="""));
                        where2 = where2.Substring(@"title=""".Length);
                        char[] c = { '"', ':' };
                        String[] ou = where.Split(c);

                        String[] ou1 = where2.Split(c);

                        if (CommonPlace.edges.ContainsKey(r["domain"].ToString(), r["name"].ToString(), r["domain"].ToString(), ou[0])) continue;

                        CommonPlace.edges.Add(r["domain"].ToString(), r["name"].ToString(), r["domain"].ToString(), ou[0]);

                        CommonPlace.entryEdgesTable.Rows.Add(r["name"].ToString().GetHashCode() + ou[0].GetHashCode(), ou[0].GetHashCode(),
                                    r["groupID"].ToString(), r["groupName"].ToString(), r["domain"].ToString(), ou[0], ou1[0],
                                    r["name"].ToString(), r["fullname"].ToString(), r["name"].ToString().GetHashCode());

                             //add the edge
                        if (CommonPlace.nodes.GetWeight(r["domain"].ToString(), ou[0]) > 0)
                        {
                            CommonPlace.nodes.PlusWeight(r["domain"].ToString(), ou[0]);
                            continue;
                                 
                        }
                        CommonPlace.nodes.Add(r["domain"].ToString(), ou[0]);
                        DateTime created = CommonPlace.ReturnCreationDate(r["domain"].ToString(), ou[0]);
                        CommonPlace.entryVerticesTable.Rows.Add(ou[0].GetHashCode(), r["groupID"], r["groupName"], r["domain"], ou[0], ou1[0], current, 0, "", "", "",
                                                        created.Year, created.Day, created.Month, created.TimeOfDay.ToString());
                         
                        //addlocaltable
                        //addglobalverticestable
    

                        Console.WriteLine(ou1[0]);
                   
                        foreach (String la in l)
                        {
                            if (la == r["domain"].ToString())
                            {
                               
                                continue;
                            }
                            String lookup1 = CommonPlace.LookUpPage(r["domain"].ToString(), ou[0]);
                            String result = CommonPlace.ReturnLangLink(la, lookup1);
                            if (result == "") continue;
                            if (CommonPlace.IsInPair(r["name"].ToString(), result)) continue;
                           
                            CommonPlace.domainPair.Add(new DomainPair(r["domain"].ToString(), la, ou[0], result));
                        }

                        

                        local.Rows.Add(ou[0].GetHashCode(), r["groupID"], r["groupName"], r["domain"], ou[0], ou1[0], current, 0, "", "", "",
                                                            created.Year, created.Day, created.Month, created.TimeOfDay.ToString());
                 
                        Extractor(local, CommonPlace.Depth, current + 1, false);

                    }
                }


            }
        }


    }
}

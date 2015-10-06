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


                DataTable local = CommonPlace.CreateDataTable();
                String lookup = CommonPlace.LookUpPage(r["domain"].ToString(), r["name"].ToString());
                String htmlCode;


                //CookieContainer cookie = new CookieContainer();
                //WebClient client = new WebClient();
                //client.Encoding = System.Text.Encoding.UTF8;


                //client.Headers.Add(HttpRequestHeader.Cookie, "somecookie");
                //htmlCode = client.DownloadString(lookup);
                //client.Dispose();


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
                        Console.WriteLine(ou1[0]);
                        //CommonPlace.domainPair.Add(new DomainPair(r["domain"],));



                        if (CommonPlace.visited.ContainsKey(r["domain"].ToString(), ou[0]))
                        {
                            CommonPlace.visited.PlusWeight(r["domain"].ToString(), ou[0]);
                            continue;
                        }
                        /// skip:
                        CommonPlace.visited.Add(r["domain"].ToString(), ou[0]);


                        foreach (String la in l)
                        {
                            if (la == r["domain"].ToString()) continue;
                            String lookup1 = CommonPlace.LookUpPage(r["domain"].ToString(), ou[0]);
                            String result = CommonPlace.ReturnLangLink(la, lookup1);
                            if (result == "") continue;
                            if (CommonPlace.IsInPair(r["name"].ToString(), result)) continue;

                            CommonPlace.domainPair.Add(new DomainPair(r["domain"].ToString(), la, ou[0], result));
                        }



                        //DateTime created = CommonPlace.ReturnCreationDate(r["domain"].ToString(), ou1[0]);
                        CommonPlace.entrypagesTable.Rows.Add(ou[0].GetHashCode(), r["groupID"], r["groupName"], r["domain"], ou[0], ou1[0], r["name"], r["fullname"], current, r["ID"]);//, created.ToString("MMddyyyy"));



                        local.Rows.Add(ou[0].GetHashCode(), r["groupID"], r["groupName"], r["domain"], ou[0], ou1[0], r["name"], r["fullname"], current, r["ID"]);


                        Extractor(local, CommonPlace.Depth, current + 1, false);

                    }
                }


            }
        }


    }
}

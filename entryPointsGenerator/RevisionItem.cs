using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entryPointsGenerator
{
    public static class RevisionItem
    {
        public static char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

        static public Dictionary<String, Int16> GetDict(string domain)
        {
            Dictionary<String, Int16> month = new Dictionary<string, short>();
            if (domain == "en")
            {
                month.Add("January", 1);
                month.Add("February", 2);
                month.Add("March", 3);
                month.Add("April", 4);
                month.Add("May", 5);
                month.Add("June", 6);
                month.Add("July", 7);
                month.Add("August", 8);
                month.Add("September", 9);
                month.Add("October", 10);
                month.Add("November", 11);
                month.Add("December", 12);
                return month;
            }
            if (domain == "ru")
            {
                month.Add("января", 1);
                month.Add("февраля", 2);
                month.Add("марта", 3);
                month.Add("апреля", 4);
                month.Add("мая", 5);
                month.Add("июня", 6);
                month.Add("июля", 7);
                month.Add("августа", 8);
                month.Add("сентября", 9);
                month.Add("октября", 10);
                month.Add("ноября", 11);
                month.Add("декабря", 12);
                return month;
            }

            if (domain == "uk")
            {
                month.Add("січня", 1);
                month.Add("лютого", 2);
                month.Add("березня", 3);
                month.Add("квітня", 4);
                month.Add("травня", 5);
                month.Add("червня", 6);
                month.Add("липня", 7);
                month.Add("серпня", 8);
                month.Add("вересня", 9);
                month.Add("жовтня", 10);
                month.Add("листопада", 11);
                month.Add("грудня", 12);
                return month;
            }

            if (domain == "de")
            {
                month.Add("Jan", 1);
                month.Add("Feb", 2);
                month.Add("Mär", 3);
                month.Add("Apr", 4);
                month.Add("Mai", 5);
                month.Add("Jun", 6);
                month.Add("Jul", 7);
                month.Add("Aug", 8);
                month.Add("Sep", 9);
                month.Add("Okt", 10);
                month.Add("Nov", 11);
                month.Add("Dez", 12);
                return month;
            }

            if (domain == "sv")
            {
                month.Add("januari", 1);
                month.Add("februari", 2);
                month.Add("mars", 3);
                month.Add("april", 4);
                month.Add("maj", 5);
                month.Add("juni", 6);
                month.Add("juli", 7);
                month.Add("augusti", 8);
                month.Add("september", 9);
                month.Add("oktober", 10);
                month.Add("november", 11);
                month.Add("december", 12);
                return month;
            }


            return month;
        }

        public static DateTime GetRevisionItem(String dt, String domain)
        {
            Dictionary<String, Int16> month = new Dictionary<string, short>();
            month = RevisionItem.GetDict(domain);
            DateTime stamp = new DateTime();
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            String[] inn = dt.Split(delimiterChars);
            List<string> input = new List<string>();
            int hour;
            int minute;
            int day;
            int monthh;
            int year;
            short buf = 0;
            for (int i = 0; i < inn.Length; i++)
            {
                if (Int16.TryParse(inn[i], out buf) || inn[i].Length > 2)
                    input.Add(inn[i]);
            }
            //17:00, 30 April 2015
            if (domain != "sv")
            {
                hour = Int16.Parse(input[0]);
                minute = Int16.Parse(input[1]);
                day = Int16.Parse(input[2]);
                monthh = month[input[3]];
                year = Int16.Parse(input[4]);
            }
            else
            {
                hour = Int16.Parse(input[3]);
                minute = Int16.Parse(input[4]);
                day = Int16.Parse(input[0]);
                monthh = month[input[1]];
                year = Int16.Parse(input[2]);
            }

            stamp = new DateTime(year, monthh, day, hour, minute, 0);
            return (stamp);
        }


    }
}


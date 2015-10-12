using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entryPointsGenerator
{
    public class EdgeVisited
    {
        List<string> domain1;
        List<string> name1;
        List<string> domain2;
        List<string> name2;
     
        public EdgeVisited()
        {
            domain1 = new List<string>();
            name1 = new List<string>();
            domain2 = new List<string>();
            name2 = new List<string>();
        }

        public void Add(string d1, string n1,string d2, string n2)
        {
            domain1.Add(d1);
            name1.Add(n1);
            domain2.Add(d2);
            name2.Add(n2);
        }





        public Boolean ContainsKey(string d1, string n1,string d2, string n2)
        {
            for (int i = 0; i < domain1.Count; i++)
            {
                if (d1 == domain1[i] && n1 == name1[i] & d2 == domain2[i] && n2 == name2[i])
                {
                    return true;
                }

                if ((d2 == domain1[i] && n2 == name1[i] & d1 == domain2[i] && n1 == name2[i]))
                {
                    return true;
                }
            }
            return false;
        }


    }
}

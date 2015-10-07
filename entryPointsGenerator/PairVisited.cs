using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entryPointsGenerator
{
    public class PairVisited
    {
        List<string> domain;
        List<string> name;
        List<int> weight;
        public PairVisited()
        {
            domain = new List<string>();
            name = new List<string>();
            weight = new List<int>();
        }

        public void Add(string d, string n)
        {
            domain.Add(d);
            name.Add(n);
            weight.Add(1);
        }

        public void AddZero(string d, string n)
        {
            domain.Add(d);
            name.Add(n);
            weight.Add(0);
        }


        public void PlusWeight(string d, string n)
        {
            for (int i = 0; i < domain.Count; i++)
            {
                if (d == domain[i] && n == name[i])
                {
                    weight[i]++;
                    return;
                }
            }
        }

        public int GetWeight(string d, string n)
        {
            for (int i = 0; i < domain.Count; i++)
            {
                if (d == domain[i] && n == name[i]) return weight[i];

            }
            return 0;
        }

        public Boolean ContainsKey(string d, string n)
        {
            for (int i = 0; i < domain.Count; i++)
            {
                if (d == domain[i] && n == name[i])
                {
                    return true;
                }
            }
            return false;
        }


    }
}

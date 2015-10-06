using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entryPointsGenerator
{
    public class DomainPair
    {
        public String lang1;
        public String lang2;
        public String page1;
        public String page2;

        public DomainPair(String Lang1, String Lang2, String Page1, String Page2)
        {
            lang1 = Lang1;
            lang2 = Lang2;
            page1 = Page1;
            page2 = Page2;
        }


    }
}

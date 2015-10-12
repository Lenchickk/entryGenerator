using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entryPointsGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            RunForAll(1);
            //RunForAll(2);
            //RunForAll(3);

        }

        static void RunForAll(Int16 d)
        {
            String viewDirectory = @"C:\Users\Lenchick\Google Drive\WASHU FALL2015\play ground\pipe\analysis\";
            String outDirectory = @"C:\Users\Lenchick\Google Drive\WASHU FALL2015\play ground\pipe\check\";
            String[] viewsFiles = System.IO.Directory.GetFiles(viewDirectory, "*_catalog.csv");

            CommonPlace.Activate(d);
            int count = -1;

            foreach (String file in viewsFiles)
            {
                count++;
                System.Random rand = new Random();
                EntryGate.LoadInitials(file);
                Digger.Wraper();
                CommonPlace.TableToFile(outDirectory + rand.Next(100).ToString() + "___" + CommonPlace.Depth.ToString());
           }


        }


    }
}

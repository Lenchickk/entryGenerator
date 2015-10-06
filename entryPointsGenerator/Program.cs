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
            String viewDirectory = @"C:\Users\Lenchick\Google Drive\WASHU FALL2015\play ground\pipe\";
            String[] viewsFiles = System.IO.Directory.GetFiles(viewDirectory, "*_catalog.csv");

            CommonPlace.Activate();
            int count = -1;
            foreach (String file in viewsFiles)
            {
                count++;
                System.Random rand = new Random();
                EntryGate.LoadInitials(file);
                Digger.Wraper();
                //String str = file.Substring(file.i.IndexOf(@"\"), file.Length - file.IndexOf("_catalog"));
                CommonPlace.TableToFile(viewDirectory + rand.Next(100).ToString() + CommonPlace.Depth.ToString() + "_" + count.ToString() + "_in.csv");
                //helper.processPipeFile(file);
            }

        }


    }
}

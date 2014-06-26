using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NameGenerate
{
    class Program
    {
        static void Main(string[] args)
        {
            MarkhovBrain markhov = null;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(@AppDomain.CurrentDomain.BaseDirectory+"/dict.txt", System.IO.FileMode.Open);
                System.IO.StreamReader r = new System.IO.StreamReader(fs);
                markhov = new MarkhovBrain(r);
                r.Close();
            }
            catch
            {
                Console.WriteLine("No dict.txt found... it must be in the same directory as this program.");
                Console.WriteLine("A dict.txt contains 1 word per line. These create the Markhov Chains.");
                Console.WriteLine("Press any key to quit.");
                Console.ReadKey();
            }
            if (markhov != null)
            {
                Console.WriteLine("Successful creation of Markhov database. Total Entries: " + markhov.num_add + ".");
                Console.WriteLine("This Program uses weighted Markhov chains to create words that follow existing");
                Console.WriteLine("   character patterns in a supplied dictionary.");
                Console.WriteLine("The more instances of a certain character progression e.g. (ab=>cd) the more");
                Console.WriteLine("   likely it is to occur.");
                Console.WriteLine("Terminators have been given a bonus 100 weight to make the words shorter.");

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("");
                Console.WriteLine("       Created by Greg Billings reachable at gregorymbillings@gmail.com");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("");
                Console.WriteLine("Press any key to begin.");
                Console.ReadKey();
                markhov.program_loop();
            }
        }
    }
}

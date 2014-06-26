using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NameGenerate
{
    class Markhov
    {
        public bool at_beginning = false;
        public string to;
        public int instances;

        public Markhov()
        {

        }

        public Markhov(string pto, bool patbeginning)
        {
            at_beginning = patbeginning;
            instances = 1;
            to = pto;
            if (to.Length < 2)
                instances += 100;
        }

        public void increment_instances()
        {
            instances++;
        }

      
        public override string ToString()
        {
            return "=>" + to + " (" + instances + ")";
        }
    }
}

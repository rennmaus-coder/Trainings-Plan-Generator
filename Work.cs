using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainings_plan_Generator
{
    class Work
    {
        public string Name { get; set; }
        public Hashtable Stages { get; set; }
        public Hashtable Muscles { get; set; }
        public string Tutorial { get; set; }
        public bool Seconds { get; set; }
    }
}

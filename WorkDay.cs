using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainings_plan_Generator
{
    class WorkDay
    {
        public bool train { get; set; }
        public List<Exercise> work { get; set; }
        public WorkDay(bool training)
        {
            train = training;
        }
        public WorkDay(List<Exercise> work, bool training)
        {
            train = training;
            this.work = work;
        }
    }
}

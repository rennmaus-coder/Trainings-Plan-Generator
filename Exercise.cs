using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainings_plan_Generator
{
    class Exercise
    {
        public string Name { get; set; }
        public int Reps { get; set; } = 0;
        public bool Pause { get; set; }
        public int Duration { get; set; } = 0;
        public string Tutorial { get; set; }

        public Exercise(string name, int reps, bool pause, int duration, string tutorial)
        {
            Name = name;
            Reps = reps;
            Pause = pause;
            Duration = duration;
            Tutorial = tutorial;
        }

        public Exercise(string name, int reps, bool pause, string tutorial)
        {
            Name = name;
            Reps = reps;
            Pause = pause;
            Tutorial = tutorial;
        }

        public Exercise(string name, bool pause, int duration, string tutorial)
        {
            Name = name;
            Pause = pause;
            Duration = duration;
            Tutorial = tutorial;
        }

        public Exercise(bool pause, int duration)
        {
            Pause = pause;
            Duration = duration;
        }
    }
}

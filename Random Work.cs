using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainings_plan_Generator
{
    class Random_Work : MainWindow
    {
        public List<WorkDay> getRandomWork(List<Work> works)
        {

            List<WorkDay> res = new List<WorkDay>();

            ArrayList selected = new ArrayList();
            if (abs.IsChecked.Value)
            {
                selected.Add("ABS");
            }
            if (chest.IsChecked.Value)
            {
                selected.Add("CHEST");
            }
            if (back.IsChecked.Value)
            {
                selected.Add("BACK");
            }
            if (leg.IsChecked.Value)
            {
                selected.Add("LEG");
            }
            if (arms.IsChecked.Value)
            {
                selected.Add("ARM");
            }

            List<Work> chosen = new List<Work>();

            StringBuilder sr = new StringBuilder();

            foreach (Work work in works)
            {

                List<KeyValuePair<string, int>> muscles = HashtableToKeyValue(work.Muscles);

                sr.AppendLine(selected[0] + " " + selected[1]);
                bool added = false;
                if (selected.Contains(muscles[0].Key))
                {
                    if (muscles[0].Value > 6)
                    {
                        chosen.Add(work);
                        added = true;
                    }
                }
                if (selected.Contains(muscles[1].Key) && !added)
                {
                    if (muscles[1].Value > 6)
                    {
                        chosen.Add(work);
                        added = true;
                    }
                }
                if (selected.Contains(muscles[2].Key) && !added)
                {
                    if (muscles[2].Value > 6)
                    {
                        chosen.Add(work);
                        added = true;
                    }
                }
                if (selected.Contains(muscles[3].Key) && !added)
                {
                    if (muscles[3].Value > 6)
                    {
                        chosen.Add(work);
                    }
                }
            }

            sr.Clear();

            Random rand = new Random(345);

            if (diff.Text == "easy")
            {
                for (int day = 0; day <= Convert.ToInt32(length.Text); day++)
                {
                    sr.AppendLine("tag: " + day.ToString());
                    if (day % 2 == 1)
                    {
                        WorkDay workDay = new WorkDay(false);
                        res.Add(workDay);
                    }
                    else
                    {
                        List<Exercise> ex = new List<Exercise>();

                        for (int i = 0; i <= Convert.ToInt32(training.Text); i += 1)
                        {
                            sr.AppendLine("Minute :" + i.ToString());
                            if (i % 4 == 0)
                            {
                                ex.Add(new Exercise(true, 60));
                                continue;
                            }

                            int index = rand.Next(0, chosen.Count);
                            Work work = chosen[index];

                            if (work.Seconds)
                            {
                                ex.Add(new Exercise(work.Name, false, 30, work.Tutorial));
                                continue;
                            }


                            int reps = Convert.ToInt32(work.Stages["EASY"]) + rand.Next(-3, 5);

                            if (reps <= 0)
                            {
                                reps = 1;
                            }

                            ex.Add(new Exercise(work.Name, reps, false, 30, work.Tutorial));
                        }

                        res.Add(new WorkDay(ex, true));

                    }
                }
            }

            return res;
        }

        private List<KeyValuePair<string, int>> HashtableToKeyValue(Hashtable table)
        {
            List<string> keys = new List<string>();
            List<int> vals = new List<int>();
            List<KeyValuePair<string, int>> res = new List<KeyValuePair<string, int>>();

            foreach (string k in table.Keys)
            {
                keys.Add(k);
            }
            foreach (Int64 k in table.Values)
            {
                vals.Add(Convert.ToUInt16(k));
            }

            for (int i = 0; i < keys.Count; i++)
            {
                res.Add(new KeyValuePair<string, int>(keys[i], vals[i]));
            }
            return res;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace Trainings_plan_Generator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool check()
        {
            bool ret = true;
            int counter = 0;
            training.BorderBrush = System.Windows.Media.Brushes.Gray;
            name.BorderBrush = System.Windows.Media.Brushes.Gray;
            length.BorderBrush = System.Windows.Media.Brushes.Gray;
            if (training.Text.Equals(""))
            {
                training.BorderBrush = System.Windows.Media.Brushes.Red;
                ret = false;
            }
            if (name.Text.Equals(""))
            {
                name.BorderBrush = System.Windows.Media.Brushes.Red;
                ret = false;
            }
            if (length.Text.Equals(""))
            {
                length.BorderBrush = System.Windows.Media.Brushes.Red;
                ret = false;
            }
            if (abs.IsChecked.Value)
            {
                counter++;
            }
            if (chest.IsChecked.Value)
            {
                counter++;
            }
            if (back.IsChecked.Value)
            {
                counter++;
            }
            if (leg.IsChecked.Value)
            {
                counter++;
            }
            if (arms.IsChecked.Value)
            {
                counter++;
            }

            if (counter <= 1)
            {
                feedback.Text += "\nAt least two checkboxes must be checked!";
                ret = false;
            }
            return ret;
        }

        private void generate(object sender, RoutedEventArgs e)
        {
            if (!check())
            {
                return;
            }

            string file = File.ReadAllText("./Resources/ExercisesConfig.json");
            List<Work> works = JsonConvert.DeserializeObject<List<Work>>(file);
            List<WorkDay> days = getRandomWork(works);
            string html = createHTML(days);
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = name.Text;
            dialog.DefaultExt = ".html";
            dialog.Filter = "Html Document | *.html";

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                File.WriteAllText(filename, html);
                feedback.Text = "Sucessfully created Trainings Plan!";

            } else
            {
                feedback.Text = "Couldn't create Trainings Plan, please try again!";
            }
        }

        private string createHTML(List<WorkDay> days)
        {
            string BEGIN = "<!DOCTYPE html>" +
                        "<html>" +
                        "  <head>" +
                        "    <meta charset='utf-8'/>" +
                        "    <title>" + name.Text + "</title>" +
                        "  </head>" +
                        "  <body>" +
                        "  <h1 align=center>" + name.Text + "</h1><br>" +
                        "  <ul>";

            string END = " </ul>" +
                      "  <style>" +
                      "    body {" +
                      "      font-family: \"Segoe UI\"," +
                      "    }" +
                      "    li {" +
                      "     margin: 10px 10px 10px 10px" +
                      "    }" +
                      "  </style>" +
                      " </body>" +
                      "</html>";

            StringBuilder content = new StringBuilder();
            content.Append(BEGIN);
            int counter = 1;
            foreach (WorkDay day in days)
            {
                content.AppendLine("</ul><br><h2>Tag " + counter + "</h2><ul>");
                counter++;
                if (!day.train)
                {
                    content.AppendLine("<br><li>Rest Day</li><br>");
                    continue;
                }
                foreach (Exercise work in day.work)
                {
                    if (work.Pause)
                    {
                        content.AppendLine("<li>Pause (60s)</li>");
                        continue;
                    }
                    if (work.Reps != 0)
                    {
                        content.AppendLine("<li><a href='" + work.Tutorial + "'>Übung: " + work.Name + ",\t Reps: " + work.Reps + "  +  Pause (30sek) </a>");
                    }
                    if (work.Reps == 0)
                    {
                        content.AppendLine("<li><a href='" + work.Tutorial + "'>Übung: " + work.Name + ",\t 30sek  +  Pause (30sek)</a>");
                    }
                }
            }
            content.Append(END);
            return content.ToString();
        }

        private List<WorkDay> getRandomWork(List<Work> works)
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

            foreach (Work work in works)
            {

                List<KeyValuePair<string, int>> muscles = HashtableToKeyValue(work.Muscles);

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


            Random rand = new Random();

            if (diff.Text == "easy")
            {
                for (int day = 0; day <= Convert.ToInt32(length.Text); day++)
                {
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
                            if (i % 4 == 0 && i != 0)
                            {
                                ex.Add(new Exercise(true, 60));
                                continue;
                            }

                            int index = rand.Next(0, chosen.Count);
                            Work work = chosen[index];

                            if (work.Seconds || time.Text == "Use only Seconds")
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

            else if (diff.Text == "medium")
            {
                for (int day = 0; day <= Convert.ToInt32(length.Text); day++)
                {
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
                            if (i % 4 == 0 && i != 0)
                            {
                                ex.Add(new Exercise(true, 60));
                                continue;
                            }

                            int index = rand.Next(0, chosen.Count);
                            Work work = chosen[index];

                            if (work.Seconds || time.Text == "Use only Seconds")
                            {
                                ex.Add(new Exercise(work.Name, false, 30, work.Tutorial));
                                continue;
                            }


                            int reps = Convert.ToInt32(work.Stages["MEDIUM"]) + rand.Next(-3, 5);

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

            else if (diff.Text == "intense")
            {
                for (int day = 0; day <= Convert.ToInt32(length.Text); day++)
                {
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
                            if (i % 4 == 0 && i != 0)
                            {
                                ex.Add(new Exercise(true, 60));
                                continue;
                            }

                            int index = rand.Next(0, chosen.Count);
                            Work work = chosen[index];

                            if (work.Seconds || time.Text == "Use only Seconds")
                            {
                                ex.Add(new Exercise(work.Name, false, 30, work.Tutorial));
                                continue;
                            }


                            int reps = Convert.ToInt32(work.Stages["INTENSE"]) + rand.Next(-3, 5);

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

            if (diff.Text == "hard")
            {
                for (int day = 0; day <= Convert.ToInt32(length.Text); day++)
                {
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
                            if (i % 4 == 0 && i != 0)
                            {
                                ex.Add(new Exercise(true, 60));
                                continue;
                            }

                            int index = rand.Next(0, chosen.Count);
                            Work work = chosen[index];

                            if (work.Seconds || time.Text == "Use only Seconds")
                            {
                                ex.Add(new Exercise(work.Name, false, 30, work.Tutorial));
                                continue;
                            }


                            int reps = Convert.ToInt32(work.Stages["HARD"]) + rand.Next(-3, 5);

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



        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            name.BorderBrush = System.Windows.Media.Brushes.Gray;
        }

        private void length_TextChanged(object sender, TextChangedEventArgs e)
        {
            length.BorderBrush = System.Windows.Media.Brushes.Gray;
        }

        private void train_TextChanged(object sender, TextChangedEventArgs e)
        {
            training.BorderBrush = System.Windows.Media.Brushes.Gray;
        }

        private void MaskNumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !TextIsNumeric(e.Text);
        }

        private void MaskNumericPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string input = (string)e.DataObject.GetData(typeof(string));
                if (!TextIsNumeric(input)) e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool TextIsNumeric(string input)
        {
            return input.All(c => Char.IsDigit(c) || Char.IsControl(c));
        }
    }
}

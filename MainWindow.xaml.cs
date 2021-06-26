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
                selected.Add("ARMS");
            }

            List<Work> chosen = new List<Work>();

            foreach (Work work in works)
            {
                IOrderedEnumerable<DictionaryEntry> sorted = work.Muscles.Cast<DictionaryEntry>().OrderBy(entry => entry.Value);
                Hashtable sort = (Hashtable)sorted.Cast<Hashtable>();
                ArrayList muscles = ToList(sort.Keys);
                List<string> temp = new List<string>();
                if (selected.Contains(muscles[0]))
                {
                    temp.Add(muscles[0].ToString());
                }
                if (selected.Contains(muscles[1]))
                {
                    temp.Add(muscles[1].ToString());
                }
                if (temp.Contains(muscles[0]))
                {
                    chosen.Add(work);
                }
            }

            if (diff.SelectedItem.ToString() == "easy")
            {
                for (int day = 0; day < Convert.ToInt32(length.Text); day++)
                {
                    if (day % 2 == 1)
                    {
                        WorkDay workDay = new WorkDay(false);
                        res.Add(workDay);
                    }
                    else
                    {
                        Random rand = new Random();
                        Work work = works[rand.Next(0, works.Count - 1)];
                        WorkDay workDay = new WorkDay(true);
                    }
                }
            }
            return null;
        }

        private void generate(object sender, RoutedEventArgs e)
        {
            if (!check())
            {
                return;
            }

            string file = File.ReadAllText("./Resources/ExercisesConfig.json");
            List<Work> works = JsonConvert.DeserializeObject<List<Work>>(file);
        }

        private ArrayList ToList(ICollection coll)
        {
            ArrayList list = new ArrayList();
            foreach (var item in coll)
            {
                list.Add(item);
            }
            return list;
        }
    }
}

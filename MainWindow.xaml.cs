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
using Trainings_plan_Generator;

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
            Random_Work randomWork = new Random_Work();
            List<WorkDay> days = randomWork.getRandomWork(works);
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
                      " </body>" +
                      "</html>";

            StringBuilder content = new StringBuilder();
            foreach (WorkDay day in days)
            {
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

                    }
                }
            }
            return content.ToString();
        }
    }
}

using System;
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
            feedback.Text = "";
            if (training.Text.Equals(""))
            {
                feedback.Text += "\nPlease fill the \"Training Length (Minutes)\" Field!";
                ret = false;
            }
            if (name.Text.Equals(""))
            {
                feedback.Text += "\nPlease fill the name Field!";
                ret = false;
            }
            if (length.Text.Equals(""))
            {
                feedback.Text += "\nPlease fill the Length Field!";
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
            if (ret)
            {
                feedback.Text = "";
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

            foreach (Work work in works)
            {
                
            }
        }
    }
}

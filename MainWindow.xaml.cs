using System;
using System.Collections.Generic;
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
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace GameLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string selected;
        public static string path = @"D:\GameLauncher\games.txt";
        public static string settpath = @"D:\GameLauncher\settings.txt";
        static List<Data> datas = new List<Data>();
        static List<bool> settings = new List<bool>();

        public MainWindow()
        {
            InitializeComponent();
            foreach (var i in File.ReadAllLines(settpath))
                settings.Add(bool.Parse(i));
            foreach (var i in File.ReadAllLines(path))
                datas.Add(new Data(i));
            foreach (var i in datas)
                lb.Items.Add(i.name);
            if (settings.Count > 0)
            {
                chbutton.IsChecked = settings[0];
                chclick.IsChecked = settings[1];
                chenter.IsChecked = settings[2];
            }

        }

        private void playbtn_Click(object sender, RoutedEventArgs e)
        {
            if (lb.SelectedItem != null)
                Process.Start(datas.Find(x => x.name == lb.SelectedItem.ToString()).path);
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            Window aw = new AddWindows();
            aw.ShowDialog();
        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            if (selected != null)
            {
                string editedpath = edbox1.Text,
                editedname = edlab1.Text;
                List<string> lines = new List<string>(File.ReadAllLines(path));
                lines[lb.SelectedIndex] = $"{editedname};{editedpath}";
                StreamWriter sw = new StreamWriter(path);
                foreach (var i in lines)
                    sw.WriteLine(i);
                sw.Close();
                lb.Items.Clear();
                lb.Items.Refresh();
                datas.Clear();
                foreach (var i in File.ReadAllLines(path))
                    datas.Add(new Data(i));
                foreach (var i in datas)
                    lb.Items.Add(i.name);
            }
            else MessageBox.Show("First you have to select a list item!");
        }

        private void rembtn_Click(object sender, RoutedEventArgs e)
        {
            if (lb.SelectedItem != null)
            {
                lb.Items.Remove(lb.SelectedItem);
                List<string> lines = new List<string>(File.ReadAllLines(path));
                lines.Remove(lines[lb.SelectedIndex + 1]);
                StreamWriter sw = new StreamWriter(path);
                foreach (var i in lines)
                    sw.WriteLine(i);
                sw.Close();
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (lb.Items.Count < File.ReadAllLines(path).Length)
            {
                foreach (var i in File.ReadAllLines(path))
                {
                    if (!datas.Contains(new Data(i)))
                        datas.Add(new Data(i));
                }
                lb.Items.Add(datas.Last().name);
            }
        }

        private void maintc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (maintc.SelectedItem == lhead)
            {
                try
                {
                    selected = lb.SelectedItem.ToString();
                }
                catch (Exception)
                {
                }
            }
            edlab1.Text = selected;
            try
            {
                edbox1.Text = datas.Find(x => x.name == selected).path;
            }
            catch (Exception)
            {
            }
        }

        private void lb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (chclick.IsChecked == true)
            {
                if (lb.SelectedItem != null)
                {
                    try
                    {
                        Process.Start(datas.Find(x => x.name == lb.SelectedItem.ToString()).path);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show($"Error: {exc.Message}");
                    }
                }
            }
        }

        private void lb_KeyDown(object sender, KeyEventArgs e)
        {
            if (chenter.IsChecked == true)
            {
                if (e.Key == Key.Enter)
                {
                    if (lb.SelectedItem != null)
                        Process.Start(datas.Find(x => x.name == lb.SelectedItem.ToString()).path);
                }
            }

        }

        private void chenter_Checked(object sender, RoutedEventArgs e) //2
        {
            e.Handled = true;
            if (chenter.IsChecked == true) settings[2] = true;
            else settings[2] = false;
            StreamWriter sw = new StreamWriter(settpath);
            foreach (var i in settings)
                sw.WriteLine(i);
            sw.Close();
        }

        private void chclick_Checked(object sender, RoutedEventArgs e) //1
        {
            if (chclick.IsChecked == true) settings[1] = true;
            else settings[1] = false;
            StreamWriter sw = new StreamWriter(settpath);
            foreach (var i in settings)
                sw.WriteLine(i);
            sw.Close();
        }

        private void chbutton_Checked(object sender, RoutedEventArgs e) //0
        {
            if (chbutton.IsChecked == true)
            {
                settings[0] = true;
                playbtn.Visibility = Visibility.Visible;
            }
            else
            {
                settings[0] = false;
                playbtn.Visibility = Visibility.Hidden;
            }
            StreamWriter sw = new StreamWriter(settpath);
            foreach (var i in settings)
                sw.WriteLine(i);
            sw.Close();
        }

        private void openbtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Browse..";
            ofd.Filter = "Shortcuts (*.lnk)|*.lnk|Executable files (*.exe)|*.exe|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                edbox1.Text = ofd.FileName;
            }
        }
    }
}

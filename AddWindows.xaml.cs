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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Drawing.Configuration;

namespace GameLauncher
{
    /// <summary>
    /// Interaction logic for AddWindows.xaml
    /// </summary>
    public partial class AddWindows : Window
    {
        public AddWindows()
        {
            InitializeComponent();
        }

        private void additem_Click(object sender, RoutedEventArgs e)
        {
            if (pathBox.Text == "" || nameBox.Text == "")
            {
                ErrorWindow error = new ErrorWindow();
                error.ShowDialog();
            }
            else
            {
                StreamWriter sw = new StreamWriter(MainWindow.path, append: true);
                sw.WriteLine($"{nameBox.Text};{pathBox.Text}");
                sw.Close();

                this.Close();
            }
        }

        private void openbtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Browse..";
            ofd.Filter = "Shortcuts (*.lnk)|*.lnk|Executable files (*.exe)|*.exe|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                pathBox.Text = ofd.FileName;
            }
        }
    }
}

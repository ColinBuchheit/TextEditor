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
using System.IO; // Make sure to include this for File IO operations

namespace PlainTextEditor
{
    public partial class MainWindow : Window
    {
        // It's a good practice to declare class-level variables at the top
        private string currentFilePath = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Your existing MenuItem_Click, if not used, can be removed or left empty if you plan to use it later

        private void New_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.Clear();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string filename = openFileDialog.FileName;
                MainTextBox.Text = File.ReadAllText(filename);
            }
        } // This closing brace was missing in your code

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveAs_Click(sender, e);
                return;
            }

            File.WriteAllText(currentFilePath, MainTextBox.Text);
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                currentFilePath = saveFileDialog.FileName;
                File.WriteAllText(currentFilePath, MainTextBox.Text);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Simple Text Editor\nVersion 1.0\n2024", "About");
        }
    }
}

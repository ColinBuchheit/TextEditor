using System;
using System.Windows;
using Microsoft.Win32;
using System.IO;

namespace PlainTextEditor
{
    public partial class MainWindow : Window
    {
        private TextDocument document = new TextDocument();
        private bool isDocumentModified = false;
        private string lastAccessedDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public MainWindow()
        {
            InitializeComponent();
            UpdateMenuItems();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmSaveIfModified())
            {
                document = new TextDocument();
                MainTextBox.Clear();
                isDocumentModified = false;
                UpdateMenuItems();
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmSaveIfModified())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    DefaultExt = ".txt",
                    Filter = "Text documents (.txt)|*.txt",
                    InitialDirectory = lastAccessedDirectory
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    document.Open(openFileDialog.FileName);
                    MainTextBox.Text = document.Content;
                    isDocumentModified = false;
                    lastAccessedDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                    UpdateMenuItems();
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(document.FilePath))
            {
                SaveAs_Click(sender, e);
                return;
            }

            try
            {
                document.Content = MainTextBox.Text;
                document.Save();
                isDocumentModified = false;
                UpdateMenuItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt",
                InitialDirectory = lastAccessedDirectory
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                document.Content = MainTextBox.Text;
                document.SaveAs(saveFileDialog.FileName);
                isDocumentModified = false;
                lastAccessedDirectory = Path.GetDirectoryName(saveFileDialog.FileName);
                UpdateMenuItems();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmSaveIfModified())
            {
                Application.Current.Shutdown();
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Simple Text Editor\nVersion 1.0\n2024", "About");
        }

        private void MainTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            isDocumentModified = true;
            UpdateMenuItems();
        }

        private bool ConfirmSaveIfModified()
        {
            if (isDocumentModified)
            {
                var result = MessageBox.Show("Do you want to save changes to your document?", "Confirm", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel)
                {
                    return false;
                }
                else if (result == MessageBoxResult.Yes)
                {
                    Save_Click(this, new RoutedEventArgs());
                    return !isDocumentModified;
                }
            }
            return true;
        }

        private void UpdateMenuItems()
        {
            // Ensure this method correctly references your Save menu item, like `SaveMenuItem.IsEnabled = ...;`
            SaveMenuItem.IsEnabled = !string.IsNullOrEmpty(document.FilePath);
        }
    }
}

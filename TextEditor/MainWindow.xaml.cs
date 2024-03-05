using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace PlainTextEditor
{
    public partial class MainWindow : Window
    {
        private string currentFilePath = null;
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
                MainTextBox.Clear();
                currentFilePath = null;
                isDocumentModified = false;
                UpdateMenuItems();
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmSaveIfModified())
            {
                var openFileDialog = new OpenFileDialog
                {
                    DefaultExt = ".txt",
                    Filter = "Text documents (.txt)|*.txt",
                    InitialDirectory = lastAccessedDirectory
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        currentFilePath = openFileDialog.FileName;
                        MainTextBox.Text = File.ReadAllText(currentFilePath);
                        isDocumentModified = false;
                        lastAccessedDirectory = Path.GetDirectoryName(currentFilePath);
                        UpdateMenuItems();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to open file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(currentFilePath))
                {
                    SaveAs_Click(sender, e);
                }
                else
                {
                    File.WriteAllText(currentFilePath, MainTextBox.Text);
                    isDocumentModified = false;
                    UpdateMenuItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt",
                InitialDirectory = !string.IsNullOrEmpty(currentFilePath) ? Path.GetDirectoryName(currentFilePath) : lastAccessedDirectory
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    currentFilePath = saveFileDialog.FileName;
                    File.WriteAllText(currentFilePath, MainTextBox.Text);
                    isDocumentModified = false;
                    lastAccessedDirectory = Path.GetDirectoryName(currentFilePath);
                    UpdateMenuItems();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save as: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                var result = MessageBox.Show("Do you want to save changes to your document?", "Confirm", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    return false;
                }
                else if (result == MessageBoxResult.Yes)
                {
                    Save_Click(this, new RoutedEventArgs());
                    return !isDocumentModified; // Return false if Save was cancelled
                }
            }
            return true;
        }

        private void UpdateMenuItems()
        {
            // Assuming SaveMenuItem is named and accessible
            SaveMenuItem.IsEnabled = isDocumentModified || !string.IsNullOrEmpty(currentFilePath);
            // Other menu items can be similarly managed based on the application's state
        }
    }
}

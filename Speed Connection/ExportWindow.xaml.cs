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

namespace Speed_Connection
{
    /// <summary>
    /// Логика взаимодействия для ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window
    {
        public string SelectedDirectory { get; private set; }

        public ExportWindow()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedDirectory = ExportDirectoryTextBox.Text;

            if (string.IsNullOrWhiteSpace(SelectedDirectory))
            {
                MessageBox.Show("Please enter a valid export directory.");
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}

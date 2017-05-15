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
using System.Windows.Forms;
using System.IO;

namespace Eye_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void trackBtn_Click(object sender, RoutedEventArgs e)
        {
            Camera cameraWindow = new Camera();
            cameraWindow.Show();
            this.Close();
        }

        private void browseBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Title = "File Manager";
            fileDlg.InitialDirectory = @"c:\";
            fileDlg.Filter = "Pdf files (*.pdf)|*.pdf|Pdf files (*.pdf)|*.pdf";
            fileDlg.FilterIndex = 2;
            fileDlg.RestoreDirectory = true;
            if (fileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = fileDlg.FileName;
                System.Diagnostics.Process.Start(filePath);
            }
        }
    }
}

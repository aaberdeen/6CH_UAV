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

namespace WolfAC10_WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Logging : Window
    {
        public Logging()
        {
            InitializeComponent();
            LogFile_textBox.Text = Properties.Settings.Default.logFile;
            Logging_time_comboBox.SelectedIndex = Properties.Settings.Default.loggingIndex;
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.SaveFileDialog openFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            
            
            // Show the dialog and get result.
            openFileDialog1.DefaultExt = "csv";
            openFileDialog1.Filter = "CSV Files (.csv)|*.csv|Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.AddExtension = true;
            Nullable<bool> result = openFileDialog1.ShowDialog();
            if (result == true) // Test result.
            {
                string file = openFileDialog1.FileName;
                LogFile_textBox.Text = file;
            }
            else
            {
                
            }
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OK_button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.logFile = LogFile_textBox.Text;


            //ReadWriteCSV.CsvFileReader CSV_R = new ReadWriteCSV.CsvFileReader(Properties.Settings.Default.logFile);
            //int CSVlineCount = CSV_R.RowCount();
            //CSV_R.Close();
            //  Properties.Settings.Default.logFileLength = CSVlineCount;





            Properties.Settings.Default.loggingIndex = Logging_time_comboBox.SelectedIndex;
            int time_in_ms = 1000;

            switch (Logging_time_comboBox.SelectedIndex)
            {
                case 0: time_in_ms = 100; //1s
                    break;
                case 1: time_in_ms = 200; //5s
                    break;
                case 2: time_in_ms = 300; //10s
                    break;
                case 3: time_in_ms = 400; //30s
                    break;
                case 4: time_in_ms = 500; //1min
                    break;
                case 5: time_in_ms = 600; //5min
                    break;
                case 6: time_in_ms = 700; //10min
                    break;
                case 7: time_in_ms = 800; //1min
                    break;
                case 8: time_in_ms = 900; //5min
                    break;
                case 9: time_in_ms = 1000; //10min
                    break;

            }
            Properties.Settings.Default.loggingTime_ms = time_in_ms;
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}

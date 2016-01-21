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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WolfAC10_WPF
{
    /// <summary>
    /// Interaction logic for Terminal_Window.xaml
    /// </summary>
    public partial class Terminal_Window : UserControl
    {
        ConsoleContent dc = new ConsoleContent();

        public Terminal_Window()
        {
            InitializeComponent();
           // (this.Content as FrameworkElement).DataContext = this;
        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
             InputBlock.KeyDown += InputBlock_KeyDown;
            InputBlock.Focus();
        }
        private void InputBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dc.ConsoleInput = InputBlock.Text;
            //    _mcu_read.SendData(InputBlock.Text);
                toSend = InputBlock.Text;
                dc.RunCommand();
                InputBlock.Focus();
                Scroller.ScrollToBottom();
            }
            else
            {

            }
        }

        public string toSend
        {
            get { return (string)GetValue(toSendProperty); }
            set { SetValueDp(toSendProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty toSendProperty =
            DependencyProperty.Register("toSend", typeof(string),
            typeof(Terminal_Window), null);

        public event PropertyChangedEventHandler PropertyChanged;

        void SetValueDp(DependencyProperty property, object value, [System.Runtime.CompilerServices.CallerMemberName] String p = null)
        {

            SetValue(property, value);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
                
        }

    }

    public class ConsoleContent : INotifyPropertyChanged
    {
        string consoleInput = string.Empty;
        ObservableCollection<string> consoleOutput = new ObservableCollection<string>() { "Lets Go..." };

        public string ConsoleInput
        {
            get
            {
                return consoleInput;
            }
            set
            {
                consoleInput = value;
                OnPropertyChanged("ConsoleInput");
            }
        }

        public ObservableCollection<string> ConsoleOutput
        {
            get
            {
                return consoleOutput;
            }
            set
            {
                consoleOutput = value;
                OnPropertyChanged("ConsoleOutput");
            }
        }

        public void RunCommand()
        {
            ConsoleOutput.Add(ConsoleInput);
            if (ConsoleOutput.Count >= 100)
            {
                while (ConsoleOutput.Count >= 100)
                {
                    ConsoleOutput.Remove(ConsoleOutput[0]);
                }

            }
            // do your stuff here.
            ConsoleInput = String.Empty;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

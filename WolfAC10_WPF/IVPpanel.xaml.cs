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

namespace WolfAC10_WPF
{
    /// <summary>
    /// Interaction logic for IVPpanel.xaml
    /// </summary>
    public partial class IVPpanel : UserControl
    {
        public IVPpanel()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }

        public string Title 
        {
            get { return Title_groupBox.Header as string; }
            set { Title_groupBox.Header = value; }
        }
        
        
        
        public string Volt 
        {
            get { return (string)GetValue(VoltProperty); }
            set { SetValueDp(VoltProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VoltProperty =
            DependencyProperty.Register("Volt", typeof(string),
            typeof(IVPpanel), null);


        public string Amp
        {
            get { return (string)GetValue(AmpProperty); }
            set { SetValueDp(AmpProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AmpProperty =
            DependencyProperty.Register("Amp", typeof(string),
            typeof(IVPpanel), null);

        public string Watt
        {
            get { return (string)GetValue(WattProperty); }
            set { SetValueDp(WattProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WattProperty =
            DependencyProperty.Register("Watt", typeof(string),
            typeof(IVPpanel), null);


        public event PropertyChangedEventHandler PropertyChanged;

        void SetValueDp(DependencyProperty property, object value, [System.Runtime.CompilerServices.CallerMemberName] String p = null) 
        {

            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        

        
    }
}

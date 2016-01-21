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

namespace WolfAC10_WPF
{
    /// <summary>
    /// Interaction logic for LED.xaml
    /// </summary>
    public partial class Green_LED : UserControl
    {

        public BitmapImage img; 
        public Green_LED()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

             //img = this.FindResource("greenLED") as BitmapImage;
           // BitmapImage r = this.FindResource("redLED") as BitmapImage;
          //  BitmapImage r = new BitmapImage(){UriSource = "images\red-led.png" as Uri};

            

           // img.UriSource = r.UriSource;
        }

        //public BitmapImage colour
        //{
            
        //    set{
        //        img= value;
        //    }
        //}
        
       
        

        static FrameworkPropertyMetadata propertymetadata = new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
        FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(picStatePropertyChanged), new CoerceValueCallback(picStateCoerceValue),
        false, UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty picStateProperty = DependencyProperty.Register("picState", typeof(Nullable<bool>), typeof(Green_LED), propertymetadata, new ValidateValueCallback(picStateValidate));

        private static void picStatePropertyChanged(DependencyObject dobj,
        DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            //MessageBox.Show(string.Format(
            //   "Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));

        }

        private static object picStateCoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency property value is reevaluated. The return value is the
            //latest value set to the dependency property

            //MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));

            return Value;
        }

        private static bool picStateValidate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            // inst.EN = (bool)Value;
            return true;
        }

        public Nullable<bool> picState
        {
            get
            {
                return GetValue(picStateProperty) as Nullable<bool>;
            }
            set
            {
                SetValue(picStateProperty, value);
            }
        }

    }
}

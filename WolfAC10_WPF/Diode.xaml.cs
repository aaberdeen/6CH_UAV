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
    /// Interaction logic for Diode.xaml
    /// </summary>
    public partial class Diode : UserControl
    {

        //private static bool _picState;

        //public bool picState
        //{
        //    get { return (bool)GetValue(picStateProperty); }
        //    set { SetValue(picStateProperty, value); }
        //}

       

        static FrameworkPropertyMetadata propertymetadata = new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
        FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(picStatePropertyChanged), new CoerceValueCallback(picStateCoerceValue),
        false, UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty picStateProperty = DependencyProperty.Register("picState", typeof(Nullable<bool>), typeof(Diode), propertymetadata, new ValidateValueCallback(picStateValidate));

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
                return  GetValue(picStateProperty) as Nullable<bool>;
            }
            set
            {
                SetValue(picStateProperty, value);
            }
        }


       // public static DiodeContent inst;
        

        public Diode()
        {
            InitializeComponent();
            
           // inst = new DiodeContent();
            (this.Content as FrameworkElement).DataContext = this;
           // DataContext = inst;

        }


    }
    //public class DiodeContent : INotifyPropertyChanged
    //{
    //    private Nullable<bool> _EN;


    //    public Nullable<bool> EN
    //    {
    //        get
    //        {
    //            return _EN;
    //        }
    //        set
    //        {
    //            _EN = value;
    //            OnPropertyChanged("EN");
    //        }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    void OnPropertyChanged(string propertyName)
    //    {
    //        if (null != PropertyChanged)
    //            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    //    }

    //}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WolfAC10_WPF
{
    public class Channel : INotifyPropertyChanged
    {
       // private bool _HYDRATE;
       // private Nullable<bool> _ENABLE_STK;
        private int _Volt_int;
        private string _Volt;
        private int _Amp_int;
        private int _LOAD;
        private string _Amp;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string p)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(
                    this, new PropertyChangedEventArgs(p));
        }

        public string Amp
        {
            get
            {
                
                return string.Format("{0}", _Amp_int);
      
            }
            set
            {
                int x;
                if (int.TryParse(value, out x))
                {
                    this._Amp = value;
                    this._Amp_int = Convert.ToInt32(value);
                    this._LOAD = (_Volt_int * _Amp_int) / 1000000;
                    this.OnPropertyChanged("Amp");
                    this.OnPropertyChanged("LOAD");
                }
            }
        }
        public string Volt
        {
            get
            {
                //if (this._V_STKA != null)
                //{
                //return this._V_STKA;
                return string.Format("{0}", _Volt_int);
                //}
                //else
                //{
                //    return "";
                //}
            }
            set
            {
                this._Volt = value;
                this._Volt_int = Convert.ToInt32(value);
                this._LOAD = (_Volt_int * _Amp_int) / 1000000;
                this.OnPropertyChanged("Volt");
                this.OnPropertyChanged("LOAD");
            }
        }
        public string LOAD
        {
            get
            {
                // int P = _VSTKA_int * _I_STKA_int;
                return string.Format("{0}", _LOAD);

            }

        }
    }


}

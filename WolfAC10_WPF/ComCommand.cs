using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace WolfAC10_WPF
{
    public class MCU_READ : INotifyPropertyChanged
    {
        private SerialPort _comport = new SerialPort();
        private bool _ReadyToSend;
        public static int linesToRead;

        private string _text;
       
        private string _dataToSend;

        //Channels
        public Channel ch_A1 = new Channel();
        public Channel ch_B1 = new Channel();
        public Channel bat_op = new Channel();
        public Channel bat_ip = new Channel();
        public Channel Load = new Channel();


        //Digitals
        private bool _DCDC_ENABLE;
        private bool _GREEN_LED;
        private bool _RED_LED;
        private bool _HYDRATEA;
        private bool _HYDRATEB;
        private bool _V_BAT_MEAS_EN;
        private bool _ENABLE_LOAD;
        private Nullable<bool> _ENABLE_STKA;
        private bool _ENABLE_STKB;
        private bool _ENABLE_BAT;
        private bool _NCHRG_ENABLE;
        private bool _CARTRIDGE_PWR;
        private bool _DIVIDER_MODE_SEL;


        //ADCs
      //  private string _VLOAD_MEAS;
      //  private int _VLOAD_MEAS_int;
      //  private int _I_LOAD_int;
      //  private int _P_LOAD;
      //  private int _Pout_DCDC;
        private string _IdcdcLimit;
        private string _StkPreload;
        private string _Fan1;
        private string _Fan2;
        private string _AdcTrigger;
        private string _PurgeValve;
        private string _InletValve;
        private string _OutDaq;
        private string _InDaq;
        private string _DataDump;

      //  private string _I_LOAD;
        private string _VBOP;
        private string _I_BOP;
        private string _I_DCDC;
        private int _I_DCDC_int;
        private string _EXT_5V;
        private string _RX_Error;
        private string _RX_Good;
        private int _RX_Error_int;
        private int _RX_Good_int;
        private string _TempA;
        private string _TempB;
        private string _PRESS_H2;

        //I2C
        private string _Register01;
        private string _Register02;
        private string _Register03;
        private string _Register04;
        private string _Register05;
        private string _Register06;
        private string _Register07;

        //Reg#1 decode
        private string _fault;
        private string _STAT;

        //Reg#2 decode
        private string _Iin_limit;
        private bool _EN_STAT;
        private bool _EN_TERM;
        private bool _nCE;

        //Reg#3 decode
       // private string _Vbatreg;
       // private string _USB_DET;


        //Reg#4 decode
        private int _I_chg_int;
        private string _I_chg_str;

        //Reg#5 decod

        private string _LOOP_STATUS;
        private bool _LOW_CHG;
        private bool _DPDM_EN;
        private bool _CE_STATUS;
        private string _VINDPM;


        //Reg#6 decode

        private bool _Two_xTMR_EN;
        private string _TMR;
        private bool _SYSOFF;
        private string _TS_STAT;

        //Reg#7 decode
      //  private string _Vovp;
       // private bool _CLR_VDP;
       // private bool _FORCE_BATDET;
       // private bool _FORCE_PTM;



        public MCU_READ(string portName)
        {
            bool error = false;

            if (_comport.IsOpen) _comport.Close();
            else
            {
                // Set the port's settings
                _comport.BaudRate = 115200;
                _comport.DataBits = 8;
                _comport.StopBits = StopBits.One;
                _comport.Parity = Parity.None;
                //   _comport.ReadTimeout = 1000;

                _comport.PortName = portName;
                try
                {
                    // Open the port
                    _comport.Open();
                }
                catch (UnauthorizedAccessException) { error = true; }
                //catch (IOException) { error = true; }
                catch (ArgumentException) { error = true; }

            }

            if (error)
            {
                throw new ArgumentException("comPort Exceprion");
            }

        }

        public void close()
        {
            this._comport.Close();
        }
        public string dataToSend
        {
            get { return _dataToSend; }
            set
            {
                _dataToSend = value;
                SendData(_dataToSend);
                this.OnPropertyChanged("dataToSend");

            }

        }

        public void SendData(string data)
        {

            try
            {


                this.comport.Write(string.Format("{0}\r", data));



            }
            catch (Exception)
            {
                throw new ArgumentException("comPort send problem");
            }

        }

 


        public bool ReadyToSend
        {
            get { return this._ReadyToSend; }
            set { this._ReadyToSend = value; }
        }

 

        public SerialPort comport
        {
            get { return this._comport; }
            set { this._comport = value; }
        }


        public string text
        {
            get { return this._text; }
            set { this._text = value; }
        }


        //Digitals
        public bool NCHRG_ENABLE
        {
            get { return this._NCHRG_ENABLE; }
            set
            {
                this._NCHRG_ENABLE = value;
                this.OnPropertyChanged("NCHRG_ENABLE");

            }
        }
        public System.Windows.Media.Brush NCHRG_ENABLE_COLOUR
        {
            get
            {
                if (_NCHRG_ENABLE)
                {
                    return System.Windows.Media.Brushes.Red;
                }
                else
                {
                    return System.Windows.Media.Brushes.Green;
                }

            }

        }


        public bool DCDC_ENABLE
        {
            get { return this._DCDC_ENABLE; }
            set
            {
                this._DCDC_ENABLE = value;
                this.OnPropertyChanged("DCDC_ENABLE");
            }
        }


        public bool GREEN_LED
        {
            get { return this._GREEN_LED; }
            set
            {
                this._GREEN_LED = value;
                this.OnPropertyChanged("GREEN_LED");
            }
        }
        public System.Windows.Media.Brush GREEN_LED_COLOUR
        {
            get
            {
                if (_GREEN_LED)
                {
                    return System.Windows.Media.Brushes.Green;
                }
                else
                {
                    return System.Windows.Media.Brushes.White;
                }
            }

        }
        public bool RED_LED
        {
            get { return this._RED_LED; }
            set
            {
                this._RED_LED = value;
                this.OnPropertyChanged("RED_LED");
            }
        }
        public System.Windows.Media.Brush RED_LED_COLOUR
        {
            get
            {
                if (_RED_LED)
                {
                    return System.Windows.Media.Brushes.Red;
                }
                else
                {
                    return System.Windows.Media.Brushes.White;
                }
            }

        }
        public bool HYDRATEA
        {
            get { return this._HYDRATEA; }
            set
            {
                this._HYDRATEA = value;
                this.OnPropertyChanged("HYDRATEA");
            }
        }
        public bool HYDRATEB
        {
            get { return this._HYDRATEB; }
            set
            {
                this._HYDRATEB = value;
                this.OnPropertyChanged("HYDRATEB")
                    ;
            }
        }
        public bool V_BAT_MEAS_EN
        {
            get { return this._V_BAT_MEAS_EN; }
            set
            {
                this._V_BAT_MEAS_EN = value;
                this.OnPropertyChanged("V_BAT_MEAS_EN");
            }
        }
    

        public bool ENABLE_LOAD
        {
            get { return this._ENABLE_LOAD; }
            set
            {
                this._ENABLE_LOAD = value;
                this.OnPropertyChanged("ENABLE_LOAD");
            }
        }

        public Nullable<bool> ENABLE_STKA
        {
            get
            {
                return this._ENABLE_STKA;
            }
            set
            {
                this._ENABLE_STKA = value;
                this.OnPropertyChanged("ENABLE_STKA");
            }
        }
 
        public bool ENABLE_STKB
        {
            get { return this._ENABLE_STKB; }
            set
            {
                this._ENABLE_STKB = value;
                this.OnPropertyChanged("ENABLE_STKB");
            }
        }

        public bool ENABLE_BAT
        {
            get { return this._ENABLE_BAT; }
            set
            {
                this._ENABLE_BAT = value;
                this.OnPropertyChanged("ENABLE_BAT");
            }

        }

        public bool CARTRIDGE_PWR
        {
            get { return this._CARTRIDGE_PWR; }
            set { this._CARTRIDGE_PWR = value; }
        }
        public bool DIVIDER_MODE_SEL
        {
            get { return this._DIVIDER_MODE_SEL; }
            set { this._DIVIDER_MODE_SEL = value; }
        }

        //ADCs


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string p)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(
                    this, new PropertyChangedEventArgs(p));
        }
        public string VBOP
        {
            get
            {
                if (this._VBOP != null)
                {
                    return this._VBOP;
                }
                else
                {
                    return "";
                }
            }
            set { this._VBOP = value; }
        }

        public string I_BOP
        {
            get
            {
                if (this._I_BOP != null)
                {
                    return this._I_BOP;
                }
                else
                {
                    return "";
                }
            }
            set { this._I_BOP = value; }
        }
        public string I_DCDC
        {
            get
            {
                if (this._I_DCDC != null)
                {
                    return this._I_DCDC;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                int x;
                if (int.TryParse(value, out x))
                {
                    this._I_DCDC = value;
                    this._I_DCDC_int = Convert.ToInt32(value);
                 //   this._Pout_DCDC = (_I_DCDC_int * _VLOAD_MEAS_int) / 1000000;
                    this.OnPropertyChanged("I_DCDC");
                    this.OnPropertyChanged("Pout_DCDC ");
                }
            }
        }
        //public string Pout_DCDC
        //{
        //    get
        //    {

        //        return string.Format("{0}", _Pout_DCDC);

        //    }
        //}
        public int RX_Error_int
        {
            get
            {
                return this._RX_Error_int;
            }
            set
            {
                this._RX_Error_int = value;
                //this._RX_Good = Convert.ToInt32(value);
                this.OnPropertyChanged("RX_Error_int");
                this.OnPropertyChanged("RX_Error");
            }
        }
        public string RX_Error
        {
            get
            {
                if (this._RX_Error != null)
                {
                    return string.Format("{0}", this._RX_Error_int);
                }
                else
                {
                    return "";
                }
            }
            set
            {
                _RX_Error_int++;
                this._RX_Error = string.Format("{0}", _RX_Error_int);
                this.OnPropertyChanged("RX_Error");
            }
        }
        public int RX_Good_int
        {
            get
            {
                    return this._RX_Good_int;
            }
            set
            {
                this._RX_Good_int = value;
                //this._RX_Good = Convert.ToInt32(value);
                this.OnPropertyChanged("RX_Good_int");
                this.OnPropertyChanged("RX_Good");
            }
        }
        public string RX_Good
        {
            get
            {
                if (this._RX_Good != null)
                {
                    return string.Format("{0}", this._RX_Good_int);
                }
                else
                {
                    return "";
                }
            }
            set
            {
                RX_Good_int++;
                this._RX_Good = string.Format("{0}",RX_Good_int);
                this.OnPropertyChanged("RX_Good");
            }
        }
        public string EXT_5V
        {
            get
            {
                if (this._EXT_5V != null)
                {
                    return this._EXT_5V;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this._EXT_5V = value;
                this.OnPropertyChanged("EXT_5V");
            }
        }

        public string TempA
        {
            get
            {
                if (this._TempA != null)
                {
                    return this._TempA;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this._TempA = value;
                this.OnPropertyChanged("TempA");
            }
        }
        public string TempB
        {
            get
            {
                if (this._TempB != null)
                {
                    return this._TempB;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this._TempB = value;
                this.OnPropertyChanged("TempB");
            }
        }
        public string PRESS_H2
        {
            get
            {
                if (this._PRESS_H2 != null)
                {
                    return this._PRESS_H2;
                }
                else
                {
                    return "";
                }
            }
            set { this._PRESS_H2 = value; }
        }



        //I2C
        public string Register01
        {
            get
            {
                if (this._Register01 != null)
                {
                    return this._Register01;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this._Register01 = value;


                string crop = value.Substring(2, (value.Length - 2));
                int bValue = int.Parse(crop, System.Globalization.NumberStyles.HexNumber);
                int Fault = bValue & 0x0f;
                int STAT = bValue & 0x30;

                switch (STAT)
                {
                    case 0: _STAT = "Ready";
                        break;
                    case 16: _STAT = "Charging";
                        break;
                    case 17: _STAT = "Done";
                        break;
                    case 18: _STAT = "Fault";
                        break;
                    default: break;


                }
                switch (Fault)
                {
                    case 0: _fault = "Normal";
                        break;
                    case 1: _fault = "Input OVP";
                        break;
                    case 2: _fault = "Input UVLO";
                        break;
                    case 3: _fault = "Sleep";
                        break;
                    case 4: _fault = "Bat Temp Fault";
                        break;
                    case 5: _fault = "Bat OVP";
                        break;
                    case 6: _fault = "Thermal Shutdown";
                        break;
                    case 7: _fault = "Timer Fault";
                        break;
                    case 8: _fault = "No Bat";
                        break;
                    case 9: _fault = "ISET short";
                        break;
                    case 10: _fault = "Input Fault & LDO low";
                        break;

                    default: break;
                }

            }
        }
        public string Register02
        {
            get
            {
                if (this._Register02 != null)
                {
                    return this._Register02;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this._Register02 = value;

                string crop = value.Substring(2, (value.Length - 2));
                int bValue = int.Parse(crop, System.Globalization.NumberStyles.HexNumber);
                int Fault = bValue & 0x0f;
                int Iin_limit = bValue & 0x70;
                int EN_STAT = bValue & 0x8;
                int EN_TERM = bValue & 0x4;
                int nCE = bValue & 0x2;

                switch (Iin_limit)
                {
                    case 0: _Iin_limit = "100mA";
                        break;
                    case 16: _Iin_limit = "150mA";
                        break;
                    case 32: _Iin_limit = "500mA";
                        break;
                    case 48: _Iin_limit = "900mA";
                        break;
                    case 64: _Iin_limit = "1500mA";
                        break;
                    case 80: _Iin_limit = "2000mA";
                        break;
                    case 96: _Iin_limit = "Ext ILIM";
                        break;
                    case 112: _Iin_limit = "none";
                        break;
                    default: break;
                }
                switch (EN_STAT)
                {
                    case 0: _EN_STAT = false;
                        break;
                    case 8: _EN_STAT = true;
                        break;
                    default: break;
                }
                switch (EN_TERM)
                {
                    case 0: _EN_TERM = false;
                        break;
                    case 4: _EN_TERM = true;
                        break;
                    default: break;
                }
                switch (nCE)
                {
                    case 0: _nCE = false;
                        break;
                    case 2: _nCE = true;
                        break;
                    default: break;
                }

            }
        }
        public string Register03
        {
            get
            {
                if (this._Register03 != null)
                {
                    return this._Register03;
                }
                else
                {
                    return "";
                }
            }
            set { this._Register03 = value; }
        }
        public string Register04
        {
            get
            {
                if (this._Register04 != null)
                {
                    return this._Register04;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this._Register04 = value;


                string crop = value.Substring(2, (value.Length - 2));
                int bValue = int.Parse(crop, System.Globalization.NumberStyles.HexNumber);
                bool bValue_b7 = (bValue & (1 << 7)) != 0;
                bool bValue_b6 = (bValue & (1 << 6)) != 0;
                bool bValue_b5 = (bValue & (1 << 5)) != 0;
                bool bValue_b4 = (bValue & (1 << 4)) != 0;
                bool bValue_b3 = (bValue & (1 << 3)) != 0;
                bool bValue_b2 = (bValue & (1 << 2)) != 0;
                bool bValue_b1 = (bValue & (1 << 1)) != 0;
                bool bValue_b0 = (bValue & (1 << 0)) != 0;

                int I_chg = 500;

                if (bValue_b7) { I_chg += 800; }
                if (bValue_b6) { I_chg += 400; }
                if (bValue_b5) { I_chg += 200; }
                if (bValue_b4) { I_chg += 100; }
                if (bValue_b3) { I_chg += 50; }

                if ((500 <= I_chg) && (I_chg <= 2000))
                {
                    _I_chg_int = I_chg;
                    _I_chg_str = "" + I_chg;

                }

            }
        }
        public string Register05
        {
            get
            {
                if (this._Register05 != null)
                {
                    return this._Register05;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this._Register05 = value;

                string crop = value.Substring(2, (value.Length - 2));
                int bValue = int.Parse(crop, System.Globalization.NumberStyles.HexNumber);
                int loop_stat = bValue & 0xC0;
                int lowChg = bValue & 0x20;
                int DPDM = bValue & 0x10;
                int CEstat = bValue & 0x8;
                int VinDPM = bValue & 0x7;

                switch (loop_stat)
                {
                    case 0: _LOOP_STATUS = "no loop";
                        break;
                    case 0x40: _LOOP_STATUS = "V reg loop";
                        break;
                    case 0x80: _LOOP_STATUS = "I lim loop";
                        break;
                    case 0xC0: _LOOP_STATUS = "Therm loop";
                        break;
                    default: break;
                }
                switch (lowChg)
                {
                    case 0: _LOW_CHG = false;
                        break;
                    case 0x20: _LOW_CHG = true;
                        break;
                    default: break;
                }
                switch (DPDM)
                {
                    case 0: _DPDM_EN = false;
                        break;
                    case 0x10: _DPDM_EN = true;
                        break;
                    default: break;
                }
                switch (CEstat)
                {
                    case 0: _CE_STATUS = false;
                        break;
                    case 0x8: _CE_STATUS = true;
                        break;
                    default: break;
                }
                switch (VinDPM)
                {
                    case 0: _VINDPM = "4.2V";
                        break;
                    case 1: _VINDPM = "4.28V";
                        break;
                    case 2: _VINDPM = "4.36V";
                        break;
                    case 3: _VINDPM = "4.44V";
                        break;
                    case 4: _VINDPM = "4.52V";
                        break;
                    case 5: _VINDPM = "4.6V";
                        break;
                    case 6: _VINDPM = "4.68V";
                        break;
                    case 7: _VINDPM = "4.76V";
                        break;
                    default: break;
                }

            }
        }


        public string Register06
        {
            get
            {
                if (this._Register06 != null)
                {
                    return this._Register06;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this._Register06 = value;

                string crop = value.Substring(2, (value.Length - 2));
                int bValue = int.Parse(crop, System.Globalization.NumberStyles.HexNumber);
                int tmren = bValue & 0x80;
                int tmr = bValue & 0x60;
                int sysoff = bValue & 0x10;
                int tsstat = bValue & 0x7;


                switch (tmren)
                {
                    case 0: _Two_xTMR_EN = false;
                        break;
                    case 0x80: _Two_xTMR_EN = true;
                        break;
                    default: break;
                }
                switch (tmr)
                {
                    case 0: _TMR = "0.75h";
                        break;
                    case 0x20: _TMR = "6h";
                        break;
                    case 0x40: _TMR = "9h";
                        break;
                    case 0x60: _TMR = "dis";
                        break;
                    default: break;
                }
                switch (sysoff)
                {
                    case 0: _SYSOFF = false;
                        break;
                    case 0x10: _SYSOFF = true;
                        break;
                    default: break;
                }
                switch (tsstat)
                {
                    case 0: _TS_STAT = "normal";
                        break;
                    case 0x4: _TS_STAT = "< cold";
                        break;
                    case 0x5: _TS_STAT = "> freeze";
                        break;
                    case 0x6: _TS_STAT = "< freeze";
                        break;
                    case 0x7: _TS_STAT = "open";
                        break;
                    default: break;
                }

            }
        }
        public string Register07
        {
            get
            {
                if (this._Register07 != null)
                {
                    return this._Register07;
                }
                else
                {
                    return "";
                }
            }
            set { this._Register07 = value; }
        }

        //Reg#1 decode
        public string fault
        {
            get
            {
                if (this._fault != null)
                {
                    return this._fault;
                }
                else
                {
                    return "";
                }
            }
            set { this._fault = value; }
        }


        public string STAT
        {
            get
            {
                if (this._STAT != null)
                {
                    return this._STAT;
                }
                else
                {
                    return "";
                }
            }
            set { this._STAT = value; }
        }

        //Reg#2 decode

        public string DataDump
        {
            get
            {
                if (this._DataDump != null)
                {
                    return this._DataDump;
                }
                else
                {
                    return "";
                }
            }
            set { this._DataDump = value; }
        }

        public string InDaq
        {
            get
            {
                if (this._InDaq != null)
                {
                    return this._InDaq;
                }
                else
                {
                    return "";
                }
            }
            set { this._InDaq = value; }
        }

        public string OutDaq
        {
            get
            {
                if (this._OutDaq != null)
                {
                    return this._OutDaq;
                }
                else
                {
                    return "";
                }
            }
            set { this._OutDaq = value; }
        }


        public string InletValve
        {
            get
            {
                if (this._InletValve != null)
                {
                    return this._InletValve;
                }
                else
                {
                    return "";
                }
            }
            set { this._InletValve = value; }
        }


        public string PurgeValve
        {
            get
            {
                if (this._PurgeValve != null)
                {
                    return this._PurgeValve;
                }
                else
                {
                    return "";
                }
            }
            set { this._PurgeValve = value; }
        }

        public string AdcTrigger
        {
            get
            {
                if (this._AdcTrigger != null)
                {
                    return this._AdcTrigger;
                }
                else
                {
                    return "";
                }
            }
            set { this._AdcTrigger = value; }
        }

        public string Fan1
        {
            get
            {
                if (this._Fan1 != null)
                {
                    return this._Fan1;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this._Fan1 = value;
                this.OnPropertyChanged("Fan1");
            }
        }

        public string Fan2
        {
            get
            {
                if (this._Fan2 != null)
                {
                    return this._Fan2;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this._Fan2 = value;
                this.OnPropertyChanged("Fan2");
            }
        }

        public string StkPreload
        {
            get
            {
                if (this._StkPreload != null)
                {
                    return this._StkPreload;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this._StkPreload = value;
                this.OnPropertyChanged("StkPreload");
            }
        }

        public string IdcdcLimit
        {
            get
            {
                if (this._IdcdcLimit != null)
                {
                    return this._IdcdcLimit;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this._IdcdcLimit = value;
                this.OnPropertyChanged("IdcdcLimit");
            }
        }


        public string Iin_limit
        {
            get
            {
                if (this._Iin_limit != null)
                {
                    return this._Iin_limit;
                }
                else
                {
                    return "";
                }
            }
            set { this._Iin_limit = value; }
        }

        public bool EN_STAT
        {
            get
            {
                return this._EN_STAT;
            }
            set { this._EN_STAT = value; }
        }

        public bool EN_TERM
        {
            get
            {
                return this._EN_TERM;
            }
            set { this._EN_TERM = value; }
        }

        public bool nCE
        {
            get
            {
                return this._nCE;
            }
            set { this._nCE = value; }
        }

        //Reg#3 decode

        //Reg#4 decode
        public int I_chg_int
        {
            get
            {
                if ((this._I_chg_int >= 500) & (this._I_chg_int <= 2000))
                {
                    return this._I_chg_int;
                }
                else
                {
                    return 500;
                }
            }
            set
            {
                this._I_chg_int = value;
            }
        }

        public string I_chg_str
        {
            get
            {
                if (this._I_chg_str != null)
                {
                    return this._I_chg_str;
                }
                else
                {
                    return "";
                }
            }
            set { this._I_chg_str = value; }
        }

        //Reg#5 decode
        public string LOOP_STATUS
        {
            get
            {
                if (this._LOOP_STATUS != null)
                {
                    return this._LOOP_STATUS;
                }
                else
                {
                    return "";
                }
            }
            set { this._LOOP_STATUS = value; }
        }
        public bool LOW_CHG
        {
            get
            {
                return this._LOW_CHG;
            }
            set { this._LOW_CHG = value; }
        }
        public bool DPDM_EN
        {
            get
            {
                return this._DPDM_EN;
            }
            set { this._DPDM_EN = value; }
        }
        public bool CE_STATUS
        {
            get
            {
                return this._CE_STATUS;
            }
            set { this._CE_STATUS = value; }
        }
        public string VINDPM
        {
            get
            {
                if (this._VINDPM != null)
                {
                    return this._VINDPM;
                }
                else
                {
                    return "";
                }
            }
            set { this._VINDPM = value; }
        }
        //Reg#6 decode
        public bool Two_xTMR_EN
        {
            get
            {
                return this._Two_xTMR_EN;
            }
            set { this._Two_xTMR_EN = value; }
        }
        public string TMR
        {
            get
            {
                if (this._TMR != null)
                {
                    return this._TMR;
                }
                else
                {
                    return "";
                }
            }
            set { this._TMR = value; }
        }
        public bool SYSOFF
        {
            get
            {
                return this._SYSOFF;
            }
            set { this._SYSOFF = value; }
        }
        public string TS_STAT
        {
            get
            {
                if (this._TS_STAT != null)
                {
                    return this._TS_STAT;
                }
                else
                {
                    return "";
                }
            }
            set { this._TS_STAT = value; }
        }

    }


}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Threading;
using System.ComponentModel;
using System.IO.Ports;

namespace WolfAC10_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MCU_READ _mcu_read; // = new MCU_READ();
        private BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        System.Windows.Threading.DispatcherTimer timer1 = new System.Windows.Threading.DispatcherTimer();
        private TextBox _textBox;
        ConsoleContent dc = new ConsoleContent();
       // DiodeContent Diode_Stk_A = new DiodeContent();
        private bool _InFrame = false;
        const int DLE = 0x10;
        const int STX = 0x2;
        private byte _DLEflag = 0;
        private int _PortArrayCount = 0;
        bool fullFrame = false;
        private string[] _PortArray = new string[200];
        private int _PortArrayMax = 200;

        public MainWindow()
        {
            InitializeComponent();
            timer1.Tick += timer1_Tick;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            _textBox = new TextBox();
          //  DataContext = dc;
            
        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           // InputBlock.KeyDown += InputBlock_KeyDown;
          //  InputBlock.Focus();
        }
        //private void InputBlock_KeyDown_1(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        dc.ConsoleInput = InputBlock.Text;
        //        _mcu_read.SendData(InputBlock.Text);
        //        dc.RunCommand();
        //        InputBlock.Focus();
        //        Scroller.ScrollToBottom();
        //       // _mcu_read.SendData(InputBlock.Text);
        //    }
        //    else
        //    {

        //    }
        //}
        //void InputBlock_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.H)//Key.Enter)
        //    {
        //        dc.ConsoleInput = InputBlock.Text;
        //        dc.RunCommand();
        //        InputBlock.Focus();
        //        Scroller.ScrollToBottom();
        //    }
        //    else
        //    {

        //    }
        //}

        private void toggle_button_colour()
        {
            // toggle auto read button
            //if (auto_read_button.Background == System.Windows.Media.Brushes.Red)
            //{
            //    auto_read_button.Background = System.Windows.Media.Brushes.LightSalmon;
            //}
            //else
            //{
            //    auto_read_button.Background = System.Windows.Media.Brushes.Red;
            //}
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            toggle_button_colour();

            Read_All();

            write_log_line();
        }

        private void write_log_line()
        {
            if (Properties.Settings.Default.enableLogging)
            {
                try
                {
                    // log to file
                    ReadWriteCSV.CsvFileWriterAppend CSV_W = new ReadWriteCSV.CsvFileWriterAppend(Properties.Settings.Default.logFile);

                    ReadWriteCSV.CsvRow row = new ReadWriteCSV.CsvRow();
                    //row.Add(DateTime.Now.ToString());
                    //CSV_W.WriteRow(row);

                    //row.Clear();
                    //ADCs
                    row.Add(String.Format("{0}", DateTime.Now));
                    row.Add(String.Format("{0}", _mcu_read.Load.Volt));
                    row.Add(String.Format("{0}", _mcu_read.Load.Amp));
                    row.Add(String.Format("{0}", _mcu_read.ch_A1.Volt));
                    row.Add(String.Format("{0}", _mcu_read.ch_A1.Amp));
                    row.Add(String.Format("{0}", _mcu_read.ch_B1.Volt));
                    row.Add(String.Format("{0}", _mcu_read.ch_B1.Amp));
                    row.Add(String.Format("{0}", _mcu_read.bat_op.Volt));
                    row.Add(String.Format("{0}", _mcu_read.bat_op.Amp));
                    row.Add(String.Format("{0}", _mcu_read.EXT_5V));
                    row.Add(String.Format("{0}", _mcu_read.TempA));
                    row.Add(String.Format("{0}", _mcu_read.TempB));




                    //Digitals
                    row.Add(String.Format("{0}", _mcu_read.DCDC_ENABLE));
                    row.Add(String.Format("{0}", _mcu_read.GREEN_LED));
                    row.Add(String.Format("{0}", _mcu_read.RED_LED));
                    row.Add(String.Format("{0}", _mcu_read.HYDRATEA));
                    // row.Add(String.Format("{0}", _mcu_read.V_BAT_MEAS_EN));
                    row.Add(String.Format("{0}", _mcu_read.ENABLE_LOAD));
                    row.Add(String.Format("{0}", _mcu_read.ENABLE_STKA));
                    row.Add(String.Format("{0}", _mcu_read.ENABLE_STKB));
                    row.Add(String.Format("{0}", _mcu_read.ENABLE_BAT));
                    // row.Add(String.Format("{0}", _mcu_read.NCHRG_ENABLE));
                    // row.Add(String.Format("{0}", _mcu_read.CARTRIDGE_PWR));
                    // row.Add(String.Format("{0}", _mcu_read.DIVIDER_MODE_SEL));

                    ////I2C
                    //row.Add(String.Format("{0}", _mcu_read.Register01));
                    //row.Add(String.Format("{0}", _mcu_read.Register02));
                    //row.Add(String.Format("{0}", _mcu_read.Register03));
                    //row.Add(String.Format("{0}", _mcu_read.Register04));
                    //row.Add(String.Format("{0}", _mcu_read.Register05));
                    //row.Add(String.Format("{0}", _mcu_read.Register06));
                    //row.Add(String.Format("{0}", _mcu_read.Register07));

                    ////I2C decode
                    ////#1
                    //row.Add(String.Format("{0}", _mcu_read.fault));
                    //row.Add(String.Format("{0}", _mcu_read.STAT));
                    ////#2
                    //row.Add(String.Format("{0}", _mcu_read.Iin_limit));
                    //row.Add(String.Format("{0}", _mcu_read.EN_STAT));
                    //row.Add(String.Format("{0}", _mcu_read.EN_TERM));
                    //row.Add(String.Format("{0}", _mcu_read.nCE));
                    ////#3
                    ////row.Add(String.Format("BAT VbatReg"));
                    ////row.Add(String.Format("BAT USB_DET"));
                    ////#4
                    //row.Add(String.Format("{0}", _mcu_read.I_chg_str));
                    ////#5
                    //row.Add(String.Format("{0}", _mcu_read.LOOP_STATUS));
                    //row.Add(String.Format("{0}", _mcu_read.LOW_CHG));
                    //row.Add(String.Format("{0}", _mcu_read.DPDM_EN));
                    //row.Add(String.Format("{0}", _mcu_read.CE_STATUS));
                    //row.Add(String.Format("{0}", _mcu_read.VINDPM));
                    ////#6
                    //row.Add(String.Format("{0}", _mcu_read.Two_xTMR_EN));
                    //row.Add(String.Format("{0}", _mcu_read.TMR));
                    //row.Add(String.Format("{0}", _mcu_read.SYSOFF));
                    //row.Add(String.Format("{0}", _mcu_read.TS_STAT));
                    ////#7


                    CSV_W.WriteRow(row);

                    CSV_W.Close();
                }
                catch
                {
                    Properties.Settings.Default.enableLogging = false;
                    Properties.Settings.Default.Save();
                   // enableLoggingToolStripMenuItem.IsChecked = Properties.Settings.Default.enableLogging;
                    

                    MessageBox.Show("log file error, logging disabled");

                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ComPort_Connect();
        }

        
        private void ComPort_Disconnect()
        {
            backgroundWorker1.CancelAsync();
            _mcu_read.close();
            
        }
        
        private void ComPort_Connect()
        {
           
            if (comboBox1.Text == "")
            {
                MessageBox.Show(this, "No Comport selected.  Please select comport from drop down menu.", "", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                try
                {

                    // base.DataContext = new MCU_READ(comboBox1.Text);

                    _mcu_read = new MCU_READ(comboBox1.Text);
                    // base.DataContext = _mcu_read;
                    DataContext = new
                    {
                        info = _mcu_read,
                        ch_A1 = _mcu_read.ch_A1,
                        ch_B1 = _mcu_read.ch_B1,
                        bat_op = _mcu_read.bat_op,
                        bat_ip = _mcu_read.bat_ip,
                        load = _mcu_read.Load,
                        extendedInfo = dc,
                        // moreInfo = Diode_Stk_A,
                    };

                    if (_mcu_read.comport.IsOpen)
                    {
                        backgroundWorker1.RunWorkerAsync();
                        Read_All();
                        // DisableControls(this, true);
                    }
                    //cerate new log file
                    write_Log_Header();
                    write_log_line();

                }
                catch
                {
                    MessageBox.Show(this, "Could not open the COM port.  Most likely it is already in use, has been removed, or is unavailable.", "COM Port Unavalible", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Read_All()
        {//dataDump
            //Thread.Sleep(40);
            //GPIO_READ_ALL();
            //Thread.Sleep(40);
            //// _mcu_read.SendData("batchg_r *");  

            //Thread.Sleep(40);
            //_mcu_read.SendData("adc_r *");

            //Thread.Sleep(40);
            ////_mcu_read.SendData("gpio_r DCDC_ENABLE");
            ////Thread.Sleep(40);
        }
        private void GPIO_READ_ALL()
        {
            Thread.Sleep(40);
            _mcu_read.SendData(GPIO.read(GPIO.ALL));
        }
        private string get_comms()
        {
            // If the com port has been closed, message box, return -1;
            if (!_mcu_read.comport.IsOpen)
            {
                MessageBox.Show("comport not open");
                return "-2";
            }
            else
            {
                try
                {
                    string read = _mcu_read.comport.ReadLine();
                    //if(read.Count() >=1)
                    //{
                    //    MCU_READ.linesToRead--;
                    //}
                    return read;      //returns -1 if end of stream has been read
                }
                catch
                {
                    return "-1";
                }
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            bool Do_Work = true;

            while (Do_Work)
            {

                string strUART = "";

                strUART = get_comms();


                processRawData(true, strUART);
               
                if (strUART.Count() <= 2)
                {
                    Thread.Sleep(200);
                }

                if (strUART == "-1")
                {
                    // reached end of buffer
                    //So we can stop the background worker for now start again on buffer RX
                    //backgroundWorker1.CancelAsync();
                    //Do_Work = false;
                }
                if (strUART == "-2")
                {
                    Do_Work = false;
                }

                /*
                 * 
                 */

                string[] UART_string_array = strUART.Split(new char[] { ' ' });
                int com_string_array_length = UART_string_array.Count() - 1;
                string UART_command = UART_string_array[0];
                string UART_val = "";
                if (com_string_array_length == 1)
                {
                    if (UART_string_array[1].Length >= 2)
                    {
                        UART_val = UART_string_array[1];
                        UART_val = UART_val.Remove(UART_val.Length - 2);
                    }

                }

                
                /*Add in coede to do comma separated values  */

                parseCommandLine(UART_string_array, UART_command, UART_val);


                backgroundWorker1.ReportProgress(1, strUART);

            }



        }

        /// <summary>
        /// Checks for start of framw DLE STX
        /// </summary>
        /// <param name="com_byte"></param>
        /// <returns></returns>
        private bool DleStxCheck(string com_byte)
        {
            if (com_byte == "\rSTX,")                      //Possible SoF
            {
                _DLEflag = 1;
                return true;
            }

            //if (_DLEflag == 1 & com_byte == "\rETX,")          //SoF Indication
            //{
            //    _DLEflag = 0;
            //    return true;
            //}
            return false;
        }
        // <summary>
        // puts bytes into byte array true when _PortArrayCount equals _PktLengthInt 
        // </summary>
        // <param name="com_byte"></param>
        // <returns>true when _PortArrayCount equals _PktLengthInt </returns>
        //private bool fillPortArray(string com_byte)
        //{
        //    _PortArray[_PortArrayCount] = com_byte;
        //    //if (_PortArrayCount == 2)                                                         //Pull out packet length once we have it
        //    //{
        //    //    _PktLengthInt = _PortArray[2] + 2;   //Add 2 because my length includes DLE and STX bytes but DLE byte is always 0 not x10
        //    //}
        //    //if (_PktLengthInt > _PortArrayMax - 1)   //ERROR THE LENGTH IS WAY TO BIG!!!
        //    //{
        //    //    _InFrame = false;
        //    //    _PortArrayCount = 0;
        //    //    _PktLengthInt = 0;
        //    //    for (int i = 0; i < _PortArrayMax; i++)
        //    //    {
        //    //        _PortArray[i] = 1;                          //Clear Down Array
        //    //    }
        //    //}

        //    //if ((_PortArrayCount == _PktLengthInt) & (_PortArrayCount > 3))
        //    //{ return true; }
        //    //else
        //    //{ return false; }
        //}
        private bool processRawData(bool Do_Work, string com_byte)
        {

            

            if (!_InFrame)
            {
                if (DleStxCheck(com_byte))
                {
                    _PortArrayCount = 0;
                    _InFrame = true;
                }
            }
            if (_InFrame) //Put bytes into array
            {
                bool frameEnd = false;

                if (com_byte == "\rETX,")                           //ETX so end of frame
                { frameEnd = true; } 
                
                //***   Check for SOF or EOF
                if (DleStxCheck(com_byte) & (_PortArrayCount != 0))      // In a frame but we see another STX So start again!
                {
                    _mcu_read.RX_Error = "";  // no value needed to count errors
                    frameRestart(); }
                else
                {
                    string UART_val = "";
                    int x;
                    if (com_byte.Length >= 2)                       //Is it a good byte?? length 
                    {
                        UART_val = com_byte;
                        UART_val = UART_val.Substring(1);
                        UART_val = UART_val.Remove(UART_val.Length - 1);

                        
                        if (DleStxCheck(com_byte))  //STX do nothing
                        {
                            _PortArray[_PortArrayCount] = UART_val;
                        }
                        else if (com_byte == "\rETX,")  //ETX check it is the EOF and handle data
                        {
                            _PortArray[_PortArrayCount] = UART_val;

                            if (_PortArrayCount >= 30)  // Have we got a full frame
                            {
                                fullFrame = true;
                            }
                            else
                            {
                                fullFrame = false;
                            }

                            if (frameEnd & fullFrame)      // full frame and frame end
                            {

                                int n = 1;
                                _mcu_read.RED_LED = Convert.ToBoolean(Convert.ToInt32(_PortArray[n++]));    // RedLed:  Red LED on [on], off [off]
                                _mcu_read.GREEN_LED = Convert.ToBoolean(Convert.ToInt32(_PortArray[n++]));  //GrnLed:  Green LED on [on], off [off]
                                n++;    //CartPwr:  Cartridge Power on [on], off [off]
                                _mcu_read.ENABLE_STKA = Convert.ToBoolean(Convert.ToInt32(_PortArray[n++]));  //    StkA:  Stack A Enable [en], disable [dis]
                                _mcu_read.ENABLE_STKB = Convert.ToBoolean(Convert.ToInt32(_PortArray[n++])); //    StkB:  Stack B Enable [en], disable [dis]
                                _mcu_read.ENABLE_BAT = Convert.ToBoolean(Convert.ToInt32(_PortArray[n++])); //    Batt:  Battery Enable [en], disable [dis]
                                n++; //    McuWake:  MCU Wake Enable [en], disable [dis]
                                _mcu_read.ENABLE_LOAD = Convert.ToBoolean(Convert.ToInt32(_PortArray[n++])); //    Load:  Load Enable [en], disable [dis]
                                n++; //    DivModeSel:  Divider Mode Select on [on], off [off]
                                _mcu_read.DCDC_ENABLE = Convert.ToBoolean(Convert.ToInt32(_PortArray[n++])); //    DCDC:  DCDC Enable [en], disable [dis]
                                _mcu_read.NCHRG_ENABLE = Convert.ToBoolean(Convert.ToInt32(_PortArray[n++])); //    Charge:  Charge Enable [en], disable [dis]
                                _mcu_read.V_BAT_MEAS_EN = Convert.ToBoolean(Convert.ToInt32(_PortArray[n++]));//    VbatMeas:  Battery Measurement Enable [en], disable [dis]
                                _mcu_read.HYDRATEA = Convert.ToBoolean(Convert.ToInt32(_PortArray[n++]));     //    HydStkA:  Hydrate Stack A [on], disable [off]
                                _mcu_read.HYDRATEB = Convert.ToBoolean(Convert.ToInt32(_PortArray[n++]));      //    HydStkB:  Hydrate Stack B [on], disable [off]
                                _mcu_read.EXT_5V = _PortArray[n++];                                             //    Ext5V:  External 5V Input
                                //_mcu_read.VLOAD_MEAS = _PortArray[n++];//    Vload:  Load Output Voltage
                                _mcu_read.Load.Volt = _PortArray[n++];//    Vload:  Load Output Voltage
                                //_mcu_read.I_BAT = _PortArray[n++];   //    Ibat:  Battery Output Current
                                _mcu_read.bat_op.Amp = _PortArray[n++];
                               // _mcu_read.V_STKA = _PortArray[n++];//    VstkA:  Stack A Output Voltage
                                _mcu_read.ch_A1.Volt = _PortArray[n++];
                                _mcu_read.I_DCDC = _PortArray[n++]; //    Idcdc:  DCDC Output Current
                                //_mcu_read.I_LOAD = _PortArray[n++]; //    Iload:  Load Output Current
                                _mcu_read.Load.Amp = _PortArray[n++]; //    Iload:  Load Output Current
                               // _mcu_read.V_STKB = _PortArray[n++]; //    VstkB:  Stack B Output Voltage
                                _mcu_read.ch_B1.Volt = _PortArray[n++];
                                //_mcu_read.I_STKB = _PortArray[n++]; //    IstkB:  Stack B Output Current
                                _mcu_read.ch_B1.Amp = _PortArray[n++];//
                                _mcu_read.TempB = _PortArray[n++]; //    TempStkB:  Stack B Temperature
                               // _mcu_read.V_BAT = _PortArray[n++]; //    Vbat:  Battery Output Voltage
                                _mcu_read.bat_op.Volt = _PortArray[n++];
                               // _mcu_read.I_STKA = _PortArray[n++]; //    IstkA:  Stack A Output Current
                                _mcu_read.ch_A1.Amp = _PortArray[n++];//    VstkA:  Stack A Output Voltage
                                _mcu_read.TempA = _PortArray[n++]; //    TempStkA:  Stack A Temperature
                                _mcu_read.IdcdcLimit = _PortArray[n++];  //    IdcdcLimit:  DCDC Current Limit - Min 0, Max 20000 mA
                                _mcu_read.StkPreload = _PortArray[n++]; //    StkPreload:  Stack Preload - Min 0, Max 10000 mV
                                _mcu_read.Fan1 = _PortArray[n++]; //    Fan1:  Fan 1 Speed - Min 0, Max 100 %
                                _mcu_read.Fan2 = _PortArray[n++];  //    Fan2:  Fan 2 Speed - Min 0, Max 100 %
                                _mcu_read.AdcTrigger = _PortArray[n++]; //    AdcTrigger:  ADC Trigger - Min 0, Max 100 %
                                _mcu_read.PurgeValve = _PortArray[n++]; //    PurgeValve:  Purge Valve - Min 0, Max 100 %
                                _mcu_read.InletValve = _PortArray[n++];  //    InletValve:  Inlet Valve - Min 0, Max 100 %
                                _mcu_read.OutDaq = _PortArray[n++]; //    OutDaq:  DAQ outputs enable [en], disable [dis]
                                _mcu_read.InDaq = _PortArray[n++]; //    InDaq:  DAQ inputs enable [en], disable [dis]
                                _mcu_read.DataDump = _PortArray[n++];  //    DataDump:  Data Dump Period - Min 100, Max 10000 ms
                                _mcu_read.RX_Good ="";  // no value needed to count errors
                                write_log_line();
                                frameRestart();
                            } //if frame end
                        }
                        else if (UART_val.Except("-0123456789").Any())   // if it's not one of these
                        {
                            _mcu_read.RX_Error = "";  // no value needed to count errors
                            frameRestart();
                            //  throw new Exception("Invalid character.");
                        }
                        else
                        {
                            if (int.TryParse(UART_val, out x))          //Is it a good byte?? can be made and interger
                            {
                                _PortArray[_PortArrayCount] = UART_val;
                            }
                            else
                            {
                                _mcu_read.RX_Error = "";  // no value needed to count errors
                                frameRestart();
                                //  throw new Exception("Invalid character.");
                            }
     
                        }
    
                    }
                   else
                   {
                    frameRestart();
                   }
                    
                }
                if (_PortArrayCount < _PortArrayMax - 1)
                {
                    _PortArrayCount++;
                }
                else
                { //PortArrayCount Error
                    _mcu_read.RX_Error = "";  // no value needed to count errors
                    frameRestart();
                }

                 
            }
            return Do_Work;
        }

        private void frameRestart()
        {
            _InFrame = false;
            _PortArrayCount = 0;
            fullFrame = false;

            for (int i = 0; i < _PortArrayMax; i++)
            {
                _PortArray[i] = "";                          //Clear Down Array
            }
        }

        private void parseCommandLine(string[] UART_string_array, string UART_command, string UART_val)
        {
            switch (UART_command)
            {

                /*
                 * BAT_I_0: BAT_I_1: BAT_I_2: BAT_I_3: 
                 * CARTRIDGE_PWR: VALVE_OPEN_CLOSE: VALVE_DRIVE_EN: 
                 * ENABLE_MAINS: ENABLE_STK: ENABLE_BAT: MCU_WAKE_EN: USB_EN: DIVIDER_MODE_SEL: DCDC_ENABLE: 
                 * NCHRG_ENABLE: V_BAT_MEAS_EN: HYDRATE: RED_LED: GREEN_LED: */

                case "\r" + GPIO.NCHRG_ENABLE + ":":
                    //case "\rNCHRG_ENABLE:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.NCHRG_ENABLE = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    { _mcu_read.NCHRG_ENABLE = false; }
                    else { break; }
                    break;

                case "\r" + GPIO.DCDC_ENABLE + ":":
                    //case "\rDCDC_ENABLE:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.DCDC_ENABLE = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    { _mcu_read.DCDC_ENABLE = false; }
                    else { break; }
                    break;

                case "\r" + GPIO.GREEN_LED + ":":
                    // case "\rGREEN_LED:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.GREEN_LED = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    { _mcu_read.GREEN_LED = false; }
                    else { break; }
                    break;

                case "\r" + GPIO.ENABLE_LOAD + ":":
                    //case "\rDCDC_ENABLE:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.ENABLE_LOAD = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    { _mcu_read.ENABLE_LOAD = false; }
                    else { break; }
                    break;

                case "\rRED_LED:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.RED_LED = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    { _mcu_read.RED_LED = false; }
                    else { break; }
                    break;

                case "\rHYDRATE:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.HYDRATEA = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    { _mcu_read.HYDRATEA = false; }
                    else { break; }
                    break;

                case "\rV_BAT_MEAS_EN:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.V_BAT_MEAS_EN = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    { _mcu_read.V_BAT_MEAS_EN = false; }
                    else { break; }
                    break;

                case "\rDIVIDER_MODE_SEL:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.DIVIDER_MODE_SEL = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    { _mcu_read.DIVIDER_MODE_SEL = false; }
                    else { break; }
                    break;

                case "\rCARTRIDGE_PWR:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.CARTRIDGE_PWR = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    { _mcu_read.CARTRIDGE_PWR = false; }
                    else { break; }
                    break;


                case "\rUSB_EN:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.ENABLE_LOAD = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    { _mcu_read.ENABLE_LOAD = false; }
                    else { break; }
                    break;

                case "\rENABLE_STKA:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.ENABLE_STKA = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    { _mcu_read.ENABLE_STKA = false; }
                    else { break; }
                    break;

                case "\rENABLE_STKB:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.ENABLE_STKB = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    { _mcu_read.ENABLE_STKB = false; }
                    else { break; }
                    break;

                case "\rENABLE_BAT:":
                    if (UART_string_array[UART_string_array.Count() - 1] == "1")
                    { _mcu_read.ENABLE_BAT = true; }
                    else if (UART_string_array[UART_string_array.Count() - 1] == "0")
                    {
                        _mcu_read.ENABLE_BAT = false;
                    }
                    else { break; }
                    break;
                //I2C
                case "\rRegister01": _mcu_read.Register01 = UART_string_array[UART_string_array.Count() - 1];
                    break;
                case "\rRegister02": _mcu_read.Register02 = UART_string_array[UART_string_array.Count() - 1];
                    break;
                case "\rRegister03": _mcu_read.Register03 = UART_string_array[UART_string_array.Count() - 1];
                    break;
                case "\rRegister04": _mcu_read.Register04 = UART_string_array[UART_string_array.Count() - 1];
                    break;
                case "\rRegister05": _mcu_read.Register05 = UART_string_array[UART_string_array.Count() - 1];
                    break;
                case "\rRegister06": _mcu_read.Register06 = UART_string_array[UART_string_array.Count() - 1];
                    break;
                case "\rRegister07": _mcu_read.Register07 = UART_string_array[UART_string_array.Count() - 1];
                    break;

                //ADC
                /*
                 * EXT_5V: V_DROP: VUSB_MEAS: I_BAT: V_STK: DCDC_IN: I_USB: VBOP: I_BOP: PRESS_H2: V_BAT: I_STK: TEMP_STK: */

                case "\rTEMP_STKA:":
                    _mcu_read.TempA = UART_val;
                    break;
                case "\rTEMP_STKB:":
                    _mcu_read.TempB = UART_val;
                    break;
                case "\rPRESS_H2:":
                    _mcu_read.PRESS_H2 = UART_val;
                    break;
                case "\rEXT_5V:": _mcu_read.EXT_5V = UART_val;
                    break;
              //  case "\rV_DROP:": _mcu_read.V_DROP = UART_val;
                //    break;
                case "\rV_LOAD:": _mcu_read.Load.Volt = UART_val;
                    break;
                case "\rI_BAT:": _mcu_read.bat_op.Amp = UART_val;
                    break;
                case "\rV_STKB:": _mcu_read.ch_B1.Volt = UART_val;
                    break;
                case "\rV_STKA:": _mcu_read.ch_A1.Volt = UART_val;
                    break;
                case "\rI_LOAD:": _mcu_read.Load.Amp = UART_val;
                    break;
                case "\rVBOP:": _mcu_read.VBOP = UART_val;
                    break;
                case "\rI_BOP:": _mcu_read.I_BOP = UART_val;
                    break;
                case "\rI_DCDC:": _mcu_read.I_DCDC = UART_val;
                    break;
                case "\rV_BAT:": _mcu_read.bat_op.Volt = UART_val;
                    break;
                case "\rI_STKA:": _mcu_read.ch_A1.Amp = UART_val;
                    break;
                case "\rI_STKB:": _mcu_read.ch_B1.Amp = UART_val;
                    break;
                default: break;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
            if ((string)e.UserState != "")
            { 
            string noCR = ((string)e.UserState).Remove(0, 1);
            
            dc.ConsoleInput = noCR;
            dc.RunCommand();
            }
          //  InputBlock.Focus();
          //  Scroller.ScrollToBottom();



            //if (_mcu_read.ENABLE_LOAD) 
            //{ 
            //    LOAD_ENABLE_pictureBox.Source = (ImageSource)Resources["M_100_Green"]; 
            //}
            //else 
            //{ 
            //    LOAD_ENABLE_pictureBox.Source = (ImageSource)Resources["M_100_Red"]; 
            //}
    }

        private void RED_LED_EN_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(GPIO.write(GPIO.Set, GPIO.RED_LED));
        }
        private static void write_Log_Header()
        {
            //Add new headder to file
            // log to file

            try
            {
                
               
               
               
                
            
                ReadWriteCSV.CsvRow row = new ReadWriteCSV.CsvRow();
                //row.Add(DateTime.Now.ToString());
                //CSV_W.WriteRow(row);

                //row.Clear();
                //ADCs
                row.Add(String.Format("Date Time"));
                row.Add(String.Format("V_LOAD"));
                row.Add(String.Format("I_Load"));
                row.Add(String.Format("V_STKA"));
                row.Add(String.Format("I_STKA"));
                row.Add(String.Format("V_STKB"));
                row.Add(String.Format("I_STKB"));
                row.Add(String.Format("V_BAT"));
                row.Add(String.Format("I_BAT"));
                row.Add(String.Format("EXT_5V"));
                row.Add(String.Format("TempA"));
                row.Add(String.Format("TempB"));




                //Digitals
                row.Add(String.Format("DCDC_ENABLE"));
                row.Add(String.Format("GREEN_LED"));
                row.Add(String.Format("RED_LED"));
                row.Add(String.Format("HYDRATE"));
                //row.Add(String.Format("V_BAT_MEAS_EN"));

                row.Add(String.Format("EN_LOAD"));
                row.Add(String.Format("ENABLE_STKA"));
                row.Add(String.Format("ENABLE_STKB"));
                row.Add(String.Format("ENABLE_BAT"));
                // row.Add(String.Format("NCHRG_ENABLE"));
                // row.Add(String.Format("CARTRIDGE_PWR"));
                // row.Add(String.Format("DIVIDER_MODE_SEL"));

                ////I2C
                //row.Add(String.Format("Register01"));
                //row.Add(String.Format("Register02"));
                //row.Add(String.Format("Register03"));
                //row.Add(String.Format("Register04"));
                //row.Add(String.Format("Register05"));
                //row.Add(String.Format("Register06"));
                //row.Add(String.Format("Register07"));

                ////I2C decode
                ////#1
                //row.Add(String.Format("BAT Fault"));
                //row.Add(String.Format("BAT STAT"));
                ////#2
                //row.Add(String.Format("BAT Iin_limit"));
                //row.Add(String.Format("BAT EN_STAT"));
                //row.Add(String.Format("BAT EN_TERM"));
                //row.Add(String.Format("BAT nCE"));
                ////#3
                ////row.Add(String.Format("BAT VbatReg"));
                ////row.Add(String.Format("BAT USB_DET"));
                ////#4
                //row.Add(String.Format("BAT I_chg_str"));
                ////#5
                //row.Add(String.Format("BAT LOOP_STATUS"));
                //row.Add(String.Format("BAT LOW_CHG"));
                //row.Add(String.Format("BAT DPDM_EN"));
                //row.Add(String.Format("BAT CE_STATUS"));
                //row.Add(String.Format("BAT VinDPM"));
                ////#6
                //row.Add(String.Format("BAT 2xTMR_EN"));
                //row.Add(String.Format("BAT TMR"));
                //row.Add(String.Format("BAT SYSOFF"));
                //row.Add(String.Format("BAT TS_STAT"));
                ////#7

            
                if (Properties.Settings.Default.append)
                {
                    ReadWriteCSV.CsvFileWriterAppend CSV_W_appened = new ReadWriteCSV.CsvFileWriterAppend(Properties.Settings.Default.logFile);
                    CSV_W_appened.WriteRow(row);
                    CSV_W_appened.Close();
                }
                else
                {
                    ReadWriteCSV.CsvFileWriter CSV_W_new = new ReadWriteCSV.CsvFileWriter(@Properties.Settings.Default.logFile);
                    CSV_W_new.WriteRow(row);
                    CSV_W_new.Close();
                }

                
                

                
            }
            catch
            {
                MessageBox.Show("log file error");
            }
        }

        private void auto_read_button_Click(object sender, RoutedEventArgs e)
        {
        //    string butString = (string)auto_read_button.Content;

        //    if (butString == "Auto Read Start")
        //    {
        //        auto_read_button.Content = "Auto Read Stop";
        //        auto_read_button.Background = System.Windows.Media.Brushes.Red;
        //        timer1.Interval = timer1.Interval = new TimeSpan(0,0,0,0,Properties.Settings.Default.loggingTime_ms);

        //        newHeaderToLogFile();

        //        timer1.Start();

        //    }
        //    else
        //    {
        //        auto_read_button.Content = "Auto Read Start";
        //        timer1.Stop();
        //        auto_read_button.Background = System.Windows.Media.Brushes.Gray;
        //    }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void comboBox1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (string s in SerialPort.GetPortNames())
                comboBox1.Items.Add(s);
        
        }

        private void read_gpio_button_Click(object sender, RoutedEventArgs e)
        {
            GPIO_READ_ALL();
        }

        private void Read_ADC_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData("adc_r *");
        }

 

        private void ENABLE_A_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(GPIO.write(GPIO.Set, GPIO.ENABLE_STKA));
            GPIO_READ_ALL();
        }

        private void DISABLE_A_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(GPIO.write(GPIO.Clear, GPIO.ENABLE_STKA));
            GPIO_READ_ALL();
        }

        private void ENABLE_B_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(GPIO.write(GPIO.Set, GPIO.ENABLE_STKB));
            GPIO_READ_ALL();
        }

        private void DISABLE_B_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(GPIO.write(GPIO.Clear, GPIO.ENABLE_STKB));
            GPIO_READ_ALL();
        }

        private void ENABLE_BAT_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(GPIO.write(GPIO.Set, GPIO.ENABLE_BAT));
            GPIO_READ_ALL();
        }

        private void DISABLE_BAT_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(GPIO.write(GPIO.Clear, GPIO.ENABLE_BAT));
            GPIO_READ_ALL();
        }

        private void RED_LED_DIS_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(GPIO.write(GPIO.Clear, GPIO.RED_LED));
        }

        private void GREEN_LED_EN_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(GPIO.write(GPIO.Set, GPIO.GREEN_LED));
        }

        private void GREEN_LED_DIS_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(GPIO.write(GPIO.Clear, GPIO.GREEN_LED));
        }

        private void DCDC_ENABLE_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(GPIO.write(GPIO.Clear, GPIO.DCDC_ENABLE));
            GPIO_READ_ALL();
        }

        private void DCDC_DISABLE_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(GPIO.write(GPIO.Set, GPIO.DCDC_ENABLE));
            GPIO_READ_ALL();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Logging logform = new Logging();
            logform.Show();
        }

        private void help_button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData("help");
        }

        private void test_button_Click(object sender, RoutedEventArgs e)
        {


            _mcu_read.Load.Volt = "10000";
            _mcu_read.Load.Amp = "1000";
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData("dac1 3000");
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            _mcu_read.SendData(txtSendData.Text);
        }

        private void txtSendData_PreviewKeyDown(object sender, KeyEventArgs e)
        {
             if (e.Key == Key.Enter)
             {
                 _mcu_read.SendData(txtSendData.Text);
             }
        }

        private void enableLoggingToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (enableLoggingToolStripMenuItem.IsChecked)
            {
                enableLoggingToolStripMenuItem.IsChecked = false;
                Properties.Settings.Default.enableLogging = false;
                enableLoggingToolStripMenuItem.IsChecked = Properties.Settings.Default.enableLogging;
                Properties.Settings.Default.Save();
            }
            else
            {

                enableLoggingToolStripMenuItem.IsChecked = true;
                Properties.Settings.Default.enableLogging = true;
                enableLoggingToolStripMenuItem.IsChecked = Properties.Settings.Default.enableLogging;
                Properties.Settings.Default.Save();
            }
        }



        private void MenuItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            enableLoggingToolStripMenuItem.IsChecked = Properties.Settings.Default.enableLogging;
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            ComPort_Disconnect();
        }








    }




}

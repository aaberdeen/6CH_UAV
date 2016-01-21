using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfAC10_WPF
{
    public class GPIO
    {

            public const bool Set = true;
            public const bool Clear = false;
            public const string ALL = "*";
            public const string NCHRG_ENABLE = "NCHRG_ENABLE";
            public const string DCDC_ENABLE = "DCDC_ENABLE";
            public const string ENABLE_BAT = "ENABLE_BAT";
            public const string ENABLE_STKB = "ENABLE_STKB";
            public const string ENABLE_STKA = "ENABLE_STKA";
            public const string ENABLE_LOAD = "ENABLE_LOAD";
            public const string RED_LED = "RED_LED";
            public const string GREEN_LED = "GREEN_LED";
            public const string V_BAT_MEAS_EN = "V_BAT_MEAS_EN";
            public const string DIVIDER_MODE_SEL = "DIVIDER_MODE_SEL";
            public const string CARTRIDGE_PWR = "CARTRIDGE_PWR";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"> true = set</param>
        /// <param name="cmd">GPIO</param>
        /// <returns></returns>
        public static string write(bool set, string cmd)
        {
           
            string setStr = "";

                if(set)
                {
                    setStr = "gpio_s ";
                }
                else
                {
                    setStr = "gpio_c ";
                }

                setStr = setStr + cmd;
                return setStr;
            }

        public static string read(string cmd)
        {

            string setStr = "gpio_r ";

            setStr = setStr + cmd;  
            return setStr;
        }


    }
}

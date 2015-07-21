using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NailShop.Utils
{
    public static class Utils
    {
        public static string FormatDateTime(DateTime pDate)
        {
            string lang = "vi-VN", mDate = "";
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[Constant.LANGUAGE] != null)
                lang = HttpContext.Current.Session[Constant.LANGUAGE].ToString();
            if (lang == "vi-VN")
                mDate = String.Format("{0:dd/MM/yyyy hh:mm tt}", pDate);
            else
                mDate = String.Format("{0:MM/dd/yyyy hh:mm tt}", pDate);

            return mDate;
        }

        public static string FormatDate(DateTime pDate)
        {
            string lang = "vi-VN", mDate = "";
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[Constant.LANGUAGE] != null)
                lang = HttpContext.Current.Session[Constant.LANGUAGE].ToString();
            //if (lang == "vi-VN")
            //    mDate = String.Format("{0:dd/MM/yyyy}", pDate);
            //else
            mDate = String.Format("{0:MM/dd/yyyy}", pDate);
            return mDate;
        }

        public static string FormatNumber(decimal amount)
        {
            string lang = "vi-VN", returnVal = "0";
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[Constant.LANGUAGE] != null)
                lang = HttpContext.Current.Session[Constant.LANGUAGE].ToString();

            returnVal = String.Format(System.Globalization.CultureInfo.GetCultureInfo(lang), "{0:N}", Math.Round(amount, 2));
            returnVal = (lang == "vi-VN") ? returnVal.Substring(0, returnVal.Length - 3) : returnVal;
            return returnVal;
        }

        public static string FormatNumberCurrency(decimal amount)
        {
            string lang = "vi-VN", returnVal = "0";
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[Constant.LANGUAGE] != null)
                lang = HttpContext.Current.Session[Constant.LANGUAGE].ToString();

            returnVal = String.Format(System.Globalization.CultureInfo.GetCultureInfo(lang), "{0:N}", Math.Round(amount, 2));
            returnVal = (lang == "vi-VN") ? returnVal.Substring(0, returnVal.Length - 3) : returnVal;
            return "$"+ returnVal;
        }

        public static string SplitWords(string strInput, int NumberCharacter)
        {
            string temp = "", result = "";
            string[] arrWord = strInput.Split(' ');

            if (strInput == null || strInput.Length <= NumberCharacter)
                return strInput;
            for (var i = 0; i < arrWord.Length - 1; i++)
            {
                temp += arrWord[i].Trim() + " ";
                if (temp.Length < NumberCharacter)
                    result = temp;
                else
                    break;
            }
            return result.Trim() + "...";
        }

    }
}
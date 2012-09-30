using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Flexible_computing
{
    class Data
    {

        String leftData;
        String rightData;
        public void modifycator(int inAccuracy, String inString, int inputStringFormat)
        {   
                if (inString.IndexOf('/')!=-1){
                    leftData = modifycatorString (inAccuracy, inString.Substring(0, inString.IndexOf('/')), inputStringFormat);
                    rightData = modifycatorString (inAccuracy, inString.Substring(inString.IndexOf('/') + 1), inputStringFormat);
                }
                else
                    if (inString.IndexOf(';')!=-1){
                        leftData = modifycatorString (inAccuracy, inString.Substring(0, inString.IndexOf(';')), inputStringFormat);
                        rightData = modifycatorString (inAccuracy, inString.Substring(inString.IndexOf(';') + 1), inputStringFormat);
                    }
                    else
                        leftData = modifycatorString (inAccuracy, inString, inputStringFormat);
        }
        private String modifycatorString (int inAccuracy, String dataString, int inputStringFormat)
        {
                if (dataString.Length > inAccuracy)
                    dataString = dataString.Substring(0, inAccuracy);
    
            dataString = dataString.Replace('E', 'e');
            dataString = dataString.Replace('.', ',');
    
            if (dataString.IndexOf(',') == 0)
	        dataString = "0" + dataString;

            if ((dataString[0] != '-') && (dataString[0] != '+'))
                dataString = "+" + dataString;   
    
            if (dataString.IndexOf(',') == -1)
	        if (dataString.IndexOf('e') != -1)
                    dataString = dataString.Substring(0, dataString.IndexOf('e')) + ",0" + dataString.Substring(dataString.IndexOf('e'));
	        else
                    dataString = dataString + ",0";
	  
            if ((dataString[dataString.IndexOf('e') + 1] != '+')&&
    	        (dataString[dataString.IndexOf('e') + 1] != '-'))
    	        dataString = dataString.Replace("e", "e+");
	    
            if (dataString.IndexOf('e') == -1)
    	        dataString = dataString + "e+0";

    
            return dataString;
        }
        private bool testInput(String inStr, int inputStringFormat)
        {
            
            //Regex myPattern1 = ;
            Match match;
            switch(inputStringFormat){
                default:
                case 0:
                case 3:{
                    match = Regex.Match(inStr,@"[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+$");
                    break;
                }
                case 1:
                case 4:{
                    match = Regex.Match(inStr,@"[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+/[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+$");
                    break;
                }
                case 2:
                case 5:{
                    match = Regex.Match(inStr, @"[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+;[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+$");
                    break;
                }    
            }
            //matcher = myPattern.matcher(inStr);
            if (match.Success)
               return true;
            else
               return false;
            }
    


    }
}

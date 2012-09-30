using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flexible_computing
{
    class StringUtil : IStringUtil
    {

    public StringUtil(){}
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputStr">Входная строка для обработки</param>
    /// <returns>Возвращает строку без избыточных нулей вначале и конце числа</returns>
	public String deleteZeroFromNumber(String inputStr)
    {
      // удаление 0 в начале числа
        String outStr = "";
        char [] trimparams = {'0'};
        int i=0,z =0,k=0;;
        //inputStr.TrimStart('0');
        //inputStr.TrimEnd('0');
        if (inputStr.Length >= 3)
        {
            if ((inputStr[0] == '-') || (inputStr[0] == '+'))
                z++;
            for (i = z; i < inputStr.IndexOf(',') - 1; i++)
            {
                if (inputStr[i] == '0')
                    continue;
                else
                    break;
            }

            if (z == 1)
                outStr = inputStr[0].ToString();



            if (inputStr[inputStr.Length - 1] == '0')
            {
                for (k = inputStr.Length - 1; k > inputStr.IndexOf(',') + 1; k--)
                    if (inputStr[k] != '0')
                        break;
            }
            else
                k = inputStr.Length - 1;
                outStr += inputStr.Substring(i, k + 1 - i);
                return outStr;
        }else
               // if (inputStr.Length > 0)
               // {
                //    if (outStr[0] != ',')
                 //       return outStr;
                  //  else
                  //      return outStr = '0' + outStr;
                //}
        return "0,0";

	}


	public String addDot(String inputStr)
    {
		// определение смещения запятой и ее добавление в случае отсутствия
		if (inputStr.IndexOf(',') == -1)
			inputStr += ",0";
		return inputStr;
	}

    public byte[] ToByteArray(String src)
    {
        byte[] dst = new byte[src.Length + 2];
        int i;
        for (i = 0; i < src.Length; i++)
        {
            dst[i] = (byte)src[i];
        }
        dst[src.Length] = (byte)'\r';
        dst[src.Length + 1] = (byte)'\n';

        return dst;
    }
    
    /// <summary>
    /// Copies amount of strings from String Array Src to String Array Dst
    /// </summary>
    /// <param name="Src">String Array Source</param>
    /// <param name="Dst">String Array Destination</param>
    /// <param name="count">Amount of string to copied</param>
    public void strstr(String[] Src, String[] Dst, int count)
    {
        int i;
        for (i = 0; i < count; i++)
        {
            Dst[i] = Src[i];
        }
    }

    }// END StringUtil Class

    public interface IStringUtil
    {
        byte[] ToByteArray(String src);
    }


    public class ExceptionUtil
    {
        String[] MessageStack;
        int Count;
        int Max;
        public bool isExceptionRised = false;
        public ExceptionUtil()
        {
            this.Count = 0;
            this.MessageStack = new String[10];
            this.Max = 2;
        }

        public bool AddException(String Mess)
        {

            if (Max > Count)
            {
                MessageStack[Count] = Mess;
                Count++;
                return true;
            }
            else
            {
                long tempL = Max * Max;
                if (tempL < 900000)
                {
                    int i;
                    String[] tempArr = new String[Max * Max];

                    for (i = 0; i < Max; i++)
                        tempArr[i] = MessageStack[i];

                    tempArr[Max] = Mess;
                    MessageStack = tempArr;
                    Max *= Max;
                    Count++;
                    return true;
                }
                else
                    return false;
            }

        }
        public String[] Messages()
        {
            if (Count > 0)
            {
                String[] returnMess = new String[Count];
                (new StringUtil()).strstr(MessageStack, returnMess, Count);
                return returnMess;
            }
            else
                return null;
        }
        public String LastMesssage()
        { return MessageStack[MessageStack.Length]; }
        
        
    }
    

}

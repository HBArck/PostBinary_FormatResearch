using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.Deployment.Application.Manifest;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;

namespace Flexible_computing
{
    public delegate void progIncCallback();
    public delegate void progIncThreadHeandler();
    public partial class Form1 : Form
    {
        bool TetraCheck = false;
        byte errorCounter;

        //char Flags BEGIN
        bool isMinusWritten = false;
        bool isCommaWritten = false;
        bool isSlashWritten = false;
        bool isFloatSelected = false;
        bool isIntervalSelected = false;
        bool[] currentType = new bool[4];// = { false,false,false};
        bool isTabIndexChanged = false;
        bool isCalcsReset = false;
        bool isFirstTime = true;
        bool currentCCOnTabs; // false - 2cc, true 16cc
        bool recalcClicked = false;// Did "Recalc" button was last clicked
        bool calcClicked = false;// Did "Start" button was last clicked
        Stopwatch timeCounter = new Stopwatch();
        bool isNum32Refreshed = true;
        bool isNum64Refreshed = true;
        bool isNum128Refreshed = true;
        bool isNum256Refreshed = true;
        bool calcFinished = false;
        bool isFormClosing = false;
        Thread threadCalc;
        //char Flags END

       
        //StringMath stringMath = new StringMath();
        StringUtil stringUtil = new StringUtil();
        static ExceptionUtil exceptionUtil = new ExceptionUtil();
        //String tempInput, tempOutput;

        FCCore Core ;
        ImageList il = new ImageList();
        TreeNode mainNode;
        bool isFormatInfoFullView = false;       

        //String[] textRes; // tbRes
        String[] textCalcError;   // tbCalcError
        // Left & Right part of Float & Interval
        String LeftPart;
        String RightPart;
        /* Temporary Var's
        public String out32Dec;
        public String out32Bin;
        public String out32DecE;
        public String out32BinE;

        public String out64Dec;
        public String out64Bin;
        public String out64DecE;
        public String out64BinE;
        public String out64Dec_2;
        public String out64Bin_2;
        public String out64DecE_2;
        public String out64BinE_2;

        public String out128Dec;
        public String out128Bin;
        public String out128DecE;
        public String out128BinE;
        public String out128Dec_2;
        public String out128Bin_2;
        public String out128DecE_2;
        public String out128BinE_2;

        public String out256Dec;
        public String out256Bin;
        public String out256DecE;
        public String out256BinE;
        public String out256Dec_2;
        public String out256Bin_2;
        public String out256DecE_2;
        public String out256BinE_2;

        */
   
        int roundType = 0;
        int inputStringFormat = 0;
        int[] version;
        int lang=1;

        public enum LockCommands { Start = 1, Recalc = 2 };
        public enum excps  
        {
            FCCoreGeneralException, FCCoreFunctionException, Exception, FCCoreArithmeticException
        };

        public Form1()
        {
            InitializeComponent();  
        }

        private void Form1_Enter(object sender, EventArgs e)
        {
            this.SetTopLevel(true);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            MinimumSize = new Size(870,MinimumSize.Height);
            Width = 870;
            dataGridView1.Rows.Add(3);
            dataGridView2.Rows.Add(12);
            //textCalcError= new String[4];
            //var  temp = Core.Num32.GetType().GetProperty("Mantisa");
            //temp.SetValue(Core.Num32, "new mantissa",null);
            //tbMantisa32.Text = Core.Num32.Mantisa;
            tTime.Start();
            tabControl_Format.SelectedIndex = 0;

            radioFloat.Enabled = false;
            radioInterval.Enabled = false;
            roundType = 1;
            label18.Text = miToInt.Text;

            this.MaximumSize = new Size(this.Size.Width - 100, this.Size.Height);
            this.MinimumSize = new Size(this.Size.Width - 100, this.Size.Height);
            currentType[0] = true;
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            //Version vv = Assembly.GetAssembly().GetName().Version;
            version = new int[] {v.Major, v.Minor, v.Build };
            tbMenuVer.Text = "Версия [" + v.Major + "." + v.Minor + "." + v.Build + "]";
            // clearVars();
            bClear_Click(sender, e);
            stlStatus.Text = "";
            Core = new FCCore(1, exceptionUtil, this.progressBar1, progInc);
            // -0,3e-1 - Exception
            // Load XML INIT PARAMS HERE
            //

            if (checkKnownProblems())
            {
               // MessageBox.Show("No konown problems are detected.");
                problemStatus.Visible = true;
            }
            else
            {
                problemStatus.Visible = false;
            }
            
            treeView1.ImageList = il;
            il.Images.Add(Properties.Resources.demon_blood);
            il.Images.Add(Properties.Resources.arithmetic_exception);
            il.Images.Add(Properties.Resources.function_exception);
            il.Images.Add(Properties.Resources.general_exception);
            il.Images.Add(Properties.Resources.arrow_right);
            il.Images.Add(Properties.Resources.warning);
            mainNode = treeView1.Nodes.Add("Core","Core",0); 
            treeView1.SelectedImageIndex = 4;         
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(inStr[lang][8, 0], inStr[lang][2, 4] + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                isFormClosing = true;
                tTime.Stop();
                e.Cancel = true;
                if (Core.thread32 != null)
                {

                    if ((Core.thread32.ThreadState & (System.Threading.ThreadState.Running | System.Threading.ThreadState.Background)) != 0)
                        bStop_Thread32_Click(sender, e);
                    //Core.thread32.Abort();

                }
                if (Core.thread64 != null)
                {
                    if ((Core.thread64.ThreadState & (System.Threading.ThreadState.Running | System.Threading.ThreadState.Background)) != 0)
                        bStop_Thread64_Click(sender, e);
                    //Core.thread64.Suspend();
                }
                if (Core.thread128 != null)
                {
                    if ((Core.thread128.ThreadState & (System.Threading.ThreadState.Running | System.Threading.ThreadState.Background)) != 0)
                        bStop_Thread128_Click(sender, e);
                    //Core.thread128.Suspend();
                }
                if (Core.thread256 != null)
                {
                    if ((Core.thread256.ThreadState & (System.Threading.ThreadState.Running | System.Threading.ThreadState.Background)) != 0)
                        bStop_Thread256_Click(sender, e);
                    //Core.thread256.Suspend();
                }
                //if (Core.thread64_right != null)
                //{
                //    if (Core.thread64_right.IsAlive)
                //        Core.thread64_right.Suspend();
                //}
                //if (Core.thread128_right != null)
                //{
                //    if (Core.thread128_right.IsAlive)
                //        Core.thread128_right.Suspend();
                //}
                //if (Core.thread256_right != null)
                //{
                //    if (Core.thread256_right.IsAlive)
                //        Core.thread256_right.Suspend();
                //}
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //FillForm();
            // calcResultsAndErrors(this, null);
            refreshNumberStatus();
            if (isFirstTime)
            {
                tbInput.Focus();
                isFirstTime = false;
            }
            lExp32Len.Text = "( " + tbExp32.TextLength + " )";
            lExp64Len.Text = "( " + tbExp64.TextLength + " )";
            lExp128Len.Text = "( " + tbExp128.TextLength + " )";
            lExp256Len.Text = "( " + tbExp256.TextLength + " )";

            lMan32Len.Text = "( " + tbMantisa32.TextLength + " )";
            lMan64Len.Text = "( " + tbMantisa64.TextLength + " )";
            lMan128Len.Text = "( " + tbMantisa128.TextLength + " )";
            lMan256Len.Text = "( " + tbMantisa256.TextLength + " )";

            // Debugging Thread States
            lTh1.Text = Core.thread32.ThreadState.ToString();
            lTh2.Text = Core.thread64.ThreadState.ToString();
            lTh3.Text = Core.thread128.ThreadState.ToString();
            lTh4.Text = Core.thread256.ThreadState.ToString();

            lTh2R.Text = Core.thread64_right.ThreadState.ToString();
            lTh3R.Text = Core.thread128_right.ThreadState.ToString();
            lTh4R.Text = Core.thread256_right.ThreadState.ToString();

            if (!isNum32Refreshed)
            {
                if (isThreadsRunning(0, false) == 0)
                {
                    //bStop_Thread32.Enabled = false;
                    progInc();
                    isNum32Refreshed = true;
                }
            }

            if (!isNum64Refreshed)
            {
                if (inputStringFormat == 0)
                {
                    if (isThreadsRunning(1, false) == 0)
                    {
                        //bStop_Thread64.Enabled = false;
                        progInc();
                        isNum64Refreshed = true;
                    }
                }
                else
                {
                    if (isThreadsRunning(1, true) == 0)
                    {
                        //bStop_Thread64.Enabled = false;
                        progInc();
                        isNum64Refreshed = true;
                    }
                }
            }

            if (!isNum128Refreshed)
            {
                if (inputStringFormat == 0)
                {
                    if (isThreadsRunning(2, false) == 0)
                    {
                        //bStop_Thread128.Enabled = false;
                        progInc();
                        isNum128Refreshed = true;
                    }
                }
                else
                {
                    if (isThreadsRunning(2, true) == 0)
                    {
                        //bStop_Thread128.Enabled = false;
                        progInc();
                        isNum128Refreshed = true;
                    }
                }
            }

            if (!isNum256Refreshed)
            {
                if (inputStringFormat == 0)
                {
                    if (isThreadsRunning(3, false) == 0)
                    {
                        progInc();
                        isNum256Refreshed = true;
                        calcFinished = true;
                       // bStop_Thread256.Enabled = false;
                        timeOnForm("Paint 256 Finished");
                    }
                }
                else
                {
                    if (isThreadsRunning(3, true) == 0)
                    {
                        progInc();
                        isNum256Refreshed = true;
                        calcFinished = true;
                       // bStop_Thread256.Enabled = false;
                        timeOnForm("Paint 256 Finished + Right");
                    }
                }
            }

            if ((isNum256Refreshed) && (calcFinished))
            {
                if (isThreadsRunning(-1, inputStringFormat == 0 ? false : true) == 0)
                {
                    progressBar1.Value = 0;
                    progressBar1.UseWaitCursor = false;
                    progressBar1.Refresh();
                    bStart.Enabled = true;
                    //ThreadPool.QueueUserWorkItem(calcResultsAndErrors);
                    calcResultsAndErrors(null);
                    bCancelStart.Visible = false;
                    calcFinished = false;
                    UnLockComponents();
                    timeOnForm("Paint CalcFinished");
                }
            }
            else
            {
                if (isThreadsRunning(-1, inputStringFormat == 0 ? false : true) == 0)
                {
                    progressBar1.Value = 0;
                    progressBar1.Refresh();
                }
            }
        }

      
        public bool checkKnownProblems()
        {
            int i,j,r,exFound;
            String currNum="";
            String vers="";
            String [] tempV;
            String[] newException;
            try
            {
                FileStream stream = System.IO.File.Open("log.dat", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                if (stream != null)
                {
                    byte[] inpptString= new byte[stream.Length + 1];
                    int bytesReaded = stream.Read(inpptString, 0, (int)(stream.Length));
                    
                    //tabControl1.TabPages.Add("Known Error");
                    //RichTextBox rbErrors = new RichTextBox();
                    //  tabControl1.TabPages["Known Errors"].Controls.Add(rbErrors);
                    //rbErrors.Text = "Error";
                    r = i = 0;
                    while (inpptString.Length > i)
                    {
                        j = i;
                        currNum = "";
                        while (inpptString[j] != '\r')
                        {
                            currNum += (char)inpptString[j];
                            j++;
                            if (j == inpptString.Length-1)
                                break;
                        }
                        r++;// rows found
                        i = j+1;
                        // First row is version
                        if (r == 1)
                        {
                            vers = currNum;
                        }
                        else
                        lbKnownErrors.Items.Add(currNum);
                        i++;
                    }

                    newException =  new String[r-1];
                    exFound = 0;

                    //if (vers!="")
                    //{
                     //   tempV = vers.Split('.');
                        if (lbKnownErrors.Items.Count > 0)
                        for  (i = 0 ; i < lbKnownErrors.Items.Count ; i++)
                        {
                            try
                            { // check if error is still of current interest(актуальна)
                                throw new FCCoreGeneralException("My Exception");
                            }
                            catch (FCCoreGeneralException ex)
                            {
                                newException[i] = (lbKnownErrors.Items[i]).ToString();
                                exFound++;
                            }
                        }
                    //}

                    stream.Close();
                    lbKnownErrors.Items.Clear();

                    //Writing new version in log.dat in 1 row  position
                    String outt = version[0].ToString()+"."+version[1].ToString()+"."+version[2].ToString();
                    byte [] outputStr = new byte[outt.Length+2];


                    FileStream streamwr = File.Create("log.dat");
                    //streamwr.Seek(0, SeekOrigin.Begin);
                    streamwr.Write(  stringUtil.ToByteArray(outt),0,outt.Length+2);
                       
                    //Adding new exceptions to file and listbox for Errors
                    for (i = 0; i < newException.Length; i++)
                    {
                        lbKnownErrors.Items.Add(newException[i]);
                        streamwr.Write(stringUtil.ToByteArray( newException[i] ), 0, newException[i].Length + 2);
                    }
                    streamwr.Close();
                    if (lbKnownErrors.Items.Count > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
             
            }
            catch(FileNotFoundException fex)
            {
                //MessageBox.Show("Log File doesn't exist");
                return false;
            }
            catch (FCCoreGeneralException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public String testExponentNumber(String inputStr, int Limit)
        {
            // 1,123e-123 // 1,123e-123
            // 1123e-123  // 1323e+123
            Match matcher;
            String part1, part2, part3, sign;
            int iDot, iExp, res, expNum, delta, subres;
            int p1, p2;
            String resStr;
            //matcher = Regex.Match(inputStr, "[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+$");
            //if (matcher.Success)
            //   return matcher.Value;
            part1 = "";
            part2 = "";
            part3 = "";
            sign = "";
            iDot = inputStr.IndexOf(",");
            iExp = inputStr.IndexOf("e");
            if (iDot != -1)
            {// 1,
                part1 = inputStr.Substring(0, iDot);
            }
            else
            {
                p1 = part1.Length;
                if (p1 <= Limit)
                    return inputStr;
                else
                    return part1.Substring(0, p1 - Limit);
            }

            if (iExp != -1)
            {// ,123e
                if ((inputStr[iExp + 1] == '-') || (inputStr[iExp + 1] == '+'))
                {
                    part2 = inputStr.Substring(iDot + 1, iExp - iDot - 1);
                    part3 = inputStr.Substring(iExp + 2, inputStr.Length - iExp - 2);
                    sign = inputStr.Substring(iExp + 1, 1);

                    p1 = part1.Length;
                    p2 = part2.Length;
                    expNum = int.Parse(part3);

                    res = p1 + p2 + expNum;
                    // 123,123e-123 num 3
                    delta = Limit - res;
                    if (delta < 0)
                    {

                        subres = res - Math.Abs(delta);
                        subres = Math.Abs(subres);
                        if (p1 <= Limit)
                        {
                            resStr = part1;
                            if (p2 <= Limit - p1)
                            {
                                resStr += "," + part2;
                                if (expNum < Limit - (p1 + p2))
                                {
                                    resStr += "e" + sign + part3;
                                    return resStr;
                                }
                                else
                                {
                                    subres = (Limit - (p1 + p2));
                                    if (subres > 0)
                                    {
                                        resStr += "e" + sign + subres.ToString();
                                        return resStr;
                                    }
                                    else
                                        return resStr;
                                }
                            }
                            else
                            {
                                if (Limit - p1 != 0)
                                    resStr += "," + part2.Substring(0, Limit - p1);
                                else
                                    resStr += ",0";
                                return resStr;
                            }
                        }
                        else
                            resStr = part1.Substring(0, Limit) + ",0";
                    }
                    else return inputStr;
                }
                else
                    return "";
            }
            else
            {
                p1 = part1.Length;
                part2 = inputStr.Substring(iDot + 1, inputStr.Length - 1 - p1);
                p2 = part2.Length;
                if (p2 + p1 <= Limit)
                {
                    return inputStr;
                }
                else
                {
                    if (p1 <= Limit)
                    {
                        if (p2 == Limit - p1)
                            return part1;
                        else
                            if (Limit - p1 == 0)
                                return part1 + ",0";
                            else
                                return part1 + "," + part2.Substring(0, Limit - p1);
                    }
                    else
                        return part1.Substring(0, Limit);
                }
            }


            return "";
        }
        public String modifyMe(int inAccuracy, String inpStr)
        {
            String dataString = inpStr;
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

            if ((dataString[dataString.IndexOf('e') + 1] != '+') &&
                (dataString[dataString.IndexOf('e') + 1] != '-'))
                dataString = dataString.Replace("e", "e+");

            if (dataString.IndexOf('e') == -1)
                dataString = dataString + "e+0";


            return dataString;
        }
        public bool RegxTest()
        {
            /*
             * �������� ������� ������ �� ������������ �������:
             * +123,567e-89
             */
            String dataString = tbInput.Text;
            Match matcher;
            bool Part1Succ = false, Part2Succ = false;
            String part1, part2;
            switch (inputStringFormat)
            {
                default:
                case 0:
                    {
                        matcher = Regex.Match(dataString, "[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+$");
                        break;
                    }
                case 1:
                    {
                        //matcher = Regex.Match(dataString, "[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+/[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+$");
                        //matcher = Regex.Match(dataString, "[\\-\\+]\\d+[.\\d+]\\d+e[\\+\\-]\\d+/[\\-\\+]\\d+[.\\d+]\\d+e[\\+\\-]\\d+$");
                        if (dataString.Contains("/"))
                        {
                            int temp_index = dataString.IndexOf('/');
                            part1 = dataString.Substring(0, temp_index);
                            part2 = dataString.Substring(temp_index + 1, dataString.Length - (1 + temp_index));
                            matcher = Regex.Match(part1, "[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+$");
                            if (matcher.Success)
                                Part1Succ = true;
                            matcher = Regex.Match(part2, "[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+$");
                            if (matcher.Success)
                                Part2Succ = true;
                        }
                        else
                            matcher = null;
                        break;
                    }
                case 2:
                    {
                        matcher = Regex.Match(dataString, "[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+;[\\-\\+]\\d+[,\\d+]\\d+e[\\+\\-]\\d+$");
                        break;
                    }
            }
            //matcher = myPattern.matcher(dataString);
            if ((matcher.Success) || ((Part1Succ) && (Part2Succ)))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Проверяет заполнена ли строка для текущего формата правильно
        /// </summary>
        public void checkInputStringForCurrentFormat()
        {
            int currFormat = getIndexType();
            String inputString = "";//= tbInput.Text;
            int t1, t2, i;

            if (tbInput.Multiline == true)
            {
                for (i = 0; i < tbInput.Lines.Length; i++)
                    inputString += tbInput.Lines[i];

            }
            else
            {
                inputString = tbInput.Text;
            }
            t1 = inputString.IndexOf("/");
            t2 = inputString.IndexOf(";");

            switch (currFormat)
            {
                case 0:

                    if ((t1 != -1) || (t2 != -1)) // Если хоть один из символов есть, то
                    {
                        if (t1 != -1)
                            tbInput.Text = inputString.Substring(0, t1);
                        if (t2 != -1)
                            tbInput.Text = inputString.Substring(0, t2);
                    }// иначе строка нужного формата
                    break;
                case 1:
                    // Если символ отсут
                    if (t1 == -1) 
                    {
                        tbInput.Text = inputString + "/1,0";
                    }
                    else
                    {
                        if (t1 + 1 == inputString.Length)
                        {
                            tbInput.Text = inputString + "0,0";
                        }
                    }

                    if (t2 != -1)
                    {
                        tbInput.Text = inputString.Substring(0, t2);
                    }
                    break;
                case 2:
                    if (t1 != -1)
                    {
                        tbInput.Text = inputString.Substring(0, t1);
                    }
                    if (t2 == -1)
                    {
                        tbInput.Text = inputString + ";0,0";
                    }
                    else
                    {
                        if (t2 + 1 == inputString.Length)
                        {
                            tbInput.Text = inputString + "0,0";
                        }
                    }
                    break;
            }
        }
 
        public void refreshNumberStatus()
        {
            switch (tabControl_Format.SelectedIndex)
            {
                case 0:
                    switch (inputStringFormat)
                    {
                        case 0:lNormDenorm.Text = inStr[lang][9, (int)Core.Num32.NumberState];
                                lNormDenorm.ForeColor = ColorsForState[(int)Core.Num32.NumberState];
                                break;
                        case 1: lNormDenorm.Text = inStr[lang][9, (int)Core.Num32.NumberState] + " / " + inStr[lang][9, (int)Core.Num32.NumberStateRight];
                                lNormDenorm.ForeColor = ColorsForState[ColorsForState.Length-1];
                                break;
                        case 2: lNormDenorm.Text = inStr[lang][9, (int)Core.Num32.NumberState] + " ; " + inStr[lang][9, (int)Core.Num32.NumberStateRight];
                                lNormDenorm.ForeColor = ColorsForState[ColorsForState.Length-1];
                                break;
                    }
                    break;
                case 1:
                    switch (inputStringFormat)
                    {
                        case 0: lNormDenorm.Text = inStr[lang][9, (int)Core.Num64.NumberState];
                                lNormDenorm.ForeColor = ColorsForState[(int)Core.Num64.NumberState];
                                break;
                        case 1: lNormDenorm.Text = inStr[lang][9, (int)Core.Num64.NumberState] + " / " + inStr[lang][9, (int)Core.Num64.NumberStateRight];
                                lNormDenorm.ForeColor = ColorsForState[ColorsForState.Length-1];
                                break;
                        case 2: lNormDenorm.Text = inStr[lang][9, (int)Core.Num64.NumberState] + " ; " + inStr[lang][9, (int)Core.Num64.NumberStateRight];
                                lNormDenorm.ForeColor = ColorsForState[ColorsForState.Length-1];
                                break;
                    }   
                    break;
                case 2:
                     switch (inputStringFormat)
                    {
                        case 0: lNormDenorm.Text = inStr[lang][9, (int)Core.Num128.NumberState];
                                lNormDenorm.ForeColor = ColorsForState[(int)Core.Num128.NumberState];
                                break;
                        case 1: lNormDenorm.Text = inStr[lang][9, (int)Core.Num128.NumberState] + " / " + inStr[lang][9, (int)Core.Num128.NumberStateRight];
                                lNormDenorm.ForeColor = ColorsForState[ColorsForState.Length-1];
                                break;
                        case 2: lNormDenorm.Text = inStr[lang][9, (int)Core.Num128.NumberState] + " ; " + inStr[lang][9, (int)Core.Num128.NumberStateRight];
                                lNormDenorm.ForeColor = ColorsForState[ColorsForState.Length-1];
                                break;
                    }   
                    break;
                case 3:
                    switch (inputStringFormat)
                    {
                        case 0: lNormDenorm.Text = inStr[lang][9, (int)Core.Num256.NumberState];
                                lNormDenorm.ForeColor = ColorsForState[(int)Core.Num128.NumberState];
                                break;
                        case 1: lNormDenorm.Text = inStr[lang][9, (int)Core.Num256.NumberState] + " / " + inStr[lang][9, (int)Core.Num256.NumberStateRight];
                                lNormDenorm.ForeColor = ColorsForState[ColorsForState.Length-1];
                                break;
                        case 2: lNormDenorm.Text = inStr[lang][9, (int)Core.Num256.NumberState] + " ; " + inStr[lang][9, (int)Core.Num256.NumberStateRight];
                                lNormDenorm.ForeColor = ColorsForState[ColorsForState.Length-1];
                                break;
                    }   
                    break;
            }
        }
        
        private void progInc()
        {
            if (progressBar1.InvokeRequired)
            {
                progIncCallback p = new progIncCallback(progInc);
                this.Invoke(p); //, new object[] { val }
            }
            else
            {
                if (progressBar1.Value < 100)
                {
                    if (progressBar1.Value + 5 <= progressBar1.Maximum)
                        progressBar1.Value += 5;
                    else
                        progressBar1.Value = progressBar1.Maximum;
                }
            }
        }
        public void proginc(int val)
        {
            if (progressBar1.Value + val < progressBar1.Maximum)
                progressBar1.Value += val;
            else
                progressBar1.Value = progressBar1.Maximum;
        }
        public delegate void setProgressDel(Object threadContext);
        public void setProgress(Object threadContext)
        {
            int val = (int)threadContext;
            if (progressBar1.InvokeRequired)
            {
                setProgressDel d = new setProgressDel(setProgress);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                if ((val <= 100) && (val>0))
                {
                    progressBar1.Value = val;
                }
            }
        }


        public delegate void logResultsDel(Object threadContext);
        public void logResults(Object threadContext)
        {
            Thread.CurrentThread.IsBackground = true;
            while (isThreadsRunning(3, inputStringFormat == 0 ? false : true) != 0)
            {
                if (isFormClosing)
                {
                    try
                    {
                        Thread.CurrentThread.Abort(); 
                    }
                    catch (Exception ex)
                    { return; }
                }
                Thread.Sleep(1000);
            }


            if (richTextBox1.InvokeRequired)
            {
                logResultsDel d = new logResultsDel(logResults);
                this.Invoke(d, new object[] { threadContext });
            }
            else
            {
                if (cbClearLog.Checked)
                    richTextBox1.Text = "";
                if (inputStringFormat == 0)
                {

                    richTextBox1.Text += "      Значения pb32       =[" + Core.Num32.CorrectResultExp + "  ]\r\n";
                    richTextBox1.Text += "      Погрешность pb32=[" + Core.Num32.Error + "  ]\r\n";

                    richTextBox1.Text += "      Значения pb64       =[" + Core.Num64.CorrectResultExp + "  ]\r\n";
                    richTextBox1.Text += "      Погрешность pb64=[" + Core.Num64.Error + "  ]\r\n";

                    richTextBox1.Text += "      Значения pb128       =[" + Core.Num128.CorrectResultExp + "  ]\r\n";
                    richTextBox1.Text += "      Погрешность pb128=[" + Core.Num128.Error + "  ]\r\n";

                    richTextBox1.Text += "      Значения pb256       =[" + Core.Num256.CorrectResultExp + "  ]\r\n";
                    richTextBox1.Text += "      Погрешность pb256=[" + Core.Num256.Error + "  ]\r\n\r\n";

                }
                else
                {
                    if (inputStringFormat == 1)
                    {
                        richTextBox1.Text += "      Значения pb64       =[" + Core.Num64.CorrectResultFractionExpL + "/" + Core.Num64.CorrectResultFractionExpR + "  ]\n";
                        richTextBox1.Text += "      Погрешность pb64=[  " + Core.Num64.ErrorFractionLeft + "/" + Core.Num64.ErrorFractionRight + "  ]\n";

                        richTextBox1.Text += "      Значения pb128       =[" + Core.Num128.CorrectResultFractionExpL + "/" + Core.Num128.CorrectResultFractionExpR + "  ]\r\n";
                        richTextBox1.Text += "      Погрешность pb128=[  " + Core.Num128.ErrorFractionLeft + "/" + Core.Num128.ErrorFractionRight + "  ]\r\n";

                        richTextBox1.Text += "      Значения pb256       =[" + Core.Num128.CorrectResultFractionExpL + "/" + Core.Num128.CorrectResultFractionExpR + "  ]\r\n";
                        richTextBox1.Text += "      Погрешность pb256=[  " + Core.Num256.ErrorFractionLeft + "/" + Core.Num256.ErrorFractionRight + "  ]\r\n\r\n";
                    }
                    else
                    {
                        richTextBox1.Text += "      Значения pb64       =[" + Core.Num64.CorrectResultIntervalExpL + "/" + Core.Num64.CorrectResultIntervalExpR + "  ]\n";
                        richTextBox1.Text += "      Погрешность pb64=[  " + Core.Num64.ErrorIntervalLeft + ";" + Core.Num64.ErrorFractionRight + "  ]\n";

                        richTextBox1.Text += "      Значения pb128       =[" + Core.Num128.CorrectResultIntervalExpL + "/" + Core.Num128.CorrectResultIntervalExpR + "  ]\r\n";
                        richTextBox1.Text += "      Погрешность pb128=[  " + Core.Num128.ErrorIntervalLeft + ";" + Core.Num128.ErrorFractionRight + "  ]\r\n";

                        richTextBox1.Text += "      Значения pb256       =[" + Core.Num128.CorrectResultIntervalExpL + "/" + Core.Num128.CorrectResultIntervalExpR + "  ]\r\n";
                        richTextBox1.Text += "      Погрешность pb256=[" + Core.Num256.ErrorIntervalLeft + ";" + Core.Num256.ErrorFractionRight + "  ]\r\n\r\n";

                    }
                }
            }
        }

        public delegate void FillFormDel(Object threadContext);
        /// <summary>
        /// Filling all fields on Form
        /// Uses : calcFinished, 
        /// </summary>
        public void FillForm(Object threadContext)
        {
            Thread.CurrentThread.IsBackground = true;
            while (isThreadsRunning(3, inputStringFormat == 0 ? false : true) != 0)
            {
                if (isFormClosing)
                {
                    try
                    { Thread.CurrentThread.Abort(); }
                    catch (Exception ex)
                    { return; }
                }
                Thread.Sleep(1000);
            }
          
                settbS32Text(Core.Num32.Sign == "-" ? "1" : "0");
                settbS64Text(Core.Num64.Sign == "-" ? "1" : "0");
                settbS128Text(Core.Num128.Sign == "-" ? "1" : "0");
                settbS32Text(Core.Num32.Sign == "-" ? "1" : "0");

                settbExp32Text(Core.Num32.Exponenta);
                settbExp64Text(Core.Num64.Exponenta);
                settbExp128Text(Core.Num128.Exponenta);
                settbExp256Text(Core.Num256.Exponenta);

                settbMan32Text(Core.Num32.Mantisa);
                settbMan64Text(Core.Num64.Mantisa);
                settbMan128Text(Core.Num128.Mantisa);
                settbMan256Text(Core.Num256.Mantisa);

                if (Core.NumberFormat != 0)
                {
                    settbS64_2Text(Core.Num64.SignRight == "-" ? "1" : "0");
                    settbS128_2Text(Core.Num128.SignRight == "-" ? "1" : "0");
                    settbS256_2Text(Core.Num32.SignRight == "-" ? "1" : "0");

                    settbExp64_2Text(Core.Num64.ExponentaRight);
                    settbExp128_2Text(Core.Num128.ExponentaRight);
                    settbExp256_2Text(Core.Num256.ExponentaRight);

                    settbMan64_2Text(Core.Num64.MantisaRight);
                    settbMan128_2Text(Core.Num128.MantisaRight);
                    settbMan256_2Text(Core.Num256.MantisaRight);
                }
                if (currentCCOnTabs == true)
                {
                    convert2to16();
                }
        }

        public delegate void calcResultsAndErrorsDel(Object threadContext);
        public void calcResultsAndErrors(Object threadContext)
        {
            Thread.CurrentThread.IsBackground = true;
            while (isThreadsRunning(3, inputStringFormat == 0 ? false : true) != 0)
            {
                if (isFormClosing)
                {
                    try
                    { Thread.CurrentThread.Abort(); }
                    catch (Exception ex)
                    { return; }
                }
                Thread.Sleep(1000);
            }
          
            //isTabIndexChanged = true;
            if (tabControl_Format.InvokeRequired)
            {
                calcResultsAndErrorsDel d = new calcResultsAndErrorsDel(calcResultsAndErrors);
                this.Invoke(d, new object[] { threadContext });
            }
            else
            {
                switch (tabControl_Format.SelectedIndex)
                {
                    case 0:
                        //tbCalcError.Text = Errors[0];
                        tbCalcError.Text = Core.Num32.Error;
                        if (rB10cc32.Checked)
                            if (!cBExp32.Checked)
                                //tbRes.Text = out32Dec;
                                tbRes.Text = Core.Num32.CorrectResult;
                            else
                                //tbRes.Text = out32DecE;
                                tbRes.Text = Core.Num32.CorrectResultExp;
                        else
                            if (rB2cc32.Checked)
                                if (!cBExp32.Checked)
                                    //tbRes.Text = out32Bin;
                                    tbRes.Text = Core.Num32.CorrectResult2cc;
                                else
                                    //tbRes.Text = out32BinE;
                                    tbRes.Text = Core.Num32.CorrectResult2ccExp;
                        break;

                    case 1:
                        switch (inputStringFormat)
                        {
                            //case 0: tbCalcError.Text = Errors[1]; break; 
                            case 0: tbCalcError.Text = Core.Num64.Error; break;
                            case 1: tbCalcError.Text = Core.Num64.ErrorFractionLeft + " / " + Core.Num64.ErrorFractionRight; break;
                            case 2: tbCalcError.Text = Core.Num64.ErrorIntervalLeft + " ; " + Core.Num64.ErrorFractionRight; break;
                        }
                        if (rB10cc32.Checked)
                            if (!cBExp32.Checked)
                                switch (inputStringFormat)
                                {
                                    //case 0: tbRes.Text = out64Dec; break;
                                    case 0: tbRes.Text = Core.Num64.CorrectResult; break;
                                    case 1: tbRes.Text = Core.Num64.CorrectResultFractionL + " / " + Core.Num64.CorrectResultFractionR; break;
                                    case 2: tbRes.Text = Core.Num64.CorrectResultIntervalL + " ; " + Core.Num64.CorrectResultIntervalR; break;
                                }
                            else
                                switch (inputStringFormat)
                                {
                                    //case 0: tbRes.Text = out64DecE; break;
                                    case 0: tbRes.Text = Core.Num64.CorrectResultExp; break;
                                    case 1: tbRes.Text = Core.Num64.CorrectResultFractionExpL + " / " + Core.Num64.CorrectResultFractionExpR; break;
                                    case 2: tbRes.Text = Core.Num64.CorrectResultIntervalExpL + " ; " + Core.Num64.CorrectResultIntervalExpR; break;
                                }
                        else
                            if (rB2cc32.Checked)
                                if (!cBExp32.Checked)
                                    switch (inputStringFormat)
                                    {
                                        //case 0: tbRes.Text = out64Bin; break;
                                        case 0: tbRes.Text = Core.Num64.CorrectResult2cc; break;
                                        case 1: tbRes.Text = Core.Num64.CorrectResultFraction2ccL + " / " + Core.Num64.CorrectResultFraction2ccR; break;
                                        case 2: tbRes.Text = Core.Num64.CorrectResultInterval2ccL + " ; " + Core.Num64.CorrectResultInterval2ccR; break;
                                    }
                                else
                                    switch (inputStringFormat)
                                    {
                                        //case 0: tbRes.Text = out64BinE; break;
                                        case 0: tbRes.Text = Core.Num64.CorrectResult2ccExp; break;
                                        case 1: tbRes.Text = Core.Num64.CorrectResultFraction2ccExpL + " / " + Core.Num64.CorrectResultFraction2ccExpR; break;
                                        case 2: tbRes.Text = Core.Num64.CorrectResultInterval2ccExpL + " ; " + Core.Num64.CorrectResultInterval2ccExpR; break;
                                    }
                        break;

                    case 2:
                        switch (inputStringFormat)
                        {
                            //case 0: tbCalcError.Text = Errors[2]; break;
                            case 0: tbCalcError.Text = Core.Num128.Error; break;
                            case 1: tbCalcError.Text = Core.Num128.ErrorFractionLeft + " / " + Core.Num128.ErrorFractionRight; break;
                            case 2: tbCalcError.Text = Core.Num128.ErrorIntervalLeft + " ; " + Core.Num128.ErrorFractionRight; break;
                        }
                        if (rB10cc32.Checked)
                            if (!cBExp32.Checked)
                                switch (inputStringFormat)
                                {
                                    case 0: tbRes.Text = Core.Num128.CorrectResult; break;
                                    case 1: tbRes.Text = Core.Num128.CorrectResultFractionL + " / " + Core.Num128.CorrectResultFractionR; break;
                                    case 2: tbRes.Text = Core.Num128.CorrectResultIntervalL + " ; " + Core.Num128.CorrectResultIntervalR; break;
                                }
                            else
                                switch (inputStringFormat)
                                {
                                    case 0: tbRes.Text = Core.Num128.CorrectResultExp; break;
                                    case 1: tbRes.Text = Core.Num128.CorrectResultFractionExpL + " / " + Core.Num128.CorrectResultFractionExpR; break;
                                    case 2: tbRes.Text = Core.Num128.CorrectResultIntervalExpL + " ; " + Core.Num128.CorrectResultIntervalExpR; break;
                                }
                        else
                            if (rB2cc32.Checked)
                                if (!cBExp32.Checked)
                                    switch (inputStringFormat)
                                    {
                                        case 0: tbRes.Text = Core.Num128.CorrectResult2cc; break;
                                        case 1: tbRes.Text = Core.Num128.CorrectResultFraction2ccL + " / " + Core.Num128.CorrectResultFraction2ccR; break;
                                        case 2: tbRes.Text = Core.Num128.CorrectResultInterval2ccL + " ; " + Core.Num128.CorrectResultInterval2ccR; break;
                                    }
                                else
                                    switch (inputStringFormat)
                                    {
                                        case 0: tbRes.Text = Core.Num128.CorrectResult2ccExp; break;
                                        case 1: tbRes.Text = Core.Num128.CorrectResultFraction2ccExpL + " / " + Core.Num128.CorrectResultFraction2ccExpL; break;
                                        case 2: tbRes.Text = Core.Num128.CorrectResultInterval2ccExpL + " ; " + Core.Num128.CorrectResultInterval2ccExpL; break;
                                    }
                        break;
                    case 3:
                        //tbCalcError.Text = Errors[3];
                        switch (inputStringFormat)
                        {
                            //case 0: tbCalcError.Text = Errors[3]; break;
                            case 0: tbCalcError.Text = Core.Num256.Error; break;
                            case 1: tbCalcError.Text = Core.Num256.ErrorFractionLeft + " / " + Core.Num256.ErrorFractionRight; break;
                            case 2: tbCalcError.Text = Core.Num256.ErrorIntervalLeft + " ; " + Core.Num256.ErrorFractionRight; break;
                        }
                        if (rB10cc32.Checked)
                            if (!cBExp32.Checked)
                                switch (inputStringFormat)
                                {
                                    case 0: tbRes.Text = Core.Num256.CorrectResult; break;
                                    case 1: tbRes.Text = Core.Num256.CorrectResultFractionL + " / " + Core.Num256.CorrectResultFractionR; break;
                                    case 2: tbRes.Text = Core.Num256.CorrectResultIntervalL + " ; " + Core.Num256.CorrectResultIntervalR; break;
                                }
                            //tbRes.Text = out256Dec;
                            else
                                switch (inputStringFormat)
                                {
                                    case 0: tbRes.Text = Core.Num256.CorrectResultExp; break;
                                    case 1: tbRes.Text = Core.Num256.CorrectResultFractionExpL + " / " + Core.Num256.CorrectResultFractionExpR; break;
                                    case 2: tbRes.Text = Core.Num256.CorrectResultIntervalExpL + " ; " + Core.Num256.CorrectResultIntervalExpR; break;
                                }
                        //tbRes.Text = out256DecE;
                        else
                            if (rB2cc32.Checked)
                                if (!cBExp32.Checked)
                                    switch (inputStringFormat)
                                    {
                                        case 0: tbRes.Text = Core.Num256.CorrectResult2cc; break;
                                        case 1: tbRes.Text = Core.Num256.CorrectResultFraction2ccL + " / " + Core.Num256.CorrectResultFraction2ccR; break;
                                        case 2: tbRes.Text = Core.Num256.CorrectResultInterval2ccL + " ; " + Core.Num256.CorrectResultInterval2ccR; break;
                                    }
                                //tbRes.Text = out256Bin;
                                else
                                    switch (inputStringFormat)
                                    {
                                        case 0: tbRes.Text = Core.Num256.CorrectResult2ccExp; break;
                                        case 1: tbRes.Text = Core.Num256.CorrectResultFraction2ccExpL + " / " + Core.Num256.CorrectResultInterval2ccExpR; break;
                                        case 2: tbRes.Text = Core.Num256.CorrectResultInterval2ccExpL + " ; " + Core.Num256.CorrectResultInterval2ccExpL; break;
                                    }
                        //tbRes.Text = out256BinE;
                        break;
                }
                tbRes.Refresh();
                tbCalcError.Refresh();

            }

            
        }

        //public delegate void TimerCountDel(Object threadContext);
        private void TimerCount(object sender, EventArgs e)
        {
            String elapsedTime = "";
            //stlStatus.Text = "Статус : Работаю... Осталось 32,64,128,256";
            bool i32=false;
            bool i64=false;
            bool i128=false;
            bool i256=false;
            stlTime.Text = "";
            //if (stlStatus)
            //Thread.CurrentThread.IsBackground = true;
            //while (isThreadsRunning(-1, inputStringFormat == 0 ? false : true) != 0)
            //{
                //if (isFormClosing)
                //{
                //    try
                //    { Thread.CurrentThread.Abort(); }
                //    catch (Exception ex)
                //    { return; }
                //}
                stlStatus.Text = "Статус : Работаю... ";
                if (isThreadsRunning(0, inputStringFormat == 0 ? false : true) != 0)
                {
                    if (!i32)
                    {
                        stlStatus.Text += " 32 ";
                        i32 = true;
                    }
                }
                else
                {
                    i64 = false;
                    i128 = false;
                    i256 = false;
                }

                if (isThreadsRunning(1, inputStringFormat == 0 ? false : true) != 0)
                {
                    if (!i64)
                    {
                        stlStatus.Text += "64 ";
                        i64 = true;
                    }
                }
                else
                {
                    i128 = false;
                    i256 = false;
                }

                if (isThreadsRunning(2, inputStringFormat == 0 ? false : true) != 0)
                {
                    if (!i128)
                    {
                        stlStatus.Text += "128 ";
                        i128 = true;
                    }
                }
                else
                {
                    i256 = false;
                }

                if (isThreadsRunning(3, inputStringFormat == 0 ? false : true) != 0)
                {
                    if (!i256)
                    {
                        stlStatus.Text += "256 ";
                        i256 = true;
                    }
                }
            
            //    Thread.Sleep(500);
            //}

            stlStatus.Text = "Статус : Завершено...";
            timeCounter.Stop();
            elapsedTime = "";
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                timeCounter.Elapsed.Hours, timeCounter.Elapsed.Minutes, timeCounter.Elapsed.Seconds,
                timeCounter.Elapsed.Milliseconds);

            stlTime.Text = " Время преоборазования : " + elapsedTime;
            timeCounter.Reset();
        }

        public delegate void dMakeCalculations();
        public void Calculation(Object threadContext)
        {
            String datastr = "";
            String datastrRight = "";
            int k;
            TreeNode tempNode;
            try
            {
                if (tbInput.Multiline == true)
                {
                    for (k = 0; k < tbInput.Lines.Length; k++)
                        datastr += tbInput.Lines[k];
                }
                else
                {
                    datastr = tbInput.Text;
                }
                if (cbClearLog.Checked)
                { // Очищать ли "Логи" при пересчете или нет
                    richTextBox1.Clear();
                }
                isCalcsReset = false;
                if (datastr.Length == 0)
                {
                    if (isTabIndexChanged == false)
                        setErrorOnInput();
                }
                else
                {
                    if ((tbInput.Text.IndexOf(",") == -1) && (tbInput.Text.IndexOf("e") == -1))
                    {
                        tbInput.Text += ",0";
                    }
                    //else
                    //{ 
                    //    if (tbInput.Text.IndexOf("e") != -1)
                    //      tt =   tbInput.Text.Substring(0,tbInput.Text.IndexOf("e"))+".0"+
                    //}
                    checkInputStringForCurrentFormat();
                    tbInput.Text = datastr;
                    // tbInput.Text = my754.testExponentNumber(tbInput.Text, (int)nUpDown.Value);
                    datastr = tbInput.Text;

                    Core.SetNumber(datastr);
                    // ThreadPool.QueueUserWorkItem(Core.SetNumber, datastr);

                    //calcFinished = true;

                    //checkInputStringForCurrentFormat();
                }
            }
            catch (FCCoreArithmeticException exA)
            {
                tempNode = mainNode.Nodes.Add("1", exA.Message, 1);
                tempNode.Nodes.Add(tbInput.Text);
            }
            catch (FCCoreFunctionException exF)
            {
                tempNode = mainNode.Nodes.Add("1", exF.Message, 2);
                tempNode.Nodes.Add(tbInput.Text);
            }
            catch (FCCoreGeneralException exG)
            {
                tempNode = mainNode.Nodes.Add("1", exG.Message, 3);
                tempNode.Nodes.Add(tbInput.Text);
            }
            catch (Exception ex)
            {// Rember this Exception in ExceptionStack
                tempNode = mainNode.Nodes.Add("1", ex.Message, 5);
                tempNode.Nodes.Add(tbInput.Text);
            }
        }
        private void bStart_Click(object sender, EventArgs e)
        {
            // Buttons 
            bStart.Enabled = false;
            bCancelStart.Visible = true;

            String outputLog="";
            String numLeft,numRight;
            Byte[] outputStr;
            bool was16cc = false;
            int i, separatorInd = 0 ;
            String separ = "";
            timeCounter.Reset();
            timeCounter.Start();
            if (tbInput.Text.IndexOf('/') != -1)
            {
                separ = "/";
                separatorInd = tbInput.Text.IndexOf('/');
            }
            if (tbInput.Text.IndexOf(';') != -1)
            {
                separ = ";";
                separatorInd = tbInput.Text.IndexOf(';');
            }
            
            try
            {
                if (currentCCOnTabs == true)
                {
                    convert16to2();
                    was16cc = true;
                }
                switch (inputStringFormat)
                {
                    case 0:
                        tbInput.Text = testExponentNumber(tbInput.Text, (int)nUpDown.Value);
                        break;
                    case 1:
                    case 2:
                        numLeft = tbInput.Text.Substring(0, separatorInd);
                        numRight = tbInput.Text.Substring(separatorInd + 1);
                        tbInput.Text = testExponentNumber(numLeft, (int)nUpDown.Value);
                        tbInput.Text+= separ + testExponentNumber(numRight, (int)nUpDown.Value);
                        break;
                }
                switch (inputStringFormat)
                {
                    case 0:
                        tbInput.Text = modifyMe((int)nUpDown.Value,tbInput.Text);
                        break;
                    case 1:
                    case 2:
                        numLeft = tbInput.Text.Substring(0, separatorInd);
                        numRight = tbInput.Text.Substring(separatorInd + 1);
                        tbInput.Text = modifyMe((int)nUpDown.Value, numLeft);
                        tbInput.Text += separ + modifyMe((int)nUpDown.Value, numRight);
                        break;
                }
                //tbInput.Text = modifyMe((int)nUpDown.Value);
                if (RegxTest())
                {
                    calcFinished = false;
                    isNum32Refreshed = false;
                    isNum64Refreshed = false;
                    isNum128Refreshed = false;
                    isNum256Refreshed = false;
                    stlStatus.Text = "Статус : Работаю... Осталось 32,64,128,256";
                    LockComponents((byte)LockCommands.Start);
                    //ThreadPool.QueueUserWorkItem(this.Calculation);
                    Calculation(1);          // Make Threading Here

                    refreshNumberStatus();
                    ThreadPool.QueueUserWorkItem(logResults);
                    ThreadPool.QueueUserWorkItem(FillForm);
                    ThreadPool.QueueUserWorkItem(calcResultsAndErrors);
                    tStatus.Start();
                    //ThreadPool.QueueUserWorkItem(TimerCount);
                    //calcResultsAndErrors(this, null);
                    //Form1_Paint(this, null);
                    //stlStatus.Text = "Статус : Завершено...";
                    if (isThreadsRunning(3, true) == 0)
                    {
                        bStart.Enabled = true;
                        bCancelStart.Visible = false;
                    }
                }
                else
                    setErrorOnInput();
               // progressBar1.Value = 0;
                
            }
            catch (FCCoreGeneralException ex)
            {
                catchException(excps.FCCoreGeneralException, ex.Message);
            }
            catch (FCCoreFunctionException ex)
            {
                catchException(excps.FCCoreFunctionException, ex.Message);
            }
            catch (Exception ex)
            {
                catchException(excps.FCCoreFunctionException,ex.Message);
            }
            statusStrip.Refresh();
            
        }
        private void recalculate_Click(object sender, EventArgs e)
        {
            int i, loop_counter, temp, selectedTab;
            loop_counter = inputStringFormat == 0 ? 1 : 2;
            String sign, currSeparator;
            String outputLog = "";
            String tempStr = "";
            Byte[] outputStr;
            selectedTab = tabControl_Format.SelectedIndex;
            bool was16cc = false;
            if (currentCCOnTabs == true)
            {
                convert16to2Recalc();
                was16cc = true;
            }
            stlStatus.Text = "Статус : Работаю... ";
            try
            {
                LockComponents((byte)LockCommands.Recalc);
                ThreadPool.QueueUserWorkItem(o =>
                {
                    switch (selectedTab)
                    {
                        case 0: // 32
                            if (inputStringFormat == 0)
                            {
                                //result = my754.selectOut(tbSign32.Text, tbExp32.Text, tbMantisa32.Text);
                                progInc();
                                Core.Num32.Sign = tbSign32.Text;
                                progInc();
                                Core.Num32.Exponenta = tbExp32.Text;
                                progInc();
                                Core.Num32.Mantisa = tbMantisa32.Text;
                                progInc();
                                Core.changeNumberState(false, true);
                                progInc();
                                Core.calcRes(Core.Num32, PartOfNumber.Left);
                                progInc();
                                changetbInputText(modifyMe((int)nUpDown.Value, Core.Num32.CorrectResult));
                                setProgress(100);
                            }
                            break;
                        case 1: // 64
                            switch (inputStringFormat)
                            {
                                case 0:
                                    //  result = my754.selectOut(tbSign64.Text, tbExp64.Text, tbMantisa64.Text);
                                    Core.Num64.Sign = tbSign64.Text; progInc();
                                    Core.Num64.Exponenta = tbExp64.Text; progInc();
                                    Core.Num64.Mantisa = tbMantisa64.Text; progInc();
                                    Core.changeNumberState(false, true); progInc();
                                    Core.calcRes(Core.Num64, PartOfNumber.Left); progInc();
                                    changetbInputText(modifyMe((int)nUpDown.Value, Core.Num64.CorrectResult));
                                    setProgress(100);
                                    break;

                                //result = my754.selectOut(sign, tbExp64_2.Text, tbMantisa64_2.Text);
                                case 1:
                                case 2:
                                    currSeparator = inputStringFormat == 1 ? "/" : ";"; progInc();
                                    if (inputStringFormat == 1)
                                    {
                                        Core.Num64.Sign = tbSign64.Text;
                                        Core.Num64.SignRight = tbSign64_2.Text; progInc();
                                    }
                                    else
                                        Core.Num64.Sign = tbSign64.Text;
                                    progInc();
                                    Core.Num64.Exponenta = tbExp64.Text;
                                    Core.Num64.Mantisa = tbMantisa64.Text; progInc();
                                    Core.changeNumberState(false, true); progInc();
                                    Core.calcRes(Core.Num64, PartOfNumber.Left); progInc();
                                    //tbInput.Text = Core.Num64.CorrectResult;
                                    progInc();
                                    Core.Num64.ExponentaRight = tbExp64_2.Text; progInc();
                                    Core.Num64.MantisaRight = tbMantisa64_2.Text; progInc();
                                    Core.changeNumberState(true, true); progInc();
                                    Core.calcRes(Core.Num64, PartOfNumber.Right); progInc();
                                    if (inputStringFormat == 1)
                                        changetbInputText(modifyMe((int)nUpDown.Value, Core.Num64.CorrectResultFractionL) + currSeparator + modifyMe((int)nUpDown.Value, Core.Num64.CorrectResultFractionR));
                                    else
                                        changetbInputText(modifyMe((int)nUpDown.Value, Core.Num64.CorrectResultIntervalL) + currSeparator + modifyMe((int)nUpDown.Value, Core.Num64.CorrectResultIntervalR));
                                    setProgress(100);
                                    break;
                            }
                            break;
                        case 2: // 128
                            switch (inputStringFormat)
                            {
                                case 0:
                                    //  result = my754.selectOut(tbSign64.Text, tbExp64.Text, tbMantisa64.Text);
                                    Core.Num128.Sign = tbSign128.Text; progInc();
                                    Core.Num128.Exponenta = tbExp128.Text; progInc();
                                    Core.Num128.Mantisa = tbMantisa128.Text; progInc();
                                    Core.changeNumberState(false, true); progInc();
                                    Core.calcRes(Core.Num128, PartOfNumber.Left); progInc();
                                    changetbInputText(modifyMe((int)nUpDown.Value, Core.Num128.CorrectResult));
                                    setProgress(100);
                                    break;

                                //result = my754.selectOut(sign, tbExp64_2.Text, tbMantisa64_2.Text);
                                case 1:
                                case 2:
                                    currSeparator = inputStringFormat == 1 ? "/" : ";"; progInc();
                                    if (inputStringFormat == 1)
                                    {
                                        Core.Num128.Sign = tbSign128.Text; progInc();
                                        Core.Num128.SignRight = tbSign128_2.Text;
                                    }
                                    else
                                        Core.Num128.Sign = tbSign128.Text; progInc();

                                    Core.Num128.Exponenta = tbExp128.Text; progInc();
                                    Core.Num128.Mantisa = tbMantisa128.Text; progInc();
                                    Core.changeNumberState(false, true); progInc();
                                    Core.calcRes(Core.Num128, PartOfNumber.Left); progInc();
                                    //tbInput.Text = modifyMe((int)nUpDown.Value, Core.Num128.CorrectResult);

                                    Core.Num128.ExponentaRight = tbExp128_2.Text; progInc();
                                    Core.Num128.MantisaRight = tbMantisa128_2.Text; progInc();
                                    Core.changeNumberState(true, true); progInc();
                                    Core.calcRes(Core.Num128, PartOfNumber.Right); progInc();
                                    if (inputStringFormat == 1)
                                        changetbInputText(modifyMe((int)nUpDown.Value, Core.Num128.CorrectResultFractionL) + currSeparator + modifyMe((int)nUpDown.Value, Core.Num128.CorrectResultFractionR));
                                    else
                                        changetbInputText(modifyMe((int)nUpDown.Value, Core.Num128.CorrectResultIntervalL) + currSeparator + modifyMe((int)nUpDown.Value, Core.Num128.CorrectResultIntervalR));
                                    setProgress(100);
                                    break;
                            }
                            break;
                        case 3: // 256
                            switch (inputStringFormat)
                            {
                                case 0:
                                    //  result = my754.selectOut(tbSign64.Text, tbExp64.Text, tbMantisa64.Text);
                                    Core.Num256.Sign = tbSign256.Text; progInc();
                                    Core.Num256.Exponenta = tbExp256.Text; progInc();
                                    Core.Num256.Mantisa = tbMantisa256.Text; progInc();
                                    Core.changeNumberState(false, true); progInc();
                                    Core.calcRes(Core.Num256, PartOfNumber.Left); progInc();
                                    changetbInputText(modifyMe((int)nUpDown.Value, Core.Num256.CorrectResult)); setProgress(100);
                                    break;

                                //result = my754.selectOut(sign, tbExp64_2.Text, tbMantisa64_2.Text);
                                case 1:
                                case 2:
                                    currSeparator = inputStringFormat == 1 ? "/" : ";"; progInc();
                                    if (inputStringFormat == 1)
                                    {
                                        Core.Num256.Sign = tbSign256.Text; progInc();
                                        Core.Num256.SignRight = tbSign256_2.Text;
                                    }
                                    else
                                        Core.Num256.Sign = tbSign256.Text; progInc();

                                    Core.Num256.Exponenta = tbExp256.Text; progInc();
                                    Core.Num256.Mantisa = tbMantisa256.Text; progInc();
                                    Core.changeNumberState(false, true); progInc();
                                    Core.calcRes(Core.Num256, PartOfNumber.Left); progInc();
                                    //tbInput.Text = modifyMe((int)nUpDown.Value, Core.Num256.CorrectResult);

                                    Core.Num256.ExponentaRight = tbExp256_2.Text; progInc();
                                    Core.Num256.MantisaRight = tbMantisa256_2.Text; progInc();
                                    Core.changeNumberState(true, true); progInc();
                                    Core.calcRes(Core.Num256, PartOfNumber.Right); progInc();
                                    if (inputStringFormat == 1)
                                        changetbInputText(modifyMe((int)nUpDown.Value, Core.Num256.CorrectResultFractionL) + currSeparator + modifyMe((int)nUpDown.Value, Core.Num256.CorrectResultFractionR));
                                    else
                                        changetbInputText(modifyMe((int)nUpDown.Value, Core.Num256.CorrectResultIntervalL) + currSeparator + modifyMe((int)nUpDown.Value, Core.Num256.CorrectResultIntervalR));
                                    setProgress(100);
                                    break;
                            }
                            break;
                    }

                    if (was16cc)
                    {
                        convert2to16();
                    }
                    stlStatus.Text = "Статус : Завершено...";
                    setProgress(0);
                });

            }
            catch (FCCoreArithmeticException ex)
            {
                catchException(excps.FCCoreArithmeticException, ex.Message);
            }
            catch (FCCoreGeneralException ex)
            {
                catchException(excps.FCCoreGeneralException, ex.Message);
            }
            catch (FCCoreFunctionException ex)
            {
                catchException(excps.FCCoreFunctionException, ex.Message);
            }
            catch (Exception ex)
            {
                catchException(excps.Exception, ex.Message);
            }

            statusStrip.Refresh();
        }
     
        public void catchException(excps ex,String message)
        {
            String outputLog = "";
            Byte[] outputStr;
            Stream str;
            TreeNode tempNode;
            int i,selInd;
            selInd = tabControl_Format.SelectedIndex;
            String currSelectedTab = selInd == 0 ? " 32p " : selInd==1 ? " 64p " :selInd == 2? " 128p ": " 256p ";
            String currSN = currentCCOnTabs == false ? " 2cc " : " 16cc ";
            String currNF = inputStringFormat == 0 ? " Integer " : inputStringFormat == 1 ? " Fratcion " : " Interval ";
            richTextBox1.Text += "\r\nException Handler: Number=" + tbInput.Text;

            outputLog = tbInput.Text + currSN + currNF + currSelectedTab + " \r\n";
            switch (ex)
            {
                case excps.FCCoreArithmeticException:

                    tempNode = mainNode.Nodes.Add("1", message+ "\r\n" + outputLog, 1);//exA.Message
                    tempNode.Nodes.Add(tbInput.Text);
                    break;
                case excps.FCCoreFunctionException:

                    tempNode = mainNode.Nodes.Add("1", message+ "\r\n" + outputLog, 2); //exF.Message
                    tempNode.Nodes.Add(tbInput.Text);
                    break;
                case excps.FCCoreGeneralException:

                    tempNode = mainNode.Nodes.Add("1", message+ "\r\n" +outputLog, 3);//exG.Message
                    tempNode.Nodes.Add(tbInput.Text);
                    break;
                case excps.Exception:
                    // Rember this Exception in ExceptionStack
                    tempNode = mainNode.Nodes.Add("1",ex.ToString()+"  " + message + "\r\n" + outputLog, 5);//ex.Message
                    tempNode.Nodes.Add(tbInput.Text);
                    break;
            }
            switch (ex)
            {
                case excps.FCCoreGeneralException:
                   
                    // open file to write result
                    str = File.OpenWrite("Log.dat");

                    // convert string to byte[]
                    outputStr = new byte[outputLog.Length];
                    for (i = 0; i < outputLog.Length; i++)
                    {
                        outputStr[i] = (byte)outputLog[i];
                    }
                    str.Seek(0, SeekOrigin.End);
                    str.Write(outputStr, 0, (int)(outputStr.Length));
                    str.Close();
                    break;

                case excps.FCCoreFunctionException:
                case excps.Exception:
                    str = File.OpenWrite("err.dat");

                    // convert string to byte[]
                    outputStr = new byte[outputLog.Length];
                    for (i = 0; i < outputLog.Length; i++)
                    {
                        outputStr[i] = (byte)outputLog[i];
                    }
                    str.Seek(0, SeekOrigin.End);
                    str.Write(outputStr, 0, (int)(outputStr.Length));
                    str.Close();
                    break;
            }
        }

        /// <summary>
        /// Function depending on tabControl converts all Format Fields (exp,mantisa, if necesery MF & CF)
        /// from 2cc to 16cc & back.
        /// Функция в зависимости от выбранной закладки переводит все поля формата (экспоненту, мантису,
        /// и если необходимо Код_Формата и Модификатор_формата) из 2сс в 16сс и обратно
        /// </summary>
        /// <param name="inputSize">one of 4 options : 32, 64, 128, 256</param>
        public void convert2to16()
        {
            String temp_str;
            String result;
            Object temp_tb ;
            int size = 0;
            int i;
            int count = 2;
            String number = "Num";
            try
            {
                int currMBits = 0;
                //int inputStrForm = inputStringFormat == 0 ? 0 : 1;
                for (int j = 0; j < 4; j++)
                {
                    size = 32 << j;
                    for (i = 0; i < 4; i++)
                    {
                        if ((tabControl_Format.SelectedIndex == 0) && (i > 1))
                            continue;
                        temp_str = FormatFileds[i] + size.ToString();

                        temp_tb = tabControl_Format.TabPages[j].Controls["gBFieldContainer" + size.ToString()].Controls[temp_str];

                        result = Core.convert2to16(((TextBox)temp_tb).Text);
                        switch (inputStringFormat)
                        {
                            case 0:
                                currMBits = Core.Numbers[j].MBits;
                                break;
                            case 1:
                            case 2:
                                currMBits = Core.Numbers[j].MBitsFI;
                                break;
                        }
                        if (i > 1)
                            ((TextBox)temp_tb).Text = result;//.Substring(0, currMBits);
                        else
                            ((TextBox)temp_tb).Text = result;//.Substring(0, currMBits);

                    }
                    if ((tabControl_Format.SelectedIndex > 0) && (size > 32))
                    {
                        if (getIndexType() != 0)
                        {
                            for (i = 0; i < 2; i++)
                            {
                                temp_str = FormatFileds[i] + size.ToString() + "_2";

                                temp_tb = tabControl_Format.TabPages[j].Controls["gBFieldContainer" + size.ToString()].Controls[temp_str];

                                result = Core.convert2to16(((TextBox)temp_tb).Text);

                                if (i > 1)
                                    ((TextBox)temp_tb).Text = result;
                                else
                                    ((TextBox)temp_tb).Text = result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FCCoreGeneralException("Func 'convert2to16'=["+ex.Message+"]");
            }
        }
        /// <summary>
        /// Converts var's fileds values from 16cc to 2cc 
        /// </summary>
        public void convert16to2()
        {
            try
            {
                tbExp32.Text = Core.Num32.Exponenta;
                tbMantisa32.Text = Core.Num32.Mantisa;
                tbMF32.Text = Core.Num32.MF;
                tbCF32.Text = Core.Num32.CF;

                tbExp64.Text = Core.Num64.Exponenta;
                tbMantisa64.Text = Core.Num64.Mantisa;
                tbMF64.Text = Core.Num64.MF;
                tbCF64.Text = Core.Num64.CF;

                tbMF128.Text = Core.Num64.MF;
                tbCF128.Text = Core.Num64.CF;
                tbMF256.Text = Core.Num64.MF;
                tbCF256.Text = Core.Num64.CF;
                if (radioFloat.Checked)
                {
                    tbExp64_2.Text = Core.Num64.ExponentaRight;
                    tbMantisa64_2.Text = Core.Num64.MantisaRight;

                    tbExp128_2.Text = Core.Num128.ExponentaRight;
                    tbMantisa128_2.Text = Core.Num128.MantisaRight;

                    tbExp256_2.Text = Core.Num256.ExponentaRight;
                    tbMantisa256_2.Text = Core.Num256.MantisaRight;
                }
                if (radioInterval.Checked)
                {
                    tbSign64_2.Text = Core.Num64.SignRight;
                    tbExp64_2.Text = Core.Num64.ExponentaRight;
                    tbMantisa64_2.Text = Core.Num64.MantisaRight;

                    tbSign128_2.Text = Core.Num128.SignRight;
                    tbExp128_2.Text = Core.Num128.ExponentaRight;
                    tbMantisa128_2.Text = Core.Num128.MantisaRight;

                    tbSign256_2.Text = Core.Num256.SignRight;
                    tbExp256_2.Text = Core.Num256.ExponentaRight;
                    tbMantisa256_2.Text = Core.Num256.MantisaRight;
                }
                tbExp128.Text = Core.Num128.Exponenta;
                tbMantisa128.Text = Core.Num128.Mantisa;
                tbMF128.Text = Core.Num128.MF;
                tbCF128.Text = Core.Num128.CF;

                tbExp256.Text = Core.Num256.Exponenta;
                tbMantisa256.Text = Core.Num256.Mantisa;
                tbMF256.Text = Core.Num256.MF;
                tbCF256.Text = Core.Num256.CF;
            }
            catch (Exception ex)
            {
                throw new FCCoreGeneralException("Func 'convert16to2'=["+ex.Message+"]");
            }
        }
        /// <summary>
        /// Converts text fileds values from 16cc to 2cc 
        /// </summary>
        public void convert16to2Recalc()
        {
            int e32, m32, e64, m64, e128, m128, e256, m256;
            int e64fi, m64fi, e128fi, m128fi, e256fi, m256fi;
            
            e32 = Core.Num32.EBits;
            m32 = Core.Num32.MBits;
            e64 = Core.Num64.EBits;
            m64 = Core.Num64.MBits;
            e128 = Core.Num128.EBits;
            m128 = Core.Num128.MBits;
            e256 = Core.Num256.EBits;
            m256 = Core.Num256.MBits;

            e64fi = Core.Num64.EBitsFI;
            m64fi = Core.Num64.MBitsFI;
            e128fi = Core.Num128.EBitsFI;
            m128fi = Core.Num128.MBitsFI;
            e256fi = Core.Num256.EBitsFI;
            m256fi = Core.Num256.MBitsFI;

            tbMF32.Text = Core.convert16to2(tbMF32.Text, 1);
            tbCF32.Text = Core.convert16to2(tbCF32.Text, 1);

            tbMF64.Text = Core.convert16to2(tbMF64.Text, 2);
            tbCF64.Text = Core.convert16to2(tbCF64.Text, 2);

            tbMF128.Text = Core.convert16to2(tbMF128.Text, 4);
            tbCF128.Text = Core.convert16to2(tbCF128.Text, 4);

            tbMF256.Text = Core.convert16to2(tbMF256.Text, 4);
            tbCF256.Text = Core.convert16to2(tbCF256.Text, 4);

            if (radioInteger.Checked)
            {
                if (!chbTetra.Checked)
                {
                    tbExp32.Text = Core.convert16to2(tbExp32.Text, e32);
                    tbMantisa32.Text = Core.convert16to2(tbMantisa32.Text, m32);

                    tbExp64.Text = Core.convert16to2(tbExp64.Text, e64);
                    tbMantisa64.Text = Core.convert16to2(tbMantisa64.Text, m64);

                    tbExp128.Text = Core.convert16to2(tbExp128.Text, e128);
                    tbMantisa128.Text = Core.convert16to2(tbMantisa128.Text, m128);

                    tbExp256.Text = Core.convert16to2(tbExp256.Text, e256);
                    tbMantisa256.Text = Core.convert16to2(tbMantisa256.Text, m256);
                    return;
                }
                else
                {
                    tbExp32.Text = Core.convert16to2(tbExp32.Text, e32*2);
                    tbMantisa32.Text = Core.convert16to2(tbMantisa32.Text, m32*2);

                    tbExp64.Text = Core.convert16to2(tbExp64.Text, e64 * 2);
                    tbMantisa64.Text = Core.convert16to2(tbMantisa64.Text, m64 * 2);

                    tbExp128.Text = Core.convert16to2(tbExp128.Text, e128 * 2);
                    tbMantisa128.Text = Core.convert16to2(tbMantisa128.Text, m128 * 2);

                    tbExp256.Text = Core.convert16to2(tbExp256.Text, e256 * 2);
                    tbMantisa256.Text = Core.convert16to2(tbMantisa256.Text, m256 * 2);
                    return;
                }
            }
            
            if (radioFloat.Checked || radioInterval.Checked)
            {
                if (chbTetra.Checked)
                {
                    tbExp32.Text = Core.convert16to2(tbExp32.Text, e32 * 2);
                    tbMantisa32.Text = Core.convert16to2(tbMantisa32.Text, m32 * 2);
                    tbExp64_2.Text = Core.convert16to2(tbExp64_2.Text, e64fi * 2);
                    tbMantisa64_2.Text = Core.convert16to2(tbMantisa64_2.Text, m64fi * 2);

                    tbExp128_2.Text = Core.convert16to2(tbExp128_2.Text, e128fi * 2);
                    tbMantisa128_2.Text = Core.convert16to2(tbMantisa128_2.Text, m128fi * 2);

                    tbExp256_2.Text = Core.convert16to2(tbExp256_2.Text, e256fi * 2);
                    tbMantisa256_2.Text = Core.convert16to2(tbMantisa256_2.Text, m256fi * 2);
                }
                else
                {
                    tbExp32.Text = Core.convert16to2(tbExp32.Text, e32);
                    tbMantisa32.Text = Core.convert16to2(tbMantisa32.Text, m32);
                    tbExp64.Text = Core.convert16to2(tbExp64.Text, e64fi);
                    tbMantisa64.Text = Core.convert16to2(tbMantisa64.Text, m64fi);

                    tbExp128.Text = Core.convert16to2(tbExp128.Text, e128fi);
                    tbMantisa128.Text = Core.convert16to2(tbMantisa128.Text, m128fi);

                    tbExp256.Text = Core.convert16to2(tbExp256.Text, e256fi);
                    tbMantisa256.Text = Core.convert16to2(tbMantisa256.Text, m256fi);

                    tbExp64_2.Text = Core.convert16to2(tbExp64_2.Text, e64fi);
                    tbMantisa64_2.Text = Core.convert16to2(tbMantisa64_2.Text, m64fi);

                    tbExp128_2.Text = Core.convert16to2(tbExp128_2.Text, e128fi);
                    tbMantisa128_2.Text = Core.convert16to2(tbMantisa128_2.Text, m128fi);

                    tbExp256_2.Text = Core.convert16to2(tbExp256_2.Text, e256fi);
                    tbMantisa256_2.Text = Core.convert16to2(tbMantisa256_2.Text, m256fi);
                }
            }
            
        }
        private void l2ccTo16cc_Click(object sender, EventArgs e)
        {
            try
            {
                if (l2ccTo16cc.Text == "2cc")
                {
                    l2ccTo16cc.Text = "16cc";
                    currentCCOnTabs = true;
                    convert2to16();
                    changeBitLength(1);
                    tbMantisa128.ScrollBars = ScrollBars.None;
                    tbMantisa128.TextAlign = HorizontalAlignment.Center;
                    tbMantisa128.Size = new Size(tbMantisa128.Size.Width, 24);
                    tbMantisa128_2.ScrollBars = ScrollBars.None;
                    tbMantisa128_2.TextAlign = HorizontalAlignment.Center;
                    tbMantisa128_2.Size = new Size(tbMantisa128.Size.Width, 24);
                    if (inputStringFormat > 0)
                    {
                        tbMantisa256.ScrollBars = ScrollBars.None;
                        tbMantisa256.TextAlign = HorizontalAlignment.Center;
                        tbMantisa256.Size = new Size(tbMantisa256.Size.Width, 24);
                        tbMantisa256_2.ScrollBars = ScrollBars.None;
                        tbMantisa256_2.TextAlign = HorizontalAlignment.Center;
                        tbMantisa256_2.Size = new Size(tbMantisa256.Size.Width, 24);
                    }
                }
                else
                {
                    convert16to2();
                    l2ccTo16cc.Text = "2cc";
                    currentCCOnTabs = false;
                    changeBitLength(0);
                    tbMantisa128.ScrollBars = ScrollBars.Horizontal;
                    tbMantisa128.TextAlign = HorizontalAlignment.Left;
                    tbMantisa128.Size = new Size(tbMantisa128.Size.Width, 41);
                    tbMantisa128_2.ScrollBars = ScrollBars.Horizontal;
                    tbMantisa128_2.TextAlign = HorizontalAlignment.Left;
                    tbMantisa128_2.Size = new Size(tbMantisa128.Size.Width, 41);
                    if (inputStringFormat > 0)
                    {
                        tbMantisa256.ScrollBars = ScrollBars.Horizontal;
                        tbMantisa256.TextAlign = HorizontalAlignment.Left;
                        tbMantisa256.Size = new Size(tbMantisa256.Size.Width, 41);
                        tbMantisa256_2.ScrollBars = ScrollBars.Horizontal;
                        tbMantisa256_2.TextAlign = HorizontalAlignment.Left;
                        tbMantisa256_2.Size = new Size(tbMantisa256.Size.Width, 41);
                    }
                }
                Form1_Paint(sender, null);
            }
            catch(Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'l2ccTo16cc_Click'=["+ex.Message+"]");
            }
        }
        /// <summary>
        /// Changes Bit Length for all formats
        /// </summary>
        /// <param name="cc">Current scale of notation</param>
        public void changeBitLength(int cc)
        {
            int factor = cc == 0 ? 1: 4 ;
            switch (inputStringFormat)
            {
                case 0:
                    tbExp32.MaxLength = Core.Num32.EBits % factor > 0 ? Core.Num32.EBits / factor + 1 : Core.Num32.EBits / factor;
                    tbExp64.MaxLength = Core.Num64.EBits % factor > 0 ? Core.Num64.EBits / factor + 1 : Core.Num64.EBits / factor;
                    tbExp128.MaxLength = Core.Num128.EBits % factor > 0 ? Core.Num128.EBits / factor + 1 : Core.Num128.EBits / factor;
                    tbExp256.MaxLength = Core.Num256.EBits % factor > 0 ? Core.Num256.EBits / factor + 1 : Core.Num256.EBits / factor;

                    tbMantisa32.MaxLength = Core.Num32.MBits % factor > 0 ? Core.Num32.MBits / factor + 1 : Core.Num32.MBits / factor;
                    tbMantisa64.MaxLength = Core.Num64.MBits % factor > 0 ? Core.Num64.MBits / factor + 1 : Core.Num64.MBits / factor;
                    tbMantisa128.MaxLength = Core.Num128.MBits % factor > 0 ? Core.Num128.MBits / factor + 1 : Core.Num128.MBits / factor;
                    tbMantisa256.MaxLength = Core.Num256.MBits % factor > 0 ? Core.Num256.MBits / factor + 1 : Core.Num256.MBits / factor;

                    // MF & MC
                    break;
                case 1:
                case 2:
                    tbExp64.MaxLength = Core.Num64.EBitsFI % factor > 0 ? Core.Num64.EBitsFI / factor + 1 : Core.Num64.EBitsFI / factor;
                    tbExp128.MaxLength = Core.Num128.EBitsFI % factor > 0 ? Core.Num128.EBitsFI / factor + 1 : Core.Num128.EBitsFI / factor;
                    tbExp256.MaxLength = Core.Num256.EBitsFI % factor > 0 ? Core.Num256.EBitsFI / factor + 1 : Core.Num256.EBitsFI / factor;

                    tbMantisa64.MaxLength = Core.Num64.MBitsFI % factor > 0 ? Core.Num64.MBitsFI / factor + 1 : Core.Num64.MBitsFI / factor;
                    tbMantisa128.MaxLength = Core.Num128.MBitsFI % factor > 0 ? Core.Num128.MBitsFI / factor + 1 : Core.Num128.MBitsFI / factor;
                    tbMantisa256.MaxLength = Core.Num256.MBitsFI % factor > 0 ? Core.Num256.MBitsFI / factor + 1 : Core.Num256.MBitsFI / factor;
                    
                    tbExp64_2.MaxLength = Core.Num64.EBitsFI % factor > 0 ? Core.Num64.EBitsFI / factor + 1 : Core.Num64.EBitsFI / factor;
                    tbExp128_2.MaxLength = Core.Num128.EBitsFI % factor > 0 ? Core.Num128.EBitsFI / factor + 1 : Core.Num128.EBitsFI / factor;
                    tbExp256_2.MaxLength = Core.Num256.EBitsFI % factor > 0 ? Core.Num256.EBitsFI / factor + 1 : Core.Num256.EBitsFI / factor;

                    tbMantisa64_2.MaxLength = Core.Num64.MBitsFI % factor > 0 ? Core.Num64.MBitsFI / factor + 1 : Core.Num64.MBitsFI / factor;
                    tbMantisa128_2.MaxLength = Core.Num128.MBitsFI % factor > 0 ? Core.Num128.MBitsFI / factor + 1 : Core.Num128.MBitsFI / factor;
                    tbMantisa256_2.MaxLength = Core.Num256.MBitsFI % factor > 0 ? Core.Num256.MBitsFI / factor + 1 : Core.Num256.MBitsFI / factor;
                    break;
            }
            
        }
                 
        public delegate void changetbInputTextDel(Object inputString);
        private void changetbInputText(Object inputString)
        {
            String inputStr = (String)inputString;
            if (tbInput.InvokeRequired)
            {
                changetbInputTextDel d = new changetbInputTextDel(changetbInputText);
                this.Invoke(d, new object[] { inputString });
            }
            else 
            {
                tbInput.Text = inputStr;
            }
        }
       
        
        /// <summary>
        /// Locking all  components when calculations
        /// </summary>
        /// <param name="Command">1 - Start; 2 - Recalculate</param>
        public void LockComponents(byte Command)
        {
            timeOnForm("Lock Components");
            // menu
                menuStrip_Global.Enabled = false;
            // 2cc 16cc
                l2ccTo16cc.Enabled = false;
            // Buttons
                bLoad.Enabled = false;
                bClear.Enabled = false;
                groupBox3.Enabled = false;
                nUpDown.Enabled = false;
                tbInput.Enabled = false;

                bStop_Thread32.Enabled = true;
                bStop_Thread64.Enabled = true;
                bStop_Thread128.Enabled = true;
                bStop_Thread256.Enabled = true;

                switch (Command)
                {
                    case 1: recalculate.Enabled = false; break;
                    case 2: bStart.Enabled = false; break;
                }
        }

        /// <summary>
        /// Unlocking all components after calculations
        /// </summary>
        public void UnLockComponents()
        {
            timeOnForm("Unlock Components");
            // menu
                menuStrip_Global.Enabled = true;
            // 2cc 16cc
                l2ccTo16cc.Enabled = true;
            // Buttons
                bLoad.Enabled = true;
                bClear.Enabled = true;
                groupBox3.Enabled = true;
                nUpDown.Enabled = true;
                tbInput.Enabled = true;

                //bStop_Thread32.Enabled = false;
                //bStop_Thread64.Enabled = false;
                //bStop_Thread128.Enabled = false;
                //bStop_Thread256.Enabled = false;
            // Special Buttons 
                bStart.Enabled = true;
                recalculate.Enabled = true;
        }
       
        public bool canAddDigit(char inChar, int bit)
        { 
            String hex_table = "0123456789ABCDEF";
            int ind = hex_table.IndexOf(inChar);
            if (ind > (2 >> bit))
            {
                return false;
            }
            else
                return true;
        }
        private void tb_KeyUp(object sender, KeyEventArgs e)
        {  
            bool isMantisa = false;
            bool isExp = false;
            String numFormat = "";
            String propName = "";
            int numf=0;
            int curr_len = ((TextBox)sender).Text.Length;
            int curr_bit =0;
            int max_lenExp = 0, max_lenMan = 0;
            int max_lenExpFI = 0, max_lenManFI = 0;

            try
            {
                if (((TextBox)sender).Text.Length > 0)
                {
                    if (((TextBox)sender).Name[2] == 'E') // Exp
                    {
                        isExp = true;
                        propName = "EBits";
                    }
                    else // Mantissa
                    {
                        isMantisa = true;
                        propName = "MBits";
                    }

                    switch (tabControl_Format.SelectedIndex)
                    {
                        case 0:
                            numFormat = "32"; numf = 0;
                            if (inputStringFormat == 0)
                            {
                                max_lenExp = Core.Num32.EBits;
                                max_lenMan = Core.Num32.MBits;
                                curr_bit = (int)Core.Num32.GetType().GetProperty(propName).GetValue(Core.Num32, null);
                            }
                            max_lenExpFI = 0;//Core.Num32.EBitsFI;
                            max_lenManFI = 0;//Core.Num32.MBitsFI;
                            break;
                        case 1:
                            numFormat = "64"; numf = 1;
                            switch (inputStringFormat)
                            {
                                case 0:
                                    max_lenExp = Core.Num64.EBits;
                                    max_lenMan = Core.Num64.MBits;
                                    curr_bit = (int)Core.Num64.GetType().GetProperty(propName).GetValue(Core.Num64, null);
                                    break;
                                case 1:
                                case 2:
                                    max_lenExpFI = Core.Num64.EBitsFI;
                                    max_lenManFI = Core.Num64.MBitsFI;
                                    curr_bit = (int)Core.Num64.GetType().GetProperty(propName + "FI").GetValue(Core.Num64, null);
                                    break;
                            }
                            break;
                        case 2:
                            numFormat = "128"; numf = 2;
                            switch (inputStringFormat)
                            {
                                case 0:
                                    max_lenExp = Core.Num128.EBits;
                                    max_lenMan = Core.Num128.MBits;
                                    curr_bit = (int)Core.Num128.GetType().GetProperty(propName).GetValue(Core.Num128, null);
                                    break;
                                case 1:
                                case 2:
                                    max_lenExpFI = Core.Num128.EBitsFI;
                                    max_lenManFI = Core.Num128.MBitsFI;
                                    curr_bit = (int)Core.Num128.GetType().GetProperty(propName + "FI").GetValue(Core.Num128, null);
                                    break;
                            }
                            break;
                        case 3:
                            numFormat = "256"; numf = 3;
                            switch (inputStringFormat)
                            {
                                case 0:
                                    max_lenExp = Core.Num256.EBits;
                                    max_lenMan = Core.Num256.MBits;
                                    curr_bit = (int)Core.Num256.GetType().GetProperty(propName).GetValue(Core.Num256, null);
                                    break;
                                case 1:
                                case 2:
                                    max_lenExpFI = Core.Num256.EBitsFI;
                                    max_lenManFI = Core.Num256.MBitsFI;
                                    curr_bit = (int)Core.Num256.GetType().GetProperty(propName + "FI").GetValue(Core.Num256, null);
                                    break;
                            }
                            break;
                    }

                    if (currentCCOnTabs == true) // 16cc
                    {
                        // if bit length is more and the last bit is greater then it should be with current bit quantity
                        // Если кол-во бит превышает допустимое кол-во и старший бит больше чем он должен быть с текущим кол-вом бит
                        //char tt =
                        bool checkRule = true ? Core.convert10to16((1 << curr_bit % 4) - 1) >= ((TextBox)sender).Text[0] : false;
                        if (curr_len * 4 > curr_bit && !checkRule)
                        {
                            if (!canAddDigit(((TextBox)sender).Text[0], curr_bit % 4))
                            {
                                int temp = curr_bit % 4;
                                char tempres = Core.convert10to16((1 << temp) - 1);
                                ((TextBox)sender).Text = tempres + ((TextBox)sender).Text.Substring(1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                catchException(excps.Exception,ex.Message);
            }
        
        }
        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
            if (currentCCOnTabs)
            {
                if (((e.KeyChar < '0') || (e.KeyChar > '9')) && ((e.KeyChar < 'A') || (e.KeyChar > 'Z')) &&(e.KeyChar != '\b'))
                    e.KeyChar = '\0';
            }
            else
            {
                if (((e.KeyChar < '0') || (e.KeyChar > '1')) && (e.KeyChar != '\b'))
                    e.KeyChar = '\0';
            }
        }

      
        private void bCancelStart_Click(object sender, EventArgs e)
        {
            if ((Core.thread32.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0)
            { 
                try
                {
                    Core.thread32.Abort();
                }
                catch (Exception ex)
                {
                }
            }
            if ((Core.thread64.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0)
            {
                try
                {
                    Core.thread64.Abort();
                }
                catch (Exception ex)
                {
                }
            }
            if ((Core.thread128.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0)
            {
                try
                {
                    Core.thread128.Abort();
                }
                catch (Exception ex)
                {

                }
            }
            if ((Core.thread256.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0)
            {
                try
                {
                    Core.thread256.Abort();
                }
                catch (Exception ex)
                {
                }
            }


            if ((Core.thread64_right.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0)
            {
                try
                {
                    Core.thread64_right.Abort();
                }
                catch (Exception ex)
                {
                }
            }
            if ((Core.thread128_right.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0)
            {
                try
                {
                    Core.thread128_right.Abort();
                }
                catch (Exception ex)
                {
                }
            }
            if ((Core.thread256_right.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0)
            {
                try
                {
                    Core.thread256_right.Abort();
                }
                catch (Exception ex)
                {
                }
            }
            
            bCancelStart.Visible = false;
            bStart.Enabled = true;
            stlStatus.Text = "Статус : Отменено...";
            bClear_Click(sender,e);
        }
      
        /// <summary>
        /// Verifies if thread(s) are still running 
        /// </summary>
        /// <param name="ThreadNumber">Thread Number to verify (0..3), if -1 then check for all Threads</param>
        /// <param name="includeRightPart">Verifies left and right part if true, else only left part</param>
        /// <returns>-1 -  Error thread == null ; 0 - There is no Thread are Running; 1 - There are at least one Thread is Running.</returns>
        private int isThreadsRunning(int ThreadNumber, bool includeRightPart)
        {
            switch (ThreadNumber)
            {
                case -1:
                    if ((Core.thread32 == null) || (Core.thread64 == null) || (Core.thread128 == null) || (Core.thread256 == null))
                    {
                        if (includeRightPart)
                        {
                            if ((Core.thread64_right == null) || (Core.thread128_right == null) || (Core.thread256_right == null))
                                return -1;
                        }
                        else
                            return -1;
                    }

                    if ((Core.thread32.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0){
                        if ((Core.thread64.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0){
                            if ((Core.thread128.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0){
                                if ((Core.thread256.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0){
                                    if (includeRightPart)
                                    {
                                        if ((Core.thread64_right.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0){
                                            if ((Core.thread128_right.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0){
                                                if ((Core.thread256_right.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0)
                                                {
                                                    return 0;
                                                }else return 1;
                                            }else return 1;
                                        }else return 1;
                                    }
                                    else return 0;
                                }else return 1;
                            }else return 1;
                        }else return 1;
                    }else return 1;
                    //break;
                case 0:
                    if (Core.thread32 == null)
                        return -1;
                    if ((Core.thread32.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0)
                    {
                        return 0;
                    }
                    else
                        return 1;
                    //break;
                case 1:
                    if (Core.thread64 == null)
                    {
                        if (includeRightPart)
                        {
                            if (Core.thread64_right == null)
                                return -1;
                        }
                        else
                            return -1;
                    }
                    if ((Core.thread64.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0)
                    {
                        if (includeRightPart)
                        {
                            if ((Core.thread64_right.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0)
                            {
                                return 0;
                            }
                            else return 1;
                        }
                        else
                            return 0;
                    }
                    else return 1;
                   // break;

                case 2:
                    if (Core.thread128 == null)
                    {
                        if (includeRightPart)
                        {
                            if (Core.thread128_right == null)
                                return -1;
                        }
                        else
                            return -1;
                    }
                    if ((Core.thread128.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0)
                    {
                        if (includeRightPart)
                        {
                            if ((Core.thread128_right.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0)
                            {
                                return 0;
                            }
                            else
                                return 1;
                        }
                        else
                            return 0;
                    }
                    else return 1;
                    //break;

                case 3:
                    if (Core.thread256 == null)
                    {
                        if (includeRightPart)
                        {
                            if (Core.thread256_right == null)
                                return -1;
                        }
                        else
                            return -1;
                    }
                    if ((Core.thread256.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0)
                    {
                        if (includeRightPart)
                        {
                            if ((Core.thread256_right.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Aborted)) != 0)
                            {
                                return 0;
                            }
                            else return 1;
                        }
                        else
                            return 0;
                    }
                    else return 1;
                    //break;

                default:
                    return -1;
            }
        }

        private void cbDebug_CheckedChanged(object sender, EventArgs e)
        {

            if (((CheckBox)sender).Checked)
            {
                this.MinimumSize = new Size(this.Size.Width + 500, this.Size.Height);
                this.MaximumSize = new Size(this.Size.Width + 500, this.Size.Height);
            }
            else
            {
                this.MinimumSize = new Size(this.Size.Width - 500, this.Size.Height);
                this.MaximumSize = new Size(this.Size.Width - 500, this.Size.Height);
            }
        }

        private void bStop_Thread32_Click(object sender, EventArgs e)
        {
            if (Core.thread32 != null)
            {
                if ((Core.thread32.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) != 0)
                {
                    try
                    {
                        Core.thread32.Abort();
                        lbEvent.Items.Add("32 Abort");
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private void bStop_Thread64_Click(object sender, EventArgs e)
        {
            if (Core.thread64 != null)
            {
                if ((Core.thread64.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) != 0)
                {
                    try
                    {
                        Core.thread64.Abort();
                        lbEvent.Items.Add("64 Abort");
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            if (Core.thread64_right != null)
            {
                if ((Core.thread64_right.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) != 0)
                {
                    try
                    {
                        Core.thread64_right.Abort();
                        lbEvent.Items.Add("64 Abort2");
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private void bStop_Thread128_Click(object sender, EventArgs e)
        {
            if (Core.thread128 != null)
            {
                if ((Core.thread128.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) != 0)
                {
                    try
                    {
                        Core.thread128.Abort();
                        lbEvent.Items.Add("128 Abort");
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            if (Core.thread128_right != null)
            {
                if ((Core.thread128_right.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) != 0)
                {
                    try
                    {
                        Core.thread128_right.Abort();
                        lbEvent.Items.Add("128 Abort2");
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private void bStop_Thread256_Click(object sender, EventArgs e)
        {
            if (Core.thread256 != null)
            {
                if ((Core.thread256.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0)
                {
                    try
                    {
                        Core.thread256.Abort();
                        lbEvent.Items.Add("256 Abort");
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            if (Core.thread256_right != null)
            {
                if ((Core.thread256_right.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) != 0)
                {
                    try
                    {
                        Core.thread256_right.Abort();
                        lbEvent.Items.Add("256 Abort2");
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        // Exponent
        public delegate void settbExp32TextDel(Object threadContext);
        public void settbExp32Text(String text)
        {
            if (tbExp32.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbExp32.Text = text;
            }
        }
     //   public delegate void settbExp64TextDel(Object threadContext);
        public void settbExp64Text(String text)
        {
            if (tbExp64.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbExp64.Text = text;
            }
        }
     //   public delegate void settbExp128TextDel(Object threadContext);
        public void settbExp128Text(String text)
        {
            if (tbExp128.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbExp128.Text = text;
            }
        }
     //   public delegate void settbExp256TextDel(Object threadContext);
        public void settbExp256Text(String text)
        {
            if (tbExp256.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbExp256.Text = text;
            }
        }

        public void settbExp64_2Text(String text)
        {
            if (tbExp64_2.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbExp64_2.Text = text;
            }
        }
        public void settbExp128_2Text(String text)
        {
            if (tbExp128_2.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbExp128_2.Text = text;
            }
        }
        public void settbExp256_2Text(String text)
        {
            if (tbExp256_2.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbExp256_2.Text = text;
            }
        }

        // Mantissa
        public void settbMan32Text(String text)
        {
            if (tbMantisa32.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbMantisa32.Text = text;
            }
        }
        public void settbMan64Text(String text)
        {
            if (tbMantisa64.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbMantisa64.Text = text;
            }
        }
        public void settbMan128Text(String text)
        {
            if (tbMantisa128.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbMantisa128.Text = text;
            }
        }
        public void settbMan256Text(String text)
        {
            if (tbMantisa256.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbMantisa256.Text = text;
            }
        }
        
        public void settbMan64_2Text(String text)
        {
            if (tbMantisa64_2.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbMantisa64_2.Text = text;
            }
        }
        public void settbMan128_2Text(String text)
        {
            if (tbMantisa128_2.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbMantisa128_2.Text = text;
            }
        }
        public void settbMan256_2Text(String text)
        {
            if (tbMantisa256_2.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbMantisa256_2.Text = text;
            }
        }

        // Sign
        public void settbS32Text(String text)
        {
            if (tbSign32.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbSign32.Text = text;
            }
        }
        public void settbS64Text(String text)
        {
            if (tbSign64.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbSign64.Text = text;
            }
        }
        public void settbS128Text(String text)
        {
            if (tbSign128.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbSign128.Text = text;
            }
        }
        public void settbS256Text(String text)
        {
            if (tbSign256.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbSign256.Text = text;
            }
        }

        public void settbS64_2Text(String text)
        {
            if (tbSign64_2.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbSign64_2.Text = text;
            }
        }
        public void settbS128_2Text(String text)
        {
            if (tbSign128_2.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbSign128_2.Text = text;
            }
        }
        public void settbS256_2Text(String text)
        {
            if (tbSign256_2.InvokeRequired)
            {
                settbExp32TextDel d = new settbExp32TextDel(FillForm);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbSign256_2.Text = text;
            }
        }


//#################################  DEBUG Func's Section #########################################

        public void timeOnForm(String text)
        {
            lbTime.Items.Add(System.DateTime.Now.ToString("hh:mm:ss:ms"));
            lbEvent.Items.Add(text);
        }
        private void tTime_Tick(object sender, EventArgs e)
        {
            lTime.Text = "Time: " + System.DateTime.Now.ToString("HH:mm:ss");
            
            //32
            dataGridView1.Rows[0].Cells[0].Value = Core.Num32.Name;
                dataGridView1.Rows[0].Cells[1].Value = Core.Num32.NumberState;
                dataGridView1.Rows[0].Cells[3].Value = Core.Num32.CorrectResult;
            //64
            dataGridView1.Rows[1].Cells[0].Value = Core.Num64.Name;
                // StateL 
                dataGridView1.Rows[1].Cells[1].Value = Core.Num64.NumberState;
                // StateR
                dataGridView1.Rows[1].Cells[2].Value = Core.Num64.NumberStateRight;
                // Res L
                dataGridView1.Rows[1].Cells[3].Value = Core.Num64.CorrectResult;
                // Res R
                dataGridView1.Rows[1].Cells[4].Value = Core.Num64.CorrectResultFractionL + "/" + Core.Num64.CorrectResultFractionR;

            // 128
            dataGridView1.Rows[2].Cells[0].Value = Core.Num128.Name;
                // StateL 
                dataGridView1.Rows[2].Cells[1].Value = Core.Num128.NumberState;
                // StateR
                dataGridView1.Rows[2].Cells[2].Value = Core.Num128.NumberStateRight;
                // Res L
                dataGridView1.Rows[2].Cells[3].Value = Core.Num128.CorrectResult;
                // Res R
                dataGridView1.Rows[2].Cells[4].Value = Core.Num128.CorrectResultFractionL + "/" + Core.Num256.CorrectResultFractionR;

            // 256
            dataGridView1.Rows[3].Cells[0].Value = Core.Num256.Name;
                // StateL 
                dataGridView1.Rows[3].Cells[1].Value = Core.Num256.NumberState;
                // StateR
                dataGridView1.Rows[3].Cells[2].Value = Core.Num256.NumberStateRight;
                // Res L
                dataGridView1.Rows[3].Cells[3].Value = Core.Num256.CorrectResult;
                // Res R
                dataGridView1.Rows[3].Cells[4].Value = Core.Num256.CorrectResultFractionL + "/" + Core.Num256.CorrectResultFractionR;



            //64
            dataGridView2.Rows[1].Cells[0].Value = Core.Num64.Name;
                // Exp R 
                dataGridView2.Rows[1].Cells[1].Value = Core.Num64.Exponenta;
                // Man L 
                dataGridView2.Rows[1].Cells[2].Value = Core.Num64.Mantisa;
                // Exp R
                dataGridView2.Rows[1].Cells[3].Value = Core.Num64.ExponentaRight;
                // Man R
                dataGridView2.Rows[1].Cells[4].Value = Core.Num64.MantisaRight;

            // 128
            dataGridView2.Rows[2].Cells[0].Value = Core.Num128.Name;
                // Exp L 
                dataGridView2.Rows[2].Cells[1].Value = Core.Num128.Exponenta;
                // Men L
                dataGridView2.Rows[2].Cells[2].Value = Core.Num128.Mantisa;
                // Exp R
                dataGridView2.Rows[2].Cells[3].Value = Core.Num128.ExponentaRight;
                // Man R
                dataGridView2.Rows[2].Cells[4].Value = Core.Num128.MantisaRight;

            // 256
            dataGridView2.Rows[3].Cells[0].Value = Core.Num256.Name;
                // Exp L 
                dataGridView2.Rows[3].Cells[1].Value = Core.Num256.Exponenta;
                // Man L
                dataGridView2.Rows[3].Cells[2].Value = Core.Num256.Mantisa;
                // Exp R
                dataGridView2.Rows[3].Cells[3].Value = Core.Num256.ExponentaRight;
                // Man R
                dataGridView2.Rows[3].Cells[4].Value = Core.Num256.MantisaRight;

            // Binary String
            dataGridView2.Rows[4].Cells[0].Value = "BinaryIntPart";
            dataGridView2.Rows[4].Cells[1].Value = Core.BinaryIntPart;

            dataGridView2.Rows[5].Cells[0].Value = "BinaryFloatPart";
            dataGridView2.Rows[5].Cells[1].Value = Core.BinaryFloatPart;

            dataGridView2.Rows[6].Cells[0].Value = "BinaryIntPartFILeft";
            dataGridView2.Rows[6].Cells[1].Value = Core.BinaryIntPartFILeft;

            dataGridView2.Rows[7].Cells[0].Value = "BinaryFloatPartFILeft";
            dataGridView2.Rows[7].Cells[1].Value = Core.BinaryFloatPartFILeft;

            dataGridView2.Rows[8].Cells[0].Value = "BinaryIntPartFIRight";
            dataGridView2.Rows[8].Cells[1].Value = Core.BinaryIntPartFIRight;

            dataGridView2.Rows[9].Cells[0].Value = "BinaryFloatPartFIRight";
            dataGridView2.Rows[9].Cells[1].Value = Core.BinaryFloatPartFIRight;
        }
    
    }//class Form1
}// namespace

//  число не число над мантисой BEGIN
// jLabelInfoDigit32.setText("число=" + my754.getSignCharacter() + my754.getState(32));
// jLabelInfoDigit64.setText("число=" + my754.getSignCharacter() + my754.getState(64));
// jLabelInfoDigit128.setText("число=" + my754.getSignCharacter() + my754.getState(128));
// b256_Info.setText("число=" + my754.getSignCharacter() + my754.getState(256));
//  число не число над мантисой END
/* Old func ver 2
 * public bool TestParts(int inputFormat)
{
    String datastr = "";
    int separatorIndex, i;
    String part1;
    String part2;
    int inAccuracy = (int)nUpDown.Value;

    if (tbInput.Multiline == true)
    {
        for (i = 0; i < tbInput.Lines.Length; i++)
            datastr += tbInput.Lines[i];

    }
    else
    {
        datastr = tbInput.Text;
    }


    if ((inputFormat == 1) || (inputFormat == 2))
    {
        if (my754.getInputStringFormat() == 1) // Float
        {
            separatorIndex = datastr.IndexOf('/');
            if (separatorIndex != -1)
            {
                part1 = datastr.Substring(0, separatorIndex);
                part2 = datastr.Substring(separatorIndex + 1, datastr.Length - (1 + separatorIndex));

            }
            else return false;
        }
        else  // Interval
        {
            separatorIndex = datastr.IndexOf(';');
            if (separatorIndex != -1)
            {
                part1 = datastr.Substring(0, separatorIndex);
                part2 = datastr.Substring(separatorIndex + 1, datastr.Length - (1 + separatorIndex));
            }
            else return false;
        }

        my754.setString(part1);
        my754.modifyMe(inAccuracy);
        part1 = my754.getString();

        my754.setString(part2);
        my754.modifyMe(inAccuracy);
        part2 = my754.getString();

        LeftPart = part1;
        RightPart = part2;
        return true;
    }
    else
        return false;
}
*/
/*Old Function ver 2
private void recalculate_Click(object sender, EventArgs e)
{
    String[] result = new String[4];
    String tIn, tOut, tStr, currSeparator, sign;
    int i, loop_counter, temp;
    loop_counter = inputStringFormat == 0 ? 1 : 2;
    currSeparator = inputStringFormat == 0 ? "" : inputStringFormat == 1 ? "/" : ";";
    fillSEM();
    my754.changeNumberState();
    // Translate 16cc to 2cc and recalc
    for (i = 0; i < loop_counter; i++)
    {
        switch (tabControl_Format.SelectedIndex)
        {
            case 0:
                result = my754.selectOut(tbSign32.Text, tbExp32.Text, tbMantisa32.Text);
                break;
            case 1:
                if (i == 0)
                    result = my754.selectOut(tbSign64.Text, tbExp64.Text, tbMantisa64.Text);
                else
                {
                    sign = inputStringFormat == 1 ? tbSign64.Text : tbSign64_2.Text;
                    result = my754.selectOut(sign, tbExp64_2.Text, tbMantisa64_2.Text);
                }
                break;
            case 2:
                if (i == 0)
                    result = my754.selectOut(tbSign128.Text, tbExp128.Text, tbMantisa128.Text);
                else
                {
                    sign = inputStringFormat == 1 ? tbSign128.Text : tbSign128_2.Text;
                    result = my754.selectOut(sign, tbExp128_2.Text, tbMantisa128_2.Text);
                }
                break;
            case 3:
                if (i == 0)
                    result = my754.selectOut(tbSign256.Text, tbExp256.Text, tbMantisa256.Text);
                else
                {
                    sign = inputStringFormat == 1 ? tbSign256.Text : tbSign256_2.Text;
                    result = my754.selectOut(sign, tbExp256_2.Text, tbMantisa256_2.Text);
                }
                break;
        }

        temp = 32 << tabControl_Format.SelectedIndex;
        if (i == 0)
        {
            OutPutVars["out" + temp.ToString() + "Dec"] = result[0];
            OutPutVars["out" + temp.ToString() + "Bin"] = result[1];
            OutPutVars["out" + temp.ToString() + "DecE"] = result[2];
            OutPutVars["out" + temp.ToString() + "BinE"] = result[3];
            // Calc Res & Error
        }
        else
        {
            OutPutVars["out" + temp.ToString() + "Dec_2"] = result[0];
            OutPutVars["out" + temp.ToString() + "Bin_2"] = result[1];
            OutPutVars["out" + temp.ToString() + "DecE_2"] = result[2];
            OutPutVars["out" + temp.ToString() + "BinE_2"] = result[3];
            // Calc Res+= & Error+=
        }

        // NEXT CALC's need to Erase BEGIN
        my754.inputString = result[0];
        tIn = my754.getInputString();
        tOut = result[0];
        if (tIn[0] == '+')
            tIn = tIn.Substring(1);
        if (tOut[0] == '+')
            tOut = tOut.Substring(1);

        tStr = stringUtil.convertToExp(calculator.sub(tIn, tOut));
        if (inputStringFormat == 0)
        {
            Results[0] = result[0]; //Results[1] = result[0];
            switch (tabControl_Format.SelectedIndex)
            {
                case 0: Errors[0] = tStr; break;
                case 1: Errors[1] = tStr; break;
                case 2: Errors[2] = tStr; break;
                case 3: Errors[3] = tStr; break;
            }

            // Errors[1] = tStr;
        }
        else
        {
            if (i == 0)
            {
                Results_2[1] = result[0] + " " + currSeparator + " ";
                Errors_2[1] = tStr + " " + currSeparator + " ";
            }
            else
            {
                Results_2[1] += result[0];
                Errors_2[1] += tStr;
            }
        }

        if (inputStringFormat == 0)
        {
            tbRes.Text = result[0];
            tbInput.Text = result[0];
            tbCalcError.Text = Errors[0];
        }
        else
            if (i == 0)
            {
                tbInput.Text = Out[0] + " " + currSeparator + " ";
                tbRes.Text = Out[0] + " " + currSeparator + " ";
                tbCalcError.Text = Errors_2[1];
            }
            else
            {
                tbInput.Text += Out[0];
                tbRes.Text += Out[0];
                tbCalcError.Text += tStr;
            }
        // ENG Erase Calculations 

    }// for

    refreshOutPutVars();
    calcResultsAndErrors(this, null);
    tbInput.Text = tbRes.Text;
}
*/
/* Old Function ver 1 
 * public void makeCalc()
{
    String datastr = "";
    String tempInput = "";
    String tempOutput = "";
    String tempString = "";
    String currSeparator="";
    String elapsedTime = "";
    String tt="";
    int k;
    //DateTime startTime = System.DateTime.Now; //System.currentTimeMillis();
    Stopwatch st = new Stopwatch();
    toolStripProgressBar1.Value = 0;
    tbInput.BackColor = Color.FromKnownColor(KnownColor.Window);
    errorTimer.Stop();
    st.Start();
    int inAccuracy = (int)nUpDown.Value;                
    stringMath.setAccurancy(nUpDown.Value);
    //richTextBox1.Text += "Начало расчета \r\n";
    stlStatus.Text = "Статус : Начало Работы";
    if (tbInput.Multiline == true)
    {
        for (k = 0; k < tbInput.Lines.Length; k++)
            datastr += tbInput.Lines[k];
    }
    else
    {
        datastr = tbInput.Text;
    }
    if (ckBClearLog.Checked){ // Очищать ли "Логи" при пересчете или нет
        richTextBox1.Clear();
    }
    isCalcsReset = false;
    if (datastr.Length == 0)
    {
        if (isTabIndexChanged == false)
            setErrorOnInput();
    }
    else
    {
        if ((tbInput.Text.IndexOf(",") == -1) && (tbInput.Text.IndexOf("e") == -1))
        {
            tbInput.Text += ",0";
        }
        //else
        //{ 
        //    if (tbInput.Text.IndexOf("e") != -1)
        //      tt =   tbInput.Text.Substring(0,tbInput.Text.IndexOf("e"))+".0"+
        //}

        tbInput.Text = my754.testExponentNumber(tbInput.Text, (int)nUpDown.Value);
        stlStatus.Text = "Статус : Работаю...";
        my754.setInputStringFormat(inputStringFormat); // inputStringFormat = число дробь интервал и тетракод
        checkInputStringForCurrentFormat();
                
        int cycleCount = my754.getInputStringFormat() == 0 ? 1 : 2;
        for (int i = 0; i < cycleCount; i++ )
        {
            tempInput = "";
            tempOutput = "";

            TestParts(inputStringFormat);

            if (inputStringFormat > 0){
                if (inputStringFormat == 1){
                    currSeparator = "/";
                }else{
                    currSeparator = ";";
                }
                my754.setString(LeftPart + currSeparator + RightPart);
                }else{
                    my754.setString(tbInput.Text);
                    my754.modifyMe(inAccuracy);
                }

            if (my754.RegxTest())
            {
                if ( (i == 0) && (inputStringFormat != 0) )
                {
                    my754.setString(LeftPart);
                    my754.modifyMe(inAccuracy);
                }
                if ((i == 1) && (inputStringFormat != 0))
                {
                    my754.setString(RightPart);
                    my754.modifyMe(inAccuracy);
                }
                        
                richTextBox1.Text += "Разбор числа на SEM\r\n";
                CalcInputString();

                if (cycleCount == 1) // Integer only Type for pb32
                {
                    richTextBox1.Text += "  -Формирование pb32\r\n";
                           
                   // tbCF32.Text = my754.getCF32();

                    // Обратное преобразование и определение погрешности: 
                    Out = my754.selectOut(my754.getSign(), my754.exp32, my754.M32);

                    tbSign32.Text = my754.getSign();
                    tbExp32.Text = my754.exp32;
                    tbMantisa32.Text = my754.M32;
                    tbMF32.Text = my754.MF32;

                    out32Dec = Out[0];
                    out32Bin = Out[1];
                    out32DecE = Out[2];
                    out32BinE = Out[3];
                    if (my754.exp32[0] != '-')
                    {
                        tempInput = my754.getInputString();
                        tempOutput = out32Dec;
                        if (tempInput[0] == '+')
                            tempInput = tempInput.Substring(1);
                        if (tempOutput[0] == '+')
                            tempOutput = tempOutput.Substring(1);

                        Results[0] = out32Dec;
                        if (tempInput[0] == '-')
                            tempInput = tempInput.Substring(1);
                        if (tempOutput[0] == '-')
                            tempOutput = tempOutput.Substring(1);
                        Errors[0] = stringUtil.convertToExp(calculator.sub(tempInput, tempOutput));
                        if (tabControl_Format.SelectedIndex == 0)
                        {
                            tbRes.Text = Results[0];
                            tbCalcError.Text = Errors[0];
                        }
                    }
                    else
                    {
                        Results[0] = Out[0];
                        Errors[0] = Out[0];
                        if (tabControl_Format.SelectedIndex == 0)
                        {
                            tbRes.Text = Results[0];
                            tbCalcError.Text = Errors[0];
                        }
                    }
                    richTextBox1.Text += "      Значения pb32       =[  " + Results[0] + "  ]\r\n";
                    richTextBox1.Text += "      Погрешность pb32=[  " + Errors[0] + "  ]\r\n";
                }

                if ((cycleCount == 2) && ( toolStripProgressBar1.Value != 100 ))
                {
                    toolStripProgressBar1.Value += 12;
                }
                else
                    toolStripProgressBar1.Value += 25;
                //_______/////////___64___///////////////////////
                richTextBox1.Text += "  -Формирование pb64, pb128, pb256 \r\n";
                tbSign64.Text =tbSign128.Text =tbSign256.Text = my754.getSign();

                        
                      

                // _______64
                Out = my754.selectOut(my754.getSign(), my754.exp64, my754.M64);
                if (i == 0){
                    out64Dec = Out[0];
                    out64Bin = Out[1];
                    out64DecE = Out[2];
                    out64BinE = Out[3];
                }else{
                    out64Dec_2 = my754.out64Dec_2 = Out[0];
                    out64Bin_2 = my754.out64Bin_2 = Out[1];
                    out64DecE_2 = my754.out64DecE_2 = Out[2];
                    out64BinE_2 = my754.out64BinE_2 = Out[3];
                }
                        
                if (my754.exp64[0] != '-')
                {
                    tempInput = my754.getInputString();
                    if (i==0)
                        tempOutput = out64Dec;
                    else
                        tempOutput = out64Dec_2;
                    if (tempInput[0] == '+')
                        tempInput = tempInput.Substring(1);
                    if (tempOutput[0] == '+')
                        tempOutput = tempOutput.Substring(1);

                    tempString = stringUtil.convertToExp(calculator.sub(tempInput, tempOutput));
                    if (inputStringFormat == 0){
                        Results[1] = out64Dec;
                        Errors[1] = tempString;
                        }else {
                            if (i == 0)
                            {
                                Results_2[1] = out64Dec + " " + currSeparator + " ";
                                Errors_2[1] = tempString + " " + currSeparator + " ";
                            }
                            else
                            {
                                Results_2[1] += out64Dec_2;
                                Errors_2[1] += tempString;
                            }
                    }
                            
                    if (tabControl_Format.SelectedIndex == 1){
                        if (inputStringFormat == 0)
                        {
                            tbRes.Text = out64Dec;
                            tbCalcError.Text = Errors[1];
                        }
                        else
                            if (i == 0)
                            {
                                tbRes.Text = out64Dec + " " + currSeparator + " ";
                                tbCalcError.Text = Errors_2[1];
                            }
                            else
                            {
                                tbRes.Text += out64Dec_2;
                                tbCalcError.Text += tempString;
                            }                                    
                    }
                }
                else
                {
                    if (inputStringFormat == 0)
                        Errors[1] = Results[1] = Out[0];
                    else
                        if (i == 0)
                        {
                            Results_2[1] = Out[0] + " " + currSeparator + " \n\r";
                            Errors_2[1] = Out[4] + " " + currSeparator + " \n\r";
                        }
                        else
                        {
                            Results_2[1] += Out[0];
                            Errors_2[1] += Out[4];
                        }

                    if (tabControl_Format.SelectedIndex == 1)
                    {
                        if (inputStringFormat == 0)
                        {
                            tbRes.Text = Results[1]; 
                            tbCalcError.Text = Errors[1];
                        }
                        else
                        {
                            if (i == 0)
                            {
                                tbRes.Text = Out[0] + " " + currSeparator + " \n\r";
                                tbCalcError.Text = Errors_2[1];
                            }
                            else 
                            {
                                tbRes.Text += Out[0];
                                tbCalcError.Text += Out[4];
                            }
                        }
                    }
                }
                      
                if (cycleCount == 2)
                {
                    toolStripProgressBar1.Value += 12;
                }
                else
                    toolStripProgressBar1.Value += 25;

                // _______128
                Out = my754.selectOut(my754.getSign(), my754.exp128, my754.M128);
                if (i == 0){
                    out128Dec = Out[0];
                    out128Bin = Out[1];
                    out128DecE = Out[2];
                    out128BinE = Out[3];
                }else {
                    out128Dec_2 = my754.out128Dec_2 = Out[0];
                    out128Bin_2 = my754.out128Bin_2 = Out[1];
                    out128DecE_2 = my754.out128DecE_2 = Out[2];
                    out128BinE_2 = my754.out128BinE_2 = Out[3];
                }
                        
                if (my754.exp128[0] != '-')
                {
                    tempInput = my754.getInputString();
                    if (i==0)
                        tempOutput = out128Dec;
                    else
                        tempOutput = out128Dec_2;
                    if (tempInput[0] == '+')
                        tempInput = tempInput.Substring(1);
                    if (tempOutput[0] == '+')
                        tempOutput = tempOutput.Substring(1);

                    tempString = stringUtil.convertToExp(calculator.sub(tempInput, tempOutput));
                    if (inputStringFormat == 0)
                    {
                        Results[2] = out128Dec;
                        Errors[2] = tempString;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            Results_2[2] = out128Dec + " " + currSeparator + " "; 
                            Errors_2[2] = tempString + " " + currSeparator + " ";
                        }
                        else
                        {
                            Results_2[2] += out128Dec_2; 
                            Errors_2[2] += tempString;
                        }
                    }

                    if (tabControl_Format.SelectedIndex == 2)
                    {
                        if (inputStringFormat == 0)
                        {
                            tbRes.Text = out128Dec;
                            tbCalcError.Text = Errors[2];
                        }
                        else
                        {
                            if (i == 0)
                            {
                                tbRes.Text = out128Dec + " " + currSeparator + " ";
                                tbCalcError.Text = Errors_2[2];
                            }
                            else 
                            {
                                tbRes.Text += out128Dec_2;
                                tbCalcError.Text += tempString;
                            }
                        }
                    }
                }
                else
                {
                    if (inputStringFormat == 0)
                        Errors[2] = Results[2] = Out[0];
                    else
                        if (i == 0)
                        {
                            Results_2[2] = Out[0] + " " + currSeparator + " ";
                            Errors_2[2] = Out[4] + " " + currSeparator + " ";
                        }
                        else 
                        {
                            Results_2[2] += Out[0];
                            Errors_2[2] += Out[4];
                        }
                            
                    if (tabControl_Format.SelectedIndex == 2)
                    {
                        if (inputStringFormat == 0)
                        {
                            tbRes.Text = Results[2];  
                            tbCalcError.Text = Errors[2];
                        }
                        else
                        {
                            if (i == 0)
                            {
                                tbRes.Text = Out[0] + " " + currSeparator + " ";
                                tbCalcError.Text = Errors_2[2];
                            }else { 
                                tbRes.Text += Out[0];
                                tbCalcError.Text += Out[4];
                            }
                        }
                    }
                }

                if (cycleCount == 2)
                {
                    toolStripProgressBar1.Value += 12;
                }
                else
                    toolStripProgressBar1.Value += 25;

                // _______256
                Out = my754.selectOut(my754.getSign(), my754.exp256, my754.M256);
                if (i == 0)
                {
                    out256Dec = Out[0];
                    out256Bin = Out[1];
                    out256DecE = Out[2];
                    out256BinE = Out[3];
                }else {
                    out256Dec_2 = my754.out256Dec_2 = Out[0];
                    out256Bin_2 = my754.out256Bin_2 = Out[1];
                    out256DecE_2 = my754.out256DecE_2 = Out[2];
                    out256BinE_2 = my754.out256BinE_2 = Out[3];
                }

                if (my754.exp256[0] != '-')
                {
                    tempInput = my754.getInputString();
                    if (i==0)
                        tempOutput = out256Dec;
                    else
                        tempOutput = out256Dec_2;
                    if (tempInput[0] == '+')
                        tempInput = tempInput.Substring(1);
                    if (tempOutput[0] == '+')
                        tempOutput = tempOutput.Substring(1);

                    tempString = stringUtil.convertToExp(calculator.sub(tempInput, tempOutput));
                    if (inputStringFormat == 0)
                    {
                        Results[3] = out256Dec;
                        Errors[3] = tempString;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            Results_2[3] = out256Dec + " " + currSeparator + " ";
                            Errors_2[3] = tempString + " " + currSeparator + " ";
                        }
                        else
                        {
                            Results_2[3] += out256Dec_2;
                            Errors_2[3] += tempString;
                        }
                    }

                           
                    if (tabControl_Format.SelectedIndex == 3)
                    {
                        if (inputStringFormat == 0)
                        {
                            tbRes.Text = out256Dec;
                            tbCalcError.Text = Errors[3];
                        }
                        else
                        {
                            if (i == 0)
                            {
                                tbRes.Text = out256Dec + " " + currSeparator + " ";
                                tbCalcError.Text = Errors_2[3];
                            }
                            else 
                            {
                                tbRes.Text += out256Dec ;
                                tbCalcError.Text += tempString;
                            }
                        }
                    }
                }
                else
                {
                    if (inputStringFormat == 0)
                        Errors[3] = Results[3] = Out[0];
                    else
                        if (i == 0)
                        {
                            Results_2[3] = Out[0] + " " + currSeparator + " ";
                            Errors_2[3] = Out[4] + " " + currSeparator + " ";
                        }
                        else
                        { 
                            Results_2[3] += Out[0];
                            Errors_2[3] += Out[4]; 
                        }

                    if (tabControl_Format.SelectedIndex == 3)
                    {
                        if (inputStringFormat == 0)
                        {
                            tbRes.Text = Results[3];
                            tbCalcError.Text = Errors[3];
                        }
                        else
                        {
                            if (i == 0)
                            {
                                tbRes.Text = Out[0] + " " + currSeparator + " ";
                                tbCalcError.Text = Errors_2[3];
                            }
                            else 
                            { 
                                 tbRes.Text += Out[0];
                                 tbCalcError.Text += Out[4];
                            }
                        }
                    }
                }


                if ((cycleCount == 2) && ( toolStripProgressBar1.Value != 100 ))
                {
                    toolStripProgressBar1.Value += 12;
                }
                else
                    toolStripProgressBar1.Value += 25;
            }
            else
            {
                setErrorOnInput();
                break;
            }

            if ((cycleCount == 2) && ( toolStripProgressBar1.Value != 100 ))
            {
                if (i == 0)
                    toolStripProgressBar1.Value = 50;
                if (i == 1)
                    toolStripProgressBar1.Value = 100;

            } else
                if ((i == 0) && ( toolStripProgressBar1.Value != 100 ))
                    toolStripProgressBar1.Value = 100;

            if (i == 0)
            {
                tbExp64.Text = my754.exp64_1 = my754.exp64;
                tbExp128.Text = my754.exp128_1 = my754.exp128;
                tbExp256.Text = my754.exp256_1 = my754.exp256;

                tbMantisa64.Text = my754.M64_1 = my754.M64;
                tbMantisa128.Text = my754.M128_1 = my754.M128;
                tbMantisa256.Text = my754.M256_1 = my754.M256;
            }
            else
            {
                tbExp64_2.Text = my754.exp64_2 = my754.exp64;
                tbExp128_2.Text = my754.exp128_2 = my754.exp128;
                tbExp256_2.Text = my754.exp256_2 = my754.exp256;

                tbMantisa64_2.Text = my754.M64_2 = my754.M64;
                tbMantisa128_2.Text = my754.M128_2 = my754.M128;
                tbMantisa256_2.Text = my754.M256_2 = my754.M256;
            }
        }// for (cycle)

        st.Stop();
        elapsedTime = "";
        elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            st.Elapsed.Hours, st.Elapsed.Minutes, st.Elapsed.Seconds,
            st.Elapsed.Milliseconds / 10);
                
        stlStatus.Text = "Статус : Готово.  ";
        stlTime.Text = " Время преоборазования : " + elapsedTime;

        if (cycleCount < 2)
        {
            richTextBox1.Text += "      Значения pb64       =[  " + Results[1] + "  ]\r\n";
            richTextBox1.Text += "      Погрешность pb64=[  " + Errors[1] + "  ]\r\n";

            richTextBox1.Text += "      Значения pb128       =[  " + Results[2] + "  ]\r\n";
            richTextBox1.Text += "      Погрешность pb128=[  " + Errors[2] + "  ]\r\n";

            richTextBox1.Text += "      Значения pb256       =[  " + Results[3] + "  ]\r\n";
            richTextBox1.Text += "      Погрешность pb256=[  " + Errors[3] + "  ]\r\n";
        }
        else
        {
            richTextBox1.Text += "      Значения pb64       =[  "+ Results_2[1] + "  ]\n";
            richTextBox1.Text += "      Погрешность pb64=[  " + Errors_2[1] + "  ]\n";

            richTextBox1.Text += "      Значения pb128       =[  "  + Results_2[2] + "  ]\r\n";
            richTextBox1.Text += "      Погрешность pb128=[  " + Errors_2[2] + "  ]\r\n";

            richTextBox1.Text += "      Значения pb256       =[  " + Results_2[3] + "  ]\r\n";
            richTextBox1.Text += "      Погрешность pb256=[  " + Errors_2[3] + "  ]\r\n";
        }
                
        switch (tabControl_Format.SelectedIndex)
        {
            case 0:
                lNormDenorm.Text = inStr[lang][9, (int)my754.state32];
                lNormDenorm.ForeColor = ColorsForState[(int)my754.state32];
                break;
            case 1: 
                lNormDenorm.Text = inStr[lang][9, (int)my754.state64];
                lNormDenorm.ForeColor = ColorsForState[(int)my754.state64];
                break;
            case 2:
                lNormDenorm.Text = inStr[lang][9, (int)my754.state128];
                lNormDenorm.ForeColor = ColorsForState[(int)my754.state128];
                break;
            case 3:
                lNormDenorm.Text = inStr[lang][9, (int)my754.state256];
                lNormDenorm.ForeColor = ColorsForState[(int)my754.state256];
                break;
        }
    }//if
    toolStripProgressBar1.Value = 0;
    if (l2ccTo16cc.Text == "16cc")
    {
        convert2to16();
    }
    calcResultsAndErrors(this,null);
}// func makeCalc()
*/

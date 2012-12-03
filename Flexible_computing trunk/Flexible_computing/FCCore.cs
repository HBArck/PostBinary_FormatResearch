using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

// Ввод значения
// Получения SEM
// Получени точного результата
// Получение погрешности
namespace Flexible_computing
{
    public class FCCoreGeneralException : System.Exception
    {
        public FCCoreGeneralException()
        {
        }
        public FCCoreGeneralException(string message)
            : base(message)
        {
        }
        public FCCoreGeneralException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class FCCoreArithmeticException : System.Exception
    {
        public FCCoreArithmeticException()
        {
        }
        public FCCoreArithmeticException(string message)
            : base(message)
        {
        }
        public FCCoreArithmeticException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class FCCoreFunctionException : System.Exception
    {
        public FCCoreFunctionException()
        {
        }
        public FCCoreFunctionException(string message)
            : base(message)
        {
        }
        public FCCoreFunctionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    
    
    public enum CalculationStatus
    {
        Ok,
        Exception,
        Error
    };
    public enum stateOfNumber
    {
        normalized,
        denormalized,
        zero,
        infinite,
        NaN,
        error
    };
    public enum PartOfNumber
    {
        Left = 1,
        Right = 2,
    };
    public class Number
    {
        public String Name;
        public String Sign;
        public String SignRight;
        public String SignCharacter;
        public String ExpSign;
        private String E;
        public String Exponenta
        {
            get { return E; }
            set
            {
                int currEBits = 0;
                switch (Format)
                {
                    case 0: currEBits = EBits; break;
                    case 1:
                    case 2: currEBits = EBitsFI; break;
                    case 3: break; // TetraCode
                    case 4: break; // TetraCodeFI

                }
                if (currEBits == value.Length)
                {
                    E = value;
                }
                else
                {
                    if (currEBits < value.Length)
                    {
                        E = value.Substring(value.Length - currEBits, currEBits);
                    }
                    else
                    {
                        int i = 0;
                        E = "";
                        for (i = 0; i < Math.Abs(currEBits - value.Length); i++)
                        {
                            E += "0";
                        }
                        E += value;
                    }

                }

            }
        }

        private String ER;
        public String ExponentaRight
        {
            get { return ER; }
            set
            {
                int currEBits = 0;
                switch (Format)
                {
                    case 0: currEBits = EBits; break;
                    case 1:
                    case 2: currEBits = EBitsFI; break;
                    case 3: break; // TetraCode
                    case 4: break; // TetraCodeFI

                }
                if (currEBits == value.Length)
                {
                    ER = value;
                }
                else
                {
                    if (currEBits < value.Length)
                    {
                        ER = value.Substring(0, currEBits);
                    }
                    else
                    {
                        int i = 0;
                        ER = "";
                        for (i = 0; i < Math.Abs(currEBits - value.Length); i++)
                        {
                            ER += "0";
                        }
                        ER += value;
                    }
                }
            }
        }

        private String M;
        public String Mantisa
        {
            get { return M; }
            set
            {
                int currMBits = 0;
                switch (Format)
                {
                    case 0: currMBits = MBits; break;
                    case 1:
                    case 2: currMBits = MBitsFI; break;
                    case 3: break; // TetraCode
                    case 4: break; // TetraCodeFI

                }
                if (currMBits == value.Length + 1)
                {
                    M = value;
                }
                else
                {
                    if (currMBits < value.Length + 1)
                    {
                        M = value.Substring(value.Length - currMBits, currMBits);
                    }
                    else
                    {
                        int i = 0;
                        M = "";
                        for (i = 0; i < Math.Abs(currMBits - value.Length + 1); i++)
                        {
                            M += "0";
                        }
                        M += value;
                    }
                }
            }
        } // EXCEPTION NEEDED !!!!!     <<<<<<<<<<<<<<----------------

        private String MR;
        public String MantisaRight
        {
            get { return MR; }
            set
            {
                int currMBits = 0;
                switch (Format)
                {
                    case 0: currMBits = MBits; break;
                    case 1:
                    case 2: currMBits = MBitsFI; break;
                    case 3: break; // TetraCode
                    case 4: break; // TetraCodeFI

                }
                if (currMBits == value.Length + 1)
                {
                    MR = value;
                }
                else
                {
                    if (currMBits < value.Length + 1)
                    {
                        MR = value.Substring(0, currMBits);
                    }
                    else
                    {
                        int i = 0;
                        MR = "";
                        for (i = 0; i < Math.Abs(currMBits - value.Length + 1); i++)
                        {
                            MR += "0";
                        }
                        MR += value;
                    }
                }
            }
        } // EXCEPTION NEEDED !!!!!     <<<<<<<<<<<<<<----------------
       
        public String MF;
        public String CF;

        private String NormMIN;
        private String DenormMIN;
        private String NormMAX;
        private String DenormMAX;

        private int EB;
        public int EBits
        {
            get { return EB; }
            set { if ((value < int.MaxValue) && (value > int.MinValue)) EB = value; }
        }
        private int EBfi;
        public int EBitsFI // Number of Bits in  Float or Interval Format
        {
            get { return EBfi; }
            set { if ((value < int.MaxValue) && (value > int.MinValue)) EBfi = value; }
        }
        private int MB;
        public int MBits 
        {
            get { return MB; }
            set { if ((value < int.MaxValue)&&(value>int.MinValue)) MB = value; }
        }
        private int MBfi;
        public int MBitsFI
        {
            get { return MBfi; }
            set { if ((value < int.MaxValue) && (value > int.MinValue)) MBfi = value; }
        }
        private int MFBits;
        private int CFBits;

        public String Normalized;
        public String Denormalized;
        public String DenormalizedRight;
        public String IntPartDenormalized;
        public String FloatPartDenormalized;

        public String IntPartDenormalizedFI;
        public String FloatPartDenormalizedFI;

        public int Offset;
        public int OffsetFI;
        public int OffsetTetra;
        public int OffsetFITetra;

        private String BIP;
        public String BinaryIntPart
        {
            get { return BIP; }
            set
            {
                //if (Offset == value.Length)
                //    BIP = value;
                //else
                //{
                //    if (Offset > value.Length)
                //    {
                //        int i = 0;
                //        BIP = "";
                //        for (i = 0; i < Offset - value.Length; i++)
                //        {
                //            BIP += "0";
                //        }
                //        BIP = value;
                //    }
                //    else
                //        BIP = value.Substring(value.Length - Offset, Offset);
                //}
                BIP = value;
            }
        } // EXCEPTION NEEDED !!!!!     <<<<<<<<<<<<<<----------------

        private String BFP;
        public String BinaryFloatPart
        {
            get { return BFP; }
            set
            {
                //if (Offset + MBits == value.Length)
                //    BIP = value;
                //else
                //{
                //    if (Offset + MBits > value.Length)
                //    {
                //        int i = 0;
                //        BFP = value;
                //        for (i = 0; i < Offset + MBits - value.Length; i++)
                //        {
                //            BFP += "0";
                //        }

                //    }
                //    else
                //        BFP = value.Substring(0, Offset + MBits);
                //}
                BFP = value;
            }
        } // EXCEPTION NEEDED !!!!!     <<<<<<<<<<<<<<----------------

        private String BIPfiL;
        public String BinaryIntPartFILeft
        {
            get { return BIPfiL; }
            set
            {
                //if (Offset == value.Length)
                //    BIP = value;
                //else
                //{
                //    if (Offset > value.Length)
                //    {
                //        int i = 0;
                //        BIP = "";
                //        for (i = 0; i < Offset - value.Length; i++)
                //        {
                //            BIP += "0";
                //        }
                //        BIP = value;
                //    }
                //    else
                //        BIP = value.Substring(value.Length - Offset, Offset);
                //}
                BIPfiL = value;
            }
        } // EXCEPTION NEEDED !!!!!     <<<<<<<<<<<<<<----------------

        private String BFPfiL;
        public String BinaryFloatPartFILeft
        {
            get { return BFPfiL; }
            set
            {
                //if (Offset + MBits == value.Length)
                //    BIP = value;
                //else
                //{
                //    if (Offset + MBits > value.Length)
                //    {
                //        int i = 0;
                //        BFP = value;
                //        for (i = 0; i < Offset + MBits - value.Length; i++)
                //        {
                //            BFP += "0";
                //        }

                //    }
                //    else
                //        BFP = value.Substring(0, Offset + MBits);
                //}
                BFPfiL = value;
            }
        } // EXCEPTION NEEDED !!!!!     <<<<<<<<<<<<<<----------------

        private String BIPfiR;
        public String BinaryIntPartFIRight
        {
            get { return BIPfiR; }
            set
            {
                //if (Offset == value.Length)
                //    BIP = value;
                //else
                //{
                //    if (Offset > value.Length)
                //    {
                //        int i = 0;
                //        BIP = "";
                //        for (i = 0; i < Offset - value.Length; i++)
                //        {
                //            BIP += "0";
                //        }
                //        BIP = value;
                //    }
                //    else
                //        BIP = value.Substring(value.Length - Offset, Offset);
                //}
                BIPfiR = value;
            }
        } // EXCEPTION NEEDED !!!!!     <<<<<<<<<<<<<<----------------

        private String BFPfiR;
        public String BinaryFloatPartFIRight
        {
            get { return BFPfiR; }
            set
            {
                //if (Offset + MBits == value.Length)
                //    BIP = value;
                //else
                //{
                //    if (Offset + MBits > value.Length)
                //    {
                //        int i = 0;
                //        BFP = value;
                //        for (i = 0; i < Offset + MBits - value.Length; i++)
                //        {
                //            BFP += "0";
                //        }

                //    }
                //    else
                //        BFP = value.Substring(0, Offset + MBits);
                //}
                BFPfiR = value;
            }
        } // EXCEPTION NEEDED !!!!!     <<<<<<<<<<<<<<----------------

        public CalculationStatus CalcStatus;
        public stateOfNumber NumberState;
        public stateOfNumber NumberStateRight;

        public String CorrectResult;
        public String CorrectResultExp;

        public String CorrectResult2cc;
        public String CorrectResult2ccExp;

        public String CorrectResultFractionL;
        public String CorrectResultFractionR;
        public String CorrectResultFractionExpL;
        public String CorrectResultFractionExpR;

        public String CorrectResultFraction2ccL;
        public String CorrectResultFraction2ccR;
        public String CorrectResultFraction2ccExpL;
        public String CorrectResultFraction2ccExpR;

        public String CorrectResultIntervalL;
        public String CorrectResultIntervalR;
        public String CorrectResultIntervalExpL;
        public String CorrectResultIntervalExpR;

        public String CorrectResultInterval2ccL;
        public String CorrectResultInterval2ccR;
        public String CorrectResultInterval2ccExpL;
        public String CorrectResultInterval2ccExpR;


        public String Error;
        public String ErrorFractionLeft;
        public String ErrorFractionRight;
        public String ErrorIntervalLeft;
        public String ErrorIntervalRight;

        public byte Format; // Integer , Fraction , Interval

        public Number(String Name) { this.Name = Name; }
        public Number(String inCF, int inEBits, int inEBitsFI, int inMBits, int inMBitsFI, int inMFBits, int inCFBits, int inOffset, int inOffsetFI, String inName)
        {
            Name = inName;

            CF = inCF;

            EBits = inEBits;
            EBitsFI = inEBitsFI;
            MBits = inMBits;
            MBitsFI = inMBitsFI;
            MFBits = inMFBits;
            CFBits = inCFBits;

            Sign = "";
            SignCharacter = "";
            ExpSign = "";
            E = "";
            ER = "";
            Exponenta = "";
            ExponentaRight = "";
            Mantisa = "";
            MantisaRight = "";
            MF = "";

            NormMIN = NormMAX = "";
            DenormMIN = DenormMAX = "";
            Normalized = Denormalized = IntPartDenormalized = FloatPartDenormalized = "";

            BinaryFloatPart = "";
            BinaryIntPart = "";
            CalcStatus = CalculationStatus.Ok;
            NumberState = stateOfNumber.zero;
            Offset = inOffset;
            OffsetFI = inOffsetFI;
        }
        public void SetNormMinMax(String MIN, String MAX)
        {
            this.NormMIN = MIN;
            this.NormMAX = MAX;
        }
        public void SetDenormMinMax(String MIN, String MAX)
        {
            this.DenormMIN = MIN;
            this.DenormMAX = MAX;
        }
    }// Number Class

    public class FCCore : UserControl, ISynchronizeInvoke, IBindableComponent, IComponent, IDisposable //, IWin32Window
    {
        //public fields - out put
        //public enum Num32Number { D0 =  5,605193857299268283694918333159664525121047767506063087028273135559164330743442405946552753448486328125 , D1 =  1,1754937903029017780419081677304123618522130446673107709024430034511037035382940985073219053447246551513671875 , N0 =  1,1754943508222875079687365372222456778186655567720875215087517062784172594547271728515625 , N1 =  3,40282285791300048856692911642763067392 , Inf =  3,40282366920938463463374607431768211456 , Nn0 =  3,40282529180215292676737999009778499584 , Nn1 =  6.80564571582600097713385823285526134784  };
        public enum diapNames{ D0=0, D1=1, N0=2,N1=3, Inf=4, Nn0=5, Nn1=6};
        public int[] N32Exp = {-45 ,-38, -38 ,+38 ,+38 , +38 ,+38 };        
        public int[] N64Exp = {-323,-308,-308,+308 , +308 , +308 , +308};
        public int[] N128Exp = { -4963,-4932 ,   -4932 ,  +4932 ,  +4932 ,   +4932 ,  +4932  };
        public int[] N256Exp = { -157892  ,  -157826  ,  -157826  ,  +157826  ,  +157826  ,  +157826  ,  +157826   };

        String inputString;
        String inputStringL;
        String inputStringR;
        public String NormalizedNumber;
        public String NormalizedNumberRight;
        public String DenormalizedNumber;
        public String DenormalizedNumberLeft;
        public String DenormalizedNumberRight;
        public String IntPartDenormalized;
        public String FloatPartDenormalized;

        public String IntPartDenormalizedFILeft;
        public String FloatPartDenormalizedFILeft;
        public String IntPartDenormalizedFIRight;
        public String FloatPartDenormalizedFIRight;
        public String DenormIntPart;
        public String DenormFloatPart;
        public String DenormIntPartFI;
        public String DenormFloatPartFI;
        public String BinaryIntPart;
        public String BinaryFloatPart;
        public String BinaryIntPartFILeft;
        public String BinaryFloatPartFILeft;
        public String BinaryIntPartFIRight;
        public String BinaryFloatPartFIRight;

        public String ExpSign;
        private String SignLeft;
        public String SignCharacterLeft;
        private String SignRight;
        public String SignCharacterRight;
        public String E;
        public String iPart;  // Выходной перенос для сложения
        public int Accurancy; // This value should : 1) Readed from init.xml 2) Check correctness -> less then 4 billion (int)
        private byte Round;
        public byte Rounding // 0 - to zero 1 - to number 2 - to Pos Inf 3 - to Neg Inf 4 - to Pos Neg Inf
        {
            get { return Round; }
            set
            {
                if (value > 4 || value < 0)
                    Round = 0;
                else
                    Round = value;
            }
        }
        private byte Format;
        
        public byte NumberFormat
        {
            get { return Format; }
            set
            {
                switch (value)
                {
                    case 0:
                    case 1:
                    case 2:
                        Num32.Format = Num64.Format = Num128.Format = Num256.Format = Format = value;
                    break;

                    default:
                        Num32.Format = Num64.Format = Num128.Format = Num256.Format = Format = 0; break;
                }
            }
        }
        bool RightPartCalculating = false;
        public Number Num32, Num64, Num128, Num256;
        private Number[] nums;
        public Number[] Numbers
        {
            get { return nums; }
            set
            {
                if (value != null)
                {
                    nums = value;
                }
            }
        }
        //String[] Denorm = { "5,60519386e-45", "1,17549379e-38" ,        // 32   MIN MAX
        //                    "7,90505033e-323","2,22507386e-308",        // 64
        //                    "1,65764483e-4963","3,36210314e-4932",      // 128
        //                    "1,8286336e-157892","1,54061213e-157826"};  // 256

        //String[] Norm = {   "1,17549379e-38", "3,40282286e-38" ,        // 32
        //                    "2,22507386e-308","1,79769313e-308",        // 64
        //                    "3,36210314e-4932","1,18973150e-4932",      // 128
        //                    "1,54061213e-157826","2,5963757e-157826"};  // 256
        //String dataString; // Need for: 'NormolizeNumber'
        public enum CalculationStatus
        {
            Ok,
            Exception,
            Error
        };
        
        CalculationStatus calcStatus = CalculationStatus.Ok;
        //private fields - lux maintance
        ExceptionUtil exceptionUtil;
        System.Windows.Forms.ProgressBar progressBar;
        public Thread thread32, thread64, thread128, thread256;
        public Thread thread64_right, thread128_right, thread256_right;
        

        progIncThreadHeandler progIncThreadSafe;

        public FCCore(byte Rounding, ExceptionUtil exc_util,System.Windows.Forms.ProgressBar progress,progIncThreadHeandler ThreadSaveIncrementProgressBar)
        { //                     sCF    nE nEFI  nM  nMFI nMF nCF    off  offFI   name
            Num32 = new Number("0", 8, 0, 21, 0, 1, 1, 127, 0, "Num32");
            Num64 = new Number("01", 11, 8, 48, 21, 2, 2, 1023, 127, "Num64");
            Num128 = new Number("011", 15, 11, 104, 48, 5, 3, 16383, 1023, "Num128");
            Num256 = new Number("0111", 20, 15, 219, 104, 12, 4, 524287, 16383, "Num256");
            Numbers = new Number[4] { Num32 , Num64 , Num128 , Num256 };

            thread32 = new Thread(Calculate32);
            thread64 = new Thread(new ParameterizedThreadStart(Calculate64));
            thread128 = new Thread(new ParameterizedThreadStart(Calculate128));
            thread256 = new Thread(new ParameterizedThreadStart(Calculate256));
            thread64_right = new Thread(new ParameterizedThreadStart(Calculate64));
            thread128_right = new Thread(new ParameterizedThreadStart(Calculate128));
            thread256_right = new Thread(new ParameterizedThreadStart(Calculate256));

            thread32.Name = "thread32";
            thread64.Name = "thread64";
            thread128.Name = "thread128"; 
            thread256.Name = "thread256";
            thread64_right.Name = "thread64_right";
            thread128_right.Name = "thread128_right";
            thread256_right.Name = "thread256_right"; 
                   
            exceptionUtil = exc_util;
            progressBar = progress;
            progIncThreadSafe = ThreadSaveIncrementProgressBar;
            this.Rounding = Rounding;
            this.NumberFormat = 0;            
        }

        public void SetNumberWithCurr(String InputNumber, int input)
        {
            inputString = InputNumber;

            Num32.CalcStatus = Num64.CalcStatus = Num128.CalcStatus = Num256.CalcStatus = Flexible_computing.CalculationStatus.Ok;
            Num32.NumberState = Num64.NumberState = Num128.NumberState = Num256.NumberState = stateOfNumber.zero;

            if (NumberFormat == 0)
            {
                NormalizedNumber = NormalizeNumber(inputString, 1000, PartOfNumber.Left);
                // Denormalize Number
                Num32.Denormalized = Num64.Denormalized = Num128.Denormalized = Num256.Denormalized = DenormalizeNumber(NormalizedNumber, PartOfNumber.Left);
                // Convert from 10cc to 2cc

                this.BinaryIntPart = convert10to2IPart(Num32.IntPartDenormalized);
                this.BinaryFloatPart = convert10to2FPart(Num32.FloatPartDenormalized);
                FillBinaryVars(PartOfNumber.Left);
                // Fill SEM 
                switch (input)
                {
                    case 32:
                        Num32.Exponenta = selectExp(Num32, PartOfNumber.Left);
                        Num32.Mantisa = selectMantissa(Num32, NumberFormat, PartOfNumber.Left);
                        changeNumberState(32, PartOfNumber.Left);
                        calcRes(Num32, PartOfNumber.Left);
                        calcError(Num32, PartOfNumber.Left);
                        break;

                    case 64:
                        Num64.Exponenta = selectExp(Num64, PartOfNumber.Left);
                        Num64.Mantisa = selectMantissa(Num64, NumberFormat, PartOfNumber.Left);
                        changeNumberState(64, PartOfNumber.Left);
                        calcRes(Num64, PartOfNumber.Left);
                        calcError(Num64, PartOfNumber.Left);
                        break;

                    case 128:
                        Num128.Exponenta = selectExp(Num128, PartOfNumber.Left);
                        Num128.Mantisa = selectMantissa(Num128, NumberFormat, PartOfNumber.Left);
                        changeNumberState(128, PartOfNumber.Left);
                        calcRes(Num128, PartOfNumber.Left);
                        calcError(Num128, PartOfNumber.Left);
                        break;

                    case 256:
                        Num256.Exponenta = selectExp(Num256, PartOfNumber.Left);
                        Num256.Mantisa = selectMantissa(Num256, NumberFormat, PartOfNumber.Left);
                        changeNumberState(256, PartOfNumber.Left);
                        calcRes(Num256, PartOfNumber.Left);
                        calcError(Num256, PartOfNumber.Left);
                        break;
                }
            }

        }

        /// <summary>
        /// Function Calculates correct 2cc value based on arg = 'InputNumber' and sets calculation result
        /// Uses funcs : Normolize
        /// Uses attrib: Accurancy,
        /// </summary>
        /// <param name="InputNumber">New Number to Calculate</param>
        public void SetNumber(String InputNumber)
        {
            bool repeatConvertion = false;
            String tempNumber="";
            inputString = InputNumber;
            try
            {
                RightPartCalculating = false;
                //progressBar.Value = 0;
                Num32.CalcStatus = Num64.CalcStatus = Num128.CalcStatus = Num256.CalcStatus = Flexible_computing.CalculationStatus.Ok;
                Num32.NumberState = Num64.NumberState = Num128.NumberState = Num256.NumberState = stateOfNumber.zero;
                Num32.NumberStateRight = Num64.NumberStateRight = Num128.NumberStateRight = Num256.NumberStateRight = stateOfNumber.zero;

                thread32 = new Thread(Calculate32);
                thread64 = new Thread(Calculate64);
                thread128 = new Thread(Calculate128);
                thread256 = new Thread(Calculate256);

                
                // Check Number State
                // Normalize Number
                if (NumberFormat == 0)
                {
                    NormalizedNumber = NormalizeNumber(inputString, 2000, PartOfNumber.Left);
                    // Denormalize Number
                    Num32.Denormalized = Num64.Denormalized = Num128.Denormalized = Num256.Denormalized = DenormalizedNumber = DenormalizeNumber(NormalizedNumber, PartOfNumber.Left);
                    // Convert from 10cc to 2cc
                    
                        // test State HERE
                        tempNumber = convertToExp(DenormalizedNumber);
                        defineNumberState(tempNumber, PartOfNumber.Left);
                        if (Num256.NumberState != stateOfNumber.error)
                        {
                            this.BinaryIntPart = convert10to2IPart(IntPartDenormalized);
                            do
                            {
                                this.BinaryFloatPart = convert10to2FPart(FloatPartDenormalized);
                                if (isStringZero(BinaryFloatPart) && isStringZero(BinaryIntPart))
                                {
                                    AdditionalAccurancy += 500;
                                    repeatConvertion = true;
                                }
                                else
                                {
                                    repeatConvertion = false;
                                    AdditionalAccurancy = 0;
                                }
                            } while (repeatConvertion);

                            progIncThreadSafe();
                            FillBinaryVars(PartOfNumber.Left);
                        }
                        else
                        {
                            this.BinaryIntPart = "0";
                            this.BinaryFloatPart = "0";
                            FillBinaryVars(PartOfNumber.Left);
                            progIncThreadSafe();
                        }
                        // Fill SEM 
                        if (Num32.NumberState != stateOfNumber.error)
                        {
                            //#if (!DEBUG)
                            //    Calculate32();
                            //#else
                                thread32.IsBackground = true;
                                thread32.Start();
                           // #endif
                        }
                        else { progIncThreadSafe(); progIncThreadSafe(); progIncThreadSafe(); }
                        if (Num64.NumberState != stateOfNumber.error)
                        {
                            //#if (!DEBUG)
                            //    Calculate64((Object)false);
                            //#else
                                thread64.IsBackground = true;
                                thread64.Start(false);
                            //#endif
                        }
                        else { progIncThreadSafe(); progIncThreadSafe(); progIncThreadSafe(); }
                        if (Num128.NumberState != stateOfNumber.error)
                        {
                            //#if (!DEBUG)
                            //    Calculate128((Object)false);
                            //#else
                            thread128.IsBackground = true;
                                thread128.Start(false);
                            //#endif
                        }
                        else { progIncThreadSafe(); progIncThreadSafe(); progIncThreadSafe(); }
                        if (Num256.NumberState != stateOfNumber.error)
                        {
                            //#if (!DEBUG)
                            //    Calculate256((Object)false);
                            //#else
                            thread256.IsBackground = true;
                                thread256.Start(false);
                            //#endif
                        }
                        else { progIncThreadSafe(); progIncThreadSafe(); progIncThreadSafe(); }
                }
                else
                {
                    thread64_right = new Thread(Calculate64);
                    thread128_right = new Thread(Calculate128);
                    thread256_right = new Thread(Calculate256);

                    /*>>>>>>>>>>>>>>>>>>>>> LEFT PART BEGIN <<<<<<<<<<<<<<<<<<<<<<<*/
                    //RightPartCalculating = false;
                    if (NumberFormat == 1)
                    {
                        inputStringR = inputString.Substring(inputString.IndexOf("/") + 1);
                        inputString = inputString.Substring(0, inputString.IndexOf("/"));
                    }
                    else
                    {
                        inputStringR = inputString.Substring(inputString.IndexOf(";") + 1);
                        inputString = inputString.Substring(0, inputString.IndexOf(";"));
                    }

                    NormalizedNumber = NormalizeNumber(inputString, 2000, PartOfNumber.Left);
                    // Denormalize Number
                    Num64.Denormalized = Num128.Denormalized = Num256.Denormalized = DenormalizedNumberLeft = DenormalizeNumber(NormalizedNumber, PartOfNumber.Left);
                    // Convert from 10cc to 2cc
                   
                        tempNumber = convertToExp(DenormalizedNumberLeft);
                        defineNumberState(tempNumber, PartOfNumber.Left);
                        if (Num256.NumberState != stateOfNumber.error)
                        {
                            this.BinaryIntPartFILeft= convert10to2IPart(IntPartDenormalizedFILeft);
                            do{
                                this.BinaryFloatPartFILeft = convert10to2FPart(FloatPartDenormalizedFILeft);
                                if (isStringZero(BinaryFloatPartFILeft) && isStringZero(BinaryIntPartFILeft))
                                {
                                    AdditionalAccurancy += 500;
                                    repeatConvertion = true;
                                }
                                else
                                {
                                    repeatConvertion = false;
                                    AdditionalAccurancy = 0;
                                }
                            }
                            while(repeatConvertion);
                            
                            FillBinaryVars(PartOfNumber.Left);
                            // Fill SEM 
                        }
                        else
                        {
                            this.BinaryIntPart = "0";
                            this.BinaryFloatPart = "0";
                            FillBinaryVars(PartOfNumber.Left);
                        }
                        if (Num64.NumberState != stateOfNumber.error)
                        { 
                            //#if (!DEBUG)
                            //    Calculate64((Object)false);
                            //#else
                                thread64.IsBackground = true;
                                thread64.Start(false);
                            //#endif
                        }
                        if (Num128.NumberState != stateOfNumber.error)
                        {   
                            //#if (!DEBUG)
                            //    Calculate128((Object)false);
                            //#else
                                thread128.IsBackground = true;
                                thread128.Start(false);
                            //#endif
                        }
                        if (Num256.NumberState != stateOfNumber.error)
                        {   
                            //#if (!DEBUG)
                            //    Calculate256((Object)false);
                            //#else
                            thread256.IsBackground = true;
                            thread256.Start(false);
                            //#endif
                        }
                       // changeNumberState(false,false);
                   
                    /*>>>>>>>>>>>>>>>>>>>>> LEFT PART END <<<<<<<<<<<<<<<<<<<<<<<*/

                    /*>>>>>>>>>>>>>>>>>>>>> RIGTH PART BEGIN <<<<<<<<<<<<<<<<<<<<<<<*/
                        
                    //RightPartCalculating = true;
                    NormalizedNumberRight = NormalizeNumber(inputStringR, 2000, PartOfNumber.Right);
                    // Denormalize Number
                    Num64.DenormalizedRight = Num128.DenormalizedRight = Num256.DenormalizedRight = DenormalizedNumberRight = DenormalizeNumber(NormalizedNumberRight, PartOfNumber.Right);
                    // Convert from 10cc to 2cc
                    
                        tempNumber = convertToExp(DenormalizedNumberRight);
                        defineNumberState(tempNumber, PartOfNumber.Right);
                        if (Num256.NumberStateRight != stateOfNumber.error)
                        {
                            this.BinaryIntPartFIRight = convert10to2IPart(IntPartDenormalizedFIRight);
                            do
                            {
                                this.BinaryFloatPartFIRight = convert10to2FPart(FloatPartDenormalizedFIRight);
                                if (isStringZero(BinaryFloatPartFIRight) && isStringZero(BinaryIntPartFIRight))
                                {
                                    AdditionalAccurancy += 500;
                                    repeatConvertion = true;
                                }
                                else
                                {
                                    repeatConvertion = false;
                                    AdditionalAccurancy = 0;
                                }
                            }
                            while (repeatConvertion);
                            FillBinaryVars(PartOfNumber.Right);
                            // Fill SEM 
                        }
                        else
                        {
                            this.BinaryIntPartFIRight = "0";
                            this.BinaryFloatPartFIRight = "0";
                            FillBinaryVars(PartOfNumber.Right);
                        }

                        if (Num64.NumberStateRight != stateOfNumber.error)
                        {
                            //#if (DEBUG)
                            //    Calculate64((Object)true);
                            //#else
                                thread64_right.IsBackground = true;
                                thread64_right.Start(true); 
                            //#endif
                            
                        }
                        if (Num128.NumberStateRight != stateOfNumber.error)
                        {
                            
                            //#if (DEBUG)
                            //    Calculate128((Object)true);
                            //#else
                                thread128_right.IsBackground = true;
                                thread128_right.Start(true);
                            //#endif
                        }
                        if (Num256.NumberStateRight != stateOfNumber.error)
                        {
                            //#if (DEBUG)
                            //    Calculate256((Object)true);                        
                            //#else
                                thread256_right.IsBackground = true;
                                thread256_right.Start(true);
                            //#endif

                        }
                        /*>>>>>>>>>>>>>>>>>>>>> RIGTH PART END <<<<<<<<<<<<<<<<<<<<<<<*/
                }
            }
            catch (Exception ex)
            {
                throw new FCCoreFunctionException("Func 'SetNumber(String InputNumber)' =[ " + ex.Message + " ]");
            }
        }
       
        /// <summary>
        /// Function Calculates correct 2cc value based on arg = 'InputNumber' and sets calculation result
        /// Uses funcs : Normolize
        /// Uses attrib: Accurancy,
        /// </summary>
        /// <param name="InputNumber">New Number to Calculate</param>
        public void SetNumber(Object threadContext)
        {
            String InputNumber = (String)threadContext;
            String tempNumber = "";
            inputString = InputNumber;
            try
            {
                Num32.CalcStatus = Num64.CalcStatus = Num128.CalcStatus = Num256.CalcStatus = Flexible_computing.CalculationStatus.Ok;
                Num32.NumberState = Num64.NumberState = Num128.NumberState = Num256.NumberState = stateOfNumber.zero;
                Num32.NumberStateRight = Num64.NumberStateRight = Num128.NumberStateRight = Num256.NumberStateRight = stateOfNumber.zero;
                thread32 = new Thread(Calculate32);
                thread64 = new Thread(Calculate64);
                thread128 = new Thread(Calculate128);
                thread256 = new Thread(Calculate256);

                // Check Number State
                // Normalize Number
                if (NumberFormat == 0)
                {
                    NormalizedNumber = NormalizeNumber(inputString, 2000, PartOfNumber.Left);
                    // Denormalize Number
                    Num32.Denormalized = Num64.Denormalized = Num128.Denormalized = Num256.Denormalized = DenormalizeNumber(NormalizedNumber, PartOfNumber.Left);
                    // Convert from 10cc to 2cc
                   
                        tempNumber = convertToExp(DenormalizedNumber);
                        defineNumberState(tempNumber, PartOfNumber.Left);
                        if (Num256.NumberState != stateOfNumber.error)
                        {
                            this.BinaryIntPart = convert10to2IPart(IntPartDenormalized);
                            this.BinaryFloatPart = convert10to2FPart(FloatPartDenormalized);

                            FillBinaryVars(PartOfNumber.Left);
                        }
                        else
                        {
                            this.BinaryIntPart = "0";
                            this.BinaryFloatPart = "0";
                            FillBinaryVars(PartOfNumber.Left);
                        }
                        // Fill SEM 
                        if (Num32.NumberState != stateOfNumber.error)
                        {
                            thread32.Start();
                        }
                        if (Num64.NumberState != stateOfNumber.error)
                        {
                            thread64.Start();                            
                        }
                        if (Num128.NumberState != stateOfNumber.error)
                        {
                            thread128.Start();
                        }
                        if (Num256.NumberState != stateOfNumber.error)
                        {
                            thread256.Start();
                        }
                }
                else
                {
                    /*>>>>>>>>>>>>>>>>>>>>> LEFT PART BEGIN <<<<<<<<<<<<<<<<<<<<<<<*/
                    RightPartCalculating = false;
                    if (NumberFormat == 1)
                    {
                        inputStringR = inputString.Substring(inputString.IndexOf("/") + 1);
                        inputString = inputString.Substring(0, inputString.IndexOf("/"));
                    }
                    else
                    {
                        inputStringR = inputString.Substring(inputString.IndexOf(";") + 1);
                        inputString = inputString.Substring(0, inputString.IndexOf(";"));
                    }

                    NormalizedNumber = NormalizeNumber(inputString, 2000, PartOfNumber.Left);
                    // Denormalize Number
                    Num64.Denormalized = Num128.Denormalized = Num256.Denormalized = DenormalizeNumber(NormalizedNumber, PartOfNumber.Left);
                    // Convert from 10cc to 2cc
                    //if ((IntPartDenormalized != "0") || (FloatPartDenormalized != "0"))
                    //{
                        tempNumber = convertToExp(DenormalizedNumber);
                        defineNumberState(tempNumber, PartOfNumber.Left);
                        if (Num256.NumberState != stateOfNumber.error)
                        {
                            this.BinaryIntPart = convert10to2IPart(IntPartDenormalized);
                            this.BinaryFloatPart = convert10to2FPart(FloatPartDenormalized);
                            FillBinaryVars(PartOfNumber.Left);
                            // Fill SEM 
                        }
                        else
                        {
                            this.BinaryIntPart = "0";
                            this.BinaryFloatPart = "0";
                            FillBinaryVars(PartOfNumber.Left);
                        }
                        if (Num64.NumberState != stateOfNumber.error)
                        {
                            thread64.Start();       
                        }
                        if (Num128.NumberState != stateOfNumber.error)
                        {
                            thread128.Start();       
                        }
                        if (Num256.NumberState != stateOfNumber.error)
                        {
                            thread256.Start();       
                        }
                        changeNumberState(false, false);
                    
                    /*>>>>>>>>>>>>>>>>>>>>> LEFT PART END <<<<<<<<<<<<<<<<<<<<<<<*/

                    /*>>>>>>>>>>>>>>>>>>>>> RIGTH PART BEGIN <<<<<<<<<<<<<<<<<<<<<<<*/
                        RightPartCalculating = true;
                    NormalizedNumber = NormalizeNumber(inputStringR, 2000, PartOfNumber.Right);
                    // Denormalize Number
                    Num64.DenormalizedRight = Num128.DenormalizedRight = Num256.DenormalizedRight = DenormalizeNumber(NormalizedNumber, PartOfNumber.Right);
                    // Convert from 10cc to 2cc
                    //if ((IntPartDenormalized != "0") || (FloatPartDenormalized != "0"))
                    //{
                        tempNumber = convertToExp(DenormalizedNumber);
                        defineNumberState(tempNumber, PartOfNumber.Right);
                        if (Num256.NumberStateRight != stateOfNumber.error)
                        {
                            this.BinaryIntPart = convert10to2IPart(IntPartDenormalized);
                            this.BinaryFloatPart = convert10to2FPart(FloatPartDenormalized);
                            FillBinaryVars(PartOfNumber.Right);
                            // Fill SEM 
                        }
                        else
                        {
                            this.BinaryIntPart = "0";
                            this.BinaryFloatPart = "0";
                            FillBinaryVars(PartOfNumber.Right);
                        }

                        if (Num64.NumberStateRight != stateOfNumber.error)
                        {
                            thread64.Start();       
                        }
                        if (Num128.NumberStateRight != stateOfNumber.error)
                        {
                            thread128.Start();       
                        }
                        if (Num256.NumberStateRight != stateOfNumber.error)
                        {
                            thread256.Start();       
                        }
                        /*>>>>>>>>>>>>>>>>>>>>> RIGTH PART END <<<<<<<<<<<<<<<<<<<<<<<*/
                }
            }
            catch (Exception ex)
            {
                //throw new FCCoreFunctionException("Func 'SetNumber(Object threadContext)' =[ " + ex.Message + " ]");
                exceptionUtil.isExceptionRised = true;
                exceptionUtil.AddException("Func 'SetNumber(Object threadContext)' =[" + ex.Message + " ]");
            }
        }
      
        public void Calculate32()
        {
            try
            {
                Num32.Exponenta = selectExp(Num32, PartOfNumber.Left);
                progIncThreadSafe();
                Num32.Mantisa = selectMantissa(Num32, NumberFormat, PartOfNumber.Left);
                progIncThreadSafe();
                Num32.NumberState = changeNumberState(32, PartOfNumber.Left);
                calcRes(Num32, PartOfNumber.Left);
                calcError(Num32, PartOfNumber.Left);
                progIncThreadSafe();
            }
            catch (Exception ex)
            {
                //if (Thread.CurrentThread != null)
                //throw new FCCoreFunctionException("Func 'Calculate32'=["+ex.Message+"]");
            }
        }
        public void Calculate64(object RightPartCalculation)
        {
            try
            {
                bool RightPart = (bool)RightPartCalculation;
                PartOfNumber tempPart = RightPart == false ? PartOfNumber.Left: PartOfNumber.Right;
                if (!RightPart)
                    Num64.Exponenta = selectExp(Num64, PartOfNumber.Left);
                else
                    Num64.ExponentaRight = selectExp(Num64, PartOfNumber.Right);
                progIncThreadSafe();
                if (!RightPart)
                {
                    Num64.Mantisa = selectMantissa(Num64, NumberFormat, PartOfNumber.Left);
                    Num64.NumberState = changeNumberState(64, tempPart);
                }
                else
                {
                    Num64.MantisaRight = selectMantissa(Num64, NumberFormat, PartOfNumber.Right);
                    Num64.NumberStateRight = changeNumberState(64, tempPart);
                }
                progIncThreadSafe();
                calcRes(Num64, tempPart);
                calcError(Num64, tempPart);
                progIncThreadSafe();
            }
            catch (Exception ex)
            {  
                //if (Thread.CurrentThread!=null)
                // throw new FCCoreFunctionException("Func 'Calculate64'=[" + ex.Message + "]");
            }
        }
        public void Calculate128(object RightPartCalculation)
        {
            try
            {
                bool RightPart = (bool)RightPartCalculation;
                PartOfNumber tempPart = RightPart == false ? PartOfNumber.Left : PartOfNumber.Right;
                if (!RightPart)
                    Num128.Exponenta = selectExp(Num128, PartOfNumber.Left);
                else
                    Num128.ExponentaRight = selectExp(Num128, PartOfNumber.Right);
                progIncThreadSafe();
                if (!RightPart)
                {
                    Num128.Mantisa = selectMantissa(Num128, NumberFormat, PartOfNumber.Left);
                    Num128.NumberState = changeNumberState(128, tempPart);
                }
                else
                {
                    Num128.MantisaRight = selectMantissa(Num128, NumberFormat, PartOfNumber.Right);
                    Num128.NumberStateRight = changeNumberState(128, tempPart);
                }
                progIncThreadSafe();
                calcRes(Num128, tempPart);
                calcError(Num128, tempPart);
                progIncThreadSafe();
            }
            catch (Exception ex)
            {
                //if (Thread.CurrentThread != null)
                //throw new FCCoreFunctionException("Func 'Calculate128'=[" + ex.Message + "]");
            }
        }
        public void Calculate256(object RightPartCalculation)
        {
            try
            {
                bool RightPart = (bool)RightPartCalculation;
                PartOfNumber tempPart = RightPart == false ? PartOfNumber.Left : PartOfNumber.Right;
                if (!RightPart)
                    Num256.Exponenta = selectExp(Num256, PartOfNumber.Left);
                else
                    Num256.ExponentaRight = selectExp(Num256, PartOfNumber.Right);
                progIncThreadSafe();
                if (!RightPart)
                {
                    Num256.Mantisa = selectMantissa(Num256, NumberFormat, PartOfNumber.Left);
                    Num256.NumberState = changeNumberState(256, tempPart);
                }
                else
                {
                    Num256.MantisaRight = selectMantissa(Num256, NumberFormat, PartOfNumber.Right);
                    Num256.NumberStateRight = changeNumberState(256, tempPart);
                }
                progIncThreadSafe();
                changeNumberState(256, tempPart);
                calcRes(Num256, tempPart);
                calcError(Num256, tempPart);
                progIncThreadSafe();
            }
            catch (Exception ex)
            {
                //if (Thread.CurrentThread != null)
                //throw new FCCoreFunctionException("Func 'Calculate256'=[" + ex.Message + "]");
            }
        }

        /// <summary>
        /// Separates number on Left and Right Part
        /// </summary>
        public void SeparateInputNumber()
        {
            switch (NumberFormat)
            {
                case 1:
                    inputStringL = inputString.Substring(0,inputString.IndexOf(";"));
                    inputStringR = inputString.Substring(inputString.IndexOf(";") + 1, inputString.Length);
                    break;
                case 2:
                    inputStringL = inputString.Substring(0,inputString.IndexOf("/"));
                    inputStringR = inputString.Substring(inputString.IndexOf("/") + 1, inputString.Length);
                    break;
            }
        }

        public CalculationStatus GetCalcStatus()
        {
            return calcStatus;
        }

        public String NormalizeNumber(String dataString, int inAccuracy,PartOfNumber Left_Right)
        {
            try
            {
                if (dataString.Length > inAccuracy)
                    dataString = dataString.Substring(0, inAccuracy);

                if (dataString.Contains("E"))
                    dataString = dataString.Replace('E', 'e');
                //else
                // return null || Rise FCCoreException
                if (dataString.Contains("."))
                    dataString = dataString.Replace('.', ',');
                //else
                // return null || Rise FCCoreException

                if (dataString.IndexOf(',') == 0)
                    dataString = "0" + dataString;


                if ((dataString[0] != '-') && (dataString[0] != '+'))
                {
                    dataString = "+" + dataString;
                    if (NumberFormat == 0)
                        SignLeft = "0";
                    else
                    {
                        if (Left_Right == PartOfNumber.Left)
                            SignLeft = "0";
                        else
                            SignRight = "0";
                    }
                }
                else {
                    if (NumberFormat == 0)
                        SignLeft = "1";
                    else
                    {
                        if (Left_Right == PartOfNumber.Left)
                            SignLeft = "1";
                        else
                            SignRight = "1";
                    }
                }


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
            }
            catch (Exception ex)
            {
                throw new Exception("NormalizeNumber:" + ex.Message);
            }
            return dataString;
        }

        /* 
            * signExp       -> ExpSign
            * sign          -> Sign
            * signCharacter -> SignCharacter
            * exp           -> E
            * iPart         -> IntPartDenormalized
            * fPart         -> FloatPartDenormalized
            * inputString   -> Denormalized
            */
        /// <summary>
        /// Denormolizes number
        /// </summary>
        /// <param name="dataString">Input String to dernomolize</param>
        /// <param name="Left_Right">True if left part now working, else right</param>
        /// <returns>Denormolized number as String</returns>
        public String DenormalizeNumber(String dataString ,PartOfNumber Left_Right)
        {   
            String denormNumber = "";
            String denormIntPart = "", denormFloatPart = "";
            String[] tempArray;
            try
            {
                ExpSign = dataString.Substring(dataString.IndexOf('e') + 1, 1);
                if (dataString[0] == '+')
                {
                    if (NumberFormat == 0)
                    {
                        SignLeft = "0";
                        SignCharacterLeft = "+";
                    }
                    else
                    {
                        if (Left_Right == PartOfNumber.Left)
                        {
                            SignLeft = "0";
                            SignCharacterLeft = "+";
                        }
                        else
                        {
                            SignRight = "0";
                            SignCharacterRight = "+";
                        }
                    }
                }
                else
                    if (dataString[0] == '-')
                    {
                        if (NumberFormat == 0)
                        {
                            SignLeft = "1";
                            SignCharacterLeft = "-";
                        }
                        else
                        {
                            if (Left_Right == PartOfNumber.Left)
                            {
                                SignLeft = "1";
                                SignCharacterLeft = "-";
                            }
                            else
                            {
                                SignRight = "1";
                                SignCharacterRight = "-";
                            }
                        }
                    }
                    else
                    {
                        if (NumberFormat == 0)
                        {
                            SignLeft = "0";
                            SignCharacterLeft = "+";
                        }
                        else
                        {
                            if (Left_Right == PartOfNumber.Left)
                            {
                                SignLeft = "0";
                                SignCharacterLeft = "+";
                            }
                            else
                            {
                                SignRight = "0";
                                SignCharacterRight = "+";
                            }
                        }
                    }
                        //throw new Exception("Func [selectSEM]:= NoSignException.");

                int index = dataString.IndexOf('e') + 1;
                if (index < dataString.Length)
                    E = dataString.Substring(index, dataString.Length - index);
                else
                    throw new Exception("Func [selectSEM]:= NoExponentaException.");

                int iExp = Math.Abs(int.Parse(E));
                if ((dataString[0] == '-') || (dataString[0] == '+'))
                    dataString = dataString.Substring(1);
                /*iPart */
                denormIntPart = dataString.Substring(0, dataString.IndexOf(','));
                index = dataString.IndexOf(',') + 1;

                /*fPart*/
                denormFloatPart = dataString.Substring(index, dataString.IndexOf('e') - index);////+1
                if (ExpSign == "+")
                {
                    String fPartTemp = denormFloatPart;
                    if (iExp > 0)
                    {
                        tempArray = new String[Math.Abs( iExp - denormFloatPart.Length)];
                        for (int i = 0; i < (Math.Abs( iExp - denormFloatPart.Length)); i++)
                            tempArray[i] = "0";
                        fPartTemp = fPartTemp + String.Join("",tempArray) ;
                        denormFloatPart = "0";
                    }
                    denormIntPart = denormIntPart + fPartTemp.Substring(0, iExp);
                    denormFloatPart = fPartTemp.Substring(iExp);
                    if (denormFloatPart.Length == 0)
                        denormFloatPart = "0";
                }
                else
                {
                    String iPartTemp = denormIntPart;
                    tempArray = new String[Math.Abs(iExp - denormIntPart.Length)];
                    for (int i = 0; i < Math.Abs((iExp - denormIntPart.Length)); i++)
                        tempArray[i] = "0";
                        iPartTemp = String.Join("", tempArray) + iPartTemp;
                    if (iExp > denormIntPart.Length)
                    {
                        denormFloatPart = iPartTemp + denormFloatPart;
                        denormIntPart = "0";
                    }
                    else
                    {
                        denormFloatPart = iPartTemp.Substring(iPartTemp.Length - iExp) + denormFloatPart;
                        if (iPartTemp.Length != iExp)
                            denormIntPart = iPartTemp.Substring(0, iPartTemp.Length - iExp);
                        else
                            denormIntPart = "0";
                    }
                }
                // iPart = myUtil.deleteZeroFromNumber(iPart);
                // if (iPart[0] == '0')
                //    iPart = iPart.Substring(1);
                while ((denormIntPart[0] == '0') && (denormIntPart.Length > 1))
                {
                    denormIntPart = denormIntPart.Substring(1);
                }
                // Compact to one statement num32 = num64 = num128 = num256 = denorm
                if (Left_Right == PartOfNumber.Left)
                    denormNumber = SignCharacterLeft + denormIntPart + "," + denormFloatPart;
                else
                    denormNumber = SignCharacterRight + denormIntPart + "," + denormFloatPart;
                //Num32.Denormalized = Num64.Denormalized = Num128.Denormalized = Num256.Denormalized = 
                if (NumberFormat == 0)
                {
                    Num32.IntPartDenormalized = Num64.IntPartDenormalized = Num128.IntPartDenormalized = Num256.IntPartDenormalized = IntPartDenormalized = denormIntPart;
                    Num32.FloatPartDenormalized = Num64.FloatPartDenormalized = Num128.FloatPartDenormalized = Num256.FloatPartDenormalized = FloatPartDenormalized = denormFloatPart;
                    DenormIntPart = denormIntPart;
                    DenormFloatPart = denormFloatPart;
                }
                else
                {
                    if (Left_Right == PartOfNumber.Left)
                    {
                        IntPartDenormalizedFILeft = denormIntPart;
                        FloatPartDenormalizedFILeft = denormFloatPart;
                    }
                    else
                    {
                        IntPartDenormalizedFIRight = denormIntPart;
                        FloatPartDenormalizedFIRight = denormFloatPart;
                    }
                    //Num32.IntPartDenormalizedFI = Num64.IntPartDenormalizedFI = Num128.IntPartDenormalizedFI = Num256.IntPartDenormalizedFI = IntPartDenormalizedFI = denormIntPart;
                    //Num32.FloatPartDenormalizedFI = Num64.FloatPartDenormalizedFI = Num128.FloatPartDenormalizedFI = Num256.FloatPartDenormalizedFI = FloatPartDenormalizedFI = denormFloatPart;
                    DenormIntPartFI = denormIntPart;
                    DenormFloatPartFI = denormFloatPart;
                }
                return denormNumber;
            }
            catch (Exception ex)
            {
                //throw new FCCoreFunctionException("Exception in Func ['selectSEM'] Mess=[" + ex.Message + "]");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Fills all binary string 
        /// </summary>
        /// <param name="Left_Right">False - Left part og number, else - Right </param>
        public void FillBinaryVars(PartOfNumber Left_Right)
        {
            if (((this.BinaryIntPart != null) && (this.BinaryFloatPart != null)) || ((this.BinaryFloatPartFIRight != null) && (this.BinaryIntPartFIRight != null)) || ((this.BinaryFloatPartFILeft != null) && (this.BinaryIntPartFILeft != null)))
            {
                if (Num32 != null)
                {
                    Num32.BinaryIntPart = this.BinaryIntPart;
                    Num32.BinaryFloatPart = this.BinaryFloatPart;
                }
                if (Num64 != null)
                {
                    if (NumberFormat == 0)
                    {
                        Num64.BinaryIntPart = this.BinaryIntPart;
                        Num64.BinaryFloatPart = this.BinaryFloatPart;
                    }
                    else
                    {
                        if (Left_Right == PartOfNumber.Left)
                        {
                            Num64.BinaryIntPartFILeft = this.BinaryIntPartFILeft;
                            Num64.BinaryFloatPartFILeft = this.BinaryFloatPartFILeft;
                        }
                        else
                        {
                            Num64.BinaryIntPartFIRight = this.BinaryIntPartFIRight;
                            Num64.BinaryFloatPartFIRight = this.BinaryFloatPartFIRight;
                        }
                    }
                }
                if (Num128 != null)
                {
                    if (NumberFormat == 0)
                    {
                        Num128.BinaryIntPart = this.BinaryIntPart;
                        Num128.BinaryFloatPart = this.BinaryFloatPart;
                    }
                    else
                    {
                        if (Left_Right == PartOfNumber.Left)
                        {
                            Num128.BinaryIntPartFILeft = this.BinaryIntPartFILeft;
                            Num128.BinaryFloatPartFILeft = this.BinaryFloatPartFILeft;
                        }
                        else
                        {
                            Num128.BinaryIntPartFIRight = this.BinaryIntPartFIRight;
                            Num128.BinaryFloatPartFIRight = this.BinaryFloatPartFIRight;
                        }
                    }
                }
                if (Num256 != null)
                {
                    if (NumberFormat == 0)
                    {
                        Num256.BinaryIntPart = this.BinaryIntPart;
                        Num256.BinaryFloatPart = this.BinaryFloatPart;
                    }
                    else
                    {
                        if (Left_Right == PartOfNumber.Left)
                        {
                            Num256.BinaryIntPartFILeft = this.BinaryIntPartFILeft;
                            Num256.BinaryFloatPartFILeft = this.BinaryFloatPartFILeft;
                        }
                        else
                        {
                            Num256.BinaryIntPartFIRight = this.BinaryIntPartFIRight;
                            Num256.BinaryFloatPartFIRight = this.BinaryFloatPartFIRight;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Defines if input string consist only of symbol '1'
        /// </summary>
        /// <param name="inExp"></param>
        /// <returns>True - if all symbols in exp are '1' ; False - if one or more symbols are '0'</returns>
        public bool checkExpFull(String inExp)
        {
            int i;
            for (i = 0; i < inExp.Length; i++)
            {
                if (inExp[i] == '0')
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check if the input string consist only from 1 bits
        /// </summary>
        /// <param name="inStr">Input string for check</param>
        /// <returns>True - if there none '0' symbols in input string ; False -  one or more symbols are '0'</returns>
        public bool checkStringFull(String inStr)
        {
            int i;
            for (i = 0; i < inStr.Length; i++)
            {
                if (inStr[i] == '0')
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Finds number state based on exponent, mantisa and input number
        /// </summary>
        /// <param name="exp">True - exponent consist only of '0' ; False - has one or more '1'</param>
        /// <param name="man">True - mantisa consist only of '0' ; False - has one or more '1'</param>
        /// <param name="num">True - number consist only of '0' ; False - has one or more '1'</param>
        /// <param name="expFull">True - exponent consist only of '1' ; False - has one or more '0'</param>
        /// <returns>Number state</returns>
        public stateOfNumber checkNumberState(bool exp, bool man, bool num, bool expFull,bool isRecalculation)
        {
            // exp == 0 
            if (exp)
            {
                if (man)
                {
                    if (num)  // exp == 0 && man == 0 && num == 0
                        return stateOfNumber.zero;
                    else // exp == 0 && man == 0 && num != 0
                        if (!isRecalculation)
                            return stateOfNumber.error;
                        else
                            return stateOfNumber.zero;
                }
                else // exp == 0 && man != 0
                    return stateOfNumber.denormalized;
            }
            else
            {

                if (expFull) // exp = 111111
                {
                    if (man) // man == 0
                    {
                        return stateOfNumber.infinite;
                    }
                    else
                    {
                        return stateOfNumber.NaN;
                    }
                }
                else         // exp = 111110
                {
                    return stateOfNumber.normalized;
                }

            }
        }

        /// <summary>
        /// Defines states for all numbers in FCCore.
        /// If Exp or Mantisa in number are empty , number state won't be calculated.
        /// </summary>
        public stateOfNumber changeNumberState(int inputNumCapacity,PartOfNumber Left_Right)
        {
            bool expZero, manZero, numZero, expFull;
            expZero = manZero = numZero = expFull = false;
            try
            {
                if (NumberFormat == 0)
                    numZero = isStringZero(inputString);
                else
                    if (Left_Right == PartOfNumber.Left)
                        numZero = isStringZero(inputString);
                    else
                        numZero = isStringZero(inputStringR);

                switch (inputNumCapacity)
                {
                    case 32:
                        {
                            if ((NumberFormat == 0) && (Left_Right == PartOfNumber.Left))
                            {
                                if ((Num32.Exponenta != "") && (Num32.Mantisa != ""))
                                {
                                    expZero = isStringZero(Num32.Exponenta);
                                    manZero = isStringZero(Num32.Mantisa);
                                    expFull = checkExpFull(Num32.Exponenta);
                                    if (Num32.NumberState != stateOfNumber.error)
                                    //    Num32.NumberState = checkNumberState(expZero, manZero, numZero, expFull, false);
                                    return checkNumberState(expZero, manZero, numZero, expFull, false);
                                }
                            } throw new FCCoreArithmeticException("Func changeNumberState (int,PartOfNumber)=['Format 32 Exponenta or Mantissa is empty.'");
                        break;
                        }
                    case 64:
                        {
                            if ((NumberFormat > 0) && (Left_Right == PartOfNumber.Right))
                            {
                                if ((Num64.ExponentaRight != "") && (Num64.MantisaRight != ""))
                                {
                                    expZero = isStringZero(Num64.ExponentaRight);
                                    manZero = isStringZero(Num64.MantisaRight);
                                    expFull = checkExpFull(Num64.ExponentaRight);
                                    if (Num64.NumberStateRight != stateOfNumber.error)
                                    //    Num64.NumberStateRight = checkNumberState(expZero, manZero, numZero, expFull, false);
                                    return checkNumberState(expZero, manZero, numZero, expFull, false);
                                } throw new FCCoreArithmeticException("Func changeNumberState (int,PartOfNumber)=['Format 64 ExponentaR or MantissaR is empty.'");
                            }
                            else
                            {
                                if ((Num64.Exponenta != "") && (Num64.Mantisa != ""))
                                {
                                    expZero = isStringZero(Num64.Exponenta);
                                    manZero = isStringZero(Num64.Mantisa);
                                    expFull = checkExpFull(Num64.Exponenta);
                                    if (Num64.NumberState != stateOfNumber.error)
                                    //    Num64.NumberState = checkNumberState(expZero, manZero, numZero, expFull, false);
                                    return checkNumberState(expZero, manZero, numZero, expFull, false);
                                } throw new FCCoreArithmeticException("Func changeNumberState (int,PartOfNumber)=['Format 64 Exponenta or Mantissa is empty.'");
                            }
                            break;
                        }
                    case 128:
                        {
                            if ((NumberFormat > 0) && (Left_Right == PartOfNumber.Right))
                            {
                                if ((Num128.ExponentaRight != "") && (Num128.MantisaRight != ""))
                                {
                                    expZero = isStringZero(Num128.ExponentaRight);
                                    manZero = isStringZero(Num128.MantisaRight);
                                    expFull = checkExpFull(Num128.ExponentaRight);
                                    if (Num128.NumberStateRight != stateOfNumber.error)
                                    //    Num128.NumberStateRight = checkNumberState(expZero, manZero, numZero, expFull, false);
                                    return checkNumberState(expZero, manZero, numZero, expFull, false);
                                } throw new FCCoreArithmeticException("Func changeNumberState (int,PartOfNumber)=['Format 128 ExponentaR or MantissaR is empty.'");
                            }
                            else
                            {
                                if ((Num128.Exponenta != "") && (Num128.Mantisa != ""))
                                {
                                    expZero = isStringZero(Num128.Exponenta);
                                    manZero = isStringZero(Num128.Mantisa);
                                    expFull = checkExpFull(Num128.Exponenta);
                                    if (Num128.NumberState != stateOfNumber.error)
                                    //    Num128.NumberState = checkNumberState(expZero, manZero, numZero, expFull, false);
                                    return checkNumberState(expZero, manZero, numZero, expFull, false);
                                } throw new FCCoreArithmeticException("Func changeNumberState (int,PartOfNumber)=['Format 128 Exponenta or Mantissa is empty.'");
                            }
                            break;
                        }
                    case 256:
                        {
                            if ((NumberFormat > 0) && (Left_Right == PartOfNumber.Right))
                            {
                                if ((Num256.ExponentaRight != "") && (Num256.MantisaRight != ""))
                                {
                                    expZero = isStringZero(Num256.ExponentaRight);
                                    manZero = isStringZero(Num256.MantisaRight);
                                    expFull = checkExpFull(Num256.ExponentaRight);
                                    if (Num256.NumberStateRight != stateOfNumber.error)
                                    //    Num256.NumberStateRight = checkNumberState(expZero, manZero, numZero, expFull, false);
                                    return checkNumberState(expZero, manZero, numZero, expFull, false);
                                } throw new FCCoreArithmeticException("Func changeNumberState (int,PartOfNumber)=['Format 256 ExponentaR or MantissaR is empty.'");
                            }
                            else
                            {
                                if ((Num256.Exponenta != "") && (Num256.Mantisa != ""))
                                {
                                    expZero = isStringZero(Num256.Exponenta);
                                    manZero = isStringZero(Num256.Mantisa);
                                    expFull = checkExpFull(Num256.Exponenta);
                                    if (Num256.NumberState != stateOfNumber.error)
                                    //    Num256.NumberState = checkNumberState(expZero, manZero, numZero, expFull, false);
                                    return checkNumberState(expZero, manZero, numZero, expFull, false);
                                } throw new FCCoreArithmeticException("Func changeNumberState (int,PartofNumber)=['Format 256 Exponenta or Mantissa is empty.'");
                            }
                            break;
                        }
                    default:
                        //return stateOfNumber.error;
                        throw new FCCoreArithmeticException("Func changeNumberState (int,PartOfNumber)=['Such format doesn't exists.']");
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new FCCoreGeneralException("Func changeNumberState(int inputNumCapacity,PartOfNumber RightPart) = [ " + ex.Message + " ]");
            }
        }
        /// <summary>
        /// Defines states for all numbers in FCCore.
        /// If Exp or Mantisa in number are empty , number state won't be calculated.
        /// </summary>
        public void changeNumberState(bool rightPart,bool recalculation)
        {
            int i;
            bool expZero, manZero, numZero, expFull;
            //stateOfNumber state = stateOfNumber.NaN;
            try
            {
                expZero = manZero = numZero = expFull = false;
                if (!recalculation)
                    numZero = isStringZero(inputString);
                else
                    numZero = false;
                
                if (rightPart)
                {
                    numZero = isStringZero(inputStringR);
                }

                for (i = 32; i <= 256; i *= 2)
                {

                    switch (i)
                    {
                        case 32:
                            {
                                if (NumberFormat == 0)
                                {
                                    if ((Num32.Exponenta != "") && (Num32.Mantisa != ""))
                                    {
                                        expZero = isStringZero(Num32.Exponenta);
                                        manZero = isStringZero(Num32.Mantisa);
                                        expFull = checkExpFull(Num32.Exponenta);
                                        if (Num32.NumberState != stateOfNumber.error)
                                            Num32.NumberState = checkNumberState(expZero, manZero, numZero, expFull, recalculation);
                                    }
                                }
                                break;
                            }
                        case 64:
                            {
                                if ((NumberFormat != 0) && rightPart)
                                {
                                    if ((Num64.ExponentaRight != "") && (Num64.MantisaRight != ""))
                                    {
                                        expZero = isStringZero(Num64.ExponentaRight);
                                        manZero = isStringZero(Num64.MantisaRight);
                                        expFull = checkExpFull(Num64.ExponentaRight);
                                        if (Num64.NumberStateRight != stateOfNumber.error)
                                            Num64.NumberStateRight = checkNumberState(expZero, manZero, numZero, expFull, recalculation);
                                    }

                                }
                                else
                                {
                                    if ((Num64.Exponenta != "") && (Num64.Mantisa != ""))
                                    {
                                        expZero = isStringZero(Num64.Exponenta);
                                        manZero = isStringZero(Num64.Mantisa);
                                        expFull = checkExpFull(Num64.Exponenta);
                                        if (Num64.NumberState != stateOfNumber.error)
                                            Num64.NumberState = checkNumberState(expZero, manZero, numZero, expFull, recalculation);
                                    }
                                }
                                break;
                            }
                        case 128:
                            {
                                if ((NumberFormat != 0) && rightPart)
                                {
                                    if ((Num128.ExponentaRight != "") && (Num128.MantisaRight != ""))
                                    {
                                        expZero = isStringZero(Num128.ExponentaRight);
                                        manZero = isStringZero(Num128.MantisaRight);
                                        expFull = checkExpFull(Num128.ExponentaRight);
                                        if (Num128.NumberStateRight != stateOfNumber.error)
                                            Num128.NumberStateRight = checkNumberState(expZero, manZero, numZero, expFull, recalculation);
                                    }

                                }
                                else
                                {
                                    if ((Num128.Exponenta != "") && (Num128.Mantisa != ""))
                                    {
                                        expZero = isStringZero(Num128.Exponenta);
                                        manZero = isStringZero(Num128.Mantisa);
                                        expFull = checkExpFull(Num128.Exponenta);
                                        if (Num128.NumberState != stateOfNumber.error)
                                            Num128.NumberState = checkNumberState(expZero, manZero, numZero, expFull, recalculation);
                                    }
                                }
                                break;
                            }
                        case 256:
                            {
                                if ((NumberFormat != 0) && rightPart)
                                {
                                    if ((Num256.ExponentaRight != "") && (Num256.MantisaRight != ""))
                                    {
                                        expZero = isStringZero(Num256.ExponentaRight);
                                        manZero = isStringZero(Num256.MantisaRight);
                                        expFull = checkExpFull(Num256.ExponentaRight);
                                        if (Num256.NumberStateRight != stateOfNumber.error)
                                            Num256.NumberStateRight = checkNumberState(expZero, manZero, numZero, expFull, recalculation);
                                    }

                                }
                                else
                                {
                                    if ((Num256.Exponenta != "") && (Num256.Mantisa != ""))
                                    {
                                        expZero = isStringZero(Num256.Exponenta);
                                        manZero = isStringZero(Num256.Mantisa);
                                        expFull = checkExpFull(Num256.Exponenta);
                                        if (Num256.NumberState != stateOfNumber.error)
                                            Num256.NumberState = checkNumberState(expZero, manZero, numZero, expFull, recalculation);
                                    }
                                }
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FCCoreGeneralException("Func 'changeNumberState(bool rightPart,bool recalculation)' = [ " + ex.Message + " ]");
            }
        }


        private enum pb32  { min=-45, max=38 ,mint=-7,maxt=4};
        private enum pb64  { min=-323, max=308, minfi=-45, maxfi=38, minfit=-7, maxfit=4, mint=-45, maxt=38 };
        private enum pb128 { min=-4963, max=4932, minfi=-323, maxfi=308, minfit=-45, maxfit=38, mint=-323, maxt=308 };
        private enum pb256 { min=-157892, max=157826, minfi=-4963, maxfi=4932, minfit=-4963, maxfit=4932, mint=-4963, maxt=4932 };
        
        /// <summary> new func
        /// Defines states for all numbers in FCCore, based on nmber bounds
        /// </summary>
        public void defineNumberState(String inNum, PartOfNumber RightPart)
        {
            String Exp="";
            int exp=0;
            int min32, max32, min64, max64, min128, max128, min256, max256;
            min32 = max32 = min64 = max64 = min128 = max128 = min256 = max256 = 0;
            if (inNum.IndexOf('e') != -1)
            {
                Exp = inNum.Substring(inNum.IndexOf('e') + 1);
                exp = int.Parse(Exp);
            }
            
            switch (NumberFormat)
            {
                case 0: 
                    min32 = (int)pb32.min;
                    min64 = (int)pb64.min;
                    min128 = (int)pb128.min;
                    min256 = (int)pb256.min;

                    max32 = (int)pb32.max;
                    max64 = (int)pb64.max;
                    max128 = (int)pb128.max;
                    max256 = (int)pb256.max;
                     break;
                case 1: 
                case 2:
                    min32 = (int)pb32.min;
                    max32 = (int)pb32.max;

                    min64 = (int)pb64.minfi;
                    min128 = (int)pb128.minfi;
                    min256 = (int)pb256.minfi;

                    max64 = (int)pb64.maxfi;
                    max128 = (int)pb128.maxfi;
                    max256 = (int)pb256.maxfi;
                    break;
                case 3: // tetra
                    min32 = (int)pb32.mint;
                    min64 = (int)pb64.mint;
                    min128 = (int)pb128.mint;
                    min256 = (int)pb256.mint;

                    max32 = (int)pb32.maxt;
                    max64 = (int)pb64.maxt;
                    max128 = (int)pb128.maxt;
                    max256 = (int)pb256.maxt;
                    break;
                case 4: // float|| interval + tetra                    
                    min64 = (int)pb64.minfit;
                    min128 = (int)pb128.minfit;
                    min256 = (int)pb256.minfit;

                    max64 = (int)pb64.maxfit;
                    max128 = (int)pb128.maxfit;
                    max256 = (int)pb256.maxfit;
                    break;
            }

            if (exp != 0)
            {
                
                if ((exp < min32) || (exp > max32))
                {
                    if (RightPart == PartOfNumber.Left)
                        Num32.NumberState = stateOfNumber.error;
                    //else
                    //    Num32.NumberStateRight = stateOfNumber.error;
                }

                if ((exp < min64) || (exp > max64))
                {
                    if (RightPart == PartOfNumber.Left)
                        Num64.NumberState = stateOfNumber.error;
                    else
                        Num64.NumberStateRight = stateOfNumber.error;
                }

                if ((exp < min128) || (exp > max128))
                {
                    if (RightPart == PartOfNumber.Left)
                        Num128.NumberState = stateOfNumber.error;
                    else
                        Num128.NumberStateRight = stateOfNumber.error;
                }

                if ((exp < min256) || (exp > max256))
                {
                    if (RightPart == PartOfNumber.Left)
                        Num256.NumberState = stateOfNumber.error;
                    else
                        Num64.NumberStateRight = stateOfNumber.error;
                }
            }
            else
            {
                //32 
                if ( RightPart == PartOfNumber.Left)
                Num32.NumberState = stateOfNumber.normalized;

                //64
                if (RightPart == PartOfNumber.Right)
                {
                    Num64.NumberStateRight = stateOfNumber.normalized;
                }
                else
                {
                    Num64.NumberState = stateOfNumber.normalized;
                }

                //128
                if (RightPart == PartOfNumber.Right)
                {
                    Num128.NumberStateRight = stateOfNumber.normalized;
                }
                else
                {
                    Num128.NumberState = stateOfNumber.normalized;
                }

                //256
                if (RightPart == PartOfNumber.Right)
                {
                    Num256.NumberStateRight = stateOfNumber.normalized;
                }
                else
                {
                    Num256.NumberState = stateOfNumber.normalized;
                }
            }
        }

        /// <summary>
        /// Changes Input Number if symbols in denormalized form more then Limit.
        /// </summary>
        /// <param name="inputStr">Input Normolized Number.</param>
        /// <param name="Limit">Quatity of symbols that should be in denormolized number.</param>
        /// <returns>Cutted Normolized Number if it Leght more than Limit.</returns>
        public String checkExponentQuantity(String inputStr, int Limit)
        {
            // 1,123e-123 // 1,123e-123
            // 1123e-123  // 1323e+123

            String part1, part2, part3, sign;
            int iDot, iExp, res, expNum, delta, subres;
            int p1, p2;
            String resStr;
            part1 = "";
            part2 = "";
            part3 = "";
            sign = "";
            try
            {
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
            }
            catch (Exception ex)
            {
                throw new FCCoreGeneralException("Func [checkExponentQuantity]:= General Exception.");
            }
            return "";
        }

        //----------------------   Arithmetics  BEGIN
        public const int ACCURANCY = 2000;
        public static int AdditionalAccurancy = 0;
        /// <summary>
        /// Converts float part of number form 10cc to 2cc.Lenght depends from constant 'ACCURANCY'
        /// Funcs using: NONE
        /// Vars using : ACCURANCY
        /// </summary>
        /// <param name="inString">Input Number to convert</param>
        /// <returns>Returns number float part in 2cc</returns>
        public static String convert10to2FPart(String inString)
        {// accurancy   -> ACCURANCY
            String result = "";
            String outString = "";
            int plusOne = 0;
            int countAccuracy, i, currNumber;

            try
            {
                if (inString == "0")
                {
                    result = "0";
                    return result;
                }


                for (countAccuracy = 0; countAccuracy < ACCURANCY + AdditionalAccurancy; countAccuracy++)
                {

                    outString = "";
                    plusOne = 0;
                    for (i = inString.Length; i > 0; i--)
                    {
                        currNumber = int.Parse(inString[i - 1].ToString());
                        if (currNumber < 5)
                        {
                            outString = (currNumber * 2 + plusOne).ToString() + outString;
                            plusOne = 0;
                        }
                        else
                        {
                            outString = (currNumber * 2 + plusOne - 10).ToString() + outString;
                            plusOne = 1;
                            if (i == 1)
                                outString = "1" + outString;
                        }
                    }

                    if (countAccuracy != ACCURANCY)
                    {
                        if (outString.Length > inString.Length)
                        {
                            result = result + "1";
                            outString = outString.Substring(1);
                        }
                        else
                        {
                            result = result + "0";
                        }

                    }
                    inString = outString;
                }
            }
            catch (Exception ex)
            {
                //throw new FCCoreException();
                throw new Exception("Func [convert10to2FPart]:=" + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Converts integer part of number form 10cc to 2cc.Lenght depends only from number
        /// Funcs using: NONE
        /// Vars using : NONE
        /// </summary>
        /// <param name="inString">Input Number to convert</param>
        /// <returns>Returns number integer part in 2cc</returns>
        public static String convert10to2IPart(String inString)
        {
            String result = ""; 	// Результат каждого деления
            String balanse = ""; 	// Массив остатков от каждого деления (исходное число в 2 с/с)
            String balanseTemp = "";// Временная переменная для подсчета остатка
            int activeDividend;		// Текущее делимое
            bool saveMinus = false;
            int i = 0;

            try
            {
                if (inString.IndexOf(',') != -1)
                    inString = inString.Substring(0, inString.IndexOf(','));
                if (inString[0] == '-')
                {
                    saveMinus = true;
                    i++;
                }
                else
                    if (inString[0] == '+')
                        inString = inString.Substring(1);

                while ((inString[i] == '0') && (inString.Length > 1))
                {
                    if (!saveMinus)
                        inString = inString.Substring(1);
                    else
                        inString = "-" + inString.Substring(2);
                }

                result = inString;
                int iRes;
                while (true)
                {	          // цикл по всем делениям (14,7,3,1)
                    /*
                     *        14 |_2_
                     *        14 |7	 |_2_
                     *		  --  6  |3  |_2_
                     *		   0  --  2  |1  
                     *			  1   --
                     *				  1	
                     *
                     *									balanse=1110
                     *					result=14
                     *					result=7
                     *					   ...
                     *					result=1
                     */
                    if (result == "")
                        break;

                    inString = result;
                    result = "";
                    inString = inString + ("0");
                    activeDividend = int.Parse(inString[0].ToString());

                    for (i = 0; i < (inString.Length - 1); i++)
                    { // деление предыдущего результата  
                        switch (activeDividend)
                        {
                            case 0:
                                {
                                    result = result + ("0");
                                    break;
                                }
                            case 1:
                                {
                                    if (i != 0)
                                        result = result + ("0");
                                    if (i == (inString.Length - 2))
                                        balanseTemp = "1";
                                    break;
                                }
                            default:
                                {
                                    iRes = activeDividend / 2;
                                    result = result + ((iRes));
                                    activeDividend %= 2;
                                    balanseTemp = activeDividend.ToString();
                                    break;
                                }
                        }
                        if ((activeDividend != 0) || (inString[i + 1] != '0'))
                            activeDividend = int.Parse((activeDividend).ToString() + inString[i + 1].ToString());
                    }
                    balanse = balanseTemp + (balanse);

                    if (result.Length == 1)//  Выход из цикла 
                    {
                        int iTemp = int.Parse(result);	//  когда результат=1, или =0
                        if ((iTemp == 0) || (iTemp == 1))				//
                            break;								//
                    }
                }
                balanse = result + (balanse);
            }
            catch (Exception ex)
            {
                //throw new FCCoreException();
                throw new Exception("Func [convert10to2IPart]:=" + ex.Message);
            }
            return balanse;
        }

        /// <summary>
        /// Finds if string consist only of '0'
        /// </summary>
        /// <param name="inStr">String to check</param>
        /// <returns>True - if consist; False - if there is one or more symbols alike '0'</returns>
        public bool isStringZero(String inStr)
        {
            int i;

            if (inStr != null)
            {
                for (i = 1; i < 10; i++)
                {
                    if (inStr.Contains(i.ToString()))
                        return false;
                }
                return true;
            }
            else
            {
                throw new FCCoreGeneralException("Func 'isStringZero' = [ Input String is Null. ]");
            }
        }

        //----------------------   Arithmetics  END
        //----------------------   String Utils  BEGIN

        /// <summary>
        /// Compares two number in 10cc. Integer and Float Parts must be compare SEPARATLY !
        /// Using : StringUtil.cs::deleteZeroFromNumber()
        /// </summary>
        /// <param name="A">Input String to compare, first Operand</param>
        /// <param name="B">Input String to compare, second Operand</param>
        /// <returns>0 - A equal B; 1 - A greater than B; 2 - A less than B</returns>
        public static int compare(String A, String B)
        {
            int i, offsetA, offsetB;

            try
            {
                if ((A[0] == '-') && (B[0] != '-'))
                    return 2;

                if ((B[0] == '-') && (A[0] != '-'))
                    return 1;

                if (A.IndexOf(',') == -1)
                    A += ",0";
                offsetA = A.Length - A.IndexOf(',') - 1;

                if (B.IndexOf(',') == -1)
                    B += ",0";
                offsetB = B.Length - B.IndexOf(',') - 1;

                int offset = 0;

                if (offsetA >= offsetB)
                    offset = offsetA;
                else
                    offset = offsetB;

                for (i = 0; i < offset; i++)
                {
                    if (offsetA <= 0)
                        A += "0";
                    if (offsetB <= 0)
                        B += "0";
                    offsetA--;
                    offsetB--;
                }

                A = deleteZeroFromNumber(A);
                B = deleteZeroFromNumber(B);

                if (A.Length > B.Length)
                    return 1;
                else
                    if (B.Length > A.Length)
                        return 2;
                    else
                    {
                        i = 0;
                        while (i != A.Length)
                        {
                            if (A[i] == B[i])
                            {
                                i++;
                                continue;
                            }
                            if (A[i] != B[i])
                            {
                                if (A[i] > B[i])
                                    return 1;
                                else
                                    if (B[i] > A[i])
                                        return 2;
                            }
                        }
                        if (i == A.Length)
                            return 0;
                        return 0;
                    }
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'compare' = [ "+ex.Message+" ]");
            }

        }

        /// <summary>
        /// Trims the number from symbol '0'
        /// </summary>
        /// <param name="inputStr">Входная строка для обработки</param>
        /// <returns>Возвращает строку без избыточных нулей вначале и конце числа</returns>
        public static String deleteZeroFromNumber(String inputStr)
        {
            // удаление 0 в начале числа
            String outStr = "";
            char[] trimparams = { '0' };
            int i = 0, z = 0, k = 0; 
            //inputStr.TrimStart('0');
            //inputStr.TrimEnd('0');
            try
            {
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
                }
                else
                    return "0,0";
            }
            catch (Exception ex)
            {
                throw new FCCoreGeneralException("Func 'deleteZeroFromNumber' = [ "+ex.Message+" ]");
            }
        }

        //----------------------   String Utils  END

        // TO DO - EXPONENTA CAN BE LEFT OR RIGHT PART of Number 
        public void sumExp(Number inNumber, String inStr)
        {
            String E;
            int iE, Offset = 0;
            E = inNumber.Exponenta; // It can be Fraction or Interval
            switch (NumberFormat)
            {
                case 0: Offset = inNumber.Offset; break;
                case 1:
                case 2: Offset = inNumber.OffsetFI; break;
                case 3: Offset = inNumber.OffsetTetra; break;
                case 4: Offset = inNumber.OffsetFITetra; break;
            }
            E = convert2to10IPart(E);
            iE = int.Parse(E) - Offset;
            // Check if inStr > E.Max

            iE += int.Parse(inStr); // This Addition is UNSECURE !
            // Check if iE > E.Max
            E = convert10to2IPart((iE + Offset).ToString());
            inNumber.Exponenta = E;
        }

        /// <summary>
        /// Calculates Exponent for specified number
        /// Uses : Number.BinaryIntPart,Number.BinaryFloatPart
        /// </summary>
        /// <param name="inNumber">Number - var from which exponenta need to be taken</param>
        /// <param name="Left_Right">False - Left part og number, else - Right </param>
        /// <returns>Returns Exponent in 2cc</returns>
        public String selectExp(Number inNumber, PartOfNumber Left_Right)
        {

            int z = 0;
            int Offset = 0;
            String temp, result = "";
            String bynaryStringInt = "", bynaryStringFloat = "";
            if (this.NumberFormat == 0)
            {
                bynaryStringInt = inNumber.BinaryIntPart;
                bynaryStringFloat = inNumber.BinaryFloatPart;
            }
            else 
            {
                if (Left_Right ==  PartOfNumber.Left)
                {// Left part of number
                    bynaryStringInt = inNumber.BinaryIntPartFILeft;
                    bynaryStringFloat = inNumber.BinaryFloatPartFILeft;
                }
                else
                {// Right part of number
                    bynaryStringInt = inNumber.BinaryIntPartFIRight;
                    bynaryStringFloat = inNumber.BinaryFloatPartFIRight;
                }
            }

            if (bynaryStringInt != null)
            {
                if (bynaryStringInt != "")
                {
                    temp = bynaryStringInt + bynaryStringFloat;
                }
                else
                {
                    inNumber.CalcStatus = Flexible_computing.CalculationStatus.Exception;
                    if (NumberFormat == 0)
                    { inNumber.NumberState = stateOfNumber.error; }
                    else
                        if (Left_Right == PartOfNumber.Left)
                        { inNumber.NumberState = stateOfNumber.error; }
                        else
                        { inNumber.NumberStateRight = stateOfNumber.error; }
                    throw new FCCoreArithmeticException("Exception in Func ['selectExp'] Mess=[ Empty String - BynaryIntPart ] (" + inNumber.Name + ")");
                }
            }
            else
            {
                inNumber.CalcStatus = Flexible_computing.CalculationStatus.Exception;

                if (NumberFormat == 0)
                { inNumber.NumberState = stateOfNumber.error; }
                else
                    if (Left_Right == PartOfNumber.Left)
                    { inNumber.NumberState = stateOfNumber.error; }
                    else
                    { inNumber.NumberStateRight = stateOfNumber.error; }
                throw new FCCoreArithmeticException("Exception in Func ['selectExp'] Mess=[ Null - BynaryIntPart ] (" + inNumber.Name + ")");
            }
            try
            {
                switch (NumberFormat)
                {
                    case 0: Offset = inNumber.Offset; break;
                    case 1:
                    case 2: Offset = inNumber.OffsetFI; break;
                    case 3: Offset = inNumber.OffsetTetra; break;
                    case 4: Offset = inNumber.OffsetFITetra; break;
                }
                if (bynaryStringInt.IndexOf('1') != -1)
                {
                    temp = bynaryStringInt;
                    temp = temp.TrimStart('0');
                    z = temp.Length - (temp.IndexOf('1') + 1);

                    if (z > Offset)
                        z = Offset;
                }
                else
                    if (bynaryStringFloat.IndexOf('1') != -1)
                    {
                        temp = bynaryStringFloat;
                        temp = temp.TrimEnd('0');
                        z = (temp.IndexOf('1') + 1);
                        z *= -1;
                        if (z < -Offset)
                            z = -Offset;
                    }
                    else
                    {
                        z = -Offset;
                    }

                result = convert10to2IPart((z + Offset).ToString());
                //inNumber.Exponenta = result;
                return result;
            }
            catch (Exception ex)
            {
                throw new FCCoreFunctionException("Exception in Func ['selectExp'] Mess=[" + ex.Message + "]");
            }
        }

        /// <summary>
        /// Calculates Mantissa for specified number
        /// Uses : Number.BinaryIntPart,Number.BinaryFloatPart
        /// </summary>
        /// <param name="inNumber">Number - var from which mantissa need to be taken</param>
        /// <param name="Left_Right">False - Left part og number, else - Right </param>
        /// <returns>Returns Mantissa in 2cc</returns>
        public String selectMantissa(Number inNumber,int inputStringFormat, PartOfNumber Left_Right)
        {
            int i,l, z = 0;
            int currMBits;
            String result = "";
            String M = "";
            String[] tempArray;
            int offsetDot = 1;
            String Sign;
            String bynaryStringInt = "", bynaryStringFloat = "";

            /* Sign */
            if ((NumberFormat == 0) || (Left_Right == PartOfNumber.Left))
                Sign = SignCharacterLeft;
            else
                Sign = SignCharacterRight;

            if (this.NumberFormat == 0)
            {
                bynaryStringInt = inNumber.BinaryIntPart;
                bynaryStringFloat = inNumber.BinaryFloatPart;
            }
            else
            {
                if (Left_Right == PartOfNumber.Left)
                {// Left part of number
                    bynaryStringInt = inNumber.BinaryIntPartFILeft;
                    bynaryStringFloat = inNumber.BinaryFloatPartFILeft;
                }
                else
                {// Right part of number
                    bynaryStringInt = inNumber.BinaryIntPartFIRight;
                    bynaryStringFloat = inNumber.BinaryFloatPartFIRight;
                }
            }

            try
            {
                if ((bynaryStringInt != null) && (bynaryStringFloat != null))
                {
                    if ((bynaryStringInt != "") && (bynaryStringFloat != ""))
                    {
                        if (bynaryStringInt.IndexOf('1') != -1)
                        {
                            offsetDot = bynaryStringInt.IndexOf('1');
                            result = bynaryStringInt.Substring(offsetDot + 1) + bynaryStringFloat;
                        }
                        else
                            if (bynaryStringFloat.IndexOf('1') != -1)
                                if (isStringZero(inNumber.Exponenta))
                                    result = "" + bynaryStringFloat.Substring(inNumber.Offset - 1, inNumber.MBits + 1);
                                else
                                {
                                    offsetDot = bynaryStringFloat.IndexOf('1') + 1;
                                    result = "" + bynaryStringFloat.Substring(offsetDot);
                                }
                            else
                            {
                                switch (inputStringFormat)
                                {
                                    case 0: currMBits = inNumber.MBits; break;
                                    case 1:
                                    case 2: currMBits = inNumber.MBitsFI; break;
                                    default: currMBits = inNumber.MBits; break;
                                }
                                tempArray = new String[currMBits];
                                for (i = 0; i < currMBits; i++)
                                    tempArray[i] = "0";
                                result = result + String.Join("", tempArray);
                            }
                    }
                    else
                    {
                        throw new FCCoreArithmeticException("Exception in Func ['selectMantissa'] Mess=[ Empty String - BynaryIntPart or BynaryFloatPart  ] (" + inNumber.Name + ")");
                    }
                }
                else
                {
                    throw new FCCoreArithmeticException("Exception in Func ['selectMantissa'] Mess=[ Null - BynaryIntPart or BynaryFloatPart ] (" + inNumber.Name + ")");
                }

                switch (inputStringFormat)
                {
                    case 0: currMBits = inNumber.MBits; break;
                    case 1:
                    case 2: currMBits = inNumber.MBitsFI; break;
                    default: currMBits = inNumber.MBits; break;
                }
                if (result.Length <= currMBits)
                {
                    // After Research Modification HERE NEEDED !
                    l = currMBits + 1 - result.Length;
                    tempArray = new String[l];
                    for (i = 0; i < l;i++)
                    {
                        tempArray[i] = "0";
                    }
                    result = result + String.Join("",tempArray);
                }
                switch (Rounding)
                {
                    case 0:
                        M = result.Substring(0, currMBits);
                        break;
                    case 1:
                        if (isStringZero(inNumber.Exponenta))
                        {
                            tempArray = new String[offsetDot];
                            for (i = 0; i < offsetDot; i++)
                                tempArray[i] = "0";
                            M = M + String.Join("", tempArray);

                            M += result.Substring(0, currMBits + 1 - offsetDot);
                        }
                        else
                            M = result.Substring(0, currMBits + 0);
                        if ((result[currMBits] == '1') && (Sign[0] == '+'))
                        {
                            if (!checkStringFull(M))
                            {
                                M = convert2to10IPart(M);
                                //M = sumIPart(M, "1");
                                M = Addition(M, "1");
                            }
                            else
                            {
                                M = "0";
                                if (checkExpFull(inNumber.Exponenta))
                                {
                                    if (NumberFormat==0)
                                        inNumber.NumberState = stateOfNumber.NaN;
                                    else
                                        inNumber.NumberStateRight = stateOfNumber.NaN;
                                }
                                else
                                {
                                    sumExp(inNumber, "1");
                                }
                            }
                            M = convert10to2IPart(M);
                            if (M.Length + 1 == currMBits)
                            {
                                M = "0" + M;
                            }
                            else
                                if (M.Length < currMBits)
                                {
                                    l = currMBits - M.Length;
                                    tempArray = new String[l];
                                    for (i = 0; i < l; i++)
                                        tempArray[i] = "0";
                                    M = String.Join("", tempArray) + M;
                                }
                        }
                       // inNumber.Mantisa = M;
                        break;

                    case 2:// +Inf 
                        M = result.Substring(1, currMBits);
                            if (Sign[0] == '+')
                            {
                                if (!checkStringFull(M))
                                {
                                    M = convert2to10IPart(M);
                                    //M = sumIPart(M, "1");
                                    M = Addition(M, "1");
                                }
                                else
                                {
                                    M = "0";
                                    if (checkExpFull(inNumber.Exponenta))
                                    {
                                        if (NumberFormat==0)
                                            inNumber.NumberState = stateOfNumber.NaN;
                                        else
                                            inNumber.NumberStateRight = stateOfNumber.NaN;
                                    }
                                    else
                                    {
                                        sumExp(inNumber, "1");
                                    }
                                }
                                M = convert10to2IPart(M);
                            }
                        break;

                    case 3:
                        // -Inf
                        M = result.Substring(1, currMBits);
                        if (Sign[0] == '-')
                        {
                            if (!checkStringFull(M))
                            {
                                M = convert2to10IPart(M);
                                M = Addition(M, "1");
                                //M = sumIPart(M, "1");
                            }
                            else
                            {
                                M = "0";
                                if (checkExpFull(inNumber.Exponenta))
                                {
                                    if (NumberFormat==0)
                                        inNumber.NumberState = stateOfNumber.NaN;
                                    else
                                        inNumber.NumberStateRight = stateOfNumber.NaN;
                                }
                                else
                                {
                                    sumExp(inNumber, "1");
                                }
                            }
                            M = convert10to2IPart(M);
                        }
                        break;
                }
     
                return M;
            }
            catch (Exception ex)
            {
                throw new FCCoreFunctionException("Exception in Func ['selectMantissa'] Mess=[" + ex.Message + "]");
            }
        }

        /// <summary>
        /// Based on Number Exp and Mantisa calculates Correct Value and Error
        /// Uses: Number.E, Number.M
        /// </summary>
        /// <param name="inNumber">Input variable.</param>
        public void calcRes(Number inNumber,PartOfNumber RightPart)
        {

            String M = "", E = "", Mr = "", Er = "", binIPartOut, binFPartOut;
            try
            {
                int z,cycle;
                int Offset = 0;
                int precision = 1900;
                String Sign = "";
                // temp Vars
                stateOfNumber currentState;
                String[] tempArray;
                switch (inNumber.Name)
                {
                    case "Num32":  precision = 1000; break;
                    case "Num64":  precision = 1200; break;
                    case "Num128": precision = 1800; break;
                    case "Num256": precision = 1900; break;
                }
                switch (NumberFormat)
                {
                    case 0: Offset = inNumber.Offset; break;
                    case 1:
                    case 2: Offset = inNumber.OffsetFI; break;
                    case 3: Offset = inNumber.OffsetTetra; break;
                    case 4: Offset = inNumber.OffsetFITetra; break;
                }

                cycle = NumberFormat == 0 ? 1 : 2;
                z = RightPart == PartOfNumber.Left ? 0 : 1;
                //for (z = 0; z < cycle; z++)
                //{
                    if (NumberFormat == 0 || z==0) // Number
                    {
                        M = inNumber.Mantisa;
                        E = inNumber.Exponenta;
                    }
                    else // Fraction or Interval
                    {
                        Mr = inNumber.MantisaRight;
                        Er = inNumber.ExponentaRight;
                    }
                    if (z == 0)
                        currentState = inNumber.NumberState;
                    else
                        currentState = inNumber.NumberStateRight;

                    switch (currentState)
                    {
                        case stateOfNumber.normalized:
                            if (NumberFormat==0)
                                calcResForNorm(inNumber, M, E, Offset, precision, z);
                            else
                            {
                                if (RightPart == PartOfNumber.Left)
                                    calcResForNorm(inNumber, M, E, Offset, precision, z);
                                else
                                    calcResForNorm(inNumber, Mr, Er, Offset, precision, z);
                            }
                          
                            break;

                        case stateOfNumber.denormalized:
                            if (NumberFormat == 0)
                                calcResForDenorm(inNumber, M, E, Offset, precision, z);
                            else
                            {
                                if (RightPart == PartOfNumber.Left)
                                    calcResForDenorm(inNumber, M, E, Offset, precision, z);
                                else
                                    calcResForDenorm(inNumber, Mr, Er, Offset, precision, z);
                            }

                            break;

                        case stateOfNumber.zero:
                            calcResForZero(inNumber,z,cycle);
                            
                            break;

                        default:
                            calcResForNan(inNumber, z, cycle);
                          
                            break;

                    }//switch
                    stateOfNumber tempState = RightPart == PartOfNumber.Right ? inNumber.NumberStateRight : inNumber.NumberState;

                    /* Sign */
                    if (RightPart == PartOfNumber.Left)
                        Sign = SignCharacterLeft;
                    else
                        Sign = SignCharacterRight;

                    if ((tempState == stateOfNumber.normalized) || (tempState == stateOfNumber.denormalized))
                    {
                        switch(NumberFormat)
                        {
                            case 0:
                                inNumber.CorrectResultExp = Sign + convertToExp(inNumber.CorrectResult);
                                inNumber.CorrectResult2ccExp = Sign + convertToExp(inNumber.CorrectResult2cc);
                                break;
                            case 1:
                                if (z == 0)
                                {
                                    inNumber.CorrectResultFractionExpL = Sign + convertToExp(inNumber.CorrectResultFractionL);
                                    inNumber.CorrectResultFraction2ccExpL = Sign + convertToExp(inNumber.CorrectResultFraction2ccL);
                                }
                                else
                                {
                                    inNumber.CorrectResultFractionExpR = Sign + convertToExp(inNumber.CorrectResultFractionR);
                                    inNumber.CorrectResultFraction2ccExpR = Sign + convertToExp(inNumber.CorrectResultFraction2ccR);
                                }
                                break;
                            case 2:
                                if (z == 0)
                                {
                                    inNumber.CorrectResultIntervalExpL = Sign + convertToExp(inNumber.CorrectResultIntervalL);
                                    inNumber.CorrectResultInterval2ccExpL = Sign + convertToExp(inNumber.CorrectResultInterval2ccL);
                                }
                                else
                                {
                                    inNumber.CorrectResultIntervalExpR = Sign + convertToExp(inNumber.CorrectResultIntervalR);
                                    inNumber.CorrectResultInterval2ccExpR = Sign + convertToExp(inNumber.CorrectResultInterval2ccR);
                                }
                                break;
                        }
                    }

                //}// for
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'calcRes' = [ "+ex.Message+" ]");
            }
        }
        public void calcResForNorm(Number inNumber, String M, String E, int Offset, int precision, int z)
        {
            String binIPartOut, binFPartOut, Sign;
            String[] tempArray;
            try
            {
                M = "1" + M;
                E = convert2to10IPart(E);
                int iE = int.Parse(E) - Offset;
                if (iE > M.Length)
                {
                    tempArray = new String[Math.Abs(iE) + 1];
                    for (int i = 0; i <= iE; i++)
                        tempArray[i] = "0";
                    M = M + String.Join("", tempArray);

                }
                if (iE >= 0)
                {
                    if (iE + 1 <= M.Length)
                    {
                        binIPartOut = M.Substring(0, iE + 1);
                        //binFPartOut = "0" + M.Substring(iE + 1);
                        binFPartOut = M.Substring(iE + 1);
                    }
                    else
                    {
                        int temp = M.Length;
                        binIPartOut = M.Substring(0, temp);
                        binFPartOut = M.Substring(temp);
                    }
                }
                else
                {
                    // After Research
                    int max = 0;
                    tempArray = new String[Math.Abs(iE) +1];
                    for (int i = 1; i < Math.Abs(iE); i++) //for (int i = -1; i > iE; i--)
                    {
                        tempArray[max] = "0";
                        max++;
                    }
                    if (max > 0)
                        M = String.Join("", tempArray) + M;
                    else
                        M = "0" + M; // WAS  commented  WARNING

                    binIPartOut = "0";
                    binFPartOut = M;
                }

                /* Sign */
                if ((z == 0)||(NumberFormat == 0))
                    Sign = SignCharacterLeft;
                else
                    Sign = SignCharacterRight;

                switch (NumberFormat)
                {
                    case 0:
                        inNumber.CorrectResult = Sign + convert2to10IPart(binIPartOut) + "," + convert2to10FPart(binFPartOut, precision);
                        inNumber.CorrectResult2cc = Sign + binIPartOut + "," + binFPartOut;
                        break;
                    case 1:
                        if (z == 0)
                        {
                            inNumber.CorrectResultFractionL = Sign + convert2to10IPart(binIPartOut) + "," + convert2to10FPart(binFPartOut, precision);
                            inNumber.CorrectResultFraction2ccL = Sign + binIPartOut + "," + binFPartOut;
                        }
                        else
                        {
                            inNumber.CorrectResultFractionR = Sign + convert2to10IPart(binIPartOut) + "," + convert2to10FPart(binFPartOut, precision);
                            inNumber.CorrectResultFraction2ccR = Sign + binIPartOut + "," + binFPartOut;
                        }
                        break;
                    case 2:
                        if (z == 0)
                        {
                            inNumber.CorrectResultIntervalL = Sign + convert2to10IPart(binIPartOut) + "," + convert2to10FPart(binFPartOut, precision);
                            inNumber.CorrectResultInterval2ccL = Sign + binIPartOut + "," + binFPartOut;
                        }
                        else
                        {
                            inNumber.CorrectResultIntervalR = Sign + convert2to10IPart(binIPartOut) + "," + convert2to10FPart(binFPartOut, precision);
                            inNumber.CorrectResultInterval2ccR = Sign + binIPartOut + "," + binFPartOut;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new FCCoreFunctionException("Func 'calcResForNorm' = [ " + ex.Message + " ]");
            }
            //break;
        }
        public void calcResForDenorm(Number inNumber, String M, String E, int Offset, int precision, int z)
        {
            String[] tempArray;
            String Sign;
            try
            {
                tempArray = new String[Math.Abs(Offset) + 1];
                for (int i = 1; i <= Offset; i++)
                    tempArray[i] = "0";
                M = String.Join("", tempArray) + M;

                /* Sign */
                if ((z == 0) || (NumberFormat == 0))
                    Sign = SignCharacterLeft;
                else
                    Sign = SignCharacterRight;

                switch (NumberFormat)
                {
                    case 0:
                        inNumber.CorrectResult = Sign + "0," + convert2to10FPart(M, precision);
                        inNumber.CorrectResult2cc = Sign + "0," + M;
                        break;
                    case 1:
                        if (z == 0)
                        {
                            inNumber.CorrectResultFractionL = Sign + "0," + convert2to10FPart(M, precision);
                            inNumber.CorrectResultFraction2ccL = Sign + "0," + M;
                        }
                        else
                        {
                            inNumber.CorrectResultFractionR = Sign + "0," + convert2to10FPart(M, precision);
                            inNumber.CorrectResultFraction2ccR = Sign + "0," + M;
                        }
                        break;
                    case 2:
                        if (z == 0)
                        {
                            inNumber.CorrectResultIntervalL = Sign + "0," + convert2to10FPart(M, precision);
                            inNumber.CorrectResultInterval2ccL = Sign + "0," + M;
                        }
                        else
                        {
                            inNumber.CorrectResultIntervalR = Sign + "0," + convert2to10FPart(M, precision);
                            inNumber.CorrectResultInterval2ccR = Sign + "0," + M;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new FCCoreFunctionException("Func 'calcResForDenorm' = [ " + ex.Message + " ]");
            }
        }
        public void calcResForZero(Number inNumber, int z, int cycle)
        {
            try
            {
                String Sign;
                switch (inNumber.Name)
                {
                    case "Num32":
                        Num32.Exponenta = "00000000";
                        Num32.Mantisa = "000000000000000000000"; // 21
                        break;
                    case "Num64":
                        if (z == 0)
                        {
                            Num64.Exponenta = Format == 0 ? "00000000000" : "00000000";
                            Num64.Mantisa = Format == 0 ? "000000000000000000000000000000000000000000000000" : "000000000000000000000"; // 48 - 21
                        }
                        else
                        {
                            Num64.ExponentaRight = Format == 0 ? "00000000000" : "00000000";
                            Num64.MantisaRight = Format == 0 ? "000000000000000000000000000000000000000000000000" : "000000000000000000000"; // 48 - 21
                        }
                        break;
                    case "Num128":
                        if (z == 0)
                        {
                            Num128.Exponenta = Format == 0 ? "000000000000000" : "00000000000";
                            Num128.Mantisa = Format == 0 ? "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" : "000000000000000000000000000000000000000000000000";  // 104 - 48
                        }
                        else
                        {
                            Num128.ExponentaRight = Format == 0 ? "000000000000000" : "00000000000";
                            Num128.MantisaRight = Format == 0 ? "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" : "000000000000000000000000000000000000000000000000";  // 104 - 48
                        }
                        break;
                    case "Num256":
                        if (z == 0)
                        {
                            Num256.Exponenta = Format == 0 ? "00000000000000000000" : "000000000000000";
                            Num256.Mantisa = Format == 0 ? "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" : "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"; // 219 - 104
                        }
                        else
                        {
                            Num256.ExponentaRight = Format == 0 ? "00000000000000000000" : "000000000000000";
                            Num256.MantisaRight = Format == 0 ? "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" : "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"; // 219 - 104
                        }
                        break;
                }

                /* Sign */
                if ((z == 0) || (NumberFormat == 0))
                    Sign = SignCharacterLeft;
                else
                    Sign = SignCharacterRight;

                if (NumberFormat == 0)
                {
                    inNumber.CorrectResult = Sign + "0,0";
                    inNumber.CorrectResultExp = Sign + "0,0";
                    inNumber.CorrectResult2cc = Sign + "0,0";
                    inNumber.CorrectResult2ccExp = Sign + "0,0";
                    inNumber.Error = Sign + "0,0";
                }

                if (cycle == 2 && NumberFormat == 1)
                {
                    if (z == 0)
                    {
                        inNumber.CorrectResultFractionL = Sign + "0,0";
                        inNumber.CorrectResultFractionExpL = Sign + "0,0";
                        inNumber.CorrectResultFraction2ccL = Sign + "0,0";
                        inNumber.CorrectResultFraction2ccExpL = Sign + "0,0";
                        inNumber.ErrorFractionLeft = Sign + "0,0";
                    }
                    if (z == 1)
                    {
                        inNumber.CorrectResultFractionR = Sign + "0,0";
                        inNumber.CorrectResultFractionExpR = Sign + "0,0";
                        inNumber.CorrectResultFraction2ccR = Sign + "0,0";
                        inNumber.CorrectResultFraction2ccExpR = Sign + "0,0";
                        inNumber.ErrorIntervalRight = Sign + "0,0";
                    }
                }

                if (cycle == 2 && NumberFormat == 2)
                {
                    if (z == 0)
                    {
                        inNumber.CorrectResultIntervalL = Sign + "0,0";
                        inNumber.CorrectResultIntervalExpL = Sign + "0,0";
                        inNumber.CorrectResultInterval2ccL = Sign + "0,0";
                        inNumber.CorrectResultInterval2ccExpL = Sign + "0,0";
                        inNumber.ErrorIntervalLeft = Sign + "0,0";
                    }
                    if (z == 1)
                    {
                        inNumber.CorrectResultIntervalR = Sign + "0,0";
                        inNumber.CorrectResultIntervalExpR = Sign + "0,0";
                        inNumber.CorrectResultInterval2ccR = Sign + "0,0";
                        inNumber.CorrectResultInterval2ccExpR = Sign + "0,0";
                        inNumber.ErrorIntervalRight = Sign + "0,0";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FCCoreFunctionException("Func 'calcResForZero' = [ " + ex.Message + " ]");
            }
        }
        public void calcResForInf(Number inNumber, String M, String E, int Offset, int precision, int z)
        {
            String Sign;
            try
            {
                /* Sign */
                if ((z == 0) || (NumberFormat == 0))
                    Sign = SignCharacterLeft;
                else
                    Sign = SignCharacterRight;
                inNumber.CorrectResult = DenormalizedNumber;
                inNumber.CorrectResultExp = NormalizedNumber;

                inNumber.CorrectResult2cc = Sign + convert10to2IPart(DenormIntPart) + "," + convert10to2FPart(DenormFloatPart);
                inNumber.CorrectResult2ccExp = Sign + convertToExp(convert10to2IPart(DenormIntPart) + "," + convert10to2FPart(DenormFloatPart));

                inNumber.Error = Sign + "0,0";
            }
            catch (Exception ex)
            {
                throw new FCCoreFunctionException("Func 'calcResForInf' = [ " + ex.Message + " ]");
            }

        }
        public void calcResForNan(Number inNumber, int z, int cycle)
        {
            try
            {
                if (NumberFormat == 0)
                {
                    inNumber.CorrectResult = "Невозможно представить в данном формате";
                    inNumber.CorrectResultExp = "Невозможно представить в данном формате";
                    inNumber.CorrectResult2cc = "Невозможно представить в данном формате";
                    inNumber.CorrectResult2ccExp = "Невозможно представить в данном формате";
                    inNumber.Error = "Невозможно представить в данном формате";
                }

                if (cycle == 2 && NumberFormat == 1)
                {
                    if (z == 0)
                    {
                        inNumber.CorrectResultFractionL = "Невозможно представить в данном формате";
                        inNumber.CorrectResultFractionExpL = "Невозможно представить в данном формате";
                        inNumber.CorrectResultFraction2ccL = "Невозможно представить в данном формате";
                        inNumber.CorrectResultFraction2ccExpL = "Невозможно представить в данном формате";
                        inNumber.ErrorFractionLeft = "Невозможно представить в данном формате";
                    }
                    if (z == 1)
                    {
                        inNumber.CorrectResultFractionR = "Невозможно представить в данном формате";
                        inNumber.CorrectResultFractionExpR = "Невозможно представить в данном формате";
                        inNumber.CorrectResultFraction2ccR = "Невозможно представить в данном формате";
                        inNumber.CorrectResultFraction2ccExpR = "Невозможно представить в данном формате";
                        inNumber.ErrorFractionRight = "Невозможно представить в данном формате";
                    }
                }


                if (cycle == 2 && NumberFormat == 2)
                {
                    if (z == 0)
                    {
                        inNumber.CorrectResultIntervalL = "Невозможно представить в данном формате";
                        inNumber.CorrectResultIntervalExpL = "Невозможно представить в данном формате";
                        inNumber.CorrectResultInterval2ccL = "Невозможно представить в данном формате";
                        inNumber.CorrectResultInterval2ccExpL = "Невозможно представить в данном формате";
                        inNumber.ErrorIntervalLeft = "Невозможно представить в данном формате";
                    }
                    if (z == 1)
                    {
                        inNumber.CorrectResultIntervalR = "Невозможно представить в данном формате";
                        inNumber.CorrectResultIntervalExpR = "Невозможно представить в данном формате";
                        inNumber.CorrectResultInterval2ccR = "Невозможно представить в данном формате";
                        inNumber.CorrectResultInterval2ccExpR = "Невозможно представить в данном формате";
                        inNumber.ErrorIntervalRight = "Невозможно представить в данном формате";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FCCoreFunctionException("Func 'calcResForNan' = [ " + ex.Message + " ]");
            }
        }

        /// <summary>
        /// Calculates Error based on input and correct numbers.
        /// </summary>
        /// <param name="inNumber">User Number</param>
        public void calcError(Number inNumber, PartOfNumber RightPart)
        {
            try
            {
                int z,cycle;
 
                cycle = NumberFormat == 0 ? 1 : 2;
                z = RightPart == PartOfNumber.Left ? 0 : 1;
                //for (z = 0; z < cycle; z++)
                //{
                    if ((inNumber.NumberState != stateOfNumber.error && NumberFormat == 0) || (NumberFormat != 0 && inNumber.NumberStateRight != stateOfNumber.error))
                    {
                        String tempInput = "";// DenormalizedNumber; 
                        if (z == 0)
                            tempInput = inNumber.Denormalized;
                        else
                            tempInput = inNumber.DenormalizedRight;

                        if ((tempInput[0] == '+') || (tempInput[0] == '-'))
                            tempInput = tempInput.Substring(1);

                        String temp2;
                        switch (NumberFormat)
                        {
                            case 0:
                                if (inNumber.CorrectResult[0] == '+')
                                    temp2 = inNumber.CorrectResult.Substring(1);
                                else
                                    temp2 = inNumber.CorrectResult;

                                inNumber.Error = convertToExp(Subtraction(tempInput, temp2));
                                break;
                            case 1:
                                if (z == 0)
                                {
                                    if (inNumber.CorrectResultFractionL[0] == '+')
                                        temp2 = inNumber.CorrectResultFractionL.Substring(1);
                                    else
                                        temp2 = inNumber.CorrectResultFractionL;

                                    inNumber.ErrorFractionLeft = convertToExp(Subtraction(tempInput, temp2)) ;
                                }
                                else
                                {
                                    if (inNumber.CorrectResultFractionR[0] == '+')
                                        temp2 = inNumber.CorrectResultFractionR.Substring(1);
                                    else
                                        temp2 = inNumber.CorrectResultFractionR;

                                    inNumber.ErrorFractionRight = convertToExp(Subtraction(tempInput, temp2));
                                }
                                break;
                            case 2:
                                if (z == 0)
                                {
                                    if (inNumber.CorrectResultIntervalL[0] == '+')
                                        temp2 = inNumber.CorrectResultIntervalL.Substring(1);
                                    else
                                        temp2 = inNumber.CorrectResultIntervalL;

                                    inNumber.ErrorIntervalLeft = convertToExp(Subtraction(tempInput, temp2)) ;
                                }
                                else
                                {
                                    if (inNumber.CorrectResultIntervalR[0] == '+')
                                        temp2 = inNumber.CorrectResultIntervalR.Substring(1);
                                    else
                                        temp2 = inNumber.CorrectResultIntervalR;

                                    inNumber.ErrorIntervalRight = convertToExp(Subtraction(tempInput, temp2));
                                }
                                break;
                        }
                    }
                //}// for
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'calcError' = [ "+ex.Message+" ]");
            }
        }
        //----------------------   NOT TESTED FUNCS BEGIN

        public String convert2to10IPart(String inString)
        {
            /*
             * Перевод целой части числа из 2 с/с в 10 с/с
             */
            String result = "0";
            String tempRes = "0";
            String factor = "1"; // множитель=степени 2
            try
            {
                if (isStringZero(inString))
                {
                    return "0";
                }
                if ((inString == "0") || (inString == ""))
                    return "0";
                for (int i = inString.Length; i > 0; i--)
                {
                    if (inString[i - 1] == '1')
                        tempRes = Addition(result, factor);
                    result = tempRes;
                    factor = Multiplication(factor, "2");
                }

                if ((result != "0") && (result != ""))
                {
                    result = result.Substring(1);
                    if (result.IndexOf(',') != -1)
                        return result.Substring(0, result.IndexOf(','));
                    else
                        return result;
                }
                else
                    return "0";
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'convert2to10IPart' = [ " + ex.Message + " ]");
            }
        }
        public String convert2to10FPart(String inString, int Precision)
        {
            /*
             * Перевод дробной части числа из 2 с/с в 10 с/с
             */
            String result = "0";
            String divider = "0,5"; // делитель=степени 2

            if (isStringZero(inString))
            {
                return "0";
            }
            try
            {
                for (int i = 0; i < inString.Length; i++)
                {
                    if (inString[i] == '1')
                    {
                        divider = DevideBy2(i + 1);
                        result = Addition(result, divider);
                    }
                   // divider = Devision(divider, "2", Precision);
                }
                return result.Substring(result.IndexOf(',') + 1, result.Length - result.IndexOf(',') - 1);
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'convert2to10FPart' = [ " + ex.Message + " ]");
            }
        }
        public String convertToExp(String inputStr)
        {
            String currentValue;
            String Exp = "";
            String signExp;
            try
            {

                if (inputStr.IndexOf('e') != -1)
                {
                    currentValue = inputStr;
                    Exp = inputStr.Substring(inputStr.IndexOf('e') + 1);
                    inputStr = inputStr.Substring(0, inputStr.IndexOf('e'));
                }
                if ((inputStr[0] != '-') && (inputStr[0] != '+'))
                    inputStr = "+" + inputStr;

                if (isStringZero(inputStr) == true)
                    return inputStr.Substring(1, 3);

                String outString = "";
                String signTemp = "+";

                if ((inputStr[1] == '0') && (inputStr[2] == ','))
                {

                    int offset = 0;
                    for (int i = 3; i < inputStr.Length; i++)
                        if (inputStr[i] != '0')
                        {
                            offset = i;
                            break;
                        }
                    if (inputStr.Length == offset + 1)
                        inputStr += "0";

                    String temp1, temp2, temp3;
                    temp1 = inputStr.Substring(offset, 1);
                    temp2 = inputStr.Substring(offset + 1);
                    temp3 = (offset - 2).ToString();
                    //outString = signTemp + temp1 + ","+ temp2 +"e-"+ temp3;
                    if (Exp == "")
                    {
                        if (int.Parse(temp3) > 0)
                            signExp = "-";
                        else
                            signExp = "+";
                        outString = temp1 + "," + temp2 + "e" + signExp + temp3;
                    }
                    else
                    {
                        int res = int.Parse("-" + temp3) + int.Parse(Exp);
                        if (res >= 0)
                            outString = temp1 + "," + temp2 + "e+" + res.ToString();
                        else
                            outString = temp1 + "," + temp2 + "e" + res.ToString();
                    }
                }
                else
                {
                    int offset = inputStr.IndexOf(',') - 2;
                    if (Exp != "")
                        offset += int.Parse(Exp);
                    if (offset >= 0)
                        signExp = "+";
                    else
                        signExp = "";
                    inputStr = inputStr.Replace(",", "");
                    outString = inputStr.Substring(0, 2) + "," + inputStr.Substring(2) + "e" + signExp + offset.ToString();
                    outString = outString.Substring(1, outString.Length - 1);
                }
                return outString;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'convertToExp' = [ " + ex.Message + " ]");
            }
        }
        
        public String convertToExp2(String inputStr)
        {
            String currentValue;
            String Exp = "";
            String signExp;
            try
            {

                if (inputStr.IndexOf('e') != -1)
                {
                    currentValue = inputStr;
                    Exp = inputStr.Substring(inputStr.IndexOf('e') + 1);
                    inputStr = inputStr.Substring(0, inputStr.IndexOf('e'));
                }
                if ((inputStr[0] != '-') && (inputStr[0] != '+'))
                    inputStr = "+" + inputStr;

                if (inputStr.CompareTo("+0,0") == 0)
                    return inputStr.Substring(1, 3);

                String outString = "";
                String signTemp = "+";

                if ((inputStr[1] == '0') && (inputStr[2] == ','))
                {

                    int offset = 0;
                    for (int i = 3; i < inputStr.Length; i++)
                        if (inputStr[i] != '0')
                        {
                            offset = i;
                            break;
                        }
                    if (inputStr.Length == offset + 1)
                        inputStr += "0";

                    String temp1, temp2, temp3;
                    temp1 = inputStr.Substring(offset, 1);
                    temp2 = inputStr.Substring(offset + 1);
                    temp3 = (offset - 2).ToString();
                    //outString = signTemp + temp1 + ","+ temp2 +"e-"+ temp3;
                    if (Exp == "")
                    {
                        if (int.Parse(temp3) > 0)
                            signExp = "-";
                        else
                            signExp = "+";
                        outString = temp1 + "," + temp2 + "e" + signExp + temp3;
                    }
                    else
                    {
                        int res = int.Parse("-" + temp3) + int.Parse(Exp);
                        if (res >= 0)
                            outString = temp1 + "," + temp2 + "e+" + res.ToString();
                        else
                            outString = temp1 + "," + temp2 + "e" + res.ToString();
                    }
                }
                else
                {
                    int offset = inputStr.IndexOf(',') - 2;
                    if (Exp != "")
                        offset += int.Parse(Exp);
                    if (offset >= 0)
                        signExp = "+";
                    else
                        signExp = "";
                    inputStr = inputStr.Replace(",", "");
                    outString = inputStr.Substring(0, 2) + "," + inputStr.Substring(2) + "e" + signExp + offset.ToString();
                    outString = outString.Substring(1, outString.Length - 1);
                }
                return outString;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'convertToExp' = [ " + ex.Message + " ]");
            }
        }
     
        //----------------------   NOT TESTED FUNCS END

        //----------------------   TESTED 

        public String deleteZeroFromNumberV2(String inStr)
        {
            String result = "";
            String sign = "";
            String LeftPart;
            String RightPart;
            if ((inStr[0] == '-') || (inStr[0] == '+'))
            {
                sign = inStr.Substring(0, 1);
                inStr = inStr.Substring(1);
            }

            if (inStr.IndexOf(",") != -1)
            {
                LeftPart = inStr.Substring(0, inStr.IndexOf(","));
                RightPart = inStr.Substring(inStr.IndexOf(",") + 1);
                if (LeftPart.Length > 1)
                    LeftPart = LeftPart.TrimStart('0');
                if (RightPart.Length > 1)
                    RightPart = RightPart.TrimEnd('0');
                if (LeftPart == "") LeftPart = "0";
                if (RightPart == "") RightPart = "0";
                result = sign + LeftPart + "," + RightPart;
            }
            else
            {
                LeftPart = inStr.Substring(0);
                result = sign + LeftPart + ",0";
            }

            return result;
        }

        /// <summary>
        /// Devides devident A on devider B 
        /// </summary>
        /// <param name="Devident">Number which is devided.</param>
        /// <param name="Devider">Number which is devides.</param>
        /// <returns>Quotient</returns>
        public String Devision(String Devident, String Devider,int Precision)
        {
            String SignResult = "", Result = "";
            String iPartResult = "", fPartResult = "";
            String iPartDevident = "", fPartDevident = "";
            String iPartDevider = "", fPartDevider = "";
            int res;
            int.TryParse(Devident, out res);
            try
            {
                if (isStringZero(Devident))
                {
                    return "+0,0";
                }
                if (isStringZero(Devider))
                {
                    throw new FCCoreArithmeticException("Func 'Devision' = [ Division by Zero ]");
                }
                //Temporary Vars
                String signDevident = "", signDevider = "";
                int LenFloatPartDevident = 0, LenFloatPartDevider = 0;
                int i, z;
                int DeviderDot = 0;
                int DevidentDot = 0;
                char[] chArr = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                BigInteger BigDevident = 0;
                BigInteger BigDevider = 0;
                int precision = Precision;

                /* Sign Result */
                if ((Devident[0] == '-') || (Devident[0] == '+'))
                {
                    signDevident = Devident.Substring(0, 1);
                    Devident = Devident.Substring(1);  // Devident without sign = 123123,123
                }
                else
                    signDevident = "+";

                if ((Devider[0] == '-') || (Devider[0] == '+'))
                {
                    signDevider = Devider.Substring(0, 1);
                    Devider = Devider.Substring(1);  // Devider without sign = 789789,789
                }
                else
                    signDevider = "+";

                if (signDevident == "+")
                {
                    if (signDevider == "+")
                        SignResult = "+";
                    else
                        SignResult = "-";
                }
                else // if signDevident == "-"
                {
                    if (signDevider == "+")
                        SignResult = "-";
                    else
                        SignResult = "+";
                }

                /*Float Part*/
                DevidentDot = Devident.IndexOf(",");
                DeviderDot = Devider.IndexOf(",");

                if (DevidentDot != -1)
                {
                    LenFloatPartDevident = Devident.Length - DevidentDot - 1;    // -1 - ','
                }
                else
                {
                    LenFloatPartDevident = 0; fPartDevident = ""; iPartDevident = Devident;
                }

                if (DeviderDot != -1)
                {
                    LenFloatPartDevider = Devider.Length - DeviderDot - 1;      // -1 - ','
                }
                else
                {
                    LenFloatPartDevider = 0; fPartDevider = ""; iPartDevider = Devider;
                }

                if (LenFloatPartDevident > LenFloatPartDevider)
                {
                    for (i = 0; i < LenFloatPartDevident - LenFloatPartDevider; i++)// After Research Modification NEEDED !
                    {
                        Devider += "0";
                    }
                }
                else
                {
                    if (LenFloatPartDevident < LenFloatPartDevider)
                        for (i = 0; i < LenFloatPartDevider - LenFloatPartDevident; i++)// After Research Modification NEEDED !
                        {
                            Devident += "0";
                        }
                }

                Devider = Devider.Replace(",", "");
                Devident = Devident.Replace(",", "");
                //iPartDevident += fPartDevident; // 123123
                //iPartDevider += fPartDevider;   // 678678


                i = iPartDevident.IndexOfAny(chArr);
                if ((i != -1) && (i != 0))
                {
                    for (z = 0; z < i; z++) // After Research Modification NEEDED !
                    {
                        iPartDevider += "0";
                    }
                    iPartDevident = iPartDevident.Substring(i);
                }

                i = iPartDevider.IndexOfAny(chArr);
                if ((i != -1) && (i != 0))
                {
                    for (z = 0; z < i; z++) // After Research Modification NEEDED !
                    {
                        iPartDevident += "0";
                    }
                    iPartDevider = iPartDevider.Substring(i);
                }

                BigDevident = BigInteger.Parse(Devident);
                BigDevider = BigInteger.Parse(Devider);

                Result = SignResult + divV2(BigDevident, BigDevider, precision);
                return Result;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'Devision' = [ " + ex.Message + " ]");
            }
        }

        private String divV2(BigInteger Divident, BigInteger Divider, int Precision)
        {
            String result = "";
            int counter = 0;
            BigInteger BigResult;
            BigInteger reminder;
            while (counter < Precision) // After Research Modification NEEDED !
            {
                BigResult = BigInteger.DivRem(Divident, Divider, out reminder);
                if (counter == 0)
                    result += BigResult.ToString() + ",";
                else
                    result += BigResult.ToString();
                if (reminder.Equals(0))
                    break;
                counter++;
                Divident = reminder * 10;
            }
            return result;
        }

        private String mulV2(BigInteger Multiplicator, BigInteger Factor)
        {
            BigInteger Result = Multiplicator * Factor;
            return Result.ToString();
        }

        /// <summary>
        /// Multiplies two float digits.
        /// Permissions: Input Numbers can be with sign's or without
        /// Warnings: Can't be in exponential form.
        /// </summary>
        /// <param name="Multiplicand">Number to multiply</param>
        /// <param name="Factor">Number wich is multiplied</param>
        /// <returns></returns>
        public String Multiplication(String Multiplicator, String Factor)
        {
            String SignResult = "", Result = "";
            String iPartMultiplicator = "", fPartMultiplicator = "";
            String iPartFactor = "", fPartFactor = "";

            //Temporary Vars
            String signMultiplicator = "", signFactor = "";
            int LenFloatPartMultiplicator = 0, LenFloatPartFactor = 0;
            int i, z;
            int MultiplicatorDot = 0;
            int FactorDot = 0;
            int MultiplicatorExp = 0, FactorExp = 0;
            char[] chArr = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            BigInteger BigMultiplicator = 0;
            BigInteger BigFactor = 0;

            try
            {
                if (isStringZero(Multiplicator) || isStringZero(Factor))
                {
                    return "+0,0";
                }

                /* Sign Result */
                if ((Multiplicator[0] == '-') || (Multiplicator[0] == '+'))
                {
                    signMultiplicator = Multiplicator.Substring(0, 1);
                    Multiplicator = Multiplicator.Substring(1);  // Devident without sign = 123123,123
                }
                else
                    signMultiplicator = "+";

                if ((Factor[0] == '-') || (Factor[0] == '+'))
                {
                    signFactor = Factor.Substring(0, 1);
                    Factor = Factor.Substring(1);  // Devider without sign = 789789,789
                }
                else
                    signFactor = "+";

                if (signMultiplicator == "+")
                {
                    if (signFactor == "+")
                        SignResult = "+";
                    else
                        SignResult = "-";
                }
                else // if signDevident == "-"
                {
                    if (signFactor == "+")
                        SignResult = "-";
                    else
                        SignResult = "+";
                }

                /*Float Part*/
                MultiplicatorDot = Multiplicator.IndexOf(",");
                FactorDot = Factor.IndexOf(",");

                if (MultiplicatorDot != -1)
                {
                    LenFloatPartMultiplicator = Multiplicator.Length - MultiplicatorDot - 1;    // -1 - ','
                    fPartMultiplicator = Multiplicator.Substring(MultiplicatorDot + 1);  // copy float part Devident //  123
                    iPartMultiplicator = Multiplicator.Substring(0, MultiplicatorDot);
                }
                else
                { LenFloatPartMultiplicator = 1; fPartMultiplicator = "0"; iPartMultiplicator = Multiplicator; }

                if (FactorDot != -1)
                {
                    LenFloatPartFactor = Factor.Length - FactorDot - 1;      // -1 - ','
                    fPartFactor = Factor.Substring(FactorDot + 1);  // copy float part Devider // 789
                    iPartFactor = Factor.Substring(0, FactorDot);
                }
                else
                { LenFloatPartFactor = 1; fPartFactor = "0"; iPartFactor = Factor; }

                MultiplicatorExp = -LenFloatPartMultiplicator;
                FactorExp = -LenFloatPartFactor;

                iPartMultiplicator += fPartMultiplicator; // 123123
                iPartFactor += fPartFactor;   // 678678

                BigMultiplicator = BigInteger.Parse(iPartMultiplicator);
                BigFactor = BigInteger.Parse(iPartFactor);

                Result = mulV2(BigMultiplicator, BigFactor);
                if (LenFloatPartFactor + LenFloatPartMultiplicator >= Result.Length)
                {
                    z = Result.Length;
                    for (i = 0; i < LenFloatPartFactor + LenFloatPartMultiplicator - z + 1; i++)
                    {
                        Result = "0" + Result;
                    }
                }
                Result = SignResult + Result.Insert(Result.Length - Math.Abs(LenFloatPartMultiplicator + LenFloatPartFactor), ",");
                return deleteZeroFromNumberV2(Result);
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'Multiplication' = [ " + ex.Message + " ]");
            }
        }

        /// <summary>
        /// Adds Operand1 to Operand2
        /// Permissions: Input Numbers can be with sign's or without
        /// </summary>
        /// <param name="Operand1">First operand of addition</param>
        /// <param name="Operand2">Second operand of addition</param>
        /// <returns>Number wich is representation an addition of two numbers</returns>
        public String Addition(String Operand1, String Operand2)
        {
            String SignResult = "", Result = "";
            String iPartOperand1 = "", fPartOperand1 = "";
            String iPartOperand2 = "", fPartOperand2 = "";

            //Temporary Vars
            String signOperand1 = "", signOperand2 = "";
            int LenFloatPartOperand1 = 0, LenFloatPartOperand2 = 0;
            int i, z;
            int Operand1Dot = 0;
            int Operand2Dot = 0;
            int Operand1Exp = 0, Operand2Exp = 0;
            char[] chArr = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            BigInteger BigOperand1 = 0;
            BigInteger BigOperand2 = 0;
            try
            {
                /* Sign Result */
                if ((Operand1[0] == '-') || (Operand1[0] == '+'))
                {
                    signOperand1 = Operand1.Substring(0, 1);
                    Operand1 = Operand1.Substring(1);  // Devident without sign = 123123,123
                }
                else
                    signOperand1 = "+";

                if ((Operand2[0] == '-') || (Operand2[0] == '+'))
                {
                    signOperand2 = Operand2.Substring(0, 1);
                    Operand2 = Operand2.Substring(1);  // Devider without sign = 789789,789
                }
                else
                    signOperand2 = "+";

                if (signOperand1 == "+")
                {
                    if (signOperand2 == "+")
                        SignResult = "+";
                    else
                        SignResult = "-";
                }
                else // if signDevident == "-"
                {
                    if (signOperand2 == "+")
                        SignResult = "-";
                    else
                        SignResult = "+";
                }

                if (isStringZero(Operand1) && !isStringZero(Operand2))
                {
                    return signOperand2 + Operand2;
                }
                else
                {
                    if (!isStringZero(Operand1) && isStringZero(Operand2))
                    {
                        return signOperand1 + Operand1;
                    }
                    else
                        if (isStringZero(Operand1) && isStringZero(Operand2))
                            return "+0,0";
                }


                /*Float Part*/
                Operand1Dot = Operand1.IndexOf(",");
                Operand2Dot = Operand2.IndexOf(",");

                if (Operand1Dot != -1)
                {
                    LenFloatPartOperand1 = Operand1.Length - Operand1Dot - 1;    // -1 - ','
                    fPartOperand1 = Operand1.Substring(Operand1Dot + 1);  // copy float part Devident //  123
                    iPartOperand1 = Operand1.Substring(0, Operand1Dot);
                }
                else
                { LenFloatPartOperand1 = 1; fPartOperand1 = "0"; iPartOperand1 = Operand1; }

                if (Operand2Dot != -1)
                {
                    LenFloatPartOperand2 = Operand2.Length - Operand2Dot - 1;      // -1 - ','
                    fPartOperand2 = Operand2.Substring(Operand2Dot + 1);  // copy float part Devider // 789
                    iPartOperand2 = Operand2.Substring(0, Operand2Dot);
                }
                else
                { LenFloatPartOperand2 = 1; fPartOperand2 = "0"; iPartOperand2 = Operand2; }

                if (LenFloatPartOperand1 >= LenFloatPartOperand2)
                {
                    for (i = 0; i < LenFloatPartOperand1 - LenFloatPartOperand2; i++) // After Research Modification NEEDED !
                    {
                        fPartOperand2 += "0";
                    }
                }
                else
                {
                    for (i = 0; i < LenFloatPartOperand2 - LenFloatPartOperand1; i++) // After Research Modification NEEDED !
                    {
                        fPartOperand1 += "0";
                    }
                }

                Operand1Exp = -LenFloatPartOperand1;
                Operand2Exp = -LenFloatPartOperand2;

                iPartOperand1 += fPartOperand1; // 123123
                iPartOperand2 += fPartOperand2;   // 678678

                BigOperand1 = BigInteger.Parse(iPartOperand1);
                BigOperand2 = BigInteger.Parse(iPartOperand2);
                Result = addV2(BigOperand1, BigOperand2);
                if ((LenFloatPartOperand1 >= Result.Length) || (LenFloatPartOperand2 >= Result.Length))
                {
                    z = Result.Length;
                    for (i = 0; i < LenFloatPartOperand2 + LenFloatPartOperand1 - z + 1; i++) // After Research Modification NEEDED !
                    {
                        Result = "0" + Result;
                    }
                }
                if (LenFloatPartOperand1 > LenFloatPartOperand2)
                    Result = SignResult + Result.Insert(Result.Length - Math.Abs(LenFloatPartOperand1), ",");
                else
                    Result = SignResult + Result.Insert(Result.Length - Math.Abs(LenFloatPartOperand2), ",");
                return deleteZeroFromNumberV2(Result);
            }
            catch (Exception ex)
            { throw new FCCoreArithmeticException("Func 'Addition' = [" + ex.Message + "]"); }
        }

        private  String addV2(BigInteger Operand1, BigInteger Operand2)
        {
            BigInteger Result;
            try
            {
                Result = Operand1 + Operand2;
                return Result.ToString();
            }
            catch (Exception ex)
            { throw new FCCoreArithmeticException("Func 'addV2' = [" + ex.Message + "]"); }
        }

        public  String Subtraction(String Operand1, String Operand2)
        {
            String SignResult = "", Result = "";
            String fPartOp1 = "", fPartOp2 = "";
            String iPartOp1 = "", iPartOp2 = "";

            //Temporary Vars
            String signOperand1 = "", signOperand2 = "";
            int LenFractionPartOperand1 = 0, LenFractionPartOperand2 = 0, BiggerLen = 0;
            int i, z, temp;

            int index;
            char[] chArr = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            BigInteger BigOperand1 = 0;
            BigInteger BigOperand2 = 0;
            try
            {
                // 0 - Num
                if ((Operand1 == "0,0") || (Operand1 == "+0,0") || (Operand1 == "0"))
                {
                    if (Operand2[0] == '-')
                        return "+" + Operand2.Substring(1);
                    else // change sign to opposite
                    {
                        if (Operand2[0] == '+') // if string = "+0,0"
                        {
                            Operand2 = "-" + Operand2.Substring(1);
                            if (Operand2.IndexOf(',') != -1)
                                return Operand2;
                            else
                                return Operand2 + ",0";
                        }
                        else // if string = "0,0"
                        {
                            Operand2 = "-" + Operand2;
                            if (Operand2.IndexOf(',') == -1)
                                return Operand2 + ",0";
                        }
                    }
                }

                // Num - 0
                if ((Operand2 == "0,0") || (Operand2 == "+0,0") || (Operand2 == "0"))
                {
                    if ((Operand1[0] != '-') && (Operand1[0] != '+'))
                        Operand1 = "+" + Operand1;
                    if (Operand1.IndexOf(',') != -1)
                        return Operand1;
                    else
                        return Operand1 + ",0";
                }


                if (Operand1[0] == '-')
                {
                    signOperand1 = "-";
                    Operand1 = Operand1.Substring(1);
                }
                else
                {
                    if (Operand1[0] == '+')
                        Operand1 = Operand1.Substring(1);
                    signOperand1 = "+";

                }

                if (Operand2[0] == '-')
                {
                    Operand2 = Operand2.Substring(1);
                    signOperand2 = "-";
                }
                else
                {
                    if (Operand2[0] == '+')
                        Operand2 = Operand2.Substring(1);
                    signOperand2 = "+";
                }

                if (((signOperand1 == "+") && (signOperand2 == "-")) || ((signOperand1 == "-") && (signOperand2 == "+")))
                {
                    if (signOperand2 == "-")
                        Operand2 = Operand2.Substring(1);
                    if (signOperand1 == "-")
                        Operand1 = Operand1.Substring(1);

                    return Addition(Operand1, Operand2);
                }

                //Separating fration and integer part of number
                index = Operand1.IndexOf(',');
                if (index != -1)
                {
                    iPartOp1 = Operand1.Substring(0, index);
                    fPartOp1 = Operand1.Substring(index + 1, Operand1.Length - index - 1);
                    LenFractionPartOperand1 = fPartOp1.Length;
                }
                else
                {
                    iPartOp1 = Operand1;
                    LenFractionPartOperand1 = 0;
                }

                index = Operand2.IndexOf(',');
                if (index != -1)
                {
                    iPartOp2 = Operand2.Substring(0, index);
                    fPartOp2 = Operand2.Substring(index + 1, Operand2.Length - index - 1);
                    LenFractionPartOperand2 = fPartOp2.Length;
                }
                else
                {
                    iPartOp2 = Operand2;
                    LenFractionPartOperand2 = 0;
                }

                 if (LenFractionPartOperand1 < LenFractionPartOperand2)
                {
                    BiggerLen = LenFractionPartOperand2;
                    for (i = 0; i < LenFractionPartOperand2 - LenFractionPartOperand1; i++)
                        Operand1 += "0";
                }
                else
                    if (LenFractionPartOperand1 > LenFractionPartOperand2)
                    {
                        BiggerLen = LenFractionPartOperand1;
                        for (i = 0; i < LenFractionPartOperand1 - LenFractionPartOperand2; i++)
                            Operand2 += "0";
                    }
                    else
                    { BiggerLen = LenFractionPartOperand1; }


                BigOperand1 = BigInteger.Parse(Operand1.Replace(",", ""));
                BigOperand2 = BigInteger.Parse(Operand2.Replace(",", ""));
                Result = subV2(BigOperand1, BigOperand2).ToString();
                if (Result[0] == '-')
                {
                    SignResult = "-";
                    Result = Result.Substring(1);
                }
                else
                    SignResult = "+";
                if (Result.Length == 1)
                {
                    if (BiggerLen == 0)
                        Result += ",0";
                    else
                    {
                        for (i = 0; i < BiggerLen - 1; i++) // After Research Modification NEEDED !
                            Result = "0" + Result;
                        Result = "0," + Result;
                    }
                }
                else
                    if (BiggerLen == 0)
                        Result += ",0";
                    else
                    {
                        index = Result.Length;
                        if (BiggerLen > index)
                        {
                            for (i = 0; i < BiggerLen - index ; i++) // After Research Modification NEEDED !
                                Result = "0" + Result;
                            Result = "0," + Result;
                        }
                        else
                            Result = Result.Substring(0, Result.Length - BiggerLen) + "," + Result.Substring(Result.Length - BiggerLen, BiggerLen);
                    }
                return SignResult + Result;
            }
            catch (Exception ex)
            { throw new FCCoreArithmeticException("Func 'Addition' = [" + ex.Message + "]"); }
        }

        private static String subV2(BigInteger Operand1, BigInteger Operand2)
        {
            BigInteger Result;
            try
            {
                Result = Operand1 - Operand2;
                return Result.ToString();
            }
            catch (Exception ex)
            { throw new FCCoreArithmeticException("Func 'subV2' = [" + ex.Message + "]"); }
        }
        public static String DevideBy2(int Degree)
        {
            BigInteger Devident = 1;
            String Result;
            int Exp = 0;// Onle negative value from -1 to -Inf
            String[] temp;
            int i;
            for (i = 0; i <= Degree; i++)
            {
                Devident *= 5;
                if ((Devident.ToString()[0] == '1') && (i + 1 <= Degree))
                {
                    Exp++;
                }
            }

            temp = new String[Exp];
            for (i = 0; i < Exp; i++)
                temp[i] = "0";
            Result = String.Join("", temp) + Devident.ToString();
            //if (Degree != 0)
                return "+0," + Result;
            //else
            //    return "+" + Result;
        }

        public String convert2to16(String bin)
        {
            String hex = "";

            try
            {
                int iTemp = bin.Length % 4;

                if (iTemp > 0)
                    for (int i = 0; i < 4 - iTemp; i++)
                        bin = "0" + bin;

                for (int i = 0; i < bin.Length; i += 4)
                {
                    String temp = bin.Substring(i, 4);
                    switch (int.Parse(temp))
                    {
                        case 0: hex += "0"; break;
                        case 1: hex += "1"; break;
                        case 10: hex += "2"; break;
                        case 11: hex += "3"; break;
                        case 100: hex += "4"; break;
                        case 101: hex += "5"; break;
                        case 110: hex += "6"; break;
                        case 111: hex += "7"; break;
                        case 1000: hex += "8"; break;
                        case 1001: hex += "9"; break;
                        case 1010: hex += "A"; break;
                        case 1011: hex += "B"; break;
                        case 1100: hex += "C"; break;
                        case 1101: hex += "D"; break;
                        case 1110: hex += "E"; break;
                        case 1111: hex += "F";break;
                    default: break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'convert2to16' = ["+ ex.Message+ "]");
            }

            return hex;
        }

        public String convert16to2(String hex,int bitOutput)
        {
            int i;
            String result="";
            String []tempArray;
            String hex_table = "0123456789ABCDEF";
            String [] bin_table = {"0000","0001","0010","0011",
                                  "0100","0101","0110","0111",
                                  "1000","1001","1010","1011",
                                  "1100","1101","1110","1111"};
            try
            {
                tempArray = new String[hex.Length];

                for (i = 0; i < hex.Length; i++)
                {
                    tempArray[i] = bin_table[hex_table.IndexOf(hex[i])];
                }
                result = String.Join("", tempArray);
                if (result.Length > bitOutput)
                    result = result.Substring(result.Length - bitOutput);
                return result;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'convert16to2(String hex)' =["+ ex.Message +"]");
            }

        }

        public char convert10to16(int inDigit)
        {
            String hex_table = "0123456789ABCDEF";
            if (inDigit > 15)
                return '0';
            else
                return hex_table[inDigit];
        }


        /* Temporary Func's
         public String sumFPart(String a, String b)
        {
            //
            //  Возвращает сумму двух чисел. Все числа представлены в виде строк. 
            //
            int iRes = 0;
            int plusOne = 0;
            String result = "";
            try
            {
                int aLength = a.Length;
                int bLength = b.Length;
                
                //if (this.compare(a, "0") != 0)
                //    for (int i = 0; i <= (bLength - aLength + 2); i++)
                //        a = a + "0";
                

                if (aLength != bLength)
                {
                    if (aLength > bLength)
                        for (int i = 0; i < (aLength - bLength); i++)
                            b = b + "0";
                    else
                        for (int i = 0; i < (bLength - aLength); i++)
                            a = a + "0";
                }


                for (int i = a.Length - 1; i >= 0; i--)
                {
                    iRes = int.Parse(a[i].ToString()) +
                           int.Parse(b[i].ToString()) + plusOne;

                    if (iRes < 10)
                    {
                        result = iRes.ToString() + result;
                        plusOne = 0;
                    }
                    else
                    {
                        result = (iRes - 10).ToString() + result;
                        plusOne = 1;
                    }
                }
                if (plusOne == 1)
                    result = "1" + result;

                // удаление 0 в конце числа
                for (int i = result.Length - 1; i > 0; i--)
                {
                    if (result[i] != '0')
                    {
                        result = result.Substring(0, i + 1);
                        break;
                    }
                }

                if (plusOne == 1)
                {
                    iPart = result.Substring(0, 1);
                    result = result.Substring(1);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'sumFPart' = [ " + ex.Message + " ]");
            }
        }

        public String div2(String inString)
        {
            String result = ""; 	// Результат каждого деления
            int activeDividend;		// Текущее делимое
            int iRes;
            try
            {
                result = "";
                inString = inString + "0";
                activeDividend = int.Parse(inString[0].ToString());

                for (int i = 0; i < (inString.Length - 1); i++)
                { // деление предыдущего результата  
                    switch (activeDividend)
                    {
                        case 0:
                            {
                                result = result + "0";
                                break;
                            }
                        case 1:
                            {
                                if (i != 0)
                                    result = result + "0";
                                break;
                            }
                        default:
                            {
                                iRes = activeDividend / 2;
                                result = result + iRes.ToString();
                                activeDividend %= 2;
                                break;
                            }
                    }
                    if ((activeDividend != 0) || (inString[i + 1] != '0'))
                        activeDividend = int.Parse(activeDividend.ToString() + (inString[i + 1]).ToString());
                }

                if (result.Length < (inString.Length - 1))
                    result = "0" + result;

                return result;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'div2' = [ " + ex.Message + " ]");
            }
        }
        */
    }// FCCore Class

}// namespace

/*
   public String sumFPart(String a, String b)
        {
            //
            //  Возвращает сумму двух чисел. Все числа представлены в виде строк. 
            //
            int iRes = 0;
            int plusOne = 0;
            String result = "";
            try
            {
                int aLength = a.Length;
                int bLength = b.Length;
                /*
                if (this.compare(a, "0") != 0)
                    for (int i = 0; i <= (bLength - aLength + 2); i++)
                        a = a + "0";
                */
/*
                if (aLength != bLength)
                {
                    if (aLength > bLength)
                        for (int i = 0; i < (aLength - bLength); i++)
                            b = b + "0";
                    else
                        for (int i = 0; i < (bLength - aLength); i++)
                            a = a + "0";
                }


                for (int i = a.Length - 1; i >= 0; i--)
                {
                    iRes = int.Parse(a[i].ToString()) +
                           int.Parse(b[i].ToString()) + plusOne;

                    if (iRes < 10)
                    {
                        result = iRes.ToString() + result;
                        plusOne = 0;
                    }
                    else
                    {
                        result = (iRes - 10).ToString() + result;
                        plusOne = 1;
                    }
                }
                if (plusOne == 1)
                    result = "1" + result;

                // удаление 0 в конце числа
                for (int i = result.Length - 1; i > 0; i--)
                {
                    if (result[i] != '0')
                    {
                        result = result.Substring(0, i + 1);
                        break;
                    }
                }

                if (plusOne == 1)
                {
                    iPart = result.Substring(0, 1);
                    result = result.Substring(1);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'sumFPart' = [ " + ex.Message + " ]");
            }
        }

        public String sumIPart(String a, String b)
        {
            //
            //  Возвращает сумму двух чисел. Все числа представлены в виде строк. 
            //
            int iRes = 0;
            int plusOne = 0;
            String result = "";
            try
            {
                int aLength = a.Length;
                int bLength = b.Length;

                if (aLength != bLength)
                {
                    if (aLength > bLength)
                        for (int i = 0; i < (aLength - bLength); i++)
                            b = "0" + b;
                    else
                        for (int i = 0; i < (bLength - aLength); i++)
                            a = "0" + a;
                }

                for (int i = a.Length - 1; i >= 0; i--)
                {
                    iRes = int.Parse(a[i].ToString()) +
                           int.Parse(b[i].ToString()) + plusOne;

                    if (iRes < 10)
                    {
                        result = iRes.ToString() + result;
                        plusOne = 0;
                    }
                    else
                    {
                        result = (iRes - 10).ToString() + result;
                        plusOne = 1;
                    }
                }
                if (plusOne == 1)
                    result = "1" + result;

                return result;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'sumIPart' = [ " + ex.Message + " ]");
            }
        }

        public String mul2(String inString)
        {
            String outString = "";
            int plusOne = 0;
            try
            {
                for (int i = inString.Length; i > 0; i--)
                {
                    int thisNumber = int.Parse(inString[i - 1].ToString());
                    if (thisNumber < 5)
                    {
                        outString = (thisNumber * 2 + plusOne).ToString() + outString;
                        plusOne = 0;
                    }
                    else
                    {
                        outString = (thisNumber * 2 + plusOne - 10).ToString() + outString;
                        plusOne = 1;
                        if (i == 1)
                            outString = "1" + outString;
                    }
                }
                return outString;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'mul2' = [ " + ex.Message + " ]");
            }
        }
        public String div2(String inString)
        {
            String result = ""; 	// Результат каждого деления
            int activeDividend;		// Текущее делимое
            int iRes;
            try
            {
                result = "";
                inString = inString + "0";
                activeDividend = int.Parse(inString[0].ToString());

                for (int i = 0; i < (inString.Length - 1); i++)
                { // деление предыдущего результата  
                    switch (activeDividend)
                    {
                        case 0:
                            {
                                result = result + "0";
                                break;
                            }
                        case 1:
                            {
                                if (i != 0)
                                    result = result + "0";
                                break;
                            }
                        default:
                            {
                                iRes = activeDividend / 2;
                                result = result + iRes.ToString();
                                activeDividend %= 2;
                                break;
                            }
                    }
                    if ((activeDividend != 0) || (inString[i + 1] != '0'))
                        activeDividend = int.Parse(activeDividend.ToString() + (inString[i + 1]).ToString());
                }

                if (result.Length < (inString.Length - 1))
                    result = "0" + result;

                return result;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'div2' = [ " + ex.Message + " ]");
            }
        }

       

        public String sub(String a, String b)
        {
            String iPartA = "0";
            String fPartA = "0";
            String iPartB = "0";
            String fPartB = "0";
            String iPartC = "0";
            String fPartC = "0";
            String sign = "", result = "", PartTemp = "";

            try
            {

                if (a.IndexOf(',') != -1)
                {
                    iPartA = a.Substring(0, a.IndexOf(','));
                    fPartA = a.Substring(a.IndexOf(',') + 1);
                }
                else
                    iPartA = a;

                if (b.IndexOf(',') != -1)
                {
                    iPartB = b.Substring(0, b.IndexOf(','));
                    fPartB = b.Substring(b.IndexOf(',') + 1);
                }
                else
                    iPartB = b;

                if (a[0] == '-')
                {
                    iPartA = iPartA.Substring(1);
                    if (b[0] == '-')
                    {
                        iPartB = iPartB.Substring(1);

                        PartTemp = iPartB;
                        iPartB = iPartA;
                        iPartA = PartTemp;

                        PartTemp = fPartB;
                        fPartB = fPartA;
                        fPartA = PartTemp;
                    }
                    else
                    {
                        fPartC = this.sumFPart(fPartA, fPartB);
                        iPartC = this.sumIPart(iPartA, iPartB);

                        if (this.iPart != null)
                            iPartC = this.sumIPart(iPartC, this.iPart);

                        return "-" + iPartC + "," + fPartC;
                    }
                }
                else
                    if (b[0] == '-')
                    {
                        iPartB = iPartB.Substring(1);
                        fPartC = this.sumFPart(fPartA, fPartB);
                        iPartC = this.sumIPart(iPartA, iPartB);

                        if (this.iPart != null)
                            iPartC = this.sumIPart(iPartC, this.iPart);

                        return iPartC + "," + fPartC;
                    }
                    //was without 'else'
                    else
                    {// 
                        if (compare(iPartA, iPartB) == 1)
                        {
                            fPartC = subFPart(fPartA, fPartB);
                            iPartC = subIPart(iPartA, iPartB);
                        }
                        else
                            if (compare(iPartA, iPartB) == 0)
                            {
                                if ((compare(fPartA, fPartB) == 0) || (compare(fPartA, fPartB) == 2))
                                {
                                    fPartC = subFPart(fPartA, fPartB);
                                    iPartC = subIPart(iPartA, iPartB);
                                }
                                else
                                {
                                    fPartC = subFPart(fPartB, fPartA);
                                    iPartC = subIPart(iPartB, iPartA);
                                }
                            }
                            else
                            {
                                fPartC = subFPart(fPartB, fPartA);
                                iPartC = subIPart(iPartB, iPartA);
                            }

                    }

                if (this.iPart != null)
                    iPartC = subIPart(iPartC, this.iPart);

                if (fPartC[0] == '-')
                {
                    fPartC = fPartC.Substring(1);
                    if (iPartC[0] == '-')
                        sign = "-";
                }
                //String tempString = sign + iPartC + "," + fPartC;
                result = iPartC + "," + fPartC;
                result = deleteZeroFromNumber(result);

                return result;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'sub' = [ " + ex.Message + " ]");
            }
        }

        public String subIPart(String a, String b)
        {
            // возвращает разность двух чисел
            String result = "";
            int minusOne = 0;
            String signResult = "";
            try
            {
                // Выравнивание длин входных чисел
                int aLength = a.Length;
                int bLength = b.Length;
                if (aLength != bLength)
                {
                    if (aLength > bLength)
                        for (int i = 0; i < (aLength - bLength); i++)
                            b = "0" + b;
                    else
                        for (int i = 0; i < (bLength - aLength); i++)
                            a = "0" + a;
                }

                // если a < b, то поменять их местами
                // и выставить знак результата отрицательным
                if (compare(a, b) == 2)
                {
                    String strTemp = a;
                    a = b;
                    b = strTemp;
                    signResult = "-";
                }

                // собственно вычитание

                for (int i = a.Length - 1; i >= 0; i--)
                {
                    if ((a[i] + minusOne) >= b[i])
                    {
                        result = (int.Parse(a[i].ToString()) + minusOne -
                                                  int.Parse(b[i].ToString())).ToString() + result;
                        minusOne = 0;
                    }
                    else
                    {
                        result = (int.Parse(a[i].ToString()) + minusOne -
                                                  int.Parse(b[i].ToString()) + 10) + result;
                        minusOne = -1;
                    }
                }

                // удаление 0 в начале числа
                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i] != '0')
                    {
                        result = result.Substring(i);
                        break;
                    }
                }
                return signResult + result;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'subIPart' = [ " + ex.Message + " ]");
            }
        }

        public String subFPart(String a, String b)
        {
            // возвращает разность двух чисел
            String result = "";
            int minusOne = 0;
            String signResult = "";
            try
            {
                // Выравнивание длин входных чисел
                int aLength = a.Length;
                int bLength = b.Length;
                if (aLength != bLength)
                {
                    if (aLength > bLength)
                        for (int i = 0; i < (aLength - bLength); i++)
                            b += "0";
                    else
                        for (int i = 0; i < (bLength - aLength); i++)
                            a += "0";
                }
                // если a < b, то поменять их местами
                // и выставить знак результата отрицательным
                if (compare(a, b) == 2)
                {
                    String strTemp = a;
                    a = b;
                    b = strTemp;
                    signResult = "-";
                }

                // собственно вычитание
                for (int i = a.Length - 1; i >= 0; i--)
                {
                    if ((a[i] + minusOne) >= b[i])
                    {
                        result = (int.Parse(a[i].ToString()) + minusOne -
                                                  int.Parse(b[i].ToString())) + result;
                        minusOne = 0;
                    }
                    else
                    {
                        result = (int.Parse(a[i].ToString()) + minusOne -
                                                  int.Parse(b[i].ToString()) + 10).ToString() + result;
                        minusOne = -1;
                    }
                }

                // удаление 0 в конце числа
                for (int i = result.Length - 1; i > 0; i--)
                {
                    if (result[i] != '0')
                    {
                        result = result.Substring(0, i + 1);
                        break;
                    }
                }
                return signResult + result;
            }
            catch (Exception ex)
            {
                throw new FCCoreArithmeticException("Func 'subFPart' = [ " + ex.Message + " ]");
            }
        }
*/
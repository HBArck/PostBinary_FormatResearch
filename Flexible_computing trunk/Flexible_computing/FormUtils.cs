using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using System.IO;

namespace Flexible_computing
{
    public partial class Form1 : Form
    {
        public String[] FormatFileds =
        { 
            "tbExp",
            "tbMantisa",
            "tbMF",
            "tbCF"
        };

        public String[] numberState = 
        {
            "нормализованное",   // norm
            "денормализованное", // denorm 
            "нуль",              // zero
            "бесконечность",     // inf
            "не число",          // Nan
            "ошибка предсталения"// error
        };
        public Color[] ColorsForState = 
        { 
            Color.Green,
            Color.Indigo,
            Color.RoyalBlue,
            Color.OrangeRed,
            Color.Red,
            Color.Red,
            Color.BlueViolet
        };

        private static String[,] languageStringsEng =  {
                        /*__0__*/   { "Start", "ReCalculate","Load","Save","Copy","Clear" },
                        /*__1__*/   { "Logs", "Analize","Statistics","File","Properties","Help" },
                        /*__2__*/   { "InputData","Number bits","Accurately Value from pbinary","Error: Input number - pb","Exit","Contats..."},
                        /*__3__*/   { "About programm...","Number","Fraction","Interval","Basis","Binary"},
                        /*__4__*/   { "PostBinary","Copy to","File","Clipboard","Type Data to Save","Number Bits"},
                        /*__5__*/   { "Input Data","Accurately Value","Error","Language","Test Mode","Sign"},
                        /*__6__*/   { "Num of chars:","Exponent","Mantisa","Indentifier","PostBinary Format Converter (Flexible Computing)","Help"},
                        /*__7__*/   { "Rounding","Values Format","","","TetraCode","Version"},
                        /*__8__*/   { "Are you sure? \r\n All calculations will be lost!","To near Num","To  -∞ ; To  +∞","To  +∞","To  -∞ ","To нулю"},
                        /*__9__*/   { "Normolized","Denormolized","Zero","Inf","Nan","Err"}
                                 };//{"","","","","",""}

        private static String[,] languageStringsRus =  {
                        /*__0__*/   { "Старт", "Перерасчет","Загрузить","Сохранить","Копировать","Обнулить" },
                        /*__1__*/   { "Логи", "Анализ","Статистика","Файл","Параметры","Справка" },
                        /*__2__*/   { "Ввод исходных данных","Значения полей числа","Значения полученные из pbinary","Погрешность: исходное число - pb","Выход","Свяжитесь с нами..."},
                        /*__3__*/   { "О программе...","Число","Дробь","Интервал","Базис","Бинарный"},
                        /*__4__*/   { "ПостБинарный","Выбор типа копирования","В Файл","В Буфер обмена","Формат данных для хранения","Поля формата"},
                        /*__5__*/   { "Входное число","Точное значение","Погрешность","Язык","Тестовый Режим","Знак"},
                        /*__6__*/   { "Кол-во чисел:","Порядок","Мантиса","Идентификатор","Преобразователь постбинарных форматов (Flexible Computing) ","Помощь"},
                        /*__7__*/   { "Округление","Формат значений","","","Тетракод","Версия"},
                        /*__8__*/   { "Вы уверены, что хотите выйти? \r\n Все данные будут утеряны!","к бл. числу","к  -∞ ; к  +∞","к  +∞","к  -∞ ","к нулю"},
                        /*__9__*/   { "Нормализованное","Денормализованное","Нуль","Бесконечность","Не число","Не представимо"}
                                 };

        private static String[,] languageStringsGer =  {
                        /*__0__*/   { "Start", "Nachkalkulation","Download","Sparen","Kopieren","Löschen" },
                        /*__1__*/   { "Logbücher", "Analyze","Statistik","File","Die Parametr","Auskunft" },
                        /*__2__*/   { "Ввод исходных данных","Значения полей числа","Значения полученные из pbinary","Погрешность: исходное число - pb","Выход","Свяжитесь с нами..."},
                        /*__3__*/   { "Über das Programm...","Anzahl","Bruchzahl","Intervall","Basis","Binär"},
                        /*__4__*/   { "NachBinär","Выбор типа копирования","В Файл","В Буфер обмена","Формат данных для хранения","Поля формата"},
                        /*__5__*/   { "Входное число","Точное значение","Погрешность","Язык","Тестовый Режим","Знак"},
                        /*__6__*/   { "Кол-во чисел:","Порядок","Мантиса","Идентификатор","Преобразователь постбинарных форматов (Flexible Computing) ","Помощь"},
                        /*__7__*/   { "Округление","Формат значений","","","Тетракод","Версия"},
                        /*__8__*/   { "Вы уверены, что хотите выйти? \r\n Все данные будут утеряны!","к бл. числу","к  -∞ ; к  +∞","к  +∞","к  -∞ ","к нулю"},
                        /*__9__*/   { "Нормализованное","Денормализованное","Нуль","Бесконечность","Не число","Не представимо"}
                                 };//{"","","","","",""}
        private static String[,] languageStringsUkr =  {
                        /*__0__*/   { "Старт", "Перерасчет","Загрузить","Сохранить","Копировать","Обнулить" },
                        /*__1__*/   { "Логи", "Анализ","Статистика","Файл","Параметры","Справка" },
                        /*__2__*/   { "Ввод исходных данных","Значения полей числа","Значения полученные из pbinary","Погрешность: исходное число - pb","Выход","Свяжитесь с нами..."},
                        /*__3__*/   { "О программе...","Число","Дробь","Интервал","Базис","Бинарный"},
                        /*__4__*/   { "ПостБинарный","Выбор типа копирования","В Файл","В Буфер обмена","Формат данных для хранения","Поля формата"},
                        /*__5__*/   { "Входное число","Точное значение","Погрешность","Язык","Тестовый Режим","Знак"},
                        /*__6__*/   { "Кол-во чисел:","Порядок","Мантиса","Идентификатор","Преобразователь постбинарных форматов (Flexible Computing) ","Помощь"},
                        /*__7__*/   { "Округление","Формат значений","","","Тетракод","Версия"},
                        /*__8__*/   { "Вы уверены, что хотите выйти? \r\n Все данные будут утеряны!","к бл. числу","к  -∞ ; к  +∞","к  +∞","к  -∞ ","к нулю"},
                        /*__9__*/   { "Нормализованное","Денормализованное","Нуль","Бесконечность","Не число","Не представимо"}
                                 };//{"","","","","",""}
        String[][,] inStr = { languageStringsEng, languageStringsRus, languageStringsGer, languageStringsUkr };
        /// <summary>
        /// Changing language based on input var lang
        /// </summary>
        /// <param name="lang">0-English; 1-Russian; 2-German; 4-Ukrainian</param>
        public void changeLanguage(int lang)
        {

            bStart.Text = стартToolStripMenuItem.Text = inStr[lang][0, 0];
            recalculate.Text = перерасчетToolStripMenuItem.Text = inStr[lang][0, 1];
            bLoad.Text = загрузитьToolStripMenuItem1.Text = inStr[lang][0, 2];
            сохранитьToolStripMenuItem.Text = inStr[lang][0, 3];
            bCopy.Text = копироватьToolStripMenuItem.Text = inStr[lang][0, 4];
            bClear.Text = обнулитьToolStripMenuItem.Text = inStr[lang][0, 5];

            tabControlFooter.TabPages[0].Text = inStr[lang][1, 0];
            tabControlFooter.TabPages[1].Text = inStr[lang][1, 1];
            tabControlFooter.TabPages[2].Text = inStr[lang][1, 2];
            файлToolStripMenuItem1.Text = inStr[lang][1, 3];
            параметрыToolStripMenuItem1.Text = inStr[lang][1, 4];
            справкаToolStripMenuItem.Text = inStr[lang][1, 5];

            groupBox_Input.Text = inStr[lang][2, 0];
            gBFieldContainer32.Text = gBFieldContainer64.Text = gBFieldContainer256.Text = gBFieldContainer128.Text = inStr[lang][2, 1];
            gbRes.Text = inStr[lang][2, 2];
            gbError.Text = inStr[lang][2, 3];
            выходToolStripMenuItem.Text = выходToolStripMenuItem1.Text = inStr[lang][2, 4];
            свяжитесьСНамиToolStripMenuItem.Text = inStr[lang][2, 5];

            оПрограммеToolStripMenuItem.Text = inStr[lang][3, 0];
            radioInteger.Text = числоToolStripMenuItem.Text = inStr[lang][3, 1];
            radioFloat.Text = дробьToolStripMenuItem.Text = inStr[lang][3, 2];
            radioInterval.Text = интервалToolStripMenuItem.Text = inStr[lang][3, 3];
            базисToolStripMenuItem.Text = inStr[lang][3, 4];
            бинарныйToolStripMenuItem.Text = inStr[lang][3, 5];

            постбинарныйToolStripMenuItem.Text = inStr[lang][4, 0];
            выборТипаКопированияToolStripMenuItem.Text = inStr[lang][4, 1];
            вФайлToolStripMenuItem.Text = inStr[lang][4, 2];
            вБуферОбменаToolStripMenuItem.Text = inStr[lang][4, 3];
            форматДанныхДляХраненияToolStripMenuItem.Text = inStr[lang][4, 4];
            поляФорматаToolStripMenuItem.Text = inStr[lang][4, 5];

            входноеЧислоToolStripMenuItem.Text = inStr[lang][5, 0];
            точноеЗначениеToolStripMenuItem.Text = inStr[lang][5, 1];
            погрешностьToolStripMenuItem.Text = inStr[lang][5, 2];
            языкToolStripMenuItem.Text = inStr[lang][5, 3];
            тестовыйРежимToolStripMenuItem.Text = inStr[lang][5, 4];
            label1.Text = label35.Text = label59.Text = label79.Text = inStr[lang][5, 5];

            label19.Text = inStr[lang][6, 0];
            label11.Text = label22.Text = label45.Text = label65.Text = inStr[lang][6, 1];
            label2.Text = label27.Text = label49.Text = label69.Text = inStr[lang][6, 2];
            label3.Text = label21.Text = label44.Text = label64.Text = inStr[lang][6, 3];
            this.Text = inStr[lang][6, 4];
            помощьToolStripMenuItem1.Text = inStr[lang][6, 5];


            toolStripMenuItem1.Text = округлениеToolStripMenuItem.Text = inStr[lang][7, 0];
            форматЗначенийToolStripMenuItem.Text = inStr[lang][7, 1];
            chbTetra.Text = inStr[lang][7, 4];


            tbMenuVer.Text = "" + inStr[lang][7, 5] +"[" + version[0] + "." + version[1] + "." + version[2] + "]";
            miToInt.Text = inStr[lang][8, 1];
            miToNPInf.Text = inStr[lang][8, 2];
            miToPInf.Text = inStr[lang][8, 3];
            miToNInf.Text = inStr[lang][8, 4];
            miToZero.Text = inStr[lang][8, 5]; changeCheckStateForRounding((byte)roundType);
            switch (tabControl_Format.SelectedIndex)
            {
                case 0:
                    lNormDenorm.Text = inStr[lang][9, (int)Core.Num32.NumberState];
                    lNormDenorm.ForeColor = ColorsForState[(int)Core.Num32.NumberState];
                    break;
                case 1:
                    lNormDenorm.Text = inStr[lang][9, (int)Core.Num64.NumberState];
                    lNormDenorm.ForeColor = ColorsForState[(int)Core.Num64.NumberState];
                    break;
                case 2:
                    lNormDenorm.Text = inStr[lang][9, (int)Core.Num128.NumberState];
                    lNormDenorm.ForeColor = ColorsForState[(int)Core.Num128.NumberState];
                    break;
                case 3:
                    lNormDenorm.Text = inStr[lang][9, (int)Core.Num256.NumberState];
                    lNormDenorm.ForeColor = ColorsForState[(int)Core.Num256.NumberState];
                    break;
            }
        }

        private void chbTetra_CheckStateChanged(object sender, EventArgs e)
        {
            TetraCheck = (TetraCheck) ? false : true;
            if ((TetraCheck) && (tabControl_Format.SelectedIndex <= 1)) // NO SUCH FORMATS 64/16pi or 64/16fp
            {
                
                radioFloat.Enabled = false;
                radioInterval.Enabled = false;
                дробьToolStripMenuItem.Enabled = false;
                интервалToolStripMenuItem.Enabled = false;
                числоToolStripMenuItem.Checked = true;
            }
            else
            {
                radioFloat.Enabled = true;
                radioInterval.Enabled = true;
                дробьToolStripMenuItem.Enabled = true;
                интервалToolStripMenuItem.Enabled = true;
            }
        }

//---------------------------   BLOCK of input Validating func's -------------------
        private void tbInput_TextChanged(object sender, EventArgs e)
        {
            if (this.tbInput.Text.Count() > 0)
                bStart.Enabled = true;
            else
                if (this.tbInput.Text.Count() == 0)
                    bStart.Enabled = false;
        }
        public void setErrorOnInput()
        {// 
            tbInput.BackColor = Color.Red;
            errorCounter = 0;
            errorTimer.Start();
        }
        private void errorTimer_Tick(object sender, EventArgs e)
        {
            if (errorCounter < 6)
            {
                errorCounter++;
            }
            else
            {
                tbInput.BackColor = Color.FromKnownColor(KnownColor.Window);
                errorTimer.Stop();
            }
        }


        /// <summary>
        /// This function check if in input string still has 
        /// charachters ',' '-' '/' ';' in it, and set flags
        /// 
        /// Функиция  проверяет, произошли ли изменения во входной строке,
        /// содержит ли она все так же символы ',' '-' '/' ';' , и выставляет соответ. флаги
        /// </summary>
        public void checkFlagState(char checkingChar)
        {
            String checkingString = tbInput.Text;

            if (checkingString.Contains('-') || (checkingChar == '-'))
                isMinusWritten = true;
            else
                isMinusWritten = false;

            if (checkingString.Contains(',') || (checkingChar == ',') || (checkingChar == '.'))
                isCommaWritten = true;
            else
                isCommaWritten = false;

            if (checkingString.Contains('/') || (checkingChar == '/'))
                isSlashWritten = true;
            else
                isSlashWritten = false;
        }

        private void tbInput_KeyPress(object sender, KeyPressEventArgs e)
        {// Ctrl + C  =3
         // Ctrl + V  =22
         // Ctrl + X  =24
            String temp;
            if (((e.KeyChar > '9') || (e.KeyChar < '0')) && (
                 (e.KeyChar != 'm') && (e.KeyChar != 'M') &&
                 (e.KeyChar != 'e') && (e.KeyChar != 'E') &&
                 (e.KeyChar != 'A') && (e.KeyChar != 'a') && (e.KeyChar != '+') &&
                 (e.KeyChar != '-') && (e.KeyChar != '/') && (e.KeyChar != '\\') &&
                 (e.KeyChar != '.') && (e.KeyChar != ',') && (e.KeyChar != ';') &&
                 (e.KeyChar != 3) && (e.KeyChar != 22) && (e.KeyChar != 24) && 
                 (e.KeyChar != '\b') && (e.KeyChar != 13))
               )
                e.KeyChar = '\0';

            if (e.KeyChar == 'm')
                e.KeyChar = 'M';
            if (e.KeyChar == 'a')
                e.KeyChar = 'A';

            // Raplacing the characters
            if ((e.KeyChar == '.') || (e.KeyChar == ','))
            {
                e.KeyChar = ',';
                if (isCommaWritten)
                {
                    if ((roundType > 0) && ((isFloatSelected) || (isIntervalSelected)))
                        e.KeyChar = '\0';
                    else
                    {
                        if (roundType == 1)
                            isFloatSelected = true;
                        if (roundType == 2)
                            isIntervalSelected = true;
                    }
                }
                else
                    isCommaWritten = true;
            }

            if ((e.KeyChar == '-') && (isMinusWritten))
                e.KeyChar = '-';// was e.KeyChar = '\0'
            else
                isMinusWritten = true;

            if (((e.KeyChar == '\\') || (e.KeyChar == '/')) && (isSlashWritten))
            {
                e.KeyChar = '\0';
            }
            else
                isSlashWritten = true;

            // Check if special characters are still in input String
            checkFlagState(e.KeyChar);
            temp = tbInput.Text;
            if (e.KeyChar == 13)
            {
                bStart_Click(sender, e);
                e.KeyChar = '\0';
                return;
            }

            tbInput.Text = temp;
        }

        public void changeFiledsFormat(byte isNormal)
        {
            if (isNormal == 0)
            {
                tbMF64.Location = new Point(tbMF64.Location.X, 30);
                tbMF128.Location = new Point(tbMF128.Location.X, 30);
                tbMF256.Location = new Point(tbMF256.Location.X, 30);

                tbCF64.Location = new Point(tbCF64.Location.X, 30);
                tbCF128.Location = new Point(tbCF128.Location.X, 30);
                tbCF256.Location = new Point(tbCF256.Location.X, 30);

                tbExp64_2.Visible = tbMantisa64_2.Visible = false;
                tbExp128_2.Visible = tbMantisa128_2.Visible = false;
                tbExp256_2.Visible = tbMantisa256_2.Visible = false;

                tbSign64_2.Visible = tbSign128_2.Visible = tbSign256_2.Visible = false;
                //64
                label16.Visible = label13.Visible = label14.Visible = label15.Visible = label25.Visible = false;
                label31.Text = "51"; label32.Text = "52"; 
                //128
                label17.Visible = label20.Visible = label23.Visible = label24.Visible = label26.Visible = false;
                label55.Text = "111"; label56.Text = "112";
                //256
                label28.Visible = label29.Visible = label30.Visible = label37.Visible = label36.Visible = false;
                label75.Text = "234"; label76.Text = "235";

                //LABEL's BEGIN
                l64_0.Location = new Point(l64_0.Location.X, 16);
                l64_1.Location = new Point(l64_1.Location.X, 16);
                l64_2.Location = new Point(l64_2.Location.X, 16);
                l64_3.Location = new Point(l64_3.Location.X, 16);
                l64_4.Location = new Point(l64_4.Location.X, 16);

                l128_0.Location = new Point(l128_0.Location.X, 16);
                l128_1.Location = new Point(l128_1.Location.X, 16);
                l128_2.Location = new Point(l128_2.Location.X, 16);
                l128_3.Location = new Point(l128_3.Location.X, 16);
                label53.Location = new Point(label53.Location.X, 16);

                l256_0.Location = new Point(l256_0.Location.X, 16);
                l256_1.Location = new Point(l256_1.Location.X, 16);
                l256_2.Location = new Point(l256_2.Location.X, 16);
                l256_3.Location = new Point(l256_3.Location.X, 16);
                label73.Location = new Point(label73.Location.X, 16);
                //LABEL's END
            }

            if ((isNormal == 1) || (isNormal == 2))
            {

                tbMF64.Location = new Point(tbMF64.Location.X, 73);
                tbMF128.Location = new Point(tbMF128.Location.X, 73);
                tbMF256.Location = new Point(tbMF256.Location.X, 73);

                tbCF64.Location = new Point(tbCF64.Location.X, 73);
                tbCF128.Location = new Point(tbCF128.Location.X, 73);
                tbCF256.Location = new Point(tbCF256.Location.X, 73);

                tbExp64_2.Visible = tbMantisa64_2.Visible = true;
                tbExp128_2.Visible = tbMantisa128_2.Visible = true;
                tbExp256_2.Visible = tbMantisa256_2.Visible = true;

                //LABEL's BEGIN
                l64_0.Location = new Point(l64_0.Location.X, 100);
                l64_1.Location = new Point(l64_1.Location.X, 100);
                l64_2.Location = new Point(l64_2.Location.X, 100);
                l64_3.Location = new Point(l64_3.Location.X, 100);
                l64_4.Location = new Point(l64_4.Location.X, 100);

                l128_0.Location = new Point(l128_0.Location.X, 100);
                l128_1.Location = new Point(l128_1.Location.X, 100);
                l128_2.Location = new Point(l128_2.Location.X, 100);
                l128_3.Location = new Point(l128_3.Location.X, 100);
                label53.Location = new Point(label53.Location.X,100);

                l256_0.Location = new Point(l256_0.Location.X, 100);
                l256_1.Location = new Point(l256_1.Location.X, 100);
                l256_2.Location = new Point(l256_2.Location.X, 100);
                l256_3.Location = new Point(l256_3.Location.X, 100);
                label73.Location = new Point(label73.Location.X, 100);
                // 64
                label13.Visible = label14.Visible = label15.Visible = label25.Visible  = true;
                label31.Text = "54"; label32.Text = "55";
                // 128
                label17.Visible = label20.Visible = label23.Visible = label26.Visible = true;
                label55.Text = "115"; label56.Text = "116"; 
                // 256
                label28.Visible = label29.Visible = label30.Visible = label37.Visible = true;
                label75.Text = "239"; label76.Text = "240";

                //LABEL's END

                if (isNormal == 1)
                {
                    tbSign64_2.Visible = tbSign128_2.Visible = tbSign256_2.Visible = false;
                    // 64
                    label16.Visible = false;
                    // 128
                    label24.Visible = false;
                    // 256
                    label36.Visible = false;
                }
                if (isNormal == 2)
                {
                    tbSign64_2.Visible = tbSign128_2.Visible = tbSign256_2.Visible = true;
                    // 64
                    label16.Visible = true;
                    // 128
                    label24.Visible = true;
                    // 256
                    label36.Visible = true;
                }
            }
        }

//----------------------------------- BLOCK Tabs Changing Func's

        public void textChangeOnForm()
        {
            int tabIndex = tabControl_Format.SelectedIndex;
            int i;
            String stringEnd32;

            tbCF32.Text = currentCCOnTabs == false ? "0" : "0"; Core.Num64.CF = "0";
            tbCF64.Text = currentCCOnTabs == false ? "01" : "1"; Core.Num64.CF = "01";
            tbCF128.Text = currentCCOnTabs == false ? "011" : "3"; Core.Num128.CF = "011";
            tbCF256.Text = currentCCOnTabs == false ? "0111" : "7"; Core.Num256.CF = "0111";

            switch (inputStringFormat)
            {
                case 0:
                    gbRes.Text = inStr[lang][2, 2] + (32 << tabIndex);
                    gbError.Text = inStr[lang][2, 3] + (32 << tabIndex);
                    gbRes.Enabled = true;
                    gbError.Enabled = true;
                    tbMF32.Text = currentCCOnTabs == false ? "0" : "0";   Core.Num32.MF = "0";
                    tbMF64.Text = currentCCOnTabs == false ? "00" : "0";   Core.Num64.MF = "00";
                    tbMF128.Text = currentCCOnTabs == false ?"00000" : "0"; Core.Num128.MF = "00000";
                    tbMF256.Text = currentCCOnTabs == false ?"000000000000" : "0"; Core.Num256.MF = "000000000000";

                    for (i = 1; i < 4; i++)
                        tabControl_Format.TabPages[i].Text = "pbinary" + (32 << i);
                    break;
                case 1:
                    
                    tbMF64.Text = currentCCOnTabs ==false ? "01":"1";
                    tbMF128.Text =currentCCOnTabs ==false ? "00001":"1";
                    tbMF256.Text =currentCCOnTabs ==false ? "000000000001":"1";

                    Core.Num64.MF = "01";
                    Core.Num128.MF = "00001";
                    Core.Num256.MF = "000000000001";
                    if (tabIndex > 0)
                    {
                        gbRes.Enabled = true;
                        gbError.Enabled = true;
                        gbRes.Text = inStr[lang][2, 2] + (32 << tabIndex) + "/" + (16 << tabIndex) + "f";
                        gbError.Text = inStr[lang][2, 3] + (32 << tabIndex) + "/" + (16 << tabIndex) + "f";
                        for (i = 1; i < 4; i++)
                            tabControl_Format.TabPages[i].Text = "pbinary" + (32 << i) + "/" + (16 << i) + "f";
                    }
                    else
                    {
                        gbRes.Enabled = false;
                        gbError.Enabled = false;
                    }

                    break;
                case 2:

                    tbMF64.Text = currentCCOnTabs ==false ? "10":"2";
                    tbMF128.Text =currentCCOnTabs ==false ? "00010":"2";
                    tbMF256.Text =currentCCOnTabs ==false ? "000000000010":"2";

                    Core.Num64.MF = "10";
                    Core.Num128.MF = "00010";
                    Core.Num256.MF = "0000000010";

                    if (tabIndex > 0)
                    {
                        gbRes.Enabled = true;
                        gbError.Enabled = true;
                        gbRes.Text = inStr[lang][2, 2] + (32 << tabIndex) + "/" + (16 << tabIndex) + "i";
                        gbError.Text = inStr[lang][2, 3] + (32 << tabIndex) + "/" + (16 << tabIndex) + "i";
                        for (i = 1; i < 4; i++)
                            tabControl_Format.TabPages[i].Text = "pbinary" + (32 << i) + "/" + (16 << i) + "i";
                    }
                    else
                    {
                        gbRes.Enabled = false;
                        gbError.Enabled = false;
                    }
                    break;
            }
        }

        public byte getIndexType()
        {
            if (currentType[0] == true)
                return 0;
            if (currentType[1] == true)
                return 1;
            if (currentType[2] == true)
                return 2;
            return 4;
        }

        public void addCharsToInput(int nextType)
        {
            String temp = tbInput.Text;
            int iTemp = getIndexType();
            int temp2;
            tbInput.Text = "";
            if (iTemp != nextType)
                switch (iTemp)
                {
                    case 0:
                        if (nextType == 1)
                        {
                            if (temp.IndexOf("/") == -1)
                                if (temp.Length > 0)
                                {
                                    tbInput.Text = temp + "/1,0";
                                    break;
                                }
                                else
                                {
                                    tbInput.Text = "1,0/1,0";
                                    break;
                                }
                            else
                                tbInput.Text = temp;
                        }
                        else
                        {
                            if (temp.IndexOf(";") == -1)
                            {
                                if (temp.Length > 0)
                                {
                                    tbInput.Text = temp + ";0,0";
                                    break;
                                }
                                else
                                {
                                    tbInput.Text = "1,0;0,0";
                                    break;
                                }
                            }
                            else
                                tbInput.Text = temp;
                        }
                        break;
                    case 1:
                        if (nextType == 0)
                        {
                            if (temp.IndexOf("/") != -1)
                                tbInput.Text = temp.Substring(0, temp.IndexOf("/"));
                        }
                        if (nextType == 2)
                        {
                            temp2 = temp.IndexOf("/");
                            if (temp2 != -1)
                            {
                                if (temp.Length == 0)
                                    tbInput.Text = "1,0";
                                tbInput.Text += temp.Substring(0, temp.IndexOf("/"));
                                tbInput.Text += ";";
                                tbInput.Text += temp.Substring(temp.IndexOf("/") + 1, temp.Length - temp.IndexOf("/") - 1);
                            }
                            else
                                tbInput.Text = "1,0/1,0";
                        }
                        break;
                    case 2:
                        if (nextType == 0)
                        {
                            if (temp.IndexOf(";") != -1)
                                tbInput.Text = temp.Substring(0, temp.IndexOf(";"));
                        }
                        if (nextType == 1)
                        {
                            temp2 = temp.IndexOf(";");
                            if (temp2 != -1)
                            {
                                if (temp.Length == 0)
                                    tbInput.Text = "1,0";
                                tbInput.Text += temp.Substring(0, temp.IndexOf(";"));
                                tbInput.Text += "/";
                                tbInput.Text += temp.Substring(temp.IndexOf(";") + 1, temp.Length - temp.IndexOf(";") - 1);
                            }
                            else
                                tbInput.Text = "1,0;0,0";
                        }
                        break;
                }
        }

// ------- BLOCK Type Format Radio Buttons
        public void changeTypeFormat(int nextType)
        {
            for (int i = 0; i < 3; i++)
                currentType[i] = false;
            switch (nextType)
            {
                case 0: currentType[0] = true; break;
                case 1: currentType[1] = true; break;
                case 2: currentType[2] = true; break;
            }

        }

        /// <summary>
        /// This function changes for different formats it's Bit Count
        /// Integer - Normal
        /// Fraction & Interval - Less
        /// </summary>
        public void changeTextBoxMaxSymbols()
        {
            switch (inputStringFormat)
            {
                case 0: 
                    tbExp64.MaxLength = 11;
                    tbExp128.MaxLength = 15;
                    tbExp256.MaxLength = 20;
                    tbMantisa64.MaxLength = 48;
                    tbMantisa128.MaxLength = 109;
                    tbMantisa256.MaxLength = 214;
                    
                    break;
                case 1:                    
                case 2:
                    tbExp64.MaxLength = 8;
                    tbExp128.MaxLength = 11;
                    tbExp256.MaxLength = 15;
                    tbMantisa64.MaxLength = 21;
                    tbMantisa128.MaxLength = 48;
                    tbMantisa256.MaxLength = 109;
                    break;
                case 3: // TetraCode
                    tbExp64.MaxLength = 16;
                    tbExp128.MaxLength = 22;
                    tbExp256.MaxLength = 30;
                    tbMantisa64.MaxLength = 42;
                    tbMantisa128.MaxLength = 96;
                    tbMantisa256.MaxLength = 208;
                    break;
                case 4: // TetraCode Fraction & Interval
                    tbExp64.MaxLength = 16; // NONE
                    tbExp128.MaxLength = 16;
                    tbExp256.MaxLength = 22;
                    tbMantisa64.MaxLength = 42; // NONE
                    tbMantisa128.MaxLength = 42;
                    tbMantisa256.MaxLength = 96;
                    break;
            }

        }

        private void radioInteger_Click(object sender, EventArgs e)
        {
            if (Core.NumberFormat != 0)
            {
                changeFiledsFormat(0);
                inputStringFormat = 0;
                textChangeOnForm();
                //changePasportFormat(inputStringFormat, tabControl_Format.SelectedIndex);

                miToNPInf.Visible = false;
                miToPInf.Visible = true;
                miToNInf.Visible = true;
                addCharsToInput(0);
                changeCheckStateForRounding(1);
                changeTypeFormat(0);
                Core.NumberFormat = 0;
                changeTextBoxMaxSymbols();
                changeBitLength(currentCCOnTabs == true ? 1:0 );
                chbTetra.Enabled = true;
               // bStart_Click(sender, e);
              
                tbExp32.Enabled = true;
                tbMantisa32.Enabled = true;              
            }
        }
        private void radioFloat_Click(object sender, EventArgs e)
        {
            if (Core.NumberFormat != 1)
            {
                
                inputStringFormat = 1;
                changeFiledsFormat(1);
                textChangeOnForm();
                //changePasportFormat(inputStringFormat, tabControl_Format.SelectedIndex);

                miToNPInf.Visible = false;
                miToPInf.Visible = true;
                miToNInf.Visible = true;

                addCharsToInput(1);

                changeTypeFormat(1);
                changeCheckStateForRounding(1);
                Core.NumberFormat = 1;
                changeTextBoxMaxSymbols();
                changeBitLength(currentCCOnTabs == true ? 1 : 0);
                if (tabControl_Format.SelectedIndex <= 1)
                    chbTetra.Enabled = false;
                else
                    chbTetra.Enabled = true;
                //bStart_Click(sender, e);

                tbExp32.Enabled = false;
                tbMantisa32.Enabled = false;
            }
        }
        private void radioInterval_Click(object sender, EventArgs e)
        {
            if (Core.NumberFormat != 2)
            {
                inputStringFormat = 2;
                changeFiledsFormat(2);
                textChangeOnForm();
               // changePasportFormat(inputStringFormat, tabControl_Format.SelectedIndex);

                miToNPInf.Visible = true;
                miToPInf.Visible = false;
                miToNInf.Visible = false;

                addCharsToInput(2);

                changeCheckStateForRounding(4);
                changeTypeFormat(2);
                Core.NumberFormat = 2;
                changeTextBoxMaxSymbols();
                changeBitLength(currentCCOnTabs == true ? 1 : 0);
                if (tabControl_Format.SelectedIndex <= 1)
                    chbTetra.Enabled = false;
                else
                    chbTetra.Enabled = true;
               // bStart_Click(sender, e);

                tbExp32.Enabled = false;
                tbMantisa32.Enabled = false;
            }
        }

//-------------------------------------------------------------- BLOCK Result Buttons
        private void rB10cc32_Click(object sender, EventArgs e){
            //bStart_Click(sender, e);
            ThreadPool.QueueUserWorkItem(calcResultsAndErrors);
        }
        private void rB2cc32_Click(object sender, EventArgs e){
            //bStart_Click(sender, e);
            ThreadPool.QueueUserWorkItem(calcResultsAndErrors);
        }
        private void cBExp32_CheckedChanged(object sender, EventArgs e){
            //bStart_Click(sender, e);
            ThreadPool.QueueUserWorkItem(calcResultsAndErrors);
        }

//-------------------------------------------------------------- BLOCK Rounding Buttons
        private void miToZero_Click(object sender, EventArgs e){
            changeCheckStateForRounding(0);
        }
        private void miToInt_Click(object sender, EventArgs e){
            changeCheckStateForRounding(1);
        }
        private void miToPInf_Click(object sender, EventArgs e){
            changeCheckStateForRounding(2);
        }
        private void miToNInf_Click(object sender, EventArgs e){
            changeCheckStateForRounding(3);
        }
        private void miToNPInf_Click(object sender, EventArgs e){
            changeCheckStateForRounding(4);
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutForm = new About();
            aboutForm.ShowDialog(this);
        }
        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(inStr[lang][8, 0], inStr[lang][2, 4] + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void обнулитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bClear_Click(sender, e);
        }

        // Buttons on Form 
        /// <summary>
        /// Copies Current Number to ClipBoard
        /// </summary>
        private void bCopy_Click(object sender, EventArgs e)
        {
            String TextToClipBoard = "";
            switch (inputStringFormat)
            {
                case 0:
                    switch (tabControl_Format.SelectedIndex)
                    {
                        case 0:
                            TextToClipBoard = tbSign32.Text + tbExp32.Text + tbMantisa32.Text + tbMF32.Text + tbCF32.Text;
                            break;
                        case 1:
                            TextToClipBoard = tbSign64.Text + tbExp64.Text + tbMantisa64.Text + tbMF64.Text + tbCF64.Text;
                            break;
                        case 2:
                            TextToClipBoard = tbSign128.Text + tbExp128.Text + tbMantisa128.Text + "0" + tbMF128.Text + tbCF128.Text;
                            break;
                        case 3:
                            TextToClipBoard = tbSign256.Text + tbExp256.Text + tbMantisa256.Text + "00000000" + tbMF256.Text + tbCF256.Text;
                            break;
                    }
                    break;
                case 1:
                    switch (tabControl_Format.SelectedIndex)
                    {
                        //case 0:
                        //  TextToClipBoard = tbSign32.Text + tbExp32.Text + tbMantisa32.Text + tbMF32.Text + tbCF32.Text;
                        //break;
                        case 1:
                            TextToClipBoard = tbSign64.Text + tbExp64.Text + tbMantisa64.Text + tbExp64_2.Text + tbMantisa64_2.Text + tbMF64.Text + tbCF64.Text;
                            break;
                        case 2:
                            TextToClipBoard = tbSign128.Text + tbExp128.Text + tbMantisa128.Text + tbExp128_2.Text + tbMantisa128_2.Text + tbMF128.Text + tbCF128.Text;
                            break;
                        case 3:
                            TextToClipBoard = tbSign256.Text + tbExp256.Text + tbMantisa256.Text + tbExp256_2.Text + tbMantisa256_2.Text + "00000000" + tbMF256.Text + tbCF256.Text;
                            break;
                    }
                    break;
                case 2:
                    switch (tabControl_Format.SelectedIndex)
                    {
                        //case 0:
                        //  TextToClipBoard = tbSign32.Text + tbExp32.Text + tbMantisa32.Text + tbMF32.Text + tbCF32.Text;
                        //break;
                        case 1:
                            TextToClipBoard = tbSign64.Text + tbExp64.Text + tbMantisa64.Text + tbSign64_2.Text + tbExp64_2.Text + tbMantisa64_2.Text + tbMF64.Text + tbCF64.Text;
                            break;
                        case 2:
                            TextToClipBoard = tbSign128.Text + tbExp128.Text + tbMantisa128.Text + tbSign128_2.Text + tbExp128_2.Text + tbMantisa128_2.Text + tbMF128.Text + tbCF128.Text;
                            break;
                        case 3:
                            TextToClipBoard = tbSign256.Text + tbExp256.Text + tbMantisa256.Text + tbSign256_2.Text + tbExp256_2.Text + tbMantisa256_2.Text + "00000000" + tbMF256.Text + tbCF256.Text;
                            break;
                    }
                    break;
            }
            Clipboard.SetText(TextToClipBoard); // скопировал текст в буфер
        }
        private void bClear_Click(object sender, EventArgs e)
        {
            tbRes.Text = "+0,0";
            tbCalcError.Text = "0,0";
            tbInput.Text = "0,0";
            lNormDenorm.ForeColor = ColorsForState[2];
            lNormDenorm.Text = numberState[2];

            tbSign32.Text = "0";
            tbSign64.Text = "0";
            tbSign128.Text = "0";
            tbSign256.Text = "0";

            tbExp32.Text = currentCCOnTabs == false ? "00000000" : "00"; // Done 8
            tbExp64.Text = currentCCOnTabs == false ? inputStringFormat == 0 ? "00000000000" : "00000000" : inputStringFormat == 0 ? "000" : "00"; // Done 11
            tbExp128.Text = currentCCOnTabs == false ? inputStringFormat == 0 ? "000000000000000" : "00000000000" : inputStringFormat == 0 ? "0000" : "000";
            tbExp256.Text = currentCCOnTabs == false ? inputStringFormat == 0 ? "00000000000000000000" : "000000000000000" : inputStringFormat == 0 ? "00000" : "0000";

            tbExp64_2.Text = currentCCOnTabs == false ? "00000000" : "00"; // Done 11
            tbExp128_2.Text = currentCCOnTabs == false ? "00000000000" : "000";
            tbExp256_2.Text = currentCCOnTabs == false ? "000000000000000" : "0000";

            tbMantisa32.Text = currentCCOnTabs == false ? "000000000000000000000" : "000000"; // Done 21
            tbMantisa64.Text = currentCCOnTabs == false ? inputStringFormat == 0 ? "000000000000000000000000000000000000000000000000" : "000000000000000000000" : inputStringFormat == 0 ? "000000000000" : "000000"; // Done 48
            tbMantisa128.Text = currentCCOnTabs == false ? inputStringFormat == 0 ? "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" : "000000000000000000000000000000000000000000000000" : inputStringFormat == 0 ? "00000000000000000000000000" : "000000000000"; // Done 104
            tbMantisa256.Text = currentCCOnTabs == false ? inputStringFormat == 0 ? "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" : "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" : inputStringFormat == 0 ? "0000000000000000000000000000000000000000000000000000000" : "00000000000000000000000000";// Done 219

            tbMantisa64_2.Text = currentCCOnTabs == false ? inputStringFormat == 0 ? "000000000000000000000000000000000000000000000000" : "000000000000000000000" : inputStringFormat == 0 ? "000000000000" : "000000"; // Done 48
            tbMantisa128_2.Text = currentCCOnTabs == false ? inputStringFormat == 0 ? "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" : "000000000000000000000000000000000000000000000000" : inputStringFormat == 0 ? "00000000000000000000000000" : "000000000000"; // Done 104
            tbMantisa256_2.Text = currentCCOnTabs == false ? inputStringFormat == 0 ? "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" : "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" : inputStringFormat == 0 ? "0000000000000000000000000000000000000000000000000000000" : "00000000000000000000000000";// Done 219

            tbMF32.Text = currentCCOnTabs == false ? "0" : "0";
            tbMF64.Text = currentCCOnTabs == false ? "00" : "0";
            tbMF128.Text = currentCCOnTabs == false ? "00000" : "00";
            tbMF256.Text = currentCCOnTabs == false ? "000000000000" : "000";
            tbCF32.Text = currentCCOnTabs == false ? "0" : "0";
            tbCF64.Text = currentCCOnTabs == false ? "01" : "1";
            tbCF128.Text = currentCCOnTabs == false ? "011" : "3";
            tbCF256.Text = currentCCOnTabs == false ? "0111" : "7";

            //richTextBox1.Clear();

            // this.clearVars();// Clears All vars in this class
            isCalcsReset = true;
        }
        private void problemStatus_Click(object sender, EventArgs e)
        {
            int i;
            String[] tempStr;
            try
            {
                //FileStream fStream = File.Open("log.dat", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                //if (fStream != null)
                //{
                //richTextBox1.Text += "Add some";
                //tabControl1.TabPages.Add("Known Errors");
                //tabKnownErrors.
                //RichTextBox rbErrors = new RichTextBox();
                //tabControlFooter.TabPages["Known Errors"].Controls.Add(rbErrors);
                //rbErrors.Text = "Error";
                //richTextBox1.Text += "\r\nException :[" + ex.Message + "]\r\n";


                tabControlFooter.SelectedIndex = 4;
                tempStr = exceptionUtil.Messages();
                if (tempStr != null)
                {
                    for (i = 0; i < tempStr.Length; i++)
                        lbKnownErrors.Items.Add("\r\nException # " + i + " :[" + tempStr[i] + "]\r\n");
                    FileStream fStream = File.Open("err.dat", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
                    if (fStream != null)
                    {
                        for (i = 0; i < tempStr.Length; i++)
                            fStream.Write(stringUtil.ToByteArray(tempStr[i]), 0, tempStr[i].Length);
                    }
                    else
                        problemStatus.ToolTipText = "Исключения не были записаны!";

                }
                else
                {
                }
                //}
            }
            catch (Exception ex)
            {

                richTextBox1.Text += "\r\nException :[" + ex.Message + "]\r\n";
            }
            finally
            {

            }
        }
     

        private void числоToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (radioInteger.Enabled == true)
                radioInteger.Checked = true;
        }
        private void дробьToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (radioFloat.Enabled == true)
                radioFloat.Checked = true;
        }
        private void интервалToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (radioInterval.Enabled == true)
                radioInterval.Checked = true;
        }

        private void changePasportFormat(int inputStringFormat, int selectedTab)
        {/*
            switch (selectedTab)
            {
                case 0: //32
                    dataGridView1.Rows[0].Cells[1].Value = Properties.Resources.Format32_TypeInt;

                    break;

                case 1: //64
                    switch (inputStringFormat)
                    {
                        case 0:
                            dataGridView1.Rows[0].Cells[1].Value = Properties.Resources.Format64_TypeInt;
                            break;
                        case 1:
                            dataGridView1.Rows[0].Cells[1].Value = Properties.Resources.Format64_TypeF;
                            break;
                        case 2:
                            dataGridView1.Rows[0].Cells[1].Value = Properties.Resources.Format64_TypeI;
                            break;
                    }
                    break;

                case 2: //128
                    switch (inputStringFormat)
                    {
                        case 0: break;
                        case 1: break;
                        case 2: break;
                    }
                    break;

                case 3: //256
                    switch (inputStringFormat)
                    {
                        case 0: break;
                        case 1: break;
                        case 2: break;
                    }
                    break;
            }
            */
        }


        private void тестовыйРежимToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestForm tf = new TestForm();
            tf.ShowDialog();
        }
        private void bTestNumber_Click(object sender, EventArgs e)
        {
            //tbInput.Text = my754.testExponentNumber(tbInput.Text, (int)nUpDown.Value);
            //  NewFuncsTester();
        }

        private void числоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            дробьToolStripMenuItem.Checked = false;
            интервалToolStripMenuItem.Checked = false;
            if (radioInteger.Enabled == true)
                radioInteger.Checked = true;

        }
        private void дробьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            числоToolStripMenuItem.Checked = false;
            интервалToolStripMenuItem.Checked = false;
            if (radioFloat.Enabled == true)
                radioFloat.Checked = true;

        }
        private void интервалToolStripMenuItem_Click(object sender, EventArgs e)
        {
            числоToolStripMenuItem.Checked = false;
            дробьToolStripMenuItem.Checked = false;
            if (radioInterval.Enabled == true)
                radioInterval.Checked = true;

        }

        private void постбинарныйToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (бинарныйToolStripMenuItem.Visible == false)
            {
                постбинарныйToolStripMenuItem.Visible = false;
                бинарныйToolStripMenuItem.Visible = true;

                постбинарныйToolStripMenuItem.Checked = true;
                бинарныйToolStripMenuItem.Checked = false;
            }
        }
        private void бинарныйToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            /*
               if (бинарныйToolStripMenuItem.Visible == false)
                {
                    постбинарныйToolStripMenuItem.Visible = true;
                    бинарныйToolStripMenuItem.Visible = false;

                    постбинарныйToolStripMenuItem.Checked = false;
                    бинарныйToolStripMenuItem.Checked = true;
                }
            */
        }

        private void changeLanguage(object sender, EventArgs e)
        {

            switch (((ToolStripMenuItem)sender).Text)
            {
                case "English": lang = 0; changeLanguage(0); // Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                    break;
                case "Русский": lang = 1; changeLanguage(1); // Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru"); 
                    break;
                case "Deutsch": lang = 0; changeLanguage(0); break;
                case "Українска": lang = 0; changeLanguage(0); break;
            }
        }


        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "toolStripMenuItem2")
            {
                if (lbKnownErrors.SelectedIndex != -1)
                {
                    Clipboard.SetText(lbKnownErrors.SelectedItem.ToString());
                }
                else
                {
                    MessageBox.Show("Выберите элемент!");
                }
            }
            if (e.ClickedItem.Name == "toolStripMenuItem7")
            {
                if (lbKnownErrors.SelectedIndex != -1)
                {
                    tbInput.Text = lbKnownErrors.SelectedItem.ToString();
                    bStart_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Выберите элемент!");
                }
            }
        }
        private void contextMenuExceptions_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "toolStripMenuItem8")
            {
                if (mainNode.Nodes.Count > 0)
                {
                    mainNode.Nodes.Clear();
                }
                else
                {
                    MessageBox.Show("Дерево элементов пустое!");
                }
            }
        }
        private void LogsContextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "Clear_Logs")
            {
                richTextBox1.Text = "";
            }
        }


        private void tabControl_Format_SelectedIndexChanged(object sender, EventArgs e)
        {
            //changePasportFormat(inputStringFormat, tabControl_Format.SelectedIndex);
            if (tabControl_Format.SelectedIndex == 0)
            {
                radioFloat.Enabled = false;
                radioInterval.Enabled = false;
                дробьToolStripMenuItem.Enabled = false;
                интервалToolStripMenuItem.Enabled = false;
                числоToolStripMenuItem.Checked = true;
            }
            else
            {
                if (!TetraCheck)
                {
                    radioFloat.Enabled = true;
                    radioInterval.Enabled = true;
                    дробьToolStripMenuItem.Enabled = true;
                    интервалToolStripMenuItem.Enabled = true;
                }
                else
                {
                    if (tabControl_Format.SelectedIndex > 1)
                    {
                        radioFloat.Enabled = true;
                        radioInterval.Enabled = true;
                        дробьToolStripMenuItem.Enabled = true;
                        интервалToolStripMenuItem.Enabled = true;
                        chbTetra.Enabled = true;
                    }
                    else
                    {
                        chbTetra.Enabled = false;
                        if (chbTetra.Checked)
                            chbTetra.Checked = false;
                    }
                }
            }
            textChangeOnForm();
            refreshNumberStatus();
            //l2ccTo16cc_Click(sender,e);
            if (!isCalcsReset)
                ThreadPool.QueueUserWorkItem(calcResultsAndErrors);
        }
        public void changeCheckStateForRounding(byte newRounding)
        {
            miToZero.Checked = false;
            miToPInf.Checked = false;
            miToNInf.Checked = false;
            miToInt.Checked = false;
            miToNPInf.Checked = false;
            switch (newRounding)
            {
                case 0: miToZero.Checked = true; label18.Text = miToZero.Text; roundType = 0; Core.Rounding = 0; break;
                case 2: miToPInf.Checked = true; label18.Text = miToPInf.Text; roundType = 2; Core.Rounding = 2; break;
                case 3: miToNInf.Checked = true; label18.Text = miToNInf.Text; roundType = 3; Core.Rounding = 3; break;
                case 4: miToNPInf.Checked = true; label18.Text = miToNPInf.Text; break;
                case 1: miToInt.Checked = true; label18.Text = miToInt.Text; roundType = 1; Core.Rounding = 1; break;
            }
        }


        // Information Forms of current format
        private void bpb32Info_Click(object sender, EventArgs e)
        {
            if (!TetraCheck)
            {
                Forms.Formpb32 pb32Info = new Forms.Formpb32();
                pb32Info.Show();
            }
            else
            {
                Forms.Formpb32_16p pb32Info = new Forms.Formpb32_16p();
                pb32Info.Show();
            }
        }


        /*------------------------------------------- JUNK  ------------------------------------------------*/
        private void bMaximizeFooterTab_Click(object sender, EventArgs e)
        {
            if (!isFormatInfoFullView)
            {
                tabControlFooter.Size = new Size(tabControlFooter.Size.Width + 25, 501);
                tabControlFooter.Location = new Point(12, 172);
                bMaximizeFooterTab.BackgroundImage = Properties.Resources.arrow_in;
                isFormatInfoFullView = true;

                // inside controls on Tabs
                richTextBox1.Size = new Size(richTextBox1.Size.Width, 440);
                richTextBox1.Location = new Point(12, 2);

                lbKnownErrors.Size = new Size(lbKnownErrors.Size.Width, 440);
                lbKnownErrors.Location = new Point(12, 2);

                treeView1.Size = new Size(treeView1.Size.Width, 440);
                treeView1.Location = new Point(12, 2);
            }
            else
            {
                tabControlFooter.Size = new Size(tabControlFooter.Size.Width, 134);
                tabControlFooter.Location = new Point(23, 519);
                bMaximizeFooterTab.BackgroundImage = Properties.Resources.arrow_out;
                isFormatInfoFullView = false;

                // inside controls on Tabs
                richTextBox1.Size = new Size(richTextBox1.Size.Width, 119);
                richTextBox1.Location = new Point(1, 2);

                lbKnownErrors.Size = new Size(lbKnownErrors.Size.Width, 95);
                lbKnownErrors.Location = new Point(2, 2);

                treeView1.Size = new Size(treeView1.Size.Width, 100);
                treeView1.Location = new Point(2, 2);
            }

        }

    }// Form class
}// namespace



/* Unused func
        public void refreshOutPutVars()
        {
            switch (tabControl_Format.SelectedIndex)
            {
                case 0:
                    out32Dec = OutPutVars["out32Dec"];
                    out32Bin = OutPutVars["out32Bin"];
                    out32DecE = OutPutVars["out32DecE"];
                    out32BinE = OutPutVars["out32BinE"];
                    break;
                case 1:
                    out64Dec = OutPutVars["out64Dec"];
                    out64Bin = OutPutVars["out64Bin"];
                    out64DecE = OutPutVars["out64DecE"];
                    out64BinE = OutPutVars["out64BinE"];
                    out64Dec_2 = OutPutVars["out64Dec_2"];
                    out64Bin_2 = OutPutVars["out64Bin_2"];
                    out64DecE_2 = OutPutVars["out64DecE_2"];
                    out64BinE_2 = OutPutVars["out64BinE_2"];
                    break;
                case 2:
                    out128Dec = OutPutVars["out128Dec"];
                    out128Bin = OutPutVars["out128Bin"];
                    out128DecE = OutPutVars["out128DecE"];
                    out128BinE = OutPutVars["out128BinE"];
                    out128Dec_2 = OutPutVars["out128Dec_2"];
                    out128Bin_2 = OutPutVars["out128Bin_2"];
                    out128DecE_2 = OutPutVars["out128DecE_2"];
                    out128BinE_2 = OutPutVars["out128BinE_2"];
                    break;
                case 3:
                    out256Dec = OutPutVars["out256Dec"];
                    out256Bin = OutPutVars["out256Bin"];
                    out256DecE = OutPutVars["out256DecE"];
                    out256BinE = OutPutVars["out256BinE"];
                    out256Dec_2 = OutPutVars["out256Dec_2"];
                    out256Bin_2 = OutPutVars["out256Bin_2"];
                    out256DecE_2 = OutPutVars["out256DecE_2"];
                    out256BinE_2 = OutPutVars["out256BinE_2"];
                    break;
            }
        }
        */
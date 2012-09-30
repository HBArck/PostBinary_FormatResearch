using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
namespace Flexible_computing
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
            label2.Text = "Версия [ " + v.Major + "." + v.Minor + "." + v.Build + " ]" + "   Дата [ "+buildDate.Day + "." + buildDate.Month + "."+buildDate.Year+" ]";
        }
    }
}

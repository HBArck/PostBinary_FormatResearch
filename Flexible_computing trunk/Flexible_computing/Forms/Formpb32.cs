using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Flexible_computing.Forms
{
    public partial class Formpb32 : Form
    {
        public Formpb32()
        {
            InitializeComponent();
        }

        private void Formpb32Info_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void Formpb32Info_Leave(object sender, EventArgs e)
        {
            this.SetTopLevel(false);
        }
    }
}

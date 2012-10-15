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
    public partial class PropertiesForm : Form
    {
        public PropertiesForm()
        {
            InitializeComponent();
        }

        private void PropertiesForm_Load(object sender, EventArgs e)
        {
            lPropertiesSaved.Visible = false;
        }

        public void LoadProperties()
        { 

        }

        public void SaveProperties()
        {
            lPropertiesSaved.Visible = true;
        }
    }
}

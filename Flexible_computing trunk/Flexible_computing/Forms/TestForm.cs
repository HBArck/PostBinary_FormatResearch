using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Flexible_computing
{
    public partial class TestForm : Form
    {

        
        public TestForm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lFormats.Items.Count != 0)
            {
                if (!lTest.Items.Contains(lFormats.SelectedItem))
                {
                    lTest.Items.Add(lFormats.SelectedItem);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (lTest.Items.Count != 0)
            {
                lTest.Items.Remove(lTest.SelectedItem);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (lFormats.Items.Count != 0)
            {
                foreach (String oc in lFormats.Items)
                {
                    if (!lTest.Items.Contains(oc))
                        lTest.Items.Add(oc);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rbGenerator.Checked)
            { 

            }
            else
                if (rbFromFile.Checked)
                {
                    if (textBox1.Text != "")
                    {
                        //Load file here
                    }
                    else
                        MessageBox.Show("Choose File to Load Test Numbers");
                }

        }
    }
}

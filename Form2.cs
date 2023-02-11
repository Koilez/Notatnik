using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB5_6
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
           
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;
            if (textBox1.Text != "")
            {
                main.name = textBox1.Text;
                Close();
            }
            else
            {
                MessageBox.Show("Error! File name avoid.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            operation(1, 1, oduzimanje);

            textBox2.PasswordChar = '*';
            textBox2.MaxLength = 12;
        }

        private int operation(int a, int b, Func<int, int, int> inputFun)
        {
            return inputFun(a, b);
        }

        int f1(int a, int b)
        {
            return a + b;
        }

        int f2(int a, int b)
        {
            return a - b;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Form2().Show();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

    }
}

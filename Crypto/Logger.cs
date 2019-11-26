using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crypto
{
    public partial class Logger : Form
    {
        public Logger()
        {
            InitializeComponent();
            richTextBox1.ReadOnly = true;
        }

        private void Logger_Load(object sender, EventArgs e)
        {

        }

        public static void WriteLine(string text)
        {
            richTextBox1.ScrollToCaret();
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.SelectionLength = 0;
            //richTextBox1.SelectionColor = color;
            richTextBox1.SelectedText = "[" + System.DateTime.Now.ToShortTimeString() + "]" + " " + text + "\r\n";
        }

        public static void WriteLine(string text, Color color)
        {
            richTextBox1.ScrollToCaret();
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = color;
            richTextBox1.SelectedText = "[" + System.DateTime.Now.ToShortTimeString() + "]" + " " + text + "\r\n";
        }
    }
}

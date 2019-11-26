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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Logger logger = new Logger();
            logger.StartPosition = FormStartPosition.CenterScreen;
            logger.Size = new Size(logger.Size.Width, logger.Size.Height+400);
            logger.Show();
            this.Hide();

            Cryption.MyAesEncrypt();
        }
    }
}

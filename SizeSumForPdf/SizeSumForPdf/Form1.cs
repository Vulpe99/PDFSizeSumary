using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SizeSumForPdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thank you for using this app!\n" +
                "For the latest version, visit: \n" +
                "https://github.com/Vulpe99/SizeSummaryForPDF \n" +
                "Created by Marius Labau", "About");
        }
    }
}

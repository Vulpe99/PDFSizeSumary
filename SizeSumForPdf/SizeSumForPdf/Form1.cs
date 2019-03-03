using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text.pdf;

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

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<int, int> standardPaper = new Dictionary<int, int>();
            Dictionary<int, int> otherSizes = new Dictionary<int, int>();

            fileNamesTextbox.Clear();
            standardPaper.Clear();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "PDF Files (*.PDF) | *.pdf";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                foreach (var item in ofd.SafeFileNames)
                {
                    fileNamesTextbox.Text += item + "\n";
                }
                foreach (var file in ofd.FileNames)
                {
                    PdfReader reader = new PdfReader(file);
                     
                    for (int i = 0; i < reader.NumberOfPages; i++)
                    {
                        iTextSharp.text.Rectangle rect = reader.GetPageSize(i + 1);
                        int rectLatime = (int)Math.Round(rect.Width / 72 * 25.4f, 0);
                        int rectLungime = (int)Math.Round(rect.Height / 72 * 25.4f, 0);

                        if (rectLatime > rectLungime)
                        {
                            int temp = rectLatime;
                            rectLatime = rectLungime;
                            rectLungime = temp;
                        }
                        //switch (rectLatime)
                        //{
                        //    case 914:
                        //        if (standardPaper.ContainsKey(rectLatime))
                        //        {

                        //        }
                        //        break;
                        //    default:
                        //        break;
                        //}

                        if (standardPaper.ContainsKey(rectLatime))
                        {
                            standardPaper[rectLatime] += rectLungime;
                        }
                        else standardPaper.Add(rectLatime, rectLungime);

                        otherSizesTextbox.Clear();
                        foreach (KeyValuePair<int, int> item in standardPaper)
                        {
                            otherSizesTextbox.Text += $"{item.Key} x {(float)item.Value/1000} \n";
                        }
                    }
                }
            }
        }
    }
}

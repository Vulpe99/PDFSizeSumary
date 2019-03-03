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
//TODO: Add comments for all code chunks;

namespace SizeSumForPdf
{

    public partial class Form1 : Form
    {
        public Dictionary<int, int> standardPaper = new Dictionary<int, int>();
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


            fileNamesTextbox.Clear();
            standardPaper.Clear();
            ClearSizes();
            int filesCount = 0;
            int pagesCount = 0;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "PDF Files (*.PDF) | *.pdf";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                foreach (var item in ofd.SafeFileNames)
                {
                    fileNamesTextbox.Text += item + "\n";
                    filesCount++;
                }
                foreach (var file in ofd.FileNames)
                {
                    PdfReader reader = new PdfReader(file);

                    for (int i = 0; i < reader.NumberOfPages; i++)
                    {
                        pagesCount++;
                        iTextSharp.text.Rectangle rect = reader.GetPageSize(i + 1);
                        int rectLatime = (int)Math.Round(rect.Width / 72 * 25.4f, 0);
                        int rectLungime = (int)Math.Round(rect.Height / 72 * 25.4f, 0);


                        if (rectLatime > rectLungime)
                        {
                            int temp = rectLatime;
                            rectLatime = rectLungime;
                            rectLungime = temp;
                        }


                        if (standardPaper.ContainsKey(rectLatime))
                        {
                            if ((rectLatime > 209 && rectLatime < 217) && (rectLungime >260 && rectLungime <300))
                            {
                                standardPaper[rectLatime] += 1;
                            }
                            else if ((rectLatime > 280 && rectLatime < 305) && (rectLungime > 400 && rectLungime < 430))
                            {
                                standardPaper[rectLatime] += 1;
                            }
                            else if ((rectLatime > 280 && rectLatime < 305) && rectLungime > 440)
                            {
                                standardPaper[9999] += rectLungime;
                            }
                            else standardPaper[rectLatime] += rectLungime;
                        }
                        else
                        {
                            if ((rectLatime > 209 && rectLatime < 217) && (rectLungime > 260 && rectLungime < 300))
                            {
                                standardPaper.Add(rectLatime, 1);
                            }
                            else if ((rectLatime > 280 && rectLatime < 305) && (rectLungime > 400 && rectLungime < 430))
                            {
                                standardPaper.Add(rectLatime, 1);
                            } else if ((rectLatime > 280 && rectLatime < 305) && rectLungime > 440)
                            {
                                standardPaper.Add(9999, rectLungime);
                            }
                            else standardPaper.Add(rectLatime, rectLungime);
                        }
                    }
                }
            }

            foreach (KeyValuePair<int, int> item in standardPaper)
            {
                if (item.Key > 209 && item.Key < 217)
                {
                    textBox_a4.Text += item.Value.ToString() + " + ";
                }
                else if (item.Key > 280 && item.Key < 305)
                {
                    textBox_a3.Text += item.Value.ToString() + " + ";
                }
                else if (item.Key == 9999)
                {
                    textBox_a3Rola.Text += ((float)item.Value / 1000).ToString() + " + ";
                }
                else if (item.Key > 412 && item.Key < 435)
                {
                    textBox_a2.Text += ((float)item.Value / 1000).ToString() + " + ";
                }
                else if (item.Key > 570 && item.Key < 599)
                {
                    textBox_a1.Text += ((float)item.Value / 1000).ToString() + " + ";
                }
                else if (item.Key > 800 && item.Key < 855)
                {
                    textBox_a0.Text += ((float)item.Value / 1000).ToString() + " + ";
                }
                else if (item.Key > 856 && item.Key < 920)
                {
                    textBox_a0plus.Text += ((float)item.Value / 1000).ToString() + " + ";
                } else
                {
                    otherSizesTextbox.Text += $"{item.Key}mm x {(float)item.Value / 1000}m \n";
                }
            }



            //Show a message for succesful job
            MessageBox.Show($"Done! We parsed trough {filesCount} files and {pagesCount} pages. \n" +
                $"You can click on Copy buttons and paste to Excel!", "Succes!");
            filesLabel.Text = filesCount.ToString();
            pagesLabel.Text = pagesCount.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fileNamesTextbox.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearSizes();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(standardPaper.Count * 5);
            string rand = "";
            sb.AppendLine("Page size\tLength (m)");
            foreach (KeyValuePair<int, int> kvp in standardPaper)
            {
                if (kvp.Key==210)
                {
                    rand = "A4" + "\t" + kvp.Value; 
                } else if(kvp.Key==297)
                {
                    rand = "A3" + "\t" + kvp.Value;
                } else if(kvp.Key==9999)
                {
                    rand = "A3 rola" + "\t" + kvp.Value;
                }
                else
                { 
                rand = kvp.Key + "\t" + (float)kvp.Value/1000;
                }
                sb.AppendLine(rand);
            }
            if (sb.Length > 0)
            {
                Clipboard.Clear();
                Clipboard.SetText(sb.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!(fileNamesTextbox.Text == null))
            {
                Clipboard.Clear();
                Clipboard.SetText(fileNamesTextbox.Text);
            }
        }

        private void ClearSizes() {
            otherSizesTextbox.Clear();
            textBox_a4.Clear();
            textBox_a3.Clear();
            textBox_a3Rola.Clear();
            textBox_a2.Clear();
            textBox_a1.Clear();
            textBox_a0.Clear();
            textBox_a0plus.Clear();
        }
    }
}

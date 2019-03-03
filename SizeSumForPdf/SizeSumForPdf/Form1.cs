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
            otherSizesTextbox.Clear();
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
                                if (rectLatime == 210 && rectLungime == 297)
                            {
                                standardPaper[rectLatime] += 1;
                            }
                                else if(rectLatime == 297 && rectLungime == 420)
                                {
                                standardPaper[rectLatime] += 1;
                            }
                            else standardPaper[rectLatime] += rectLungime;
                            }
                            else
                        {
                            if (rectLatime == 210 && rectLungime == 297)
                            {
                                standardPaper.Add(rectLatime, 1);
                            }
                            else if (rectLatime == 297 && rectLungime == 420)
                            {
                                standardPaper.Add(rectLatime, 1);
                            }
                            else standardPaper.Add(rectLatime, rectLungime);
                            }
                        





                    }
                }
            }



            
           
            foreach (KeyValuePair<int, int> item in standardPaper)
            {
                if (item.Key == 210)
                {
                    otherSizesTextbox.Text += $"A4 = {item.Value} pages \n";
                }
                else if (item.Key == 297 && item.Value == 420)
                {
                    otherSizesTextbox.Text += $"A3 = {item.Value} pages \n";
                }
                else
                {
                    otherSizesTextbox.Text += $"{item.Key}mm x {(float)item.Value / 1000}m \n";
                }

                switch (item.Key)
                {
                    case 211:
                    case 210:
                    case 209:
                        textBox_a4.Text += item.Value.ToString();
                        break;
                    case 298:
                    case 297:
                    case 296:
                        textBox_a3.Text += item.Value.ToString();
                        break;
                    case 421:
                    case 420:
                    case 419:
                        textBox_a2.Text += ((float)item.Value/1000).ToString() + " + ";
                        break;
                    case 595:
                    case 594:
                    case 593:
                        textBox_a1.Text += ((float)item.Value / 1000).ToString() + " + ";
                        break;
                    case 842:
                    case 841:
                    case 840:
                        textBox_a0.Text += ((float)item.Value / 1000).ToString() + " + ";
                        break;
                    case 915:
                    case 914:
                    case 913:
                        textBox_a0plus.Text += ((float)item.Value / 1000).ToString() + " + ";
                        break;
                    default:
                        break;
                }
            }



            //Show a message for succesful job
            MessageBox.Show($"Done! We parsed trough {filesCount} files and {pagesCount} pages. \n" +
                $"You can click on Copy buttons and paste to Excel!", "Succes!");
            filesLabel.Text = filesCount.ToString();
            pagesLabel.Text = pagesCount.ToString();
        }
    }
}

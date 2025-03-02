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

namespace notatnik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void zamknijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();

            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            list.Add(line);
                        }
                        textBox1.Lines = list.ToArray();
                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Błąd odczytu pliku" + ex.Message);
            }           
        }

        private void zapiszJakoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nazwa = openFileDialog1.FileName;
            if (nazwa.Length > 0) saveFileDialog1.FileName = nazwa;
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    nazwa = saveFileDialog1.FileName;
                    ZapiszDoTxt(nazwa, textBox1.Lines);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu pliku" + ex.Message);
            }
        }

        private static void ZapiszDoTxt(string nazwa, string[] text)
        {
            using (StreamWriter sw = new StreamWriter(nazwa))
            {
                foreach (string line in text)
                {
                    sw.WriteLine(line);
                }
            }
        }

        private void czcionkaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = textBox1.Font;
            fontDialog1.Color = textBox1.ForeColor;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Font = fontDialog1.Font;
                textBox1.ForeColor = fontDialog1.Color;
            }
        }

        private void tłoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = textBox1.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.BackColor = colorDialog1.Color;
            }
        }

        private void cofnijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Undo();
        }

        private void wytnijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Cut();
        }

        private void kopiujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        private void wklejToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Paste();
        }

        private void usuńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = "";
        }

        private void zaznaczWszystkoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }
    }
}

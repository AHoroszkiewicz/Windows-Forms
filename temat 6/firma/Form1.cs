using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace firma
{
    public partial class Form1 : Form
    {
        const string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Studia\Windows Forms\temat 6\firma\firma.mdf;Integrated Security=True;Connect Timeout=30";
        static DataContext bazaDanychFirma = new DataContext(connectionString);
        static Table<Pracownik> listaPracownikow = bazaDanychFirma.GetTable<Pracownik>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = "Lista Pracowników: " + "\n";
            var lp = from Pracownik in listaPracownikow select Pracownik;
            foreach (var p in lp)
            {
                s += p.Imię + " "+ p.Nazwisko + " " + p.Email + " " + p.Telefon + "\n";
            }
            MessageBox.Show(s);
        }
    }
}

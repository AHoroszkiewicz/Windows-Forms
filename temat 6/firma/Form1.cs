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
        static Table<FakturaSprzedazy> listaFaktur = bazaDanychFirma.GetTable<FakturaSprzedazy>();
        static Table<Kontrahent> listaKontrahentow = bazaDanychFirma.GetTable<Kontrahent>();
        private bool ostatniGridToPracownik = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void listaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lp = from Pracownik in listaPracownikow
                     orderby Pracownik.Nazwisko
                     select new
                     {
                         Pracownik.Id,
                         Pracownik.Imię,
                         Pracownik.Nazwisko,
                         Pracownik.Email,
                         Pracownik.Telefon
                     };
            dataGridView1.DataSource = lp;
        }

        private void nowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int noweId = listaPracownikow.Max(p => p.Id) + 1;
            Pracownik nowyPracownik = new Pracownik
            {
                Id = noweId,
                Imię = textBoxImie.Text,
                Nazwisko = textBoxNazwisko.Text,
                Email = textBoxEmail.Text,
                Telefon = textBoxTelefon.Text
            };
            listaPracownikow.InsertOnSubmit(nowyPracownik);
            bazaDanychFirma.SubmitChanges();
            listaToolStripMenuItem_Click(this, null);
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;
            ostatniGridToPracownik = true;

            int wiersz = dataGridView1.CurrentRow.Index;
            int id = (int)dataGridView1.Rows[wiersz].Cells[0].Value;
            string imie = dataGridView1.Rows[wiersz].Cells[1].Value.ToString();
            string nazwisko = dataGridView1.Rows[wiersz].Cells[2].Value.ToString();
            string email = dataGridView1.Rows[wiersz].Cells[3].Value.ToString();
            string telefon = dataGridView1.Rows[wiersz].Cells[4].Value.ToString();

            textBoxImie.Text = imie;
            textBoxNazwisko.Text = nazwisko;
            textBoxEmail.Text = email;
            textBoxTelefon.Text = telefon;
            textBoxPracownikId.Text = id.ToString();

            var lf = from FakturaSprzedazy in listaFaktur
                 where FakturaSprzedazy.PracownikId == id
                 orderby FakturaSprzedazy.Id
                 select new
                 {
                     FakturaSprzedazy.Id,
                     FakturaSprzedazy.PracownikId,
                     FakturaSprzedazy.KontrahentId,
                     FakturaSprzedazy.Numer,
                     FakturaSprzedazy.Netto,
                     FakturaSprzedazy.Vat,
                     FakturaSprzedazy.Data,
                     FakturaSprzedazy.Zaplacono
                 };

            if (checkBox1.Checked)
            {
                lf = lf.Where(f => f.Zaplacono < f.Netto + f.Vat);
            }

            dataGridView2.DataSource = lf;
        }

        private void usuńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;
            int wiersz = dataGridView1.CurrentRow.Index;
            int id = (int)dataGridView1.Rows[wiersz].Cells[0].Value;
            IEnumerable<Pracownik> usunPracownik = from Pracownik in listaPracownikow
                                                   where Pracownik.Id == id
                                                   select Pracownik;
            if (usunPracownik != null)
            {
                listaPracownikow.DeleteAllOnSubmit(usunPracownik);
                bazaDanychFirma.SubmitChanges();
                listaToolStripMenuItem_Click(this, null);
            }
        }

        private void edycjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;
            int wiersz = dataGridView1.CurrentRow.Index;
            int id = (int)dataGridView1.Rows[wiersz].Cells[0].Value;
            foreach (Pracownik p in listaPracownikow)
            {
                if (p.Id == id)
                {
                    p.Imię = textBoxImie.Text;
                    p.Nazwisko = textBoxNazwisko.Text;
                    p.Email = textBoxEmail.Text;
                    p.Telefon = textBoxTelefon.Text;
                }
            }
            bazaDanychFirma.SubmitChanges();
            listaToolStripMenuItem_Click(this, null);
        }

        private void dodajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int noweId = listaFaktur.Max(f => f.Id) + 1;
            if (dataGridView1.CurrentRow == null)
                return;
            int idPracownika = (int)dataGridView1.CurrentRow.Cells[0].Value;
            string paddedId = noweId.ToString().PadLeft(2, '0');
            FakturaSprzedazy nowaFaktura = new FakturaSprzedazy
            {
                Id = noweId,
                Numer = "V/"+ paddedId +"/25",
                Netto = double.Parse(textBoxNetto.Text),
                Vat = double.Parse(textBoxNetto.Text)*0.23,
                Data = DateTime.Now,
                Zaplacono = double.Parse(textBoxZaplacono.Text),
                KontrahentId = int.Parse(textBoxKontrahentId.Text),
                PracownikId = idPracownika
            };
            listaFaktur.InsertOnSubmit(nowaFaktura);
            bazaDanychFirma.SubmitChanges();
            dataGridView1_Click(this, null);
        }

        private void edytujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
                return;
            int wiersz = dataGridView2.CurrentRow.Index;
            int id = (int)dataGridView2.Rows[wiersz].Cells[0].Value;
            foreach (FakturaSprzedazy f in listaFaktur)
            {
                if (f.Id == id)
                {
                    f.Netto = double.Parse(textBoxNetto.Text);
                    f.Vat = double.Parse(textBoxNetto.Text) * 0.23;
                    f.Zaplacono = double.Parse(textBoxZaplacono.Text);
                    f.PracownikId = int.Parse(textBoxPracownikId.Text);
                    f.KontrahentId = int.Parse(textBoxKontrahentId.Text);
                }
            }
            bazaDanychFirma.SubmitChanges();
            dataGridView1_Click(this, null);
        }

        private void dataGridView2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
                return;
            int wiersz = dataGridView2.CurrentRow.Index;
            int id = (int)dataGridView2.Rows[wiersz].Cells[0].Value;
            string netto = dataGridView2.Rows[wiersz].Cells[4].Value.ToString();
            string zaplacono = "";
            if (dataGridView2.Rows[wiersz].Cells[7].Value != null)
            {
                zaplacono = dataGridView2.Rows[wiersz].Cells[7].Value.ToString();
            }
            string pracownikId = dataGridView2.Rows[wiersz].Cells[1].Value.ToString();
            string kontrahentId = dataGridView2.Rows[wiersz].Cells[2].Value.ToString();

            textBoxNetto.Text = netto;
            textBoxZaplacono.Text = zaplacono;
            textBoxPracownikId.Text = pracownikId;
            textBoxKontrahentId.Text = kontrahentId;
        }

        private void usuńToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
                return;
            int wiersz = dataGridView2.CurrentRow.Index;
            int id = (int)dataGridView2.Rows[wiersz].Cells[0].Value;
            IEnumerable<FakturaSprzedazy> usunFaktura = from FakturaSprzedazy in listaFaktur
                                                        where FakturaSprzedazy.Id == id
                                                        select FakturaSprzedazy;
            if (usunFaktura != null)
            {
                listaFaktur.DeleteAllOnSubmit(usunFaktura);
                bazaDanychFirma.SubmitChanges();
                if (ostatniGridToPracownik)
                    dataGridView1_Click(this, null);
                else
                    dataGridView3_Click(this, null);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ostatniGridToPracownik)
                dataGridView1_Click(this, null);
            else
                dataGridView3_Click(this, null);
        }

        private void listaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var lk = from Kontrahent in listaKontrahentow
                     orderby Kontrahent.Nazwa
                     select new
                     {
                         Kontrahent.Id,
                         Kontrahent.Nazwa,
                         Kontrahent.Nip,
                         Kontrahent.Ulica,
                         Kontrahent.Miasto,
                     };
            dataGridView3.DataSource = lk;
        }

        private void dodajToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int noweId = listaKontrahentow.Max(k => k.Id) + 1;
            Kontrahent nowyKontrahent = new Kontrahent
            {
                Id = noweId,
                Nazwa = textBoxNazwa.Text,
                Nip = textBoxNip.Text,
                Ulica = textBoxUlica.Text,
                Miasto = textBoxMiasto.Text
            };
            listaKontrahentow.InsertOnSubmit(nowyKontrahent);
            bazaDanychFirma.SubmitChanges();
            listaToolStripMenuItem1_Click(this, null);
        }

        private void edytujToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow == null)
                return;
            int wiersz = dataGridView3.CurrentRow.Index;
            int id = (int)dataGridView3.Rows[wiersz].Cells[0].Value;
            foreach (Kontrahent k in listaKontrahentow)
            {
                if (k.Id == id)
                {
                    k.Nazwa = textBoxNazwa.Text;
                    k.Nip = textBoxNip.Text;
                    k.Ulica = textBoxUlica.Text;
                    k.Miasto = textBoxMiasto.Text;
                }
            }
            bazaDanychFirma.SubmitChanges();
            listaToolStripMenuItem1_Click(this, null);
        }

        private void dataGridView3_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow == null)
                return;
            ostatniGridToPracownik = false;

            int wiersz = dataGridView3.CurrentRow.Index;
            int id = (int)dataGridView3.Rows[wiersz].Cells[0].Value;
            string nazwa = dataGridView3.Rows[wiersz].Cells[1].Value.ToString();
            string nip = dataGridView3.Rows[wiersz].Cells[2].Value.ToString();
            string ulica = dataGridView3.Rows[wiersz].Cells[3].Value.ToString();
            string miasto = dataGridView3.Rows[wiersz].Cells[4].Value.ToString();
            textBoxNazwa.Text = nazwa;
            textBoxNip.Text = nip;
            textBoxUlica.Text = ulica;
            textBoxMiasto.Text = miasto;
            textBoxKontrahentId.Text = id.ToString();

            var lf = from FakturaSprzedazy in listaFaktur
                     where FakturaSprzedazy.KontrahentId == id
                     orderby FakturaSprzedazy.Id
                     select new
                     {
                         FakturaSprzedazy.Id,
                         FakturaSprzedazy.PracownikId,
                         FakturaSprzedazy.KontrahentId,
                         FakturaSprzedazy.Numer,
                         FakturaSprzedazy.Netto,
                         FakturaSprzedazy.Vat,
                         FakturaSprzedazy.Data,
                         FakturaSprzedazy.Zaplacono
                     };
            if (checkBox1.Checked)
            {
                lf = lf.Where(f => f.Zaplacono < f.Netto + f.Vat);
            }
            dataGridView2.DataSource = lf;
        }
    }
}

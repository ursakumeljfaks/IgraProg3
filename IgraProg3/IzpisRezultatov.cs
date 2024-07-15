using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IgraProg3
{
    public partial class IzpisRezultatov : Form
    {
        string istiVzdevek;
        Uporabnik uporabnik;
        public IzpisRezultatov(string vzdevek, int poskusi, int cas)
        {
            InitializeComponent();

            vzdevekTextBox.Text = vzdevek;
            casTextBox.Text = cas.ToString();
            poskusiTextBox.Text = poskusi.ToString();
            tockeTextBox.Text = (cas + 5 * poskusi).ToString();

            // izpis top5 časov v textBox
            uporabnik = new Uporabnik(vzdevek, poskusi, cas);
            top5TextBox.Text = uporabnik.top5();

            istiVzdevek = vzdevek;
        }

        private void ponovnaIgraGumb_Click(object sender, EventArgs e)
        {
            Form igra = new Igra(istiVzdevek);
            this.Hide(); // Skrije okno z rezultati
            igra.ShowDialog(); // pokazemo igralno povrsino
            this.Close(); // zapre okno rezultati
        }

        private void izbrisiRezultateGumb_Click(object sender, EventArgs e)
        {
            uporabnik.pobrisiTabele();
            MessageBox.Show("Uspešno izbrisani tabeli Uporabniki in Rezultati!");
            top5TextBox.Text = string.Empty;
        }
    }
}

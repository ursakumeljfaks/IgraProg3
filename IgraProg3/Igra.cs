using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Xml.Linq;
using System.Media;

namespace IgraProg3
{
    public partial class Igra : Form
    {

        Random random = new Random();
        private Dictionary<string, Image> slikiceSlovar = new Dictionary<string, Image>();
        private List<string> slikiceKljuci = new List<string>();
        SoundPlayer soundPlayer;

        Label prviKlik, drugiKlik;

        private int pretecenCas = 0;
        private int stPoskusov = 0;
        private string vzdevek;

        public Igra(string ime)
        {
            InitializeComponent();
            vzdevek = ime;

            PrirediSlikice();
            timerIgre.Start();
        }

        /// <summary>
        /// timer uporabljen takrat, ko nismo nasli para, ker ponastavi prviKlik in drugiKlik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // vse slike na labelih prviKlik in drugiKlik nastavimo na null in prav tako label-a sama nastavimo na null
            timer1.Stop();

            prviKlik.Image = null;
            drugiKlik.Image = null;

            prviKlik = null;
            drugiKlik = null;

            stPoskusov++;
            stPoskusovLabel.Text = $"Število neuspešnih poskusov: {stPoskusov}";
        }

        /// <summary>
        /// pregleda ali ima vsak label svojo sliko, če jo ima je igre konec, ustvari novega uporabnika s pridobljenimi rezultati, ustvari okno za izpis, predvaja zvok in skrije to okno za igro
        /// </summary>
        private void AliJeKonecIgre()
        {
            Label label;
            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                label = (Label)tableLayoutPanel1.Controls[i];

                // slikica je null, kar pomeni, da se ni konec igre
                if (label.Image == null)
                    return;
            }
            // dodamo novega uporabnika v bazo oz posodobimo rezultat
            Uporabnik novUporabnik = new Uporabnik(vzdevek, stPoskusov, pretecenCas);
            novUporabnik.UstvariBazoCeJeNi();
            novUporabnik.dodaj();
            // ustvarimo form izpis rezultatov
            IzpisRezultatov rezultati = new IzpisRezultatov(vzdevek, stPoskusov, pretecenCas);
            // zvok aplavza za konec igre
            soundPlayer = new SoundPlayer(Properties.Resources.applause_sound);//new SoundPlayer("applause_sound.wav");
            soundPlayer.Play();
            // ustavimo timer igre
            timerIgre.Stop();
            // skrijemo form igra
            this.Hide();
            // prikazemo form rezultatov
            rezultati.ShowDialog();
            // zapremo form igra
            this.Close();
        }

        /// <summary>
        /// slike iz imenika Images nalozi v slovar (slikiceSlovar) {imeSlikeBrezKončnice : Image(te slike)} in 2x doda v seznam (slikiceKljuci) imena slik
        /// </summary>
        private void NaloziSlikice()
        {
            string[] potiSlikic = Directory.GetFiles("Images");

            foreach (string pot in potiSlikic)
            {
                string kljuc = Path.GetFileNameWithoutExtension(pot); // za kljuc si shranimo samo ime slike brez koncnice
                Image image = Image.FromFile(pot); // ustvari sliko 

                slikiceSlovar.Add(kljuc, image);
                slikiceKljuci.Add(kljuc); // dodamo 2x v slovar imena slik, ker moramo imeti vsako sliko 2x
                slikiceKljuci.Add(kljuc);
            }
        }

        /// <summary>
        /// vsakemu label-u na tableLayoutPanel1 priredi oz. dodeli lastnost Tag, ki je ime slike
        /// </summary>
        private void PrirediSlikice()
        {
            NaloziSlikice();

            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label label = (Label)control;
                if (label != null)
                {
                    int rndStevilo = random.Next(0, slikiceKljuci.Count);
                    string kljuc = slikiceKljuci[rndStevilo];
                    label.Tag = kljuc; // vsakemu label-u v tableLayoutPanel1 dodelimo lastnost Tag, ki je naključno ime slike iz seznama imen slik
                    slikiceKljuci.RemoveAt(rndStevilo); // odstranimo sliko iz seznama, ker je ne smemo več uporabiti
                }
            }
        }

        /// <summary>
        /// timer celotne igre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerIgre_Tick(object sender, EventArgs e)
        {
            pretecenCas++;
            casLabel.Text = $"Pretečeni čas: {pretecenCas}";
        }

        /// <summary>
        /// vsakemu label-u prikaže sliko
        /// </summary>
        /// <param name="label">label na tableLayoutPanel1</param>
        private void PrikaziSliko(Label label)
        {
            string kljuc = label.Tag.ToString();
            label.Image = slikiceSlovar[kljuc];
        }

        /// <summary>
        /// odzivna metoda na dogodek Click posameznega label-a v tableLayoutPanel1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_Click(object sender, EventArgs e)
        {
            // preprečimo, da uporabnik ne bo mogel v času izvajanja timerja, klikniti več kot dvakrat
            if (prviKlik != null && drugiKlik != null)
                return;

            Label labelKlik = (Label)sender;

            // preprečimo, da uporabnik ne more klikniti na slikico, ki je že odprta oz. da ne uniči programa
            if (labelKlik == null || labelKlik.Image != null) { return; }

            // če nismo še nič kliknili prvič, nastavimo prviKlik na to sliko in jo prikažemo
            if (prviKlik == null)
            {
                prviKlik = labelKlik;
                // da vidimo, kaj smo kliknili
                PrikaziSliko(labelKlik);
                return;
            }

            // v drugem kliku skranimo v drugiKlik to sliko in jo prikažemo
            drugiKlik = labelKlik;
            PrikaziSliko(labelKlik);

            // preverimo, če so morda že najdeni vsi pari
            AliJeKonecIgre();

            // če smo našli par
            if (prviKlik.Tag.ToString() == drugiKlik.Tag.ToString())
            {
                // resetira oba labela prviKlik in drugiKlik ter predvaja zvok za uspešnost 
                soundPlayer = new SoundPlayer(Properties.Resources.correct_sound);//new SoundPlayer("correct_sound.wav");
                soundPlayer.Play();
                prviKlik = null;
                drugiKlik = null;
            }
            else // če para nismo našli
            {
                // predvajamo zvok, ker nismo našli para
                soundPlayer = new SoundPlayer(Properties.Resources.error_sound); //new SoundPlayer("error_sound.wav");
                soundPlayer.Play();
                // pričnemo timer1, ki pokliče dogodek Tick, kjer se resetirajo slike in prviKlik ter drugiKlik (po domače s tem poskrbimo, da se skrijejo vse slikic)
                timer1.Start();
            }
        }


    }
}

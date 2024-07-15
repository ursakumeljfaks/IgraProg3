using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IgraProg3
{
    public partial class Vpis : Form
    {

        public Vpis()
        {
            InitializeComponent();
            if (obstojSlik() is true)
            {
                obstojSlikTextBox.Text = "Imenik slik lahko zamenjaš!";
                slikeButton.Text = "Zamenjaj imenik";
            }
            else
            {
                obstojSlikTextBox.Text = "V programu je imenik slik prazen!";
                slikeButton.Text = "Naloži imenik";
            }
        }

        /// <summary>
        /// glavni gumb Igraj
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void igrajGumb_Click(object sender, EventArgs e)
        {
            // ce vzdevka ni, uporabniku sporoci in mora ponovno vpisati
            if (string.IsNullOrWhiteSpace(vzdevek.Text))
            {
                MessageBox.Show("Vnesite vzdevek, preden začnete igro.");
                return;
            }

            // ustvarimo igralno povrsino z vnesenim vzdevkom
            Form igra = new Igra(vzdevek.Text);
            this.Hide(); // Skrije okno za vpis
            igra.ShowDialog(); // pokazemo igralno povrsino
            this.Close(); // Zapre okno za vpis
        }

        /// <summary>
        /// preveri ali so v imeniku Images kakšne slike, ker če so lahko igramo, drugače jih moramo prej naložiti
        /// </summary>
        /// <returns></returns>
        private bool obstojSlik()
        {
            string shraniSlike = @"Images"; // imenik Images

            if (Directory.Exists(shraniSlike)) // preverimo ali imenik Images obstaja
            {
                // preverimo ali imenik vsebuje kakršne koli datoteke
                if (Directory.GetFiles(shraniSlike).Length > 0)
                {
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// metoda za klik na gumb za nalaganje imenika slik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slikeButton_Click(object sender, EventArgs e)
        {
            string shraniSlike = @"Images";

            // FolderBrowserDialog je za izbiro celotnega imenika oz. mape
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                // prikaz ibirnika imenikov oz. map
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    // ce mapo Images že obstaja, jo najprej pobrišemo, zato da se nam ne bo igra sesula
                    if (Directory.Exists(shraniSlike)) 
                    {
                        Directory.Delete(shraniSlike, true); 
                    }
                    // ustvarimo imenik Images
                    Directory.CreateDirectory(shraniSlike);

                    // dobimo pot izbranega imenika
                    string izbraniImenik = fbd.SelectedPath;

                    // pridobimo vse datoteke iz izbranega imenika
                    string[] slike = Directory.GetFiles(izbraniImenik, "*.*", SearchOption.TopDirectoryOnly) // iščemo v trenutni mapi vse možne datoteke
                                              .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                          s.EndsWith(".png", StringComparison.OrdinalIgnoreCase)).ToArray(); // izmed vseh datotek v imeniku izberemo le tiste s končnico jpg in png ne glede na velikost črk
                                                                                                                            // in pretvorimo v string[]

                    if (slike.Length > 8)
                    {
                        MessageBox.Show("Slik mora biti največ 8!");
                        return;
                    }

                    foreach (string slika in slike)
                    {
                        string fileName = Path.GetFileName(slika); // pridobimo ime datoteke s sliko
                        string fileSavePath = Path.Combine(shraniSlike, fileName); // zdruzi imenik Images z imenom slike => Images\slika.jpg
                        // Bitmap razred vsebuje informacije o pikslih neke slike
                        using (Bitmap originalImage = new Bitmap(slika)) 
                        {
                            // nastavimo novo višino in širino 
                            int newWidth = 100; 
                            int newHeight = 100;
                            using (Bitmap resizedImage = prirediVelikost(originalImage, newWidth, newHeight)) // priredimo novo sliko višini in širini
                            {
                                resizedImage.Save(fileSavePath); // shranimo to novo sliko v Images
                            }
                        }
                    }

                    MessageBox.Show("Uspešno naložene slike!");
                }
            }
            // po nalozenem imeniku pise na gumbu, da slikice zamenjamo
            if (obstojSlik() is true)
            {
                obstojSlikTextBox.Text = "Slike lahko zamenjaš!";
                slikeButton.Text = "Zamenjaj imenik slik!";
            }

        }

        /// <summary>
        /// vrne novo Bitmap sliko z določenimi dimenzijami
        /// </summary>
        /// <param name="slika">slika, ki jo želimo spremeniti na novo širino in dolžino</param>
        /// <param name="sirina">sirina nove slike</param>
        /// <param name="visina">visina nove slike</param>
        /// <returns>nova Bitmap slika</returns>
        private Bitmap prirediVelikost(Image slika, int sirina, int visina)
        {
            // ustvarimo pravokotnik kamor se bo izrisala nova slika
            Rectangle pravokotnik = new Rectangle(0, 0, sirina, visina);
            Bitmap novaSlika = new Bitmap(sirina, visina);

            // nastavimo ločljivost nove slike na enako kot jo je imela stara
            novaSlika.SetResolution(slika.HorizontalResolution, slika.VerticalResolution);

            using (Graphics g = Graphics.FromImage(novaSlika)) // omogoča risanje na sliko novaSlika
            {
                g.CompositingMode = CompositingMode.SourceCopy; // nova slika bo prepisala vsebino stare slike
                g.CompositingQuality = CompositingQuality.HighQuality; // nastavimo grafiko nove slike na najboljšo kakovost
                g.InterpolationMode = InterpolationMode.HighQualityBicubic; // s tem nastavimo način interpolacije za prilagajanje velikosti slike
                g.SmoothingMode = SmoothingMode.HighQuality; // s tem nastavimo način glajenja za grafiko
                g.PixelOffsetMode = PixelOffsetMode.HighQuality; // s tem nastavimo način odmika pikslov

                // ImageAttributes se uporablja za upravljanje lastnosti risanja slike
                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY); // to preprečuje vidne črte na robovih slike pri prilagajanju velikosti
                    g.DrawImage(slika, pravokotnik, 0, 0, slika.Width, slika.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return novaSlika;
        }
    }
}

using System;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace IgraProg3
{
    public class Uporabnik
    {
        private readonly string povezovalniNiz = $"Data Source=baza.sqlite;Version=3;";
        private string Vzdevek { get; set; }
        private int Poskusi { get; set; }
        private int Cas { get; set; }

        public Uporabnik(string vzdevek, int poskusi, int cas)
        {
            Vzdevek = vzdevek;
            Poskusi = poskusi;
            Cas = cas;
        }

        /// <summary>
        /// metoda pobrise vse rezultate v tabelah Uporabniki in Rezultati
        /// </summary>
        public void pobrisiTabele()
        {
            try
            {
                using (var conn = new SQLiteConnection(povezovalniNiz))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"DELETE from Uporabniki";
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"DELETE from Rezultati";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// metoda vrne username z vzdevkom vzdevek in vrne njegov cas v primeru, da ta uporabnik obstaja
        /// </summary>
        /// <param name="vzdevek"></param>
        /// <param name="prejsnjiCas"></param>
        /// <returns></returns>
        public string uporabnikInCas(string vzdevek, out int prejsnjiCas)
        {
            prejsnjiCas = 0;
            string uporabnik = "";

            try
            {
                using (var conn = new SQLiteConnection(povezovalniNiz))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"SELECT username, cas FROM Uporabniki WHERE username=@vzdevek";
                        cmd.Parameters.AddWithValue("@vzdevek", vzdevek);

                        using (SQLiteDataReader rezultat = cmd.ExecuteReader())
                        {
                            if (rezultat.Read())
                            {
                                uporabnik = rezultat["username"].ToString();
                                prejsnjiCas = int.Parse(rezultat["cas"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            return uporabnik;
        }

        /// <summary>
        /// metoda za dodajanje ali posodabljanje uporabnika v bazi
        /// </summary>
        public void dodaj()
        {
            try
            {
                using (var conn = new SQLiteConnection(povezovalniNiz))
                {
                    conn.Open();

                    // pridobimo uporabnika in njegov cas iz baze
                    string uporabnik = uporabnikInCas(Vzdevek, out int prejsnjiCas);

                    // ce je trenutni cas boljsi od prejsnjega tabelo posodobi obe tabeli
                    if (!string.IsNullOrEmpty(uporabnik) && prejsnjiCas > Cas)
                    {
                        using (var cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = @"UPDATE Uporabniki SET poskusi=@poskusi, cas=@cas WHERE username=@vzdevek";
                            cmd.Parameters.AddWithValue("@vzdevek", Vzdevek);
                            cmd.Parameters.AddWithValue("@poskusi", Poskusi);
                            cmd.Parameters.AddWithValue("@cas", Cas);
                            cmd.ExecuteNonQuery();
                        }

                        using (var cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = @"UPDATE Rezultati SET tocke=@tocke WHERE username=@vzdevek";
                            cmd.Parameters.AddWithValue("@vzdevek", Vzdevek);
                            cmd.Parameters.AddWithValue("@tocke", Cas + 5 * Poskusi);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    // ce je uporabnik ze v bazi, ampak ni pridobil boljsega casa naj zakljuci metodo
                    else if (!string.IsNullOrEmpty(uporabnik) && prejsnjiCas < Cas)
                    {
                        return;
                    }
                    // uporabnika ce ga se ni doda v bazo
                    else
                    {
                        using (var cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = @"INSERT INTO Uporabniki (username, poskusi, cas) VALUES (@vzdevek, @poskusi, @cas)";
                            cmd.Parameters.AddWithValue("@vzdevek", Vzdevek);
                            cmd.Parameters.AddWithValue("@poskusi", Poskusi);
                            cmd.Parameters.AddWithValue("@cas", Cas);
                            cmd.ExecuteNonQuery();
                        }

                        using (var cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = @"INSERT INTO Rezultati (username, tocke) VALUES (@vzdevek, @tocke)";
                            cmd.Parameters.AddWithValue("@vzdevek", Vzdevek);
                            cmd.Parameters.AddWithValue("@tocke", Cas + 5 * Poskusi);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// pridobi 5 najboljših rezultatov iz baze po času
        /// </summary>
        /// <returns>vrne niz z vsemi podatki</returns>
        public string top5()
        {
            try
            {
                StringBuilder vrstica = new StringBuilder();
                using (var conn = new SQLiteConnection(povezovalniNiz))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"SELECT username, tocke FROM Rezultati ORDER BY tocke LIMIT 5";

                        using (SQLiteDataReader rezultat = cmd.ExecuteReader())
                        {

                            if (rezultat.HasRows)
                            {
                                int i = 1;
                                while (rezultat.Read())
                                { 
                                    vrstica.AppendLine($"{i}. {rezultat["username"]}: {rezultat["tocke"]} sekund");
                                    i++;
                                }
                            }

                        }
                    }
                    return vrstica.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }

        }
    

}
}

using System.Data;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Collections.ObjectModel;


namespace Projekat
{
    public class Artikal
    {
        private int broj;
        private int sifra;
        private string naziv;
        private float cena;
        private int kolicina;
        
        private int naStanju;
        private string napomena;

        private int pomocniINDEX;
    

        public int Sifra { get => sifra; set => sifra = value; }
        public string Naziv { get => naziv; set => naziv = value; }
        public float Cena { get => cena; set => cena = value; }
        public int Kolicina { get => kolicina; set => kolicina = value; }
        public string Napomena { get => napomena; set => napomena = value; }
        public int NaStanju { get => naStanju; set => naStanju = value; }
        public int Broj { get => broj; set => broj = value; }
        public int PomocniINDEX { get => pomocniINDEX; set => pomocniINDEX = value; }

        public Artikal()
        {
            Sifra = 0;
            Naziv = "";
            Cena = 0;
            Kolicina = 0;
            
            Napomena = "";
        }

        

        public static string table = "Artikli";

        public static DataTable getArticles()
        {
            DataSet dsArtikli = Konekcija.executeQuery("Select * from " + (Artikal.table) + "  WHERE Datum_brisanja IS NULL;", Artikal.table);
            return dsArtikli.Tables[Artikal.table];
        }

        public static bool Delete(int ArtikalID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlDELETE = "UPDATE " + Artikal.table + " SET Datum_brisanja = date('now') WHERE ID = @ID";
            SQLiteCommand komanda = new SQLiteCommand(sqlDELETE, konekcija);
            komanda.Parameters.AddWithValue("@ID", ArtikalID);
            try
            {
                konekcija.Open();

                komanda.ExecuteNonQuery();
                return true;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;

            }
            finally
            {
                konekcija.Close();
            }
        }

        public static bool Update(int sifra, string naziv, float cena, int kolicina, string tekstNapomene, int napomenaID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);

            string sql = "UPDATE Artikli set  Naziv=@Naziv, Cena=@Cena, Kolicina=@Kolicina, Datum_izmene=date('now') where ID=@ID";
            string sqlNapomena = "UPDATE Napomene set Tekst=@Tekst WHERE ID=@NapomenaID";

            SQLiteCommand komanda2 = new SQLiteCommand(sql, konekcija);
        
            komanda2.Parameters.AddWithValue("@Naziv", naziv);
            komanda2.Parameters.AddWithValue("@Cena", cena);
            komanda2.Parameters.AddWithValue("@Kolicina", kolicina);
            komanda2.Parameters.AddWithValue("@ID", sifra);

            SQLiteCommand komandaNapomene = new SQLiteCommand(sqlNapomena, konekcija);
            komandaNapomene.Parameters.AddWithValue("@Tekst", tekstNapomene);
            komandaNapomene.Parameters.AddWithValue("@NapomenaID", napomenaID);

            try
            {
                konekcija.Open();
               komanda2.ExecuteNonQuery();
               komandaNapomene.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                konekcija.Close();
            }


        }
        public static bool Insert(int sifra, string naziv, float cena, int kolicina,   string napomena)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);

            string sql = "insert into Artikli(ID, Naziv, Cena, Kolicina, Datum_kreiranja) values (@ID, @Naziv, @Cena, @Kolicina,date('now') )";

            SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
            komanda.Parameters.AddWithValue("@ID", sifra);
            komanda.Parameters.AddWithValue("@Naziv", naziv);
            komanda.Parameters.AddWithValue("@Cena", cena);
            komanda.Parameters.AddWithValue("@Kolicina", kolicina);
  

           string sqlNapomena = "Insert into Napomene(Tekst) values (@napomena)";
           SQLiteCommand komandaNapomena = new SQLiteCommand(sqlNapomena, konekcija);
            komandaNapomena.Parameters.AddWithValue("@napomena", napomena);

            try
            {
                konekcija.Open();
                komanda.ExecuteNonQuery();
              komandaNapomena.ExecuteNonQuery();

                return true;


            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                konekcija.Close();
            }
        }

        public static bool InsertDataIntoArtikliNapomene(int ArtikalID, int NapomenaID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sql = "Insert into Artikli_napomene(Artikal_ID, Napomena_ID, Datum_unosa) values (@ArtikalID, @NapomenaID,date('now')) ";

            SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
            komanda.Parameters.AddWithValue("@ArtikalID", ArtikalID);
            komanda.Parameters.AddWithValue("@NapomenaID", NapomenaID);

            try
            {
                konekcija.Open();
                komanda.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                konekcija.Close();
            }
        }

        public static string VratiArtikal_ID(string naziv)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sql = "Select ID from Artikli where Naziv=@naziv";
            SQLiteCommand komandaID = new SQLiteCommand(sql, konekcija);
            komandaID.Parameters.AddWithValue("@naziv", naziv);

            string id = "";

            SQLiteDataReader reader = null;
            try
            {
                konekcija.Open();
                reader = komandaID.ExecuteReader();
                while (reader.Read())
                {
                    id = reader[0].ToString();

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                konekcija.Close();
            }

            return id;

        }

        public static string VratiNapomena_ID_Tekst(string napomena)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlID = "select ID from Napomene where tekst=@tekst";

            SQLiteCommand komandaID = new SQLiteCommand(sqlID, konekcija);
            komandaID.Parameters.AddWithValue("@tekst", napomena);

            string id = "";

            SQLiteDataReader reader = null;
            try
            {
                konekcija.Open();
                reader = komandaID.ExecuteReader();
                while (reader.Read())
                {
                    id = reader[0].ToString();

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                konekcija.Close();
            }

            return id;
        }


        public static string VratiTekstNapomene(int ArtikalID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sql = "Select n.Tekst from Artikli_napomene as an  inner join Artikli as a  on a.ID=an.Artikal_ID inner join Napomene as n on an.Napomena_ID=n.ID  WHERE a.ID=@ArtikalID";


            SQLiteCommand komandaID = new SQLiteCommand(sql, konekcija);
            komandaID.Parameters.AddWithValue("@ArtikalID", ArtikalID);

            string tekst = "";

            SQLiteDataReader reader = null;
            try
            {
                konekcija.Open();
                reader = komandaID.ExecuteReader();
                while (reader.Read())
                {
                    tekst = reader[0].ToString();

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                konekcija.Close();
            }

            return tekst;
        }

        public static string VratiNapomena_ID_Artikal(int artikalID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlID = "select an.Napomena_ID from Artikli_napomene as an where an.Artikal_ID=@ArtikalID";

            SQLiteCommand komandaID = new SQLiteCommand(sqlID, konekcija);
            komandaID.Parameters.AddWithValue("@ArtikalID", artikalID);

            string id = "";

            SQLiteDataReader reader = null;
            try
            {
                konekcija.Open();
                reader = komandaID.ExecuteReader();
                while (reader.Read())
                {
                    id = reader[0].ToString();

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                konekcija.Close();
            }

            return id;
        }




        public static string vratiCenu(int ArtikalID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sql = "Select a.Cena from Artikli as a  WHERE a.ID=@ArtikalID";


            SQLiteCommand komandaID = new SQLiteCommand(sql, konekcija);
            komandaID.Parameters.AddWithValue("@ArtikalID", ArtikalID);

            string tekst = "";

            SQLiteDataReader reader = null;
            try
            {
                konekcija.Open();
                reader = komandaID.ExecuteReader();
                while (reader.Read())
                {
                    tekst = reader[0].ToString();

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                konekcija.Close();
            }

            return tekst;
        }

        public static bool UpdateKolicinaInArticle(int ArtikalID, int prodato)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlUpdate = "UPDATE " + Artikal.table + " SET Kolicina = Kolicina - @prodato WHERE ID = @ID";
            SQLiteCommand komanda = new SQLiteCommand(sqlUpdate, konekcija);
            komanda.Parameters.AddWithValue("@ID", ArtikalID);
            komanda.Parameters.AddWithValue("@prodato", prodato);
            try
            {
                konekcija.Open();

                komanda.ExecuteNonQuery();
                return true;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;

            }
            finally
            {
                konekcija.Close();
            }
        }
    }
}


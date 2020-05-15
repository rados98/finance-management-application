using System.Data;
using System;
using System.Windows;
using System.Text;

using System.Data.SQLite;

namespace Projekat
{
    class Faktura
    {
        private int id;
        private string brojFakture;
        private string rokZaUplatu;
        private int klijentID;
        private string nazivKlijenta;
        private string napomena;
        private double ukupnaCena;


        public string BrojFakture { get => brojFakture; set => brojFakture = value; }
        public string RokZaUplatu { get => rokZaUplatu; set => rokZaUplatu = value; }
        public int KlijentID { get => klijentID; set => klijentID = value; }
        public string Napomena { get => napomena; set => napomena = value; }
        public string NazivKlijenta { get => nazivKlijenta; set => nazivKlijenta = value; }
        public int Id { get => id; set => id = value; }
        public double UkupnaCena { get => ukupnaCena; set => ukupnaCena = value; }

        public static string table = "Fakture";

        public static DataTable getFactures()
        {
            DataSet dsFakture = Konekcija.executeQuery("Select * from " + (Faktura.table) + "  WHERE Datum_brisanja IS NULL;", Faktura.table);
            return dsFakture.Tables[Faktura.table];
        }

        public static string VratiBrojFakture()
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sql = "Select (count(*) + 1) ||'-' || substr(strftime('%Y', Datum_kreiranja),3,2 ) from Fakture where strftime('%Y', Datum_kreiranja)=strftime('%Y', 'now')";
            SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();
            string pomoc = "";
            try
            {
                konekcija.Open();
                reader = komanda.ExecuteReader();
                while (reader.Read())
                {
                    sb.Append(reader[0].ToString());
                }

                pomoc = sb.ToString();
                return pomoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return pomoc;
            }
            finally
            {
                konekcija.Close();
            }
        }

        public static bool Delete(int FakturaID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlDELETE = "UPDATE " + Faktura.table + " SET Datum_brisanja = date('now') WHERE ID=@ID";
            SQLiteCommand komanda = new SQLiteCommand(sqlDELETE, konekcija);
            komanda.Parameters.AddWithValue("@ID", FakturaID);
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

        public static bool InsertFakturaOnly(Faktura faktura)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);

            string sqlINSERT = "Insert into Fakture(Broj_fakture, Rok_za_uplatu,Klijent_ID,Ukupan_iznos,Datum_kreiranja) values (@Broj_fakture, @Rok_za_uplatu, @Klijent_ID,@Ukupan_iznos,date('now'))";
            SQLiteCommand komandaINSERT = new SQLiteCommand(sqlINSERT, konekcija);
            komandaINSERT.Parameters.AddWithValue("@Broj_fakture", faktura.BrojFakture);
            komandaINSERT.Parameters.AddWithValue("@Rok_za_uplatu", faktura.RokZaUplatu);
            komandaINSERT.Parameters.AddWithValue("@Klijent_ID", faktura.KlijentID);
            komandaINSERT.Parameters.AddWithValue("@Ukupan_iznos", faktura.UkupnaCena);





            try
            {
                konekcija.Open();
                komandaINSERT.ExecuteNonQuery();
              
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

        public static bool InsertFactureZakOkvir(int fakturaID, int okvirID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlNapomena = "Insert into Faktura_zakonski_okvir(Faktura_ID, Okvir_ID) values (@faktura_ID, @okvir_ID)";
            SQLiteCommand komandaNapomena = new SQLiteCommand(sqlNapomena, konekcija);
            komandaNapomena.Parameters.AddWithValue("@faktura_ID", fakturaID);
            komandaNapomena.Parameters.AddWithValue("@okvir_ID", okvirID);
            try
            {
                konekcija.Open();
              
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

        public static bool InsertFactureArticles(int fakturaID, int artikalID, float cena, int PDV,int kolicina)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);



            string sqlINSERT2 = "INSERT INTO Faktura_artikli(Faktura_ID, Artikal_ID, Cena, PDV, Kolicina) values" +
                "(@fakturaID, @artikalID, @cena, @PDV, @kolicina)";
            SQLiteCommand komandaINSERT2 = new SQLiteCommand(sqlINSERT2, konekcija);
            komandaINSERT2.Parameters.AddWithValue("@fakturaID", fakturaID);
            komandaINSERT2.Parameters.AddWithValue("@artikalID", artikalID);
            komandaINSERT2.Parameters.AddWithValue("@cena", cena);
            komandaINSERT2.Parameters.AddWithValue("@PDV", PDV);
           
            komandaINSERT2.Parameters.AddWithValue("@kolicina", kolicina);


           




            try
            {
                konekcija.Open();
               
                komandaINSERT2.ExecuteNonQuery();
                
               
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

        public static bool InsertNapomeneInFacture(string napomena)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlNapomena = "Insert into Napomene(Tekst) values (@napomena)";
            SQLiteCommand komandaNapomena = new SQLiteCommand(sqlNapomena, konekcija);
            komandaNapomena.Parameters.AddWithValue("@napomena", napomena);
            try
            {
                konekcija.Open();

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

        public static bool InsertDataIntoFaktureNapomene(int FakturaID, int NapomenaID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sql = "Insert into Faktura_napomene(Faktura_ID, Napomena_ID, Datum_unosa) values (@FakturaID, @NapomenaID, date('now')) ";

            SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
            komanda.Parameters.AddWithValue("@FakturaID", FakturaID);
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

        public static string VratiNapomena_ID_Tekst(string napomena)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlID = "select ID from Napomene where Tekst=@tekst";

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


        public static string VratiIDFakture(string brojFakture)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlID = "select ID from Fakture where Broj_fakture=@brojFakture";

            SQLiteCommand komandaID = new SQLiteCommand(sqlID, konekcija);
            komandaID.Parameters.AddWithValue("@brojFakture", brojFakture);

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

        public static bool Update(Faktura faktura, int fakturaID, int napomenaID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);

            string sqlUPDATE = "UPDATE Fakture set Rok_za_uplatu=@Rok_za_uplatu, Klijent_ID=@Klijent_ID, Datum_izmene=date('now') WHERE ID=@ID";
            SQLiteCommand komandaINSERT = new SQLiteCommand(sqlUPDATE, konekcija);
            komandaINSERT.Parameters.AddWithValue("@ID", fakturaID);
            komandaINSERT.Parameters.AddWithValue("@Rok_za_uplatu", faktura.RokZaUplatu);
            komandaINSERT.Parameters.AddWithValue("@Klijent_ID", faktura.KlijentID);

            string sqlNapomena = "UPDATE Napomene set Tekst=@Tekst WHERE ID=@NapomenaID";
            SQLiteCommand komandaNapomene = new SQLiteCommand(sqlNapomena, konekcija);
            komandaNapomene.Parameters.AddWithValue("@Tekst", faktura.Napomena);
            komandaNapomene.Parameters.AddWithValue("@NapomenaID", napomenaID);

            try
            {
                konekcija.Open();
                komandaINSERT.ExecuteNonQuery();
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

        public static string VratiNapomena_ID_Faktura(int fakturaID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlID = "select fn.Napomena_ID from Fakture_napomene as fn where fn.Faktura_ID=@FakturaID";

            SQLiteCommand komandaID = new SQLiteCommand(sqlID, konekcija);
            komandaID.Parameters.AddWithValue("@FakturaID", fakturaID);

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


        public static string VratiTekstNapomene(int FakturaID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sql = "Select n.Tekst from fakture_napomene as fn  inner join Fakture as f  on f.ID=fn.Faktura_ID inner join Napomene as n on fn.Napomena_ID=n.ID  WHERE f.ID=@fakturaID";


            SQLiteCommand komandaID = new SQLiteCommand(sql, konekcija);
            komandaID.Parameters.AddWithValue("@fakturaID", FakturaID);

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


    }
}

using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
namespace Projekat
{
    public class Klijent
    {
        private int id;
        private string naziv;
        private double pib;
        private double maticniB;
        private string adresa;
        private string adresaEX;
        private string tekuciR;
        private string napomena;

        public static string table = "Klijenti";

        public string Naziv { get => naziv; set => naziv = value; }
        public double Pib { get => pib; set => pib = value; }
        public double MaticniB { get => maticniB; set => maticniB = value; }
        public string Adresa { get => adresa; set => adresa = value; }
        public string AdresaEX { get => adresaEX; set => adresaEX = value; }
        public string TekuciR { get => tekuciR; set => tekuciR = value; }
        public string Napomena { get => napomena; set => napomena = value; }
        public int Id { get => id; set => id = value; }

        public static DataTable getClients()
        {
            DataSet dsKlijenti = Konekcija.executeQuery("Select * from " + (Klijent.table) + "  WHERE Datum_brisanja IS NULL;", Klijent.table);
            return dsKlijenti.Tables[Klijent.table];

        }

        public static bool Delete(int KlijentID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlDELETE = "UPDATE " + Klijent.table + " SET Datum_brisanja = date('now') WHERE ID=@ID";            
            SQLiteCommand komanda = new SQLiteCommand(sqlDELETE, konekcija);
            komanda.Parameters.AddWithValue("@ID", KlijentID);

            try
            {
                konekcija.Open();

                komanda.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                konekcija.Close();
            }
        }

        public static bool Insert(Klijent klijent)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);


            string sql = "Insert into Klijenti(PIB, Naziv, MaticniBroj, Adresa, AdresaEks, TekuciRacun, Datum_kreiranja) " +
                " values(@PIB, @Naziv, @MaticniBroj, @Adresa, @AdresaEks, @TekuciRacun,  date('now')) ";
            SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
            komanda.Parameters.AddWithValue("@PIB", klijent.Pib);
            komanda.Parameters.AddWithValue("@Naziv", klijent.Naziv);
            komanda.Parameters.AddWithValue("@MaticniBroj", klijent.MaticniB);
            komanda.Parameters.AddWithValue("@Adresa", klijent.Adresa);
            komanda.Parameters.AddWithValue("@AdresaEks", klijent.AdresaEX);
            komanda.Parameters.AddWithValue("@TekuciRacun", klijent.TekuciR);

            string sqlNapomena = "Insert into Napomene(Tekst) values (@napomena)";
            SQLiteCommand komandaNapomena = new SQLiteCommand(sqlNapomena, konekcija);
            komandaNapomena.Parameters.AddWithValue("@napomena", klijent.Napomena);

            

            try
            {
                konekcija.Open();

                komanda.ExecuteNonQuery();
                komandaNapomena.ExecuteNonQuery();
                return true;

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;

            }
            finally
            {
                konekcija.Close();
            }
        }

        public static bool InsertDataIntoKlijentiNapomene(int KlijentID, int NapomenaID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sql = "Insert into Klijenti_napomene(Klijent_ID, Napomena_ID, Datum_unosa) values (@KlijentID, @NapomenaID,date('now')) ";

            SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
            komanda.Parameters.AddWithValue("@KlijentID", KlijentID);
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

        public static bool Update(Klijent klijent, int klijentID ,int napomenaID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);


            string sql = "UPDATE Klijenti set PIB=@PIB, Naziv=@Naziv, MaticniBroj=@MaticniBroj, Adresa=@Adresa, AdresaEks=@AdresaEks, TekuciRacun=@TekuciRacun, Datum_izmene= date('now') WHERE ID=@ID";
            
            SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
            komanda.Parameters.AddWithValue("@PIB", klijent.Pib);
            komanda.Parameters.AddWithValue("@Naziv", klijent.Naziv);
            komanda.Parameters.AddWithValue("@MaticniBroj", klijent.MaticniB);
            komanda.Parameters.AddWithValue("@Adresa", klijent.Adresa);
            komanda.Parameters.AddWithValue("@AdresaEks", klijent.AdresaEX);
            komanda.Parameters.AddWithValue("@TekuciRacun", klijent.TekuciR);
            komanda.Parameters.AddWithValue("@ID", klijentID);

            string sqlNapomena = "UPDATE Napomene set Tekst=@Tekst WHERE ID=@NapomenaID";
            SQLiteCommand komandaNapomene = new SQLiteCommand(sqlNapomena, konekcija);
            komandaNapomene.Parameters.AddWithValue("@Tekst", klijent.Napomena);
            komandaNapomene.Parameters.AddWithValue("@NapomenaID", napomenaID);


            try
            {
                konekcija.Open();

                komanda.ExecuteNonQuery();
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

        public static string VratiNazivKlijenta(int ID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlID = "select Naziv from Klijenti where ID=@ID";

            SQLiteCommand komandaID = new SQLiteCommand(sqlID, konekcija);
            komandaID.Parameters.AddWithValue("@ID", ID);

            string naziv = "";

            SQLiteDataReader reader = null;
            try
            {
                konekcija.Open();
                reader = komandaID.ExecuteReader();
                while (reader.Read())
                {
                    naziv = reader[0].ToString();

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

            return naziv;
        }

        public static string VratiKlijent_ID(string nazivSelektovanogKlijenta)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlID = "select ID from Klijenti where Naziv=@naziv";

            SQLiteCommand komandaID = new SQLiteCommand(sqlID, konekcija);
            komandaID.Parameters.AddWithValue("@naziv", nazivSelektovanogKlijenta);

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

        public static string VratiNapomena_ID_Klijent(int klijentID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlID = "select kn.Napomena_ID from Klijenti_napomene as kn where kn.Klijent_ID=@KlijentID";

            SQLiteCommand komandaID = new SQLiteCommand(sqlID, konekcija);
            komandaID.Parameters.AddWithValue("@KlijentID", klijentID);

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

        public static string VratiTekstNapomene(int KlijentID)
        {
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sql = "Select n.Tekst from Klijenti_napomene as kn  inner join Klijenti as k  on k.ID=kn.Klijent_ID inner join Napomene as n on kn.Napomena_ID=n.ID  WHERE k.ID=@KlijentID";
        

            SQLiteCommand komandaID = new SQLiteCommand(sql, konekcija);
            komandaID.Parameters.AddWithValue("@KlijentID", KlijentID);

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




    }
}

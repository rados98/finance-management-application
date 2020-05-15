using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Windows;

namespace Projekat
{
    public static class SettingsClass
    {

        public class PodaciIzdavaocaRacuna
        {

            private string naziv;
            private long pib;
            private long maticniBroj;
            private string ulica;
            private long broj;
            private string grad;
            private long postanskiBroj;

            public static PodaciIzdavaocaRacuna pir = new PodaciIzdavaocaRacuna();

            public static string nazivGrupe = "Podaci izdavaoca racuna";

            public string Naziv { get => naziv; set => naziv = value; }
            public long PIB { get => pib; set => pib = value; }
            public long MaticniBroj { get => maticniBroj; set => maticniBroj = value; }
            public string Ulica { get => ulica; set => ulica = value; }
            public long Broj { get => broj; set => broj = value; }
            public string Grad { get => grad; set => grad = value; }
            public long PostanskiBroj { get => postanskiBroj; set => postanskiBroj = value; }

            public static PodaciIzdavaocaRacuna vratiPIR()
            {
                PodaciIzdavaocaRacuna pir = new PodaciIzdavaocaRacuna();

                SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
                string sql = "select Value from Options where Entity='Podaci izdavaoca racuna' and Key='Naziv'";
                SQLiteCommand komandaNAZIV = new SQLiteCommand(sql, konekcija);
                SQLiteDataReader reader = null;

                string sql2 = "select Value from Options where Entity='Podaci izdavaoca racuna' and Key='PIB'";
                SQLiteCommand komandaPIB = new SQLiteCommand(sql2, konekcija);
                SQLiteDataReader reader2 = null;

                string sql3 = "select Value from Options where Entity='Podaci izdavaoca racuna' and Key='Maticni broj'";
                SQLiteCommand komandaMATICNI = new SQLiteCommand(sql3, konekcija);
                SQLiteDataReader reader3 = null;

                string sql4 = "select Value from Options where Entity='Podaci izdavaoca racuna' and Key='Ulica'";
                SQLiteCommand komandaULICA = new SQLiteCommand(sql4, konekcija);
                SQLiteDataReader reader4 = null;

                string sql5 = "select Value from Options where Entity='Podaci izdavaoca racuna' and Key='Broj'";
                SQLiteCommand komandaBROJ = new SQLiteCommand(sql5, konekcija);
                SQLiteDataReader reader5 = null;

                string sql6 = "select Value from Options where Entity='Podaci izdavaoca racuna' and Key='Grad'";
                SQLiteCommand komandaGRAD = new SQLiteCommand(sql6, konekcija);
                SQLiteDataReader reader6 = null;

                string sql7 = "select Value from Options where Entity='Podaci izdavaoca racuna' and Key='Postanski broj'";
                SQLiteCommand komandaPOSTANSKI = new SQLiteCommand(sql7, konekcija);
                SQLiteDataReader reader7 = null;
                try
                {
                    konekcija.Open();
                    reader = komandaNAZIV.ExecuteReader();
                   while(reader.Read())
                    {
                        pir.Naziv = reader[0].ToString();                      
                    }

                    reader2 = komandaPIB.ExecuteReader();
                    while (reader2.Read())
                    {
                        pir.PIB = long.Parse( reader2[0].ToString());
                    }

                    reader3 = komandaMATICNI.ExecuteReader();
                    while (reader3.Read())
                    {
                        pir.MaticniBroj = long.Parse(reader3[0].ToString());
                    }
                    reader4 = komandaULICA.ExecuteReader();
                    while (reader4.Read())
                    {
                        pir.Ulica = reader4[0].ToString();
                    }


                    reader5 = komandaBROJ.ExecuteReader();
                    while (reader5.Read())
                    {
                        pir.Broj = long.Parse(reader5[0].ToString());
                    }

                    reader6 = komandaGRAD.ExecuteReader();
                    while (reader6.Read())
                    {
                        pir.Grad = reader6[0].ToString();
                    }

                    reader7 = komandaPOSTANSKI.ExecuteReader();
                    while (reader7.Read())
                    {
                        pir.PostanskiBroj = long.Parse(reader7[0].ToString());
                    }




                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greska u settings class(Podaci izdavaoca racuna):  " + ex.Message);

                }
                finally
                {
                    konekcija.Close();
                }

                return pir;
            }

            public static void InsertDataInOptions(string naziv, long pib, long maticnibroj, string ulica, long broj, string grad, long postanskiBroj)
            {

                SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
                string sqlNAZIV = "Insert into Options values(@entity, @key, @value)";
                SQLiteCommand komandaNAZIV = new SQLiteCommand(sqlNAZIV, konekcija);
                komandaNAZIV.Parameters.AddWithValue("@entity", SettingsClass.PodaciIzdavaocaRacuna.nazivGrupe);
                komandaNAZIV.Parameters.AddWithValue("@key", "Naziv");
                komandaNAZIV.Parameters.AddWithValue("@value", naziv);


                string sqlPIB = "Insert into Options values(@entity, @key, @value)";
                SQLiteCommand komandaPIB = new SQLiteCommand(sqlPIB, konekcija);
                komandaPIB.Parameters.AddWithValue("@entity", SettingsClass.PodaciIzdavaocaRacuna.nazivGrupe);
                komandaPIB.Parameters.AddWithValue("@key", "PIB");
                komandaPIB.Parameters.AddWithValue("@value", pib);

                string sqlMATICNIBROJ = "Insert into Options values(@entity, @key, @value)";
                SQLiteCommand komandaMATICNIBROJ = new SQLiteCommand(sqlMATICNIBROJ, konekcija);
                komandaMATICNIBROJ.Parameters.AddWithValue("@entity", SettingsClass.PodaciIzdavaocaRacuna.nazivGrupe);
                komandaMATICNIBROJ.Parameters.AddWithValue("@key", "Maticni broj");
                komandaMATICNIBROJ.Parameters.AddWithValue("@value", maticnibroj);

                string sqlULICA = "Insert into Options values(@entity, @key, @value)";
                SQLiteCommand komandaULICA = new SQLiteCommand(sqlULICA, konekcija);
                komandaULICA.Parameters.AddWithValue("@entity", SettingsClass.PodaciIzdavaocaRacuna.nazivGrupe);
                komandaULICA.Parameters.AddWithValue("@key", "Ulica");
                komandaULICA.Parameters.AddWithValue("@value", ulica);

                string sqlBROJ = "Insert into Options values(@entity, @key, @value)";
                SQLiteCommand komandaBROJ = new SQLiteCommand(sqlBROJ, konekcija);
                komandaBROJ.Parameters.AddWithValue("@entity", SettingsClass.PodaciIzdavaocaRacuna.nazivGrupe);
                komandaBROJ.Parameters.AddWithValue("@key", "Broj");
                komandaBROJ.Parameters.AddWithValue("@value", broj);

                string sqlGRAD = "Insert into Options values(@entity, @key, @value)";
                SQLiteCommand komandaGRAD = new SQLiteCommand(sqlGRAD, konekcija);
                komandaGRAD.Parameters.AddWithValue("@entity", SettingsClass.PodaciIzdavaocaRacuna.nazivGrupe);
                komandaGRAD.Parameters.AddWithValue("@key", "Grad");
                komandaGRAD.Parameters.AddWithValue("@value", grad);

                string sqlPOSTANSKIBROJ = "Insert into Options values(@entity, @key, @value)";
                SQLiteCommand komandaPOSTANSKIBROJ = new SQLiteCommand(sqlPOSTANSKIBROJ, konekcija);
                komandaPOSTANSKIBROJ.Parameters.AddWithValue("@entity", SettingsClass.PodaciIzdavaocaRacuna.nazivGrupe);
                komandaPOSTANSKIBROJ.Parameters.AddWithValue("@key", "Postanski broj");
                komandaPOSTANSKIBROJ.Parameters.AddWithValue("@value", postanskiBroj);

                string sqlDELETE = "Delete FROM Options where Entity='" + SettingsClass.PodaciIzdavaocaRacuna.nazivGrupe +"'";
                SQLiteCommand komandaDELETE = new SQLiteCommand(sqlDELETE, konekcija);
              

                try
                {
                    konekcija.Open();
                    komandaDELETE.ExecuteNonQuery();
                    komandaNAZIV.ExecuteNonQuery();
                    komandaPIB.ExecuteNonQuery();
                    komandaMATICNIBROJ.ExecuteNonQuery();
                    komandaULICA.ExecuteNonQuery();
                    komandaBROJ.ExecuteNonQuery();
                    komandaGRAD.ExecuteNonQuery();
                    komandaPOSTANSKIBROJ.ExecuteNonQuery();
                    MessageBox.Show("Uspesno ste sacuvali podesavanja o izdavaocu racuna!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greska u settings class(Podaci izdavaoca racuna):  " + ex.Message);

                }
                finally
                {
                    konekcija.Close();
                }



            }
        }

        public class Finansije
        {
            private long pDV;

            private static string nazivGrupe = "Finansije";

            public long PDV { get => pDV; set => pDV = value; }

            public static void InsertDataInOptions(long pdv)
            {
                SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
                string sqlPDV = "Insert into Options values (@entity, @key, @value)";


                SQLiteCommand komandaPDV = new SQLiteCommand(sqlPDV, konekcija);
                komandaPDV.Parameters.AddWithValue("@entity", SettingsClass.Finansije.nazivGrupe);
                komandaPDV.Parameters.AddWithValue("@key", "PDV");
                komandaPDV.Parameters.AddWithValue("@value", pdv);


                string sqlDELETE = "Delete FROM Options where Entity='" + SettingsClass.Finansije.nazivGrupe + "'";
                SQLiteCommand komandaDELETE = new SQLiteCommand(sqlDELETE, konekcija);
                try
                {
                    konekcija.Open();
                    komandaDELETE.ExecuteNonQuery();
                    komandaPDV.ExecuteNonQuery();
                    MessageBox.Show("Uspesno ste sacuvali podesavanja o finansijama!");


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greska u settings class(finansije):  " + ex.Message);

                }
                finally
                {
                    konekcija.Close();
                }

            }

            public static string vratiPDV()
            {
                SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
                string sqlPDV = "Select Value from Options where Entity=@Entity and Key=@Key";


                SQLiteCommand komandaPDV = new SQLiteCommand(sqlPDV, konekcija);
                komandaPDV.Parameters.AddWithValue("@Entity", SettingsClass.Finansije.nazivGrupe);
                komandaPDV.Parameters.AddWithValue("@Key", "PDV");

                string pdv = "";

                SQLiteDataReader reader = null;


          
                try
                {
                    konekcija.Open();
                    reader = komandaPDV.ExecuteReader();
                while(reader.Read())
                    {
                        pdv = reader[0].ToString();
                    }
                  


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greska u settings class(finansije):  " + ex.Message);

                }
                finally
                {
                    konekcija.Close();
                }

                return pdv;
            }

        }

        public class ZakonskiOkviri
        {
            private string naziv;
            private string tekst;

            public static int lastCheckedIndexZakonskiOkvir = 0;
           

            private static string nazivGrupe = "Zakonski okvir";

            public string Naziv { get => naziv; set => naziv = value; }
            public string Tekst { get => tekst; set => tekst = value; }

            public static void DeleteFromZakonski_okviri(int id)
            {
                SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
                string sql = "Delete FROM Zakonski_okviri where ID=@ID";
                SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
                komanda.Parameters.AddWithValue("@ID", id);
                try
                {
                    konekcija.Open();
                    komanda.ExecuteNonQuery();
                    MessageBox.Show("Uspesno ste obrisali zakonski okvir");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    konekcija.Close();
                }



            }

            public static void InsertIntoZakonski_okviri(string naziv, string tekst)
            {


                SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
                string sql = "INSERT INTO Zakonski_okviri(Naziv, Tekst) values (@Naziv, @Tekst)";
                SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
                komanda.Parameters.AddWithValue("@Naziv", naziv);
                komanda.Parameters.AddWithValue("@Tekst", tekst);

                try
                {
                    konekcija.Open();
                    komanda.ExecuteNonQuery();
                    MessageBox.Show("Uspesno ste sacuvali zakonski okvir");


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    konekcija.Close();
                }



            }

            public static int VratiIDZakonskogOkvira(string Naziv)
            {
                SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);

                string sql = "select ID from Zakonski_okviri where Naziv=@Naziv";
                SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
                komanda.Parameters.AddWithValue("@Naziv", Naziv);
                SQLiteDataReader reader = null;
                int pomocINT = 0;
                try
                {
                    konekcija.Open();
                    reader = komanda.ExecuteReader();
                    while (reader.Read())
                    {
                        pomocINT = int.Parse(reader[0].ToString());
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
                return pomocINT;
            }

            public static int VratiBrojOkvira()
            {
                SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);

                string sql = "select count(*) from Zakonski_okviri";
                SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);

                int broj = 0;
                try
                {
                    konekcija.Open();
                    broj = Convert.ToInt32(komanda.ExecuteScalar());

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    konekcija.Close();
                }


                return broj + 1;


            }

            public static string VratiTekstOkvira(int id)
            {
                SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
                string sql = "select * from Zakonski_okviri where ID=@ID";
                SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
                komanda.Parameters.AddWithValue("@ID", id);
                SQLiteDataReader reader = null;
                string pomoc = "";
                try
                {
                    konekcija.Open();
                    reader = komanda.ExecuteReader();
                    while (reader.Read())
                    {
                        pomoc = reader[2].ToString();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greska u tekstu okvira:" + ex.Message);

                }
                finally
                {
                    konekcija.Close();
                }

                return pomoc;

            }

           /* public static void InsertDataInOptions(string naziv, string tekst)
            {
                SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
                string sqlNAZIV = "Insert into Options values(@entity, @key, @value)";
                SQLiteCommand komandaNAZIV = new SQLiteCommand(sqlNAZIV, konekcija);
                komandaNAZIV.Parameters.AddWithValue("@entity", SettingsClass.ZakonskiOkviri.nazivGrupe);
                komandaNAZIV.Parameters.AddWithValue("@key", "Naziv");
                komandaNAZIV.Parameters.AddWithValue("@value", naziv);


                string sqlTEKST = "Insert into Options values(@entity, @key, @value)";
                SQLiteCommand komandaTEKST = new SQLiteCommand(sqlTEKST, konekcija);
                komandaTEKST.Parameters.AddWithValue("@entity", SettingsClass.ZakonskiOkviri.nazivGrupe);
                komandaTEKST.Parameters.AddWithValue("@key", "Tekst");
                komandaTEKST.Parameters.AddWithValue("@value", tekst);

                string sqlDELETE = "Delete FROM Options where Entity='" + SettingsClass.ZakonskiOkviri.nazivGrupe + "'";
                SQLiteCommand komandaDELETE = new SQLiteCommand(sqlDELETE, konekcija);

                try
                {
                    konekcija.Open();
                    komandaDELETE.ExecuteNonQuery();
                    komandaNAZIV.ExecuteNonQuery();
                    komandaTEKST.ExecuteNonQuery();
                    MessageBox.Show("Uspesno ste sacuvali podesavanja o zakonskom okviru!");


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greska u settings class:(Zakonski okviri)  " + ex.Message);

                }
                finally
                {
                    konekcija.Close();
                }

            } */
        }


    }
}

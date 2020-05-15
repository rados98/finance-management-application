using System;
using System.Windows;

using System.Windows.Input;

using System.Data.SQLite;



namespace Projekat
{
    /// <summary>
    /// Interaction logic for Unos.xaml
    /// </summary>
    public partial class UnosKlijenata : Window
    {

        bool zatvoriOdmah = false;

        Klijent klijent;

        private MainWindow mainForm;

        bool isChanged = false;

        public UnosKlijenata(MainWindow form)
        {
            InitializeComponent();
            this.mainForm = form;
        }

        public UnosKlijenata()
        {
            InitializeComponent();
        }


        public bool ValidacijaPolja()
        {
            bool naziv, pib, maticniBroj, adresa;// tekuciRacun = false;
            long pomocLONG = 0;


            textGreskaNaziv.Text = (string.IsNullOrEmpty(tbNaziv.Text)) ? "Polje 'Naziv' ne sme biti prazno!" : "";
            textGreskaNaziv.Visibility = (string.IsNullOrEmpty(tbNaziv.Text)) ? Visibility.Visible : Visibility.Hidden;
            naziv = (textGreskaNaziv.IsVisible) ? false : true;


            textGreskaPib.Text = (string.IsNullOrEmpty(tbPIB.Text)) ? "Polje 'PIB' ne sme biti prazno!" :
                textGreskaPib.Text=(!long.TryParse(tbPIB.Text, out pomocLONG)) ? "U polju 'PIB' se mogu uneti samo cifre!":
                textGreskaPib.Text = (tbPIB.Text.Length < 8) ? "U polju 'PIB' ne sme biti manje od 8 karaktera!" : "";
            textGreskaPib.Visibility = (string.IsNullOrEmpty(tbPIB.Text) || tbPIB.Text.Length < 8) ? Visibility.Visible : Visibility.Hidden;
            pib = (string.IsNullOrEmpty(tbPIB.Text) || tbPIB.Text.Length < 8) ? false : true;

            textGreskaMaticniBroj.Text = (string.IsNullOrEmpty(tbMaticniBroj.Text)) ? "Polje 'Maticni broj' ne sme biti prazno!" :
                textGreskaMaticniBroj.Text = (!long.TryParse(tbMaticniBroj.Text, out pomocLONG)) ? "U polju 'Maticni broj' se mogu uneti samo cifre!" :
                textGreskaMaticniBroj.Text = (tbMaticniBroj.Text.Length < 13) ? "U polju 'Maticni broj' ne sme biti manje od 13 cifara!" : " ";
            textGreskaMaticniBroj.Visibility = (string.IsNullOrEmpty(tbMaticniBroj.Text) || !long.TryParse(tbMaticniBroj.Text, out pomocLONG) || tbMaticniBroj.Text.Length < 13) ? Visibility.Visible : Visibility.Hidden;
            maticniBroj = (string.IsNullOrEmpty(tbMaticniBroj.Text) || !long.TryParse(tbMaticniBroj.Text, out pomocLONG) || tbMaticniBroj.Text.Length < 13) ? false : true;

            textGreskaAdresa.Text = (string.IsNullOrEmpty(tbAdresa.Text)) ? "Polje 'Adresa' ne sme biti prazno!" : "";
            textGreskaAdresa.Visibility = (string.IsNullOrEmpty(tbAdresa.Text)) ? Visibility.Visible : Visibility.Hidden;
            adresa = (textGreskaAdresa.IsVisible) ? false : true;

            //textGreskaTekuciRacun.Text = (string.IsNullOrEmpty(tbTekuciRacun.Text)) ? "Polje 'Tekuci racun' ne sme biti prazno!" :
            //    textGreskaTekuciRacun.Text = (!long.TryParse(tbTekuciRacun.Text, out pomocLONG)) ? "U polju 'Tekuci racun' se mogu uneti samo cifre!" :
            //    textGreskaTekuciRacun.Text = (tbTekuciRacun.Text.Length < 18) ? "U polju 'Tekuci racun' ne sme biti manje od 18 cifara!" : " ";
            //textGreskaTekuciRacun.Visibility = (string.IsNullOrEmpty(tbTekuciRacun.Text) || !long.TryParse(tbTekuciRacun.Text, out pomocLONG) || tbTekuciRacun.Text.Length < 13) ? Visibility.Visible : Visibility.Hidden;
            //tekuciRacun = (string.IsNullOrEmpty(tbTekuciRacun.Text) || !long.TryParse(tbTekuciRacun.Text, out pomocLONG) || tbTekuciRacun.Text.Length < 13) ? false : true;

            if (naziv && pib && maticniBroj && adresa )// tekuciRacun)
                return true;
            else
                return false;





        }



        private void HideErrors()
        {

            textGreskaNaziv.Visibility = Visibility.Hidden;


            textGreskaPib.Visibility = Visibility.Hidden;

            textGreskaMaticniBroj.Visibility = Visibility.Hidden;


            textGreskaAdresa.Visibility = Visibility.Hidden;

            textGreskaTekuciRacun.Visibility = Visibility.Hidden;
        }



        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {

            HideErrors();
            if (!Exist(mainForm.SelectedClientID()))
            {
                if (ValidacijaPolja())
                {
                    InsertDataInDB();
                    this.mainForm.showDataGridClients();
                }
            }

            else
            {
                if(ValidacijaPolja())
                {
                    UpdateDataInSB();
                    zatvoriOdmah = true;
                    this.mainForm.showDataGridClients();
                    this.Close();
                   
                }
            }


        }

      
        private void InsertDataInDB()
        {
            klijent = new Klijent();
            klijent.Naziv = tbNaziv.Text;
            klijent.Pib = Convert.ToDouble( tbPIB.Text);
            klijent.MaticniB = Convert.ToDouble(tbMaticniBroj.Text);
            klijent.TekuciR = tbTekuciRacun.Text;
            klijent.Adresa = tbAdresa.Text;
            klijent.AdresaEX = tbAdresaEX.Text;
            klijent.Napomena = tbNapomena.Text;




            if (Klijent.Insert(klijent))
            {
                int KlijentID = Convert.ToInt32( Klijent.VratiKlijent_ID(klijent.Naziv));
                int NapomenaID = Convert.ToInt32(Klijent.VratiNapomena_ID_Tekst(klijent.Napomena));
                if (Klijent.InsertDataIntoKlijentiNapomene(KlijentID, NapomenaID))
                {
                    MessageBox.Show("Uspesno ste uneli klijenta!");
                    Resetuj();
                }
            }
            else
            {
                MessageBox.Show("Konekcija neuspesna!");
            }
        }


        private void UpdateDataInSB()
        {
            klijent = new Klijent();
            klijent.Naziv = tbNaziv.Text;
            klijent.Pib = Convert.ToDouble(tbPIB.Text);
            klijent.MaticniB = Convert.ToDouble(tbMaticniBroj.Text);
            klijent.TekuciR = tbTekuciRacun.Text;
            klijent.Adresa = tbAdresa.Text;
            klijent.AdresaEX = tbAdresaEX.Text;
            klijent.Napomena = tbNapomena.Text;

            int klijentID =Convert.ToInt32( Klijent.VratiKlijent_ID(klijent.Naziv));

            int napomenaID =Convert.ToInt32( Klijent.VratiNapomena_ID_Klijent(klijentID));

          if(Klijent.Update(klijent, mainForm.SelectedClientID(), napomenaID))
            {
                MessageBox.Show("Uspesno ste azurirali klijenta!");
                Resetuj();
            }
          else
            {
                MessageBox.Show("Konekcija neuspesna!");
            }
        }

        public void DeleteClientFromDB(int KlijentID)
        {
            if (Klijent.Delete(KlijentID))
            {
                MessageBox.Show("Uspesno ste obrisali klijenta!");
                Resetuj();
            }
            else
            {
               
                MessageBox.Show("Konekcija neuspesna!");
            }
        }

        private void Resetuj()
        {
            tbNaziv.Clear();
            tbPIB.Clear();
            tbMaticniBroj.Clear();
            tbAdresa.Clear();
            tbAdresaEX.Clear();
            tbTekuciRacun.Clear();
            tbNapomena.Clear();
        }


        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TbNaziv_KeyUp(object sender, KeyEventArgs e)
        {
            this.isChanged = true;
        }

        private void TbPIB_KeyUp(object sender, KeyEventArgs e)
        {
            this.isChanged = true;
        }

        private void TbMaticniBroj_KeyUp(object sender, KeyEventArgs e)
        {
            this.isChanged = true;
        }

        private void TbAdresa_KeyUp(object sender, KeyEventArgs e)
        {
            this.isChanged = true;
        }

        private void TbAdresaEX_KeyUp(object sender, KeyEventArgs e)
        {
            this.isChanged = true;
        }

        private void TbTekuciRacun_KeyUp(object sender, KeyEventArgs e)
        {
            this.isChanged = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(zatvoriOdmah)
            {
                e.Cancel = false;
             
            }
            else if (this.isChanged && MessageBox.Show("Da li si siguran da zelis da izadjes iz forme?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                
            }



        }


        public bool Exist(int id)
        {

            bool exist = false;
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlID = "select Naziv from Klijenti where ID=@ID";

            SQLiteCommand komandaID = new SQLiteCommand(sqlID, konekcija);
            komandaID.Parameters.AddWithValue("@ID", id);

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

            if (naziv == "")
            {
                exist = false;
            }
            else
            {
                exist = true;
            }

            return exist;


        }


    }
}

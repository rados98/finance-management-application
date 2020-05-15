using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SQLite;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window

    { 
        public SettingsWindow()
        {
            InitializeComponent();
            listBoxGlavni.SelectedIndex=0;
            ListBoxOkviri.SelectedIndex = SettingsClass.ZakonskiOkviri.lastCheckedIndexZakonskiOkvir;
            tbIznosPDV.Text = SettingsClass.Finansije.vratiPDV();
            postaviPIR();
            NapuniListBoxOkviri();

        }

        private void postaviPIR()
        {
            SettingsClass.PodaciIzdavaocaRacuna pirIzBaze = SettingsClass.PodaciIzdavaocaRacuna.vratiPIR();

            tbNaziv.Text =pirIzBaze.Naziv;
            tbPIB.Text = pirIzBaze.PIB.ToString();
            tbMaticniBroj.Text = pirIzBaze.MaticniBroj.ToString();
            tbUlica.Text = pirIzBaze.Ulica;
            tbBroj.Text = pirIzBaze.Broj.ToString();
            tbGrad.Text = pirIzBaze.Grad;
            tbPostanskiBroj.Text = pirIzBaze.PostanskiBroj.ToString();
        }

        
        SettingsClass.Finansije finansije;
        SettingsClass.ZakonskiOkviri zakonskiOkviri;


        private void NapuniListBoxOkviri()
        {

            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);

            string sql = "Select * from Zakonski_okviri";
            SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
            SQLiteDataReader reader = null;
            try
            {
                konekcija.Open();
                reader = komanda.ExecuteReader();
                while (reader.Read())
                {
                    ListBoxOkviri.Items.Add(reader[1].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                konekcija.Close();
                string pomoc = "Nije selektovan";
                ListBoxOkviri.Items.Add(pomoc);
            }

        }

        private bool ValidacijaGroupBoxPodaciIzdavaocaRacuna()
        {
            bool naziv, pib, maticniBroj, ulica, broj, grad, postanskiBroj = false;
            long pomocLONG = 0;

            textGreskaNaziv.Text = (string.IsNullOrEmpty(tbNaziv.Text)) ? "Polje 'Naziv' ne sme biti prazno!" : "";
            textGreskaNaziv.Visibility = (string.IsNullOrEmpty(tbNaziv.Text)) ? Visibility.Visible : Visibility.Hidden;
            naziv = (textGreskaNaziv.IsVisible) ? false : true;

            textGreskaPIB.Text = (string.IsNullOrEmpty(tbPIB.Text)) ? "Polje 'PIB' ne sme biti prazno!" :
            textGreskaPIB.Text = (!long.TryParse(tbPIB.Text, out pomocLONG)) ? "U polju 'PIB' se mogu uneti samo cifre!" :
            textGreskaPIB.Text = (tbPIB.Text.Length < 8) ? "U polju 'PIB' ne sme biti manje od 8 cifara!" : "";
            textGreskaPIB.Visibility = (string.IsNullOrEmpty(tbPIB.Text) || tbPIB.Text.Length < 8) ? Visibility.Visible : Visibility.Hidden;
            pib = (string.IsNullOrEmpty(tbPIB.Text) || tbPIB.Text.Length < 8) ? false : true;


            textGreskaMaticniBroj.Text = (string.IsNullOrEmpty(tbMaticniBroj.Text)) ? "Polje 'Maticni broj' ne sme biti prazno!" :
            textGreskaMaticniBroj.Text = (!long.TryParse(tbMaticniBroj.Text, out pomocLONG)) ? "U polju 'Maticni broj' se mogu uneti samo cifre!" :
            textGreskaMaticniBroj.Text = (tbMaticniBroj.Text.Length < 13) ? "U polju 'Maticni broj' ne sme biti manje od 13 cifara!" : " ";
            textGreskaMaticniBroj.Visibility = (string.IsNullOrEmpty(tbMaticniBroj.Text) || !long.TryParse(tbMaticniBroj.Text, out pomocLONG) || tbMaticniBroj.Text.Length < 13) ? Visibility.Visible : Visibility.Hidden;
            maticniBroj = (string.IsNullOrEmpty(tbMaticniBroj.Text) || !long.TryParse(tbMaticniBroj.Text, out pomocLONG) || tbMaticniBroj.Text.Length < 13) ? false : true;


            textGreskaUlica.Text = (string.IsNullOrEmpty(tbUlica.Text)) ? "Polje 'Ulica' ne sme biti prazno!" : "";
            textGreskaUlica.Visibility = (string.IsNullOrEmpty(tbUlica.Text)) ? Visibility.Visible : Visibility.Hidden;
            ulica = (textGreskaUlica.IsVisible) ? false : true;

            textGreskaBroj.Text = (string.IsNullOrEmpty(tbBroj.Text)) ? "Polje 'Broj' ne sme biti prazno!" :
            textGreskaBroj.Text = (!long.TryParse(tbBroj.Text, out pomocLONG)) ? "U polju 'Broj' se mogu uneti samo cifre!" :
            textGreskaBroj.Text = (tbBroj.Text.Length > 8) ? "U polju 'Broj' ne sme biti vise od 8 cifara!" : " ";
            textGreskaBroj.Visibility = (string.IsNullOrEmpty(tbBroj.Text) || !long.TryParse(tbBroj.Text, out pomocLONG) || tbBroj.Text.Length > 8) ? Visibility.Visible : Visibility.Hidden;
            broj = (string.IsNullOrEmpty(tbBroj.Text) || !long.TryParse(tbBroj.Text, out pomocLONG) || tbBroj.Text.Length > 8) ? false : true;


            textGreskaGrad.Text = (string.IsNullOrEmpty(tbGrad.Text)) ? "Polje 'Grad' ne sme biti prazno!" : "";
            textGreskaGrad.Visibility = (string.IsNullOrEmpty(tbGrad.Text)) ? Visibility.Visible : Visibility.Hidden;
            grad = (textGreskaUlica.IsVisible) ? false : true;


            textGreskaPostanskiBroj.Text = (string.IsNullOrEmpty(tbPostanskiBroj.Text)) ? "Polje 'Postanski broj' ne sme biti prazno!" :
            textGreskaPostanskiBroj.Text = (!long.TryParse(tbPostanskiBroj.Text, out pomocLONG)) ? "U polju 'Postanski broj' se mogu uneti samo cifre!" :
            textGreskaPostanskiBroj.Text = (tbPostanskiBroj.Text.Length < 4) ? "U polju 'Postanski broj' ne sme biti manje od 4 cifara!" : " ";
            textGreskaPostanskiBroj.Visibility = (string.IsNullOrEmpty(tbPostanskiBroj.Text) || !long.TryParse(tbPostanskiBroj.Text, out pomocLONG) || tbPostanskiBroj.Text.Length < 4) ? Visibility.Visible : Visibility.Hidden;
            postanskiBroj = (string.IsNullOrEmpty(tbPostanskiBroj.Text) || !long.TryParse(tbPostanskiBroj.Text, out pomocLONG) || tbPostanskiBroj.Text.Length < 4) ? false : true;

            if (naziv && pib && maticniBroj && ulica && broj && grad && postanskiBroj)
                return true;
            else
                return false;


        }

        private bool ValidacijaGroupBoxFinansije()
        {
            bool iznosPDV = false;
            long pomocLONG = 0;
            textGreskaIznosPDV.Text = (string.IsNullOrEmpty(tbIznosPDV.Text)) ? "Polje 'Iznos PDV-a' ne sme biti prazno" :
                textGreskaIznosPDV.Text = (!long.TryParse(tbIznosPDV.Text, out pomocLONG)) ? "U polu 'Iznos PDV-a' se mogu uneti samo cifre" : "";
            textGreskaIznosPDV.Visibility = (string.IsNullOrEmpty(tbIznosPDV.Text) || !long.TryParse(tbIznosPDV.Text, out pomocLONG)) ? Visibility.Visible : Visibility.Hidden;
            iznosPDV = (textGreskaIznosPDV.IsVisible) ? false : true;

            if (iznosPDV)
                return true;
            else
                return false;

        }


        private void ListBoxGlavni_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxItemPodaciIzdavaocaRacuna.IsSelected)
            {
                gpPodaciIzdavaocaRacuna.Visibility = Visibility.Visible;
                gpFinansije.Visibility = Visibility.Hidden;
                gpZakonskiOkviri.Visibility = Visibility.Hidden;
            }
            else if (ListBoxItemFinansije.IsSelected)
            {
                gpPodaciIzdavaocaRacuna.Visibility = Visibility.Hidden;
                gpFinansije.Visibility = Visibility.Visible;
                gpZakonskiOkviri.Visibility = Visibility.Hidden;
            }
            else
            {
                gpPodaciIzdavaocaRacuna.Visibility = Visibility.Hidden;
                gpFinansije.Visibility = Visibility.Hidden;
                gpZakonskiOkviri.Visibility = Visibility.Visible;
            }

            



        }

        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (gpPodaciIzdavaocaRacuna.IsVisible)
            {
                if (ValidacijaGroupBoxPodaciIzdavaocaRacuna())
                {
                    SettingsClass.PodaciIzdavaocaRacuna.pir = new SettingsClass.PodaciIzdavaocaRacuna();
                    SettingsClass.PodaciIzdavaocaRacuna.pir.Naziv = tbNaziv.Text;
                    SettingsClass.PodaciIzdavaocaRacuna.pir.PIB = long.Parse(tbPIB.Text);
                    SettingsClass.PodaciIzdavaocaRacuna.pir.MaticniBroj = long.Parse(tbMaticniBroj.Text);
                    SettingsClass.PodaciIzdavaocaRacuna.pir.Ulica = tbUlica.Text;
                    SettingsClass.PodaciIzdavaocaRacuna.pir.Broj = long.Parse(tbBroj.Text);
                    SettingsClass.PodaciIzdavaocaRacuna.pir.Grad = tbGrad.Text;
                    SettingsClass.PodaciIzdavaocaRacuna.pir.PostanskiBroj = long.Parse(tbPostanskiBroj.Text);

                    SettingsClass.PodaciIzdavaocaRacuna.InsertDataInOptions(SettingsClass.PodaciIzdavaocaRacuna.pir.Naziv, SettingsClass.PodaciIzdavaocaRacuna.pir.PIB, SettingsClass.PodaciIzdavaocaRacuna.pir.MaticniBroj, SettingsClass.PodaciIzdavaocaRacuna.pir.Ulica, SettingsClass.PodaciIzdavaocaRacuna.pir.Broj, SettingsClass.PodaciIzdavaocaRacuna.pir.Grad, SettingsClass.PodaciIzdavaocaRacuna.pir.PostanskiBroj);

                    postaviPIR();

                  

                }
            }
            else if (gpFinansije.IsVisible)
            {
                if (ValidacijaGroupBoxFinansije())
                {
                    finansije = new SettingsClass.Finansije();
                    finansije.PDV = long.Parse(tbIznosPDV.Text);

                    SettingsClass.Finansije.InsertDataInOptions(finansije.PDV);

                    tbIznosPDV.Text = finansije.PDV.ToString(); 

                    
                }
            }
            else
            {
                if (ValidacijaZaCuvanjeZakOkvira())
                {
                    zakonskiOkviri = new SettingsClass.ZakonskiOkviri();
                    zakonskiOkviri.Naziv = tbNazivOkvira.Text;
                    zakonskiOkviri.Tekst = tbTekstOkvira.Text;
                    //SettingsClass.ZakonskiOkviri.InsertDataInOptions(zakonskiOkviri.Naziv, zakonskiOkviri.Tekst);

                    tbNazivOkvira.Text = zakonskiOkviri.Naziv;
                    tbTekstOkvira.Text = zakonskiOkviri.Tekst;

                    SettingsClass.ZakonskiOkviri.lastCheckedIndexZakonskiOkvir = ListBoxOkviri.SelectedIndex;
                  
                    MessageBox.Show("Uspesno ste sacuvali podesavanje o zakonskom okviru!", "Obavestenje!", MessageBoxButton.OK, MessageBoxImage.Information);



                }
            }


        }


        private bool ValidacijaZaCuvanjeZakOkvira()
        {
            //string pomoc = "Nije selektovan";
            bool naziv, tekst = false;
            //tekstGreskaListBoxOkvir.Text = (pomoc != ListBoxOkviri.SelectedItem.ToString() || ListBoxOkviri.SelectedIndex==-1) ? "Morate izabrati 'Nije selektovan' ako zelite da sacuvate novi okvir" : "";
            //tekstGreskaListBoxOkvir.Visibility = (pomoc != ListBoxOkviri.SelectedItem.ToString() || ListBoxOkviri.SelectedIndex != -1) ? Visibility.Visible : Visibility.Hidden;
            //listboxokvir = (tekstGreskaListBoxOkvir.IsVisible) ? false : false;

            tekstGreskaNaziv2.Text = (string.IsNullOrEmpty(tbNazivOkvira.Text)) ? "Ne sme biti prazno!" : "";
            tekstGreskaNaziv2.Visibility = (string.IsNullOrEmpty(tbNazivOkvira.Text)) ? Visibility.Visible : Visibility.Hidden;
            naziv = (tekstGreskaNaziv2.IsVisible) ? false : true;


            tekstGreskaTekst.Text = (string.IsNullOrEmpty(tbTekstOkvira.Text)) ? "Ne sme biti prazno!" : "";
            tekstGreskaTekst.Visibility = (string.IsNullOrEmpty(tbTekstOkvira.Text)) ? Visibility.Visible : Visibility.Hidden;
            tekst = (tekstGreskaTekst.IsVisible) ? false : true;

            if (naziv && tekst)
                return true;
            else
                return false;

        }



        private void ListBoxOkviri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string pomoc = "Nije selektovan";
            if (ListBoxOkviri.SelectedIndex == -1)
            {
                tbNazivZakonskogOkvira.Text = "Izaberi okvir";
            }
            else
            {
                tbNazivZakonskogOkvira.Text = ListBoxOkviri.SelectedItem.ToString();
                ListBoxOkviri.Visibility = Visibility.Hidden;
                if (ListBoxOkviri.SelectedItem.ToString() == pomoc)
                {
                    btnObrisi.IsEnabled = false;
                    tbNazivOkvira.Clear();
                    tbTekstOkvira.Clear();

                }
                else
                {
                    btnObrisi.IsEnabled = true;
                    tbNazivOkvira.Text = ListBoxOkviri.SelectedItem.ToString();
                    tbTekstOkvira.Clear();
                    tbTekstOkvira.Text = SettingsClass.ZakonskiOkviri.VratiTekstOkvira(SettingsClass.ZakonskiOkviri.VratiIDZakonskogOkvira(ListBoxOkviri.SelectedItem.ToString()));

                }
            }

            ListBoxOkviri.Visibility = Visibility.Hidden;

          


        }

        private void BtnDodajNovi_Click(object sender, RoutedEventArgs e)
        {

            ListBoxOkviri.SelectedIndex = -1;
            tbNazivOkvira.Text = "Neimenovani okvir(" + SettingsClass.ZakonskiOkviri.VratiBrojOkvira() + ")";
            tbTekstOkvira.Clear();
            btnObrisi.IsEnabled = false;
        }




        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            string naziv = ListBoxOkviri.SelectedItem.ToString();
            int ID = SettingsClass.ZakonskiOkviri.VratiIDZakonskogOkvira(naziv);

            SettingsClass.ZakonskiOkviri.DeleteFromZakonski_okviri(ID);
            tbNazivOkvira.Clear();
            tbTekstOkvira.Clear();

            ListBoxOkviri.Items.Clear();
            NapuniListBoxOkviri();
            if (ListBoxOkviri.SelectedIndex == -1)
            {
                btnObrisi.IsEnabled = false;
            }

        }

        private void BtnSacuvajOkvir_Click(object sender, RoutedEventArgs e)
        {
            if (ValidacijaZaCuvanjeZakOkvira())
            {
                string naziv = tbNazivOkvira.Text;
                string tekst = tbTekstOkvira.Text;

                SettingsClass.ZakonskiOkviri.InsertIntoZakonski_okviri(naziv, tekst);

                tbNazivOkvira.Clear();
                tbTekstOkvira.Clear();

                ListBoxOkviri.Items.Clear();
                NapuniListBoxOkviri();
                if (ListBoxOkviri.SelectedIndex == -1)
                {
                    btnObrisi.IsEnabled = false;
                }
            }
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void IzaberiOkvir_Click(object sender, RoutedEventArgs e)
        {
            ListBoxOkviri.Visibility = Visibility.Visible;
        }

        private void Okviri_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Image slika = sender as Image;
                ContextMenu meni = slika.ContextMenu;
                meni.PlacementTarget = slika;
                meni.IsOpen = true;
                e.Handled = true;
            }

        }
    }
}

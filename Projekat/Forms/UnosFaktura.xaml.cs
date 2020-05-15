using System;

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SQLite;
using System.Windows.Input;
using System.Collections.ObjectModel;


namespace Projekat
{
    /// <summary>
    /// Interaction logic for UnosFaktura.xaml
    /// </summary>
    public partial class UnosFaktura : Window
    {
        Faktura faktura;
        private MainWindow mainForm;
        public static ObservableCollection<Artikal> artikli;
        SettingsWindow setWindow = new SettingsWindow();//mora se ovde definisati, jer ce tako ubacivati jedan po jedan artikal








        bool zatvoriOdmah = false;

        public UnosFaktura(MainWindow form)
        {
            InitializeComponent();
            unesiBrojFakture();
            PuniListBoxKlijenti();
            PuniListBoxZakonskiOkvir();
            artikli = new ObservableCollection<Artikal>();
            DataGridArtikli.ItemsSource = artikli;

            this.mainForm = form;
        }

        public UnosFaktura()
        {
            InitializeComponent();
            unesiBrojFakture();
            PuniListBoxKlijenti();
            PuniListBoxZakonskiOkvir();
            artikli = new ObservableCollection<Artikal>();
            DataGridArtikli.ItemsSource = artikli;
        }


        public bool Validacija()
        {
            DateTime? selectedDate = dpRokUplate.SelectedDate;

            if (ListBoxKlijenti.SelectedIndex == -1 || ListBoxZakonskiOkvir.SelectedIndex == -1 || artikli.Count == 0)
            {
                return false;
            }
            else
            {
                if (selectedDate.HasValue)
                    return true;
                else
                    return false;
            }

        }

        private void unesiBrojFakture()
        {
            tbBrojFakture.Text = Faktura.VratiBrojFakture();  //novina-ako je prva faktura u godini ispisuje 0, ali se to resava u if klauzuli!
            if (tbBrojFakture.Text == "")
            {
                string year = (DateTime.Now).Year.ToString();
                string skracenaGodina = year.Substring(2, 2);
                tbBrojFakture.Text = "1-" + skracenaGodina;
            }
        }




        private void ListBoxKlijenti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string pomoc = "Nista od navedenog";
            if (ListBoxKlijenti.SelectedIndex == -1 || ListBoxKlijenti.SelectedItem.ToString() == pomoc)
            {
                tbNazivKlijenta.Text = "Izaberi klijenta";
            }
            else
            {
                tbNazivKlijenta.Text = ListBoxKlijenti.SelectedItem.ToString();
                ListBoxKlijenti.Visibility = Visibility.Hidden;
            }
            ListBoxKlijenti.Visibility = Visibility.Hidden;



        }
        private void ListBoxZakonskiOkvir_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string pomoc = "Nista od navedenog";
            if (ListBoxZakonskiOkvir.SelectedIndex == -1 || ListBoxZakonskiOkvir.SelectedItem.ToString() == pomoc)
                tbNazivZakonskogOkvira.Text = "Izaberi okvir";
            else
                tbNazivZakonskogOkvira.Text = ListBoxZakonskiOkvir.SelectedItem.ToString();

            ListBoxZakonskiOkvir.Visibility = Visibility.Hidden;
        }

        private void Resetuj()
        {
            unesiBrojFakture();
            ListBoxKlijenti.SelectedIndex = -1;
            ListBoxZakonskiOkvir.SelectedIndex = -1;
            dpRokUplate.SelectedDate = DateTime.Now;
            tbNapomena.Clear();
        }

        private void PuniListBoxKlijenti()
        {
            string pomoc = "Nista od navedenog";
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sql = "Select * from Klijenti";
            SQLiteCommand komanda = new SQLiteCommand(sql, konekcija);
            SQLiteDataReader reader = null;
            try
            {
                konekcija.Open();
                reader = komanda.ExecuteReader();

                while (reader.Read())
                {
                    string naziv = reader[2].ToString();
                    ListBoxKlijenti.Items.Add(naziv);

                }
                ListBoxKlijenti.Items.Add(pomoc);
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
        private void PuniListBoxZakonskiOkvir()
        {
            string pomoc = "Nista od navedenog";
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
                    string naziv = reader[1].ToString();
                    ListBoxZakonskiOkvir.Items.Add(naziv);

                }
                ListBoxZakonskiOkvir.Items.Add(pomoc);
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

        private void BtnUnosNovogKlijenta_Click(object sender, RoutedEventArgs e)
        {
            UnosKlijenata unosKlijenta = new UnosKlijenata(mainForm);
            unosKlijenta.ShowDialog();
            PuniListBoxKlijenti();

        }

        private void BtnDodajArtikal_Click(object sender, RoutedEventArgs e)
        {

            //novi = new Artikal();
            //artikli.Add(novi); //kolekcija
            //DataGridArtikli.ItemsSource = artikli;

            IzaberiArtikal popUp = new IzaberiArtikal(this);
            popUp.Show();

        }












        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (!Exist(mainForm.SelectedFactureID()))
            {
                if (Validacija())

                {
                    InsertDataInDB();
                    this.mainForm.showDataGridFactures();

                }
                else
                {
                    MessageBox.Show("Morate popuniti sva polja za unos!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Information);
                }


            }
            else
            {
                if (Validacija())
                {
                    UpdateDataInDB();
                    zatvoriOdmah = true;
                    this.Close();
                    this.mainForm.showDataGridFactures();
                }
            }
        }

        private void InsertDataInDB()
        {
            bool proba = false;
            bool proba2 = false;

            faktura = new Faktura();
            faktura.BrojFakture = tbBrojFakture.Text;
            faktura.KlijentID = int.Parse(Klijent.VratiKlijent_ID(ListBoxKlijenti.SelectedItem.ToString()));
            faktura.Napomena = tbNapomena.Text;
            faktura.UkupnaCena = UnosFaktura.racunajUkupnuCenu(artikli);
            DateTime? selectedDate = dpRokUplate.SelectedDate;
            faktura.RokZaUplatu = selectedDate.Value.ToString("yyyy-MM-dd");


            string okvirNaziv = ListBoxZakonskiOkvir.SelectedItem.ToString(); //pomoc za trazenje ID-a
            int zakonskiOkvirID = SettingsClass.ZakonskiOkviri.VratiIDZakonskogOkvira(okvirNaziv);

            if (Faktura.InsertFakturaOnly(faktura))  //faktura
            {
                int fakturaID = Convert.ToInt32(Faktura.VratiIDFakture(faktura.BrojFakture));
                proba = true;
                if (tbNapomena.Text != "")
                    if (Faktura.InsertNapomeneInFacture(faktura.Napomena))
                    {

                        int napomenaID = Convert.ToInt32(Faktura.VratiNapomena_ID_Tekst(faktura.Napomena));
                        if (Faktura.InsertDataIntoFaktureNapomene(fakturaID, napomenaID))  //fakturanapomena
                        {

                        }
                        else
                        {
                            MessageBox.Show("Greksa u konekciji1!");

                            return;

                        }
                    }
                if (Faktura.InsertFactureZakOkvir(fakturaID, zakonskiOkvirID))  //faktura okvir
                    foreach (Artikal artikal in artikli)
                    {

                        int ID = Convert.ToInt32(artikal.Sifra);
                        float cena = Convert.ToSingle(artikal.Cena);
                        int pdv = int.Parse(SettingsClass.Finansije.vratiPDV());                      
                        int kolicina = artikal.Kolicina;

                        if (Faktura.InsertFactureArticles(fakturaID, ID, cena, pdv, kolicina))  //faktura artikli
                        {
                            proba2 = true;
                            Artikal.UpdateKolicinaInArticle(ID, kolicina);   //promena stanja u magacinu

                        }
                        else
                        {
                            MessageBox.Show("Greksa u konekciji2!");
                            proba2 = false;
                            return;


                        }

                    }

            }
            else
            {
                proba = false;
            }

            if (proba && proba2)
            {
                MessageBox.Show("Uspesno ste uneli fakturu!");
                zatvoriOdmah = true;
                this.Close();
            }

            else
            {
                MessageBox.Show("Faktura nije uneta");
                zatvoriOdmah = true;
                this.Close();
            }

        }


        private void UpdateDataInDB()
        {

            faktura = new Faktura();
            faktura.BrojFakture = tbBrojFakture.Text;
            faktura.KlijentID = int.Parse(Klijent.VratiKlijent_ID(ListBoxKlijenti.SelectedItem.ToString()));
            faktura.Napomena = tbNapomena.Text;
            DateTime? selectedDate = dpRokUplate.SelectedDate;
            if (selectedDate.HasValue)
            {
                faktura.RokZaUplatu = selectedDate.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                MessageBox.Show("Morate izabrati neki datum");
            }

            int fakturaID = Convert.ToInt32(Faktura.VratiIDFakture(faktura.BrojFakture));
            int napomenaID = Convert.ToInt32(Faktura.VratiNapomena_ID_Faktura(fakturaID));
            if (Faktura.Update(faktura, mainForm.SelectedFactureID(), napomenaID))
            {
                MessageBox.Show("Uspesno ste azurirali fakturu!");
            }
            else
            {
                MessageBox.Show("Konekcija neuspesna!");
            }

        }


        public void DeleteFactureFromDB(int FakturaID)
        {
            if (Faktura.Delete(FakturaID))
            {
                MessageBox.Show("Uspesno ste obrisali fakturu!");

            }
            else
            {
                MessageBox.Show("Konekcija neuspesna!");
            }
        }


        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (zatvoriOdmah || daLiJeNestoUneto())
            {
                e.Cancel = false;


            }
            else if (MessageBoxResult.No == MessageBox.Show("Da li ste sigurni da zelite da zatvorite formu?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Information))
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
            string sqlID = "select Broj_fakture from Fakture where ID=@ID";

            SQLiteCommand komandaID = new SQLiteCommand(sqlID, konekcija);
            komandaID.Parameters.AddWithValue("@ID", id);

            string brFakture = "";

            SQLiteDataReader reader = null;
            try
            {
                konekcija.Open();
                reader = komandaID.ExecuteReader();
                while (reader.Read())
                {
                    brFakture = reader[0].ToString();

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

            if (brFakture == "")
            {
                exist = false;
            }
            else
            {
                exist = true;
            }

            return exist;


        }

        private bool daLiJeNestoUneto()
        {
            if (ListBoxKlijenti.SelectedIndex == -1)
                return false;
            else
                return true;
        }

        private void Klijenti_MouseDown(object sender, MouseButtonEventArgs e)
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
        private void ZakonskiOkvir_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void IzaberiKlijenta_Click(object sender, RoutedEventArgs e)
        {
            ListBoxKlijenti.Visibility = Visibility.Visible;

        }

        private void MenuItemZakonskiOkvir_Click(object sender, RoutedEventArgs e)
        {
            ListBoxZakonskiOkvir.Visibility = Visibility.Visible;
        }

        private void BtnStampaj_Click(object sender, RoutedEventArgs e)
        {
            PrintWindow printWindow = new PrintWindow();
            printWindow.ShowDialog();

        }

        private void DataGridArtikli_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = (DataGrid)sender;

                Artikal selektovaniArtikal = (Artikal)DataGridArtikli.SelectedItem;

                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    string brojFakture = selektovaniArtikal.Sifra.ToString();
                    string naziv = selektovaniArtikal.Naziv.ToString();
                    string cena = selektovaniArtikal.Cena.ToString();
                    string kolicina = selektovaniArtikal.Kolicina.ToString();
                    string naStanju = selektovaniArtikal.NaStanju.ToString();
                    Artikal a;

                    IzaberiArtikal popUp = new IzaberiArtikal(this);
                    popUp.comboArtikli.SelectedIndex = selektovaniArtikal.PomocniINDEX;  //pomoc za combo box
                    popUp.tbSifra.Text = brojFakture;
                    popUp.tbNaziv.Text = naziv;
                    popUp.tbCena.Text = cena;
                    popUp.tbKolicina.Text = kolicina;
                    popUp.tbNaStanju.Text = (int.Parse(naStanju) + int.Parse(kolicina)).ToString();
                    popUp.Show();
                }
                else
                {
                    MessageBox.Show("GRESKA");

                }

            }
            else
            {
                MessageBox.Show("GRESKA2");
            }
        }

        private void DataGridArtikliItemEdit_Click(object sender, RoutedEventArgs e)
        {

            DataGrid grid = DataGridArtikli;

            Artikal selektovaniArtikal = (Artikal)DataGridArtikli.SelectedItem;

                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    string brojFakture = selektovaniArtikal.Sifra.ToString();
                    string naziv = selektovaniArtikal.Naziv.ToString();
                    string cena = selektovaniArtikal.Cena.ToString();
                    string kolicina = selektovaniArtikal.Kolicina.ToString();
                    string naStanju = selektovaniArtikal.NaStanju.ToString();
                    Artikal a;

                    IzaberiArtikal popUp = new IzaberiArtikal(this);
                    popUp.comboArtikli.SelectedIndex = selektovaniArtikal.PomocniINDEX;  //pomoc za combo box
                    popUp.tbSifra.Text = brojFakture;
                    popUp.tbNaziv.Text = naziv;
                    popUp.tbCena.Text = cena;
                    popUp.tbKolicina.Text = kolicina;
                    popUp.tbNaStanju.Text = (int.Parse(naStanju) + int.Parse(kolicina)).ToString();
                    popUp.Show();
                }
                else
                {
                    MessageBox.Show("GRESKA");

                }

            }

        private void DataGridArtikliItemRemove_Click(object sender, RoutedEventArgs e)
        {
            Artikal pomocniArtikal = (Artikal)DataGridArtikli.SelectedItem;
            float ukupnaCena = 0; 
            if (MessageBoxResult.Yes == MessageBox.Show("Da li ste sigurni da zelite da obrisete artikal iz korpe " + pomocniArtikal.Naziv + "?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Information))
            {
                artikli.Remove(pomocniArtikal);
                MessageBox.Show("Uspesno ste obrisali artikal iz korpe!", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
            else
            {
                MessageBox.Show("Greska u brisanju artikla iz korpe!");
            }
            ukupnaCena = UnosFaktura.racunajUkupnuCenu(artikli);
            tbUkupanIznos.Text = ukupnaCena.ToString() + " dinara.";

        }

        public static float racunajUkupnuCenu(ObservableCollection<Artikal> korpa)
        {
            float cenaSaPDV = 0;
            float ukupnaCena = 0;
            float cenaPlusKolicina = 0;
            float pdv = Convert.ToSingle(SettingsClass.Finansije.vratiPDV());
            foreach(Artikal a in korpa)
            {
                cenaSaPDV = (pdv / 100f + 1) * a.Cena;
                cenaPlusKolicina = cenaSaPDV * a.Kolicina;
                ukupnaCena += cenaPlusKolicina;
            }

            return ukupnaCena;
        }
    }

       
    }



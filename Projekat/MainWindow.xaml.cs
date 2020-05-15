using System;
using System.Windows;
using System.Windows.Controls;
using System.Text;
using System.Windows.Input;

using System.Data;
using System.Data.SQLite;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        KolekcijaArtikala kolekcijaArtikala;
        KolekcijaKlijenata kolekcijaKlijenata;
        KolekcijaFaktura kolekcijaFaktura;

        public MainWindow()
        {
            InitializeComponent();
            this.showDataGridFactures();
        }
        public DataTable VratiTabeluKlijenti()
        {
            return Klijent.getClients();
        }
        public DataTable VratiTabeluArtikli()
        {
            return Artikal.getArticles();
        }

        public DataTable VratiTabeluFakture()
        {
            return Faktura.getFactures();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if ((MenuItem)sender == FaktureMeni)
            {
                //Otvara se forma za unos faktura
                UnosFaktura unosFaktura = new UnosFaktura(this);
                unosFaktura.ShowDialog();
            }
            if ((MenuItem)sender == KlijentiAdd)
            {
                //otvara se forma za unos klijenata
                UnosKlijenata unosForma = new UnosKlijenata(this);

                unosForma.ShowDialog();
            }
            if ((MenuItem)sender == ArtikalMeni)
            {
                //otvara se forma za unos artikala
                UnosArtikala unosArtikalaForma = new UnosArtikala(this);
                unosArtikalaForma.ShowDialog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((Button)sender == btnKlijent)
            {
                showDataGridClients();
            }
            else if ((Button)sender == btnFakture)
            {
                showDataGridFactures();
            }

            else
            {
                showDataGridArticles();
            }


        }

        public void showDataGridClients()
        {
            //DataTable dtKlijenti = VratiTabeluKlijenti();
            //DataGridMain.ItemsSource = dtKlijenti.DefaultView;
            kolekcijaKlijenata = new KolekcijaKlijenata();         
            dataGridArtikli.Visibility = Visibility.Hidden;
            datagridFakture.Visibility = Visibility.Hidden;
            datagridKlijenti.Visibility = Visibility.Visible;
            datagridKlijenti.ItemsSource = kolekcijaKlijenata;





        }

        public void showDataGridArticles()
        {
            //DataTable dtArtikli = VratiTabeluArtikli();
            //DataGridMain.ItemsSource = dtArtikli.DefaultView;
            kolekcijaArtikala = new KolekcijaArtikala();
            datagridKlijenti.Visibility = Visibility.Hidden;
            datagridFakture.Visibility = Visibility.Hidden;
            dataGridArtikli.Visibility = Visibility.Visible;
            dataGridArtikli.ItemsSource = kolekcijaArtikala;
            
            
            
           
        }

        public void showDataGridFactures()
        {
            //DataTable dtFakture = VratiTabeluFakture();
            //DataGridMain.ItemsSource = dtFakture.DefaultView;
            kolekcijaFaktura = new KolekcijaFaktura();
            datagridKlijenti.Visibility = Visibility.Hidden;
            dataGridArtikli.Visibility = Visibility.Hidden;
            datagridFakture.Visibility = Visibility.Visible;
            datagridFakture.ItemsSource = kolekcijaFaktura;
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBoxResult.No == MessageBox.Show("Da li ste sigurni da zelite da izadjete iz programa?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Information))
            {
                e.Cancel = true;
            }
        }


       

        public int SelectedFactureID()
        {
            int fakturaID = 0;
            if (datagridFakture.SelectedIndex!=-1)
            {
                Faktura selektovanaFaktura = (Faktura)datagridFakture.SelectedItem;

                fakturaID = selektovanaFaktura.Id;

            }
            
            return fakturaID;

        }


        public int SelectedClientID()
        {

            int klijentID = 0;
            if (datagridKlijenti.SelectedIndex!=-1)
            {
                Klijent selektovaniKlijent = (Klijent)datagridKlijenti.SelectedItem;

                klijentID = selektovaniKlijent.Id;

            }
            return klijentID;

        }

        public int SelectedArticleID()
        { 
            int artikalID = 0;
            if (dataGridArtikli.SelectedIndex!=-1)
            {
                Artikal selektovaniArtikal = (Artikal)dataGridArtikli.SelectedItem;
                
                artikalID = selektovaniArtikal.Sifra;

            }
            return artikalID;
           
        }

              

        private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingForm = new  SettingsWindow();
            settingForm.ShowDialog();


        }

        private void dataGridArtikli_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Artikal selektovaniArtikal = (Artikal)dataGridArtikli.SelectedItem;
            string sifra = selektovaniArtikal.Sifra.ToString();
            string naziv = selektovaniArtikal.Naziv.ToString();
            string cena = selektovaniArtikal.Cena.ToString();
            string kolicina = selektovaniArtikal.Kolicina.ToString();
            string napomena = Artikal.VratiTekstNapomene(int.Parse(sifra)).ToString();



            UnosArtikala unosArtikla = new UnosArtikala(this);
            unosArtikla.Show();
            unosArtikla.tbsifra.Text = sifra;
            unosArtikla.tbNaziv.Text = naziv;
            unosArtikla.tbCena.Text = cena;
            unosArtikla.tbKolicina.Text = kolicina;
            unosArtikla.tbNapomena.Text = napomena;
        }

        private void dataGridArtikliMenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            Artikal selektovaniArtikal = (Artikal)dataGridArtikli.SelectedItem;
            string sifra = selektovaniArtikal.Sifra.ToString();
            string naziv = selektovaniArtikal.Naziv.ToString();
            string cena = selektovaniArtikal.Cena.ToString();
            string kolicina = selektovaniArtikal.Kolicina.ToString();
            string napomena = Artikal.VratiTekstNapomene(int.Parse(sifra)).ToString();



            UnosArtikala unosArtikla = new UnosArtikala(this);
            unosArtikla.Show();
            unosArtikla.tbsifra.Text = sifra;
            unosArtikla.tbNaziv.Text = naziv;
            unosArtikla.tbCena.Text = cena;
            unosArtikla.tbKolicina.Text = kolicina;
            unosArtikla.tbNapomena.Text = napomena;
        }

        private void dataGridArtikliMenuItemRemove_Click(object sender, RoutedEventArgs e)
        {
            Artikal selektovaniArtikal = (Artikal)dataGridArtikli.SelectedItem;
            int artikalID = 0;
            if (dataGridArtikli.IsVisible)
            {
               

                artikalID = selektovaniArtikal.Sifra;

            }
            UnosArtikala unosArtikal = new UnosArtikala();
            if (MessageBoxResult.Yes == MessageBox.Show("Da li ste sigurni da zelite da obrisete artikal " + selektovaniArtikal.Naziv + "?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Information))
            {
                unosArtikal.DeleteArticleFromDB(artikalID);
            }
            showDataGridArticles();
        }

      

        

        private void datagridKlijenti_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Klijent selektovaniKlijent = (Klijent)datagridKlijenti.SelectedItem;
            int id = selektovaniKlijent.Id;
            string naziv = selektovaniKlijent.Naziv;
            string pib = selektovaniKlijent.Pib.ToString();
            string maticniBroj = selektovaniKlijent.MaticniB.ToString();
            string adresa = selektovaniKlijent.Adresa.ToString();
            string adresaEX = selektovaniKlijent.AdresaEX;
            string tekuciRacun = selektovaniKlijent.TekuciR;
            string napomena = Klijent.VratiTekstNapomene(Convert.ToInt32(id));

            UnosKlijenata unosklijenata = new UnosKlijenata(this);
            unosklijenata.Show();  //ne moze showDialog - nece povuci informacije
            unosklijenata.tbNaziv.Text = naziv;
            unosklijenata.tbPIB.Text = pib;
            unosklijenata.tbMaticniBroj.Text = maticniBroj;
            unosklijenata.tbAdresa.Text = adresa;
            unosklijenata.tbAdresaEX.Text = adresaEX;
            unosklijenata.tbTekuciRacun.Text = tekuciRacun;
            unosklijenata.tbNapomena.Text = napomena;
        }

        private void dataGridKlijentiMenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            Klijent selektovaniKlijent = (Klijent)datagridKlijenti.SelectedItem;
            int id = selektovaniKlijent.Id;
            string naziv = selektovaniKlijent.Naziv;
            string pib = selektovaniKlijent.Pib.ToString();
            string maticniBroj = selektovaniKlijent.MaticniB.ToString();
            string adresa = selektovaniKlijent.Adresa.ToString();
            string adresaEX = selektovaniKlijent.AdresaEX;
            string tekuciRacun = selektovaniKlijent.TekuciR;
            string napomena = Klijent.VratiTekstNapomene(Convert.ToInt32(id));

            UnosKlijenata unosklijenata = new UnosKlijenata(this);
            unosklijenata.Show();  //ne moze showDialog - nece povuci informacije
            unosklijenata.tbNaziv.Text = naziv;
            unosklijenata.tbPIB.Text = pib;
            unosklijenata.tbMaticniBroj.Text = maticniBroj;
            unosklijenata.tbAdresa.Text = adresa;
            unosklijenata.tbAdresaEX.Text = adresaEX;
            unosklijenata.tbTekuciRacun.Text = tekuciRacun;
            unosklijenata.tbNapomena.Text = napomena;
        }

        private void dataGridKlijentiMenuItemRemove_Click(object sender, RoutedEventArgs e)
        {
            Klijent selektovaniKlijent = (Klijent)datagridKlijenti.SelectedItem;
            int klijentID = 0;
            if (datagridKlijenti.IsVisible)
            {


                klijentID = selektovaniKlijent.Id;

            }
            UnosKlijenata unosKlijent = new UnosKlijenata();
            if (MessageBoxResult.Yes == MessageBox.Show("Da li ste sigurni da zelite da obrisete klijenta: " + selektovaniKlijent.Naziv + "?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Information))
            {
                unosKlijent.DeleteClientFromDB(klijentID);
            }
            showDataGridClients();
        }

        private void datagridFakture_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Nemoguce je izvrsiti izmenu fakture!", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void dataGridFakturaMenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Nemoguce je izvrsiti izmenu fakture!", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void dataGridFakturaMenuItemRemove_Click(object sender, RoutedEventArgs e)
        {
            Faktura selektovanaFaktura = (Faktura)datagridFakture.SelectedItem;
            int id = selektovanaFaktura.Id;
            string brojFakture = selektovanaFaktura.BrojFakture.ToString();

            UnosFaktura unosFakture = new UnosFaktura();
            if (MessageBoxResult.Yes == MessageBox.Show("Da li ste sigurni da zelite da obrisete fakturu: " + brojFakture + "?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Information))
            {
                unosFakture.DeleteFactureFromDB(id);
            }
            showDataGridFactures();
        }
    }
}



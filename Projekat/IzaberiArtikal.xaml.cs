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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzaberiArtikal.xaml
    /// </summary>
    public partial class IzaberiArtikal : Window
    {

        private Artikal novi = new Artikal();
        private KolekcijaArtikala kolekcija;
        public static int broj = 1;
        UnosFaktura unosFaktura;
        public IzaberiArtikal(UnosFaktura unosFaktura)
        {
            InitializeComponent();
            puniCombo();
            this.unosFaktura = unosFaktura;
        }

        private void puniCombo()
        {
            kolekcija = new KolekcijaArtikala();
            comboArtikli.ItemsSource = kolekcija;
        }

        private void tbPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            int sifra = int.Parse(tbSifra.Text);
            float ukupnaCena = 0;

            if (Exist(sifra))
            {
                comboArtikli.SelectedItem = tbSifra.Text;
              

                int pozicija = vratiPoziciju(sifra);
                UnosFaktura.artikli.RemoveAt(pozicija);
                
                    int stanje = Convert.ToInt32(tbNaStanju.Text);
                    int kolicina = Convert.ToInt32(tbKolicina.Text);
                    
                        novi.Broj = broj -1 ;
                        novi.Sifra = Convert.ToInt32(tbSifra.Text);
                        novi.Naziv = tbNaziv.Text;
                        novi.Cena = float.Parse(tbCena.Text);
                        novi.Kolicina = int.Parse(tbKolicina.Text);
                        novi.NaStanju = stanje - kolicina;
                     
                      UnosFaktura.artikli.Add(novi);
                ukupnaCena = UnosFaktura.racunajUkupnuCenu(UnosFaktura.artikli);
                unosFaktura.tbUkupanIznos.Text = ukupnaCena.ToString() + " dinara.";
                      
                MessageBox.Show("Uspesno ste promenili kolicinu!", "Obavestenje", MessageBoxButton.OK);
                                   
                    this.Close();
                      
            }
            else
            {
                if (tbKolicina.Text != "")
                {
                    int stanje = Convert.ToInt32(tbNaStanju.Text);
                    int kolicina = Convert.ToInt32(tbKolicina.Text);
                   
                    
                    if (stanje > kolicina)
                    {


                        
                        novi.Broj = broj;
                        novi.Sifra = Convert.ToInt32(tbSifra.Text);
                        novi.Naziv = tbNaziv.Text;
                        novi.Cena = float.Parse(tbCena.Text);
                        novi.Kolicina = int.Parse(tbKolicina.Text);
                        novi.NaStanju = stanje - kolicina;
                        novi.PomocniINDEX = comboArtikli.SelectedIndex;
                    
                        


                       
                        UnosFaktura.artikli.Add(novi);                      
                        MessageBox.Show("Uspesno ste dodali artikal u korpu!");
                        broj++;
                        ukupnaCena = UnosFaktura.racunajUkupnuCenu(UnosFaktura.artikli);
                        unosFaktura.tbUkupanIznos.Text = ukupnaCena.ToString() + " dinara.";
                        tbNaStanju.Text = novi.NaStanju.ToString();
                      
                        this.Close();
                    
                    }
                    else
                    {
                        MessageBox.Show("Artikla pod nazivom" + tbNaziv.Text + " nema na stanju u unetoj kolicini!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Morate uneti broj u polje 'Kolicina'", "Greska!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }


        }


        public bool Exist(int sifra)
        {
            bool postoji = false;
            foreach (Artikal a in UnosFaktura.artikli)
            {
                if (sifra == a.Sifra)
                {
                    postoji = true;
                    break;
                }
                else
                {
                    postoji = false;
                }


            }

            return postoji;
        }

        private int vratiPoziciju(int sifra)
        {
            int index = 0;
            foreach(Artikal a in UnosFaktura.artikli)
            {
                if (a.Sifra == sifra)
                    index = UnosFaktura.artikli.IndexOf(a);
            }
            return index;
        }
    }
}

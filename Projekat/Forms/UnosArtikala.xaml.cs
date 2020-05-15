using System;

using System.Windows;

using System.Windows.Input;

using System.Data.SQLite;


namespace Projekat
{
    /// <summary>
    /// Interaction logic for UnosArtikala.xaml
    /// </summary>
    public partial class UnosArtikala : Window
    {

        bool zatvoriOdmah = false;

        Boolean isChanged = false;

        Artikal artikal;


        private MainWindow mainForm;


        public UnosArtikala(MainWindow main)
        {
            InitializeComponent();
            this.mainForm = main;
       
        }

        public UnosArtikala()
        {
            InitializeComponent();
        }

        public bool ValidacijaPolja()
        {
            bool sifra, naziv, cena, kolicina = false;
            int pomocINT=0;
            float pomocFLOAT=0;

            textGreskaSifra.Text = (string.IsNullOrEmpty(tbsifra.Text)) ? "Polje 'Sifra' ne sme biti prazno!" :
                textGreskaSifra.Text = (!int.TryParse(tbsifra.Text, out pomocINT)) ? "U polju 'Sifra' se moraju uneti cifre!" : " ";
            textGreskaSifra.Visibility = (string.IsNullOrEmpty(tbsifra.Text) || !int.TryParse(tbsifra.Text, out pomocINT)) ? Visibility.Visible : Visibility.Hidden;
            sifra = (string.IsNullOrEmpty(tbsifra.Text) || !int.TryParse(tbsifra.Text, out pomocINT)) ? false : true;

            textGreskaNaziv.Text = (string.IsNullOrEmpty(tbNaziv.Text)) ? "Polje 'Naziv' ne sme biti prazno!" : "";
            textGreskaNaziv.Visibility = (string.IsNullOrEmpty(tbNaziv.Text)) ? Visibility.Visible : Visibility.Hidden;
            naziv = (textGreskaNaziv.IsVisible) ? false : true;

            textGreskaCena.Text = (string.IsNullOrEmpty(tbCena.Text)) ? "Polje 'Cena' ne sme biti prazno!" :
                textGreskaCena.Text = (!float.TryParse(tbCena.Text, out pomocFLOAT)) ? "U polju 'Cena' se moraju uneti cifre!" : "";
            textGreskaCena.Visibility = (string.IsNullOrEmpty(tbCena.Text) || !float.TryParse(tbCena.Text, out pomocFLOAT)) ? Visibility.Visible : Visibility.Hidden;
            cena = (textGreskaCena.IsVisible) ? false : true;


            textGreskaKolicina.Text = (string.IsNullOrEmpty(tbKolicina.Text)) ? "Polje 'Kolicina' ne sme biti prazno!" :
              textGreskaKolicina.Text = (!int.TryParse(tbKolicina.Text, out pomocINT)) ? "U polju 'Kolicina' se moraju uneti cifre!" : "";
            textGreskaKolicina.Visibility = (string.IsNullOrEmpty(tbKolicina.Text) || !int.TryParse(tbKolicina.Text, out pomocINT)) ? Visibility.Visible : Visibility.Hidden;
            kolicina = (textGreskaKolicina.IsVisible) ? false : true;

            if (sifra && naziv && cena && kolicina)
                return true;
            else
                return false;






        }

    

        private void HideErrors()
        {
           
            textGreskaSifra.Visibility = Visibility.Hidden;
            
            textGreskaNaziv.Visibility = Visibility.Hidden;

          
            textGreskaCena.Visibility = Visibility.Hidden;

           
            textGreskaKolicina.Visibility = Visibility.Hidden;

            
        }




        private void BtnPotvrdi_Click(object sender, RoutedEventArgs e)
        {

            HideErrors();

            if (!Exist(mainForm.SelectedArticleID()))
            {
                if (ValidacijaPolja())
                {
                    InsertDataInDB();
                    this.mainForm.showDataGridArticles();
                }
            }
            else
            {
               
                if(ValidacijaPolja())
                {
                    UpdateDataInDB();
                    zatvoriOdmah = true;
                    this.mainForm.showDataGridArticles();
                    this.Close();
                }
            }
        }

        private void InsertDataInDB()
        {

            artikal = new Artikal();
            artikal.Sifra = int.Parse(tbsifra.Text);
            artikal.Naziv = tbNaziv.Text;
            artikal.Cena = float.Parse(tbCena.Text);
            artikal.Kolicina = int.Parse(tbKolicina.Text);
            artikal.Napomena = tbNapomena.Text;
            string pdv = SettingsClass.Finansije.vratiPDV();

            if (Artikal.Insert(artikal.Sifra, artikal.Naziv, artikal.Cena, artikal.Kolicina, artikal.Napomena ))
            {
                int artikalID = Convert.ToInt32(Artikal.VratiArtikal_ID(artikal.Naziv));
                int napomenaID = Convert.ToInt32(Artikal.VratiNapomena_ID_Tekst(artikal.Napomena));
                if (Artikal.InsertDataIntoArtikliNapomene(artikalID, napomenaID))
                {
                    MessageBox.Show("Uspesno ste uneli artikal");
                    Resetuj();
                }
            }
            else
            {
                MessageBox.Show("Neuspesna konekcija!");
            }
        }

        private void UpdateDataInDB()
        {
            artikal = new Artikal();
            artikal.Sifra = int.Parse(tbsifra.Text);
            artikal.Naziv = tbNaziv.Text;
            artikal.Cena = float.Parse(tbCena.Text);
            artikal.Kolicina = int.Parse(tbKolicina.Text);
            artikal.Napomena = tbNapomena.Text;


            string pomoc = Artikal.VratiNapomena_ID_Artikal(artikal.Sifra);
            int napomenaID = int.Parse(pomoc);

            if(Artikal.Update(artikal.Sifra,artikal.Naziv, artikal.Cena, artikal.Kolicina, artikal.Napomena, napomenaID))
            {
                MessageBox.Show("Uspesno ste azurirali artikal");
                Resetuj();
            }
            else
            {
                MessageBox.Show("Neuspesna konekcija!");
            }
        }


        public void DeleteArticleFromDB(int ArtikalID)
        {
           if(Artikal.Delete(ArtikalID))
            {
                MessageBox.Show("Uspesno ste obrisali artikal!");
                Resetuj();
            }
           else
            {
                MessageBox.Show("Konekcija neuspesna!");
            }
           
        }



        void Resetuj()
        {
            tbsifra.Clear();
            tbNaziv.Clear();
            tbCena.Clear();
            tbKolicina.Clear();
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

        private void TbCena_KeyUp(object sender, KeyEventArgs e)
        {
            this.isChanged = true;
        }

        private void Tbsifra_KeyUp(object sender, KeyEventArgs e)
        {
            this.isChanged = true;
        }

        private void TbKolicina_KeyUp(object sender, KeyEventArgs e)
        {
            this.isChanged = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(zatvoriOdmah)
            {
                e.Cancel = false;
                
            }
        else if(isChanged && MessageBoxResult.No == MessageBox.Show("Da li ste sigurni da zelite da izadjete iz programa?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Information))
            {
                e.Cancel = true;
            }
           
  
        }

        public bool Exist(int id)
        {

            bool exist = false;
            SQLiteConnection konekcija = new SQLiteConnection(Konekcija.konekcioniString);
            string sqlID = "select Naziv from Artikli where ID=@ID";

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

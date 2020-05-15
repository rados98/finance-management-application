using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;

namespace Projekat
{
    public class KolekcijaArtikala:ObservableCollection<Artikal>
    {
        public KolekcijaArtikala()
        {
            DataTable tabelaArtikla = Artikal.getArticles();
           

            foreach (DataRow row in tabelaArtikla.Rows)
            {
                Artikal artikal = new Artikal();
                artikal.Sifra = Convert.ToInt32(row["ID"]);
                artikal.Naziv = (string)row["Naziv"];
                artikal.Cena = Convert.ToInt32(row["Cena"]);
                artikal.Kolicina = Convert.ToInt32(row["Kolicina"]);

                
                
               Add(artikal);
            }
            

        }
    }
}

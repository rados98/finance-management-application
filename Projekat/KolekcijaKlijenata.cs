using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;

namespace Projekat
{
    class KolekcijaKlijenata:ObservableCollection<Klijent>
    {
        public KolekcijaKlijenata()
        {
            DataTable tabelaKlijenti = Klijent.getClients();

            foreach(DataRow row in tabelaKlijenti.Rows)
            {
                Klijent klijent = new Klijent();
                klijent.Id = Convert.ToInt32(row["ID"]);
                klijent.Pib= Convert.ToDouble(row["PIB"]);
                klijent.Naziv= (string)row["Naziv"];
                klijent.MaticniB= Convert.ToDouble( row["MaticniBroj"]);
                klijent.Adresa= (string)row["Adresa"];
                klijent.AdresaEX= (string)row["AdresaEks"];
                klijent.TekuciR = (string)row["TekuciRacun"];

                Add(klijent);
            }
        }
    }
}

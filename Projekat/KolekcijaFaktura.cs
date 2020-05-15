using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;

namespace Projekat
{
    class KolekcijaFaktura:ObservableCollection<Faktura>
    {
        public KolekcijaFaktura()
        {
            DataTable tabelaFaktura = Faktura.getFactures();
            foreach (DataRow row in tabelaFaktura.Rows)
            {
                Faktura faktura = new Faktura();
                faktura.Id = Convert.ToInt32(row["ID"]);
                faktura.BrojFakture = (string)row["Broj_fakture"];
                faktura.RokZaUplatu = (string)row["Rok_za_uplatu"];
                faktura.KlijentID = Convert.ToInt32(row["Klijent_ID"]);
                faktura.NazivKlijenta = Klijent.VratiNazivKlijenta(faktura.KlijentID);
                faktura.UkupnaCena = Convert.ToDouble(row["Ukupan_iznos"]);


                Add(faktura);
            }
        }
    }
}

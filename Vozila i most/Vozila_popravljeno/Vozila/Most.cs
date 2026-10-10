using System;
using System.Collections.Generic;
using System.Linq;

namespace Vozila
{
    public class Most
    {
        public double Duzina { get; private set; }
        public double Nosivost { get; private set; }
        public double Razmak { get; private set; }
        public List<Vozilo> VozilaNaMostu { get; private set; }
        public int BrojVozilaPrekoMosta { get; private set; }
        public double UkupnaTezina { get { return VozilaNaMostu.Sum(v => v.Tezina); } }

        public Most(double duzina, double nosivost, double razmak)
        {
            if (duzina <= 0 || nosivost <= 0 || razmak < 0)
                throw new ArgumentOutOfRangeException("Parametri mosta");
            Duzina = duzina; Nosivost = nosivost; Razmak = razmak;
            VozilaNaMostu = new List<Vozilo>();
        }

        public bool DodajVozilo(Vozilo vozilo)
        {
            if (vozilo == null) throw new ArgumentNullException("vozilo");
            if (VozilaNaMostu.Contains(vozilo) || vozilo.Duzina > Duzina ||
                UkupnaTezina + vozilo.Tezina > Nosivost || ProveriSudar(vozilo))
                return false;
            vozilo.Pozicija = 0;
            VozilaNaMostu.Add(vozilo);
            return true;
        }

        public bool ProveriSudar(Vozilo novoVozilo)
        {
            if (novoVozilo == null) throw new ArgumentNullException("novoVozilo");
            return VozilaNaMostu.Any(v => v.Pozicija < novoVozilo.Duzina + Razmak);
        }

        public void PomjeriVozila()
        {
            foreach (Vozilo vozilo in VozilaNaMostu) vozilo.Pozicija += 5;
            foreach (Vozilo vozilo in VozilaNaMostu.Where(v => v.Pozicija >= Duzina).ToList())
                ObrisiVozilo(vozilo);
        }

        public void ObrisiVozilo(Vozilo vozilo)
        {
            if (vozilo != null && vozilo.Pozicija >= Duzina && VozilaNaMostu.Remove(vozilo))
                BrojVozilaPrekoMosta++;
        }
    }
}

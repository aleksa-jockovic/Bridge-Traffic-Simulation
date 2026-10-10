using System;
using System.Drawing;

namespace Vozila
{
    public abstract class Vozilo
    {
        public string Registracija { get; private set; }
        public double Tezina { get; private set; }
        public double Duzina { get; private set; }
        public double Pozicija { get; set; }
        protected abstract Color Boja { get; }
        protected Vozilo(string registracija, double tezina, double duzina)
        {
            if (tezina <= 0 || duzina <= 0) throw new ArgumentOutOfRangeException("Dimenzije vozila");
            Registracija = registracija; Tezina = tezina; Duzina = duzina;
        }
        public virtual void Nacrtaj(Graphics g, int x, int y)
        {
            int duzina = (int)Duzina;
            using (Brush cetka = new SolidBrush(Boja))
                g.FillRectangle(cetka, x, y, duzina, 26);
            g.FillRectangle(Brushes.LightBlue, x + duzina - 20, y + 3, 14, 10);
            g.FillEllipse(Brushes.Black, x + 6, y + 20, 14, 14);
            g.FillEllipse(Brushes.Black, x + duzina - 20, y + 20, 14, 14);
        }
    }
    public class PutnickoVozilo : Vozilo
    {
        protected override Color Boja { get { return Color.RoyalBlue; } }
        public PutnickoVozilo(string registracija, double tezina, double duzina)
            : base(registracija, tezina, duzina) { }
    }
    public class TeretnoVozilo : Vozilo
    {
        protected override Color Boja { get { return Color.DarkOrange; } }
        public TeretnoVozilo(string registracija, double tezina, double duzina)
            : base(registracija, tezina, duzina) { }
    }
}

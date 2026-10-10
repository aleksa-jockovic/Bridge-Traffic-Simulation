using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vozila
{
    public partial class Form1 : Form
    {
        private Most most;
        private readonly Random random = new Random();
        private Vozilo voziloNaCekanju;
        private int brojDodatih;
        private const int Cilj = 10;
        private readonly Button ponovo = new Button();
        private string status;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Controls.Remove(pictureBox1);
            pictureBox1.Dispose();
            Text = "Vozila preko mosta";
            ClientSize = new Size(900, 360);
            ponovo.Text = "Pokreni ponovo";
            ponovo.SetBounds(30, 280, 150, 32);
            ponovo.Click += delegate { Pokreni(); };
            Controls.Add(ponovo);
            timer1.Interval = 50;
            Pokreni();
        }

        private void Pokreni()
        {
            timer1.Stop();
            most = new Most(600, 10000, 10);
            brojDodatih = 0; voziloNaCekanju = null;
            status = "Simulacija je pokrenuta.";
            timer1.Start(); Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            most.PomjeriVozila();
            if (brojDodatih < Cilj)
            {
                if (voziloNaCekanju == null)
                {
                    bool teretno = random.Next(2) == 0;
                    voziloNaCekanju = teretno
                        ? (Vozilo)new TeretnoVozilo("NS" + random.Next(100, 1000), random.Next(3000, 5001), random.Next(100, 151))
                        : new PutnickoVozilo("BG" + random.Next(100, 1000), random.Next(1000, 2001), random.Next(50, 81));
                }
                if (most.DodajVozilo(voziloNaCekanju))
                {
                    brojDodatih++; voziloNaCekanju = null;
                    status = "Vozilo je uslo na most.";
                }
                else
                    status = most.UkupnaTezina + voziloNaCekanju.Tezina > most.Nosivost
                        ? "Vozilo ceka: ogranicenje nosivosti."
                        : "Vozilo ceka: potreban je dovoljan razmak.";
            }
            if (most.BrojVozilaPrekoMosta == Cilj && most.VozilaNaMostu.Count == 0)
            {
                timer1.Stop(); status = "Gotovo! Svih 10 vozila je preslo most.";
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (most == null) return;
            Graphics g = e.Graphics;
            g.DrawString("Nosivost: " + most.Nosivost + " kg   |   Trenutno opterecenje: " + most.UkupnaTezina + " kg", Font, Brushes.Black, 30, 25);
            g.DrawString("Preslo: " + most.BrojVozilaPrekoMosta + " / " + Cilj + "   |   Na mostu: " + most.VozilaNaMostu.Count, Font, Brushes.Black, 30, 50);
            using (Pen olovka = new Pen(Color.DimGray, 6)) g.DrawLine(olovka, 100, 160, 700, 160);
            g.FillRectangle(Brushes.Gray, 130, 165, 20, 60);
            g.FillRectangle(Brushes.Gray, 650, 165, 20, 60);
            System.Drawing.Drawing2D.GraphicsState stanje = g.Save();
            g.SetClip(new Rectangle(100, 100, 600, 60));
            foreach (Vozilo vozilo in most.VozilaNaMostu)
                vozilo.Nacrtaj(g, 100 + (int)vozilo.Pozicija, 125);
            g.Restore(stanje);
            g.DrawString(status, Font, Brushes.Black, 30, 240);
            if (voziloNaCekanju != null)
                g.DrawString("Na cekanju: " + voziloNaCekanju.Registracija + ", " + voziloNaCekanju.Tezina + " kg", Font, Brushes.Black, 30, 215);
        }
        private void Form1_Load(object sender, EventArgs e) { }
    }
}

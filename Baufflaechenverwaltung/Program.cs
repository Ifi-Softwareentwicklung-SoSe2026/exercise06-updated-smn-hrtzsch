using System;
using System.Collections.Generic;

namespace Baufflaechenverwaltung
{
    public enum FlaechenStatus { Frei, Reserviert, Bebaut }
    public enum VorhabenStatus { AntragEingereicht, Genehmigt, Abgelehnt, InBearbeitung, Abgeschlossen }

    public class Antragsteller
    {
        public string Name { get; set; } = string.Empty;
        public string Kontakt { get; set; } = string.Empty;
        public string Firma { get; set; } = string.Empty;
    }

    public class Bauvorhaben
    {
        public string Titel { get; set; } = string.Empty;
        public Antragsteller Antragsteller { get; set; } = new();
        public string GeplanteNutzung { get; set; } = string.Empty;
        public DateTime Beginn { get; set; }
        public DateTime Fertigstellung { get; set; }
        public VorhabenStatus Status { get; set; }
        public List<Bauflaeche> ZugeordneteFlaechen { get; set; } = new();

        public void StatusAktualisieren(VorhabenStatus neuerStatus)
        {
            Status = neuerStatus;
            Console.WriteLine($"Status von Vorhaben '{Titel}' auf {neuerStatus} aktualisiert.");
        }
    }

    public class Bauflaeche
    {
        public string FlaechenId { get; set; } = string.Empty;
        public double Groesse { get; set; }
        public string Lage { get; set; } = string.Empty;
        public string Nutzung { get; set; } = string.Empty;
        public string Bebaubarkeit { get; set; } = string.Empty;
        public string BPlanNummer { get; set; } = string.Empty;
        public decimal Bodenrichtwert { get; set; }
        public string Eigentuemer { get; set; } = string.Empty;
        public FlaechenStatus Status { get; set; } = FlaechenStatus.Frei;

        public bool BebaubarkeitPruefen()
        {
            bool istBebaubar = Bebaubarkeit.ToLower() == "ja";
            Console.WriteLine($"Prüfung Fläche {FlaechenId} (B-Plan {BPlanNummer}): Bebaubarkeit = {Bebaubarkeit} -> {(istBebaubar ? "Zulässig" : "Nicht zulässig")}");
            return istBebaubar;
        }

        public void FlaecheReservieren()
        {
            if (Status == FlaechenStatus.Bebaut)
            {
                Console.WriteLine($"Fläche {FlaechenId} kann nicht reserviert werden, da sie bereits bebaut ist.");
                return;
            }

            if (Status == FlaechenStatus.Frei)
            {
                Status = FlaechenStatus.Reserviert;
                Console.WriteLine($"Fläche {FlaechenId} wurde erfolgreich reserviert.");
            }
            else
            {
                Console.WriteLine($"Fläche {FlaechenId} kann nicht reserviert werden (Status: {Status}).");
            }
        }
    }

    public class Grundstueck
    {
        public string FlurstueckNummer { get; set; } = string.Empty;
        public List<Bauflaeche> Bauflaechen { get; set; } = new();
    }

    public class Bauverwaltung
    {
        public Bauvorhaben BauvorhabenAnlegen(string titel, Antragsteller ersteller, string nutzung, DateTime start, DateTime ende, List<Bauflaeche> flaechen)
        {
            var vorhaben = new Bauvorhaben
            {
                Titel = titel,
                Antragsteller = ersteller,
                GeplanteNutzung = nutzung,
                Beginn = start,
                Fertigstellung = ende,
                Status = VorhabenStatus.AntragEingereicht,
                ZugeordneteFlaechen = flaechen
            };
            Console.WriteLine($"Bauvorhaben '{titel}' wurde erfolgreich angelegt.");
            return vorhaben;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var verwaltung = new Bauverwaltung();
            var grundstueck = new Grundstueck { FlurstueckNummer = "0015 00012 001/002" };
            
            var flaeche1 = new Bauflaeche 
            {
                FlaechenId = "F1", 
                Groesse = 500, 
                Lage = "Nord", 
                Nutzung = "Brachfläche", 
                Bebaubarkeit = "ja", 
                BPlanNummer = "BP-2022-089", 
                Bodenrichtwert = 500m, 
                Eigentuemer = "Max Mustermann"
            };
            
            var flaeche2 = new Bauflaeche 
            {
                FlaechenId = "F2", 
                Groesse = 300, 
                Lage = "Süd", 
                Nutzung = "Wohngebiet", 
                Bebaubarkeit = "nein", 
                BPlanNummer = "BP-2022-090", 
                Bodenrichtwert = 600m, 
                Eigentuemer = "Lisa Schmidt",
                Status = FlaechenStatus.Bebaut
            };

            grundstueck.Bauflaechen.Add(flaeche1);
            grundstueck.Bauflaechen.Add(flaeche2);

            var antragsteller = new Antragsteller { Name = "Erika Musterfrau", Firma = "BauAG", Kontakt = "erika@bauag.de" };

            Console.WriteLine("--- Demonstration Bauflächenverwaltung ---");
            
            // Test 1: Bebaubarkeit prüfen
            flaeche1.BebaubarkeitPruefen();
            flaeche2.BebaubarkeitPruefen();

            // Test 2: Reservierung einer freien Fläche
            flaeche1.FlaecheReservieren();

            // Test 3: Reservierung einer bebauten Fläche (Validierung)
            flaeche2.FlaecheReservieren();

            // Bauvorhaben anlegen
            var meinVorhaben = verwaltung.BauvorhabenAnlegen(
                "Wohnkomplex Nord", 
                antragsteller, 
                "Wohngebäude", 
                DateTime.Now, 
                DateTime.Now.AddYears(2), 
                new List<Bauflaeche> { flaeche1 }
            );

            meinVorhaben.StatusAktualisieren(VorhabenStatus.Genehmigt);

            Console.WriteLine("--- Ende der Demonstration ---");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities;

public class Vaka
{
    public int vakaId { get; set; }

    public string vakaTuru { get; set; }
    public string vakaAdi { get; set; }
    public string vakaCiddiyetSeviyesi { get; set; }
    public string vakaDurumu { get; set; }
    public string vakaAciklamasi { get; set; }

    public DateTime vakaOlusmaTarihi { get; set; }
    public DateTime vakaKapanmaTarihi { get; set; }

    public string olayNedeni { get; set; }

    // Foreign key - raporlayan personel
    public int personelId { get; set; }
    public Personel Personel { get; set; }

    // Foreign key - ilgili ekipman
    public int ekipmanId { get; set; }
    public Ekipman Ekipman { get; set; }
}
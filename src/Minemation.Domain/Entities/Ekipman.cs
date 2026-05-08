using System;
using System.Collections.Generic;
using System.Text;


namespace Minemation.Domain.Entities;

public class Ekipman
{
    public int ekipmanId { get; set; }

    public string ekipmanAdi { get; set; } = string.Empty;
    public string ekipmanMarka { get; set; } = string.Empty;
    public string ekipmanModel { get; set; } = string.Empty;
    public string durum { get; set; } = string.Empty;

    public DateTime sonBakimTarihi { get; set; }
    public DateTime gelecekBakimTarihi { get; set; }

    public string seriNo { get; set; } = string.Empty;
    public string RFIDEtiket { get; set; } = string.Empty;

    public string ureticiFirma { get; set; } = string.Empty;
    public string tedarikciFirma { get; set; } = string.Empty;

    public DateTime uretimYili { get; set; }

    public decimal boyut { get; set; }
    public decimal agirlik { get; set; }

    public DateTime satinAlmaTarihi { get; set; }
    public DateTime garantiBaslangicTarihi { get; set; }

    public string teknikDokuman { get; set; } = string.Empty;
    public string kullanimKilavuzu { get; set; } = string.Empty;
    public string garantiBelgesi { get; set; } = string.Empty;
    public string bakimFormu { get; set; } = string.Empty;
    public string satinAlmaBelgesi { get; set; } = string.Empty;

    public string operasyonTuru { get; set; } = string.Empty;

    public Sensor? Sensor { get; set; }
}

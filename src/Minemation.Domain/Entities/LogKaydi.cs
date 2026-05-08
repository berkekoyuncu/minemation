using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities;

public class LogKaydi
{
    public int logKaydiID { get; set; }

    public string islemTipi { get; set; }
    public string logKaydiAciklamasi { get; set; }

    public DateTime logKaydiTarihi { get; set; }

    public string ipAdresi { get; set; }
    public string önemSeviyesi { get; set; }
    public string durum { get; set; }

    // Foreign key - işlemi yapan personel
    public int personelId { get; set; }
    public Personel Personel { get; set; }

    // Foreign key - ilgili ekipman
    public int ekipmanId { get; set; }
    public Ekipman Ekipman { get; set; }
}

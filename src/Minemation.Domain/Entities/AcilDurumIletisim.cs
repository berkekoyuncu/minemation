using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities;

public class AcilDurumIletisim
{
    public int acilDurumKisisiId { get; set; }

    public string acilDurumKisileriAd { get; set; } = string.Empty;
    public string acilDurumKisileriSoyad { get; set; } = string.Empty;
    public string acilDurumKisileriYakinlik { get; set; } = string.Empty;
    public string acilDurumKisileriTelNo { get; set; } = string.Empty;

    public int personelId { get; set; }
    public Personel Personel { get; set; } = null!;
}

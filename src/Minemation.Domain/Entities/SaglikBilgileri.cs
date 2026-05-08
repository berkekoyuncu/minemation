using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities;

public class SaglikBilgileri
{
    public string kanGrubu { get; set; } = string.Empty;
    public string saglikDurumu { get; set; } = string.Empty;

    public List<string> kronikHastaliklar { get; set; } = new();
    public List<string> alerjiler { get; set; } = new();

    public string saglikCalismaKisitlamalari { get; set; } = string.Empty;
    public string acilDurumNotu { get; set; } = string.Empty;
    public DateTime sonMuayeneTarihi { get; set; }

    public int personelId { get; set; }
    public Personel Personel { get; set; } = null!;
}

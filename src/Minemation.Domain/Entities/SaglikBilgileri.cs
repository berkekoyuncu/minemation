using System.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities
{
    public class SaglikBilgileri
    {
        // Hem PK hem de FK olarak kullanılacak
        public int personelId { get; set; }

        public string kanGrubu { get; set; }
        public string saglikDurumu { get; set; }

        // Bu listeleri veritabanında string olarak saklayacağız
        public List<string> kronikHastaliklar { get; set; } = new List<string>();
        public List<string> alerjiler { get; set; } = new List<string>();

        public string saglikCalismaKisitlamalari { get; set; }
        public string acilDurumNotu { get; set; }
        public DateTime sonMuayeneTarihi { get; set; }

        // Navigasyon Özelliği
        public virtual Personel Personel { get; set; }
    }
}
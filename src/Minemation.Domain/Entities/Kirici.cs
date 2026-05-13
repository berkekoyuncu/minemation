using System;

namespace Minemation.Domain.Entities
{
    public class Kirici : Ekipman
    {
        public decimal darbeEnerjisi { get; set; }
        public decimal dakikadakiDarbeSayisi { get; set; }
        public decimal calismaBasinci { get; set; }
        public string ucTipi { get; set; }
        public string gerekenYagDebisi { get; set; }

    }
}
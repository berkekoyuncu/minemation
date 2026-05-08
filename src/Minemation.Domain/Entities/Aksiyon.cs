using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities;

public class Aksiyon
{
    public int mudahaleId { get; set; }

    public DateTime mudahaleBaslangicSaati { get; set; }
    public DateTime mudahaleBitisSaati { get; set; }

    public string mudahaleTuru { get; set; }
    public string uygulananCozum { get; set; }

    // Foreign key
    public int ekipId { get; set; }
    public Ekip Ekip { get; set; }

    public int vakaId { get; set; }
    public Vaka Vaka { get; set; }
}

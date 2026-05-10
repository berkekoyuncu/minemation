using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Application.DTOs;

public class GirisYapDto
{
    public string KullaniciAdi { get; set; } = string.Empty;
    public string Sifre { get; set; } = string.Empty;
}

public class GirisSonucDto
{
    public int PersonelId { get; set; }
    public string PersonelAdi { get; set; } = string.Empty;
    public string PersonelSoyadi { get; set; } = string.Empty;
    public string Eposta { get; set; } = string.Empty;
    public string KullaniciRolu { get; set; } = string.Empty;
    public string PersonelDurumu { get; set; } = string.Empty;
    public DateTime? SonGirisTarihi { get; set; }
}

public class RfidGirisDto
{
    public string RfidKartNumarasi { get; set; } = string.Empty;
}

namespace Minemation.Desktop.Models;

public class GirisYapRequest
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

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}

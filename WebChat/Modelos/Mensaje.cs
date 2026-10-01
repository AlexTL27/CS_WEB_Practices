using System.ComponentModel.DataAnnotations;
namespace WebChat.Modelos;

public class Mensaje
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string Usuario { get; set; }
    
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(1000,ErrorMessage = "No más de 1000 caracteres")]
    public string Texto { get; set; }

    public DateTimeOffset fechaRegistro { get; set; } =  DateTimeOffset.UtcNow;
}
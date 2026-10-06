using Microsoft.AspNetCore.Mvc;
using WebChat.Data;
using WebChat.Modelos;

namespace WebChat.Controllers;

[ApiController]
[Route("api")]
public class Principal : ControllerBase
{
    private readonly ApplicationDbContext _context;


    public Principal(ApplicationDbContext applicationDbContext)
    {
        _context = applicationDbContext;
    }


    [HttpPost("AgregarMensaje")]
    public async Task<IActionResult> AgregarMensajeBase([FromBody] Mensaje mensaje)
    {
        //Si llega aquí el mensaje se supone que está bien, así que lo guardamos en la base
        _context.Mensajes.Add(mensaje);
        await _context.SaveChangesAsync();
        return Ok(new { estado = "hecho", mensajee = mensaje });
    }



    //Hacer que al iniciar el front, enviar los ultimos 50 mensajes generados
    [HttpGet("EnviarMensajes")]
    public IActionResult EnviarUltimosMensajes()
    {
        List<Mensaje> mensajes =  _context.Mensajes.OrderByDescending(m => m.Id)
            .Take(50)
            .AsEnumerable().Reverse()
            .ToList();

       
        return  Ok(new { mensajes });
    }
}


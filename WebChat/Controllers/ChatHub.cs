
using Microsoft.AspNetCore.SignalR;
using WebChat.Data;
using WebChat.Modelos;

namespace WebChat.Controllers;

public class ChatHub : Hub
{
    private readonly ApplicationDbContext _context;
    
    public ChatHub(ApplicationDbContext applicationDbContext)
    {
        _context = applicationDbContext;
    }
    
    
    public async Task SendMessage(Mensaje mensaje)
    {
        //Si llega aquí el mensaje se supone que está bien, así que lo guardamos en la base
        _context.Mensajes.Add(mensaje);
        await _context.SaveChangesAsync();
        
        await Clients.All.SendAsync("ReceiveMessage", mensaje);
    }
}
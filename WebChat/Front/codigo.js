document.addEventListener('DOMContentLoaded', () => {

    function crearMensaje(usuario ,texto, tipo){
    const contenedor = document.getElementById('contenedor-chats');
    let template;


    if (tipo === "recibido"){
        template = document.getElementById('tpl-recibido');
    }
    else if (tipo === "enviado"){
        template = document.getElementById('tpl-enviado')
    }
    else{
        return;
    }
    let usermsj = `${usuario}: ${texto}`


    const clon = template.content.cloneNode(true);
    clon.querySelector('.texto-mensaje').textContent = usermsj;

    contenedor.appendChild(clon);
    contenedor.appendChild(document.createElement('br'));
}



    // Función para recibir los ultimos 50 mensajes y mapearlos
    async function obtenerMensajes() {
        try{
            const respuesta = await fetch("http://localhost:5238/api/EnviarMensajes")
            
            if (!respuesta.ok){
                throw new Error("Ocurrio u error al obtener los datos")
            }

            const datos = await respuesta.json();

            //imprimirlos
           datos.mensajes.forEach((msj) => {
                crearMensaje(msj.usuario,msj.texto, "recibido")
           });

        }
        catch (error){
             console.log("Error:", error);
        }

    }


    const conexion = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5238/chat")
        .withAutomaticReconnect()
        .build();

    async function iniciarChat() {
        try{
            await conexion.start();
            console.log("Conectado exitosamente al ChatHub");
        }
        catch (error) {
            console.error("Error al conectar:", error);

        }
    }

    conexion.on("ReceiveMessage", (mensaje) => {
    // Extraer propiedades soportando mayúsculas o minúsculas por serialización JSON
    const usuario = mensaje.usuario || mensaje.Usuario || "Anónimo";
    const texto = mensaje.texto || mensaje.Texto || "";

    crearMensaje(usuario, texto, "recibido");
});

    //Al iniciar se llama a
    obtenerMensajes();

    //despues de inizializara el chat hub
    iniciarChat();
})


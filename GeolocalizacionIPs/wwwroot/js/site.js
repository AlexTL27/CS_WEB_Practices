// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



document.getElementById("opciones").addEventListener
    ("change", function () {
        if (this.value === "opcion2") {

            let filas = document.querySelectorAll("tbody tr");
         
            let miSet = new Set();

            filas.forEach(fila => {
                let ip = fila.cells[0].textContent.trim();

                if (miSet.has(ip)) {
                    fila.style.display = "none";
                }
                else {
                    miSet.add(ip);
                }
            })
        }
        else if (this.value === "opcion1") {
            let filas = document.querySelectorAll("tbody tr");

            filas.forEach(fila => {
                fila.style.display = "table-row";
            })
        }
    }
    );

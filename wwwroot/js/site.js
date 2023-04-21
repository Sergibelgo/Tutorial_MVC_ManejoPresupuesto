// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
async function cargarOperaciones() {
    const valorSelec = $("#TipoOperacionId").val();

    const respuesta = await fetch(url, {
        method: "POST",
        body: valorSelec,
        headers: {
            "Content-Type": "application/json"
        }
    })
    const json = await respuesta.json();
    const opciones = json.map(categoria => `<option value=${categoria.value}>${categoria.text}</option>`);
    $("#CategoriaId").html(opciones);
}

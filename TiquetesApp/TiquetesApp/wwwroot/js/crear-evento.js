document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('crear-evento-form');
    const alertContainer = document.getElementById('alert-container');
    const btnSubmit = document.getElementById('btn-submit');

    form.addEventListener('submit', async function (e) {
        e.preventDefault();
        alertContainer.innerHTML = '';
        btnSubmit.disabled = true;
        btnSubmit.textContent = 'Guardando';

        const model = {
            nombre: document.getElementById('Nombre').value.trim(),
            fecha: document.getElementById('Fecha').value,
            lugar: document.getElementById('Lugar').value.trim(),
            precio: parseFloat(document.getElementById('Precio').value) || 0,
            cantidadDisponible: parseInt(document.getElementById('CantidadDisponible').value) || 0
        };

        const result = await apiClient.post('/eventos', model);

        if (result.ok) {
            mostrarAlerta('Evento creado exitosamente.', 'success');
            setTimeout(() => window.location.href = '/eventos', 2000);
        } else {
            const errors = result.data.errors
                ? result.data.errors.join('<br>')
                : result.data.message;
            mostrarAlerta(errors);
            btnSubmit.disabled = false;
            btnSubmit.textContent = 'Guardar Evento';
        }
    });

    function mostrarAlerta(mensaje, tipo = 'danger') {
        alertContainer.innerHTML = `
            <div class="alert alert-${tipo} alert-dismissible fade show" role="alert">
                ${mensaje}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>`;
    }
});
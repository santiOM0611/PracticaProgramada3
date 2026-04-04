document.addEventListener('DOMContentLoaded', async function () {
    const eventoId = document.getElementById('compra-container').dataset.eventoId;
    const infoContainer = document.getElementById('evento-info');
    const form = document.getElementById('compra-form');
    const alertContainer = document.getElementById('alert-container');
    const totalSpan = document.getElementById('total-calculado');
    const inputCantidad = document.getElementById('Cantidad');
    const btnSubmit = document.getElementById('btn-submit');

    let precioUnitario = 0;

    // Cargar info del evento
    try {
        const result = await apiClient.get(`/eventos/${eventoId}`);

        if (!result.ok) {
            infoContainer.innerHTML = `<div class="alert alert-warning">Evento no encontrado.</div>`;
            return;
        }

        const e = result.data;
        precioUnitario = parseFloat(e.precio);

        infoContainer.innerHTML = `
            <div class="card mb-4">
                <div class="card-body">
                    <h5 class="card-title">${e.nombre}</h5>
                    <p class="card-text">
                        <strong>Fecha:</strong> ${e.fecha}<br>
                        <strong>Lugar:</strong> ${e.lugar}<br>
                        <strong>Precio por tiquete:</strong> 
                        ₡${precioUnitario.toLocaleString('es-CR', { minimumFractionDigits: 2 })}<br>
                        <strong>Disponibles:</strong> ${e.cantidadDisponible}
                    </p>
                </div>
            </div>`;

        inputCantidad.max = e.cantidadDisponible;

    } catch (error) {
        infoContainer.innerHTML = `<div class="alert alert-danger">Error al cargar el evento.</div>`;
        return;
    }

    // Calcular el total dinámicamente
    inputCantidad.addEventListener('input', function () {
        const cantidad = parseInt(this.value) || 0;
        const total = cantidad * precioUnitario;
        totalSpan.textContent = `₡${total.toLocaleString('es-CR', { minimumFractionDigits: 2 })}`;
    });

    // Enviar compra
    form.addEventListener('submit', async function (e) {
        e.preventDefault();
        alertContainer.innerHTML = '';
        btnSubmit.disabled = true;
        btnSubmit.textContent = 'Procesando';

        const model = {
            eventoId: parseInt(eventoId),
            cantidad: parseInt(inputCantidad.value) || 0
        };

        const result = await apiClient.post('/compras', model);

        if (result.ok) {
            alertContainer.innerHTML = `
                <div class="alert alert-success">
                     ${result.data.message}<br>
                    <strong>Total pagado:</strong> 
                    ₡${parseFloat(result.data.total).toLocaleString('es-CR', { minimumFractionDigits: 2 })}
                </div>`;
            setTimeout(() => window.location.href = '/compras/historial', 2500);
        } else {
            alertContainer.innerHTML = `
                <div class="alert alert-danger">${result.data.message || 'Error al procesar la compra.'}</div>`;
            btnSubmit.disabled = false;
            btnSubmit.textContent = 'Confirmar Compra';
        }
    });
});
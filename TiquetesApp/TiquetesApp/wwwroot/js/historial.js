document.addEventListener('DOMContentLoaded', cargarHistorial);

async function cargarHistorial() {
    const tbody = document.getElementById('historial-tbody');
    const loading = document.getElementById('historial-loading');
    const totalGeneral = document.getElementById('total-general');

    try {
        const result = await apiClient.get('/compras/historial');

        loading.classList.add('d-none');

        if (!result.ok) {
            tbody.innerHTML = `<tr><td colspan="6" class="text-center text-danger">Error al cargar historial.</td></tr>`;
            return;
        }

        const compras = result.data;

        if (compras.length === 0) {
            tbody.innerHTML = `<tr><td colspan="6" class="text-center text-muted">No tienes compras registradas.</td></tr>`;
            return;
        }

        let sumaTotal = 0;

        tbody.innerHTML = compras.map(c => {
            sumaTotal += parseFloat(c.total);
            return `
                <tr>
                    <td>${c.eventoNombre}</td>
                    <td>${c.eventoFecha}</td>
                    <td>${c.eventoLugar}</td>
                    <td>${c.cantidad}</td>
                    <td>₡${parseFloat(c.total).toLocaleString('es-CR', { minimumFractionDigits: 2 })}</td>
                    <td>${c.fechaCompra}</td>
                </tr>`;
        }).join('');

        totalGeneral.textContent = `₡${sumaTotal.toLocaleString('es-CR', { minimumFractionDigits: 2 })}`;

    } catch (error) {
        loading.classList.add('d-none');
        tbody.innerHTML = `<tr><td colspan="6" class="text-center text-danger">Error de conexión.</td></tr>`;
    }
}
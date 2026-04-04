document.addEventListener('DOMContentLoaded', cargarEventos);

async function cargarEventos() {
    const tbody = document.getElementById('eventos-tbody');
    const loading = document.getElementById('eventos-loading');

    try {
        const result = await apiClient.get('/eventos');

        loading.classList.add('d-none');

        if (!result.ok) {
            tbody.innerHTML = `<tr><td colspan="6" class="text-center text-danger">Error al cargar eventos.</td></tr>`;
            return;
        }

        const eventos = result.data;

        if (eventos.length === 0) {
            tbody.innerHTML = `<tr><td colspan="6" class="text-center text-muted">No hay eventos registrados.</td></tr>`;
            return;
        }

        tbody.innerHTML = eventos.map(e => `
            <tr>
                <td>${e.nombre}</td>
                <td>${e.fecha}</td>
                <td>${e.lugar}</td>
                <td>₡${parseFloat(e.precio).toLocaleString('es-CR', { minimumFractionDigits: 2 })}</td>
                <td>
                    <span class="badge ${e.cantidadDisponible > 0 ? 'bg-success' : 'bg-danger'}">
                        ${e.cantidadDisponible > 0 ? e.cantidadDisponible + ' disponibles' : 'Agotado'}
                    </span>
                </td>
                <td>
                    ${e.cantidadDisponible > 0
                ? `<a href="/compras/comprar/${e.id}" class="btn btn-primary btn-sm">Comprar</a>`
                : `<button class="btn btn-secondary btn-sm" disabled>Agotado</button>`
            }
                </td>
            </tr>`).join('');

    } catch (error) {
        loading.classList.add('d-none');
        tbody.innerHTML = `<tr><td colspan="6" class="text-center text-danger">Error de conexión.</td></tr>`;
    }
}
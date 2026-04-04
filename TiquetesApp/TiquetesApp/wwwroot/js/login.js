document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('login-form');
    const alertContainer = document.getElementById('alert-container');
    const btnSubmit = document.getElementById('btn-submit');

    form.addEventListener('submit', async function (e) {
        e.preventDefault();
        alertContainer.innerHTML = '';
        btnSubmit.disabled = true;
        btnSubmit.textContent = 'Ingresando';

        const model = {
            email: document.getElementById('Email').value.trim(),
            password: document.getElementById('Password').value,
            rememberMe: document.getElementById('RememberMe').checked
        };

        const result = await apiClient.post('/auth/login', model);

        if (result.ok) {
            // Guardar el JWT y redirigir
            apiClient.setToken(result.data.token);
            window.location.href = '/eventos';
        } else {
            mostrarAlerta(result.data.message || 'Error al iniciar sesión.');
            btnSubmit.disabled = false;
            btnSubmit.textContent = 'Ingresar';
        }
    });

    function mostrarAlerta(mensaje) {
        alertContainer.innerHTML = `
            <div class="alert alert-danger alert-dismissible fade show" role="alert">
                ${mensaje}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>`;
    }
});
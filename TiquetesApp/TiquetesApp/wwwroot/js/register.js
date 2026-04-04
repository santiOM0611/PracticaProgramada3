document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('register-form');
    const alertContainer = document.getElementById('alert-container');
    const btnSubmit = document.getElementById('btn-submit');

    form.addEventListener('submit', async function (e) {
        e.preventDefault();
        alertContainer.innerHTML = '';

        const password = document.getElementById('Password').value;
        const confirm = document.getElementById('ConfirmPassword').value;

        if (password !== confirm) {
            mostrarAlerta('Las contraseñas no coinciden.');
            return;
        }

        btnSubmit.disabled = true;
        btnSubmit.textContent = 'Registrando';

        const model = {
            email: document.getElementById('Email').value.trim(),
            password,
            confirmPassword: confirm,
            nombreCompleto: document.getElementById('NombreCompleto').value.trim()
        };

        const result = await apiClient.post('/auth/register', model);

        if (result.ok) {
            mostrarAlerta(result.data.message, 'success');
            setTimeout(() => window.location.href = '/auth/login', 2000);
        } else {
            const errors = result.data.errors
                ? result.data.errors.join('<br>')
                : result.data.message;
            mostrarAlerta(errors);
            btnSubmit.disabled = false;
            btnSubmit.textContent = 'Registrarse';
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
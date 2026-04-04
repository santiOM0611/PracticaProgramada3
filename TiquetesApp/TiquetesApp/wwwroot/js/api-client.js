// Módulo central para todas las llamadas al API.
// Maneja el token JWT en localStorage y lo incluye en cada petición.
const apiClient = {

    baseUrl: '/api',

    // Guarda el token JWT recibido tras el login
    setToken(token) {
        localStorage.setItem('jwt_token', token);
    },

    getToken() {
        return localStorage.getItem('jwt_token');
    },

    removeToken() {
        localStorage.removeItem('jwt_token');
    },

    // Construye los headers con Authorization si existe token
    _headers() {
        const headers = { 'Content-Type': 'application/json' };
        const token = this.getToken();
        if (token) headers['Authorization'] = `Bearer ${token}`;
        return headers;
    },

    async get(url) {
        const response = await fetch(this.baseUrl + url, {
            method: 'GET',
            headers: this._headers(),
            credentials: 'include'
        });
        return { ok: response.ok, status: response.status, data: await response.json() };
    },

    async post(url, body) {
        const response = await fetch(this.baseUrl + url, {
            method: 'POST',
            headers: this._headers(),
            credentials: 'include',
            body: JSON.stringify(body)
        });
        return { ok: response.ok, status: response.status, data: await response.json() };
    }
};
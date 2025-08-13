document.getElementById("loginForm")?.addEventListener("submit", async (e) => {
    e.preventDefault();

    // Ocultar error previo
    const errorEl = document.getElementById("loginError");
    if (errorEl) errorEl.classList.add("d-none");

    const fd = new FormData(e.target);
    const payload = {
        email: fd.get("email"),
        password: fd.get("password")
    };

    try {
        const res = await fetch("/Account/Login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        if (res.ok) {
            // login correcto: recargar página para reflejar sesión y rol
            location.reload();
        } else {
            if (errorEl) errorEl.classList.remove("d-none");
        }
    } catch (err) {
        if (errorEl) errorEl.classList.remove("d-none");
        console.error(err);
    }
});

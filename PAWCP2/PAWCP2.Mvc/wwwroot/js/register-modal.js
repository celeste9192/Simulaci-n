document.getElementById("registerForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const data = {
        username: this.username.value,
        email: this.email.value,
        fullName: this.fullname.value
    };

    const res = await fetch("/Account/Register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });

    if (res.ok) {
        alert("Usuario registrado con éxito. Ahora puede iniciar sesión.");
        this.reset();
        bootstrap.Modal.getInstance(document.getElementById("registerModal")).hide();
    } else {
        const errorText = await res.text();
        document.getElementById("registerError").textContent = errorText;
        document.getElementById("registerError").classList.remove("d-none");
    }

  
});

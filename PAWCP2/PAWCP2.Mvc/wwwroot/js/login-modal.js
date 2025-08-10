document.getElementById("loginForm")?.addEventListener("submit", async (e) => {
    e.preventDefault();
    const fd = new FormData(e.target);
    const payload = { email: fd.get("email") };

    const res = await fetch("/Account/Login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
    });

    if (res.ok) {
        location.reload();
    } else {
        document.getElementById("loginError")?.classList.remove("d-none");
    }
});

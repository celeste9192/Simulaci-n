using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Agregar MVC
builder.Services.AddControllersWithViews();

// Agregar soporte para sesi�n
builder.Services.AddDistributedMemoryCache(); // almacenamiento en memoria para sesi�n
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // tiempo de expiraci�n
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<AuthCookieHandler>();

builder.Services.AddHttpClient("api", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
}).AddHttpMessageHandler<AuthCookieHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// **Activar sesi�n antes de autorizaci�n**
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public class AuthCookieHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _http;
    public AuthCookieHandler(IHttpContextAccessor http) => _http = http;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var token = _http.HttpContext?.Request.Cookies["fb_access_token"];
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return base.SendAsync(request, ct);
    }
}


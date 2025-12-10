using AlphaProject.CRM.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using AlphaProject.CRM;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// leggiamo i valori da appsettings / secrets
var zitadelProjectId = builder.Configuration["Zitadel-Project-Id"];
var zitadelClientId = builder.Configuration["Zitadel-Client-Id"];

// 1) Authentication: Cookie + OpenID Connect (Zitadel)
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(options =>
    {
        options.Authority = "https://istanza1-nbzw7h.us1.zitadel.cloud";
        options.ClientId = zitadelClientId;      // Web App OIDC
        options.ResponseType = "code";           // Authorization Code
        options.UsePkce = true;                  // coerente con Zitadel PKCE
        options.SaveTokens = true;               // salva access/refresh nei cookie
        options.GetClaimsFromUserInfoEndpoint = true;

        options.CallbackPath = "/signin-oidc";
        options.SignedOutCallbackPath = "/signout-callback-oidc";

        // Scope richiesti
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add($"urn:zitadel:iam:org:project:id:{zitadelProjectId}:aud");
    });


builder.Services.AddScoped<OrdersClient>();

// 2) HttpClient che porta con sé l’access token dell’utente
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AccessTokenHandler>();

builder.Services.AddHttpClient("ApiWithUserToken", client =>
{
    client.BaseAddress = new Uri("https://localhost:7223/"); // URL della tua API
})
.AddHttpMessageHandler<AccessTokenHandler>();

// 3) Authorization: tutto il sito richiede utente autenticato
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

// 4) Middleware auth
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireAuthorization();      // tutto il Blazor richiede login

app.MapGet("/logout", async (HttpContext context) =>
{
    // Cancella il cookie di autenticazione locale
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    // Avvia il logout presso l’Identity Provider (ZITADEL)
    await context.SignOutAsync(
        OpenIdConnectDefaults.AuthenticationScheme,
        new AuthenticationProperties
        {
            // Dove tornare una volta completato il logout
            RedirectUri = "/"
        });
}).RequireAuthorization();

app.Run();
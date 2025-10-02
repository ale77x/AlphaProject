using System.ComponentModel.DataAnnotations;

namespace AlphaProject.Shared.Dtos
{
    /// <summary>
    /// DTO per le richieste di autenticazione.
    /// </summary>
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Il nome utente è obbligatorio")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "La password è obbligatoria")]
        public string Password { get; set; } = default!;
    }
}

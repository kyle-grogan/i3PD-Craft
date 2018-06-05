using System.ComponentModel.DataAnnotations;

namespace USNStarterKit.USNModels
{
    /// <summary>
    /// Not using strongly typed models here so that PureLive mode can be used
    /// </summary>
    public class USNLoginFormViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public string LoginFormButtonText { get; set; }

    }
}
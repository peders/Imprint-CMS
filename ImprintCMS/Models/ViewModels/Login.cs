using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models.ViewModels
{
	public class Login
	{
		[Required(ErrorMessage = "*")]
		public string Username { get; set; }
		[Required(ErrorMessage = "*")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
using System.ComponentModel.DataAnnotations;

namespace ImprintCMS.Models.ViewModels
{
	public class FileUpload
	{
		public string FileName { get; set; }
		[Required(ErrorMessage = "*")]
		public string FileCategory { get; set; }
		public int ReplaceId { get; set; }
	}
}
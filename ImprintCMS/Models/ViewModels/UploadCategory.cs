using System.Collections.Generic;

namespace ImprintCMS.Models.ViewModels
{
	public class UploadCategory
	{
		public string Name { get; set; }
		public IEnumerable<UploadedFile> Files { get; set; }
	}
}
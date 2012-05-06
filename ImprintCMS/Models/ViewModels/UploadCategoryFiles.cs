using System.Collections.Generic;

namespace ImprintCMS.Models.ViewModels
{
	public class UploadCategoryFiles
	{
		public string Name { get; set; }
		public IEnumerable<ListFile> Files { get; set; }
	}
}
namespace ImprintCMS.Models.ViewModels
{
	public struct ListFile
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public string Category { get; set; }
		public int ContentLength { get; set; }
		public string ContentType { get; set; }
	}
}
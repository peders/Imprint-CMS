using System.ComponentModel.DataAnnotations;
using System;

namespace ImprintCMS.Models
{
	[MetadataType(typeof(EditionMetadata))]
	public partial class Edition
	{
		public string Name
		{
			get
			{
				return String.Format("{0} ({1} – {2})", Book.Title, Number, Binding.Name);
			}
		}
	}

	public class EditionMetadata
	{
		[Required(ErrorMessage = "*")]
		public int BookId { get; set; }
		[Required(ErrorMessage = "*")]
		public int BindingId { get; set; }
		[Required(ErrorMessage = "*")]
		public DateTime ReleaseDate { get; set; }
		[Required(ErrorMessage = "*")]
		public int Number { get; set; }
		[Required(ErrorMessage = "*")]
		public string Isbn { get; set; }
	}
}
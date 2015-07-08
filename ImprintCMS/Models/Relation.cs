using System.ComponentModel.DataAnnotations;
using System;

namespace ImprintCMS.Models
{
	[MetadataType(typeof(RelationMetadata))]
	public partial class Relation
	{
		public string ReversePersonName
		{
			get
			{
				return String.Format("{0}{1}", Person.ReverseName, !String.IsNullOrWhiteSpace(Role.ShortName) ? String.Format(" ({0})", Role.ShortName) : string.Empty).Trim();
			}
		}

		public string PersonName
		{
			get
			{
				return String.Format("{0} {1}{2}", Person.FirstName, Person.LastName, !String.IsNullOrWhiteSpace(Role.ShortName) ? String.Format(" ({0})", Role.ShortName) : string.Empty).Trim();
			}
		}
	}

	public class RelationMetadata
	{
		[Required(ErrorMessage = "*")]
		public int PersonId { get; set; }
		[Required(ErrorMessage = "*")]
		public int RoleId { get; set; }
	}
}
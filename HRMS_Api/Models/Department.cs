using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace HRMS_Api.Models
{
	public class Department
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Name { get; set; }
		[MaxLength(170)]
		public string Description { get; set; }
		[ForeignKey("LocationId")]
		public virtual Location Location { get; set; }

		public int? LocationId { get; set; }
		[Required]
		public DateTime CreateDateTime { get; set; }
		[Required]
		public DateTime UpdateDateTime { get; set; }

		[JsonIgnore]
		public virtual ICollection<Job> jobs { get; set; }
	}

}

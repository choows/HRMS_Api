using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace HRMS_Api.Models
{
	public class Location
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }
		[Required]
		[MaxLength(170)]
		public string City { get; set; }
		[Required]
		[MaxLength(60)]
		public string Country { get; set; }
		[Required]
		public DateTime CreateDateTime { get; set; }
		[Required]
		public DateTime UpdateDateTime { get; set; }
		[JsonIgnore]
		public virtual ICollection<Department> department { get; set; }
	}
}

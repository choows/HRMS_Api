using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HRMS_Api.Models
{
	public class Job
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Name { get; set; }
		[MaxLength(170)]
		public string Description { get; set; }

		[ForeignKey("DepartmentId")]
		public virtual Department Department { get; set; }
		public int? DepartmentId { get; set; }
		[Required]
		public DateTime CreateDateTime { get; set; }
		[Required]
		public DateTime UpdateDateTime { get; set; }
		[JsonIgnore]
		public virtual ICollection<JobHistory> jobsHistory { get; set; }
	}
}

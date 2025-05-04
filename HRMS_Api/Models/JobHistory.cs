using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRMS_Api.Models
{
	public class JobHistory
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[ForeignKey("EmployeeId")]
		public virtual Employee Employee { get; set; }
		[ForeignKey("ManagerId")]
		public virtual Employee Manager { get; set; }

		[ForeignKey("JobId")]
		public virtual Job Job { get; set; }
		[Required]
		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }
		[Required]
		[MaxLength(50)]
		public string Status { get; set; }
		[MaxLength(500)]
		public string Comments { get; set; }
		public int? ManagerId { get; set; }
		public int? EmployeeId { get; set; }
		public int? JobId { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HRMS_Api.Models
{
	public class Employee
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Name { get; set; }
		[Required]
		public DateTime BirthDate { get; set; }
		[Required]
		[MaxLength (20)]
		public string Phone { get; set; }

		[MaxLength(100)]
		public string Email { get; set; }
		[Required]
		[MaxLength(20)]
		public string Gender { get; set; }
		[Required]
		[MaxLength(200)]
		public string Address { get; set; }

		[Required]
		[MaxLength(50)]
		public string Status { get; set; }
		[JsonIgnore]
		[InverseProperty("Employee")]
		public virtual ICollection<JobHistory> JobHistoryAsEmployee { get; set; }
		[JsonIgnore]
		[InverseProperty("Manager")]
		public virtual ICollection<JobHistory> JobHistoryAsManager { get; set; }


	}
}

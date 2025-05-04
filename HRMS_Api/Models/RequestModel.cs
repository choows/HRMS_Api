using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS_Api.Models
{
	public class AddLocationModel
	{
		[Required]
		public string Name { get; set; } //only check if name exist 
		[Required]
		public string City { get; set; }
		[Required]
		public string Country { get; set; }
	}

	public class AddDepartmentModel
	{
		[Required]
		public string Name { get; set; }
		public string Description { get; set; } = "";
		[Required]
		public int LocationId { get; set; }
	}

	public class AddJobModel
	{
		[Required]
		public string Name { get; set; }
		public string Description { get; set; } = "";
		[Required]
		public int DepartmentId { get; set; }
	}
	public class AddEmployeeModel
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public DateTime BirthDate { get; set; }
		[Required]
		public string Phone { get; set; }

		public string Email { get; set; } = "";
		[Required]
		public string Gender { get; set; }
		[Required]
		public string Address { get; set; }

		[Required]
		public string Status { get; set; }
	}

	public class EmployeeSearchCriteriaModel
	{
		public string name { get; set; } = string.Empty;
		public int? Department { get; set; }
		public string Gender { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
		public int? CurrentPage { get; set; }
		public int? PerPage { get; set; }
		public DateTime? StartDate { get; set; } = null;
		public DateTime? EndDate { get; set; } = null;
	}

	public class AddJobHistory
	{
		[Required]
		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }
		[Required]
		public string Status { get; set; }
		public string Comments { get; set; } = "";
		[Required]
		public int ManagerId { get; set; }
		[Required]
		public int EmployeeId { get; set; }
		[Required]
		public int JobId { get; set; }
	}
}

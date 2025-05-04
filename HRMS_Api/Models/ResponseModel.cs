namespace HRMS_Api.Models
{
	public class GetLocationResponse
	{
		public List<Location> Locations { get; set; }
		public int TotalRecord { get; set; }
	}
	public class GetDepartmentResponse
	{
		public List<Department> Departments { get; set; }
		public int TotalRecord { get; set; }
	}
	public class GetJobResponse
	{
		public List<Job> Jobs { get; set; }
		public int TotalRecord { get; set; }
	}
	public class GetJobHistoryResponse
	{
		public List<JobHistory> JobHistories { get; set; }
		public int TotalRecord { get; set; }
	}
	public class GetEmployeeResponse
	{
		public List<Employee> Employees { get; set; }
		public int TotalRecord { get; set; }
	}
}

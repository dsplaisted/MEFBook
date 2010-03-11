using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResumeFilterContracts
{
	public class Resume
	{
		public Dictionary<string, Proficiency> Skills { get; set; }
		public List<EmploymentHistoryItem> EmploymentHistory { get; set; }
		public List<EducationHistoryItem> EducationHistory { get; set; }
	}

	public enum Proficiency
	{
		None,
		Beginner,
		Intermediate,
		Advanced,
		Expert
	}

	public class EmploymentHistoryItem
	{
		public string Company { get; set; }
		public string Position { get; set; }
		public double YearsEmployed { get; set; }
	}

	public class EducationHistoryItem
	{
		public string InstitutionName { get; set; }
		public string DegreeSubject { get; set; }				//	Computer science, Philosophy, etc
		public string DegreeLevel { get; set; }					//	B.S., M.S., PhD
		public int YearGraduated { get; set; }
	}
}

namespace NovaApp.Models
{
    public class Project
    {
        public long Id { get; set; }

        public string title { get; set; } 

        public string Description { get; set; } = string.Empty;

        public User ClientOwner { get; set; } 

        public List<int> Jobs { get; set; } = new List<int>();

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? DeadlineDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public List<Note> Notes { get; set; } = new List<Note>();

        public List<Fund> Funds { get; set; } = new List<Fund>();

        public int? Profile { get; set; }
    }

    public class User
    {
        public long Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public List<int> Rolls { get; set; } = new List<int> { 2000 };

        public List<long> Jobs { get; set; } = new List<long>();

        public int PayPerHour { get; set; } // Default value is 0

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public int ProfileImage { get; set; } = 1;

        public int BillableHours { get; set; } = 8;

        public List<Job> CreatedJobs { get; set; } = new List<Job>();

        public List<Project> Projects { get; set; } = new List<Project>();

        public List<Note> Notes { get; set; } = new List<Note>();

        public List<Fund> Funds { get; set; } = new List<Fund>();
    }

    public class Fund
    {
        public long Id { get; set; }

        public int Expenses { get; set; } = 0;

        public int Income { get; set; } = 0;

        public string Note { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Project Project { get; set; }

        public User Owner { get; set; }
    }

    public class Job
    {
        public long Id { get; set; }

        public Project Project { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? DeadlineDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public User CreatedBy { get; set; }

        public User CreatedFor { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class Note
    {
        public long Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string NoteText { get; set; } = string.Empty;

        public User Owner { get; set; }

        public Project Project { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace NovaApp.Models
{
    public class Project
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("clientOwner")]
        public User ClientOwner { get; set; }

        [JsonPropertyName("jobs")]
        public List<int> Jobs { get; set; } = new List<int>();

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("deadlineDate")]
        public DateTime? DeadlineDate { get; set; }

        [JsonPropertyName("completedDate")]
        public DateTime? CompletedDate { get; set; }

        [JsonPropertyName("notes")]
        public List<Note> Notes { get; set; } = new List<Note>();

        [JsonPropertyName("funds")]
        public List<Fund> Funds { get; set; } = new List<Fund>();

        [JsonPropertyName("profile")]
        public int? Profile { get; set; }
    }

    public class User
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("rolls")]
        public List<int> Rolls { get; set; } = new List<int> { 2000 };

        [JsonPropertyName("jobs")]
        public List<long> Jobs { get; set; } = new List<long>();

        [JsonPropertyName("payPerHour")]
        public int PayPerHour { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("profileImage")]
        public int ProfileImage { get; set; } = 1;

        [JsonPropertyName("billableHours")]
        public int BillableHours { get; set; } = 8;

        [JsonPropertyName("createdJobs")]
        public List<Job> CreatedJobs { get; set; } = new List<Job>();

        [JsonPropertyName("projects")]
        public List<Project> Projects { get; set; } = new List<Project>();

        [JsonPropertyName("notes")]
        public List<Note> Notes { get; set; } = new List<Note>();

        [JsonPropertyName("funds")]
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

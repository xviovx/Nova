﻿using System.Text.Json.Serialization;

namespace NovaApp.Models
{

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
        public string projectId { get; set; }

        public string projectName { get; set; }

        public string clientOwnerName { get; set; }

        [JsonPropertyName("id")]
        public string id { get; set; }

        public int expenses { get; set; } = 0;

        public int income { get; set; } = 0;

        public string note { get; set; } = string.Empty;

        public DateTime createdDate { get; set; } = DateTime.UtcNow;

        public Project project { get; set; }

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

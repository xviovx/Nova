﻿using NovaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaApp.Services
{
    public interface IRestProjectsService
    {
        Task<List<Project>> RefreshProjectsListAsync();
        Task<Project> GetProject(string ProjectId);

        //add project
        Task SaveProjectAsync(createProjectDto project, bool isNewItem = false);

        Task<List<TaskDisplay>> GetTaskList(string ProjectId);
        Task UpdateProjectAsync(string projectId, createProjectDto updatedProject);
    }
}

using GeoGravityOverDose.ViewModels.Base;
using ReactiveUI.Fody.Helpers;
using GeoGravityOverDose.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive;
using System.Windows;

namespace GeoGravityOverDose.Views.Entity.CustomerEntity
{
    public class CustomerCardViewModel : BaseViewModel
    {
        [Reactive]
        public Customer Customer { get; set; }

        [Reactive]
        public Project SelectedProject { get; set; }

        public IReactiveCommand<Unit, Unit> TestMessageCommand { get; }
        public IReactiveCommand<Unit, Unit> AddProjectCommand { get; }
        public IReactiveCommand<Project, Unit> DeleteProjectCommand { get; }
        public IReactiveCommand<Project, Unit> NavigateToProjectCommand { get; }

        public int MaxLengthName { get; set; } = 20;
        public int MaxLengthPassword { get; set; } = 10;
        public int MaxLengthPhone { get; set; } = 15;

        public void TestMessage()
        {
            MessageBox.Show("1");
        }

        public void AddProject()
        {
            var Project = new Project() { Name="New Project" };
            Customer.Projects.Add(Project);
            SelectedProject = Project;
        }
        private void DeleteProject(Project project)
        {
            var _deletedProjectIndex = Customer.Projects.IndexOf(project);
            var _selectedProjectIndex = Customer.Projects.IndexOf(SelectedProject);
            Customer.Projects.Remove(project);

            if (_deletedProjectIndex == _selectedProjectIndex)
            {
                if (_deletedProjectIndex == 0)
                { SelectedProject = Customer.Projects.FirstOrDefault(); }
                else if (_deletedProjectIndex == Customer.Projects.Count)
                { SelectedProject = Customer.Projects.LastOrDefault(); }
                else if (_deletedProjectIndex > 0 && _deletedProjectIndex < Customer.Projects.Count)
                { SelectedProject = Customer.Projects[_deletedProjectIndex]; }
            }
        }

        public CustomerCardViewModel()
        {
            TestMessageCommand = ReactiveCommand.Create(TestMessage);

            AddProjectCommand = ReactiveCommand.Create(AddProject);
            DeleteProjectCommand = ReactiveCommand.Create<Project>(DeleteProject);
            
        }
    }
}


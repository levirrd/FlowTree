using Avalonia.Controls;
using Avalonia.Interactivity;
using FlowTree.Avalonia.Services;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Threading.Tasks;

namespace FlowTree.Avalonia
{
    public partial class CreateProjectWindow : Window
    {
        private readonly string _connectionString;

        public CreateProjectWindow(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
        }

        private void Cancel_Click(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }

        private async void Create_Click(object? sender, RoutedEventArgs e)
        {
            string name = ProjectNameBox.Text ?? "";
            string subtitle = ProjectSubtitleBox.Text ?? "";
            string description = ProjectDescriptionBox.Text ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                await ShowMessage("Hiba", "A projekt címe kötelező.");
                return;
            }

            try
            {
                var projectService = new ProjectService(_connectionString);

                await projectService.CreateProjectAsync(
                    name,
                    subtitle,
                    description
                );

                Close(true);
            }
            catch (Exception ex)
            {
                await ShowMessage("Hiba", ex.Message);
            }
        }

        private async Task ShowMessage(string title, string message)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard(title, message, ButtonEnum.Ok);

            await box.ShowAsync();
        }
    }
}
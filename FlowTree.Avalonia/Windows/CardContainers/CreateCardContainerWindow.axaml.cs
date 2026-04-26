using Avalonia.Controls;
using Avalonia.Interactivity;
using FlowTree.Avalonia.Services;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Threading.Tasks;

namespace FlowTree.Avalonia
{
    public partial class CreateCardContainerWindow : Window
    {
        private readonly string _connectionString;
        private readonly int _projectId;

        public CreateCardContainerWindow(string connectionString, int projectId)
        {
            InitializeComponent();

            _connectionString = connectionString;
            _projectId = projectId;
        }

        private void Cancel_Click(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }

        private async void Create_Click(object? sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text ?? "";
            string description = DescriptionBox.Text ?? "";

            if (string.IsNullOrWhiteSpace(title))
            {
                await ShowMessage("Error", "The title is necessary.");
                return;
            }


            try
            {
                var service = new CardContainerService(_connectionString);

                await service.CreateAsync(
                    _projectId,
                    title,
                    description
                    );

                Close(true);
            }
            catch (Exception ex)
            {
                await ShowMessage("Error", ex.Message);
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
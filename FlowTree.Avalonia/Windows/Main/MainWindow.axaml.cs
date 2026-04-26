using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using FlowTree.Avalonia.Data;
using FlowTree.Avalonia.Models;
using FlowTree.Avalonia.Services;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Linq;
using System.Threading.Tasks;
namespace FlowTree.Avalonia
{
    public partial class MainWindow : Window
    {
        private Project? _selectedProject;
        public MainWindow()
        {
            InitializeComponent();
            UpdateAccountUi();
        }

        private async void Login_Click(object? sender, RoutedEventArgs e)
        {
            var databaseLoginWindow = new DatabaseLoginWindow();
            await databaseLoginWindow.ShowDialog(this);

            UpdateAccountUi();

            if (!string.IsNullOrWhiteSpace(DatabaseSession.ConnectionString))
            {
                await LoadProjectsAsync();
            }
        }

        private void Logout_Click(object? sender, RoutedEventArgs e)
        {
            DatabaseSession.Clear();

            ProjectsPanel.Children.Clear();

            UpdateAccountUi();
        }

        private async void NewProject_Click(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DatabaseSession.ConnectionString))
            {
                await ShowMessage("Hiba", "El?ször csatlakozz az adatbázishoz.");
                return;
            }

            var window = new CreateProjectWindow(DatabaseSession.ConnectionString);
            bool? created = await window.ShowDialog<bool?>(this);

            if (created == true)
            {
                await LoadProjectsAsync();
            }
        }

        private void UpdateAccountUi()
        {
            if (DatabaseSession.IsLoggedIn)
            {
                AccountUsernameText.Text = DatabaseSession.Username;
                AccountDatabaseText.Text = $"Database: {DatabaseSession.DatabaseName}";
                AccountStatusText.Text = "Logged in";
                AccountStatusText.IsVisible = true;

                LoginButton.IsVisible = false;
                LogoutButton.IsVisible = true;
            }
            else
            {
                AccountUsernameText.Text = "Not logged in";
                AccountDatabaseText.Text = "Local account";
                AccountStatusText.Text = "";
                AccountStatusText.IsVisible = false;

                LoginButton.IsVisible = true;
                LogoutButton.IsVisible = false;
            }
        }

        private async Task ShowMessage(string title, string message)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard(title, message, ButtonEnum.Ok);

            await box.ShowAsync();
        }
        private async void NewCardContainer_Click(object? sender, RoutedEventArgs e)
        {
            if (_selectedProject == null)
            {
                await ShowMessage("Hiba", "El?ször válassz ki egy projektet.");
                return;
            }

            var window = new CreateCardContainerWindow(
                DatabaseSession.ConnectionString,
                _selectedProject.Id
            );

            bool? created = await window.ShowDialog<bool?>(this);

            if (created == true)
            {
                await LoadCardContainersAsync();
            }
        }
        private async Task LoadProjectsAsync()
        {
            await using var context = new FlowTreeDbContext(DatabaseSession.ConnectionString);

            var projects = await context.Projects
                .OrderBy(p => p.Id)
                .ToListAsync();

            ProjectsPanel.Children.Clear();

            foreach (var project in projects)
            {
                var card = new Border
                {
                    Background = Brush.Parse("#383838"),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(10),
                    Cursor = new Cursor(StandardCursorType.Hand),
                    Child = new StackPanel
                    {
                        Spacing = 4,
                        Children =
                        {
                            new TextBlock
                            {
                                Text = project.Name,
                                Foreground = Brushes.White,
                                FontWeight = FontWeight.SemiBold,
                                TextWrapping = TextWrapping.Wrap
                            },
                            new TextBlock
                            {
                                Text = project.Subtitle,
                                Foreground = Brush.Parse("#AAAAAA"),
                                FontSize = 12,
                                TextWrapping = TextWrapping.Wrap
                            }
                        }
                    }
                };
                card.PointerPressed += async (_, _) =>
                {
                    _selectedProject = project;

                    SelectedProjectTitleText.Text = project.Name;
                    SelectedProjectSubtitleText.Text = project.Subtitle;

                    NewCardContainerButton.IsVisible = true;

                    await LoadCardContainersAsync();
                };
                ProjectsPanel.Children.Add(card);
            }
        }
        private async Task LoadCardContainersAsync()
        {
            CardContainersPanel.Children.Clear();

            if (_selectedProject == null)
            {
                return;
            }

            var service = new CardContainerService(DatabaseSession.ConnectionString);

            var containers = await service.GetByProjectIdAsync(_selectedProject.Id);

            foreach (var container in containers)
            {
                var card = new Border
                {
                    Background = Brush.Parse("#252525"),
                    BorderBrush = Brush.Parse("#444444"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(12),
                    Padding = new Thickness(16),
                    Child = new StackPanel
                    {
                        Spacing = 6,
                        Children =
                {
                    new TextBlock
                    {
                        Text = container.IsCompleted
                            ? $"? {container.Title}"
                            : container.Title,
                        Foreground = Brushes.White,
                        FontSize = 18,
                        FontWeight = FontWeight.SemiBold,
                        TextWrapping = TextWrapping.Wrap
                    },
                    new TextBlock
                    {
                        Text = container.Description,
                        Foreground = Brush.Parse("#AAAAAA"),
                        FontSize = 13,
                        TextWrapping = TextWrapping.Wrap
                    },
                }
                    }
                };

                CardContainersPanel.Children.Add(card);
            }
        }
    }
}
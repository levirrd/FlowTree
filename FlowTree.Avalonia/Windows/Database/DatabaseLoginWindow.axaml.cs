using Avalonia.Controls;
using Avalonia.Interactivity;
using FlowTree.Avalonia.Data;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTree.Avalonia
{
    public partial class DatabaseLoginWindow : Window
    {
        public DatabaseLoginWindow()
        {
            InitializeComponent();
        }

        private void Btn_Cancel(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Btn_Connect(object? sender, RoutedEventArgs e)
        {
            string serverName = ServerNameBox.Text ?? "";
            string databaseName = DatabaseNameBox.Text ?? "";
            string username = UsernameBox.Text ?? "";
            string password = PasswordBox.Text ?? "";

            if (string.IsNullOrWhiteSpace(serverName) ||
                string.IsNullOrWhiteSpace(databaseName) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                await ShowMessage("Error", "Please fill in every field.");
                return;
            }

            string connectionString =
                $"Server={serverName};Database={databaseName};User Id={username};Password={password};TrustServerCertificate=True;";

            try
            {
                await using var db = new FlowTreeDbContext(connectionString);

                await db.Database.EnsureCreatedAsync();

                await ShowMessage(
                    "Success",
                    "Connection successful. The database was created if it did not already exist."
                );
                DatabaseSession.ConnectionString = connectionString;
                DatabaseSession.Username = username;
                DatabaseSession.DatabaseName = databaseName;
                Close();
            }
            catch (Exception ex)
            {
                await ShowMessage("Connection failed", ex.Message);
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

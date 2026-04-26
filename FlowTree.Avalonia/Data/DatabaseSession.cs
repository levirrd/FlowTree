using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTree.Avalonia.Data
{
    public static class DatabaseSession
    {
        // This class holds the current database session information, like the connection string, username, and database name.
        public static string ConnectionString { get; set; } = string.Empty;
        public static string Username { get; set; } = string.Empty;
        public static string DatabaseName { get; set; } = string.Empty;

        public static bool IsLoggedIn =>
            !string.IsNullOrWhiteSpace(ConnectionString);
        // Clears the session information, effectively logging out the user.
        public static void Clear()
        {
            ConnectionString = string.Empty;
            Username = string.Empty;
            DatabaseName = string.Empty;
        }
    }
}

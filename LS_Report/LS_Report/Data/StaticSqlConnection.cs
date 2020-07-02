using LS_Report.Models;
using SQLite;
using System.IO;

namespace LS_Report.Data
{
    public static class StaticSqlConnection
    {
        private static SQLiteConnection _connection;

        public static SQLiteConnection GetConnection()
        {
            if (_connection == null)
            {
                var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "LS_Report_DB");
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }
                _connection = new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
                _connection.CreateTable<Stored_Data_Model>();
                _connection.CreateTable<NewContactToUpload_Model>();
                _connection.CreateTable<EditContactToUpload_Model>();
                _connection.CreateTable<LocationsBackGround_Model>();
                _connection.CreateTable<ReportToUpload_Model>();
                _connection.CreateTable<UnvailibilityToUpload_Model>();
                _connection.CreateTable<Mails_Model>();
                _connection.CreateTable<QuastionnairesToUpload_Model>();
            }
            return _connection;
        }
    }
}
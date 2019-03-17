using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.Storage;


namespace App5.SqlMigrationInitial
{
    class SqlMigration
    {
        public static void Initialize()
        {
            string path = ApplicationData.Current.LocalFolder.Path + "products.db";
            using (SqliteConnection db = new SqliteConnection($"Filename = products.db"))
            {
                String migrationQuery = @"CREATE TABLE Products (
                                       Id    INTEGER PRIMARY KEY AUTOINCREMENT,
                                       ProductName   TEXT,
	                                   ProductPrice  TEXT,
	                                   ProductBarcode TEXT)";
                db.Open();
                SqliteCommand tableCreationCmd = new SqliteCommand(migrationQuery, db);
                tableCreationCmd.ExecuteReader();
            }

        }

    }
}

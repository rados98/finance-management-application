using System.Data;
using System.Data.SQLite;

namespace Projekat
{
    class Konekcija
    {
        public static string konekcioniString = @"Data Source=D:\Home\Desktop\dvd_app\DB\dvd_app_db.db; Version=3;";


        public static DataSet executeQuery(string query, string srcTable)
        {
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(query, konekcioniString);
            DataSet dataSet = new DataSet();
            dataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            dataAdapter.Fill(dataSet, srcTable);
            return dataSet;
        }
    }
}

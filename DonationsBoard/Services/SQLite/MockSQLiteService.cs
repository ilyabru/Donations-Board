using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngelBoard.Models;
using SQLite;

namespace AngelBoard.Services
{
    public class MockSQLiteService : ISQLiteService
    {
        private SQLiteAsyncConnection conn;

        public MockSQLiteService()
        {
            conn = new SQLiteAsyncConnection("Data Source=ESDonationsTestDb.sqlite;");
            conn.CreateTablesAsync<Donor, Session>().Wait();
            conn.DeleteAllAsync<Donor>().Wait();
            conn.DeleteAllAsync<Session>().Wait();
        }

        public SQLiteAsyncConnection GetConnection()
        {
            return conn;
        }
    }
}

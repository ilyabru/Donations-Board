using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace AngelBoard.Models
{
    public class DataSource
    {
        private SQLiteAsyncConnection conn;

        public DataSource(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);

            conn.CreateTableAsync<Session>().Wait();
            //conn.InsertAsync(new Session { CreateDate = DateTime.Now }).Wait();
            conn.CreateTableAsync<Angel>().Wait();
        }

        public static ObservableCollection<Angel> GetAngels()
        {
            ObservableCollection<Angel> angels = new ObservableCollection<Angel>
            {
                new Angel { Name = "Joe", Amount = "$200.00", Location = "Barrie"},
                new Angel { Name = "Ilya", Amount = "12 x 10 month", Location = "Toronto"},
                new Angel { Name = "Joe2", Amount = "$200.00", Location = "Barrie"},
                new Angel { Name = "Joe3", Amount = "$200.00", Location = "Barrie"},
                new Angel { Name = "Joe4", Amount = "$200.00", Location = "Barrie"},
                new Angel { Name = "Joe5", Amount = "$200.00", Location = "Barrie"},
                new Angel { Name = "Joe5", Amount = "$200.00", Location = "Barrie"},
                new Angel { Name = "Joe5", Amount = "$200.00", Location = "Barrie"},
                new Angel { Name = "Joe5", Amount = "$200.00", Location = "Barrie"},
                new Angel { Name = "Joe5", Amount = "$200.00", Location = "Barrie"},
                new Angel { Name = "Joe5", Amount = "$200.00", Location = "Barrie"},
                new Angel { Name = "Joe5", Amount = "$200.00", Location = "Barrie"},
                new Angel { Name = "Joe5", Amount = "$200.00", Location = "Barrie"},
            };

            return angels;
        }

        public async Task AddAngel(string name, string location, string amount)
        {
            int result = 0;
            try
            {
                //basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                if (string.IsNullOrEmpty(location))
                    throw new Exception("Valid location required");

                if (string.IsNullOrEmpty(amount))
                    throw new Exception("Valid amount required");

                result = await conn.InsertAsync(new Angel { Name = name, Location = location, Amount = amount });
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed to add {0}. Error: {1}", name, ex.Message));
            }
        }

        public async Task<List<Angel>> GetAngelsAsync()
        {
            try
            {
                return await conn.Table<Angel>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
                throw;
            }
            //return new List<Angel>();
        }

        public async Task<List<Angel>> GetAngelsAsync(int sessionId)
        {
            try
            {
                return await conn.Table<Angel>().Where(a => a.SessionId == sessionId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
                throw;
            }
            //return new List<Angel>();
        }
    }
}

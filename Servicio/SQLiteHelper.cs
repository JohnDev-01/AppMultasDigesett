using AppMultasDigesett.Modelo;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMultasDigesett.Servicio
{
    public class SQLiteHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public SQLiteHelper()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "multas.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Multa>().Wait();
        }

        public Task<int> SaveMultaAsync(Multa multa)
        {
            return _database.InsertAsync(multa);
        }

        public Task<List<Multa>> GetMultasAsync()
        {
            return _database.Table<Multa>().ToListAsync();
        }
        public Task<int> DeleteAllMultasAsync()
        {
            return _database.DeleteAllAsync<Multa>();
        }
    }
}

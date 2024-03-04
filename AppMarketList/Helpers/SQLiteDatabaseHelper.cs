using AppMarketList.Models;
using SQLite;


namespace AppMarketList.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";

            return _conn.QueryAsync<Produto>(sql, p.Descricao, p.Quantidade, p.Preco, p.Id);
        }

        public Task<List<Produto>> SelectAll()
        {

            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<int> Delete(int id)
        {

            return _conn.Table<Produto>().DeleteAsync( i => i.Id == id);
        }

        public Task<List<Produto>> SearchProduct(string p)
        {
            string sql = $"SELECT * FROM Produto WHERE descricao LIKE '%{p}%'";

            return _conn.QueryAsync<Produto>(sql);
        }
    }
}

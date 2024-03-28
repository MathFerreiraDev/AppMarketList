using SQLite;

namespace AppMarketList.Models
{
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public double Quantidade { get; set; }
        public double Preco { get; set; }

        public string Total { get => "R$ " + Quantidade * Preco; }
    }
}

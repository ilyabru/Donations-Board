using SQLite;

namespace AngelBoard.Models
{
    [Table("sponsors")]
    public class Sponsor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }
    }
}

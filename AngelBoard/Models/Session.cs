using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AngelBoard.Models
{
    [Table("sessions")]
    public class Session
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }
    }
}

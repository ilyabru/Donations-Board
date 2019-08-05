using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationBoard.Models
{
    [Table("sessions")]
    public class Session
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public DateTime CreateDate { get; set; }

        public override string ToString()
        {
            return CreateDate.ToString();
        }
    }
}

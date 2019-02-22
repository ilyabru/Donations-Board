using AngelBoard.Helpers;
using SQLite;
using System;

namespace AngelBoard.Models
{
    [Table("angels")]
    public class Angel : ObservableObject, ICloneable
    {
        private string name;
        private string location;
        private string amount;
        private bool isViewed;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int SessionId { get; set; }

        [MaxLength(300)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(ref name, value);
        }

        [MaxLength(300)]
        public string Location
        {
            get => location;
            set => SetPropertyValue(ref location, value);
        }

        [MaxLength(300)]
        public string Amount
        {
            get => amount;
            set => SetPropertyValue(ref amount, value);
        }

        public bool IsViewed
        {
            get => isViewed;
            set => SetPropertyValue(ref isViewed, value);
        }

        public Angel()
            : this(string.Empty, string.Empty, string.Empty, false)
        {

        }

        public Angel(string name, string location, string amount, bool isViewed)
        {
            Name = name;
            Location = location;
            Amount = amount;
            IsViewed = isViewed;
        }

        public object Clone()
        {
            return (Angel)MemberwiseClone();
        }
    }
}

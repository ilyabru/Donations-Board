using AngelBoard.Helpers;
using SQLite;
using System;

namespace AngelBoard.Models
{
    [Table("donors")]
    public class Donor : ObservableObject
    {
        private string name;
        private string location;
        private decimal amount;
        private bool isMonthly;
        private bool isViewed;
        private DateTime createdDate = DateTime.Now;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public Guid SessionId { get; set; }

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

        public decimal Amount
        {
            get => amount;
            set => SetPropertyValue(ref amount, value);
        }

        public bool IsMonthly
        {
            get => isMonthly;
            set => SetPropertyValue(ref isMonthly, value);
        }

        public bool IsViewed
        {
            get => isViewed;
            set => SetPropertyValue(ref isViewed, value);
        }

        public DateTime CreatedDate
        {
            get => createdDate;
            set => SetPropertyValue(ref createdDate, value);
        }

        [Ignore]
        public string DisplayAmount => isMonthly ? $"12 x {(amount / 12):C0} = {amount:C0} MONTHLY" : $"{amount:C0}";

        public void Merge(Donor source)
        {
            if (source != null)
            {
                Id = source.Id;
                SessionId = source.SessionId;
                Name = source.Name;
                Location = source.Location;
                Amount = source.Amount;
                IsMonthly = source.IsMonthly;
                IsViewed = source.IsViewed;
                CreatedDate = source.CreatedDate;
            }
        }
    }
}

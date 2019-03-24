﻿using AngelBoard.Helpers;
using SQLite;
using System;

namespace AngelBoard.Models
{
    [Table("angels")]
    public class Angel : ObservableObject
    {
        private string name;
        private string location;
        private decimal amount;
        private bool isMonthly;
        private bool isViewed;

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

        [Ignore]
        public string DisplayAmount => isMonthly ? $"12 x {(amount / 12):C2} = {amount:C2} MONTHLY" : $"{amount:C2}";

        public void Merge(Angel source)
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
            }
        }
    }
}

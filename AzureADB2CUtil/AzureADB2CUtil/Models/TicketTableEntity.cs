﻿using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDay.WebApp.Database.Entities.Table
{
    [Flags]
    public enum TicketType
    {
        None = 0,
        Regular = 2,
        Workshop = 4
    }
    
    public sealed class TicketTableEntity : TableEntity
    {
        [IgnoreProperty]
        public string AttendeeId
        {
            get => RowKey;
            set => RowKey = value;
        }

        public bool IsPayed { get; set; }

        public string CouponCode { get; set; }

        [IgnoreProperty]
        public TicketType TicketType
        {
            get
            {
                TicketType val;
                return System.Enum.TryParse(PartitionKey, true, out val) ?
                    val :
                    TicketType.None;
            }
            set => PartitionKey = value.ToString();
        }

        public int? WorkshopId { get; set; }

        public double Price { get; set; }

        public string PaymentType { get; set; }

        public TicketTableEntity()
        {
        }
    }
}

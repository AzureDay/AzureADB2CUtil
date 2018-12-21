using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AADB2C.GraphService;
using AzureADB2CUtil.Models;
using AzureDay.WebApp.Database.Entities.Table;
using CsvHelper;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureADB2CUtil
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tenantName = "";
            var applicationId = "";
            var clientSecret = "";
            
            var storageName = "";
            var storagekey = "";
            
            var b2cGraphClient = new B2CGraphClient(tenantName, applicationId, clientSecret);

            var response = await b2cGraphClient.GetAllUsersAsync();

            var accounts = GraphAccounts.Parse(response);

            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(storageName, storagekey), true);

            //Client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Table
            CloudTable table = tableClient.GetTableReference("Ticket");
            
            TableQuery<TicketTableEntity> query = new TableQuery<TicketTableEntity>();

            List<TicketTableEntity> results = new List<TicketTableEntity>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<TicketTableEntity> queryResults =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            using (var textWriter = new StreamWriter("accounts.csv"))
            {
                var csv = new CsvWriter(textWriter);

                csv.WriteHeader<CsvItem>();
                csv.NextRecord();

                foreach (var account in accounts.value)
                {
                    var tickets = results.Where(x => x.AttendeeId == account.id);

                    if (tickets.Count() == 0)
                    {
                        var item = new CsvItem
                        {
                            Id = account.id,
                            Email = account.otherMails.FirstOrDefault(),
                            FirstName = account.givenName,
                            LastName = account.surname,
                        };
                        csv.WriteRecord(item);
                        csv.NextRecord();
                    }

                    foreach (var ticket in tickets)
                    {
                        var item = new CsvItem
                        {
                            Id = account.id,
                            Email = account.otherMails.FirstOrDefault(),
                            FirstName = account.givenName,
                            LastName = account.surname,
                            TicketType = ticket.TicketType,
                            CouponCode = ticket.CouponCode,
                            IsPayed = ticket.IsPayed,
                            PaymentType = ticket.PaymentType,
                            Price = ticket.Price,
                            WorkshopId = ticket.WorkshopId,
                            City = account.city,
                            Company = account.extension_d6245cc8578e4908b91662ccd12132e2_Company,
                            JobTitle = account.jobTitle
                        };
                        
                        csv.WriteRecord(item);
                        csv.NextRecord();
                    }

                }
                csv.Flush();
                csv.Dispose();
            }

        }
    }
}

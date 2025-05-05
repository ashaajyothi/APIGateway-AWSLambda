using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace OrderManagement
{
    [DynamoDBTable("order")]
    public class Order
    {
        [DynamoDBHashKey("id")] // Partition key
        public int OrderId { get; set; }
        [DynamoDBProperty("customer_name")]
        public string CustomerName { get; set; }
        [DynamoDBProperty("description")]
        public string Description { get; set; }
    }
}

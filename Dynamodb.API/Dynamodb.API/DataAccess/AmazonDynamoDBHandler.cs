using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Dynamodb.API.DataAccess
{
    public class AmazonDynamoDBHandler
    {
        public static AmazonDynamoDBClient GetClient()
        {
            AmazonDynamoDBConfig config = new AmazonDynamoDBConfig();
            config.ServiceURL = ConfigurationManager.AppSettings["AWSServiceURL"];

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(config);

            return client;
        }
    }
}
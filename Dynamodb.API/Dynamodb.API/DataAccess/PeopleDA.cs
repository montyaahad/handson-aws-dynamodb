using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace Dynamodb.API.DataAccess
{
    public class PeopleDA
    {
        private static readonly string TableName = "People";
        public static string CreatPeopleTable()
        {
            using (AmazonDynamoDBClient client = AmazonDynamoDBHandler.GetClient())
            {
                CreateTableRequest createTableRequest = new CreateTableRequest();
                createTableRequest.TableName = TableName;
                createTableRequest.ProvisionedThroughput = new ProvisionedThroughput() { ReadCapacityUnits = 1, WriteCapacityUnits = 1 };
                createTableRequest.KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement()
                    {
                        AttributeName = "Name"
                        , KeyType = KeyType.HASH
                    },
                    new KeySchemaElement()
                    {
                        AttributeName = "Birthdate"
                        , KeyType = KeyType.RANGE
                    }
                };
                createTableRequest.AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition(){AttributeName = "Name", AttributeType = ScalarAttributeType.S}
                    , new AttributeDefinition(){AttributeName = "Birthdate", AttributeType = ScalarAttributeType.S}
                };
                CreateTableResponse createTableResponse = client.CreateTable(createTableRequest);

                TableDescription tableDescription = createTableResponse.TableDescription;

                String tableStatus = tableDescription.TableStatus.Value.ToLower();
                while (tableStatus != "active")
                {
                    Thread.Sleep(2000);
                    DescribeTableRequest describeTableRequest = new DescribeTableRequest(TableName);
                    DescribeTableResponse describeTableResponse = client.DescribeTable(describeTableRequest);
                    tableDescription = describeTableResponse.Table;
                    tableStatus = tableDescription.TableStatus.Value.ToLower();
                }
            }

            return "People table created";
        }

        public static string GetAllPeople()
        {
            string result = string.Empty;

            try
            {
                using (AmazonDynamoDBClient client = AmazonDynamoDBHandler.GetClient())
                {
                    // db access code
                    Table peopleTable = Table.LoadTable(client, TableName);

                    //get all records
                    ScanFilter scanFilter = new ScanFilter();
                    Search getAllItems = peopleTable.Scan(scanFilter);
                    List<Document> allItems = getAllItems.GetRemaining();

                    foreach (Document item in allItems)
                    {
                        foreach (string key in item.Keys)
                        {
                            DynamoDBEntry dbEntry = item[key];
                            string val = dbEntry.ToString();
                            if (key.ToLower() == "neighbours")
                            {
                                List<string> neighbours = dbEntry.AsListOfString();
                                StringBuilder valueBuilder = new StringBuilder();
                                foreach (string neighbour in neighbours)
                                {
                                    valueBuilder.Append(neighbour).Append(", ");
                                }
                                val = valueBuilder.ToString();
                            }

                            result += string.Format("Property: {0}, value: {1} \n", key, val);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // log exception
                //throw;
            }

            return result;
        }

        public static string InsertPeopleData()
        {
            using (AmazonDynamoDBClient client = AmazonDynamoDBHandler.GetClient())
            {
                Table peopleTable = Table.LoadTable(client, TableName);
                Document firstPerson = new Document();
                firstPerson["Name"] = "John2";
                firstPerson["Birthdate"] = new DateTime(1980, 06, 24);
                peopleTable.PutItem(firstPerson);
            }

            return "record inserted in People table";
        }
    }
}
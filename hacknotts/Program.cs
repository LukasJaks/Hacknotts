using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace hacknotts
{
    class Program
    {
        static void Main(string[] args)
        {
            //main heree
            Console.WriteLine("start____");
            //twilioSet();
            //connectDB();
            Task task = MainAsync();
            task.Wait();
            Console.WriteLine("done");
        }

        public static void twilioSet()
        {
            const string accountSid = "AC560021b214c33c95763bf9dcec0218d8";
            const string authToken = "91000834d15e73cdc35852b516f6cbfd";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
            body: "aaaaaaaaa, it might work",
            from: new Twilio.Types.PhoneNumber("+12562429687"),
            to: new Twilio.Types.PhoneNumber("+447308157502")
        );

            Console.WriteLine(message.Sid);
        }

        public static void connectDB()
        {
            // ...
            var client = new MongoClient(
                "mongodb+srv://lukas:lukas@cluster0-mk0rz.gcp.mongodb.net/test?retryWrites=true&w=majority"
            );
            var database = client.GetDatabase("bear_witness");
            Console.WriteLine(database);
        }

        public List<Data> timetable = new List<Data>();
        
        public void sortData(List<Data> data)
        {
            
        }

        public static List<Data> dataToSave = new List<Data>();

        public static async Task MainAsync()
        {
            dataToSave.Clear();
            Console.WriteLine("connecting");
            var client = new MongoClient(
                "mongodb+srv://lukas:lukas@cluster0-mk0rz.gcp.mongodb.net/test?retryWrites=true&w=majority"
                );

            IMongoDatabase db = client.GetDatabase("bear_witness");

            var collection = db.GetCollection<BsonDocument>("user_info");

            using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        try
                        {
                            Console.WriteLine(document);
                            Console.WriteLine(document);


                            Data account = JsonConvert.DeserializeObject<Data>(JsonConvert.SerializeObject(BsonTypeMapper.MapToDotNetValue(document)));

                            dataToSave.Add(account);
                        }
                        catch(Exception ex) { }
                    }

                    for(int i = 0; i < dataToSave.Count; i++)
                    {
                        Console.WriteLine(dataToSave[i].ToString());
                    }
                }
            }
        }


    }

    public class Data
    {
        public string name { get; set; }
        public string age { get; set; }
        public string phoneNum { get; set; }
        public List<string> sport {get;set;}
        public List<string> day { get; set; }
        public string time { get; set; }
    }
}
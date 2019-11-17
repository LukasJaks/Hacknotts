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
            //runDataServer();
            Task task = MainAsync();
            task.Wait();
            //task.Wait();
            Console.WriteLine("Waiting for event");
            
            //while(false)
            //{ /* non stoping app */}
        }

        public static void runDataServer()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(5);
            Console.WriteLine("counting");
            var timer = new System.Threading.Timer((e) =>
            {
                Task task = MainAsync();
                task.Wait();
                runDataServer();
            }, null, startTimeSpan, periodTimeSpan);
        }

        public static void twilioSet(string num)
        {
            const string accountSid = "AC560021b214c33c95763bf9dcec0218d8";
            const string authToken = "91000834d15e73cdc35852b516f6cbfd";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
            body: "aaaaaaaaa, go work out",
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
        
        public static void sortData(List<Data> data)
        {
            string time = DateTime.Now.ToString("h:mm tt");
            time = time.Replace(":", "");
            string[] days = { "Sun", "Mon", "Tue", "Wen", "Thu", "Fri", "Sat" };
            DateTime ClockInfoFromSystem = DateTime.Now;
            string currDay = days[(int)ClockInfoFromSystem.DayOfWeek];


            for (int i = 0; i < dataToSave.Count; i++)
            {
                for(int x = 0; x < dataToSave[i].day.Count; x++)
                {
                    if (currDay == dataToSave[i].day[x])
                        if (time == dataToSave[i].time[x])
                            twilioSet(dataToSave[i].phoneNum);
                }
            }
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
                            Data account = JsonConvert.DeserializeObject<Data>(JsonConvert.SerializeObject(BsonTypeMapper.MapToDotNetValue(document)));

                            dataToSave.Add(account);
                        }
                        catch(Exception ex) { }
                    }

                    sortData(dataToSave);
                }
            }
            System.Threading.Thread.Sleep(300000);
            Task task = MainAsync();
            task.Wait();
        }


    }

    public class Data
    {
        public string name { get; set; }
        public string age { get; set; }
        public string phoneNum { get; set; }
        public List<string> sport {get;set;}
        public List<string> day { get; set; }
        public List<string> time { get; set; }
    }
}
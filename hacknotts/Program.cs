using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace hacknotts
{
    class Program
    {
        static void Main(string[] args)
        {
            //main heree
            Console.WriteLine("start____");
            //twilioSet();
            connectDB();
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
            var database = client.GetDatabase("test");
        }

        public void sortData(List<Data> data)
        {
            /*
             *mon
             *tue
             *wen
             *thu
             *fri
             *sat
             *san
             * 
             */


            for(int i = 0; i < data.Count; i++)
            {

            }
        }
    }

    public class Data
    {
        public string id { get; set; }
        public string whatever { get; set; }
        public int whateverelse { get; set; }
    }
}

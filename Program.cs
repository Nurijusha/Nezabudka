using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Try
{
    public class Remind
    {
        public string Event { get; }
        public DateTime Date { get; }
        public string Message { get; }

        public Remind(string message)
        {
            var remind = SplitMessage(message);
            Event = remind.Item1;
            Date = remind.Item2;
            Message = message;
        }

        public static Tuple<string, DateTime> SplitMessage(string message)
        {
            if (message[message.Length - 1] == '.')
                message.Remove(message.Length - 1);
            var splitedMessade = message.Split(':')[1]
                .Trim()
                .Split('-')
                .Select(x => x.Trim())
                .ToArray();
            var splitedDate = splitedMessade[0].Split(' ', '.');
            var dateArray = splitedDate.Select(x => int.Parse(x)).ToArray();
            var date = new DateTime(dateArray[2], dateArray[1], dateArray[0], dateArray[3], dateArray[4], 0);
            return new Tuple<string, DateTime>(splitedMessade[1], date);
        }
    }

    class Program
    {
        public static List<Remind> AllReminds = new List<Remind>();
        static void Main(string[] args)
        {
            var remind = new Remind("Создать напоминание: 27.04.2019 19.00 - event");
            Console.WriteLine((remind.Date.CompareTo(DateTime.Now) < 0 ||
                remind.Date.Date == DateTime.Now.Date && (remind.Date.Hour < DateTime.Now.Hour ||
                    remind.Date.Hour == DateTime.Now.Hour && remind.Date.Minute <= DateTime.Now.Minute)));
        //    var tokenSource = new CancellationTokenSource();
        //    var token = tokenSource.Token;
        //    //var message = "Создать напоминание: 27.04.2019 15.26 - event";
        //    //var remind = new Remind(message);
        //    //Console.WriteLine(remind.Date.CompareTo(DateTime.Now));
        //    //AllReminds.Add(remind);
        //    //SendReminds(token);
        //    while (true)
        //    {
        //        Remind newRemind = null;
        //        try
        //        {
        //            newRemind = new Remind(Console.ReadLine());
        //        }
        //        catch
        //        {
        //            Console.WriteLine("Неверный формат");
        //        }
        //        finally
        //        {
        //            if (AllReminds.Count == 0)
        //            {
        //                lock (AllReminds)
        //                {
        //                    AllReminds.Add(newRemind);
        //                }
        //            }
        //            else
        //            {
        //                tokenSource.Cancel();
        //                lock (AllReminds)
        //                {
        //                    AllReminds.Add(newRemind);
        //                    AllReminds.OrderBy(x => x.Date);
        //                }
        //            }
        //            Task.Factory.StartNew(() => SendReminds(token));
        //        }
        //    }
        //}

        //public static void SendReminds(CancellationToken ct)
        //{
        //    while (AllReminds.Count != 0)
        //    {
        //        var interval = AllReminds[0].Date - DateTime.Now;
        //        Task.Delay(interval, ct)
        //            .ContinueWith(x => Console.WriteLine("ok"), ct)
        //            .Wait(ct);
        //        lock (AllReminds) { AllReminds.RemoveAt(0); }
        //    }
        }
    }
}

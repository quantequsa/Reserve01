using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve01
{
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //    }
    //}
    /*
    Below is a full example. 
    you can run the ReserverAHoliday both 
    Synchronously (bool r = ReserveAHolliday().Result;) 
    and Asynchronously (just call ReserveAHolliday();) 
    from MAIN (depends which line you comment). 
    and you can see the effect ("END" gets printed before / after the reservation is complete). 
    I prefer the await Task.WhenAll() methods, which is more readable. 
    also note that it's preferred to use await Task.Delay(100) instead of Thread.sleep inside GetNumber1.
    */
    class Program
    {
        static void Main(string[] args)//main cant' be async
        {
            Console.WriteLine("Wired Brain Coffee - Shop Info Tool!");
            Console.ReadLine();
            /*
            
            //int res = test().Result;//I must put await here
            Console.WriteLine("01M BEGIN");
            bool r = ReserveAHolliday().Result; //this will run Synchronously. .Result property of the task will cause your thread to block until the result is available might cause a deadlock 
            //var task = InputOutput(); BAD 03 task.GetAwaiter().GetResult()   BAD02 task.Wait  BAD01 var a = task.Result;
            Console.WriteLine("08M DONE OR NSYNC EXECUTING...");
            //OR what is the difference 
            //_ = ReserveAHolliday().Result; //this will run Synchronously because it uses .Result  this will run Synchronously.

            _ = ReserveAHolliday();  //same as ReserveAHolliday(); just syntax        //this option will run aync : you will see "END" printed before the reservation is complete.
            //_ will discard the returned Task without awaiting it's completion 
            //_ = awaitfunction(); syntactic sugar is all the same as without "_ =" would await the Task and then discard the result into unused useless local variables
            Console.WriteLine("09M DONE OR ASYNC EXECUTING...");
            Console.WriteLine("10M STILL EXECUTING BOTH");
            //Console.WriteLine("CASE0 NSYNC Both Hotel were done" + ReserveAHolliday().Result);
            Console.WriteLine("11M CASE1 NSYNC Both Hotel were done and it is " + r);
            Console.WriteLine("12M FINALE");
            Console.ReadLine();
            */
        }
        /*
    Both NSYNC and ASYNC
01M BEGIN
    05-06A SEQ1 Reserve A Hotel started for 02A FirstHotel
    05-06A SEQ1 Reserve A Hotel started for 03A SecondHotel
    04A WORKING? B4 when all
    05-06B SEQ2 Reserve A Hotel done for 03A SecondHotel
    05-06B SEQ2 Reserve A Hotel done for 02A FirstHotel
    07A DONE A7 when all
    07B DONE A7 return all
08M DONE OR NSYNC EXECUTING... WAITS 

    05-06A SEQ1 Reserve A Hotel started for 02A FirstHotel
    05-06A SEQ1 Reserve A Hotel started for 03A SecondHotel
    04A WORKING? B4 when all
09M DONE OR ASYNC EXECUTING...  does not wait!
10M STILL EXECUTING BOTH
11M CASE1 NSYNC Both Hotel were done and it is True
12M FINALE
    05-06B SEQ2 Reserve A Hotel done for 03A SecondHotel
    05-06B SEQ2 Reserve A Hotel done for 02A FirstHotel
    07A DONE A7 when all
    07B DONE A7 return all


        NSYNC all the way
01M BEGIN
    05-06A SEQ1 Reserve A Hotel started for 02A FirstHotel
    05-06A SEQ1 Reserve A Hotel started for 03A SecondHotel
    04A WORKING? B4 when all
    05-06B SEQ2 Reserve A Hotel done for 03A SecondHotel
    05-06B SEQ2 Reserve A Hotel done for 02A FirstHotel
    07A DONE A7 when all
    07B DONE A7 return all
08M DONE OR NSYNC EXECUTING...

    05-06A SEQ1 Reserve A Hotel started for 02A FirstHotel
    05-06A SEQ1 Reserve A Hotel started for 03A SecondHotel
    04A WORKING? B4 when all
    05-06B SEQ2 Reserve A Hotel done for 03A SecondHotel
    05-06B SEQ2 Reserve A Hotel done for 02A FirstHotel
    07A DONE A7 when all
    07B DONE A7 return all
09M DONE OR ASYNC EXECUTING... NSYNC also 
10M STILL EXECUTING BOTH
11M CASE1 NSYNC Both Hotel were done and it is True
12M FINALE
        */
        /*
SYNC
Reserve A Hotel started for FirstHotel
Reserve A Hotel started for SecondHotel
Reserve A Hotel done for SecondHotel
Reserve A Hotel done for FirstHotel
END
Both Hotel were done and it is True
FINALE


        OR
ASYNC
Reserve A Hotel started for FirstHotel
Reserve A Hotel started for SecondHotel
END
FINALE
Reserve A Hotel done for SecondHotel
Reserve A Hotel done for FirstHotel
        */
        //public async static Task<int> test()
        //{ //why can't I make it just: public int test()??
        //  //becuase you cannot use await in synchronous methods. 
        //    int a1, a2, a3, a4;

        //    a1 = await GetNumber1();
        //    a2 = await GetNumber1();
        //    a3 = await GetNumber1();

        //    a4 = a1 + a2 + a3;
        //    return a4;
        //}

        //public static async Task<int> GetNumber1()
        //{
        //    //await Task.Run(() =>
        //    //    {
        //    for (int i = 0; i < 10; i++)
        //    {
        //        Console.WriteLine("GetNumber1");
        //        await Task.Delay(100); // from what I read using Task.Delay is preferred to using  System.Threading.Thread.Sleep(100);
        //    }
        //    //    });
        //    return 1;
        //}
        //Send the name of the hotel and see which one takes longer both hotels will be processed
        public static async Task<bool> ReserveAHolliday()
        {
            //bool hotelOK = await ReserveAHotel();//HTTP async request
            //bool flightOK = await ReserveAHotel();////HTTP async request
            var t1 = ReserveAHotel("02A FirstHotel");   ///this is called first!  but finished second 
            var t2 = ReserveAHotel("03A SecondHotel");
            Console.WriteLine("04A WORKING? B4 when all");
            await Task.WhenAll(t1, t2);  //the task if for all to complete in force sequence even their is unfairness within
            Console.WriteLine("07A COMPLETED A7 when all");
            bool result = t1.Result && t1.Result;// hotelOK && flightOK;
            Console.WriteLine("07B DONE A7 return all");
            return result;  //this is just to show both were done
        }

        //Depending on the name of the hotel there is a wait but more for the first one even if is called first
        public static async Task<bool> ReserveAHotel(string name)  //internal messaging displays, same message but different timing 
        {
            Console.WriteLine("05-06A SEQ1 Reserve A Hotel started for " + name);
            await Task.Delay(3000);
            if (name == "FirstHotel")
                await Task.Delay(5000); //delaying first hotel on purpose...
            Console.WriteLine("05-06B SEQ2 Reserve A Hotel done for " + name);
            return true;
        }
    }
}
/*
Reserve A Hotel started for FirstHotel
Reserve A Hotel started for SecondHotel
Reserve A Hotel done for SecondHotel
Reserve A Hotel done for FirstHotel
END
*/
/*
 * 
 public async Task<bool> ReserveAHoliday()
{
        //Initialize and start this task
        Task<bool> hotelTask = ReserveAHotel();//HTTP async request
        //Initialize and start this task
        Task<bool> flightTask = ReserveFlight();////HTTP async request
        //Await until hotel task is done, and get the bool
        bool hotelOK = await hotelTask;
        //Await until flight task is done (This might have already finished while the hotel was grabbing, and get the bool
        bool flightOK = await flightTask;
        bool result = hotelOK && flightOK;
        //Return the final result
        return result;
}
 */

/*
Original problem to be fixed:
class Program
{
    static void Main(string[] args)//main cant' be async
    {
        int res = test();//I must put await here

        Console.WriteLine("END");
    }

    public async static Task<int> test()
    { //why can't I make it just: public int test()??
        int a1, a2, a3, a4;

        a1 = await GetNumber1();
        a2 = await GetNumber2();
        a3 = await GetNumber3();

        a4 = a1 + a2 + a3;
        return a4;
    }

    public static async Task<int> GetNumber1()
    {
        await Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("GetNumber1");
                    System.Threading.Thread.Sleep(100);
                }
            });
        return 1;
    }
 */

/*
Check this solution
class Program
{
    static void Main(string[] args)
    {
        var task = test();
        task.Wait(); //this stops async behaviour
        int result = task.Result;// get return value form method "test"
        Console.WriteLine("RESULT IS = " + result);
    }

    public async static Task<int> test()
    {
        int a1, a2, a3, a4;

        //run all tasks
        //all tasks are doing their job "at the same time"
        var taskA1 = GetNumber1();
        var taskA2 = GetNumber2();
        var taskA3 = GetNumber3();

        //wait for task results
        //here I am collecting results from all tasks
        a1 = await taskA1;
        a2 = await taskA2;
        a3 = await taskA3;

        //get value from all task results
        a4 = a1 + a2 + a3;
        return a4;
    }

    public static async Task<int> GetNumber1()
    {
        await Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("GetNumber1");
                    System.Threading.Thread.Sleep(100);
                }
            });
        return 1;
    }
 */ 
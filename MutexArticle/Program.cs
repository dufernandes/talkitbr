using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MutexArticle
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    Console.WriteLine("There must be an arument for executing this application:");
                    Console.WriteLine("NoLocking - execute application with no concurrency treatment.");
                    Console.WriteLine("WithLocking - execute application with lock construct. It implements the Monitor design pattern.");
                    Console.WriteLine("WithMutex - execute application with Mutex class. It implements the Monitor design pattern.");
                }
                else if (args[0].Equals("NoLocking"))
                {

                    BankAccountSimple accountSimple = new BankAccountSimple(100);

                    Thread threadAdd_50 = new Thread(() => accountSimple.AddMoney(50));
                    Thread threadWithdraw_110 = new Thread(() => accountSimple.WithdrawMoney(110));

                    threadAdd_50.Start();
                    threadWithdraw_110.Start();
                } else if (args[0].Equals("WithLocking"))
                {

                    BankAccountLock accountLock = new BankAccountLock(100);

                    Thread threadAdd_50 = new Thread(() => accountLock.AddMoney(50));
                    Thread threadWithdraw_110 = new Thread(() => accountLock.WithdrawMoney(110));

                    threadAdd_50.Start();
                    threadWithdraw_110.Start();
                }
                else if (args[0].Equals("WithMutex"))
                {

                    BankAccountMutex accountMutex = new BankAccountMutex(100);

                    Thread threadAdd_50 = new Thread(() => accountMutex.AddMoney(50));
                    Thread threadWithdraw_110 = new Thread(() => accountMutex.WithdrawMoney(110));

                    threadAdd_50.Start();
                    threadWithdraw_110.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.ReadLine();
        }
    }
}

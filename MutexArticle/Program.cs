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
                    throw new ArgumentException("There must be an arument for executing this application.");
                }

                if (args[0].Equals("NoLocking"))
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

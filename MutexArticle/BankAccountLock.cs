using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MutexArticle
{
    class BankAccountLock
    {
        private double bankMoney = 0d;

        public BankAccountLock(double money)
        {
            LogConsole("Setting initial amount of money: " + money);

            if (money < 0)
            {
                LogConsole("The entered money quantity cannot be negative. Money: " + money);
                throw new ArgumentException(GetMessageWithTreadId("The entered money quantity cannot be negative. Money: " + money));
            }

            this.bankMoney = money;
        }

        public void AddMoney(double money) 
        {
            lock (this)
            {
                LogConsole("Money to be added: " + money);

                if (money < 0)
                {
                    LogConsole("The entered money quantity cannot be negative. Money: " + money);
                    throw new ArgumentException(GetMessageWithTreadId("The entered money quantity cannot be negative. Money: " + money));
                }

                this.bankMoney = this.bankMoney + money;


                if (this.bankMoney < 0)
                {
                    LogConsole("The money quantity cannot be negative. Money: " + this.bankMoney);
                    throw new ArgumentException(GetMessageWithTreadId("The money quantity cannot be negative. Money: " + this.bankMoney));
                }

                Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": Total amount of money: " + this.bankMoney);
            }
        }

        public void WithdrawMoney(double money)
        {
            lock (this)
            {
                if (money < 0)
                {
                    LogConsole("The entered money quantity cannot be negative. Money: " + money);
                    throw new ArgumentException(GetMessageWithTreadId("The entered money quantity cannot be negative. Money: " + money));
                }

                Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": Money to be withdrawed: " + money);

                this.bankMoney = this.bankMoney - money;

                if (this.bankMoney < 0)
                {
                    LogConsole("The money quantity cannot be negative. Money: " + this.bankMoney);
                    throw new ArgumentException(GetMessageWithTreadId("The money quantity cannot be negative. Money: " + this.bankMoney));
                }

                Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": Total amount of money: " + this.bankMoney);
            }
        }

        public double GetBankStatement()
        {
            LogConsole("Bank Statement: Total amount of money: " + this.bankMoney);
            return bankMoney;
        }

        private String getCurrenThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId.ToString();
        }

        private void LogConsole(String message)
        {
            Console.WriteLine(GetMessageWithTreadId(message));
        }

        private String GetMessageWithTreadId(String message)
        {
            return "Thread ID: " + getCurrenThreadId() + ": " + message;
        }
    }
}

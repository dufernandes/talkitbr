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
            Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": Setting initial amount of money: " + money);

            if (money < 0)
            {
                Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": The entered money quantity cannot be negative. Money: " + money);
                throw new ArgumentException("Thread ID: " + getCurrenThreadId() + ": The entered money quantity cannot be negative. Money: " + money);
            }

            this.bankMoney = money;
        }

        public void AddMoney(double money) 
        {
            lock (this)
            {
                Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": Money to be added: " + money);

                if (money < 0)
                {
                    Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": The entered money quantity cannot be negative. Money: " + money);
                    throw new ArgumentException("Thread ID: " + getCurrenThreadId() + ": The entered money quantity cannot be negative. Money: " + money);
                }

                this.bankMoney = this.bankMoney + money;


                if (this.bankMoney < 0)
                {
                    Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": The money quantity cannot be negative. Money: " + this.bankMoney);
                    throw new ArgumentException("Thread ID: " + getCurrenThreadId() + ": The money quantity cannot be negative. Money: " + this.bankMoney);
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
                    Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": The entered money quantity cannot be negative. Money: " + money);
                    throw new ArgumentException("Thread ID: " + getCurrenThreadId() + ": The entered money quantity cannot be negative. Money: " + money);
                }

                Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": Money to be withdrawed: " + money);

                this.bankMoney = this.bankMoney - money;

                if (this.bankMoney < 0)
                {
                    Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": The money quantity cannot be negative. Money: " + this.bankMoney);
                    throw new ArgumentException("Thread ID: " + getCurrenThreadId() + ": The money quantity cannot be negative. Money: " + this.bankMoney);
                }

                Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": Total amount of money: " + this.bankMoney);
            }
        }

        public double GetBankStatement()
        {
            Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": Bank Statement: Total amount of money: " + this.bankMoney);
            return bankMoney;
        }

        private String getCurrenThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId.ToString();
        }
    }
}

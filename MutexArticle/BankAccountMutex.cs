﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;

namespace MutexArticle
{
    class BankAccountMutex
    {
        private double bankMoney = 0d;

        Mutex mutex = null;

        private const int MUTEX_WAIT_TIMEOUT = 5000;

        // Note: configuration based on stackoverflow answer: http://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c
        public BankAccountMutex(double money)
        {
            // get application GUID as defined in AssemblyInfo.cs
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();

            // unique id for global mutex - Global prefix means it is global to the machine
            string mutexId = string.Format("Global\\{{{0}}}", appGuid);

            // Need a place to store a return value in Mutex() constructor call
            bool createdNew;

            // set up security for multi-user usage
            // work also on localized systems (don't use just "Everyone") 
            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);

            mutex = new Mutex(false, mutexId, out createdNew, securitySettings);

            Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": Setting initial amount of money: " + money);

            if (money < 0)
            {
                Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": The entered money quantity cannot be negative. Money: " + money);
                throw new ArgumentException("Thread ID: " + getCurrenThreadId() + ": The entered money quantity cannot be negative. Money: " + money);
            }

            this.bankMoney = money;
        }

        public void AddMoney(double money = 0) 
        {
            bool hasHandle = mutex.WaitOne(MUTEX_WAIT_TIMEOUT, false);

            if (!hasHandle)
            {
                throw new TimeoutException("method void AddMoney(double): Timeout due to look waiting.");
            }

            try
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
                    Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": The money quantity cannot be negative. Money: " + money);
                    throw new ArgumentException("Thread ID: " + getCurrenThreadId() + ": The money quantity cannot be negative. Money: " + money);
                }

                Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": Total amount of money: " + this.bankMoney);
            }
            finally
            {
                if (hasHandle)
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        public void WithdrawMoney(double money = 0)
        {
            bool hasHandle = mutex.WaitOne(MUTEX_WAIT_TIMEOUT, false);

            if (!hasHandle)
            {
                throw new TimeoutException("method void AddMoney(double): Timeout due to look waiting.");
            }

            try
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
                    Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": The money quantity cannot be negative. Money: " + money);
                    throw new ArgumentException("Thread ID: " + getCurrenThreadId() + ": The money quantity cannot be negative. Money: " + money);
                }

                Console.WriteLine("Thread ID: " + getCurrenThreadId() + ": Total amount of money: " + this.bankMoney);
            }
            finally
            {
                if (hasHandle)
                {
                    mutex.ReleaseMutex();
                }
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

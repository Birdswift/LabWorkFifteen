using System;
using System.Threading;

namespace task3
{
    public class SingleRandomizer
    {
        private static SingleRandomizer instance; //хранит единственный объект класса
        private static readonly object lockObject = new object(); //блокировка доступа 

        private Random random;

        private SingleRandomizer()
        {
            random = new Random();
        }

        public static SingleRandomizer Instance //глобальный доступ к единственному экземпляру класса
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject) //блок потока
                    {
                        if (instance == null)
                        {
                            instance = new SingleRandomizer();
                        }
                    }
                }
                return instance;
            }
        }

        public int Next()
        {
            lock (lockObject)
            {
                return random.Next();
            }
        }
    }

    public class Program
    {
        public static void Main()
        {
            int numThreads = 5;

            Thread[] threads = new Thread[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                threads[i] = new Thread(() =>
                {
                    SingleRandomizer randomizer = SingleRandomizer.Instance;
                    int randomNumber = randomizer.Next();
                    Console.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId}, Random Number: {randomNumber}");
                });
                threads[i].Start();
            }
        }
    }
}
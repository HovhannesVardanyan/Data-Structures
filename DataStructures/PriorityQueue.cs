using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo
{
    /// <summary>
    /// 
    /// Implementation of Priority Queue with Heaps
    /// 
    /// </summary>

    class PriorityQueue<T> where T : IExecutable
    {
        private Heap<T> register;
        public bool IsEmpty => register.Count == 0;
        public PriorityQueue(Func<T,T,bool> condition)
        {
            register = new Heap<T>(condition);
        }
        public void AddTask(T task)
        {
            register.Insert(task);
        }
        public void DoTask()
        {
            T task = register.Extract();
            task.Execute();
        }
        public override string ToString()
        {
            return register.ToString();
        }
    }
    interface IExecutable
    {
        void Execute();
    }
    class Task : IExecutable
    {
        public string Problem;
        public int Priority;
        public Task(string prob, int pr)
        {
            Problem = prob;
            Priority = pr;
        }
        public override string ToString()
        {
            return $"{Priority} -> {Problem}";
        }
        void IExecutable.Execute()
        {
            Console.WriteLine("Dealing with problem  " + Problem);
        }
    }
}

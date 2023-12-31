using System;
using System.Collections.Generic;
namespace Pass3
{
    public class QueueZ
    {
        List<Zombie> queue = new List<Zombie>();

        public QueueZ()
        {

        }

        public void Enqueue (Zombie newZombie)
        {
            queue.Add(newZombie);
        }

        public Zombie Dequeue()
        {
            Zombie result = null;

            if (queue.Count > 0)
            {
                result = queue[0];
                queue.RemoveAt(0);
            }

            return result;
        }

        public Zombie Peek()
        {
            Zombie result = null;

            if (queue.Count > 0)
            {
                result = queue[0];
            }

            return result;
        }

        public int Size()
        {
            return queue.Count;
        }

        public bool IsEmpty()
        {
            return queue.Count == 0;
        }
    }
}

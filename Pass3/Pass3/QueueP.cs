using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Helper;
using Animation2D;
using System.IO;

namespace Pass3
{
    public class QueueP
    {
        List<Projectile> queue = new List<Projectile>();

        public QueueP()
        {

        }

        public void Enqueue(Projectile newProjectile)
        {
            queue.Add(newProjectile);
        }

        public Projectile Dequeue()
        {
            Projectile result = null;

            if (queue.Count > 0)
            {
                result = queue[0];
                queue.RemoveAt(0);
            }

            return result;
        }

        public Projectile Peek()
        {
            Projectile result = null;

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
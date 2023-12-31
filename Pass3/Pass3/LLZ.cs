using System;
namespace Pass3
{
    public class LLZ
    {
        private Zombie head;

        private int count;

        public LLZ()
        {
            count = 0;
        }

        public int GetCount()
        {
            return count;
        }

        public void AddToTail(Zombie newZombie)
        {
            if (count == 0)
            {
                head = newZombie;
            }
            else
            {
                Zombie curZombie = head;

                while (curZombie.GetNext() != null)
                {
                    curZombie = curZombie.GetNext();
                }

                curZombie.SetNext(newZombie);
            }

            count++;
        }

        public void DeleteHead()
        {
            head = head.GetNext();
            count--;
        }

        public void Delete(int position)
        {
            if (position == 0)
            {
                DeleteHead();
            }
            else if (position < count)
            {
                Zombie curZombie = head;
                for (int i = 0; i < position - 1; i++)
                {
                    curZombie = curZombie.GetNext();
                }

                if (position == count - 1)
                {
                    curZombie.SetNext(null);
                }
                else
                {
                    curZombie.SetNext(curZombie.GetNext().GetNext());
                }

                count--;
            }
        }

        public Zombie GetZombie(int position)
        {
            Zombie current = head;
            int count = 0;

            while (current != null)
            {
                if (count == position)
                {
                    return current;
                }
                
                current = current.GetNext();
                count++;
            }

             return null;
        }
    }
}
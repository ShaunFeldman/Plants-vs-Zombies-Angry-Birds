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
    public class Zombie
    {
        private int speed;
        private int health;
        private int startingHealth;

        private Color zombieCol;

        private int direction;

        private Animation zombie;

        private List<int> turningPoints = new List<int>();

        private int pointX;
        private int pointY;

        private int cubeX = 55;
        private int cubeY = 85;

        private float zombieSize;

        private int counter = 0;

        private int difficulty;

        private Zombie next;

        public Zombie(int difficulty, int direction, Texture2D zombieImg, Vector2 zombiePos, List<int> turningPoints, int wave)
        {

            this.difficulty = difficulty;
            speed = difficulty - 1;
            health = difficulty;
            startingHealth = difficulty;
            this.direction = direction;
            this.turningPoints = turningPoints;
            this.pointX = (int)zombiePos.X;
            this.pointY = (int)zombiePos.Y;

            if (wave <= 5)
            {
                zombieSize = 0.7f - (0.05f * wave);
            }
            else
            {
                zombieSize = 0.4f;
            }
            zombie = new Animation(zombieImg, 4, 1, 4, 0, 0, Animation.ANIMATE_FOREVER, 3, zombiePos, zombieSize, true);
        }

        public int GetDifficulty()
        {
            return difficulty;
        }
        public int GetSpeed()
        {
            return speed;
        }

        public int GetHealth()
        {
            return health;
        }

        public void Hit(int damage)
        {
            health -= damage;
        }

        public int GetDirection()
        {
            return direction;
        }

        public void SetDirection(int direction)
        {
            this.direction = direction;
        }

        public void SetSpeed(int speed)
        {
            this.speed = speed;
        }

        public void Walk(bool lose)
        {
            if (lose == true)
            {
                speed = 1;
            }

            if (turningPoints[counter] == 0)
            {
                zombie.destRec.X += speed * direction;

                if (pointX - zombie.destRec.X >= cubeX)
                {
                    pointX = zombie.destRec.X;
                    if (counter < turningPoints.Count - 1)
                    {
                        counter++;
                    }
                }
            }
            else
            {
                zombie.destRec.Y += turningPoints[counter] * speed;

                if (pointY - zombie.destRec.Y >= cubeY || zombie.destRec.Y - pointY >= cubeY)
                {
                    pointY = zombie.destRec.Y;
                    if (counter < turningPoints.Count -1)
                    {
                        counter++;
                    }
                }
            }

        }

        public void UpdateZombie(GameTime gameTime)
        {
            zombie.Update(gameTime);  
        }

        public void DrawZombie(SpriteBatch _spriteBatch, bool xtraCol)
        {
            if (xtraCol == true)
            {
                zombieCol = Color.Gold;
            }
            else
            {
                zombieCol = Color.White;
            }

            if (startingHealth - health == 0)
            {
                zombie.Draw(_spriteBatch, zombieCol, Animation.FLIP_NONE);
            }
            else if (health == 1)
            {
                zombie.Draw(_spriteBatch, Color.Red, Animation.FLIP_NONE);
            }
            else if (startingHealth - health == 3)
            {
                zombie.Draw(_spriteBatch, Color.PaleVioletRed, Animation.FLIP_NONE);
            }
            else
            {
                zombie.Draw(_spriteBatch, Color.MediumVioletRed, Animation.FLIP_NONE);
            }
        }

        public void SetNext(Zombie zombie)
        {
            next = zombie;
        }

        public Zombie GetNext()
        {
            return next;
        }

        public Rectangle GetPos()
        {
            return zombie.destRec;
        }

    }
}

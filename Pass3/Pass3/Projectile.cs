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
    public class Projectile
    {
        private Texture2D img;
        private Texture2D punchImg;

        private Rectangle imgRec;

        private int waveNum;

        private int maxSpeed;

        private int slingShotY = 395;
        private int slingShotX = 300;

        private double x;
        private double y;
        private double hypot;

        private double xTravel;

        private Animation punch;

        private int damage;

        private Vector2 speed = new Vector2(0, 0);
        private Vector2 projectilePos;
        private Vector2 punchPos = new Vector2(0, 0);

        private bool calculations = true;
        private bool shoot = false;
        private bool stickyPower = false;
        private bool almightyPunch;

        public Projectile(int waveNum, Texture2D imgBall, Texture2D imgArrow, Texture2D imgSword ,Texture2D aniPunch, Rectangle imgRec, bool almightyPunch, Texture2D punchImg)
        {
            this.waveNum = waveNum;

            this.almightyPunch = almightyPunch;

            this.punchImg = punchImg;

            if (almightyPunch != true)
            {
                if (waveNum <= 2)
                {
                    img = imgBall;
                    maxSpeed = 2;
                    damage = 2;
                }
                else if (waveNum <= 4)
                {
                    img = imgArrow;
                    maxSpeed = 3;
                    damage = 3;
                }
                else
                {
                    img = imgSword;
                    maxSpeed = 3;
                    damage = 5;
                }
            }
            else
            {
                img = aniPunch;
                maxSpeed = 4;
                damage = 5;
                punch = new Animation(aniPunch, 4, 1, 4, 0, 0, Animation.ANIMATE_FOREVER, 3, punchPos, 0.2f, true);
            }

            this.imgRec = imgRec;

            this.waveNum = waveNum;
        }
        
        public int GetDamage()
        {
            return damage;
        }
        public void DrawProjectile(SpriteBatch _spriteBatch)
        {
            if (almightyPunch == false)
            {
                _spriteBatch.Draw(img, imgRec, Color.White);
            }
            else
            {
                punch.destRec = imgRec;
                punch.Draw(_spriteBatch, Color.White, Animation.FLIP_NONE);
            }
        }

        public Texture2D GetImg()
        {
            return img;
        }

        public void UpdateAni(GameTime gameTime)
        {
            punch.Update(gameTime);
        }

        public Texture2D GetTexture2D()
        {
            if (almightyPunch == false)
            {
                return img;
            }
            else
            {
                return punchImg;
            }
        }

        public void SetSticky(bool tf)
        {
            stickyPower = tf;
        }
        public void SetXPos(double x)
        {
            imgRec.X = (int)x - (imgRec.Width / 2);
        }
        public void SetYPos(double y)
        {
            imgRec.Y = (int)y - (imgRec.Height / 2);
        }

        public void DrawTrajectory(SpriteBatch _spriteBatch)
        {
            x = slingShotX - imgRec.X;
            y = slingShotY - imgRec.Y;

            hypot = y / x;

            projectilePos.X = 0;
            projectilePos.Y = 0;

            xTravel = (x * 1400 / 300) - imgRec.X;

            while (true)
            {

            }
        }

        public void Shoot(GameTime gameTime)
        {
            if (calculations == true)
            {
                x = slingShotX - imgRec.X;
                y = slingShotY - imgRec.Y;

                hypot = y / x;

                projectilePos.X = 0;
                projectilePos.Y = 0;

                xTravel = (x * 1400 / 300) - imgRec.X;

                calculations = false;
            }

            speed.X = maxSpeed;
            speed.Y = maxSpeed;

            projectilePos.X += speed.X;

            if (imgRec.X < (xTravel / 2))
            {
                projectilePos.Y += (float)(hypot * speed.Y);
            }
            else
            {
                projectilePos.Y = 0;
            }

            if (imgRec.X <= xTravel)
            {
                imgRec.Y += (int)projectilePos.Y;
            }

            if (imgRec.X <= xTravel + 500)
            {
                imgRec.X += (int)projectilePos.X;
            }
            else
            {
                if (stickyPower != true)
                {
                    shoot = false;
                }
            }
        }

        public double GetXDistance()
        {
            return xTravel;
        }

        public Rectangle GetPos()
        {
            return imgRec;
        }

        public void SetCalc(bool TF)
        {
            calculations = TF;
        }

        public bool GetShoot()
        {
            return shoot;
        }

        public void SetShoot(bool TF)
        {
            shoot = TF;
        }
    }
}

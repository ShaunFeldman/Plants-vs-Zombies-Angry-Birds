using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Helper;
using Animation2D;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

//Include description of how i included course components. A few bullet points for each component

namespace Pass3
{

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Game States - Add/Remove/Modify as needed
        //These are the most common game states, but modify as needed
        //You will ALSO need to modify the two switch statements in Update and Draw
        const int MENU = 0;
        const int STARTINGINTRO = 1;
        const int INSTRUCTIONS = 2;
        const int GAMEPLAY = 3;
        const int PAUSE = 4;
        const int INVENTORY = 5;
        const int SHOP = 6;

        const int EASY = 2;
        const int MEDIUM = 3;
        const int HARD = 4;
        const int STARTINGDIRECTION = -1;

        const int UP = -1;
        const int DOWN = 1;
        const int STRAIGHT = 0;

        Song menuMusic;
        Song gameMusic;
        Song shopMusic;

        SoundEffect purchaseSound;
        SoundEffect zombieGroan;
        SoundEffect projectileShoot;
        SoundEffect projectileHit;
        SoundEffect activatePowerUp;

        int sticky = 1;
        int reload = 2;
        int punch = 3;

        int coins;
        int lifeTimeCoins;

        List<int> turningPoint1 = new List<int>();
        List<int> turningPoint2 = new List<int>();
        List<int> turningPoint3 = new List<int>();
        List<int> turningPoint4 = new List<int>();
        List<int> turningPoint5 = new List<int>(); //add more turning points, turning points assigned to zombies also depending where they start, cant go up twice if they start on the highest cube

        List<Projectile> projectiles = new List<Projectile>();

        string inventoryManager = "";

        static StreamWriter outFile;
        static StreamReader inFile;

        LLZ zombieList = new LLZ();
        QueueZ zombieQueue = new QueueZ();
        QueueP projectileQueue = new QueueP();

        int wave;

        float fadeSpeed = 0.1f;

        bool newWave;
        bool lose = false;

        Random rnd = new Random();

        bool zombieGenerator = true;

        float angle1;
        float angle2;

        //Store and set the initial game state
        int gameState = MENU;

        bool introPlayed;

        int screenWidth;
        int screenHeight;

        Texture2D grassFloorImg;
        Texture2D ballImg;
        Texture2D arrowImg;
        Texture2D swordImg;
        Texture2D slingshotImg;
        Texture2D shopImg;
        Texture2D backImg;
        Texture2D storeImg;
        Texture2D playImg;
        Texture2D exitImg;
        Texture2D coinsImg;
        Texture2D shopBackgroundImg1;
        Texture2D shopBackgroundImg2;
        Texture2D shopBackgroundImg3;
        Texture2D shopBackgroundImg4;
        Texture2D bandImg;
        Texture2D stickyImg;
        Texture2D reloadImg;
        Texture2D punchImg;
        Texture2D coinBagImg;
        Texture2D loseImg;
        Texture2D buyImg;
        Texture2D plusImg;
        Texture2D minusImg;
        Texture2D barricadeImg;
        Texture2D fenceImg;

        List<Texture2D> zombieImg = new List<Texture2D>();
        Texture2D almightyPunchImg;

        Rectangle startingRec;
        Rectangle grassFloorRec;
        Rectangle projectileRec;
        Rectangle slingshotRec;
        Rectangle[] inventory = new Rectangle[3];
        Rectangle shopRec;
        Rectangle backRec;
        Rectangle band1Rec;
        Rectangle band2Rec;
        Rectangle storeRec;
        Rectangle playRec;
        Rectangle exitRec;
        Rectangle coinsRec;
        Rectangle shopBackgroundRec1;
        Rectangle shopBackgroundRec2;
        Rectangle shopBackgroundRec3;
        Rectangle shopBackgroundRec4;
        Rectangle stickyRec;
        Rectangle reloadRec;
        Rectangle punchRec;
        Rectangle coinBagRec;
        Rectangle loseRec;
        Rectangle buyRec1;
        Rectangle buyRec2;
        Rectangle buyRec3;
        Rectangle plusRec;
        Rectangle minusRec;
        Rectangle fenceRec1;
        Rectangle fenceRec2;

        MouseState mouse;
        MouseState prevMouse;

        SpriteFont introFont;
        SpriteFont numCoins;
        SpriteFont inventoryFont;

        string[] introTxt = new string[] { "The Apocolypse", "Slide 1 intro", "slide 2 intro", "slide 3 intro if needed" };

        Vector2 introLoc = new Vector2(650, 300);
        Vector2 zombieStartingPos1 = new Vector2(1200, 185);
        Vector2 zombieStartingPos2 = new Vector2(1200, 345);
        Vector2 zombieStartingPos3 = new Vector2(1200, 430);
        Vector2 zombieStartingPos4 = new Vector2(1200, 515);
        Vector2 zombieStartingPos5 = new Vector2(1200, 600); // work on starting points for zombies

        Vector2 numCoinsPos = new Vector2(850, 0);
        Vector2 rubberBandOrigin1;
        Vector2 rubberBandOrigin2;
        Vector2 inventoryPos = new Vector2(0, 0);

        //List<Zombie> zombies = new List<Zombie>();
        List<int> stickies = new List<int>();
        List<int> reloads = new List<int>();
        List<int> almightyPunches = new List<int>();

        Timer introSlideTimer;
        Timer waveTimer;
        Timer reloadTimer;
        Timer betweenWaveTimer;
        Timer playTimer;
        Timer zombieTimer;

        bool stickyPower = false;
        bool almightyPunch = false;
        bool reloadPower = false;
        bool reloadPowerStart;

        bool xtraCol;

        int waveSticky;
        int wavePunch;
        int waveReload;

        int stickyPrice = 30;
        int reloadPrice = 30;
        int punchPrice = 30;
        int sizePrice = 30;
        int maxSize = 150;
        int minSize = 70;

        int xSave;

        int totalProjectiles;

        double firstPlace;
        double secondPlace;
        double thirdPlace;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Sets the screen width and height to my preffered standards
            this._graphics.PreferredBackBufferWidth = 1400;
            this._graphics.PreferredBackBufferHeight = 790;
            this._graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            screenWidth = _graphics.GraphicsDevice.Viewport.Width;
            screenHeight = _graphics.GraphicsDevice.Viewport.Height;

            introSlideTimer = new Timer(8000, true);
            waveTimer = new Timer(Timer.INFINITE_TIMER, false);
            reloadTimer = new Timer(2500, false);
            betweenWaveTimer = new Timer(4000, false);
            playTimer = new Timer(Timer.INFINITE_TIMER, false);
            zombieTimer = new Timer(10000, false);


            menuMusic = Content.Load<Song>("Music/Playboi Carti - Molly [guitar cover instrumental]");
            gameMusic = Content.Load<Song>("Music/Michael Jackson - Thriller - Thriller");
            shopMusic = Content.Load<Song>("Music/Merry Go Round of Life - Howl's Moving Castle (Joe Hisaishi)");

            MediaPlayer.Volume = 1f;
            MediaPlayer.IsRepeating = true;

            MediaPlayer.Play(menuMusic);
            SoundEffect.MasterVolume = 1;

            purchaseSound = Content.Load<SoundEffect>("Sounds/cash-register-purchase-87313 (online-audio-converter.com)");
            zombieGroan = Content.Load<SoundEffect>("Sounds/zombie-6851 (online-audio-converter.com)");
         //   projectileShoot = Content.Load<SoundEffect>("Sounds/shoot");
          //  projectileHit = Content.Load<SoundEffect>("Sounds/hit");
          //  activatePowerUp = Content.Load<SoundEffect>("Sounds/activatePower");



            grassFloorImg = Content.Load<Texture2D>("Images/Sprites/Grass");
            ballImg = Content.Load<Texture2D>("Images/Sprites/Ball");
            arrowImg = Content.Load<Texture2D>("Images/Sprites/Arrow");
            swordImg = Content.Load<Texture2D>("Images/Sprites/Sword");
            slingshotImg = Content.Load<Texture2D>("Images/Sprites/Slingshot");
            shopImg = Content.Load<Texture2D>("Images/Sprites/ShopImg");
            backImg = Content.Load<Texture2D>("Images/Sprites/Ball");
            storeImg = Content.Load<Texture2D>("Images/Sprites/Shop");
            playImg = Content.Load<Texture2D>("Images/Sprites/Play");
            exitImg = Content.Load<Texture2D>("Images/Sprites/Exit");
            coinsImg = Content.Load<Texture2D>("Images/Sprites/Coins");
            shopBackgroundImg1 = Content.Load<Texture2D>("Images/Sprites/backgroundShop");
            shopBackgroundImg2 = Content.Load<Texture2D>("Images/Sprites/backgroundShop");
            shopBackgroundImg3 = Content.Load<Texture2D>("Images/Sprites/backgroundShop");
            shopBackgroundImg4 = Content.Load<Texture2D>("Images/Sprites/backgroundShop");
            bandImg = Content.Load<Texture2D>("Images/Sprites/RubberBand");
            stickyImg = Content.Load<Texture2D>("Images/Sprites/stickyImg");
            reloadImg = Content.Load<Texture2D>("Images/Sprites/reloadImg");
            punchImg = Content.Load<Texture2D>("Images/Sprites/punchImg");
            coinBagImg = Content.Load<Texture2D>("Images/Sprites/InventoryImg");
            loseImg = Content.Load<Texture2D>("Images/Sprites/LoseImg");
            buyImg = Content.Load<Texture2D>("Images/Sprites/BuyButton");
            plusImg = Content.Load<Texture2D>("Images/Sprites/plus");
            minusImg = Content.Load<Texture2D>("Images/Sprites/minus");
            barricadeImg = Content.Load<Texture2D>("Images/Sprites/barricade");
            fenceImg = Content.Load<Texture2D>("Images/Sprites/fence");

            zombieImg.Add(Content.Load<Texture2D>("Images/Sprites/ZombieWalking_2"));
            zombieImg.Add(Content.Load<Texture2D>("Images/Sprites/ZombieWalking_3"));
            zombieImg.Add(Content.Load<Texture2D>("Images/Sprites/ZombieWalking_4"));
            almightyPunchImg = Content.Load<Texture2D>("Images/Sprites/PunchSprite");

            startingRec = new Rectangle(0, 0, screenWidth, screenHeight);
            grassFloorRec = new Rectangle(130, 100, screenWidth - 130, screenHeight - 100);
            projectileRec = new Rectangle(10, 300, 80, 80);
            shopRec = new Rectangle((screenWidth / 2) - 250, 0, 500, 200);
            backRec = new Rectangle(1250, 0, 100, 100);
            storeRec = new Rectangle(100, 100, 300, 200);
            playRec = new Rectangle(500, 100, 300, 200);
            exitRec = new Rectangle(900, 100, 300, 200);
            coinsRec = new Rectangle(800, 0, 20, 20);
            shopBackgroundRec1 = new Rectangle(150, 300, 200, 400);
            shopBackgroundRec2 = new Rectangle(450, 300, 200, 400);
            shopBackgroundRec3 = new Rectangle(750, 300, 200, 400);
            shopBackgroundRec4 = new Rectangle(1050, 300, 200, 400);
            stickyRec = new Rectangle(300, 0, 100, 100);
            reloadRec = new Rectangle(500, 0, 100, 100);
            punchRec = new Rectangle(700, 0, 100, 100);
            coinBagRec = new Rectangle(50, 50, 150, 150);
            loseRec = new Rectangle(0, 0, 1, 1);
            buyRec1 = new Rectangle(150, 610, 200, 90);
            buyRec2 = new Rectangle(450, 610, 200, 90);
            buyRec3 = new Rectangle(750, 610, 200, 90);
            plusRec = new Rectangle(1150, 300, 100, 90);
            minusRec = new Rectangle(1050, 300, 100, 90);
            fenceRec1 = new Rectangle(0, 100, 130, screenHeight / 2 - 50);
            fenceRec2 = new Rectangle(0, 50 + (screenHeight / 2), 130, screenHeight / 2 - 50);

            slingshotRec = new Rectangle(300, 395, 50, 70);
            inventory[0] = new Rectangle(0, 0, 50, 50);
            inventory[1] = new Rectangle(50, 0, 50, 50);
            inventory[2] = new Rectangle(100, 0, 50, 50);

            band1Rec = new Rectangle(slingshotRec.X + 5, slingshotRec.Y + 5, 1, 100);
            band2Rec = new Rectangle(slingshotRec.X + 35, slingshotRec.Y + 5, 1, 100);

            rubberBandOrigin1 = new Vector2(bandImg.Width, bandImg.Height / 2);
            rubberBandOrigin2 = new Vector2(bandImg.Width, bandImg.Height / 2);

            introFont = Content.Load<SpriteFont>("Fonts/introFont");
            numCoins = Content.Load<SpriteFont>("Fonts/coinFont");
            inventoryFont = Content.Load<SpriteFont>("Fonts/inventoryFont");

            wave = 1;

            turningPoint1.Add(STRAIGHT);
            turningPoint1.Add(STRAIGHT);
            turningPoint1.Add(DOWN);
            turningPoint1.Add(STRAIGHT);
            turningPoint1.Add(STRAIGHT);
            turningPoint1.Add(DOWN);
            turningPoint1.Add(DOWN);
            turningPoint1.Add(STRAIGHT);
            turningPoint1.Add(STRAIGHT);

            turningPoint2.Add(STRAIGHT);
            turningPoint2.Add(STRAIGHT);
            turningPoint2.Add(DOWN);
            turningPoint2.Add(STRAIGHT);
            turningPoint2.Add(DOWN);
            turningPoint2.Add(STRAIGHT);
            turningPoint2.Add(UP);
            turningPoint2.Add(UP);
            turningPoint2.Add(STRAIGHT);

            turningPoint3.Add(STRAIGHT);
            turningPoint3.Add(STRAIGHT);
            turningPoint3.Add(DOWN);
            turningPoint3.Add(STRAIGHT);
            turningPoint3.Add(UP);
            turningPoint3.Add(STRAIGHT);
            turningPoint3.Add(UP);
            turningPoint3.Add(DOWN);
            turningPoint3.Add(STRAIGHT);

            turningPoint4.Add(STRAIGHT);
            turningPoint4.Add(UP);
            turningPoint4.Add(UP);
            turningPoint4.Add(STRAIGHT);
            turningPoint4.Add(STRAIGHT);
            turningPoint4.Add(STRAIGHT);
            turningPoint4.Add(UP);
            turningPoint4.Add(UP);
            turningPoint4.Add(STRAIGHT);

            turningPoint5.Add(STRAIGHT);
            turningPoint5.Add(UP);
            turningPoint5.Add(UP);
            turningPoint5.Add(STRAIGHT);
            turningPoint5.Add(DOWN);
            turningPoint5.Add(STRAIGHT);
            turningPoint5.Add(UP);
            turningPoint5.Add(STRAIGHT);
            turningPoint5.Add(STRAIGHT);

            try
            {
                inFile = File.OpenText("Data.txt");

                string line;
                string[] data;
                int count = 0;

                while (!inFile.EndOfStream)
                {
                    line = inFile.ReadLine();
                    data = line.Split(',');

                    if (count == 0)
                    {
                        introPlayed = Convert.ToBoolean(data[0]);
                    }

                    if (count == 1)
                    {
                        for (int i = 0; i < data.Length - 1; i++)
                        {
                            stickies.Add(sticky);
                        }
                    }

                    if (count == 2)
                    {
                        for (int i = 0; i < data.Length - 1; i++)
                        {
                            reloads.Add(reload);
                        }
                    }

                    if (count == 3)
                    {
                        for (int i = 0; i < data.Length - 1; i++)
                        {
                            almightyPunches.Add(punch);
                        }
                    }

                    if (count == 4)
                    {
                        coins = Convert.ToInt32(data[0]);
                    }

                    if (count == 5)
                    {
                        projectileRec.Width = Convert.ToInt32(data[0]);
                        projectileRec.Height = Convert.ToInt32(data[1]);
                    }

                    if (count == 6)
                    {
                        firstPlace = Convert.ToDouble(data[0]);
                        secondPlace = Convert.ToDouble(data[1]);
                        thirdPlace = Convert.ToDouble(data[2]);
                    }

                    if (count == 7)
                    {
                        lifeTimeCoins = Convert.ToInt32(data[0]);
                    }

                    if (count == 8)
                    {
                        totalProjectiles = Convert.ToInt32(data[0]);
                    }

                    if (count == 9)
                    {
                        xtraCol = Convert.ToBoolean(data[0]);
                    }

                    count++;
                }

                inFile.Close();
            }
            catch
            {

            }

            if (introPlayed == false)
            {
                gameState = STARTINGINTRO;
            }
            else
            {
                gameState = MENU;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (gameState)
            {
                case MENU:
                    GraphicsDevice.Clear(Color.White);

                    IsMouseVisible = true;
                    mouse = Mouse.GetState();

                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(menuMusic);
                    }

                    if (playRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        gameState = GAMEPLAY;
                        MediaPlayer.Stop();
                    }
                    else if (storeRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        gameState = SHOP;
                        MediaPlayer.Stop();
                    }
                    else if (exitRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        try
                        {
                            outFile = File.CreateText("Data.txt");

                            outFile.WriteLine(introPlayed);

                            for (int i = 0; i < stickies.Count; i++)
                            {
                                outFile.Write("s,");
                            }

                            outFile.WriteLine("");

                            for (int i = 0; i < reloads.Count; i++)
                            {
                                outFile.Write("r,");
                            }

                            outFile.WriteLine("");

                            for (int i = 0; i < almightyPunches.Count; i++)
                            {
                                outFile.Write("p,");
                            }

                            outFile.WriteLine("");

                            outFile.WriteLine(coins);

                            outFile.WriteLine(projectileRec.Width + "," + projectileRec.Height);

                            outFile.WriteLine(firstPlace + "," + secondPlace + "," + thirdPlace);

                            outFile.WriteLine(lifeTimeCoins);

                            outFile.WriteLine(totalProjectiles);

                            outFile.WriteLine(xtraCol);

                            outFile.Close();

                            Exit();
                        }
                        catch
                        {

                        }


                    }

                    prevMouse = mouse;
                    break;
                case STARTINGINTRO:
                    GraphicsDevice.Clear(Color.White);

                    if (introSlideTimer.GetTimeRemaining() <= 0)
                    {
                        introPlayed = true;
                        gameState = GAMEPLAY;
                    }

                    introSlideTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;
                case INSTRUCTIONS:

                    break;
                case GAMEPLAY:
                    GraphicsDevice.Clear(Color.White);

                    mouse = Mouse.GetState();

                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(gameMusic);
                    }



                    if (mouse.X == 5 && mouse.Y == 0 && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        if (xtraCol == true)
                        {
                            xtraCol = false;
                        }
                        else
                        {
                            xtraCol = true;
                        }
                    }
                    if (backRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {

                        // MediaPlayer.Stop();
                        MediaPlayer.Pause();

                        zombieGenerator = true;
                        while (zombieQueue.Size() != 0)
                        {
                            zombieQueue.Dequeue();
                        }
                        while (zombieList.GetCount() != 0)
                        {
                            zombieList.DeleteHead();
                        }

                        while (projectileQueue.IsEmpty() != true)
                        {
                            projectileQueue.Dequeue();
                        }

                        projectiles.Clear();

                        wave = 1;

                        almightyPunch = false;
                        stickyPower = false;
                        reloadTimer = new Timer(2500, false);
                        reloadPowerStart = false;
                        reloadPower = false;
                        lose = false;
                        loseRec.Width = 1;
                        loseRec.Height = 1;

                        gameState = MENU;
                    }

                    if (zombieGenerator == true)
                    {
                        GenerateZombies(wave);

                        waveTimer.ResetTimer(true);
                        newWave = false;
                    }

                    if (zombieList.GetCount() == 0 && zombieQueue.IsEmpty() == true)
                    {
                        if (newWave == false)
                        {
                            betweenWaveTimer.ResetTimer(true);
                            newWave = true;
                            wave++;
                        }

                        if (betweenWaveTimer.IsFinished() == true)
                        {
                            zombieGenerator = true;
                        }
                    }

                    if (waveTimer.GetTimePassed() >= 1000 - (50 * wave) && zombieQueue.IsEmpty() != true)
                    {
                        zombieList.AddToTail(zombieQueue.Dequeue());
                        waveTimer.ResetTimer(true);
                    }

                    if (projectileQueue.IsEmpty() != true)
                    {

                        if (mouse.X >= fenceRec1.Width)
                        {
                            IsMouseVisible = false;
                            xSave = mouse.X;
                        }
                        else
                        {
                            IsMouseVisible = true;
                        }


                        projectileQueue.Peek().SetXPos(xSave);
                        projectileQueue.Peek().SetYPos(mouse.Y);
                        band1Rec.Width = (int)Math.Sqrt(Math.Pow(slingshotRec.X - xSave, 2) + Math.Pow(slingshotRec.Y - mouse.Y, 2));
                        band2Rec.Width = (int)Math.Sqrt(Math.Pow(slingshotRec.X - xSave, 2) + Math.Pow(slingshotRec.Y - mouse.Y, 2));
                        angle1 = (float)(-1 * Math.Atan2((mouse.Y - slingshotRec.Y), (slingshotRec.X - xSave)));
                        angle2 = (float)(-1 * Math.Atan2((mouse.Y - slingshotRec.Y), (slingshotRec.X - xSave)));


                        

                        if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed && mouse.X < slingshotRec.X && lose == false)
                        {
                            projectileQueue.Peek().SetCalc(true);
                            projectileQueue.Peek().SetShoot(true);
                            projectiles.Add(projectileQueue.Dequeue());
                            totalProjectiles++;
                        }
                    }
                    else
                    {
                        IsMouseVisible = true;
                    }

                    if (projectileQueue.IsEmpty() == true)
                    {
                        reloadTimer.Activate();

                        if (reloadTimer.IsFinished() == true)
                        {
                            reloadTimer.ResetTimer(false);

                            projectileQueue.Enqueue(new Projectile(wave, ballImg, arrowImg, swordImg, almightyPunchImg, projectileRec, almightyPunch, punchImg));
                            projectileQueue.Enqueue(new Projectile(wave, ballImg, arrowImg, swordImg, almightyPunchImg, projectileRec, almightyPunch, punchImg));
                            projectileQueue.Enqueue(new Projectile(wave, ballImg, arrowImg, swordImg, almightyPunchImg, projectileRec, almightyPunch, punchImg));
                        }
                    }


                    if (stickies.Count != 0 && stickyRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed && stickyPower != true && lose == false)
                    {
                        stickyPower = true;
                        stickies.RemoveAt(0);
                        waveSticky = wave;
                    }

                    if (waveSticky != wave && stickyPower == true)
                    {
                        stickyPower = false;

                        for (int i = projectiles.Count - 1; i >= 0; i--)
                        {
                            projectiles.RemoveAt(i);
                        }
                    }

                    if (reloads.Count != 0 && reloadRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed && reloadPower != true && lose == false)
                    {
                        reloadPower = true;
                        reloadPowerStart = true;
                        reloads.RemoveAt(0);
                        waveReload = wave;
                    }

                    if (waveReload != wave && reloadPower == true)
                    {
                        reloadPower = false;
                        reloadPowerStart = false;
                        reloadTimer = new Timer(2500, false);
                    }

                    if (almightyPunches.Count != 0 && punchRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed && almightyPunch != true && lose == false)
                    {
                        almightyPunches.RemoveAt(0);
                        almightyPunch = true;
                        wavePunch = wave;

                        while (projectileQueue.IsEmpty() != true)
                        {
                            projectileQueue.Dequeue();
                        }
                    }

                    if (wavePunch != wave && almightyPunch == true)
                    {
                        almightyPunch = false;

                        while (projectileQueue.IsEmpty() != true)
                        {
                            projectileQueue.Dequeue();
                        }

                        for (int i = projectiles.Count - 1;  i >= 0; i--)
                        {
                            projectiles.RemoveAt(i);
                        }
                    }

                    if (stickyPower == true)
                    {
                        if (projectileQueue.IsEmpty() != true)
                        {
                            projectileQueue.Peek().SetSticky(true);
                        }
                    }

                    if (almightyPunch == true)
                    {
                        if (projectileQueue.IsEmpty() != true)
                        {
                            if (projectileQueue.Peek().GetImg() == almightyPunchImg)
                            {
                                projectileQueue.Peek().UpdateAni(gameTime);
                            }
                        }

                        if (projectiles.Count != 0)
                        {
                            for (int i = 0; i < projectiles.Count; i++)
                            {
                                projectiles[i].UpdateAni(gameTime);

                                if (projectiles[i].GetPos().X > screenWidth || projectiles[i].GetPos().X < 0 || projectiles[i].GetPos().Y > screenHeight || projectiles[i].GetPos().Y < 0 || projectiles[i].GetShoot() != true)
                                {
                                    projectiles.RemoveAt(i);
                                }
                            }
                        }
                    }

                    if (zombieTimer.IsFinished() == true || zombieTimer.IsInactive() == true)
                    {
                        zombieGroan.CreateInstance().Play();
                        zombieTimer.ResetTimer(true);
                    }

                    if (reloadPower == true && reloadPowerStart == true)
                    {
                        reloadTimer = new Timer(200, false);
                        reloadPowerStart = false;
                    }

                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        if (projectiles[i].GetShoot() == true)
                        {
                            projectiles[i].Shoot(gameTime);
                        }
                    }

                    for (int i = 0; i < zombieList.GetCount(); i++)
                    {
                        zombieList.GetZombie(i).Walk(lose);

                        zombieList.GetZombie(i).UpdateZombie(gameTime);

                        if (zombieList.GetZombie(i).GetPos().X <= grassFloorRec.X * 2 / 3)
                        {
                            lose = true;
                        }

                        for (int j = 0; j < projectiles.Count; j++)
                        {
                            if (zombieList.GetZombie(i).GetPos().Intersects(projectiles[j].GetPos()))
                            {
                                zombieList.GetZombie(i).Hit(projectiles[j].GetDamage());

                                if (almightyPunch != true)
                                {
                                    projectiles.RemoveAt(j);
                                }

                                if (zombieList.GetZombie(i).GetHealth() <= 0)
                                {
                                    for (int a = 0; a < zombieList.GetZombie(i).GetDifficulty(); a++)
                                    {
                                        coins++;
                                        lifeTimeCoins++;
                                    }
                                    zombieList.Delete(i);

                                    break;
                                }

                                break;
                            }
                            else if (projectiles[j].GetShoot() == false && almightyPunch != true && projectiles.Count != 0)
                            {
                                projectiles.RemoveAt(j);
                            }

                        }
                    }

                    if (lose == true)
                    {
                        if (fadeSpeed <= 1)
                        {
                            fadeSpeed += 0.01f;
                        }

                        if (loseRec.Height < screenHeight)
                        {
                            loseRec.Width++;
                            loseRec.Height++;
                            loseRec.X = (screenWidth / 2) - (loseRec.Width / 2);
                            loseRec.Y = (screenHeight / 2) - (loseRec.Height / 2);
                        }
                    }

                    if (playTimer.GetTimePassed() > thirdPlace)
                    {
                        thirdPlace = playTimer.GetTimePassed();
                    }

                    if (playTimer.GetTimePassed() > secondPlace)
                    {
                        thirdPlace = secondPlace;
                        secondPlace = playTimer.GetTimePassed();
                    }

                    if (playTimer.GetTimePassed() > firstPlace)
                    {
                        thirdPlace = secondPlace;
                        secondPlace = firstPlace;
                        firstPlace = playTimer.GetTimePassed();
                    }

                    prevMouse = mouse;

                    betweenWaveTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
                    reloadTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
                    waveTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
                    playTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
                    zombieTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;
                case PAUSE:
                    GraphicsDevice.Clear(Color.White);

                    break;
                case INVENTORY:
                    GraphicsDevice.Clear(Color.White);

                    mouse = Mouse.GetState();

                    if (backRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        gameState = SHOP;
                    }

                    inventoryManager = "LeaderBoard\n1) " + firstPlace + "\n2) " + secondPlace + "\n3) " + thirdPlace
                        + "\nStickies: " + stickies.Count
                        + "\nReloads: " + reloads.Count
                        + "\nAlmighty Punches: " + almightyPunches.Count
                        + "\nBall Size Indicator (1 = min   8 = max):" + ((projectileRec.Width / 10) - 7)
                        + "\nCoins: " + coins + "\nTotal Lifetime Coins: " + lifeTimeCoins
                        + "\nTotal Projectiles Thrown" + totalProjectiles
                        + "\nZombie Secret (Shhhh):" + xtraCol;

                    prevMouse = mouse;
                    break;
                case SHOP:
                    GraphicsDevice.Clear(Color.Blue);

                    IsMouseVisible = true;
                    mouse = Mouse.GetState();

                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(shopMusic);
                    }

                    if (backRec.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        MediaPlayer.Stop();
                        gameState = MENU;
                    }

                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        if (buyRec1.Contains(mouse.Position))
                        {
                            if (coins >= stickyPrice)
                            {
                                purchaseSound.CreateInstance().Play();
                                coins = coins - stickyPrice;
                                stickies.Add(sticky);
                            }
                        }
                        else if (buyRec2.Contains(mouse.Position))
                        {
                            if (coins >= reloadPrice)
                            {
                                purchaseSound.CreateInstance().Play();
                                reloads.Add(reload);
                                coins = coins - reloadPrice;
                            }
                        }
                        else if (buyRec3.Contains(mouse.Position))
                        {
                            if (coins >= punchPrice)
                            {
                                purchaseSound.CreateInstance().Play();
                                almightyPunches.Add(punch);
                                coins = coins - reloadPrice;
                            }
                        }
                        else if (plusRec.Contains(mouse.Position) && projectileRec.Height < maxSize && projectileRec.Width < maxSize)
                        {
                            if (coins > sizePrice)
                            {
                                purchaseSound.CreateInstance().Play();
                                projectileRec.Height += 10;
                                projectileRec.Width += 10;
                                coins = coins - sizePrice;
                            }
                        }
                        else if (minusRec.Contains(mouse.Position) && projectileRec.Height > minSize && projectileRec.Width > minSize)
                        {
                            purchaseSound.CreateInstance().Play();
                            projectileRec.Height -= 10;
                            projectileRec.Width -= 10;
                            coins = coins + sizePrice;
                        }
                        else if (coinBagRec.Contains(mouse.Position))
                        {
                            gameState = INVENTORY;
                        }
                    }

                    prevMouse = mouse;
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            // TODO: Add your drawing code here
            switch (gameState)
            {
                case MENU:
                    _spriteBatch.Draw(storeImg, storeRec, Color.White);
                    _spriteBatch.Draw(playImg, playRec, Color.White);
                    _spriteBatch.Draw(exitImg, exitRec, Color.White);
                    break;
                case STARTINGINTRO:
                    GraphicsDevice.Clear(Color.Black);

                    _spriteBatch.Draw(barricadeImg, startingRec, Color.White);

                    if (introSlideTimer.GetTimePassed() <= 2000)
                    {
                        _spriteBatch.DrawString(introFont, introTxt[0], introLoc, Color.White);
                    }
                    else if (introSlideTimer.GetTimePassed() <= 4000)
                    {
                        _spriteBatch.DrawString(introFont, introTxt[1], introLoc, Color.White);
                    }
                    else if (introSlideTimer.GetTimePassed() <= 6000)
                    {
                        _spriteBatch.DrawString(introFont, introTxt[2], introLoc, Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(introFont, introTxt[3], introLoc, Color.White);
                    }

                    break;
                case INSTRUCTIONS:
                    break;
                case GAMEPLAY:
                    _spriteBatch.Draw(backImg, backRec, Color.Green);
                    _spriteBatch.Draw(grassFloorImg, grassFloorRec, Color.DarkKhaki);
                    _spriteBatch.Draw(fenceImg, fenceRec1, Color.White);
                    _spriteBatch.Draw(fenceImg, fenceRec2, Color.White);

                    for (int i = 0; i < zombieList.GetCount(); i++)
                    {
                        zombieList.GetZombie(i).DrawZombie(_spriteBatch, xtraCol);
                    }

                    if (projectileQueue.IsEmpty() != true)
                    {
                        _spriteBatch.Draw(bandImg, band2Rec, null, Color.White, angle2, rubberBandOrigin2, SpriteEffects.None, 0);
                        projectileQueue.Peek().DrawProjectile(_spriteBatch);
                    }

                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        if (projectiles[i].GetShoot() == true)
                        {
                            projectiles[i].DrawProjectile(_spriteBatch);
                        }
                    } 

                    for (int i = 0; i < projectileQueue.Size(); i++)
                    {
                        _spriteBatch.Draw(projectileQueue.Peek().GetTexture2D(), inventory[i], Color.White);
                    }

                    _spriteBatch.Draw(slingshotImg, slingshotRec, Color.White);

                    if (projectileQueue.IsEmpty() != true)
                    {
                        _spriteBatch.Draw(bandImg, band1Rec, null, Color.White, angle1, rubberBandOrigin1, SpriteEffects.None, 0);
                    }

                    if (stickies.Count != 0)
                    {
                        _spriteBatch.Draw(stickyImg, stickyRec, Color.LightGoldenrodYellow);
                    }
                    else
                    {
                        _spriteBatch.Draw(stickyImg, stickyRec, Color.DarkGray);
                    }

                    if (reloads.Count != 0)
                    {
                        _spriteBatch.Draw(reloadImg, reloadRec, Color.LightGoldenrodYellow);
                    }
                    else
                    {
                        _spriteBatch.Draw(reloadImg, reloadRec, Color.DarkGray);
                    }

                    if (almightyPunches.Count != 0)
                    {
                        _spriteBatch.Draw(punchImg, punchRec, Color.LightGoldenrodYellow);
                    }
                    else
                    {
                        _spriteBatch.Draw(punchImg, punchRec, Color.DarkGray);
                    }
                    _spriteBatch.Draw(coinsImg, coinsRec, Color.White);
                    if (lose == true)
                    {
                        _spriteBatch.Draw(loseImg, loseRec, Color.DarkSeaGreen * fadeSpeed);
                    }
                    _spriteBatch.DrawString(numCoins, Convert.ToString(coins) + "   " + wave + "   " + zombieQueue.Size(), numCoinsPos, Color.Gold);
                    
                    break;
                case PAUSE:

                    break;
                case INVENTORY:
                    _spriteBatch.Draw(backImg, backRec, Color.Green);
                    _spriteBatch.DrawString(inventoryFont, inventoryManager, inventoryPos, Color.Black);
                    break;
                case SHOP:
                    _spriteBatch.Draw(shopBackgroundImg1, shopBackgroundRec1, Color.White);
                    _spriteBatch.Draw(shopBackgroundImg2, shopBackgroundRec2, Color.White);
                    _spriteBatch.Draw(shopBackgroundImg3, shopBackgroundRec3, Color.White);
                    _spriteBatch.Draw(shopBackgroundImg4, shopBackgroundRec4, Color.White);
                    _spriteBatch.Draw(shopImg, shopRec, Color.White);
                    _spriteBatch.Draw(backImg, backRec, Color.Green);
                    _spriteBatch.Draw(coinsImg, coinsRec, Color.White);
                    _spriteBatch.Draw(coinBagImg, coinBagRec, Color.White);
                    _spriteBatch.DrawString(numCoins, Convert.ToString(coins), numCoinsPos, Color.Gold);

                    if (coins >= stickyPrice)
                    {
                        _spriteBatch.Draw(buyImg, buyRec1, Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(buyImg, buyRec1, Color.White * 0.5f);
                    }

                    if (coins >= reloadPrice)
                    {
                        _spriteBatch.Draw(buyImg, buyRec2, Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(buyImg, buyRec2, Color.White * 0.5f);
                    }

                    if (coins >= punchPrice)
                    {
                        _spriteBatch.Draw(buyImg, buyRec3, Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(buyImg, buyRec3, Color.White * 0.5f);
                    }

                    if (coins >= sizePrice && projectileRec.Height != maxSize)
                    {
                        _spriteBatch.Draw(plusImg, plusRec, Color.White);

                    }
                    else
                    {
                        _spriteBatch.Draw(plusImg, plusRec, Color.White * 0.5f);
                    }

                    if (projectileRec.Height != minSize)
                    {

                        _spriteBatch.Draw(minusImg, minusRec, Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(minusImg, minusRec, Color.White * 0.5f);
                    }
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void GenerateZombies(int waveNum)
        {
            int randomNum;

            Vector2 temp;

            List<int> turningPoints;

            Zombie[] tempStorage = new Zombie[(waveNum + 9) * 2];
            for (int i = 0; i < (waveNum + 9) * 2; i++)
            {
                randomNum = rnd.Next(1, 101);

                if (randomNum <= 20)
                {
                    temp = zombieStartingPos1;
                    turningPoints = turningPoint1;
                }
                else if (randomNum <= 40)
                {
                    temp = zombieStartingPos2;
                    turningPoints = turningPoint2;
                }
                else if (randomNum <= 60)
                {
                    temp = zombieStartingPos3;
                    turningPoints = turningPoint3;
                }
                else if (randomNum <= 80)
                {
                    temp = zombieStartingPos4;
                    turningPoints = turningPoint4;
                }
                else
                {
                    temp = zombieStartingPos5;
                    turningPoints = turningPoint5;
                }

                if (randomNum < 70)
                {
                    // zombieQueue.Enqueue(new Zombie(EASY, STARTINGDIRECTION, zombieImg[0], zombieStartingPos));
                    tempStorage[i] = new Zombie(EASY * waveNum, STARTINGDIRECTION, zombieImg[0], temp, turningPoints, waveNum);

                }
                else if (randomNum < 90)
                {
                    //zombieQueue.Enqueue(new Zombie(MEDIUM, STARTINGDIRECTION, zombieImg[1], zombieStartingPos));
                    tempStorage[i] = new Zombie(MEDIUM * waveNum, STARTINGDIRECTION, zombieImg[1], temp, turningPoints, waveNum);
                }
                else
                {
                    //zombieQueue.Enqueue(new Zombie(HARD, STARTINGDIRECTION, zombieImg[1], zombieStartingPos));
                    tempStorage[i] = new Zombie(HARD * waveNum, STARTINGDIRECTION, zombieImg[2], temp, turningPoints, waveNum);
                }
            }

            InsertionSort(tempStorage);

            zombieGenerator = false;
        }

        private void InsertionSort(Zombie[] zombies)
        {
            Zombie temp;

            int j;

            for (int i = 1; i < zombies.Length; i++)
            {
                temp = zombies[i];

                for (j = i; j > 0; j--)
                {
                    if (zombies[j - 1].GetSpeed() > temp.GetSpeed())
                    {
                        zombies[j] = zombies[j - 1];
                    }
                    else
                    {
                        break;
                    }
                }

                zombies[j] = temp;
            }

            for (int i = 0; i < zombies.Length; i++)
            {
                zombieQueue.Enqueue(zombies[i]);
            }
        }

    }
}
// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using SharpDX;
using SharpDX.Toolkit;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using System;
using System.Text;
using System.Collections.Generic;

namespace Lab
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    public class LabGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        public List<GameObject> gameObjects;
        public List<Enemy> enemies;
        private Stack<GameObject> addedGameObjects;
        private Stack<GameObject> removedGameObjects;
        private KeyboardManager keyboardManager;
        public KeyboardState keyboardState;
        public Player player;
        public Landscape landscape;
        private Portal portal;
        private Effect cubeEffect;
        private Effect spotLightEffect;
        private Effect portalEffect;
        //private Texture2D texture;
        private Texture2D texture;
        private EnemyController enemyController;
        public MainPage mainPage;
        public int score;
        public string name;
        public List<Tuple<string, int>> scoreList = new List<Tuple<string,int>>();

        //private Enemy enemy1;
        //private Enemy enemy2;
        //private Enemy enemy3;
        //private Enemy enemy4;
        //private Enemy enemy5;

        public float followerSpeed = 1.00f;
        public float finderSpeed  = 0.20f;
        private float enemyCount = 6;

        // Represents the camera's position and orientation
        public Camera camera;

        // Graphics assets
        //public Assets assets;

        // Random number generator
        public Random random;

        // World boundaries that indicate where the edge of the screen is for the camera.
        public float boundaryLeft;
        public float boundaryRight;
        public float boundaryTop;
        public float boundaryBottom;
        public bool started = false;
        public float difficulty;

        public SoundEffect backgroundSoundEffect;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabGame" /> class.
        /// </summary>
        public LabGame(MainPage mainPage)
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            // Create the keyboard manager
            keyboardManager = new KeyboardManager(this);
            //assets = new Assets(this);
            random = new Random();
            this.mainPage = mainPage;

            backgroundSoundEffect = new SoundEffect(@"Content\ghostly-drone.wav", true);
            //backgroundSoundEffect.SetVolume(0.5f);
            /*this.score = 300;
            this.name = "Erlangga";
            var task = this.WriteDataToFileAsync("textBrian1.txt", name + "\t" + score + "\n");
            task.ConfigureAwait(false);*/
        }

        public string getAsString(List<Tuple<string, int>> listInput)
        {
            string text = "";

            foreach( var s in scoreList)
            {
                text += s.Item1 + "\t" + Convert.ToInt32(s.Item2) + "\n";
            }
            return text;
        }

        public string readFromlist(string filename, Tuple<string, int> input)
        {
            string content = input.Item1 + "\t" + Convert.ToInt32(input.Item2) + "\n";
            var task = this.WriteDataToFileAsync("textBrian1.txt", content);
            task.ConfigureAwait(false);

            return getAsString(this.scoreList);
        }

        public async Task WriteDataToFileAsync(string filename, string content)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            var task = this.ReadFileContentsAsync(filename);
            //task.ConfigureAwait(false);

            scoreList.Add(Tuple.Create<string, int>(name, score));

            scoreList.Sort((a, b) => b.Item2.CompareTo(a.Item2));
            await FileIO.WriteTextAsync(file, getAsString(scoreList));
            /*if (file != null)
            {
                await FileIO.AppendTextAsync(file, getAsString(scoreList));
            }
            else
            {
                // Write some content to the file
                await FileIO.WriteTextAsync(file, getAsString(scoreList));
            }*/

        }

        private Tuple<string, int> getTuple(string input)
        {
            Tuple<string, int> output;
            String[] splitInput = input.Split('\t');
           
            output = Tuple.Create<string, int>(splitInput[0], Convert.ToInt32(splitInput[1]));
            return output;
        }

        public async Task ReadFileContentsAsync(string fileName)
        {
            var folder = ApplicationData.Current.LocalFolder;
            string text;
            try
            {
                var file = await folder.OpenStreamForReadAsync(fileName);

                using (var streamReader = new StreamReader(file))
                {
                    while ((text = streamReader.ReadLine()) != null)
                    {
                        this.scoreList.Add(getTuple(text));
                    }
                }
            }
            catch (Exception)
            {
                
            }

        }

        protected override void LoadContent()
        {
            this.score = 0;
            // Initialise game object containers.
            gameObjects = new List<GameObject>();
            addedGameObjects = new Stack<GameObject>();
            removedGameObjects = new Stack<GameObject>();
          

            // Create game objects.
            player = new Player(this);
            landscape = new Landscape(this);
            camera = new Camera(this);
            enemyController = new EnemyController(this);
            portal = new Portal(this);

            gameObjects.Add(player);
            gameObjects.Add(landscape);
            gameObjects.Add(portal);

            // add enemies
            enemies = new List<Enemy>();
            
            enemies.Add(new Enemy(this, new Vector3(10,50,10), EnemyType.Follower, followerSpeed));
            /*enemies.Add(new Enemy(this, new Vector3(10, 20, 100), EnemyType.Wanderer, finderSpeed));
            enemies.Add(new Enemy(this, new Vector3(100, 20, 100), EnemyType.Wanderer, finderSpeed));
            enemies.Add(new Enemy(this, new Vector3(100, 20, 10), EnemyType.Wanderer, finderSpeed));
            enemies.Add(new Enemy(this, new Vector3(50, 20, 50), EnemyType.Wanderer, finderSpeed));
            enemies.Add(new Enemy(this, new Vector3(70, 20, 70), EnemyType.Wanderer, finderSpeed));
            enemies.Add(new Enemy(this, new Vector3(30, 20, 70), EnemyType.Wanderer, finderSpeed));*/


            Vector3 randomPos;
            for (int i = 0; i < enemyCount; i++ )
            {
                randomPos = new Vector3(random.NextFloat(i, landscape.getWidth()), 100.0f, random.NextFloat(i, landscape.getWidth()));
                enemies.Add(new Enemy(this, randomPos, EnemyType.Wanderer, finderSpeed));   
            }


                /*for (int i = 0; i < enemies.L; i++)
                {
                    // random pos for enemy
                    Vector3 randomPos = new Vector3(random.NextFloat( i*2, landscape.getWidth()), 50.0f, random.NextFloat(i*2, landscape.getWidth()));
                    enemies.Add(new Enemy(this, randomPos, EnemyType.Wanderer, finderSpeed));
                }*/




                initEffect();
            //gameObjects.Add(new EnemyController(this));
            
            
            // Create an input layout from the vertices

            backgroundSoundEffect.Play();

            base.LoadContent();
        }


        public void LoadNewContent()
        {
            // stop enemy's sound
            player.enemySoundEffect.Stop();

            // remove old things
            gameObjects.Clear();
            enemies.Clear();
            LoadContent();
        }


        protected override void Initialize()
        {
            Window.Title = "Lab 4";
            
    
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (started)
            {
                keyboardState = keyboardManager.GetState();
                flushAddedAndRemovedGameObjects();
                enemyController.Update(gameTime);
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Update(gameTime);

                }

                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Update(gameTime);

                }
                camera.Update();
                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    //this.Exit();
                    //this.Dispose();
                    backgroundSoundEffect.Stop();
                }

                if (keyboardState.IsKeyDown(Keys.Tab))
                {
                    backgroundSoundEffect.Play();
                }

                mainPage.UpdateScore(score);
                mainPage.UpdateHP(this.player.hp);

                
                // Handle base.Update
                base.Update(gameTime);
                //Debug.WriteLine(backgroundSoundEffect.GetVolume());
            }
            

        }

        protected override void Draw(GameTime gameTime)
        {
            if (started)
            {
                // Clears the screen with the Color.CornflowerBlue
                GraphicsDevice.Clear(Color.Black);

                //texture = Content.Load<Texture2D>("texture");

                this.cubeEffect.Parameters["Projection"].SetValue(this.camera.Projection);
                this.cubeEffect.Parameters["View"].SetValue(this.camera.View);
                this.cubeEffect.Parameters["cameraPos"].SetValue(this.camera.cameraPos);
                this.spotLightEffect.Parameters["Projection"].SetValue(this.camera.Projection);
                this.spotLightEffect.Parameters["View"].SetValue(this.camera.View);
                this.spotLightEffect.Parameters["cameraPos"].SetValue(this.camera.cameraPos);
                this.portalEffect.Parameters["Projection"].SetValue(this.camera.Projection);
                this.portalEffect.Parameters["View"].SetValue(this.camera.View);
                this.portalEffect.Parameters["cameraPos"].SetValue(this.camera.cameraPos);

                //this.spotLightEffect.Parameters["Texture"].SetResource(texture);
                this.spotLightEffect.Parameters["lightAmbCol"].SetValue(Color.White.ToVector3());

                // pass lights to soptlight.fx and draw enemies
                //Vector3[] lightArr = new Vector3[enemies.Count];
                for (int i = 0; i < enemies.Count; i++)
                {
                    this.spotLightEffect.Parameters["lightPntPos" + i.ToString()].SetValue(enemies[i].pos);
                    enemies[i].Draw(gameTime, cubeEffect);
                }

                // this.spotLightEffect.Parameters["lightArr"].SetValue(lightArr);
                this.spotLightEffect.Parameters["lightCount"].SetValue(enemies.Count);
                //this.spotLightEffect.Parameters["MAX_LIGHT"].SetValue(MAX_LIGHT);


                this.spotLightEffect.Parameters["lightPntCol"].SetValue(Color.White.ToVector3());
                this.spotLightEffect.Parameters["lightDir"].SetValue(-Vector3.UnitY);

                this.texture = Content.Load<Texture2D>("texture.jpg");
                this.spotLightEffect.Parameters["Texture"].SetResource(texture);
                this.spotLightEffect.CurrentTechnique = this.spotLightEffect.Techniques["Lighting"];


                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i].type == GameObjectType.Landscape) { gameObjects[i].Draw(gameTime, spotLightEffect); }
                    else if (gameObjects[i].type == GameObjectType.Portal) { gameObjects[i].Draw(gameTime, portalEffect); }
                    else { gameObjects[i].Draw(gameTime, cubeEffect); }
                }
                // Handle base.Draw
                base.Draw(gameTime);
            
            }
        }
        // Count the number of game objects for a certain type.
        public int Count(GameObjectType type)
        {
            int count = 0;
            foreach (var obj in gameObjects)
            {
                if (obj.type == type) { count++; }
            }
            return count;
        }

        // Add a new game object.
        public void Add(GameObject obj)
        {
            if (!gameObjects.Contains(obj) && !addedGameObjects.Contains(obj))
            {
                addedGameObjects.Push(obj);
            }
        }

        // Remove a game object.
        public void Remove(GameObject obj)
        {
            if (gameObjects.Contains(obj) && !removedGameObjects.Contains(obj))
            {
                removedGameObjects.Push(obj);
            }
        }

        // Process the buffers of game objects that need to be added/removed.
        private void flushAddedAndRemovedGameObjects()
        {
            while (addedGameObjects.Count > 0) { gameObjects.Add(addedGameObjects.Pop()); }
            while (removedGameObjects.Count > 0) { gameObjects.Remove(removedGameObjects.Pop()); }
        }

        private void initEffect()
        {
            this.cubeEffect = Content.Load<Effect>("Phong");
            this.spotLightEffect = Content.Load<Effect>("Spotlight");
            this.portalEffect = Content.Load<Effect>("Portal");
            this.cubeEffect.Parameters["Projection"].SetValue(this.camera.Projection);
            this.cubeEffect.Parameters["View"].SetValue(this.camera.View);
            this.cubeEffect.Parameters["cameraPos"].SetValue(this.camera.cameraPos);
            this.spotLightEffect.Parameters["Projection"].SetValue(this.camera.Projection);
            this.spotLightEffect.Parameters["View"].SetValue(this.camera.View);
            this.spotLightEffect.Parameters["cameraPos"].SetValue(this.camera.cameraPos);

            this.texture = Content.Load<Texture2D>("enemy.png");
            this.spotLightEffect.Parameters["Texture"].SetResource(texture);
            this.spotLightEffect.CurrentTechnique = this.spotLightEffect.Techniques["Lighting"];
        }
    }
}

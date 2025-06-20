using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SolarSystem
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        BasicEffect effect;
        Matrix view;
        Matrix projection;

        Vector3 cameraPosition;
        float cameraDistance;
        float cameraAzimuth;
        float cameraElevaion;

        MouseState previousMouseState;

        List<CelestialBody> bodies;
        float G;
        float timeStep;

        Model sphereModel;

        CelestialBody sun;
        CelestialBody earth;
        CelestialBody moon;
        CelestialBody jupiter;
        CelestialBody jMoon1;
        CelestialBody jMoon2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            bodies = new List<CelestialBody>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            cameraPosition = new Vector3(0, 0, 500);
            cameraDistance = 800f;
            cameraAzimuth = 0f;
            cameraElevaion = 0.3f;
            view = Matrix.CreateLookAt(
                cameraPosition, Vector3.Zero, Vector3.Up
            );
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                GraphicsDevice.Viewport.AspectRatio,
                1f,
                10000f
            );

            G = 1f;
            timeStep = 0.01f;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            effect = new BasicEffect(GraphicsDevice);
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;

            sphereModel = Content.Load<Model>("sphere");

            sun = new CelestialBody(
                "Sun",
                10000f,
                Vector3.Zero,
                Vector3.Zero,
                50f,
                Color.Yellow
            );

            earth = new CelestialBody(
                "Earth",
                10f,
                new Vector3(150f, 0, 0),
                Vector3.Zero,
                10f,
                Color.Blue
            );
            moon = new CelestialBody(
                "Moon",
                1f,
                new Vector3(170f, 0, 0),
                // Earth's velocity + Moon's relative velocity
                Vector3.Zero,
                3f,
                Color.Gray
            );

            jupiter = new CelestialBody(
                "Jupiter",
                50f,
                new Vector3(300f, 0, 0),
                Vector3.Zero,
                20f,
                Color.Orange
            );
            jMoon1 = new CelestialBody(
                "JMoon1",
                1f,
                new Vector3(330f, 0, 0),
                Vector3.Zero,
                3f,
                Color.DimGray
            );
            jMoon2 = new CelestialBody(
                "JMoon2",
                1f,
                new Vector3(345f, 0, 0),
                Vector3.Zero,
                4f,
                Color.LightGray
            );

            bodies.Add(sun);
            bodies.Add(earth);
            bodies.Add(moon);
            bodies.Add(jupiter);
            bodies.Add(jMoon1);
            bodies.Add(jMoon2);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                float deltaX = mouseState.X - previousMouseState.X;
                float deltaY = mouseState.Y - previousMouseState.Y;

                cameraAzimuth += deltaX * 0.01f;
                cameraElevaion -= deltaY * 0.01f;

                cameraElevaion = MathHelper.Clamp(
                    cameraElevaion,
                    -MathHelper.PiOver2 + 0.1f,
                    MathHelper.PiOver2 - 0.01f
                );
            }

            previousMouseState = mouseState;

            float camX = cameraDistance *
                (float)(Math.Cos(cameraElevaion) * Math.Cos(cameraAzimuth));
            float camY = cameraDistance *
                (float)Math.Sin(cameraElevaion);
            float camZ = cameraDistance *
                (float)(Math.Cos(cameraElevaion) * Math.Sin(cameraAzimuth));

            cameraPosition = new Vector3(camX, camY, camZ);

            view = Matrix.CreateLookAt(
                cameraPosition, Vector3.Zero, Vector3.Up);

            Utilities.InitializeVelocities(bodies, G);
            // DEBUG détaillé
            if (gameTime.TotalGameTime.TotalSeconds < 2.0)
            {
                Console.WriteLine($"Time: {gameTime.TotalGameTime.TotalSeconds:F2}");
                Console.WriteLine($"Earth pos: {earth.Position}, vel: {earth.Velocity}");
                Console.WriteLine($"Moon pos: {moon.Position}, vel: {moon.Velocity}");
                Console.WriteLine($"Distance Earth-Moon: {(moon.Position - earth.Position).Length():F2}");
                Console.WriteLine("---");
            }

            foreach (CelestialBody body in bodies)
                body.Acceleration = Vector3.Zero;

            // Calcul des forces avec DEBUG
            for (int i = 0; i < bodies.Count; i++)
            {
                for (int j = 0; j < bodies.Count; j++)
                {
                    if (i == j) continue;

                    Vector3 direction = bodies[j].Position - bodies[i].Position;
                    float distance = direction.Length();

                    if (distance < 1f || float.IsNaN(distance) || distance == 0f)
                        continue;

                    float forceMagnitude = G * bodies[i].Mass * bodies[j].Mass / (distance * distance);
                    Vector3 force = Vector3.Normalize(direction) * forceMagnitude;
                    bodies[i].Acceleration += force / bodies[i].Mass;

                    // DEBUG pour les forces sur la Lune
                    if (bodies[i].Name == "Moon" && gameTime.TotalGameTime.TotalSeconds < 1.0)
                    {
                        Console.WriteLine($"Force on Moon from {bodies[j].Name}: {force}, magnitude: {forceMagnitude:F4}");
                        Console.WriteLine($"Moon acceleration from {bodies[j].Name}: {force / bodies[i].Mass}");
                    }
                }
            }

            foreach (CelestialBody body in bodies)
            {
                body.Velocity += body.Acceleration * timeStep;
                body.Position += body.Velocity * timeStep;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            foreach (CelestialBody body in bodies)
            {
                DrawBody(body);
            }

            base.Draw(gameTime);
        }

        void DrawBody(CelestialBody body)
        {
            Console.WriteLine($"Drawing {body.Name} at {body.Position}");
            if (sphereModel == null || sphereModel.Meshes.Count == 0)
                throw new Exception("There is a problem (Good Luck!)");

            Matrix world =
                Matrix.CreateScale(body.Radius) *
                Matrix.CreateTranslation(body.Position);

            foreach (ModelMesh mesh in sphereModel.Meshes)
            {
                foreach (BasicEffect meshEffect in mesh.Effects)
                {
                    meshEffect.World = world;
                    meshEffect.View = view;
                    meshEffect.Projection = projection;
                    meshEffect.DiffuseColor = body.Color.ToVector3();
                    meshEffect.EnableDefaultLighting();

                    meshEffect.CurrentTechnique.Passes[0].Apply();
                }
                mesh.Draw();
            }
        }
    }
}

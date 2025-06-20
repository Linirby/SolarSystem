using Microsoft.Xna.Framework;

namespace SolarSystem
{
    public class CelestialBody
    {
        public string Name;
        public float Mass;
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public float Radius;
        public Color Color;

        public CelestialBody(
            string name,
            float mass,
            Vector3 position,
            Vector3 velocity,
            float radius,
            Color color
        )
        {
            Name = name;
            Mass = mass;
            Position = position;
            Velocity = velocity;
            Radius = radius;
            Color = color;
        }
    }
}
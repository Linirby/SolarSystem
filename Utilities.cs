using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SolarSystem
{
    public static class Utilities
    {
        // private static bool velocitiesInitialized = false;

        public static float SpeedCalculation(
            float G, float mass, float distance)
        {
            return (float)Math.Sqrt(G * mass / distance); ;
        }

        public static void InitializeVelocities(
            List<CelestialBody> bodies, float G)
        {
            // if (velocitiesInitialized) return;

            var sun = bodies.Find(b => b.Name == "Sun");
            var earth = bodies.Find(b => b.Name == "Earth");
            var moon = bodies.Find(b => b.Name == "Moon");
            var jupiter = bodies.Find(b => b.Name == "Jupiter");
            var jMoon1 = bodies.Find(b => b.Name == "JMoon1");
            var jMoon2 = bodies.Find(b => b.Name == "JMoon2");

            // Terre
            float earthDistance = (earth.Position - sun.Position).Length();
            float earthSpeed = SpeedCalculation(G, sun.Mass, earthDistance);
            Console.WriteLine($"Earth distance from Sun: {earthDistance}");
            Console.WriteLine($"Calculated Earth orbital speed: {earthSpeed}");

            Vector3 earthDirection = Vector3.Normalize(earth.Position - sun.Position);
            Vector3 earthPerpendicular = Vector3.Normalize(Vector3.Cross(earthDirection, Vector3.UnitY));
            earth.Velocity = earthPerpendicular * earthSpeed;

            Console.WriteLine($"Earth direction: {earthDirection}");
            Console.WriteLine($"Earth perpendicular: {earthPerpendicular}");
            Console.WriteLine($"Earth final velocity: {earth.Velocity}");

            // Lune
            float moonDistance = (moon.Position - earth.Position).Length();
            float moonSpeed = SpeedCalculation(G, earth.Mass, moonDistance);
            Console.WriteLine($"Moon distance from Earth: {moonDistance}");
            Console.WriteLine($"Calculated Moon orbital speed: {moonSpeed}");

            Vector3 moonDirection = Vector3.Normalize(moon.Position - earth.Position);
            Vector3 moonPerpendicular = Vector3.Normalize(Vector3.Cross(moonDirection, Vector3.UnitY));
            moon.Velocity = earth.Velocity + moonPerpendicular * moonSpeed;

            Console.WriteLine($"Moon direction: {moonDirection}");
            Console.WriteLine($"Moon perpendicular: {moonPerpendicular}");
            Console.WriteLine($"Moon final velocity: {moon.Velocity}");

            // Jupiter
            float jupiterDistance = (jupiter.Position - sun.Position).Length();
            float jupiterSpeed = SpeedCalculation(G, sun.Mass, jupiterDistance);
            Console.WriteLine($"Jupiter distance from Sun: {jupiterDistance}");
            Console.WriteLine($"Calculated Jupiter orbital speed: {jupiterSpeed}");

            Vector3 jupiterDirection = Vector3.Normalize(jupiter.Position - sun.Position);
            Vector3 jupiterPerpendicular = Vector3.Normalize(Vector3.Cross(jupiterDirection, Vector3.UnitY));
            jupiter.Velocity = jupiterPerpendicular * jupiterSpeed;

            Console.WriteLine($"Jupiter direction: {jupiterDirection}");
            Console.WriteLine($"Jupiter perpendicular: {jupiterPerpendicular}");
            Console.WriteLine($"Jupiter final velocity: {jupiter.Velocity}");

            // Lune 1 de Jupiter
            float jMoon1Distance = (jMoon1.Position - jupiter.Position).Length();
            float jMoon1Speed = SpeedCalculation(G, jupiter.Mass, jMoon1Distance);
            Console.WriteLine($"Moon distance from Earth: {jMoon1Distance}");
            Console.WriteLine($"Calculated Moon orbital speed: {jMoon1Speed}");

            Vector3 jMoon1Direction = Vector3.Normalize(jMoon1.Position - jupiter.Position);
            Vector3 jMoon1Perpendicular = Vector3.Normalize(Vector3.Cross(jMoon1Direction, Vector3.UnitY));
            jMoon1.Velocity = jupiter.Velocity + jMoon1Perpendicular * jMoon1Speed;

            Console.WriteLine($"Moon direction: {jMoon1Direction}");
            Console.WriteLine($"Moon perpendicular: {jMoon1Perpendicular}");
            Console.WriteLine($"Moon final velocity: {jMoon1.Velocity}");

            // Lune 2 de Jupiter
            float jMoon2Distance = (jMoon2.Position - jupiter.Position).Length();
            float jMoon2Speed = SpeedCalculation(G, jupiter.Mass, jMoon2Distance);
            Console.WriteLine($"Moon distance from Earth: {jMoon2Distance}");
            Console.WriteLine($"Calculated Moon orbital speed: {jMoon2Speed}");

            Vector3 jMoon2Direction = Vector3.Normalize(jMoon2.Position - jupiter.Position);
            Vector3 jMoon2Perpendicular = Vector3.Normalize(Vector3.Cross(jMoon2Direction, Vector3.UnitY));
            jMoon2.Velocity = jupiter.Velocity + jMoon2Perpendicular * jMoon2Speed;

            Console.WriteLine($"Moon direction: {jMoon2Direction}");
            Console.WriteLine($"Moon perpendicular: {jMoon2Perpendicular}");
            Console.WriteLine($"Moon final velocity: {jMoon2.Velocity}");

            // velocitiesInitialized = true;
        }
    }
}
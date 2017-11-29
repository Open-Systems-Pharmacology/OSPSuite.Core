using System;

namespace OSPSuite.Starter.Tasks
{
   public abstract class Calculation
   {
      protected readonly Random _random = new Random();

      public abstract void Seed();

      public abstract float PointFor(float x);
   }

   public class ExponentialDecay : Calculation
   {
      private double _k;
      private double _c;

      public override void Seed()
      {
         _c = _random.Next(0, 100) / 100.0;
         _k = _random.Next(0, 100) / 100.0;
      }

      public override float PointFor(float x)
      {
         return (float)(Math.Pow(Math.E, -_k * x) * _c);
      }
   }

   public class ExponentialGrowth : Calculation
   {
      private double _k;

      public override void Seed()
      {
         _k = _random.Next(0, 100) / 100.0;
      }

      public override float PointFor(float x)
      {
         return (float)Math.Pow(Math.E, _k * x);
      }
   }

   public class GaussianCalculation : Calculation
   {
      private int _c;
      private double _b;

      public override void Seed()
      {
         _c = _random.Next(0, 200) / 100;
         _b = 2 + _random.Next(-100, 100) / 100.0;
      }

      public override float PointFor(float x)
      {
         return (float)gaussian(x, _b, _c);
      }

      private double gaussian(float x, double b, double c)
      {
         var d = (x - c) / b;
         return Math.Pow(Math.E, -Math.Pow(d, 2));
      }
   }
}

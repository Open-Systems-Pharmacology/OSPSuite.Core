using System;

namespace OSPSuite.Core.Maths.Random
{
   public class RandomGenerator
   {
      private readonly System.Random _random;

      //indicates if an extra deviates was already calculated
      bool _hasAnotherDeviate = false;
      
      //The other deviate calculated using the Box-Muller transformation in NormalDeviate
		double _otherGausianDeviate;

      public RandomGenerator(): this(new System.Random())
      {
      }
      public RandomGenerator(int seed): this(new System.Random(seed))
      {
      }
      public RandomGenerator(System.Random random)
      {
         _random = random;
      }

      /// <summary>
      /// returns a normaly distributed deviate with zero mean and unit variance, using the random generator given as parameter
      /// as the source of uniform deviates
      /// Adapted from Numerical Recipe: Normal (Gaussian) Deviates
      /// </summary>
      public double NormalDeviate()
      {
			double rsq,v1,v2;
			if  (_hasAnotherDeviate)
			{
            //we have an extra deviate handy. Reset the flag and return it
            _hasAnotherDeviate = false;
            return _otherGausianDeviate;
			}

         do 
			{
            v1 = UniformDeviate(-1, 1);      //pick two uniform number 
            v2 = UniformDeviate(-1, 1);      //in the square extending from -1 to +1
				rsq=v1*v1+v2*v2;                 //see if theyare in the unit circle
			} 
         while (rsq >= 1.0 || rsq == 0.0);
			
         //now make the box-muller transformation to get two normal deviates. 
         double fac=Math.Sqrt(-2.0*Math.Log(rsq)/rsq);

         //Return one and save one for next time
			_otherGausianDeviate=v1*fac;
         _hasAnotherDeviate = true;
			return v2*fac;
	   }

      public double UniformDeviate(double min, double  max)
      {
         return (max - min)*_random.NextDouble() + min;
      }

      /// <summary>
      /// Returns a random number between 0 and 1
      /// </summary>
      public double NextDouble()
      {
         return _random.NextDouble();
      }

      /// <summary>
      /// Returns a random integer number betwen [min, max]
      /// </summary>
      /// <param name="min">The inclusive min number</param>
      /// <param name="max">The inclusive max number</param>
      public int NextInteger(int min, int max)
      {
         return _random.Next(min, max + 1);
      }
   }
}
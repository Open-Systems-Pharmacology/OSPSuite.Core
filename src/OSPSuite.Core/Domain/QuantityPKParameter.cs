using System;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain
{
   public class QuantityPKParameter : IWithDimension, IWithName
   {
      /// <summary>
      ///    The values. One item for each individual
      /// </summary>
      public virtual float[] Values { get; private set; }

      /// <summary>
      ///    Path of underlying quantity for which pk-analyses were performed
      /// </summary>
      public virtual string QuantityPath { get; set; }

      /// <summary>
      ///    Dimension of pk parameter
      /// </summary>
      public virtual IDimension Dimension { get; set; }

      /// <summary>
      ///    Name of PK Output (Cmax, Tmax etc...)
      /// </summary>
      public virtual string Name { get; set; }

      public QuantityPKParameter()
      {
         Values = Array.Empty<float>();
      }

      public override string ToString()
      {
         return Id;
      }

      /// <summary>
      ///    Set the pkValue for the individual with id <paramref name="individualId" />
      /// </summary>
      public virtual void SetValue(int individualId, float pkValue)
      {
         if (Count <= individualId)
            throw new OSPSuiteException(Error.IndividualIdDoesNotMatchTheValueLength(individualId, Count));

         Values[individualId] = pkValue;
      }

      public virtual void SetNumberOfIndividuals(int numberOfIndividual)
      {
         Values = new float[numberOfIndividual].InitializeWith(float.NaN);
      }

      /// <summary>
      ///    Id representing the PK parameter uniquely in the simulation hierarchy
      /// </summary>
      public virtual string Id => CreateId(QuantityPath, Name);

      public virtual int Count => Values.Length;

      public static string CreateId(string quantityPath, string pkParameterName)
      {
         return new[] {quantityPath, pkParameterName}.ToPathString();
      }
   }
}
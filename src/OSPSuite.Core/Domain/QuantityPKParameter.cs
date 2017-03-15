using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public class QuantityPKParameter : IWithDimension, IWithName
   {
      /// <summary>
      ///    The values. One item for each individual
      /// </summary>
      public virtual List<float> Values { get; private set; }

      /// <summary>
      ///    Path of underlying quantity for which pkanalyses were performed
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
         Values = new List<float>();
      }

      public override string ToString()
      {
         return Id;
      }

      /// <summary>
      ///    Set the pkValue for the individual with id <paramref name="indiviudalId" />
      /// </summary>
      public virtual void SetValue(int indiviudalId, float pkValue)
      {
         if (Values.Count <= indiviudalId)
            throw new OSPSuiteException(Error.IndividualIdDoesNotMatchTheValueLength(indiviudalId, Values.Count));

         Values[indiviudalId] = pkValue;
      }

      public virtual void SetNumberOfIndividuals(int numberOfIndividal)
      {
         Values = new List<float>(new float[numberOfIndividal].InitializeWith(float.NaN));
      }

      /// <summary>
      ///    Id representing the PK parameter uniquely in the simulation hierarchy
      /// </summary>
      public virtual string Id => CreateId(QuantityPath, Name);

      public virtual int Count => Values.Count;

      public static string CreateId(string quantityPath, string pkParameterName)
      {
         return new[] { quantityPath, pkParameterName }.ToPathString();
      }
   }
}
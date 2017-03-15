using System.Collections.Generic;

namespace OSPSuite.Core.Importer
{
   public static class DimensionHelper
   {
      /// <summary>
      ///    This methods checks whether the dimensions are well defined.
      /// </summary>
      /// <param name="dimensions">List of dimension structs to be checked.</param>
      /// <returns>True, if check was succesfull.</returns>
      public static bool Check(IList<Dimension> dimensions)
      {
         var dimfound = false;
         foreach (var dimension in dimensions)
         {
            if (string.IsNullOrEmpty(dimension.Name)) throw new MissingDimensionName();
            dimension.Check();
            if (!dimension.IsDefault) continue;
            if (dimfound)
               throw new MultipleDefaultDimensionsFound();
            dimfound = true;
         }
         if (!dimfound) throw new MissingDefaultDimension();
         return true;
      }

      /// <summary>
      ///    This method clones a list of dimensions.
      /// </summary>
      /// <param name="sourceDimensions">List of dimensions to clone.</param>
      /// <returns>Cloned list of dimensions.</returns>
      public static IList<Dimension> Clone(IList<Dimension> sourceDimensions)
      {
         var newDimensions = new List<Dimension>();
         for (var i = 0; i < sourceDimensions.Count; i++)
         {
            newDimensions.Add(new Dimension());
            newDimensions[i].Name = sourceDimensions[i].Name;
            newDimensions[i].DisplayName = sourceDimensions[i].DisplayName;
            newDimensions[i].IsDefault = sourceDimensions[i].IsDefault;
            if (sourceDimensions[i].Units != null)
               newDimensions[i].Units = new List<Unit>(sourceDimensions[i].Units);
            if (sourceDimensions[i].InputParameters != null)
               newDimensions[i].InputParameters = new List<InputParameter>(sourceDimensions[i].InputParameters);
            if (sourceDimensions[i].MetaDataConditions != null)
               newDimensions[i].MetaDataConditions = new Dictionary<string, string>(sourceDimensions[i].MetaDataConditions);
         }
         return newDimensions;
      }

      /// <summary>
      ///    This method find a dimension by given name in a list of dimensions.
      /// </summary>
      /// <param name="dimensions">List of dimensions.</param>
      /// <param name="name">Name to search for.</param>
      /// <returns>Dimension with requested name.</returns>
      public static Dimension FindDimension(IList<Dimension> dimensions, string name)
      {
         foreach (var dimension in dimensions)
         {
            if (dimension.Name == name) return dimension;
         }
         throw new DimensionNotFound(name);
      }

      /// <summary>
      ///    This methods get the default dimension from a list of dimensions.
      /// </summary>
      /// <param name="dimensions">List of dimensions.</param>
      /// <returns>Default dimension.</returns>
      public static Dimension GetDefaultDimension(IList<Dimension> dimensions)
      {
         foreach (var dimension in dimensions)
         {
            if (!dimension.IsDefault) continue;
            return dimension;
         }
         throw new MissingDefaultDimension();
      }

      /// <summary>
      ///    This method copies the values of same input parameters from source dimension to target dimension.
      /// </summary>
      /// <param name="source">Dimension which input parameter values are copied.</param>
      /// <param name="target">Dimension which input parameters will be set.</param>
      public static void TakeOverInputParameters(Dimension source, Dimension target)
      {
         if (target == null) return;
         if (target.InputParameters == null) return;
         if (source == null) return;
         if (source.InputParameters == null) return;
         for (var i = 0; i < target.InputParameters.Count; i++)
         {
            var targetParameter = target.InputParameters[i];
            for (var k = 0; k < source.InputParameters.Count; k++)
            {
               var sourceParameter = source.InputParameters[k];
               if (targetParameter != sourceParameter) continue;
               targetParameter.Value = sourceParameter.Value;
               target.InputParameters[i] = targetParameter;
            }
         }
      }
   }
}
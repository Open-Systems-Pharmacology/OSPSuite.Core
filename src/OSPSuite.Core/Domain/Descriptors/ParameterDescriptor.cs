using System;

namespace OSPSuite.Core.Domain.Descriptors
{
   /// <summary>
   ///    Describes a parameter location (criteria on container and parameter )
   /// </summary>
   public class ParameterDescriptor
   {
      /// <summary>
      ///    Container criteria describing the container where the parameter resides
      /// </summary>
      public DescriptorCriteria ContainerCriteria { get; }

      /// <summary>
      ///    Parameter criteria describing the parameter itself
      /// </summary>
      public string ParameterName { get; }

      [Obsolete("For serialization")]
      public ParameterDescriptor()
      {
      }

      public ParameterDescriptor(string parameterName, DescriptorCriteria containerCriteria)
      {
         ContainerCriteria = containerCriteria;
         ParameterName = parameterName;
      }

      public ParameterDescriptor Clone()
      {
         return new ParameterDescriptor(ParameterName, ContainerCriteria.Clone());
      }
   }
}
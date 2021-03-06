﻿using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationEngineFactory
   {
      IParameterIdentificationEngine Create();
   }

   class ParameterIdentificationEngineFactory : DynamicFactory<IParameterIdentificationEngine>, IParameterIdentificationEngineFactory
   {
      public ParameterIdentificationEngineFactory(Utility.Container.IContainer container) : base(container)
      {
      }
   }
}
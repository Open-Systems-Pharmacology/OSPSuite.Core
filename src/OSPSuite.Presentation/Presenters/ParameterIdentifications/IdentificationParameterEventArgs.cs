using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public class IdentificationParameterEventArgs : EventArgs
   {
      public IdentificationParameter IdentificationParameter { get; }

      public IdentificationParameterEventArgs(IdentificationParameter identificationParameter)
      {
         IdentificationParameter = identificationParameter;
      }
   }

   public class ParameterInIdentificationParameterEventArgs : IdentificationParameterEventArgs
   {
      public ParameterSelection LinkedParameter { get;  }

      public ParameterInIdentificationParameterEventArgs(IdentificationParameter identificationParameter, ParameterSelection linkedParameter) : base(identificationParameter)
      {
         LinkedParameter = linkedParameter;
      }
   }
}
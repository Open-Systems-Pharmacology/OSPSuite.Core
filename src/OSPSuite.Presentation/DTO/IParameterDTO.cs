using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.DTO
{
   public interface IParameterDTO : IWithDisplayUnitDTO, IWithName, IPathRepresentableDTO, IWithDescription, IWithValueOrigin
   {
      /// <summary>
      ///    Returns the value in display unit
      /// </summary>
      double Value { get; set; }

      IParameter Parameter { get; }

      double KernelValue { get; }

      bool IsFavorite { get; set; }

      bool IsDiscrete { get; }

      ICache<double, string> ListOfValues { get; }

      event EventHandler ValueChanged;

      string DisplayName { get; set; }

      FormulaType FormulaType { get; set; }

      int Sequence { get; set; }

      double Percentile { get; set; }

      bool Editable { get; }

      /// <summary>
      ///    Releases possible event handler on the underlying parameter
      /// </summary>
      void Release();
   }
}
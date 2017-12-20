using System;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Presentation.DTO
{
   public interface IParameterDTO : IWithDisplayUnitDTO, IWithName, IPathRepresentableDTO, IWithDescription
   {
      /// <summary>
      ///    Returns the value in display unit
      /// </summary>
      double Value { get; set; }

      IParameter Parameter { get; }

      double KernelValue { get; }

      bool IsFavorite { get; set; }

      ValueOrigin ValueOrigin { get; }

      bool IsDiscrete { get; }

      ICache<double, string> ListOfValues { get; }

      event EventHandler ValueChanged;

      string DisplayName { get; set; }

      FormulaType FormulaType { get; set; }

      int Sequence { get; set; }

      double Percentile { get; set; }

      bool Editable { get; }

      /// <summary>
      /// Releases possible event handler on the underlying parameter
      /// </summary>
      void Release();
   }
}
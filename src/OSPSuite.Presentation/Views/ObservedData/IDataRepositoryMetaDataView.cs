using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.ObservedData;

namespace OSPSuite.Presentation.Views.ObservedData
{
   public interface IDataRepositoryMetaDataView : IView<IDataRepositoryMetaDataPresenter>
   {
      void BindToMetaData(IEnumerable<MetaDataDTO> metaData);
      void BindToMolWeight(IParameter molWeightParameter);
      bool MolWeightVisible { get;set; }
      bool MolWeightEditable { get; set; }
      void BindToLLOQ(IParameter lowerLimitsOfQuantification);
   }
}
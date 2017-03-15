using System;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Views.SensitivityAnalyses;

namespace OSPSuite.Presentation.Presenters.SensitivityAnalyses
{
   public interface ISensitivityAnalysisSetValuePresenter : IPresenter<ISensitivityAnalysisSetValueView>
   {
      Action<IParameterDTO> ApplyAll { get; set; }
      Action<IParameterDTO> ApplySelection { get; set; }
      IParameterDTO ParameterDTO { get; }
      void Edit(IParameter parameterToEdit);
      void SetParameterValue(IParameterDTO parameterDTO, double newValue);
      void ConfigureForUInt();
   }

   public class SensitivityAnalysisSetValuePresenter : AbstractPresenter<ISensitivityAnalysisSetValueView, ISensitivityAnalysisSetValuePresenter>, ISensitivityAnalysisSetValuePresenter
   {
      private readonly IParameterToParameterDTOMapper _parameterToParameterDTOMapper;
      public Action<IParameterDTO> ApplyAll { set; get; } = x => { };
      public Action<IParameterDTO> ApplySelection { set; get; } = x => { };

      public SensitivityAnalysisSetValuePresenter(ISensitivityAnalysisSetValueView view, IParameterToParameterDTOMapper parameterToParameterDTOMapper) : base(view)
      {
         _parameterToParameterDTOMapper = parameterToParameterDTOMapper;
      }

      public IParameterDTO ParameterDTO { get; private set; }

      public void Edit(IParameter parameterToEdit)
      {
         ParameterDTO = _parameterToParameterDTOMapper.MapFrom(parameterToEdit);
         _view.BindTo(ParameterDTO);
      }

      public void SetParameterValue(IParameterDTO parameterDTO, double newValue)
      {
         parameterDTO.Parameter.ValueInDisplayUnit = newValue;
      }

      public void ConfigureForUInt()
      {
         _view.ConfigureForUInt();
      }
   }
}

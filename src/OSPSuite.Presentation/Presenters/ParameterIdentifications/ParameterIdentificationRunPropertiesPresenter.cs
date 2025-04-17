using System;
using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Formatters;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationRunPropertiesPresenter : IPresenter<IParameterIdentificationRunPropertiesView>
   {
      void Edit(ParameterIdentificationRunResult runResult);
   }

   public class ParameterIdentificationRunPropertiesPresenter : AbstractPresenter<IParameterIdentificationRunPropertiesView, IParameterIdentificationRunPropertiesPresenter>, IParameterIdentificationRunPropertiesPresenter
   {
      private readonly List<IRunPropertyDTO> _allProperties;

      public ParameterIdentificationRunPropertiesPresenter(IParameterIdentificationRunPropertiesView view) : base(view)
      {
         _allProperties = new List<IRunPropertyDTO>();
      }

      public void Edit(ParameterIdentificationRunResult runResult)
      {
         _allProperties.Clear();
         mapProperties(runResult);
         _view.BindTo(_allProperties);
      }

      private void mapProperties(ParameterIdentificationRunResult runResult)
      {
         var icon = ApplicationIcons.IconFor(runResult.Status);
         _allProperties.Add(new RunPropertyDTO<double>(Captions.ParameterIdentification.TotalError, runResult.TotalError, new DoubleFormatter()));
         _allProperties.Add(new RunPropertyDTO<int>(Captions.ParameterIdentification.NumberOfEvaluations, runResult.NumberOfEvaluations, new IntFormatter()));
         _allProperties.Add(new RunPropertyDTO<TimeSpan>(Captions.ParameterIdentification.Duration, runResult.Duration, new TimeSpanFormatter()));
         _allProperties.Add(new RunPropertyDTO<RunStatus>(Captions.ParameterIdentification.Status, runResult.Status, icon: icon));
         _allProperties.Add(new RunPropertyDTO<DateTime>(Captions.ParameterIdentification.CompletedDate, runResult.DateTimeCompleted, new DateTimeFormatter(displayTime: true)));

         if (!string.IsNullOrEmpty(runResult.Message))
            _allProperties.Add(new RunPropertyDTO<string>(Captions.ParameterIdentification.RunMessage, runResult.Message, icon: icon));
      }
   }
}
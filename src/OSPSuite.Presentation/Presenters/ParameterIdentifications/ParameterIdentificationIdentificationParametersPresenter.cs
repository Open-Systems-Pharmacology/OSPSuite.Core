using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationIdentificationParametersPresenter : IPresenter<IParameterIdentificationIdentificationParametersView>, IAbstractParameterSelectionPresenter,IParameterIdentificationPresenter
   {
      void RemoveIdentificationParameter(IdentificationParameterDTO identificationParameterDTO);
      void SelectIdentificationParameter(IdentificationParameterDTO identificationParameterDTO);
      event EventHandler<IdentificationParameterEventArgs> IdentificationParameterSelected;
      event EventHandler NoIdentificationParameterSelected;
      void Refresh();
      void UseAsFactorChanged(IdentificationParameterDTO identificationParameterDTO);
      void UpdateStartValueFromSimulation(IdentificationParameterDTO identificationParameterDTO);
      void ChangeName(IdentificationParameterDTO identificationParameterDTO, string oldName, string newName);
   }

   public class ParameterIdentificationIdentificationParametersPresenter : AbstractParameterSelectionPresenter<IParameterIdentificationIdentificationParametersView, IParameterIdentificationIdentificationParametersPresenter>, IParameterIdentificationIdentificationParametersPresenter
   {
      private readonly IIdentificationParameterFactory _identificationParameterFactory;
      private readonly IIdentificationParameterToIdentificationParameterDTOMapper _identificationParameterDTOMapper;
      private readonly IIdentificationParameterTask _identificationParameterTask;
      private ParameterIdentification _parameterIdentification;
      private readonly List<IdentificationParameterDTO> _allIdentificationParameterDTOs = new List<IdentificationParameterDTO>();
      public event EventHandler<IdentificationParameterEventArgs> IdentificationParameterSelected = delegate { };
      public event EventHandler NoIdentificationParameterSelected = delegate { };

      public ParameterIdentificationIdentificationParametersPresenter(IParameterIdentificationIdentificationParametersView view, IIdentificationParameterFactory identificationParameterFactory,
         IIdentificationParameterToIdentificationParameterDTOMapper identificationParameterDTOMapper, IIdentificationParameterTask identificationParameterTask) : base(view)
      {
         _identificationParameterFactory = identificationParameterFactory;
         _identificationParameterDTOMapper = identificationParameterDTOMapper;
         _identificationParameterTask = identificationParameterTask;
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _parameterIdentification = parameterIdentification;
         updateView();
      }

      public override void AddParameters(IReadOnlyList<ParameterSelection> parameters)
      {
         var identificationParameter = _identificationParameterFactory.CreateFor(parameters, _parameterIdentification);
         if (identificationParameter == null)
            return;

         _parameterIdentification.AddIdentificationParameter(identificationParameter);
         updateView();
         selectIdentificationParameter(identificationParameter);
         ViewChanged();
      }

      private IdentificationParameterDTO dtoFrom(IdentificationParameter identificationParameter)
      {
         return _allIdentificationParameterDTOs.Find(x => Equals(x.IdentificationParameter, identificationParameter));
      }

      public void RemoveIdentificationParameter(IdentificationParameterDTO identificationParameterDTO)
      {
         _parameterIdentification.RemoveIdentificationParameter(identificationParameterDTO.IdentificationParameter);
         updateView();
         selectIdentificationParameter(_parameterIdentification.AllIdentificationParameters.FirstOrDefault());
         ViewChanged();
      }

      public void SelectIdentificationParameter(IdentificationParameterDTO identificationParameterDTO)
      {
         IdentificationParameterSelected(this, new IdentificationParameterEventArgs(identificationParameterDTO.IdentificationParameter));
      }

      private void selectIdentificationParameter(IdentificationParameter identificationParameter)
      {
         if (identificationParameter == null)
            clearSelection();
         else
         {
            var selectedDTO = dtoFrom(identificationParameter);
            View.SelectedIdentificationParameter = selectedDTO;
            SelectIdentificationParameter(selectedDTO);
         }
      }

      private void clearSelection()
      {
         NoIdentificationParameterSelected(this, EventArgs.Empty);
      }

      public void Refresh()
      {
         updateView();
      }

      public void UseAsFactorChanged(IdentificationParameterDTO identificationParameterDTO)
      {
         _identificationParameterTask.UpdateParameterRange(identificationParameterDTO.IdentificationParameter);
         updateView();
      }

    
      public void UpdateStartValueFromSimulation(IdentificationParameterDTO identificationParameterDTO)
      {
         _identificationParameterTask.UpdateStartValuesFromSimulation(identificationParameterDTO.IdentificationParameter);
         updateView();
      }

      public void ChangeName(IdentificationParameterDTO identificationParameterDTO, string oldName, string newName)
      {
         identificationParameterDTO.IdentificationParameter.Name = newName;
         SelectIdentificationParameter(identificationParameterDTO);
      }

      private void updateView()
      {
         _allIdentificationParameterDTOs.Clear();
         _allIdentificationParameterDTOs.AddRange(_parameterIdentification.AllIdentificationParameters.MapAllUsing(_identificationParameterDTOMapper));
         _view.BindTo(_allIdentificationParameterDTOs);
      }
   }
}
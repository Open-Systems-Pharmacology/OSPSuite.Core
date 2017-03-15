using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationLinkedParametersPresenter : IPresenter<IParameterIdentificationLinkedParametersView>, IParameterIdentificationPresenter
   {
      void Edit(IdentificationParameter identificationParameter);
      void RemoveParameter(LinkedParameterDTO linkedParameterDTO);
      void UnlinkParameter(LinkedParameterDTO linkedParameterDTO);
      event EventHandler<ParameterInIdentificationParameterEventArgs> ParameterRemovedFromIdentificationParameter;
      event EventHandler<ParameterInIdentificationParameterEventArgs> ParameterUnlinkedFromIdentificationParameter;
      event EventHandler<ParameterInIdentificationParameterEventArgs> ParameterLinkedToIdentificationParameter;
      bool CanRemove(LinkedParameterDTO linkedParameterDTO);
      bool CanUnlink(LinkedParameterDTO linkedParameterDTO);
      void ClearSelection();
      void AddLinkedParameters(IReadOnlyList<ParameterSelection> parameterSelections);
      void Refresh();
   }

   public class ParameterIdentificationLinkedParametersPresenter : AbstractPresenter<IParameterIdentificationLinkedParametersView, IParameterIdentificationLinkedParametersPresenter>, IParameterIdentificationLinkedParametersPresenter
   {
      private readonly ISimulationQuantitySelectionToLinkedParameterDTOMapper _linkedParameterDTOMapper;
      private readonly List<LinkedParameterDTO> _allLinkedParameters;
      private IdentificationParameter _identificationParameter;
      public event EventHandler<ParameterInIdentificationParameterEventArgs> ParameterRemovedFromIdentificationParameter = delegate { };
      public event EventHandler<ParameterInIdentificationParameterEventArgs> ParameterUnlinkedFromIdentificationParameter = delegate { };
      public event EventHandler<ParameterInIdentificationParameterEventArgs> ParameterLinkedToIdentificationParameter = delegate { };

      public ParameterIdentificationLinkedParametersPresenter(IParameterIdentificationLinkedParametersView view, ISimulationQuantitySelectionToLinkedParameterDTOMapper linkedParameterDTOMapper) : base(view)
      {
         _linkedParameterDTOMapper = linkedParameterDTOMapper;
         _allLinkedParameters = new List<LinkedParameterDTO>();
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
      }

      public void Edit(IdentificationParameter identificationParameter)
      {
         _identificationParameter = identificationParameter;
         Refresh();
      }

      public void Refresh()
      {
         _allLinkedParameters.Clear();
         _allLinkedParameters.AddRange(allExistingLinkedParametersFrom(_identificationParameter));

         rebind();
         EnumHelper.AllValuesFor<PathElement>().Each(updateColumnVisibility);
      }

      private IEnumerable<LinkedParameterDTO> allExistingLinkedParametersFrom(IdentificationParameter identificationParameter)
      {
         if (identificationParameter == null)
            return Enumerable.Empty<LinkedParameterDTO>();

         return _identificationParameter.AllLinkedParameters.MapAllUsing(_linkedParameterDTOMapper);
      }

      private void rebind()
      {
         _view.BindTo(_allLinkedParameters);
      }

      private void updateColumnVisibility(PathElement pathElement)
      {
         _view.SetVisibility(pathElement, !_allLinkedParameters.HasOnlyEmptyValuesAt(pathElement));
      }

      public void RemoveParameter(LinkedParameterDTO linkedParameterDTO)
      {
         var linkedParameter = removeLinkedParameter(linkedParameterDTO);
         if (linkedParameter == null) return;
         ParameterRemovedFromIdentificationParameter(this, new ParameterInIdentificationParameterEventArgs(_identificationParameter, linkedParameter));
      }

      private ParameterSelection removeLinkedParameter(LinkedParameterDTO linkedParameterDTO)
      {
         var linkedParameter = linkedParameterFor(linkedParameterDTO);
         if (linkedParameter == null)
            return null;

         _identificationParameter.RemovedLinkedParameter(linkedParameterDTO.Quantity);
         Refresh();
         return linkedParameter;
      }

      public void UnlinkParameter(LinkedParameterDTO linkedParameterDTO)
      {
         var linkedParameter = removeLinkedParameter(linkedParameterDTO);
         if (linkedParameter == null) return;
         ParameterUnlinkedFromIdentificationParameter(this, new ParameterInIdentificationParameterEventArgs(_identificationParameter, linkedParameter));
      }

      private ParameterSelection linkedParameterFor(LinkedParameterDTO linkedParameterDTO)
      {
         return _identificationParameter.LinkedParameterFor(linkedParameterDTO.Quantity);
      }

      public bool CanRemove(LinkedParameterDTO linkedParameterDTO)
      {
         return _identificationParameter.AllLinkedParameters.Count > 1;
      }

      public bool CanUnlink(LinkedParameterDTO linkedParameterDTO)
      {
         return _identificationParameter.AllLinkedParameters.Count > 1;
      }

      public void ClearSelection()
      {
         _identificationParameter = null;
         _allLinkedParameters.Clear();
         rebind();
      }

      public void AddLinkedParameters(IReadOnlyList<ParameterSelection> parameterSelections)
      {
         if (_identificationParameter == null)
            return;

         Do.Action(() => parameterSelections.Each(addParameterToIdentificationParameter))
            .ThenFinally(Refresh);
      }

      private void addParameterToIdentificationParameter(ParameterSelection parameterSelection)
      {
         _identificationParameter.AddLinkedParameter(parameterSelection);
         ParameterLinkedToIdentificationParameter(this, new ParameterInIdentificationParameterEventArgs(_identificationParameter, parameterSelection));
      }
   }
}
using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views.ObservedData;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public interface IDataRepositoryMetaDataPresenter : IDataRepositoryItemPresenter, IPresenter<IDataRepositoryMetaDataView>,
      IListener<ObservedDataMetaDataAddedEvent>,
      IListener<ObservedDataMetaDataChangedEvent>,
      IListener<ObservedDataMetaDataRemovedEvent>
   {
      /// <summary>
      ///    Adds a new row to the view, but does not affect the repository meta data
      /// </summary>
      void NewMetaDataAdded();

      /// <summary>
      ///    Changes a value of a meta data name/value pair in the data repository
      ///    If the data does not exist in the repository it is added
      /// </summary>
      /// <param name="metaDataDTO">The new meta data name/value pair</param>
      /// <param name="oldValue">The old value of the meta data</param>
      void MetaDataValueChanged(MetaDataDTO metaDataDTO, string oldValue);

      /// <summary>
      ///    Changes the name (or key) of a meta data name/value pair in the data repository
      ///    It will not add data to the repository if the key is not found
      /// </summary>
      /// <param name="metaDataDTO">The new meta data name/value pair</param>
      /// <param name="oldName">The old name of the meta data</param>
      void MetaDataNameChanged(MetaDataDTO metaDataDTO, string oldName);

      /// <summary>
      ///    Removes the meta data from the observed data repository being edited
      /// </summary>
      void RemoveMetaData(MetaDataDTO metaDataDTO);

      /// <summary>
      ///    Edits the metadata on multiple data repositories at once
      /// </summary>
      /// <param name="dataRepositories">The repositories to edit</param>
      void EditObservedData(IEnumerable<DataRepository> dataRepositories);

      void SetMolWeight(double oldMolWeightValueInDisplayUnit, double molWeightValueInDisplayUnit);
   }

   public class DataRepositoryMetaDataPresenter : AbstractSubPresenter<IDataRepositoryMetaDataView, IDataRepositoryMetaDataPresenter>,
      IDataRepositoryMetaDataPresenter, ILatchable
   {
      protected IEnumerable<DataRepository> _allDataRepositories;
      private NotifyList<MetaDataDTO> _metaDataDTOList;
      private readonly IEditObservedDataTask _editObservedDataTask;
      private readonly IObservedDataConfiguration _observedDataConfiguration;
      private readonly IParameterFactory _parameterFactory;
      private IEnumerable<IBusinessRule> _defaultRules;
      private readonly IDimension _molWeightDimension;
      public bool IsLatched { get; set; }

      public DataRepositoryMetaDataPresenter(IDataRepositoryMetaDataView view, IEditObservedDataTask editObservedDataTask,
         IObservedDataConfiguration observedDataConfiguration, IParameterFactory parameterFactory, IDimensionFactory dimensionFactory)
         : base(view)
      {
         _editObservedDataTask = editObservedDataTask;
         _observedDataConfiguration = observedDataConfiguration;
         _parameterFactory = parameterFactory;
         _molWeightDimension = dimensionFactory.Dimension(Constants.Dimension.MOLECULAR_WEIGHT);
      }

      /// <summary>
      ///    Binds the <paramref name="observedData" /> with the view
      /// </summary>
      /// <param name="observedData">The data repository that will be edited</param>
      public void EditObservedData(DataRepository observedData)
      {
         editObservedData(new List<DataRepository> {observedData}, MetaDataDTO.MetaDataDTORules.All());
      }

      public void EditObservedData(IEnumerable<DataRepository> observedData)
      {
         editObservedData(observedData, MetaDataDTO.MetaDataDTORules.AllForMultipleEdits());
      }

      private void editObservedData(IEnumerable<DataRepository> observedData, IEnumerable<IBusinessRule> rules)
      {
         _defaultRules = rules;
         _allDataRepositories = observedData;
         rebind();
      }

      private MetaDataDTO createDTO(IExtendedProperty extendedProperty)
      {
         var name = extendedProperty.Name;
         var values = _observedDataConfiguration.PredefinedValuesFor(name);
         return createDTO(name, extendedProperty.ValueAsObject as string, nameEditable: !isPredefinedProperty(extendedProperty), valueReadOnly: isReadOnly(extendedProperty), listOfValues: values);
      }

      private MetaDataDTO createDTO(string name, string value, bool nameEditable = true, bool valueReadOnly = false, IEnumerable<string> listOfValues = null)
      {
         return new MetaDataDTO(_defaultRules)
         {
            Name = name,
            Value = value,
            DataRepositories = _allDataRepositories,
            NameEditable = nameEditable,
            ValueReadOnly = valueReadOnly,
            ListOfValues = listOfValues
         };
      }

      public void NewMetaDataAdded()
      {
         _metaDataDTOList.Add(createDTO(name: string.Empty, value: string.Empty));
      }

      public void MetaDataNameChanged(MetaDataDTO metaDataDTO, string oldName)
      {
         handleThisChange(metaDataNameChanged, metaDataDTO, oldName, oldName);
      }

      public void RemoveMetaData(MetaDataDTO metaDataDTO)
      {
         AddCommand(_editObservedDataTask.RemoveMetaData(_allDataRepositories, mapFrom(metaDataDTO)));
      }

      public IEnumerable<string> PredefinedValuesFor(MetaDataDTO metaDataDTO)
      {
         return _observedDataConfiguration.PredefinedValuesFor(metaDataDTO.Name);
      }

      private bool isPredefinedProperty(IExtendedProperty extendedProperty)
      {
         return _observedDataConfiguration.DefaultMetaDataCategories.Contains(extendedProperty.Name);
      }

      private MetaDataKeyValue mapFrom(MetaDataDTO dto)
      {
         return new MetaDataKeyValue {Key = dto.Name, Value = dto.Value};
      }

      public void MetaDataValueChanged(MetaDataDTO metaDataDTO, string oldValue)
      {
         handleThisChange(metaDataValueChanged, metaDataDTO, oldValue, metaDataDTO.Name);
      }

      private void handleThisChange(Action<MetaDataDTO, string> actionToRun, MetaDataDTO metaDataDTO, string oldData, string originalKey)
      {
         if (!metaDataDTO.IsValid())
            return;

         if (_allDataRepositories.All(x => x.ExtendedProperties.Contains(originalKey)))
            actionToRun(metaDataDTO, oldData);
         else
            metaDataAdded(metaDataDTO);
      }

      private void metaDataAdded(MetaDataDTO metaDataDTO)
      {
         AddCommand(_editObservedDataTask.AddMetaData(_allDataRepositories, mapFrom(metaDataDTO)));
      }

      private void metaDataValueChanged(MetaDataDTO metaDataDTO, string oldValue)
      {
         AddCommand(_editObservedDataTask.ChangeMetaData(_allDataRepositories,
            new MetaDataChanged {NewName = metaDataDTO.Name, OldName = metaDataDTO.Name, OldValue = oldValue, NewValue = metaDataDTO.Value}));
      }

      private void metaDataNameChanged(MetaDataDTO metaDataDTO, string oldName)
      {
         AddCommand(_editObservedDataTask.ChangeMetaData(_allDataRepositories,
            new MetaDataChanged {NewName = metaDataDTO.Name, OldName = oldName, OldValue = metaDataDTO.Value, NewValue = metaDataDTO.Value}));
      }

      public void SetMolWeight(double oldMolWeightValueInDisplayUnit, double molWeightValueInDisplayUnit)
      {
         this.DoWithinLatch(() => { AddCommand(_editObservedDataTask.UpdateMolWeight(_allDataRepositories, molWeightValueInCoreUnit(oldMolWeightValueInDisplayUnit), molWeightValueInCoreUnit(molWeightValueInDisplayUnit))); });
      }

      private double molWeightValueInCoreUnit(double valueInDisplayUnit)
      {
         return _molWeightDimension.UnitValueToBaseUnitValue(_molWeightDimension.DefaultUnit, valueInDisplayUnit);
      }

      /// <summary>
      ///    Rebinds the repository to the view when meta data changes on a repository
      /// </summary>
      public void Handle(ObservedDataMetaDataChangedEvent eventToHandle)
      {
         handleObservedDataEvent(eventToHandle);
      }

      /// <summary>
      ///    Rebinds the repository to the view when meta data is added to the repository
      /// </summary>
      public void Handle(ObservedDataMetaDataAddedEvent eventToHandle)
      {
         handleObservedDataEvent(eventToHandle);
      }

      /// <summary>
      ///    Rebinds the repository to the view when meta data is removed from the repository
      /// </summary>
      public void Handle(ObservedDataMetaDataRemovedEvent eventToHandle)
      {
         handleObservedDataEvent(eventToHandle);
      }

      private void handleObservedDataEvent(ObservedDataEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         rebind();
      }

      private void rebind()
      {
         if (IsLatched) return;
         _metaDataDTOList = new NotifyList<MetaDataDTO>();
         _allDataRepositories.ToList().IntersectingMetaData().Each(x => _metaDataDTOList.Add(createDTO(x)));
         _view.BindToMetaData(_metaDataDTOList);

         _view.MolWeightEditable = _observedDataConfiguration.MolWeightEditable;
         var molWeightParameter = retrieveUniqueMolWeightParameter();
         var shouldBindToMolWeight = molWeightParameter != null && _observedDataConfiguration.MolWeightVisible;
         _view.MolWeightVisible = shouldBindToMolWeight;

         if (shouldBindToMolWeight)
            _view.BindToMolWeight(molWeightParameter);

         var lowerLimitsOfQuantification = retrieveLLOQs().ToList();
         if (lowerLimitsOfQuantification.Count == 1)
            _view.BindToLLOQ(lowerLimitsOfQuantification.First());
      }

      private IEnumerable<IParameter> retrieveLLOQs()
      {
         return _allDataRepositories.SelectMany(x => x.AllButBaseGrid())
            .Where(x => x.DataInfo.LLOQ.HasValue)
            .Select(createParameterForLLOQ)
            .DistinctBy(x => x.Value);
      }

      private IParameter createParameterForLLOQ(DataColumn dataColumn)
      {
         return _parameterFactory.CreateParameter(Captions.LLOQ, dataColumn.DataInfo.LLOQ, dataColumn.Dimension, displayUnit:dataColumn.DisplayUnit);
      }

      private IParameter retrieveUniqueMolWeightParameter()
      {
         var molWeights = _allDataRepositories.SelectMany(x => x.AllButBaseGrid())
            .Select(x => x.DataInfo.MolWeight)
            .Where(x => x.HasValue)
            .Distinct().ToList();

         if (molWeights.Count != 1)
            return null;

         return _parameterFactory.CreateParameter(Constants.Parameters.MOL_WEIGHT, molWeights[0], _molWeightDimension);
      }

      private bool isReadOnly(IExtendedProperty extendedProperty)
      {
         return _observedDataConfiguration.ReadOnlyMetaDataCategories.Contains(extendedProperty.Name);
      }

      private bool canHandle(ObservedDataEvent eventToHandle)
      {
         return _allDataRepositories.Contains(eventToHandle.ObservedData);
      }
   }
}
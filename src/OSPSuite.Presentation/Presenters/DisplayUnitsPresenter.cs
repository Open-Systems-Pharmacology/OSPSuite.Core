using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IDisplayUnitsPresenter : IPresenter<IDisplayUnitsView>
   {
      void Edit(DisplayUnitsManager unitsManager);

      /// <summary>
      ///    Adds a mapping to the <see cref="DisplayUnitsManager" /> being edited
      /// </summary>
      void AddDefaultUnit();

      /// <summary>
      ///    Removes the corresponding <see cref="DisplayUnitMap" /> from the the <see cref="DisplayUnitsManager" /> being edited
      /// </summary>
      void RemoveDefaultUnit(DefaultUnitMapDTO defaultUnitMapDTO);

      /// <summary>
      ///    Returns all dimensions that can be selected for the given <paramref name="defaultUnitMapDTO" />.
      /// </summary>
      IEnumerable<IDimension> AllPossibleDimensionsFor(DefaultUnitMapDTO defaultUnitMapDTO);

      /// <summary>
      ///    Returns the defined units for the given <paramref name="dimension" />
      /// </summary>
      IEnumerable<Unit> AllUnitsFor(IDimension dimension);

      /// <summary>
      ///    Saves the current units to file selected by the user
      /// </summary>
      void SaveUnitsToFile();

      /// <summary>
      ///    Loads the display units from a file selected by the user
      /// </summary>
      void LoadUnitsFromFile();
   }

   public class DisplayUnitsPresenter : AbstractPresenter<IDisplayUnitsView, IDisplayUnitsPresenter>, IDisplayUnitsPresenter
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IDialogCreator _dialogCreator;
      private readonly ISerializationTask _serializationTask;
      private DisplayUnitsManager _unitsManager;
      private readonly ICloneManager _cloneManager;

      public DisplayUnitsPresenter(IDisplayUnitsView view, IDimensionFactory dimensionFactory,
         IDialogCreator dialogCreator, ISerializationTask serializationTask, ICloneManagerForModel cloneManager)
         : base(view)
      {
         _dimensionFactory = dimensionFactory;
         _dialogCreator = dialogCreator;
         _serializationTask = serializationTask;
         _cloneManager = cloneManager;
      }

      public void Edit(DisplayUnitsManager unitsManager)
      {
         _unitsManager = unitsManager;
         bindToView();
      }

      public void AddDefaultUnit()
      {
         _unitsManager.AddDisplayUnit(new DisplayUnitMap());
         bindToView();
      }

      public void RemoveDefaultUnit(DefaultUnitMapDTO defaultUnitMapDTO)
      {
         _unitsManager.RemoveDefaultUnit(defaultUnitMapDTO.Subject);
         bindToView();
      }

      public IEnumerable<IDimension> AllPossibleDimensionsFor(DefaultUnitMapDTO defaultUnitMapDTO)
      {
         var allDimensions = _dimensionFactory.Dimensions.ToList();
         var currentDimension = defaultUnitMapDTO.Dimension;

         //Removes all dimensions already map but the one being currently edited
         _unitsManager.AllDisplayUnits
            .Where(d => !Equals(d.Dimension, currentDimension))
            .Each(d => allDimensions.Remove(d.Dimension));

         return allDimensions;
      }

      public IEnumerable<Unit> AllUnitsFor(IDimension dimension)
      {
         if (dimension == null)
            return Enumerable.Empty<Unit>();

         return dimension.Units;
      }

      private void bindToView()
      {
         var allDtos = _unitsManager.AllDisplayUnits.Select(mapFrom).ToList();
         _view.BindTo(allDtos);
         ViewChanged();
      }

      private DefaultUnitMapDTO mapFrom(DisplayUnitMap displayUnitMap)
      {
         return new DefaultUnitMapDTO(displayUnitMap);
      }

      public void SaveUnitsToFile()
      {
         var filename = _dialogCreator.AskForFileToSave(Captions.SaveUnitsToFile, Constants.Filter.UNITS_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (string.IsNullOrEmpty(filename)) return;
         _serializationTask.SaveModelPart(_unitsManager, filename);
      }

      public void LoadUnitsFromFile()
      {
         var filename = _dialogCreator.AskForFileToOpen(Captions.LoadUnitsFromFile, Constants.Filter.UNITS_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (string.IsNullOrEmpty(filename)) return;
         var displayUnitsFromFile = _serializationTask.Load<DisplayUnitsManager>(filename);
         if (displayUnitsFromFile == null) return;
         _unitsManager.UpdatePropertiesFrom(displayUnitsFromFile, _cloneManager);
         bindToView();
      }
   }
}
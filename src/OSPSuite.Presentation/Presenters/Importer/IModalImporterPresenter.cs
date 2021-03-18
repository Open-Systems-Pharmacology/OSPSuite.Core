using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IModalImporterPresenter : IDisposablePresenter
   {
      (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IImporterPresenter presenter, 
         IReadOnlyList<MetaDataCategory> metaDataCategories, 
         IReadOnlyList<ColumnInfo> columnInfos, 
         DataImporterSettings dataImporterSettings
      );
   }

   public class ModalImporterPresenter : AbstractDisposablePresenter<IModalImporterView, IModalImporterPresenter>, IModalImporterPresenter
   {
      private readonly IDataSetToDataRepositoryMapper _dataRepositoryMapper;

      public ModalImporterPresenter(IModalImporterView view, IDataSetToDataRepositoryMapper dataRepositoryMapper) : base(view)
      {
         _dataRepositoryMapper = dataRepositoryMapper;
      }

      public (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IImporterPresenter presenter, 
         IReadOnlyList<MetaDataCategory> metaDataCategories, 
         IReadOnlyList<ColumnInfo> columnInfos, 
         DataImporterSettings dataImporterSettings
      )
      {
         List<DataRepository> result = new List<DataRepository>();
         _view.FillImporterPanel(presenter.BaseView);
         var id = Guid.NewGuid().ToString();
         presenter.OnTriggerImport += (s, d) =>
         {
            var i = 0;
            foreach (var dataSet in d.DataSource.DataSets.SelectMany(ds => ds.Data))
            {
               var dataRepo = _dataRepositoryMapper.ConvertImportDataSet(d.DataSource.DataSetAt(i++));
               dataRepo.ConfigurationId = id;
               var molecule = dataRepo.ExtendedPropertyValueFor(dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation);
               var moleculeDescription = (metaDataCategories?.FirstOrDefault(md => md.Name == dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation)?.ListOfValues.FirstOrDefault(v => v.Key == dataRepo.ExtendedPropertyValueFor(dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation)))?.Value;
               var molecularWeightDescription = dataRepo.ExtendedPropertyValueFor(dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation);


               if (moleculeDescription != null)
               {
                  if (string.IsNullOrEmpty(molecularWeightDescription))
                  {
                     molecularWeightDescription = moleculeDescription;
                     if (!string.IsNullOrEmpty(dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation))
                     dataRepo.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation, Value = moleculeDescription });
                  }
                  else
                  {
                     double molWeight;
                     double moleculeMolWeight;
                     double.TryParse(moleculeDescription, out moleculeMolWeight);
                     double.TryParse(molecularWeightDescription, out molWeight);
                     if (!ValueComparer.AreValuesEqual(moleculeMolWeight, molWeight))
                        throw new InconsistenMoleculeAndMoleWeightException();
                  }
               }
               if (!string.IsNullOrEmpty(molecularWeightDescription))
               {
                  double molWeight;
                  if (double.TryParse(molecularWeightDescription, out molWeight))
                  {
                     dataRepo.AllButBaseGrid().Each(x => x.DataInfo.MolWeight = molWeight);
                  }
               }
               result.Add(dataRepo);
                  
            }
         };
         var configuration = presenter.GetConfiguration();
         configuration.Id = id;
         _view.AttachImporterPresenter(presenter);
         _view.Display();
         return (result, configuration);
      }
   }
}

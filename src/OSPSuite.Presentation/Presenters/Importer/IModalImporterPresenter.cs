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
         DataImporterSettings dataImporterSettings,
         string configurationId = null
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
         DataImporterSettings dataImporterSettings,
         string configurationId = null
      )
      {
         var result = new List<DataRepository>();
         _view.FillImporterPanel(presenter.BaseView);
         var id = Guid.NewGuid().ToString();
         presenter.OnTriggerImport += (s, d) =>
         {
            for (var i = 0; i < d.DataSource.DataSets.SelectMany(ds => ds.Data).Count(); i++)
            {
               var dataRepo = _dataRepositoryMapper.ConvertImportDataSet(d.DataSource.DataSetAt(i));
               dataRepo.ConfigurationId = id;
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
                     double.TryParse(moleculeDescription, out var moleculeMolWeight);
                     double.TryParse(molecularWeightDescription, out var molWeight);
                     if (!ValueComparer.AreValuesEqual(moleculeMolWeight, molWeight))
                        throw new InconsistenMoleculeAndMoleWeightException();
                  }
               }
               if (!string.IsNullOrEmpty(molecularWeightDescription))
               {
                  if (double.TryParse(molecularWeightDescription, out var molWeight))
                  {
                     dataRepo.AllButBaseGrid().Each(x => x.DataInfo.MolWeight = molWeight);
                  }
               }
               result.Add(dataRepo);
                  
            }
         };
         var configuration = presenter.GetConfiguration();
         configuration.Id = configurationId ?? id;
         _view.AttachImporterPresenter(presenter);
         _view.Display();
         return (result, configuration);
      }
   }
}

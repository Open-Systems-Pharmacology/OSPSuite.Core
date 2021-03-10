using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Presentation.Views.Importer;
using System;
using System.Collections.Generic;
using System.Linq;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IModalImporterPresenter : IDisposablePresenter
   {
      (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(IImporterPresenter presenter, IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings);
   }

   public class ModalImporterPresenter : AbstractDisposablePresenter<IModalImporterView, IModalImporterPresenter>, IModalImporterPresenter
   {
      private readonly IDataSetToDataRepositoryMapper _dataRepositoryMapper;

      public ModalImporterPresenter(IModalImporterView view, IDataSetToDataRepositoryMapper dataRepositoryMapper) : base(view)
      {
         _dataRepositoryMapper = dataRepositoryMapper;
      }

      public (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(IImporterPresenter presenter, IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         List<DataRepository> result = new List<DataRepository>();
         _view.FillImporterPanel(presenter.BaseView);
         var id = Guid.NewGuid().ToString();
         presenter.OnTriggerImport += (s, d) =>
         {
            var i = 0;
            foreach (var pair in d.DataSource.DataSets.KeyValues)
            {
               foreach (var data in pair.Value.Data)
               {
                  var dataRepo = _dataRepositoryMapper.ConvertImportDataSet(d.DataSource, i++, pair.Key);
                  dataRepo.ConfigurationId = id;
                  var moleculeName = "Molecule";
                  var molWeightName = "Molecular Weight";
                  var molecule = dataRepo.ExtendedPropertyValueFor(moleculeName);
                  var moleculeDescription = (metaDataCategories?.FirstOrDefault(md => md.Name == moleculeName)?.ListOfValues.FirstOrDefault(v => v.Key == dataRepo.ExtendedPropertyValueFor(moleculeName)))?.Value;
                  var molecularWeightDescription = dataRepo.ExtendedPropertyValueFor(molWeightName);

                  if (moleculeDescription != null)
                  {
                     if (string.IsNullOrEmpty(molecularWeightDescription))
                     {
                        dataRepo.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = molWeightName, Value = moleculeDescription });
                     }
                     else
                     {
                        if (moleculeDescription != molecularWeightDescription)
                           throw new InconsistenMoleculeAndMoleWeightException();
                     }
                  }

                  result.Add(dataRepo);
               }
                  
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

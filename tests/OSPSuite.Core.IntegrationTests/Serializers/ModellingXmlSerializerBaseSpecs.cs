using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Serializers
{
   public abstract class ModellingXmlSerializerBaseSpecs : XmlSerializerBaseSpecs<IOSPSuiteXmlSerializerRepository>
   {
      protected IDimension DimensionLength { get; private set; }
      protected IDimension DimensionTime { get; private set; }
      protected IDimension DimensionMolarConcentration { get; private set; }
      protected IDimension DimensionMassConcentration { get; private set; }
      protected IDimension DimensionLess { get; private set; }

      private IObjectBaseFactory _objectBaseFactory;

      protected T CreateObject<T>() where T : class, IObjectBase
      {
         return _objectBaseFactory.CreateObjectBaseFrom<T>(typeof(T));
      }

      public override void GlobalContext()
      {
         base.GlobalContext();
         var dimensionFactory = IoC.Resolve<IDimensionFactory>();
         DimensionLength = dimensionFactory.Dimension("Length");
         DimensionTime = dimensionFactory.Dimension(Constants.Dimension.TIME);
         DimensionMolarConcentration = dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         DimensionMassConcentration = dimensionFactory.Dimension(Constants.Dimension.MASS_CONCENTRATION);

         DimensionLess = Constants.Dimension.NO_DIMENSION;
         _objectBaseFactory = IoC.Resolve<IObjectBaseFactory>();
      }
   }

   public abstract class ModellingXmlSerializerWithModelBaseSpecs : ModellingXmlSerializerBaseSpecs
   {
      protected IBuildConfiguration _buildConfiguration;
      protected IObjectPathFactory _objectPathFactory;
      protected IMoleculeStartValuesCreator _moleculeStartValuesCreator;
      protected CreationResult _result;
      protected IModel _model;
      protected IModelCoreSimulation _simulation;
      protected IModelConstructor _modelConstructor;

      public override void GlobalContext()
      {
         base.GlobalContext();
         InitializeSimulation();
      }

      protected virtual void InitializeSimulation()
      {
         _objectPathFactory = IoC.Resolve<IObjectPathFactory>();
         _moleculeStartValuesCreator = IoC.Resolve<IMoleculeStartValuesCreator>();
         _buildConfiguration = IoC.Resolve<ModelHelperForSpecs>().CreateBuildConfiguration();
         _modelConstructor = IoC.Resolve<IModelConstructor>();
         _result = _modelConstructor.CreateModelFrom(_buildConfiguration, "MyModel");
         _model = _result.Model;
         _simulation = new ModelCoreSimulation
         {
            BuildConfiguration = _buildConfiguration,
            Model = _model
         };
      }
   }
}
using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Converter.v5_2;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Converter.v5_2
{
   public abstract class concern_for_DimensionConverter : ContextSpecification<IDimensionConverter>
   {
      protected XElement _element;
      protected IDimensionMapper _dimensionMapper;
      protected IParameterConverter _parameterConverter;
      private IUsingFormulaConverter _usingFormulaConverter;
      private IDataRepositoryConverter _dataRepositoryConverter;

      protected override void Context()
      {
         _element = new XElement("Root",
            new XElement("Container1",
               new XElement("Parameter1", new XAttribute(Constants.Serialization.Attribute.Dimension, "OldDim1")),
               new XElement("Parameter2", new XAttribute(Constants.Serialization.Attribute.Dimension, "Dim1"))),
            new XElement("Container2",
               new XElement("Parameter3", new XAttribute(Constants.Serialization.Attribute.Dimension, "OldDim2")),
               new XElement("Parameter4", new XAttribute(Constants.Serialization.Attribute.Dimension, "Dim2"))));

         _dimensionMapper = A.Fake<IDimensionMapper>();
         _parameterConverter = A.Fake<IParameterConverter>();
         _usingFormulaConverter = A.Fake<IUsingFormulaConverter>();
         _dataRepositoryConverter = A.Fake<IDataRepositoryConverter>();
         A.CallTo(() => _dimensionMapper.DimensionNameFor("OldDim1", false)).Returns("NewDim1");
         A.CallTo(() => _dimensionMapper.DimensionNameFor("Dim1", false)).Returns("Dim1");
         A.CallTo(() => _dimensionMapper.DimensionNameFor("OldDim2", false)).Returns("NewDim2");
         A.CallTo(() => _dimensionMapper.DimensionNameFor("Dim2", false)).Returns("Dim2");
         sut = new DimensionConverter(_dimensionMapper, _parameterConverter, _usingFormulaConverter, _dataRepositoryConverter);
      }
   }

   public class When_converting_the_dimension_defined_in_an_xml_element : concern_for_DimensionConverter
   {
      protected override void Because()
      {
         sut.ConvertDimensionIn(_element);
      }

      [Observation]
      public void should_have_replace_the_old_dimension_names_with_the_new_one()
      {
         _element.Descendants("Parameter1").First().Attribute(Constants.Serialization.Attribute.Dimension).Value.ShouldBeEqualTo("NewDim1");
         _element.Descendants("Parameter2").First().Attribute(Constants.Serialization.Attribute.Dimension).Value.ShouldBeEqualTo("Dim1");
         _element.Descendants("Parameter3").First().Attribute(Constants.Serialization.Attribute.Dimension).Value.ShouldBeEqualTo("NewDim2");
         _element.Descendants("Parameter4").First().Attribute(Constants.Serialization.Attribute.Dimension).Value.ShouldBeEqualTo("Dim2");
      }
   }

   internal class When_converting_the_dimension_in_an_simulation : concern_for_DimensionConverter
   {
      private IModelCoreSimulation _simulation;
      private IModel _model;
      private IParameter _rootPara;
      private Parameter _neighborhoodParameter;

      protected override void Context()
      {
         base.Context();
         _model = A.Fake<IModel>();
         IContainer root = new Container().WithName("root");
         var dimension = new Dimension(new BaseDimensionRepresentation(), "NewDim", "base");
         _rootPara = new Parameter().WithName("P1")
            .WithDimension(dimension)
            .WithFormula(new ConstantFormula(2))
            .WithParentContainer(root);
         _model.Root = root;

         var neighborhoods = new Container().WithName("Neighborhoods");

         _neighborhoodParameter = new Parameter().WithName("P2").WithDimension(dimension).WithFormula(new ConstantFormula(5));
         neighborhoods.Add(_neighborhoodParameter);
         _model.Root.Add(neighborhoods);
         A.CallTo(() => _model.Neighborhoods).Returns(neighborhoods);
         _simulation = new ModelCoreSimulation {Model = _model, BuildConfiguration = new BuildConfiguration()};
      }

      protected override void Because()
      {
         sut.ConvertDimensionIn(_simulation);
      }

      [Observation]
      public void should_convert_parameter_in_root()
      {
         A.CallTo(() => _parameterConverter.Convert(_rootPara, true)).MustHaveHappened(Repeated.Exactly.Once);
      }

      [Observation]
      public void should_convert_parameter_in_neighborhoods()
      {
         A.CallTo(() => _parameterConverter.Convert(_neighborhoodParameter, true)).MustHaveHappened(Repeated.Exactly.Once);
      }
   }
}
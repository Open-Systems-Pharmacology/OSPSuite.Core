using System;
using System.Collections.Generic;
using System.Threading;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterIdentificationRunFactory : ContextSpecification<IParameterIdentificationRunFactory>
   {
      private IRepository<IParameterIdentificationRunSpecificationFactory> _creatorRepository;
      private IParameterIdentificationRunSpecificationFactory _creator1;
      protected IParameterIdentificationRunSpecificationFactory _creator2;

      protected ParameterIdentification _parameterIdentification;
      protected CancellationToken _cancellationToken;
      protected ParameterIdentificationRunMode _parameterIdentificationRunModes;

      protected override void Context()
      {
         _creatorRepository = A.Fake<IRepository<IParameterIdentificationRunSpecificationFactory>>();
         _creator1 = A.Fake<IParameterIdentificationRunSpecificationFactory>();
         _creator2 = A.Fake<IParameterIdentificationRunSpecificationFactory>();
         A.CallTo(() => _creatorRepository.All()).Returns(new[] {_creator1, _creator2});
         sut = new ParameterIdentificationRunFactory(_creatorRepository);

         _parameterIdentification = A.Fake<ParameterIdentification>();
         _parameterIdentificationRunModes = A.Fake<ParameterIdentificationRunMode>();
         A.CallTo(() => _parameterIdentification.Configuration.RunMode).Returns(_parameterIdentificationRunModes);
      }
   }

   public class When_creating_the_runnable_parameter_identifications_for_an_option_that_can_be_resolved : concern_for_ParameterIdentificationRunFactory
   {
      private IReadOnlyList<IParameterIdentificationRun> _task;
      private IReadOnlyList<IParameterIdentificationRun> _result;

      protected override void Context()
      {
         base.Context();
         _task = A.Fake<IReadOnlyList<IParameterIdentificationRun>>();
         A.CallTo(() => _creator2.IsSatisfiedBy(_parameterIdentificationRunModes)).Returns(true);
         A.CallTo(() => _creator2.CreateFor(_parameterIdentification, _cancellationToken)).Returns(_task);
      }

      protected override void Because()
      {
         _result = sut.CreateFor(_parameterIdentification, _cancellationToken);
      }

      [Observation]
      public void should_return_the_runnable_identifications_created_by_the_expected_factory()
      {
         _result.ShouldBeEqualTo(_task);
      }
   }

   public class When_creating_the_runnable_parameter_identifications_for_an_option_that_cannot_be_resolved : concern_for_ParameterIdentificationRunFactory
   {    
      [Observation]
      public void should_thrown_an_exception()
      {
         The.Action(() => sut.CreateFor(_parameterIdentification, _cancellationToken)).ShouldThrowAn<ArgumentOutOfRangeException>();
      }
   }
}
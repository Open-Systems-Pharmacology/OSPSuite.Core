using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain
{
   public abstract class Project : ObjectBase, IProject
   {
      private readonly Cache<string, DataRepository> _allObservedData = new Cache<string, DataRepository>(x => x.Id, x => null);
      private readonly List<IClassifiable> _allClassifiables = new List<IClassifiable>();
      private readonly List<IClassification> _allClassifications = new List<IClassification>();
      private readonly List<ParameterIdentification> _allParameterIdentifications = new List<ParameterIdentification>();
      private readonly List<SensitivityAnalysis> _allSensitivityAnalyses = new List<SensitivityAnalysis>();

      private readonly ICache<string, CurveChartTemplate> _chartTemplates;
      public CreationMetaData Creation { get; set; }
      public string FilePath { get; set; }
      public abstract bool HasChanged { get; set; }
      public string JournalPath { get; set; }
      public Favorites Favorites { get; } = new Favorites();
      public virtual DisplayUnitsManager DisplayUnits { get; } = new DisplayUnitsManager();

      protected Project()
      {
         Name = Constants.ProjectUndefined;
         FilePath = string.Empty;
         _chartTemplates = new Cache<string, CurveChartTemplate>(x => x.Name, x => null);
         Creation = new CreationMetaData();
      }

      public abstract IReadOnlyCollection<T> All<T>() where T : class;

      public IReadOnlyCollection<DataRepository> AllObservedData => _allObservedData;

      public virtual void AddObservedData(DataRepository dataRepositoryToAdd)
      {
         _allObservedData.Add(dataRepositoryToAdd);
      }

      public virtual void RemoveObservedData(DataRepository dataRepositoryToRemove)
      {
         _allObservedData.Remove(dataRepositoryToRemove.Id);
         RemoveClassifiableForWrappedObject(dataRepositoryToRemove);
      }

      protected void RemoveClassifiableForWrappedObject(IWithId wrappedObject)
      {
         var classifiable = _allClassifiables.FindById(wrappedObject.Id);
         if (classifiable == null) return;
         RemoveClassifiable(classifiable);
      }

      public DataRepository ObservedDataBy(string dataRepositoryId) => _allObservedData[dataRepositoryId];

      public virtual DataRepository ObservedDataBy(UsedObservedData usedObservedData) => ObservedDataBy(usedObservedData.Id);
      
      public IReadOnlyCollection<IClassification> AllClassifications => _allClassifications;

      public IReadOnlyCollection<IClassification> AllClassificationsByType(ClassificationType classificationType)
      {
         return _allClassifications.Where(x => x.ClassificationType == classificationType).ToList();
      }

      public void AddClassification(IClassification classification)
      {
         if (classification == null) return;

         _allClassifications.Add(classification);
         HasChanged = true;
      }

      public IReadOnlyCollection<IClassifiable> AllClassifiables => _allClassifiables;

      public IReadOnlyCollection<TClassifiable> AllClassifiablesByType<TClassifiable>() where TClassifiable : IClassifiable
      {
         return AllClassifiables.OfType<TClassifiable>().ToList();
      }

      public void AddClassifiable(IClassifiable classifiable)
      {
         if (_allClassifiables.ExistsById(classifiable.Id)) return;

         _allClassifiables.Add(classifiable);
         HasChanged = true;
      }

      public void RemoveClassifiable(IClassifiable classifiable)
      {
         _allClassifiables.Remove(classifiable);
      }

      public void RemoveClassification(IClassification classification)
      {
         if (classification == null) return;
         _allClassifications.Remove(classification);
         HasChanged = true;
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         _allParameterIdentifications.ToList().Each(x => x.AcceptVisitor(visitor));
         _allSensitivityAnalyses.ToList().Each(x => x.AcceptVisitor(visitor));
      }

      public IClassification GetOrCreateByPath(IClassification parent, string newClassificationName, ClassificationType classificationType)
      {
         string newPath = newClassificationName;
         if (parent != null)
            newPath = parent.Path + newPath;

         var equivalent = AllClassificationsByType(classificationType).FirstOrDefault(g => string.Equals(g.Path, newPath));

         if (equivalent == null)
         {
            equivalent = new Classification { ClassificationType = classificationType, Name = newClassificationName };
            AddClassification(equivalent);
         }

         return equivalent;
      }

      public TClassifiable GetOrCreateClassifiableFor<TClassifiable, TSubject>(TSubject subject)
         where TClassifiable : Classifiable<TSubject>, new()
         where TSubject : IWithId, IWithName
      {
         var classifiableSubject = AllClassifiables.FindById(subject.Id).DowncastTo<TClassifiable>();

         if (classifiableSubject != null)
            return classifiableSubject;

         classifiableSubject = new TClassifiable { Subject = subject };
         AddClassifiable(classifiableSubject);

         return classifiableSubject;
      }

      public void AddChartTemplate(CurveChartTemplate chartTemplate)
      {
         _chartTemplates.Add(chartTemplate);
      }

      public void RemoveChartTemplate(string chartTemplateName)
      {
         _chartTemplates.Remove(chartTemplateName); ;
      }

      public CurveChartTemplate ChartTemplateByName(string templateName)
      {
         return _chartTemplates[templateName];
      }

      public void RemoveAllChartTemplates()
      {
         _chartTemplates.Clear();
      }

      public IEnumerable<CurveChartTemplate> ChartTemplates => _chartTemplates;

      public CurveChartTemplate DefaultChartTemplate
      {
         get { return ChartTemplates.FirstOrDefault(x => x.IsDefault) ?? ChartTemplates.OrderBy(x => x.Name).FirstOrDefault(); }
      }

      public IReadOnlyCollection<ParameterIdentification> AllParameterIdentifications => _allParameterIdentifications;

      public virtual void AddParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _allParameterIdentifications.Add(parameterIdentification);
         HasChanged = true;
      }

      public void RemoveParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _allParameterIdentifications.Remove(parameterIdentification);
         RemoveClassifiableForWrappedObject(parameterIdentification);
         HasChanged = true;
      }

      public IReadOnlyCollection<SensitivityAnalysis> AllSensitivityAnalyses => _allSensitivityAnalyses;

      public void AddSensitivityAnalysis(SensitivityAnalysis sensitivityAnalysis)
      {
         _allSensitivityAnalyses.Add(sensitivityAnalysis);
         HasChanged = true;
      }

      public void RemoveSensitivityAnalysis(SensitivityAnalysis sensitivityAnalysis)
      {
         _allSensitivityAnalyses.Remove(sensitivityAnalysis);
         RemoveClassifiableForWrappedObject(sensitivityAnalysis);
         HasChanged = true;
      }

      public IEnumerable<IParameterAnalysable> AllParameterAnalysables => _allParameterIdentifications.OfType<IParameterAnalysable>().Union(_allSensitivityAnalyses);


      public abstract IEnumerable<IUsesObservedData> AllUsersOfObservedData { get; }
   }
}
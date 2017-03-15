using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Nodes
{
   
   public class SensitivityAnalysisNode : ObjectWithIdAndNameNode<ClassifiableSensitivityAnalysis>, IViewItem
   {
      public SensitivityAnalysisNode(ClassifiableSensitivityAnalysis classifiableSensitivityAnalysis)
         : base(classifiableSensitivityAnalysis)
      {
      }

      public SensitivityAnalysis SensitivityAnalysis => Tag.SensitivityAnalysis;
   }
}
using System;
using System.Collections.Generic;
using System.Text;

using dnppv.contracts.patternfilter;
using dnppv.contracts.fileadapter;
using dnppv.contracts.patternrecognizer;
using dnppv.contracts.domainmodel;

namespace dnppv.patternfilter
{
    public class PatternFilter : IPatternFilter
    {
        #region IPatternFilter Members

        public IPatternList Analyse(IFileAdapter file)
        {
            IPatternRecognizer pr;
            pr = ralfw.Unity.ContainerProvider.Get().Get<IPatternRecognizer>();
            return pr.DetectPatterns(file);
        }

        #endregion
    }
}

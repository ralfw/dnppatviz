using System;
using System.Collections.Generic;
using System.Text;

using dnppv.contracts.fileadapter;
using dnppv.contracts.patternrecognizer;

namespace dnppv.pilepatternrecognizer
{
    public class PilePatternRecognizer : IPatternRecognizer
    {
        #region IPatternRecognizer Members

        public IPatternList DetectPatterns(IFileAdapter file)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using dnppv.contracts.domainmodel;
using dnppv.contracts.patternrecognizer;

namespace dnppv.pilepatternrecognizer
{
    internal class PatternOccurrence : IPatternOccurrence
    {
        private int start, end;


        public PatternOccurrence(int start, int end)
        {
            this.start = start;
            this.end = end;
        }


        #region IPatternOccurrence Members

        public int Start
        {
            get { return this.start; }
        }

        public int End
        {
            get { return this.end; }
        }

        #endregion
    }

}

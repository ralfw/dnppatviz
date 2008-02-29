using System;
using System.Collections.Generic;
using System.Text;

using dnppv.contracts.domainmodel;
using dnppv.contracts.patternrecognizer;

namespace dnppv.pilepatternrecognizer
{
    internal class PatternList : IPatternList
    {
        private List<IPattern> patterns;
        private int signalCount;


        public PatternList(int signalCount)
        {
            this.patterns = new List<IPattern>();
            this.signalCount = signalCount;
        }


        internal Pattern Add(int patternSize)
        {
            Pattern newPattern = new Pattern(patternSize);
            this.patterns.Add(newPattern);
            return newPattern;
        }


        #region IPatternList Members

        public int Count
        {
            get { return this.patterns.Count; }
        }

        public int SignalCount
        {
            get { return this.signalCount; }
        }

        public IPattern this[int index]
        {
            get { return this.patterns[index]; }
        }

        #endregion


        #region IEnumerable<IPattern> Members

        public IEnumerator<IPattern> GetEnumerator()
        {
            return this.patterns.GetEnumerator();
        }

        #endregion
        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}

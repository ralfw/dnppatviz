using System;
using System.Collections.Generic;
using System.Text;

using dnppv.contracts.patternrecognizer;

namespace dnppv.pilepatternrecognizer
{
    internal class Pattern : IPattern
    {
        private int size;
        private List<IPatternOccurrence> occurrences;


        public Pattern(int size)
        {
            this.size = size;
            this.occurrences = new List<IPatternOccurrence>();
        }


        internal PatternOccurrence Add(int start)
        {
            PatternOccurrence occ = new PatternOccurrence(start, start + this.size - 1);
            this.occurrences.Add(occ);
            return occ;
        }


        #region IPattern Members

        public int Size
        {
            get { return this.size; }
        }


        public int Count
        {
            get { return this.occurrences.Count; }
        }


        public IPatternOccurrence this[int index]
        {
            get { return this.occurrences[index]; }
        }

        #endregion


        #region IEnumerable<IPatternOccurrence> Members

        public IEnumerator<IPatternOccurrence> GetEnumerator()
        {
            return this.occurrences.GetEnumerator();
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

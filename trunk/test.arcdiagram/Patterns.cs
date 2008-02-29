using System;
using System.Collections.Generic;
using System.Text;

using dnppv.contracts.domainmodel;

namespace test.arcdiagram
{
    internal class PatternList : IPatternList
    {
        internal List<IPattern> patterns = new List<IPattern>();
        private int signalCount;


        internal PatternList(int signalCount)
        {
            this.signalCount = signalCount;
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
            return this.patterns.GetEnumerator();
        }

        #endregion
    }


    internal class Pattern : IPattern
    {
        internal List<IPatternOccurrence> occurrences = new List<IPatternOccurrence>();
        private int size;

        internal Pattern(int size)
        {
            this.size = size;
        }

        #region IPattern Members

        public int Count
        {
            get { return this.occurrences.Count; }
        }

        public int Size
        {
            get { return this.size; }
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
            return this.occurrences.GetEnumerator();
        }

        #endregion
    }


    internal class PatternOccurrence : IPatternOccurrence
    {
        private int start, end;

        internal PatternOccurrence(int start, int end)
        {
            this.start = start;
            this.end = end;
        }

        #region IPatternOccurrence Members

        public int End
        {
            get { return this.end; }
        }

        public int Start
        {
            get { return this.start; }
        }

        #endregion
    }
}

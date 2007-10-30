using System;
using System.Collections.Generic;
using System.Text;

using dnppv.pile;

namespace dnppv.pilepatternrecognizer
{
    internal class Pair : InnerRelationBase
    {
        private List<int> occurrences = new List<int>();

        public void AddOccurrence(int start)
        {
            this.occurrences.Add(start);
        }

        public int[] Occurrences
        {
            get { return this.occurrences.ToArray(); }
        }
    }
}

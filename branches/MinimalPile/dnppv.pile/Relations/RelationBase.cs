using System;
using System.Collections.Generic;
using System.Text;

namespace dnppv.pile
{
    public abstract class RelationBase
    {
        #region Id Management
        private static long idCounter = 0;

        private static long GenerateId()
        {
            return System.Threading.Interlocked.Increment(ref idCounter);
        }
        #endregion


        private long id;
        private List<RelationBase> nChildren, aChildren;


        public RelationBase()
        {
            this.id = RelationBase.GenerateId();

            this.nChildren = new List<RelationBase>();
            this.aChildren = new List<RelationBase>();
        }


        public long Id
        {
            get { return this.id; }
        }


        #region Children management
        internal void AddChild(RelationBase child, bool isNormChild)
        {
            if (isNormChild)
                lock (this.nChildren)
                {
                    this.nChildren.Add(child);
                }
            else
                lock (this.aChildren)
                {
                    this.aChildren.Add(child);
                }
        }


        public RelationBase[] NormChildren
        {
            get { lock (this.nChildren) { return this.nChildren.ToArray(); } }
        }

        public RelationBase[] AssocChildren
        {
            get { lock (this.aChildren) { return this.aChildren.ToArray(); } }
        }
        #endregion
    }
}

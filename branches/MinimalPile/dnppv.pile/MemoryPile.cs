using System;
using System.Collections.Generic;
using System.Text;

namespace dnppv.pile
{
    public class MemoryPile<TTerminalValue, TRelation> 
        where TTerminalValue : TerminalValueBase, new()
        where TRelation : InnerRelationBase, new()
    {
        private Dictionary<string, TTerminalValue> terminalValues;
        private Dictionary<string, TRelation> innerRelations;


        public MemoryPile()
        {
            this.terminalValues = new Dictionary<string, TTerminalValue>();
            this.innerRelations = new Dictionary<string, TRelation>();
        }


        #region Terminal Values
        public TTerminalValue Create(string key)
        {
            bool isNew;
            return this.Create(key, out isNew);
        }

        public TTerminalValue Create(string key, out bool isNew)
        {
            lock (this.terminalValues)
            {
                TTerminalValue tv;
                isNew = !this.terminalValues.TryGetValue(key, out tv);
                if (isNew)
                {
                    tv = new TTerminalValue();
                    tv.Initialize(key);
                    this.terminalValues.Add(key, tv);
                }
                return tv;
            }
        }


        public int CountTV
        {
            get { return this.terminalValues.Count; }
        }
        #endregion


        #region Create relation
        public TRelation Create(RelationBase nParent, RelationBase aParent)
        {
            bool isNew;
            return this.Create(nParent, aParent, out isNew);
        }

        public TRelation Create(RelationBase nParent, RelationBase aParent, out bool isNew)
        {
            string parentKey = this.ParentIdsToString(nParent, aParent);

            lock (this.innerRelations)
            {
                TRelation child = this.Get(parentKey);
                isNew = child == null;

                if (isNew)
                {
                    child = new TRelation();
                    child.Initialize(nParent, aParent);

                    this.innerRelations.Add(parentKey, child);
                }

                return child;
            }
        }
        #endregion


        #region Get relation
        public TRelation Get(RelationBase nParent, RelationBase aParent)
        {
            return Get(ParentIdsToString(nParent, aParent));
        }

        private TRelation Get(string parentKey)
        {
            TRelation child;

            lock (this.innerRelations)
            {
                if (this.innerRelations.TryGetValue(parentKey, out child))
                    return child;
                else
                    return null;
            }
        }


        public int CountInnerRelation
        {
            get { return this.innerRelations.Count; }
        }


        private string ParentIdsToString(RelationBase nParent, RelationBase aParent)
        {
            return string.Format("({0},{1})", nParent.Id, aParent.Id);
        }
        #endregion
    }
}

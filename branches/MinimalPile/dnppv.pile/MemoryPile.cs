using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace dnppv.pile
{
    public class MemoryPile<TTerminalValue, TRelation> 
        where TTerminalValue : TerminalValueBase, new()
        where TRelation : InnerRelationBase, new()
    {
        private Dictionary<string, TTerminalValue> terminalValues;
        private Hashtable innerRelations;

        public MemoryPile()
        {
            this.terminalValues = new Dictionary<string, TTerminalValue>();
            this.innerRelations = new Hashtable();
        }


        #region Terminal Values
        public TTerminalValue Create(string key)
        {
            bool isNew;
            return this.Create(key, out isNew);
        }

        public TTerminalValue Create(string key, out bool isNew)
        {
            //lock (this.terminalValues)
            //{
                TTerminalValue tv;
                isNew = !this.terminalValues.TryGetValue(key, out tv);
                if (isNew)
                {
                    tv = new TTerminalValue();
                    tv.Initialize(key);
                    this.terminalValues.Add(key, tv);
                }
                return tv;
            //}
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
            //lock (this.innerRelations)
            //{
                //TRelation child = this.Get(parentKey);
                TRelation child = this.Get(nParent, aParent);
                isNew = child == null;

                if (isNew)
                {
                    child = new TRelation();
                    child.Initialize(nParent, aParent);

                    Hashtable assocRelations;
                    if (this.innerRelations.ContainsKey(nParent.Id))
                        assocRelations = (Hashtable)this.innerRelations[nParent.Id];
                    else
                    {
                        assocRelations = new Hashtable();
                        this.innerRelations.Add(nParent.Id, assocRelations);
                    }
                    assocRelations.Add(aParent.Id, child);
                }

                return child;
            //}
        }
        #endregion


        #region Get relation
        public TRelation Get(RelationBase nParent, RelationBase aParent)
        {
            TRelation child = null;
            Hashtable assocRelations;
            if (this.innerRelations.ContainsKey(nParent.Id))
            {
                assocRelations = (Hashtable)this.innerRelations[nParent.Id];
                if (assocRelations.ContainsKey(aParent.Id))
                    child = (TRelation)assocRelations[aParent.Id];
            }
            return child;            
        }


        public int CountInnerRelation
        {
            get { return this.innerRelations.Count; }
        }
        #endregion
    }
}

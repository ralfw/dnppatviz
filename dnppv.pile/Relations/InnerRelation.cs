using System;
using System.Collections.Generic;
using System.Text;

namespace dnppv.pile
{
    public class InnerRelationBase : RelationBase
    {
        private RelationBase nParent, aParent;


        public InnerRelationBase() { }

        internal void Initialize(RelationBase nParent, RelationBase aParent)
        {
            this.nParent = nParent;
            this.nParent.AddChild(this, true);

            this.aParent = aParent;
            this.aParent.AddChild(this, false);
        }


        public RelationBase NormParent
        {
            get { return this.nParent; }
        }

        public RelationBase AssocParent
        {
            get { return this.aParent; }
        }
    }
}

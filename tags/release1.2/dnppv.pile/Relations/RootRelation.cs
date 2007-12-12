using System;
using System.Collections.Generic;
using System.Text;

namespace dnppv.pile
{
    public class RootRelation : RelationBase
    {
        private string key;


        public RootRelation() { }

        internal void Initialize(string key)
        {
            this.key = key;
        }


        public string Key
        {
            get { return this.key; }
        }
    }
}

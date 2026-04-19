using System;
using UnityEngine;

namespace Justcore
{
    public class WaitUntilAny : CustomYieldInstruction
    {
        public Func<bool>[] predicates;

        public override bool keepWaiting
        {
            get
            {
                foreach (var p in predicates)
                    if (p())
                        return false;
                return true;
            }
        }

        public WaitUntilAny(Func<bool> predicate1, Func<bool> predicate2)
        {
            predicates = new[]{predicate1, predicate2};
        }

        public WaitUntilAny(Func<bool>[] predicates)
        {
            this.predicates = predicates;
        }
    }
}
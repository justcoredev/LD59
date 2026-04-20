using System;
using UnityEngine;

namespace Justcore
{
    public class WaitUntilAll : CustomYieldInstruction
    {
        public Func<bool>[] predicates;

        public override bool keepWaiting
        {
            get
            {
                foreach (var p in predicates)
                    if (!p())
                        return true;
                return false;
            }
        }

        public WaitUntilAll(Func<bool> predicate1, Func<bool> predicate2)
        {
            predicates = new[]{predicate1, predicate2};
        }

        public WaitUntilAll(Func<bool>[] predicates)
        {
            this.predicates = predicates;
        }
    }
}
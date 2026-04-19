using System;
using UnityEngine;

namespace Justcore
{
    public class WaitUntilOrForSeconds : CustomYieldInstruction
    {
        private readonly float waitTime;
        private readonly float startTime;
        private Func<bool> predicate;

        public WaitUntilOrForSeconds(Func<bool> predicate, float seconds)
        {
            waitTime = seconds;
            startTime = Time.time;
            this.predicate = predicate;
        }

        public override bool keepWaiting
        {
            get
            {
                return !predicate() && (Time.time - startTime < waitTime);
            }
        }
    }
}
using System;
using UnityEngine;

namespace Justcore
{
    public class WaitUntilAndForSeconds : CustomYieldInstruction
    {
        private readonly float waitTime;
        private readonly float startTime;
        private Func<bool> predicate;

        public WaitUntilAndForSeconds(Func<bool> predicate, float seconds)
        {
            waitTime = seconds;
            startTime = Time.time;
            this.predicate = predicate;
        }

        public override bool keepWaiting
        {
            get
            {
                bool timePassed = (Time.time - startTime) >= waitTime;
                return !(predicate() && timePassed);
            }
        }
    }
}
using System.Collections.Generic;
using TMPEffects.Databases;
using UnityEngine;

namespace TMPEffects.TMPCommands.Commands
{
    [CreateAssetMenu(fileName = "new PauseCommand", menuName = "TMPEffects/Commands/Extension/PauseCommand")]
    public class PauseCommand : TMPCommand
    {
        public override TagType TagType => TagType.Index;
        public override bool ExecuteInstantly => false;
        public override bool ExecuteOnSkip => false;
        public override bool ExecuteRepeatable => true;

#if UNITY_EDITOR
        public override bool ExecuteInPreview => true;
#endif

        public override void ExecuteCommand(ICommandContext context)
        {
            context.Writer.StopWriter();
        }

        public override object GetNewCustomData()
        {
            // No data
            return null;
        }

        public override void SetParameters(object obj, IDictionary<string, string> parameters, ITMPKeywordDatabase keywordDatabase)
        {
            // No params
        }

        public override bool ValidateParameters(IDictionary<string, string> parameters, ITMPKeywordDatabase keywordDatabase)
        {
            // No params, thus, always valid
            return true;
        }
    }
}

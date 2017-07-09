using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{
    public interface IReagentContainer
    {
        float Volume { get; }
        float Amount { get; }
        float SpaceLeft { get; }
        List<Reagent> Reagents { get; }
        float AddReagent(Reagent reagent);
        void AddReagents(ref List<Reagent> inputReagents);
        void NormalizeReagents();
        float FlushReagents();
    }
}
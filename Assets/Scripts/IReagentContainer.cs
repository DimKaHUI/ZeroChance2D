using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{
    public interface IReagentContainer
    {
        float Volume { get; }
        float Amount { get; }
        float TransferReagents(IReagentContainer target, float amount);
        List<IReagent> AddReagents(IReagent[] reagents);
        float FlushReagents();
    }
}
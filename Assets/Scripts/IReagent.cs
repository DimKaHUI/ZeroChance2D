using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{

    public interface IReagent
    {
        float Amount { get; set; }
        string Description { get; }
        string ReagentName { get; }
    }
}

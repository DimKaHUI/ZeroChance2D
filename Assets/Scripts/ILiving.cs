using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{

    public interface ILiving : IReagentContainer
    {
        string SpeciesName { get; set; }
        float WalkSpeed { get; set; }
        float RunSpeed { get; set; }
    }
}
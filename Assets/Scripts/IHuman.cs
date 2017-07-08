using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroChance2D
{
    public interface IHuman
    {
        string Forename { get; set; }
        string Surname { get; set; }

        float WalkSpeed { get; set; }
        float RunSpeed { get; set; }
    }

}
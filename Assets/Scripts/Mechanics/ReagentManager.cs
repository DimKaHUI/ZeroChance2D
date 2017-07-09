using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reagent
{
    private float amount;
    public readonly string Name;

    public float Amount
    {
        get { return amount; }
        set
        {
            if (value >= 0)
                amount = value;
            else
                amount = 0;
        }
    };

    public Reagent(string name, float amount)
    {
        Name = name;
        this.amount = amount;
    }
}

public class ReagentManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

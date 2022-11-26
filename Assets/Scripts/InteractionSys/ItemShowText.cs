using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShowText : Interactable
{
    [SerializeField] private string message;
    public override string GetDescription() {
        return message;
    }

    public override void Interact() {
    }
}

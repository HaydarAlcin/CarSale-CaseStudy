using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetInteraction
{
    DealerManager dealerManager { get; }

    void Interaction();
    void OutlineActive();
}

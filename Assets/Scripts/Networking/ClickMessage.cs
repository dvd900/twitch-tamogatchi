using UnityEngine;
using System.Collections;

public class ClickMessage : NetMsg {
    public float x, y;

    public override int GetMsgInd() {
        return NetMsgInds.ClickMessage;
    }
}

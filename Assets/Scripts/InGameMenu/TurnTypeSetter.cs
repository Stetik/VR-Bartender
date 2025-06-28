using UnityEngine.XR.Interaction.Toolkit;

public class TurnTypeSetter
{
    private ActionBasedSnapTurnProvider snap;
    private ActionBasedContinuousTurnProvider continuous;

    public TurnTypeSetter(ActionBasedSnapTurnProvider snap, ActionBasedContinuousTurnProvider continuous)
    {
        this.snap = snap;
        this.continuous = continuous;
    }

    public void ApplyFromIndex(int index)
    {
        bool useContinuous = index == 0;

        if (useContinuous)
        {
            snap.leftHandSnapTurnAction.action.Disable();
            snap.rightHandSnapTurnAction.action.Disable();
            continuous.leftHandTurnAction.action.Enable();
            continuous.rightHandTurnAction.action.Enable();
        }
        else
        {
            snap.leftHandSnapTurnAction.action.Enable();
            snap.rightHandSnapTurnAction.action.Enable();
            continuous.leftHandTurnAction.action.Disable();
            continuous.rightHandTurnAction.action.Disable();
        }
    }
}

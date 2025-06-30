using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInputController : MonoBehaviour
{
    [Header("Grab Rays")]
    [SerializeField] private GameObject leftGrabRay;
    [SerializeField] private GameObject rightGrabRay;
    [SerializeField] private XRDirectInteractor leftDirectGrab;
    [SerializeField] private XRDirectInteractor rightDirectGrab;

    [Header("Teleportation (Right Hand Only)")]
    [SerializeField] private GameObject rightTeleportRay;
    [SerializeField] private XRRayInteractor rightRayInteractor;
    [SerializeField] private InputActionProperty rightActivate;
    [SerializeField] private InputActionProperty rightCancel;

    [Header("Left Hand Animation")]
    [SerializeField] private Animator leftHandAnimator;
    [SerializeField] private InputActionProperty leftPinchAction;
    [SerializeField] private InputActionProperty leftGripAction;

    [Header("Right Hand Animation")]
    [SerializeField] private Animator rightHandAnimator;
    [SerializeField] private InputActionProperty rightPinchAction;
    [SerializeField] private InputActionProperty rightGripAction;

    private void Update()
    {
        // GRAB RAYS
        leftGrabRay.SetActive(leftDirectGrab.interactablesSelected.Count == 0);
        rightGrabRay.SetActive(rightDirectGrab.interactablesSelected.Count == 0);

        // TELEPORTATION RAY (RIGHT HAND)
        bool hovering = rightRayInteractor.TryGetHitInfo(out _, out _, out _, out _);
        bool shouldShowTeleport =
            !hovering &&
            rightCancel.action.ReadValue<float>() == 0 &&
            rightActivate.action.ReadValue<float>() > 0.1f;

        rightTeleportRay.SetActive(shouldShowTeleport);

        // LEFT HAND ANIMATION
        float leftTrigger = leftPinchAction.action.ReadValue<float>();
        float leftGrip = leftGripAction.action.ReadValue<float>();
        leftHandAnimator.SetFloat("Trigger", leftTrigger);
        leftHandAnimator.SetFloat("Grip", leftGrip);

        // RIGHT HAND ANIMATION
        float rightTrigger = rightPinchAction.action.ReadValue<float>();
        float rightGrip = rightGripAction.action.ReadValue<float>();
        rightHandAnimator.SetFloat("Trigger", rightTrigger);
        rightHandAnimator.SetFloat("Grip", rightGrip);
    }
}

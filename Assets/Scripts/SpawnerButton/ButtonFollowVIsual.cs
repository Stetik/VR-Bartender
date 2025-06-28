using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonFollowVisual : MonoBehaviour
{
    public Vector3 localAxis;
    public Transform visualTarget;
    public float resetSpeed = 5;
    public bool freeze = false;
    public float followAngleThreshold = 45;

    private Vector3 InitialLocalPos;
    private Vector3 offset;
    private Transform pokeAttachTransform;

    private XRBaseInteractable interactable;
    private Coroutine followRoutine;

    void Awake()
    {
        InitialLocalPos = visualTarget.localPosition;
        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
        interactable.selectEntered.AddListener(Freeze);
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor interactor)
        {
            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;

            float pokeAngle = Vector3.Angle(offset, visualTarget.TransformDirection(localAxis));
            if (pokeAngle < followAngleThreshold && !freeze)
            {
                if (followRoutine != null)
                    StopCoroutine(followRoutine);

                followRoutine = StartCoroutine(FollowRoutine());
            }
        }
    }

    public void Reset(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            freeze = false;
            if (followRoutine != null)
            {
                StopCoroutine(followRoutine);
                followRoutine = StartCoroutine(ResetRoutine());
            }
        }
    }

    public void Freeze(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            freeze = true;
            if (followRoutine != null)
                StopCoroutine(followRoutine);
        }
    }

    private IEnumerator FollowRoutine()
    {
        while (!freeze)
        {
            Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
            Vector3 constraintLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);
            visualTarget.position = visualTarget.TransformPoint(constraintLocalTargetPosition);
            yield return null; // esperar un frame
        }
    }

    private IEnumerator ResetRoutine()
    {
        while (Vector3.Distance(visualTarget.localPosition, InitialLocalPos) > 0.001f)
        {
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, InitialLocalPos, Time.deltaTime * resetSpeed);
            yield return null;
        }
        visualTarget.localPosition = InitialLocalPos;
    }
}

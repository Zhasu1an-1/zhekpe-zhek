using UnityEngine;

public class WeaponAttach : MonoBehaviour
{
    public Transform handBone;
    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale = Vector3.one;

    void LateUpdate()
    {
        if (handBone == null) return;

        transform.SetParent(handBone);
        transform.localPosition = localPosition;
        transform.localRotation = Quaternion.Euler(localRotation);
        transform.localScale = localScale;
    }
}
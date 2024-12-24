using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigController : MonoBehaviour
{

    [SerializeField] private Rig _allhandIKConstraint;
    //[SerializeField] private TwoBoneIKConstraint _rightHandIKConstraint;
    //[SerializeField] private TwoBoneIKConstraint _leftHandIKConstraint;
    [SerializeField] private Rig _bodyAimConstraint;
    [SerializeField] private Rig _weaponAimConstraint;

    //==============Calling Function From Unity Events==============//

    private void UpdateHandIKConstraintWeight(float rigWeight)
    {
        _allhandIKConstraint.weight = rigWeight;
    }

    private void UpdateBodyAimConstraintWeight(float rigWeight)
    {
        _bodyAimConstraint.weight = rigWeight;
    }

    private void UpdateWeaponAimConstraint(float rigWeight)
    {
        _weaponAimConstraint.weight = rigWeight;
    }



}




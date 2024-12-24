using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{

    [SerializeField] private Transform _crossHairTarget;
    [SerializeField] private Transform _weaponHolder_Pivot;
    [SerializeField] private Transform _bulletParent;
    [SerializeField] private Transform _weaponLeftGrip;
    [SerializeField] private Transform _weaponRightGrip;

    [SerializeField]private Animator _rigController;

    private RayCastWeapon _rayCastWeapon;

    private string _weaponName;
    private bool isHolster;

    private void Start()
    {


        RayCastWeapon existRayCastWeapon = GetComponentInChildren<RayCastWeapon>();
        if (existRayCastWeapon)
        {
            Equip(existRayCastWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_rayCastWeapon)
        {
            //Handle the firing
            if (Input.GetButtonDown("Fire1") && !isHolster)
            {
                    _rayCastWeapon.StartFiring();
            }

            if (_rayCastWeapon.IsFiring)
            {
                _rayCastWeapon.UpdateFiring(Time.deltaTime);
            }

            _rayCastWeapon.UpdateBullet(Time.deltaTime);

            if (Input.GetButtonUp("Fire1"))
            {
                _rayCastWeapon.StopFiring();
            }
        }



        if (_rayCastWeapon != null &&  _weaponName == "rifle")
        {
            isHolster = _rigController.GetBool("Holster_Rifle");
        }
        else
        {
            isHolster = _rigController.GetBool("Holster_Pistol");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(_weaponName == "rifle")
            {
                _rigController.SetBool("Holster_Rifle", !isHolster);
            }
            else
            {
                
                _rigController.SetBool("Holster_Pistol", !isHolster);
            }


        }


    }



    public void Equip(RayCastWeapon newWeapon)
    {
        if (_rayCastWeapon)
        {
            Destroy(_rayCastWeapon.gameObject);
        }

        _rayCastWeapon = newWeapon;
        _rayCastWeapon.SetRayCastTarget(_crossHairTarget);
        _rayCastWeapon.SetBulletParent(_bulletParent);
        _rayCastWeapon.transform.parent = _weaponHolder_Pivot;
        _rayCastWeapon.transform.localPosition = Vector3.zero;
        _rayCastWeapon.transform.localRotation = Quaternion.identity;
        _weaponName = _rayCastWeapon.WeaponName;

        _rigController.Play("equip_" + _rayCastWeapon.WeaponName);
    }



}

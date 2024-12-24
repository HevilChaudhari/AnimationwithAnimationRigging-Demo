using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    [SerializeField] private RayCastWeapon _weaponPrefab; 

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ActiveWeapon activeWeapon))
        {
            RayCastWeapon newWeapon = Instantiate(_weaponPrefab);
            activeWeapon.Equip(newWeapon);
        }
    }
}

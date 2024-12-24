using System.Collections.Generic;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
    private class Bullet
    {
        public float m_time;
        public Vector3 m_initialPosition;
        public Vector3 m_initialVelocity;
        public TrailRenderer m_tracer;

        public void DestroyTracer()
        {
            if(m_tracer != null)
            {
                m_tracer = null;
            }
        }
    }


    [SerializeField] private ParticleSystem[] _muzzleParticle;
    [SerializeField] private ParticleSystem _hitEffect;

    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private Transform _rayCastTarget;
    [SerializeField] private Transform _bulletParent;

    [SerializeField] private TrailRenderer _bulletTracerEffect;


    [Space]
    [Header("Rifle Firing Fators")]
    [SerializeField] private int _fireRate = 25;
    [SerializeField] private float _bulletSpeed = 1000.0f;
    [SerializeField] private float _bulletDrop = 0.0f;

    private List<Bullet> _bullets= new List<Bullet>();

    public string WeaponName;
    public bool IsFiring = false;

    private float _accumulatedTime;
    private float _maxLifeTime = 3.0f;

    private Ray _ray;
    private RaycastHit _hitInfo;

    private Vector3 GetBulletPosition(Bullet bullet)
    {
        //p + v*t + 0.5*g*t*t //Quadratic Equation

        Vector3 gravity = Vector3.down * _bulletDrop;

        return (bullet.m_initialPosition) + (bullet.m_initialVelocity * bullet.m_time) + (0.5f * gravity * bullet.m_time * bullet.m_time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.m_initialPosition = position;
        bullet.m_initialVelocity = velocity;
        bullet.m_time = 0.0f;
        bullet.m_tracer = Instantiate(_bulletTracerEffect, position, Quaternion.identity);
        bullet.m_tracer.AddPosition(position);
        return bullet;
    }

    //Set Firing to True
    public void StartFiring()
    {
        IsFiring = true;
        _accumulatedTime = 0.0f;
        FireBullet();

    }

    //Fire the Bullet Constantly
    public void UpdateFiring(float deltaTime)
    {
        _accumulatedTime += deltaTime;
        float fireInterval = 1.0f / _fireRate;

        while(_accumulatedTime >= 0.0f)
        {
            FireBullet();
            _accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullet(float deltaTime)
    {
        SimulateBullet(deltaTime);
        DestroyBullets();
    }

    private void SimulateBullet(float deltaTime)
    {
        _bullets.ForEach(Bullet =>
        {
            Vector3 p0 = GetBulletPosition(Bullet);
            Bullet.m_time += deltaTime;
            Vector3 p1 = GetBulletPosition(Bullet);
            RaycastSegment(p0, p1, Bullet);
        });
    }

    private void DestroyBullets()
    {
        _bullets.RemoveAll(Bullet => Bullet.m_time >= _maxLifeTime);
    }

    //Raycast 
    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        _ray.origin = start;
        _ray.direction = direction;

        if (Physics.Raycast(_ray, out _hitInfo,distance))
        {
            _hitEffect.transform.position = _hitInfo.point;
            _hitEffect.transform.forward = _hitInfo.normal;
            _hitEffect.Emit(1);

            bullet.m_tracer.transform.position = _hitInfo.point;
            bullet.m_time = _maxLifeTime;
        }
        else
        {
            bullet.m_tracer.transform.position = end;
        }
    }

    //Fire the Bullet
    private void FireBullet()
    {
        foreach (var particle in _muzzleParticle)
        {
            particle.Emit(1);
        }

        Vector3 velocity = (_rayCastTarget.position - _raycastOrigin.position).normalized * _bulletSpeed;
        var bullet = CreateBullet(_raycastOrigin.position, velocity);
        _bullets.Add(bullet);



    }

    //Set Firing to False
    public void StopFiring()
    {
        IsFiring = false;
    }

    public void SetRayCastTarget(Transform newRayCastTarget)
    {
        _rayCastTarget = newRayCastTarget;
    }

    public void SetBulletParent(Transform newBulletParent)
    {
        _bulletParent = newBulletParent;
    }
}

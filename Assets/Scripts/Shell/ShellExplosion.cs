using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public Score m_Score;
    public TankShooting m_TankShooting = null;
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        for (int i = 0; i < colliders.Length; ++i)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody)
                continue;

            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            if (!targetHealth)
                continue;

            float damage = CalculateDamage(targetRigidbody.position);
            targetHealth.TakeDamage(damage);

            if (targetHealth.m_Score != m_Score)
            {
                m_Score.AddDamageDone(damage);
                if (targetHealth.IsDead())
                {
                    m_Score.AddKill();
                }
            }
        }

        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        GetComponent<Rigidbody>().detectCollisions = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Light>().enabled = false;

        Destroy(gameObject, m_ExplosionParticles.main.duration);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;
        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
        float damage = m_MaxDamage * relativeDistance;
        damage = Mathf.Max(0, damage);
        return damage;
    }
}
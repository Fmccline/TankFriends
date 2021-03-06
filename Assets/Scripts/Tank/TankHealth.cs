﻿using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;          
    public Slider m_Slider;                        
    public Image m_FillImage;                      
    public Color m_FullHealthColor = Color.green;  
    public Color m_ZeroHealthColor = Color.red;    
    public GameObject m_ExplosionPrefab;
    [HideInInspector] public bool m_CanTakeDamage;
    public Score m_Score;

    private bool isDead;
    private AudioSource m_ExplosionAudio;          
    private ParticleSystem m_ExplosionParticles;   
    private float m_CurrentHealth;  

    public bool IsDead()
    {
        return isDead;
    }

    public bool CanTakeDamage()
    {
        return m_CanTakeDamage;
    }

    public float GetCurrentHealth()
    {
        return m_CurrentHealth;
    }

    private void Awake()
    {
        // best practice to create particle system object then use SetActive (better performance)
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        isDead = false;

        SetHealthUI();
    }
    

    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        if (m_CanTakeDamage == false)
            return;

        m_CurrentHealth -= amount;
        m_Score.AddDamageTaken(amount);

        SetHealthUI();

        if (m_CurrentHealth <= 0f && !isDead)
        {
            OnDeath();
        }
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        m_Slider.value = m_CurrentHealth;

        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        isDead = true;
        m_Score.AddDeath();

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        gameObject.SetActive(false);
    }
}
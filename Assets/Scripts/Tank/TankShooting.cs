using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;              // Used to identify the different players.
    public Rigidbody m_Shell;                   // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
    public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
    public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
    public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.
    public ITankInput m_TankInput = new HumanTankInput();
    public Score m_Score;

    public string m_FireButton;                // The input axis that is used for launching shells.
    private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
    private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
    private bool m_IsCharging;
    private float m_MaxCooldownTime = 0.1f;
    private float m_CurrentCooldownTime = 0f;


    public float GetCurrentLaunchForce()
    {
        return m_CurrentLaunchForce;
    }

    public bool IsCharging()
    {
        return m_IsCharging;
    }

    private void OnEnable()
    {
        // When the tank is turned on, reset the launch force and the UI
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        // The fire axis is based on the player number.
        m_FireButton = "Fire" + m_PlayerNumber;

        // The rate that the launch force charges up is the range of possible forces by the max charge time.
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }


    private void Update()
    {
        // The slider should have a default value of the minimum launch force.
        m_AimSlider.value = m_MinLaunchForce;

        // If on cooldown, add change in time and return
        if (m_CurrentCooldownTime > 0f)
        {
            m_CurrentCooldownTime -= Time.deltaTime;
            return;
        }
        // If not on cooldown, make sure cooldown is 0
        m_CurrentCooldownTime = 0f;

        // If the max force has been exceeded and the shell hasn't yet been launched...
        if (m_CurrentLaunchForce >= m_MaxLaunchForce)
        {
            // ... use the max force and launch the shell.
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
       else if (m_TankInput.IsChargingShot(this) && !m_IsCharging)
        {
            // ... reset the fired flag and reset the launch force.
            m_CurrentLaunchForce = m_MinLaunchForce;

            // Change the clip to the charging clip and start it playing.
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
            m_IsCharging = true;
        }
        else if (m_TankInput.IsChargingShot(this) && !m_TankInput.IsFiringShot(this))
        {
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (m_TankInput.IsFiringShot(this))
        {
            m_IsCharging = false;
            Fire();
        }
    }


    private void Fire()
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        shellInstance.gameObject.GetComponent<ShellExplosion>().m_Score = m_Score;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.loop = false;
        m_ShootingAudio.Play();

        // Reset the launch force.  This is a precaution in case of missing button events.
        m_CurrentLaunchForce = m_MinLaunchForce;

        // Set cooldown
        m_CurrentCooldownTime = m_MaxCooldownTime;
    }
}
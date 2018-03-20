using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    //public float m_MaxSpeed = 15f;
    //public float m_Acceleration = 50f;
    //public float m_CurrentSpeed = 0f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;
    public ITankInput m_TankInput = new HumanTankInput();
    public string m_MovementAxisName;     
    public string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;         


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }

    // called every frame
    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = m_TankInput.GetMovementInput(this);
        m_TurnInputValue = m_TankInput.GetTurnInput(this);

        EngineAudio();
    }


    public void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }

    // runs every physics step
    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }


    private void Move()
    {
        //m_CurrentSpeed = Mathf.Abs(m_Rigidbody.velocity.magnitude);
        //if (m_MovementInputValue == 0)
        //    return;
        //// Adjust the position of the tank based on the player's input.
        //float directionFactor = (m_MovementInputValue < 0) ? 0.5f : 1.0f;
        //if (m_CurrentSpeed >= m_MaxSpeed)
        //{
        //    m_CurrentSpeed = m_MaxSpeed;
        //    m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * m_CurrentSpeed;
        //}

        //Vector3 movementForce = transform.forward * m_MovementInputValue * directionFactor * m_Acceleration * Time.deltaTime;
        //m_Rigidbody.AddForce(movementForce);

        float directionFactor = (m_MovementInputValue < 0) ? 0.5f : 1.0f;

        Vector3 movement = transform.forward * m_MovementInputValue * directionFactor * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        Quaternion rotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * rotation);
    }
}
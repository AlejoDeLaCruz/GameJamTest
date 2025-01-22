using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, 1)][SerializeField] private float m_CrouchSpeed = .36f;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private LayerMask m_AdditionalGround;
    [SerializeField] private LayerMask m_AdditionalGround2; // Nueva capa de suelo
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;
    [SerializeField] private Collider2D m_CrouchDisableCollider;
    [SerializeField] private float m_MaxSlopeAngle = 45f; // Máximo ángulo de pendiente que el jugador puede subir

    [Range(0, 1)][SerializeField] private float m_AirControlFactor = 0.5f; // Control en el aire (1 = control completo)

    const float k_GroundedRadius = .2f;
    private bool m_Grounded;
    const float k_CeilingRadius = .2f;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;

    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    private Life m_CambiarEscala; // Variable para acceder al script CambiarEscala

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        // Obtener el componente CambiarEscala
        m_CambiarEscala = GetComponent<Life>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // Combinar todas las máscaras de suelo para la detección
        LayerMask combinedGroundMask = m_WhatIsGround | m_AdditionalGround | m_AdditionalGround2;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, combinedGroundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        if (!crouch)
        {
            LayerMask combinedGroundMask = m_WhatIsGround | m_AdditionalGround | m_AdditionalGround2;
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, combinedGroundMask))
            {
                crouch = true;
            }
        }

        if (m_Grounded || m_AirControl)
        {
            float controlFactor = m_Grounded ? 1 : m_AirControlFactor; // Reducir control en el aire

            // Detectar pendiente
            LayerMask combinedGroundMask = m_WhatIsGround | m_AdditionalGround | m_AdditionalGround2;
            RaycastHit2D hit = Physics2D.Raycast(m_GroundCheck.position, Vector2.down, 2f, combinedGroundMask);
            if (hit.collider != null)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeAngle <= m_MaxSlopeAngle)
                {
                    // El jugador puede subir la pendiente
                    if (crouch)
                    {
                        if (!m_wasCrouching)
                        {
                            m_wasCrouching = true;
                            OnCrouchEvent.Invoke(true);
                        }

                        move *= m_CrouchSpeed;

                        if (m_CrouchDisableCollider != null)
                            m_CrouchDisableCollider.enabled = false;
                    }
                    else
                    {
                        if (m_CrouchDisableCollider != null)
                            m_CrouchDisableCollider.enabled = true;

                        if (m_wasCrouching)
                        {
                            m_wasCrouching = false;
                            OnCrouchEvent.Invoke(false);
                        }
                    }
                }
                else
                {
                    // No puede subir la pendiente si es muy empinada
                    move = 0;
                }
            }

            // Obtener la escala actual del jugador (eje Y) y modificar la velocidad
            float scaleY = transform.localScale.y;
            float speedMultiplier = 4f / scaleY; // Cuanto más pequeña la escala, más rápido se mueve

            Vector2 targetVelocity = new Vector2(move * 10f * controlFactor * speedMultiplier, m_Rigidbody2D.linearVelocity.y);
            m_Rigidbody2D.linearVelocity = Vector2.Lerp(m_Rigidbody2D.linearVelocity, targetVelocity, m_MovementSmoothing);

            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }
        }

        if (m_Grounded && jump)
        {
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

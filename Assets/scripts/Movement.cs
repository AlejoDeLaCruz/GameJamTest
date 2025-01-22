using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public PlayerMovement controller;

    [SerializeField] private float baseSpeed = 4f; // Velocidad base del jugador
    [SerializeField] private float iceSpeedMultiplier; // Multiplicador de velocidad en hielo.
    [SerializeField] private ScreenFade screenFade; // Referencia al script de fade.

    private float currentRunSpeed; // Velocidad actual que puede ser modificada por objetos.

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    private bool onIceFloor = false; // Indica si el jugador está sobre hielo.

    // --- Variables para pared ---
    [SerializeField] private float wallSlideSpeed = 2f; // Velocidad al deslizarse en la pared.
    [SerializeField] private float wallJumpForce = 15f; // Fuerza al saltar desde la pared.
    private bool isTouchingWall = false; // Indica si el jugador está tocando una pared.
    private bool isWallSliding = false; // Indica si el jugador está deslizándose por una pared.
    private Rigidbody2D rb; // Referencia al Rigidbody2D del jugador.

    private void Start()
    {
        currentRunSpeed = baseSpeed; // Inicializa la velocidad actual con la velocidad base.
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D del jugador.
    }

    void Update()
    {
        // No permitir el movimiento si la pantalla está en negro.
        if (screenFade != null && screenFade.IsScreenBlack())
        {
            return; // Si la pantalla está negra, no permitir que el jugador se mueva.
        }

        // Ajusta la velocidad según el estado (sobre hielo o no).
        currentRunSpeed = onIceFloor ? baseSpeed * iceSpeedMultiplier : baseSpeed;

        horizontalMove = Input.GetAxisRaw("Horizontal") * currentRunSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;

            // Salto desde la pared.
            if (isWallSliding)
            {
                WallJump();
            }
        }

        // Detecta si el jugador debe deslizarse por la pared.
        CheckWallSliding();
    }

    void FixedUpdate()
    {
        // Mover al personaje si no está en una pared.
        if (!isWallSliding)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        }
        jump = false;
    }

    // Detectar si el jugador entra en contacto con superficies.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detecta si el jugador está en hielo.
        if (collision.gameObject.layer == LayerMask.NameToLayer("iceFloor"))
        {
            onIceFloor = true;
        }

        // Detecta si el jugador toca una superficie mortal.
        if (collision.gameObject.layer == LayerMask.NameToLayer("deadlyFloor"))
        {
            TriggerBlackScreen(); // Activa la pantalla negra.
        }

        // Detecta si el jugador toca una pared.
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
        }
    }

    // Detectar si el jugador sale de contacto con superficies.
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("iceFloor"))
        {
            onIceFloor = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
            isWallSliding = false;
        }
    }

    // Comprueba si el jugador debe deslizarse por la pared.
    private void CheckWallSliding()
    {
        if (isTouchingWall && rb.linearVelocity.y < 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed); // Reduce la velocidad de caída.
        }
        else
        {
            isWallSliding = false;
        }
    }

    // Ejecuta un salto desde la pared.
    private void WallJump()
    {
        float jumpDirection = horizontalMove > 0 ? -1 : 1; // Saltar en dirección opuesta a la pared.
        rb.linearVelocity = new Vector2(jumpDirection * baseSpeed, wallJumpForce);
        isWallSliding = false;
    }

    // Activa la pantalla negra.
    private void TriggerBlackScreen()
    {
        if (screenFade != null)
        {
            screenFade.FadeToBlackAndBack(2f); // Pantalla negra por 2 segundos.
        }
        else
        {
            Debug.LogWarning("ScreenFade no está asignado en el inspector.");
        }
    }

    // Método para ajustar la velocidad.
    public void AdjustSpeed(float speedAdjustment)
    {
        currentRunSpeed = baseSpeed + speedAdjustment; // Asegúrate de que se sume correctamente
        Debug.Log("Velocidad ajustada: " + currentRunSpeed); // Para ver en consola si la velocidad cambia
    }
}

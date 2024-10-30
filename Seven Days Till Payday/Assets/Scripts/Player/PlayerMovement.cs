using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private float move_input;
    private bool can_walk;
    public bool is_moving = false;

    // References
    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        can_walk = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        move_input = Input.GetAxis("Horizontal");
    }
    private void FixedUpdate()
    {
        if (!can_walk)
        {
            StopPlayer();
            return;
        }
        if (Mathf.Abs(move_input) > 0.1f)
        {
            rb.velocity = new Vector2(move_input * speed, rb.velocity.y);
            is_moving = true;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            is_moving = false;
        }
    }
    private void StopPlayer()
    {
        rb.velocity = Vector2.zero;
        is_moving = false;
    }
    public void DisableMovement()
    {
        can_walk = false;
    }
    public void EnableMovement()
    {
        can_walk = true;
    }
}

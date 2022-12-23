using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Ball : MonoBehaviour
{
   private AudioSource _bounceAudio;
   private Rigidbody2D _rb;
   private float _maxSpeed = Constants.BallPhysics.maxSpeed;

   private void Awake()
   {
      _bounceAudio = gameObject.GetComponent<AudioSource>();
      _rb = gameObject.GetComponent<Rigidbody2D>();
   }

   private void OnCollisionEnter2D(Collision2D col)
   {
      _bounceAudio.Play();
      float range = Constants.BallPhysics.bounceRange;
      _rb.AddForce(new Vector2(Random.Range(-range, range), Random.Range(-range, range)));
   }

   private void FixedUpdate()
   {
      _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);
   }
}

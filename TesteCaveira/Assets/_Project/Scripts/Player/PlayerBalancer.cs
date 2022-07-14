using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerBalancer", menuName = "PlayerBalancer")]
public class PlayerBalancer : ScriptableObject
{
    public float health;
    public float speed;
    public float acceleration;
    public float deceleration;
    public float jumpForce;

}
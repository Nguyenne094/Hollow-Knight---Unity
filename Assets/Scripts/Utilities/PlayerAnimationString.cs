using System;
using UnityEngine;

namespace Utilities
{
    public class PlayerAnimationString : MonoBehaviour
    {
        internal static int Walk = Animator.StringToHash("Walk");
        internal static int AttackRight = Animator.StringToHash("Attack Right");
        internal static int AttackUp = Animator.StringToHash("Attack Up");
        internal static int AttackDown = Animator.StringToHash("Attack Down");
        internal static int CanMove = Animator.StringToHash("CanMove");
        internal static int LockVelocity = Animator.StringToHash("LockVelocity");
        internal static int Hurt = Animator.StringToHash("Hurt");
        internal static int yVelocity = Animator.StringToHash("yVelocity");
        internal static int Jump = Animator.StringToHash("Jump");
        internal static int IsGrounded = Animator.StringToHash("IsGrounded");
        internal static int Idle = Animator.StringToHash("Idle");
    }
}
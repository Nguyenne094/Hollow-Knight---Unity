using UnityEngine;

namespace Utilities
{
    public class EnemyAnimationString
    {
        internal static int IsAlive = Animator.StringToHash("IsAlive");
        internal static int CanMove = Animator.StringToHash("CanMove");
        internal static int Turn = Animator.StringToHash("Turn");
        internal static int Startle = Animator.StringToHash("Startle");
        internal static int Hurt = Animator.StringToHash("Hurt");
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Script.SO
{
    [CreateAssetMenu(fileName = "Sound Effect", menuName = "Sound/Sound Effect", order = 0)]
    public class SoundEffect : ScriptableObject
    {
        public AudioSource AudioSourcePref;
        public List<AudioClip> AudioClips;
        public Vector2 VolumeRange;
    }
}
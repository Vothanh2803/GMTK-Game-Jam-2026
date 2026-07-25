using UnityEngine;

public class EnemySFXController : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private SoundFeedback lightAttackSound;
    [SerializeField] private SoundFeedback heavyAttackSound;    

    public void PlayLightAttackSound()
    {
        if (lightAttackSound != null) lightAttackSound.PlaySound();
    }

    public void PlayHeavyAttackSound()
    {
        if (heavyAttackSound != null) heavyAttackSound.PlaySound();
    }

}

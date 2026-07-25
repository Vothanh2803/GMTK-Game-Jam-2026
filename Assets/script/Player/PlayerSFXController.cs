using UnityEngine;

public class PlayerSFXController : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private SoundFeedback lightAttackSound;
    [SerializeField] private SoundFeedback heavyAttackSound;
    [SerializeField] private SoundFeedback rageAttackSound; 
    [SerializeField] private SoundFeedback blockSound;
    [SerializeField] private SoundFeedback parryHeavySound; 
    [SerializeField] private SoundFeedback parryLightSound; 

    public void PlayLightAttackSound()
    {
        if (lightAttackSound != null) lightAttackSound.PlaySound();
    }

    public void PlayHeavyAttackSound()
    {
        if (heavyAttackSound != null) heavyAttackSound.PlaySound();
    }

    public void PlayRageAttackSound()
    {
        if (rageAttackSound != null) rageAttackSound.PlaySound();
    }

    public void PlayBlockSound()
    {
        if (blockSound != null) blockSound.PlaySound();
    }
    
    public void PlayParryHeavySound()
    {
        if (parryHeavySound != null) parryHeavySound.PlaySound();
    }

    public void PlayParryLightSound()
    {
        if (parryLightSound != null) parryLightSound.PlaySound();
    }

    
}

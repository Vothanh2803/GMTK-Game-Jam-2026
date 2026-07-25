using UnityEngine;

public class PlayerSFXController : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private SoundFeedback lightAttackSound;
    [SerializeField] private SoundFeedback heavyAttackSound;
    [SerializeField] private SoundFeedback rageAttackSound; 
    [SerializeField] private SoundFeedback blockSound;
    [SerializeField] private SoundFeedback hurtSound; 
    [SerializeField] private SoundFeedback parryHeavySound; 
    [SerializeField] private SoundFeedback parryLightSound; 

    public void PlayLightAttackSound()
    {
        if (parryHeavySound != null) lightAttackSound.PlaySound();
    }

    public void PlayHeavyAttackSound()
    {
        if (parryHeavySound != null) heavyAttackSound.PlaySound();
    }

    public void PlayRageAttackSound()
    {
        if (parryHeavySound != null) rageAttackSound.PlaySound();
    }

    public void PlayBlockSound()
    {
        if (parryHeavySound != null) blockSound.PlaySound();
    }

    public void PlayHurtSound()
    {
        if (parryHeavySound != null) hurtSound.PlaySound();
    }

    public void PlayParryHeavySound()
    {
        if (parryHeavySound != null) parryHeavySound.PlaySound();
    }

    public void PlayParryLightSound()
    {
        if (parryHeavySound != null) parryLightSound.PlaySound();
    }

    
}

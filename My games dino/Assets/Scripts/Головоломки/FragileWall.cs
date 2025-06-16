using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FragileWall : MonoBehaviour
{
    public Animator animator;
    public AudioSource breakSound;
       public float destroyDelay = 0.5f;

    private bool isBreaking = false;

    public void Break()
    {
        if (isBreaking) return;
        isBreaking = true;

        if (breakSound != null)
            breakSound.Play();
        Debug.Log("🔥 Break Trigger Set");

        if (animator != null)
            animator.SetTrigger("Break");
        Debug.Log("🎬 Current state: " + animator.GetCurrentAnimatorStateInfo(0).IsName("Break"));

        Destroy(gameObject, destroyDelay);
    }
}

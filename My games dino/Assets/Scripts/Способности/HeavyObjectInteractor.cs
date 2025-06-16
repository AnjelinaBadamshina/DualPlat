using UnityEngine;
using System.Collections;

public class HeavyObjectInteractor : MonoBehaviour
{
    private float durationRemaining = 0f;

    public void EnableInteraction(float duration)
    {
        durationRemaining = duration;
        StartCoroutine(InteractionTimer());
    }

    private IEnumerator InteractionTimer()
    {
        while (durationRemaining > 0f)
        {
            durationRemaining -= Time.deltaTime;
            yield return null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (durationRemaining > 0f)
        {
            var heavy = collision.collider.GetComponent<HeavyObjectController>();
            if (heavy != null)
            {
                heavy.AllowMovement(gameObject, durationRemaining);
            }
        }
    }
}

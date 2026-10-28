using UnityEngine;

public class SeasonUI : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {   
        animator.SetInteger("Season", (int)GameTime.Instance.CurrentSeason);
        animator.SetInteger("TimeOfDay", (int)GameTime.Instance.CurrentTimeOfDay);
    }
}
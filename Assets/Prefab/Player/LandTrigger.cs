using UnityEngine;

public class LandTrigger : MonoBehaviour
{
    [SerializeField] CharacterControl characterControl;
    [SerializeField] Animator animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterControl.jumpCount = 0;
        animator.SetTrigger("land");
    }
}

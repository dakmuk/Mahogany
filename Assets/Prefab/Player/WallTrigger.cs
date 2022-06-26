using UnityEngine;
public class WallTrigger : MonoBehaviour
{
    [SerializeField] CharacterControl characterControl;
    [SerializeField] AnimationManager animationManager;
    [SerializeField] Animator animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterControl.jumpCount = 0;
        animationManager.onWall = true;
        animator.SetBool("onWall", true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        animationManager.onWall = false;
        animator.SetBool("onWall", false);
    }
}

using UnityEngine;

public class TransitionCanvas : MonoBehaviour
{
    [SerializeField] Animator anim;
	
	public void ToBlack()
	{
		if (anim != null)
			anim.SetTrigger("toBlack");
	}
	public void FromBlack()
	{
		if (anim != null)
			anim.SetTrigger("fromBlack");
	}
}

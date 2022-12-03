using UnityEngine;

public class TransitionCanvas : MonoBehaviour
{
    [SerializeField] Animator anim;
	[SerializeField] bool fromBlackOnStart;


	private void Start() {
		if (fromBlackOnStart)
			anim.SetTrigger("fromBlack");
	}
	public void FromBlack()
	{
		if (anim != null)
			anim.SetTrigger("fromBlack");
	}
	public void ToBlack()
	{
		if (anim != null)
			anim.SetTrigger("toBlack");
	}
}

using UnityEngine;

namespace Assets.HeroEditor4D.Common.Animation
{
	public class LockBoolean : StateMachineBehaviour
	{
		public string ParameterName;
		public float ExitTime;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.SetBool(ParameterName, true);
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (stateInfo.normalizedTime >= ExitTime)
			{
				animator.SetBool(ParameterName, false);
			}
		}
	}
}
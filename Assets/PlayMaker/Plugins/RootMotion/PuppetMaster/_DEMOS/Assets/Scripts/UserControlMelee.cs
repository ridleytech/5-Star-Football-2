using System.Collections;
using UnityEngine;

namespace RootMotion.Demos {
	
	/// <summary>
	/// User input for a third person melee character controller.
	/// </summary>
	public class UserControlMelee : UserControlThirdPerson {

		public KeyCode hitKey;
		public KeyCode blockKey;
		public KeyCode strafeKey;


		protected override void Update () {
			base.Update();

			//Debug.Log("hit key: " + Input.GetKey(hitKey));

			// if(Input.GetKey(strafeKey)){
			// 	state.isStrafing =true;

			// }else{
			// 	state.isStrafing =false;

			// }


			if(Input.GetKey(hitKey)){
				state.actionIndex =1;

			}

			else if(Input.GetKey(blockKey)){
				//state.actionIndex = 2;

			} else{
				state.actionIndex = 0;}

		}
	}
}

using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.MoffeinClayMan
{
	public class DeathState : GenericCharacterDeath
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (base.modelLocator)
			{
				if (base.modelLocator.modelBaseTransform)
				{
					EntityState.Destroy(base.modelLocator.modelBaseTransform.gameObject);
				}
				if (base.modelLocator.modelTransform)
				{
					EntityState.Destroy(base.modelLocator.modelTransform.gameObject);
				}
			}
			if (NetworkServer.active)
			{
				EffectManager.SimpleEffect(DeathState.initialExplosion, base.transform.position, base.transform.rotation, true);
				EntityState.Destroy(base.gameObject);
			}
		}

		public static GameObject initialExplosion = Resources.Load<GameObject>("prefabs/effects/impacteffects/claypotprojectileexplosion");
	}
}

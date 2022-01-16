using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using R2API;

namespace EntityStates.MoffeinClayMan
{
	public class SwipeForwardTar : BaseState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = SwipeForwardTar.baseDuration / this.attackSpeedStat;
			this.modelAnimator = base.GetModelAnimator();
			Transform modelTransform = base.GetModelTransform();
			this.attack = new OverlapAttack();
			this.attack.attacker = base.gameObject;
			this.attack.inflictor = base.gameObject;
			this.attack.teamIndex = base.GetTeam();
			this.attack.damage = SwipeForwardTar.damageCoefficient * this.damageStat;
			this.attack.hitEffectPrefab = SwipeForwardTar.hitEffectPrefab;
			this.attack.isCrit = RollCrit();
			this.attack.procCoefficient = 0.5f;
			this.attack.AddModdedDamageType(ClayMen.Content.ClayGooClayMan);

			Util.PlaySound(SwipeForwardTar.attackString, base.gameObject);
			if (modelTransform)
			{
				this.attack.hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Sword");
			}
			if (this.modelAnimator)
			{
				base.PlayAnimation("Gesture, Override", "SwipeForward", "SwipeForward.playbackRate", this.duration);
				base.PlayAnimation("Gesture, Additive", "SwipeForward", "SwipeForward.playbackRate", this.duration);
			}
			if (base.characterBody)
			{
				base.characterBody.SetAimTimer(2f);
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (NetworkServer.active && this.modelAnimator && this.modelAnimator.GetFloat("SwipeForward.hitBoxActive") > 0.1f)
			{
				if (!this.hasSlashed)
				{
					EffectManager.SimpleMuzzleFlash(SwipeForwardTar.swingEffectPrefab, base.gameObject, "SwingCenter", true);
					HealthComponent healthComponent = base.characterBody.healthComponent;
					CharacterDirection component = base.characterBody.GetComponent<CharacterDirection>();
					if (healthComponent)
					{
						healthComponent.TakeDamageForce(SwipeForwardTar.selfForceMagnitude * component.forward, true, false);
					}
					this.hasSlashed = true;
				}
				this.attack.forceVector = base.transform.forward * SwipeForwardTar.forceMagnitude;
				this.attack.Fire(null);
			}
			if (base.fixedAge >= this.duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		public static float baseDuration = 1f;

		public static float damageCoefficient = 1.7f;

		public static float forceMagnitude = 1000f;

		public static float selfForceMagnitude = 1600f;

		public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/squidturrethiteffect");

		public static GameObject swingEffectPrefab = Resources.Load<GameObject>("prefabs/effects/claymanswordswing");

		public static string attackString = "Play_merc_sword_swing";

		private OverlapAttack attack;
		private Animator modelAnimator;
		private float duration;
		private bool hasSlashed;
	}
}

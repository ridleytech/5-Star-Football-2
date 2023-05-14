namespace Opsive.UltimateCharacterController.Integrations.PuppetMaster
{
    using Opsive.Shared.Events;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Character.Abilities;
    using RootMotion.Dynamics;
    using UnityEngine;

    /// <summary>
    /// Works with PuppetMaster to change control between the character controller and PuppetMaster.
    /// </summary>
    [DefaultStartType(AbilityStartType.Manual)]
    [DefaultAllowPositionalInput(false)]
    [DefaultAllowRotationalInput(false)]
    public class Puppet : Ability
    {
        [Tooltip("A reference to the Ultimate Character Controller version of the Behaviour Puppet component.")]
        public BehaviourPuppet puppet;
        [Tooltip("A reference to the PuppetMaster component.")]
        public PuppetMaster puppetMaster;
        
        public override bool IsConcurrent => true;

        private bool getUpFlag;
        private CharacterIKBase ik;

        /// <summary>
        /// Initailize the default values.
        /// </summary>
        public override void Awake()
        {
            base.Awake();

            // Listen to BehaviourPuppet lose balance and regain balance events
            puppet.onLoseBalance.unityEvent.AddListener(OnLoseBalance);
            puppet.onRegainBalance.unityEvent.AddListener(OnRegainBalance);
            puppet.onGetUpProne.unityEvent.AddListener(OnGetUp);
            puppet.onGetUpSupine.unityEvent.AddListener(OnGetUp);

            EventHandler.RegisterEvent(m_GameObject, "OnAnimatorSnapped", OnAnimatorSnapped);
        }

        /// <summary>
        /// Set the correct interpolations and callbacks.
        /// </summary>
        public override void Start()
        {
            base.Start();

            if (puppetMaster.targetAnimator.updateMode != AnimatorUpdateMode.AnimatePhysics)
            {
                Debug.LogWarning("Warning: Switching Animator.updateMode to AnimatePhysics!");
                //puppetMaster.targetAnimator.updateMode = AnimatorUpdateMode.AnimatePhysics;
            }

            // Set interpolation
            foreach (Muscle m in puppetMaster.muscles) m.rigidbody.interpolation = RigidbodyInterpolation.None;
            puppetMaster.targetRoot.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;

            puppetMaster.OnRead += OnRead;

            // Get calls from BehaviourPuppet when it moves the character
            puppet.OnPostRead += OnPostRead;
            puppet.OnPostFixedUpdate += OnPostRead;

            ik = puppetMaster.targetRoot.GetComponentInChildren<CharacterIKBase>();

            //randall change

            //if (ik != null && !ik.UseOnAnimatorIK) ik.enabled = false;
        }

        public void OnAnimatorIK(int layerIndex) {}
        public void UseOnAnimatorIK() {}

        /// <summary>
        /// The Animator controller has been snapped into position.
        /// </summary>
        private void OnAnimatorSnapped()
        {
            EventHandler.UnregisterEvent(m_GameObject, "OnAnimatorSnapped", OnAnimatorSnapped);
            
            // Update the puppet positions and rotations to match the snapped avatar.
            puppetMaster.transform.position = m_Transform.position;
            puppetMaster.transform.rotation = m_Transform.rotation;

            foreach (Muscle m in puppetMaster.muscles)
            {
                m.joint.transform.position = m.target.position;
                m.joint.transform.rotation = m.target.rotation;
            }
            
        }

        void OnRead()
        {
            //randall change
            // if (ik != null && !ik.UseOnAnimatorIK)
            // {
            //     ik.UpdateSolvers(-1);
            // }
        }
        
        void OnPostRead(float deltaTime)
        {
            // Let UltimateCharacterLocomotion know that BehaviourPuppet has moved the character (while unpinned or once when switched to GetUp state)
            if (puppet.state == BehaviourPuppet.State.Unpinned || getUpFlag)
            {
                m_CharacterLocomotion.SetPositionAndRotation(puppetMaster.targetRoot.position, puppetMaster.targetRoot.rotation, false, false);
                getUpFlag = false;
            }
        }

        /// <summary>
        /// PuppetMaster is in control of the character.
        /// </summary>
        private void OnLoseBalance()
        {
            // Disable the character controller from moving the character and changing the animation while in Unpinned state
            StartAbility();
        }

        /// <summary>
        /// Called by BehaviourPuppet when it starts getting up.
        /// </summary>
        private void OnGetUp()
        {
            getUpFlag = true;
        }

        /// <summary>
        /// PuppetMaster is no longer in control of the character.
        /// </summary>
        private void OnRegainBalance()
        {
            // Resume normal character controller behaviour.
            StopAbility();
        }

        /// <summary>
        /// The character has been destroyed.
        /// </summary>
        public override void OnDestroy()
        {
            if (puppet != null)
            {
                puppet.onLoseBalance.unityEvent.RemoveListener(OnLoseBalance);
                puppet.onRegainBalance.unityEvent.RemoveListener(OnRegainBalance);
                puppet.onGetUpProne.unityEvent.RemoveListener(OnGetUp);
                puppet.onGetUpSupine.unityEvent.RemoveListener(OnGetUp);

                puppetMaster.OnRead -= OnRead;

                puppet.OnPostRead -= OnPostRead;
                puppet.OnPostFixedUpdate -= OnPostRead;
            }
        }
    }
}

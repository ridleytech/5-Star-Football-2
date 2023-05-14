using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Opsive.UltimateCharacterController.Editor.Managers;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Camera;

//randall change
//using Opsive.Shared.Editor.UIElements.Managers;
using RootMotion.Dynamics;
using System;

namespace Opsive.UltimateCharacterController.Integrations.PuppetMaster
{

    public class PuppetMasterUCCSetup : EditorWindow
    {
        private static int ragdollLayer = 9;
        private static int collisionLayer = 10;
        private LayerMask groundLayers;
        private static Transform character;
        private static GameObject ragdoll;
        private int page;

        [MenuItem("Tools/Opsive/PuppetMaster-UCC Setup")]
        static void Init()
        {
            PuppetMasterUCCSetup window = (PuppetMasterUCCSetup)EditorWindow.GetWindow(typeof(PuppetMasterUCCSetup));
            window.Show();
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.richText = true;
            style.wordWrap = true;

            GUILayout.BeginVertical("Box");
            if (page < 11)
            {
                GUILayout.Label("<b>Step " + (page + 1) + "/11</b>", style);
            }

            GUILayout.Space(10);

            switch (page)
            {
                case 0:
                    GUILayout.TextArea("Welcome to the PuppetMaster-UCC Integration Guide. Click on <b>Next</b> to continue. \n\nIf you have already done Opsive scene and project setup as well as PuppetMaster layer setup, click to <b>Skip To Step 5</b>.", style);
                    GUILayout.Space(10);
                    if (GUILayout.Button("Skip To Step 5"))
                    {
                        page = 4;
                    }
                    break;
                case 1:
                    GUILayout.TextArea("Name 2 new layers: \n\n<b>Ragdoll</b> - used for the PuppetMaster ragdoll.  \n\n<b>Collision</b> - used for anything that can cause the puppet to take damage and fall. \n\nAssign the layer numbers below and click on <b>Setup Layer Collision Matrix</b> to ignore collisions between Character-Ragdoll and Character-Collision.", style);

                    GUILayout.Space(10);
                    ragdollLayer = EditorGUILayout.IntField(new GUIContent("Ragdoll Layer"), ragdollLayer);
                    collisionLayer = EditorGUILayout.IntField(new GUIContent("Collision Layer"), collisionLayer);
                    if (GUILayout.Button("Setup Layer Collision Matrix"))
                    {
                        Physics.IgnoreLayerCollision(ragdollLayer, 31, true);
                        Physics.IgnoreLayerCollision(collisionLayer, 31, true);
                        Debug.Log("[PuppetMaster-UCC Setup] Layer Collision Matrix Updated.");
                    }
                    break;
                case 2:
                    GUILayout.TextArea("We will need to run Opsive's scene and project setup. \n\nOpen Tools/Opsive/Ultimate Character Controller/Main Manager/Setup, click on <b>Add Managers</b> and <b>Setup Camera</b> in the Setup window. \n\nChoose camera Perspective based on whether you want to make a first person or a third person game.", style);
                    break;
                case 3:
                    GUILayout.TextArea("Switch over to the <b>Project</b> tab and click on <b>Update Buttons</b> and <b>Update Layers</b>.", style);
                    break;
                case 4:
                    GUILayout.TextArea("Add a character model to the scene.", style);
                    break;
                case 5:
                    GUILayout.TextArea("Duplicate the character model. The duplicate will be used for the ragdoll. \n\nAdd <b>BipedRagdollCreator</b> to the duplicate, click on <b>Create a Ragdoll</b>. \n\nClick on <b>Done</b> if you are happy with the ragdoll or <b>Start Editing Manually</b> if you would like to edit it with the RagdollEditor.", style);
                    break;
                case 6:
                    GUILayout.TextArea("Open Tools/Opsive/Ultimate Character Controller/Character Manager, assign your <b>original</b> model into the <b>Character</b> slot in the Character Setup Window. \n\nChoose <b>Perspective</b> based on whether you want to make a first person or a third person game. \n\nDisable <b>Ragdoll</b> in the Advanced settings. \n\nClick on <b>Build Character</b>.", style);
                    break;
                case 7:
                    GUILayout.TextArea("Assign the the slots below and click on <b>Setup PuppetMaster</b>.", style);
                    GUILayout.Space(10);
                    character = (Transform)EditorGUILayout.ObjectField(new GUIContent("Character (original model)"), character, typeof(Transform), true);
                    ragdoll = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Ragdoll (duplicated model)"), ragdoll, typeof(GameObject), true);
                    ragdollLayer = EditorGUILayout.IntField(new GUIContent("Ragdoll Layer"), ragdollLayer);
                    GUILayout.Space(10);
                    if (GUILayout.Button("Setup PuppetMaster"))
                    {
                        if (ragdoll != null && character != null)
                        {
                            if (PrefabUtility.IsPartOfPrefabInstance(ragdoll)) PrefabUtility.UnpackPrefabInstance(PrefabUtility.GetNearestPrefabInstanceRoot(ragdoll), PrefabUnpackMode.Completely, InteractionMode.UserAction);

                            var pm = ragdoll.AddComponent<RootMotion.Dynamics.PuppetMaster>();
                            pm.SetUpTo(character, 31, ragdollLayer);

                            Debug.Log("[PuppetMaster-UCC Setup] PuppetMaster setup complete.");
                        }
                        else
                        {
                            Debug.LogError("Please assign the Ragdoll and Character slots before clicking on 'Setup PuppetMaster'.");
                        }
                    }
                    break;
                case 8:
                    GUILayout.TextArea("Expand the character hierarchy, then drag the <b>Puppet (UCC)</b> prefab from the integration folder and drop it under the <b>Behaviours</b> gameobject. Assign the fields below and press on <b>Setup BehaviourPuppet</b>", style);
                    GUILayout.Space(10);
                    ragdoll = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Ragdoll (duplicated model)"), ragdoll, typeof(GameObject), true);
                    collisionLayer = EditorGUILayout.IntField(new GUIContent("Collision Layer"), collisionLayer);
                    if (GUILayout.Button("Setup BehaviourPuppet"))
                    {
                        if (ragdoll != null)
                        {
                            var p = ragdoll.transform.parent.GetComponentInChildren<BehaviourPuppet>();
                            if (p != null)
                            {
                                p.collisionLayers = RootMotion.LayerMaskExtensions.Create(collisionLayer);
                                EditorUtility.SetDirty(p);
                                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

                                Debug.Log("[PuppetMaster-UCC Setup] BehaviourPuppet setup complete.");
                            } else
                            {
                                Debug.LogError("Can not find BehaviourPuppet. Did you drag the 'Puppet (UCC)' prefab from the integration folder and dropped it under the 'Behaviours' gameobject?");
                            }
                        } else
                        {
                            Debug.LogError("Please assign the Ragdoll slot before clickin on Setup BehaviourPuppet.");
                        }
                    }
                    break;
                case 9:
                    //GUILayout.TextArea("Set 'Smoothed Bones' to 0 in the <b>Ultimate Character Locomotion</b> component on the character.", style);
                    
                    
                    GUILayout.TextArea("Take the following manual steps:", style);

                    GUILayout.Space(10);
                    GUILayout.TextArea("<b>Character Layer Manager</b>", style);
                    GUILayout.TextArea("1) Add the 'Ragdoll' layer to the 'Invisible Layers' mask.", style);
                    GUILayout.TextArea("2) Remove the 'Ragdoll' and 'Collision' layers from the 'Solid Objects Layers'.", style);

                    GUILayout.Space(10);
                    GUILayout.TextArea("<b>Ultimate Character Locomotion</b>", style);
                    GUILayout.TextArea("1) Add 'Puppet' ability to 'Abilities', assign the 'Puppet' and 'Puppet Master' fields. Puppet can be found under the 'Behaviours' gameobject.", style);
                    GUILayout.TextArea("2) Expand 'Collision' and remove 'Ragdoll' and 'Collision' from the 'Collider Layer Mask'.", style);

                    GUILayout.Space(10);
                    GUILayout.TextArea("<b>Character IK</b>", style);
                    GUILayout.TextArea("Remove the 'Ragdoll' layer from the 'Layer Mask'.", style);

                    GUILayout.Space(10);
                    GUILayout.TextArea("<b>Camera Controller</b>", style);
                    GUILayout.TextArea("Assign the original character (gameobject that has all the Opsive components) as 'Character'. Camera Controller can be found on the Main Camera.", style);


                    /*
                    GUILayout.Space(10);

                    character = (Transform)EditorGUILayout.ObjectField(new GUIContent("Character (original model)"), character, typeof(Transform), true);
                    ragdollLayer = EditorGUILayout.IntField(new GUIContent("Ragdoll Layer"), ragdollLayer);
                    collisionLayer = EditorGUILayout.IntField(new GUIContent("Collision Layer"), collisionLayer);

                    GUILayout.Space(10);
                    if (GUILayout.Button("Setup Opsive Components"))
                    {
                        if (character != null)
                        {
                            var layerManager = character.GetComponent<CharacterLayerManager>();
                            layerManager.InvisibleLayers = PuppetMasterUCCHelpers.AddToMask(layerManager.InvisibleLayers, ragdollLayer);
                            layerManager.SolidObjectLayers = PuppetMasterUCCHelpers.RemoveFromMask(layerManager.SolidObjectLayers, ragdollLayer, collisionLayer);
                            EditorUtility.SetDirty(layerManager);

                            var loco = character.GetComponent<UltimateCharacterLocomotion>();
                            var abilities = loco.Abilities;
                            Array.Resize(ref abilities, abilities.Length + 1);
                            var puppetAbility = new Puppet();
                            puppetAbility.puppetMaster = character.parent.GetComponentInChildren<RootMotion.Dynamics.PuppetMaster>();
                            puppetAbility.puppet = character.parent.GetComponentInChildren<BehaviourPuppet>();
                            abilities[abilities.Length - 1] = puppetAbility;
                            loco.Abilities = abilities;
                            //loco.SmoothedBones = // No setter
                            loco.UpdateLocation = Game.KinematicObjectManager.UpdateLocation.FixedUpdate;
                            EditorUtility.SetDirty(loco);

                            var ik = character.GetComponent<CharacterIK>();
                            ik.LayerMask = PuppetMasterUCCHelpers.RemoveFromMask(ik.LayerMask, ragdollLayer);
                            EditorUtility.SetDirty(ik);

                            var animator = character.GetComponent<Animator>();
                            animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
                            EditorUtility.SetDirty(animator);

                            var cam = UnityEngine.Camera.main.GetComponent<Opsive.UltimateCharacterController.Camera.CameraController>();
                            cam.Character = character.gameObject;
                            EditorUtility.SetDirty(cam);

                            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
                        }
                        else
                        {
                            Debug.LogError("Please assign the Character slot before clicking on 'Setup Opsive Components'.");
                        }
                    }
                    */
                    break;
                case 10:
                    GUILayout.TextArea("Copy the <b>BehaviourPuppet</b> and <b>BehaviourFall</b> Sub-State Machines from the <b>Demo_PuppetMaster</b> animator controller in the integration folder and paste them into the animator controller used by the character.", style);
                    GUILayout.Space(10);
                    GUILayout.TextArea("Create a transition from the <b>BehaviourPuppet</b> Sub-State Machine to the Idle animation state.", style);
                    break;
                case 11:
                    GUILayout.TextArea("Done!", style);
                    GUILayout.Space(10);
                    GUILayout.TextArea("Remember that all the objects you wish to be able to damage the puppet and make it lose balance, such as obstacles and other ragdolls, will have to be included in BehaviourPuppet's 'Collision Layers'.", style);
                    break;
            }

            GUILayout.Space(10);

            if (page < 11)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Prev"))
                {
                    page--;
                    page = Mathf.Max(0, page);

                    OnPageOpen();
                }
                if (GUILayout.Button("Next"))
                {
                    page++;
                    page = Mathf.Min(page, 11);

                    OnPageOpen();
                }
                GUILayout.EndHorizontal();
            } else
            {
                if (GUILayout.Button("Close"))
                {
                    Close();
                }
                
            }
            
            GUILayout.EndVertical();
        }
        
        private void OnPageOpen()
        {
            switch (page)
            {
                case 2:
                   
                    /*
                    MainManagerWindow setupWindow = (MainManagerWindow)EditorWindow.GetWindow(typeof(MainManagerWindow));
                    setupWindow.Show();
                    setupWindow.Open(typeof(SetupManager));
                    */
                    return;
                case 6:
                    /*
                    MainManagerWindow characterWindow = (MainManagerWindow)EditorWindow.GetWindow(typeof(MainManagerWindow));
                    characterWindow.Show();
                    characterWindow.Open(typeof(CharacterManager));
                    */
                    return;
                case 11:
                    Debug.Log("[PuppetMaster-UCC Setup] Setup finished, fingers crossed!");
                    return;
            }
        }
    }

    public static class PuppetMasterUCCHelpers
    {
        public static LayerMask AddToMask(this LayerMask original, params int[] layerNumbers)
        {
            return original | RootMotion.LayerMaskExtensions.LayerNumbersToMask(layerNumbers);
        }

        public static LayerMask RemoveFromMask(this LayerMask original, params int[] layerNumbers)
        {
            LayerMask invertedOriginal = ~original;
            return ~(invertedOriginal | RootMotion.LayerMaskExtensions.LayerNumbersToMask(layerNumbers));
        }
    }
}

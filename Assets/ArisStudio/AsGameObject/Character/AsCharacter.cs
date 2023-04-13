﻿using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace ArisStudio.AsGameObject.Character
{
    public class AsCharacter : MonoBehaviour, IAsCharacterState, IAsCharacterMovement
    {
        private Transform asCharacterBaseTf;

        private SkeletonAnimation sa;
        private MaterialPropertyBlock mpb;
        private MeshRenderer md;

        private string materialType;

        public bool IsCommunication { get; set; }

        private const float BehaviourDuration = 1f;

        private static readonly int FillPhase = Shader.PropertyToID("_FillPhase");


        private static AsCharacter AddToGameObject(GameObject go)
        {
            var asChar = go.AddComponent<AsCharacter>();
            return asChar;
        }

        public static AsCharacter GetAsCharacterBehavior(GameObject go)
        {
            var asChar = go.GetComponent<AsCharacter>();
            if (asChar == null)
            {
                asChar = AddToGameObject(go);
            }

            return asChar;
        }

        public void AsCharacterInit(bool isCommunication)
        {
            StateInit();
            MovementInit();
            asCharacterBaseTf = transform.parent.gameObject.transform;
            IsCommunication = isCommunication;
        }

        // Character State

        private void StateInit()
        {
            sa = GetComponent<SkeletonAnimation>();
            md = GetComponent<MeshRenderer>();
            Highlight(0);
            Disappear();
        }

        public void Show()
        {
            throw new System.NotImplementedException();
        }

        public void Hide()
        {
            throw new System.NotImplementedException();
        }

        public void Appear()
        {
            gameObject.SetActive(true);
        }

        public void Disappear()
        {
            gameObject.SetActive(false);
        }

        public void Highlight(float hl)
        {
            mpb = new MaterialPropertyBlock();
            mpb.SetFloat(FillPhase, 1 - hl);
            md.SetPropertyBlock(mpb);
        }

        public void Highlight(float hl, float time)
        {
            throw new System.NotImplementedException();
        }

        public void State(string stateName)
        {
            sa.AnimationState.SetAnimation(1, stateName, true);
        }

        public void Skin(string skinName)
        {
            sa.skeleton.SetSkin(skinName);
        }

        public void Emotion(string emotionName)
        {
            throw new System.NotImplementedException();
        }

        public void Animation(string animationName)
        {
            throw new System.NotImplementedException();
        }


        // Character Movement

        private void MovementInit()
        {
            asCharacterBaseTf.localPosition = Vector3.zero;
            asCharacterBaseTf.localScale = Vector3.one;
        }

        public void X(float x)
        {
            asCharacterBaseTf.localPosition += Vector3.right * x;
        }

        public void Y(float y)
        {
            asCharacterBaseTf.localPosition += Vector3.up * y;
        }

        public void Z(float z)
        {
            asCharacterBaseTf.localPosition += Vector3.forward * z;
        }

        public void Position(float x, float y)
        {
            asCharacterBaseTf.localPosition = new Vector3(x, y, asCharacterBaseTf.localPosition.z);
        }

        public void MoveX(float x, float time = BehaviourDuration)
        {
            asCharacterBaseTf.DOMoveX(x, time);
        }

        public void MoveY(float y, float time = BehaviourDuration)
        {
            asCharacterBaseTf.DOMoveY(y, time);
        }

        public void MovePosition(float x, float y, float time = BehaviourDuration)
        {
            asCharacterBaseTf.DOMove(new Vector3(x, y, asCharacterBaseTf.localPosition.z), time);
        }

        public void Shake(float strength, float time = BehaviourDuration)
        {
            asCharacterBaseTf.DOShakePosition(time, strength);
        }

        public void ShakeX(float strength, float time = BehaviourDuration)
        {
            asCharacterBaseTf.DOShakePosition(time, Vector3.right * strength);
        }

        public void ShakeY(float strength, float time = BehaviourDuration)
        {
            asCharacterBaseTf.DOShakePosition(time, Vector3.down * strength);
        }

        public void Scale(float scale)
        {
            asCharacterBaseTf.localScale = new Vector3(scale, scale, 1);
        }

        public void Scale(float scale, float time)
        {
            asCharacterBaseTf.DOScale(new Vector3(scale, scale, 1), time);
        }

        public void Close()
        {
            asCharacterBaseTf.localScale = new Vector3(1.5f, 1.5f, 1);
            asCharacterBaseTf.localPosition += Vector3.down * 8.5f;
        }

        public void Back()
        {
            asCharacterBaseTf.localScale = Vector3.one;
            asCharacterBaseTf.localPosition += Vector3.up * 8.5f;
        }
    }
}
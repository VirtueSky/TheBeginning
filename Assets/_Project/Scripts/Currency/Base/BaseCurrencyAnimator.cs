using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Audio;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.ObjectPooling;
using VirtueSky.Tweening;
using Random = UnityEngine.Random;

namespace TheBeginning.Currency
{
    public abstract class BaseCurrencyAnimator : MonoBehaviour
    {
        protected abstract GameObject AnimationPrefab { get; }
        protected abstract SoundData CollectSound { get; }
        protected virtual SoundData SpawnSound => null;

        [SerializeField] private CurrencyVariable currency;
        [SerializeField] private GameObjectEvent addTargetEvent;
        [SerializeField] private GameObjectEvent removeTargetEvent;
        [SerializeField] private EventNoParam moveOneCurrencyDoneEvent;
        [SerializeField] private EventNoParam moveAllCurrencyDoneEvent;

        [HeaderLine("Near", false)]
        [SerializeField] protected float durationNear = 0.3f;
        [SerializeField] protected Ease easeNear = Ease.OutQuad;
        [SerializeField] protected float offsetNear = 1f;

        [HeaderLine("Target", false)]
        [SerializeField] protected float durationTarget = 0.5f;
        [SerializeField] protected Ease easeTarget = Ease.InQuad;

        [HeaderLine("General", false)]
        [SerializeField] protected float scale = 1f;
        [FormerlySerializedAs("numberCoin")]
        [SerializeField] protected int spawnCount = 10;
        [SerializeField] private Transform holder;
        [SerializeField] private int randomDelayMilliseconds = 100;
        [SerializeField] private PlaySfxEvent playSfxEvent;

        private readonly List<GameObject> targets = new();
        private readonly List<GameObject> activePrefabs = new();
        private bool hasPlayedSound;
        private bool hasRaisedFirstMove;

        private GameObject CurrentTarget => targets.Count > 0 ? targets[^1] : null;

        protected virtual void Awake()
        {
            if (AnimationPrefab == null)
            {
                return;
            }

            for (int i = 0; i < spawnCount; i++)
            {
                GameObject instance = AnimationPrefab.Spawn(GetHolder());
                instance.DeSpawn();
            }
        }

        protected virtual void OnEnable()
        {
            if (currency == null)
            {
                Debug.LogError($"[{GetType().Name}] Currency variable is not assigned.");
                return;
            }

            currency.TransactionEvent?.AddListener(OnCurrencyTransaction);
            addTargetEvent?.AddListener(AddTarget);
            removeTargetEvent?.AddListener(RemoveTarget);
        }

        protected virtual void OnDisable()
        {
            if (currency != null)
            {
                currency.TransactionEvent?.RemoveListener(OnCurrencyTransaction);
            }

            addTargetEvent?.RemoveListener(AddTarget);
            removeTargetEvent?.RemoveListener(RemoveTarget);
            targets.Clear();
        }

        private void OnCurrencyTransaction(CurrencyTransaction transaction)
        {
            if (transaction.CurrencyType != currency.CurrencyType || transaction.AppliedDelta <= 0 ||
                !transaction.HasSourcePosition)
            {
                return;
            }

            AnimateCollection(transaction.SourcePosition);
        }

        private void AddTarget(GameObject target)
        {
            if (target != null && !targets.Contains(target))
            {
                targets.Add(target);
            }
        }

        private void RemoveTarget(GameObject target)
        {
            targets.Remove(target);
        }

        private Transform GetHolder()
        {
            return holder != null ? holder : transform;
        }

        private async void AnimateCollection(Vector3 sourcePosition)
        {
            if (CurrentTarget == null || AnimationPrefab == null)
            {
                Debug.LogWarning($"[{GetType().Name}] Animation target or prefab is missing.");
                return;
            }

            hasPlayedSound = false;
            hasRaisedFirstMove = false;
            SpawnSound?.PlaySfx(playSfxEvent);

            for (int i = 0; i < spawnCount; i++)
            {
                if (randomDelayMilliseconds > 0)
                {
                    await UniTask.Delay(Random.Range(0, randomDelayMilliseconds));
                }

                GameObject instance = AnimationPrefab.Spawn(GetHolder());
                instance.transform.position = sourcePosition;
                instance.transform.localScale = Vector3.one * scale;
                activePrefabs.Add(instance);
                AnimatePrefabToTarget(instance, OnCurrencyReachedTarget);
            }
        }

        private void OnCurrencyReachedTarget(GameObject instance)
        {
            activePrefabs.Remove(instance);
            instance.DeSpawn();

            if (!hasPlayedSound)
            {
                hasPlayedSound = true;
                CollectSound?.PlaySfx(playSfxEvent);
                ScaleTargetIcon();
            }

            if (!hasRaisedFirstMove)
            {
                hasRaisedFirstMove = true;
                moveOneCurrencyDoneEvent?.Raise();
            }

            if (activePrefabs.Count == 0)
            {
                moveAllCurrencyDoneEvent?.Raise();
                OnAnimationComplete();
            }
        }

        private void AnimatePrefabToTarget(GameObject instance, Action<GameObject> onComplete)
        {
            Vector3 nearPosition = instance.transform.position + (Vector3)Random.insideUnitCircle * offsetNear;
            Tween.Create(instance.transform.position, nearPosition, durationNear).WithEase(easeNear).WithOnComplete(() =>
            {
                GameObject target = CurrentTarget;
                if (target == null)
                {
                    onComplete(instance);
                    return;
                }

                Tween.Create(instance.transform.position, target.transform.position, durationTarget)
                    .WithEase(easeTarget)
                    .WithOnComplete(() => onComplete(instance))
                    .BindToPosition(instance.transform);
            }).BindToPosition(instance.transform);
        }

        private void ScaleTargetIcon()
        {
            GameObject target = CurrentTarget;
            if (target == null)
            {
                return;
            }

            Vector3 originalScale = target.transform.localScale;
            Vector3 bounceScale = originalScale * 1.2f;
            Tween.Create(originalScale, bounceScale, durationTarget).WithEase(Ease.OutBack).WithOnComplete(() =>
            {
                Tween.Create(bounceScale, originalScale, durationTarget).WithEase(Ease.InBack)
                    .BindToLocalScale(target.transform);
            }).BindToLocalScale(target.transform);
        }

        protected virtual void OnAnimationComplete()
        {
        }
    }
}

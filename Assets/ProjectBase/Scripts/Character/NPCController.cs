using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectBase
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class NPCController : MonoBehaviour
    {
        [SerializeField] protected NavMeshAgent _agent;
        [SerializeField] protected Animator _animator;
        public Collider _collider;

        public virtual async UniTask PlayAnimAsync(string animName)
        {
            if(this.GetCancellationTokenOnDestroy().IsCancellationRequested)
                return;
            if (_animator.HasState(0, Animator.StringToHash(animName)))
            {
                _animator.Play(animName);
                _animator.Play(animName);
                await UniTask.Yield();
                await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
            }
            else
            {
                await UniTask.Yield();
                print($"没有动画<color=green>{animName}</color>");
            }
        }

        public virtual void PlayAnim(string animName)
        {
            if (_animator.HasState(0, Animator.StringToHash(animName)))
            {
                _animator.Play(animName);
            }
            else
            {
                print($"没有动画<color=green>{animName}</color>");
            }
        }

        public virtual async UniTask SitDown(Transform target)
        {
            _agent.enabled = false;
            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
            transform.rotation = target.rotation;
            transform.position = target.position;
            _animator.Play("坐下");
            await _animator.GetAsyncAnimatorMoveTrigger().FirstOrDefaultAsync(this.GetCancellationTokenOnDestroy());
            _agent.enabled = true;
        }
        
        public async UniTask WalkTo(Transform target)
        {
            _agent.enabled = true;
            _agent.isStopped = false;
            _agent.SetDestination(target.position);
            _animator.Play("走路");
            _collider.enabled = false;
            await UniTask.WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance,cancellationToken:this.GetCancellationTokenOnDestroy());
            _animator.Play("站立");
            _collider.enabled = true;
            _agent.isStopped = true;
        }

        public virtual async UniTask StandUp()
        {
            _animator.Play("站起");
            await _animator.GetAsyncAnimatorMoveTrigger().FirstAsync(this.GetCancellationTokenOnDestroy());
            _animator.Play("站立");
        }
    }
}
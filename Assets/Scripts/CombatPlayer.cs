using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayer : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private float lostControllTime;
    private Animator _animator;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    public void DoBounce(Vector2 position)
    {
        if (!_playerController.IsInvulnerable())
        {
            _animator.SetBool("isHited", true);
            StartCoroutine(LostControl());
            _playerController.Bounce(position);
        }
        StartCoroutine(_playerController.DesactivarCollision());
    }

    private IEnumerator LostControl()
    {
        _playerController.canMove = false;
        yield return new WaitForSeconds(lostControllTime);
        _playerController.canMove = true;
        _animator.SetBool("isHited", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ObjectPooler _objectPooler;
    [SerializeField] private InputListener _inputListener;

    public ObjectPooler ObjectPooler => _objectPooler;
    public InputListener InputListener => _inputListener;
}
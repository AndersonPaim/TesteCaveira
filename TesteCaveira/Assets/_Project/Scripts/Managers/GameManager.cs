using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ObjectPooler _objectPooler;

    public ObjectPooler ObjectPooler => _objectPooler;
}
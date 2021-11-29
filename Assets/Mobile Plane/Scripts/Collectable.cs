using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;

using UnityEngine;

using UnityRandom = UnityEngine.Random;
using Random = System.Random;

/// <summary>
/// the script that is attatched to collectables that the player can collect.
/// </summary>
public class Collectable : MonoBehaviour
{
    [Header("-- Appearance Settings --")]
    [SerializeField, Tooltip("The transform of the key gameobject")] private Transform visualTransform;
    [SerializeField, Tooltip("How fast the key spins")] private float visualRotateSpeed;
    [Header("-- Collection Settings --")]
    [SerializeField] private int antiRepititionNumber = 3;
    private static List<Collectable> allCollectables = new List<Collectable>();
    // this is to prevent recently collected collectables from being set to active immediately again.
    private static Queue<Collectable> lastCollectedCollectables = new Queue<Collectable>();

    private void Awake()
    {
        allCollectables.Clear();
        StartCoroutine(SetFirstCollectable());
    }

    private void Start()
    {
        allCollectables.Add(this);
    }

    /// <summary>
    /// set the first collectable
    /// </summary>
    private IEnumerator SetFirstCollectable()
    {
        yield return new WaitForSeconds(0.02f);
        int firstCollectableIndex = UnityRandom.Range(0, allCollectables.Count);
        for(int i = 0; i < allCollectables.Count; i++)
        {
            if(i != firstCollectableIndex)
            {
                allCollectables[i].gameObject.SetActive(false);
            }
            else
            {
                allCollectables[i].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// when the player interracts with the key, add to the number of keys collected and destroy the gameobject
    /// </summary>
    public void Collect()
    {
        CollectableManager.Collect();
        //adding the collectables to the queue to prevent from doubleing up.
        lastCollectedCollectables.Enqueue(this);
        if(lastCollectedCollectables.Count > antiRepititionNumber || lastCollectedCollectables.Count == allCollectables.Count)
        {
            lastCollectedCollectables.Dequeue();
        }
        //shuffle the list of collectables and select a random one which isn't in lastCollectedCollectables to active;
        Random rnd = new Random();
        List<Collectable> randomized = allCollectables.OrderBy(_item => rnd.Next()).ToList();
        foreach(Collectable collectable in randomized)
        {
            if(!lastCollectedCollectables.Contains(collectable))
            {
                Collectable newCollectable = collectable;
                if(newCollectable)
                {
                    if(newCollectable != this)
                    {
                        newCollectable.gameObject.SetActive(true);
                        gameObject.SetActive(false);
                    }
                    break;
                }
            }
        }
        
        
    }

    // let the player just pick up the keys by just going over them.
    private void OnTriggerEnter(Collider _other)
    {
        if(_other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void FixedUpdate()
    {
        if(visualTransform)
        {
            visualTransform.Rotate(Vector3.up * (visualRotateSpeed * Time.fixedDeltaTime));
        }
    }
}

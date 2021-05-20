using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using UnityEngine.UI;

public class _Manager : MonoBehaviour
{

    [SerializeField] Transform playerSpawnPosition;
    [SerializeField] Button player1;
    [SerializeField] private List<AssetReference> _playerReferences;


    private readonly Dictionary<AssetReference, List<GameObject>> _spawnedPlayers = new Dictionary<AssetReference, List<GameObject>>();

    private readonly Dictionary<AssetReference, Queue<Vector3>> _queuedSpwanRequest = new Dictionary<AssetReference, Queue<Vector3>>();

    private readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> _asyncOperationHandles = new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();


    private void Start()
    {
        player1.onClick.Invoke();
    }
    public void targetFound()
    {
        player1.onClick.Invoke();
    }
    public void Spawn(int index)
    {

        if (index < 0 || index > _playerReferences.Count)
            return;

        AssetReference assetReference = _playerReferences[index];

        if (assetReference.RuntimeKeyIsValid() == false)
        {
            Debug.Log("Invalid key");
            return;
        }

        if (_asyncOperationHandles.ContainsKey(assetReference))
        {
            if (_asyncOperationHandles[assetReference].IsDone)
            {
                spawnPlayerFromLoadReference(assetReference, GetPosition());
            }
            else
            EnqueueSpawnForAfterInitialization(assetReference);

            return;

        }

        LoadAndSpawn(assetReference);



    }

    private void EnqueueSpawnForAfterInitialization(AssetReference assetReference)
    {
        if (_queuedSpwanRequest.ContainsKey(assetReference) == false)
            _queuedSpwanRequest[assetReference] = new Queue<Vector3>();
        _queuedSpwanRequest[assetReference].Enqueue(GetPosition());

    }

    private void LoadAndSpawn(AssetReference assetReference)
    {
        var op = Addressables.LoadAssetAsync<GameObject>(assetReference);
        _asyncOperationHandles[assetReference] = op;
        op.Completed+=(operation) =>

        {
            spawnPlayerFromLoadReference(assetReference, GetPosition());

            if (_queuedSpwanRequest.ContainsKey(assetReference))
            {
                while (_queuedSpwanRequest[assetReference]?.Any() == true)
                {
                    var position = _queuedSpwanRequest[assetReference].Dequeue();
                    spawnPlayerFromLoadReference(assetReference, position);
                }
            }


        };
    }

    private Vector3 GetPosition()
    {
        return new Vector3(playerSpawnPosition.position.x, playerSpawnPosition.position.y, playerSpawnPosition.position.z);
    }


    private void spawnPlayerFromLoadReference(AssetReference assetReference, Vector3 position)
    {
           assetReference.InstantiateAsync(position, Quaternion.identity).Completed += (asyncOperationHandle) =>
           {
               if (_spawnedPlayers.ContainsKey(assetReference) == false)
               {
                   _spawnedPlayers[assetReference] = new List<GameObject>();
               }
               _spawnedPlayers[assetReference].Add(asyncOperationHandle.Result);
               var notify = asyncOperationHandle.Result.AddComponent<NotifyOnDisable>();
               notify.Destroyed += Remove;
               notify.AssetReference = assetReference;
           };


    }

    private void Remove(AssetReference assetReference, NotifyOnDisable obj)
    {
        Addressables.ReleaseInstance(obj.gameObject);

        _spawnedPlayers[assetReference].Remove(obj.gameObject);
        if (_spawnedPlayers[assetReference].Count == 0)
        {
            Debug.Log("All removed");

            if (_asyncOperationHandles[assetReference].IsValid())
                Addressables.Release(_asyncOperationHandles[assetReference]);

            _asyncOperationHandles.Remove(assetReference);
        }
    }
   
}

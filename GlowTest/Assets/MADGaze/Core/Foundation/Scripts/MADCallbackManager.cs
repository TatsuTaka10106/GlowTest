using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using MADGazeSDK;

public class MADCallbackManager : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

	public void Update() {
		lock(_executionQueue) {
			while (_executionQueue.Count > 0) {
				_executionQueue.Dequeue().Invoke();
			}
		}
	}

	private void Enqueue(IEnumerator action) {
		lock (_executionQueue) {
			_executionQueue.Enqueue (() => {
				StartCoroutine (action);
			});
		}
	}

	public void Enqueue(Action action)
	{
		Enqueue(ActionWrapper(action));
	}
	
	public Task EnqueueAsync(Action action)
	{
		var t = new TaskCompletionSource<bool>();
		Enqueue(ActionWrapper(()=> {
			try 
			{
				action();
				t.TrySetResult(true);
			} catch (Exception ex) 
			{
				t.TrySetException(ex);
			}
		}));
		return t.Task;
	}

	
	IEnumerator ActionWrapper(Action callback)
	{
		callback();
		yield return null;
	}

	private static MADCallbackManager _instance = null;

	public static MADCallbackManager Instance {
		get {
			if (_instance == null) {
				throw new Exception ("MADCallbackManager could not find the MADCallbackManager object.");
			}
			return _instance;
		}
	}


	void Awake() {
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}

	void OnDestroy() {
		_instance = null;
	}


	/** Settings Loading Start **/
	static MADGazeSDKSettings settings;
    public static String SDK_API_KEY
    {
        get
        {
            if (!settings)
            {
                settings = Resources.Load<MADGazeSDKSettings>("MAD Gaze/SDK Settings");
            }
            return settings.APIKey;
        }
    }
	/** Settings Loading End **/
}

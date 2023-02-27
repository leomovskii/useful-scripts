using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/* 
 All Unity processes happen on the same main thread.
 When we want to bring information into the game from outside,
 it will always be in a different thread.
 This script link receives commands from anywhere and runs in Unity main thread.
*/

public class UnityMainThreadDispatcher : MonoBehaviour {

	private static readonly string ERROR_NOT_FIND = "UnityMainThreadDispatcher could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor prefab to your scene.";

	private static UnityMainThreadDispatcher _Instance;
	private readonly Queue<Action> _ActionsQueue = new Queue<Action>();

	public static UnityMainThreadDispatcher instance {
		get {
			if (_Instance == null)
				throw new Exception(ERROR_NOT_FIND);
			return _Instance;
		}
	}

	private void Awake() {
		if (_Instance == null) {
			_Instance = this;
			DontDestroyOnLoad(gameObject);
		} else
			Destroy(this);
	}

	private void Update() {
		lock (_ActionsQueue)
			while (_ActionsQueue.Count > 0)
				_ActionsQueue.Dequeue().Invoke();
	}

	public void Enqueue(IEnumerator action) {
		lock (_ActionsQueue)
			_ActionsQueue.Enqueue(() => StartCoroutine(action));
	}

	public void Enqueue(Action action) {
		Enqueue(ActionWrapper(action));
	}

	public Task EnqueueAsync(Action action) {
		var t = new TaskCompletionSource<bool>();

		void OnAction() {
			try {
				action();
				t.TrySetResult(true);
			} catch (Exception ex) {
				t.TrySetException(ex);
			}
		}

		Enqueue(ActionWrapper(OnAction));
		return t.Task;
	}

	private IEnumerator ActionWrapper(Action action) {
		action();
		yield return null;
	}
}

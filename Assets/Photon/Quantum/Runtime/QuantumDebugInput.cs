namespace Quantum {
  using Photon.Deterministic;
  using UnityEngine;

  /// <summary>
  /// A Unity script that creates empty input for any Quantum game.
  /// </summary>
  public class QuantumDebugInput : MonoBehaviour {

    private void OnEnable() {
      QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    public void PollInput(CallbackPollInput callback) {
      Quantum.Input i = new Quantum.Input();
      i.Direction = new FPVector2(UnityEngine.Input.GetAxis("Horizontal").ToFP(), UnityEngine.Input.GetAxis("Vertical").ToFP());
      callback.SetInput(i, DeterministicInputFlags.Repeatable);
    }
  }
}

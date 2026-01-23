namespace Quantum {
  using UnityEngine;

  public class LookAtCamera : QuantumEntityViewComponent <CameraViewContext>
  {
        void Update()
        {
            transform.LookAt(ViewContext.virtualCamera.transform);
        }
    }
}

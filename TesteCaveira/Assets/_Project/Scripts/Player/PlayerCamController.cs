using UnityEngine;
using Managers;

public class PlayerCamController : MonoBehaviour
{
    public delegate void CameraRotateHandler(float yRot, float xRot);
    public CameraRotateHandler OnCameraRotate;

    [SerializeField] private InputListener _inputListener;
    [SerializeField] private float _sensitivity;

    private void Start()
    {
        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        _inputListener.OnInput += ReceiveInputs;
    }

    private void RemoveDelegates()
    {
        _inputListener.OnInput -= ReceiveInputs;
    }

    private void ReceiveInputs(InputData inputData)
    {
        CamMovement(inputData.LookX, inputData.LookY);
    }

    private void CamMovement(float lookX, float lookY)
    {
        float mouseX = lookX * _sensitivity * Time.deltaTime;
        float mouseY = lookY * -_sensitivity * Time.deltaTime;

        OnCameraRotate?.Invoke(mouseX, mouseY);
    }
}
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static MobileInput Instance;

    public bool leftPressed;
    public bool rightPressed;
    public bool centerPressed;

    public GameObject mobileUI;

    public bool IsMobileControlsEnabled { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        IsMobileControlsEnabled = true;

#elif UNITY_WEBGL
        IsMobileControlsEnabled = Application.isMobilePlatform;

#else
        IsMobileControlsEnabled = false;
#endif

        if (mobileUI != null)
            mobileUI.SetActive(IsMobileControlsEnabled);
    }

    public void LeftDown()
    {
        leftPressed = true;
    }

    public void LeftUp()
    {
        leftPressed = false;
    }

    public void RightDown()
    {
        rightPressed = true;
    }

    public void RightUp()
    {
        rightPressed = false;
    }

    public void CenterDown()
    {
        centerPressed = true;
    }

    public void CenterUp()
    {
        centerPressed = false;
    }
}
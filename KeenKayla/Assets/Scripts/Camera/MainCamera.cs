using UnityEngine;
using System.Collections;

// original code on github here: https://gist.github.com/Jellybit/9f6182c25bceac06db31

public class MainCamera : MonoBehaviour 
{
    public static MainCamera instance;
    new public Camera camera;

    [Header("Basic Setup")]
    [Tooltip("Drag your player, or whatever object you want the camera to track here. If you need to get the player by name, there's a line in the code you can uncomment.")]
    public GameObject player;

    [Tooltip("Should be at least 1.1, as for this to work, the camera has to move faster than the player. Otherwise, it behaves as if the camera is locked to the player.")]
    [Range(1, 10)]
    public float scrollMultiplierX = 1.8f;
    [Range(1, 10)]
    public float scrollMultiplierY = 1f;
    [Space(10)]
    [Tooltip("The player will be kept within this area on the screen. If you have trouble visualizing it, turn on the Debug Visuals below and press play to see it.")]
    public Vector2 movementWindowSize = new Vector2(8, 6);

    [Tooltip("If the root of your character is at the feet, you can set this offset to half the player's height to compensate. You can also just use it to keep the box low to the ground or whatever you like.")]
    public Vector2 windowOffset;

    // Activate your position limitations for the Y axis by turning this on.

    [Header("Camera Movement Boundaries")]
    [Tooltip("Turn this on to have the camera use the positional limits set below. Set both limits on an axis to the same number to lock the camera so that it only moves on the other axis.")]
    public bool limitCameraMovementX = false;
    public bool limitCameraMovementY = false;
    [Space(10)]
    [Tooltip("Set the leftmost position you want the camera to be able to go.")]
    public float limitLeft;
    [Tooltip("Set the rightmost position you want the camera to be able to go.")]
    public float limitRight;
    [Space(10)]
    [Tooltip("Set the lowest position you want the camera to be able to go.")]
    public float limitBottom;
    [Tooltip("Set the highest position you want the camera to be able to go.")]
    public float limitTop;


    [Header("Debug Visuals")]
    [Tooltip("Draws a debug box on the screen while the game is running so you can see the boundaries against the player. Red boundaries mean that they are being ignored due to the following options.")]
    public bool showDebugBoxes = false;

    [HideInInspector]
    public bool activeTracking = true;

    private Vector3 cameraPosition;
    private Vector3 playerPosition;
    private Vector3 previousPlayerPosition;
    private Rect windowRect;

    private Vector3 _shakeOffset;
    private Vector3 _lastShakeOffset;

    private IEnumerator _limitTween;
    private bool tweening;

    private void Awake()
    {
        instance = this;
        camera = GetComponent<Camera>();
    }

    private void Start()
    {
        cameraPosition = transform.position;

        //Uncomment the following if you need to get the player by name.
        //player = GameObject.Find ( "Player Name" );

        if (player == null)
            Debug.LogError("You have to let the CameraControl script know which object is your player.");

        previousPlayerPosition = player.transform.position;

        ValidateLeftAndRightLimits();
        ValidateTopAndBottomLimits();

        //These are the root x/y coordinates that we will use to create our boundary rectangle.
        //Starts at the lower left, and takes the offset into account.
        float windowAnchorX = cameraPosition.x - movementWindowSize.x / 2 + windowOffset.x;
        float windowAnchorY = cameraPosition.y - movementWindowSize.y / 2 + windowOffset.y;

        //From our anchor point, we set the size of the window based on the public variable above.
        windowRect = new Rect(windowAnchorX, windowAnchorY, movementWindowSize.x, movementWindowSize.y);
    }


    private void LateUpdate()
    {
        playerPosition = player.transform.position;
        cameraPosition = transform.position - _lastShakeOffset;

        if (activeTracking && playerPosition != previousPlayerPosition)
        {
            //Get the distance of the player from the camera.
            Vector3 playerPositionDifference = playerPosition - previousPlayerPosition;

            //Move the camera this direction, but faster than the player moved.
            Vector3 multipliedDifference = playerPositionDifference;
            multipliedDifference.x *= scrollMultiplierX;
            multipliedDifference.y *= scrollMultiplierY;

            cameraPosition.x += multipliedDifference.x;
            if (PlayerController.instance.groundedCheck.onGround)
            {
                cameraPosition.y += multipliedDifference.y;
            }

            //updating our movement window root location based on the current camera position
            windowRect.x = cameraPosition.x - movementWindowSize.x / 2 + windowOffset.x;
            windowRect.y = cameraPosition.y - movementWindowSize.y / 2 + windowOffset.y;

            // We may have overshot the boundaries, or the player just may have been moving too 
            // fast/popped into another place. This corrects for those cases, and snaps the 
            // boundary to the player.
            if (!windowRect.Contains(playerPosition))
            {
                Vector3 positionDifference = playerPosition - cameraPosition;
                positionDifference.x -= windowOffset.x;
                positionDifference.y -= windowOffset.y;

                //I made a function to figure out how much to move in order to snap the boundary to the player.
                cameraPosition.x += DifferenceOutOfBounds(positionDifference.x, movementWindowSize.x);


                cameraPosition.y += DifferenceOutOfBounds(positionDifference.y, movementWindowSize.y);

            }
        }

        cameraPosition.x = Mathf.Round(32 * cameraPosition.x) / 32f;
        cameraPosition.y = Mathf.Round(32 * cameraPosition.y) / 32f;

        if (tweening || playerPosition != previousPlayerPosition)
        {
            // Here we clamp the desired position into the area declared in the limit variables.
            if (limitCameraMovementY)
            {
                cameraPosition.y = Mathf.Clamp(cameraPosition.y, limitBottom, limitTop);
            }

            if (limitCameraMovementX)
            {
                cameraPosition.x = Mathf.Clamp(cameraPosition.x, limitLeft, limitRight);
            }
        }

        // and now we're updating the camera position using what came of all the calculations above.
        transform.position = cameraPosition + _shakeOffset;

        previousPlayerPosition = playerPosition;           

        // This draws the camera boundary rectangle
        if (showDebugBoxes) DrawDebugBox();
    }

    private static float DifferenceOutOfBounds(float differenceAxis, float windowAxis)
    {
        if (Mathf.Abs(differenceAxis) <= windowAxis / 2)
            return 0f;
        else
            return differenceAxis - (windowAxis / 2) * Mathf.Sign(differenceAxis);
    }

    private void ValidateTopAndBottomLimits()
    {
        if (limitTop < limitBottom)
        {
            Debug.LogError("You have set the limitBottom (" + limitBottom + ") to a higher number than limitTop (" + limitTop + "). This makes no sense as the top has to be higher than the bottom.");
            Debug.LogError("I'm correcting this for you, but please make sure the bottom is under the top next time. If you meant to lock the camera, please set top and bottom limits to the same number.");

            float tempFloat = limitTop;

            limitTop = limitBottom;
            limitBottom = tempFloat;
        }
    }

    private void ValidateLeftAndRightLimits()
    {
        if (limitRight < limitLeft)
        {
            Debug.LogError("You have set the limitLeft (" + limitLeft + ") to a higher number than limitRight (" + limitRight + "). This makes no sense as it puts the left limit to the right of the right limit.");
            Debug.LogError("I'm correcting this for you, but please make sure the left limit is to the left of the right limit. If you meant to lock the camera, please set left and right limits to the same number.");

            float tempFloat = limitRight;

            limitRight = limitLeft;
            limitLeft = tempFloat;
        }
    }

    private void DrawDebugBox()
    {
        Vector3 cameraPos = transform.position;

        //This will draw the boundaries you are tracking in green, and boundaries you are ignoring in red.
        windowRect.x = cameraPos.x - movementWindowSize.x / 2 + windowOffset.x;
        windowRect.y = cameraPos.y - movementWindowSize.y / 2 + windowOffset.y;

        Color xColorA;
        Color xColorB;
        Color yColorA;
        Color yColorB;

        if (!activeTracking || limitCameraMovementY && cameraPos.x <= limitLeft)
            xColorA = Color.red;
        else
            xColorA = Color.green;

        if (!activeTracking || limitCameraMovementY && cameraPos.x >= limitRight)
            xColorB = Color.red;
        else
            xColorB = Color.green;

        if (!activeTracking || limitCameraMovementY && cameraPos.y <= limitBottom)
            yColorA = Color.red;
        else
            yColorA = Color.green;

        if (!activeTracking || limitCameraMovementY && cameraPos.y >= limitTop)
            yColorB = Color.red;
        else
            yColorB = Color.green;

        Vector3 actualWindowCorner1 = new Vector3(windowRect.xMin, windowRect.yMin, 0);
        Vector3 actualWindowCorner2 = new Vector3(windowRect.xMax, windowRect.yMin, 0);
        Vector3 actualWindowCorner3 = new Vector3(windowRect.xMax, windowRect.yMax, 0);
        Vector3 actualWindowCorner4 = new Vector3(windowRect.xMin, windowRect.yMax, 0);

        Debug.DrawLine(actualWindowCorner1, actualWindowCorner2, yColorA);
        Debug.DrawLine(actualWindowCorner2, actualWindowCorner3, xColorB);
        Debug.DrawLine(actualWindowCorner3, actualWindowCorner4, yColorB);
        Debug.DrawLine(actualWindowCorner4, actualWindowCorner1, xColorA);

        // And now we display the camera limits. If the camera is inactive, they will show in red.
        // There is an x in the middle of the screen to show what hits against the limit.
        if (limitCameraMovementY)
        {
            Color limitColor;

            if (!activeTracking)
                limitColor = Color.red;
            else
                limitColor = Color.cyan;

            Vector3 limitCorner1 = new Vector3(limitLeft, limitBottom, 0);
            Vector3 limitCorner2 = new Vector3(limitRight, limitBottom, 0);
            Vector3 limitCorner3 = new Vector3(limitRight, limitTop, 0);
            Vector3 limitCorner4 = new Vector3(limitLeft, limitTop, 0);

            Debug.DrawLine(limitCorner1, limitCorner2, limitColor);
            Debug.DrawLine(limitCorner2, limitCorner3, limitColor);
            Debug.DrawLine(limitCorner3, limitCorner4, limitColor);
            Debug.DrawLine(limitCorner4, limitCorner1, limitColor);

            //And a little center point

            Vector3 centerMarkCorner1 = new Vector3(cameraPos.x - 0.1f, cameraPos.y + 0.1f, 0);
            Vector3 centerMarkCorner2 = new Vector3(cameraPos.x + 0.1f, cameraPos.y - 0.1f, 0);
            Vector3 centerMarkCorner3 = new Vector3(cameraPos.x - 0.1f, cameraPos.y - 0.1f, 0);
            Vector3 centerMarkCorner4 = new Vector3(cameraPos.x + 0.1f, cameraPos.y + 0.1f, 0);

            Debug.DrawLine(centerMarkCorner1, centerMarkCorner2, Color.cyan);
            Debug.DrawLine(centerMarkCorner3, centerMarkCorner4, Color.cyan);
        }
    }

    public void StartTweenLimitsX(float leftLimit, float rightLimit)
    {
        if (!limitCameraMovementX)
        {
            ActivateLimitsX(leftLimit, rightLimit);
            return;
        }

        if (_limitTween != null)
        {
            StopCoroutine(_limitTween);
        }

        _limitTween = TweenLimitsX(leftLimit, rightLimit);
        StartCoroutine(_limitTween);
    }

    public void StartTweenLimitsY(float bottomLimit, float topLimit)
    {
        if(!limitCameraMovementY)
        {
            ActivateLimitsY(bottomLimit, topLimit);
            return;
        }

        if (_limitTween != null)
        {
            StopCoroutine(_limitTween);
        }

        _limitTween = TweenLimitsY(bottomLimit, topLimit);
        StartCoroutine(_limitTween);
    }

    public void StartTweenLimits(float leftLimit, float rightLimit, float bottomLimit, float topLimit)
    {
        if (_limitTween != null)
        {
            StopCoroutine(_limitTween);
        }

        _limitTween = TweenLimits(leftLimit, rightLimit, bottomLimit, topLimit);
        StartCoroutine(_limitTween);
    }

    private IEnumerator TweenLimitsX(float leftLimit, float rightLimit)
    {
        tweening = true;
        limitCameraMovementX = true;

        bool done = false;
        while (!done)
        {
            done = true;
            var speed = 5 * Time.deltaTime;

            if (leftLimit != limitLeft)
            {
                done = false;
                limitLeft = Mathf.MoveTowards(limitLeft, leftLimit, speed);
            }

            if (rightLimit != limitRight)
            {
                done = false;
                limitRight = Mathf.MoveTowards(limitRight, rightLimit, speed);
            }

            yield return null;
        }

        tweening = false;

        ValidateLeftAndRightLimits();
    }

    private IEnumerator TweenLimitsY(float bottomLimit, float topLimit)
    {
        tweening = true;
        limitCameraMovementY = true;

        bool done = false;

        while (!done)
        {
            done = true;
            var speed = 5 * Time.deltaTime;

            if (topLimit != limitTop)
            {
                done = false;
                limitTop = Mathf.MoveTowards(limitTop, topLimit, speed);
            }

            if (bottomLimit != limitBottom)
            {
                done = false;
                limitBottom = Mathf.MoveTowards(limitBottom, bottomLimit, speed);
            }

            yield return null;
        }

        tweening = false;
        ValidateTopAndBottomLimits();
    }

    private IEnumerator TweenLimits(float leftLimit, float rightLimit, float bottomLimit, float topLimit)
    {
        tweening = true;
        limitCameraMovementX = true;
        limitCameraMovementY = true;

        bool done = false;
        while(!done)
        {
            done = true;
            var speed = 5 * Time.deltaTime;

            if (leftLimit != limitLeft)
            {
                done = false;
                limitLeft = Mathf.MoveTowards(limitLeft, leftLimit, speed);
            }

            if (rightLimit != limitRight)
            {
                done = false;
                limitRight = Mathf.MoveTowards(limitRight, rightLimit, speed);
            }

            if (bottomLimit != limitBottom)
            {
                done = false;
                limitBottom = Mathf.MoveTowards(limitBottom, bottomLimit, speed);
            }

            if (topLimit != limitTop)
            {
                done = false;
                limitTop = Mathf.MoveTowards(limitTop, topLimit, speed);
            }

            yield return null;
        }

        tweening = false;
        ValidateLeftAndRightLimits();
        ValidateTopAndBottomLimits();
    }

    public void ActivateLimits(float leftLimit, float rightLimit, float bottomLimit, float topLimit)
    {
        ActivateLimitsX(leftLimit, rightLimit);
        ActivateLimitsY(bottomLimit, topLimit);
    }

    public void ActivateLimitsX(float leftLimit, float rightLimit)
    {
        limitLeft = leftLimit;
        limitRight = rightLimit;
        ValidateLeftAndRightLimits();
        limitCameraMovementX = true;
    }

    public void ActivateLimitsY(float bottomLimit, float topLimit)
    {
        limitBottom = bottomLimit;
        limitTop = topLimit;
        ValidateTopAndBottomLimits();
        limitCameraMovementY = true;
    }

    public void DeactivateLimits()
    {
        limitCameraMovementX = false;
        limitCameraMovementY = false;
    }

    public void MoveCamera(Vector3 targetPosition, float moveSpeed)
    {
        StartCoroutine(MoveToPosition(targetPosition, moveSpeed));
    }

    public void ZoomCamera(float zValue, float zoomSpeed)
    {
        StartCoroutine(ZoomToZValue(zValue, zoomSpeed));
    }

    public IEnumerator MoveToPosition(Vector3 targetPosition, float moveSpeed)
    {
        var origActiveTracking = activeTracking;
        activeTracking = false;

        while (transform.position != targetPosition)
        {
            targetPosition.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        activeTracking = origActiveTracking;
    }

    public IEnumerator ZoomToZValue(float zValue, float zoomSpeed)
    {
        yield return new WaitForEndOfFrame();

        var sign = Mathf.Sign(zValue - transform.position.z);

        while (transform.position.z != zValue)
        {
            var positon = transform.position;
            positon.z += zoomSpeed * Time.deltaTime * sign;
            if((sign < 0 && positon.z < zValue) || sign > 0 && positon.z > zValue)
            {
                positon.z = zValue;
            }
            transform.position = positon;
            yield return 0;
        }
    }

    public void Shake(float time, float magnitude = 0.2f, float speed = 6f)
    {
        StartCoroutine(ShakeCamera(time, magnitude, speed));
    }

    public IEnumerator ShakeCamera(float time, float magnitude, float speed)
    {
        var timer = 0f;
        while (timer < time)
        {
            var targetOffset = Random.insideUnitSphere * magnitude;
            while (_shakeOffset != targetOffset & timer < time)
            {
                timer += Time.deltaTime;
                _lastShakeOffset = _shakeOffset;
                _shakeOffset = Vector3.MoveTowards(_shakeOffset, targetOffset, speed * Time.deltaTime);
                yield return null;
            }
        }

        while (_shakeOffset != Vector3.zero)
        {
            _lastShakeOffset = _shakeOffset;
            _shakeOffset = Vector3.MoveTowards(_shakeOffset, Vector3.zero, speed * Time.deltaTime);
            yield return null;
        }

        _lastShakeOffset = Vector3.zero;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}

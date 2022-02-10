using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Camera theCamera;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Vector3 screenOffset = new Vector3(0, 30, 0);

    private PlayerManager target;
    private Renderer targetRenderer;

    private void Start()
    {
        theCamera = Camera.main;
        Transform canvas = FindObjectOfType<Canvas>().transform;
        transform.SetParent(canvas, false);
    }

    public void SetTarget(PlayerManager target)
    {
        Assert.IsNotNull(target);

        this.target = target;
        targetRenderer = target.GetComponentInChildren<Renderer>();

        playerNameText.text = target.photonView.Owner.NickName;
    }

    private void Update()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }

        playerHealthSlider.value = target.Health;
    }

    private void LateUpdate()
    {
        if (!targetRenderer.isVisible)
        {
            transform.position = new Vector3(-1000f, -1000f, 0);
            return;
        }

        Vector3 targetPos = target.transform.position;
        float targetMaxY = targetRenderer.bounds.max.y;
        Vector3 worldPos = new Vector3(targetPos.x, targetMaxY, targetPos.z);
        transform.position = theCamera.WorldToScreenPoint(worldPos) + screenOffset;
    }
}

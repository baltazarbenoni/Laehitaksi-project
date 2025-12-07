using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] RectTransform contentPanelTransform;
    [SerializeField] HorizontalLayoutGroup contentHLG;
    Vector3 targetPos;
    Vector3 pageStep;
    Vector3 contentPanelLastPosition;

    float dragThreshold;

    [SerializeField] RectTransform[] pages;


    private void Awake()
    {
        pageStep = new Vector3(-(pages[0].rect.width + contentHLG.spacing), 0, 0);
    }
    private void Start()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            RectTransform RT = Instantiate(pages[i % pages.Length], contentPanelTransform);
            RT.SetAsLastSibling();
        }
        for (int i = 0; i < pages.Length; i++)
        {
            int num = pages.Length - i - 1;
            while (num < 0)
            {
                num += pages.Length;
            }
            RectTransform RT = Instantiate(pages[num], contentPanelTransform);
            RT.SetAsFirstSibling();
        }
        contentPanelTransform.localPosition = new Vector3(0 - (pages.Length * (pages[0].rect.width + contentHLG.spacing)), contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z);
        targetPos = contentPanelTransform.localPosition;
        contentPanelLastPosition = contentPanelTransform.localPosition;

    }

    private void Update()
    {
        if (contentPanelTransform.localPosition.x > 0)
        {
            Canvas.ForceUpdateCanvases();
            contentPanelTransform.localPosition -= new Vector3(pages.Length * (pages[0].rect.width + contentHLG.spacing), contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z);
        }
        if (contentPanelTransform.localPosition.x < 0 - (pages.Length * (pages[0].rect.width + contentHLG.spacing)))
        {
            Canvas.ForceUpdateCanvases();
            contentPanelTransform.localPosition += new Vector3(pages.Length * (pages[0].rect.width + contentHLG.spacing), contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z);
        }
    }

    public void Next()
    {
        if (targetPos.x < 0 - (pages.Length - 1) * pages[0].rect.width + contentHLG.spacing)
        {
            targetPos.x = 0;
        }

        else
        {
            targetPos = contentPanelLastPosition + pageStep;
        }
        MovePage();
        contentPanelLastPosition = contentPanelTransform.localPosition;

    }

    public void Previous()
    {
        if (targetPos.x >= 0)
        {
            targetPos.x = -((pages.Length - 1) * (pages[0].rect.width + contentHLG.spacing));
        }

        else
        {
            targetPos = contentPanelLastPosition - pageStep;
        }
        MovePage();
        contentPanelLastPosition = contentPanelTransform.localPosition;

    }

    void MovePage()
    {

        contentPanelTransform.localPosition = new Vector3(targetPos.x, contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshold)
        {
            if (eventData.position.x > eventData.pressPosition.x)
            {
                Previous();
            }
            else Next();
        }
        else
        {
            MovePage();
        }
    }
}

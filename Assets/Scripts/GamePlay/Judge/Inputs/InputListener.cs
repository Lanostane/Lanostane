using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay.Judge.Inputs
{
    public class InputListener : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform GamePlayScreenRect;
        public Camera GameCamera;
        public Camera UICamera;

        public void OnBeginDrag(PointerEventData eventData)
        {
            GetWorldPosition(eventData.position, out var worldPos, out var canSendEvent);
            Debug.DrawLine(Vector3.zero, worldPos, Color.white, 0.5f);

            if (NoteJudgeManager.Instance.TryGetInputHandle(eventData.pointerId, out var handle))
            {
                handle.InValidPosition = canSendEvent;
                handle.EventType = InputEvent.StartDrag;
                handle.SetDegreeByWorldPosition(worldPos);
                handle.Dragging = true;
                NoteJudgeManager.Instance.InputHandleUpdated(handle);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            GetWorldPosition(eventData.position, out var worldPos, out var canSendEvent);
            Debug.DrawLine(Vector3.zero, worldPos, Color.red * 0.5f, 0.5f);

            if (NoteJudgeManager.Instance.TryGetInputHandle(eventData.pointerId, out var handle))
            {
                handle.InValidPosition = canSendEvent;
                handle.EventType = InputEvent.Drag;
                handle.SetDegreeByWorldPosition(worldPos);
                NoteJudgeManager.Instance.InputHandleUpdated(handle);

                if (handle.TryUpdateFlick(worldPos))
                {
                    handle.EventType = InputEvent.Flick;
                    NoteJudgeManager.Instance.InputHandleUpdated(handle);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GetWorldPosition(eventData.position, out var worldPos, out var canSendEvent);
            Debug.DrawLine(Vector3.zero, worldPos, Color.yellow, 0.5f);

            if (NoteJudgeManager.Instance.TryGetInputHandle(eventData.pointerId, out var handle))
            {
                handle.InValidPosition = canSendEvent;
                handle.EventType = InputEvent.EndDrag;
                handle.SetDegreeByWorldPosition(worldPos);
                handle.Dragging = false;
                NoteJudgeManager.Instance.InputHandleUpdated(handle);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GetWorldPosition(eventData.position, out var worldPos, out var canSendEvent);

            if (NoteJudgeManager.Instance.TryGetInputHandle(eventData.pointerId, out var handle))
            {
                handle.InValidPosition = canSendEvent;
                handle.EventType = InputEvent.PointerDown;
                handle.SetDegreeByWorldPosition(worldPos);
                handle.Holding = true;
                handle.PreviousFlickPosition = worldPos;
                NoteJudgeManager.Instance.InputHandleUpdated(handle);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            GetWorldPosition(eventData.position, out var worldPos, out var canSendEvent);
            Debug.DrawLine(Vector3.zero, worldPos, Color.cyan * 0.5f, 0.5f);

            if (NoteJudgeManager.Instance.TryGetInputHandle(eventData.pointerId, out var handle))
            {
                handle.InValidPosition = canSendEvent;
                handle.EventType = InputEvent.PointerUp;
                handle.SetDegreeByWorldPosition(worldPos);
                handle.Holding = false;
                NoteJudgeManager.Instance.InputHandleUpdated(handle);

                handle.Reset(); //Reset the Handle
            }
        }

        public void GetWorldPosition(Vector2 pointerPosition, out Vector3 worldPosition, out bool canSendEvent)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GamePlayScreenRect, pointerPosition, UICamera, out var localPoint);
            var gameplaySize = GamePlayScreenRect.rect.size;
            var screenPoint = localPoint + (gameplaySize * 0.5f);
            var viewport = new Vector3(
                screenPoint.x / gameplaySize.x,
                screenPoint.y / gameplaySize.y,
                Vector3.Distance(GameCamera.transform.position, Vector3.zero));

            worldPosition = GameCamera.ViewportToWorldPoint(viewport);
            worldPosition.z = 0.0f;

            canSendEvent = worldPosition.sqrMagnitude >= 30.25f; //Input that not far about 5.5m from core
        }
    }
}
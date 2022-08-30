using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay.Judge.Inputs
{
    public class GameInputListener : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform GamePlayScreenRect;
        public Camera MainCamera;

        void Awake()
        {
            MainCamera = Camera.main;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!IsReadyForInput())
                return;

            GetWorldPosition(eventData.position, out var worldPos, out var canSendEvent);
            Debug.DrawLine(Vector3.zero, worldPos, Color.white, 0.5f);

            if (NoteJudgeUpdater.Instance.TryGetInputHandle(eventData.pointerId, out var handle))
            {
                handle.InValidPosition = canSendEvent;
                handle.EventType = InputEvent.StartDrag;
                handle.SetDegreeByWorldPosition(worldPos);
                handle.Dragging = true;
                NoteJudgeUpdater.Instance.InputHandleUpdated(handle);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!IsReadyForInput())
                return;

            GetWorldPosition(eventData.position, out var worldPos, out var canSendEvent);
            Debug.DrawLine(Vector3.zero, worldPos, Color.red * 0.5f, 0.5f);

            if (NoteJudgeUpdater.Instance.TryGetInputHandle(eventData.pointerId, out var handle))
            {
                handle.InValidPosition = canSendEvent;
                handle.EventType = InputEvent.Drag;
                handle.SetDegreeByWorldPosition(worldPos);
                NoteJudgeUpdater.Instance.InputHandleUpdated(handle);

                if (handle.TryUpdateFlick(worldPos))
                {
                    handle.EventType = InputEvent.Flick;
                    NoteJudgeUpdater.Instance.InputHandleUpdated(handle);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!IsReadyForInput())
                return;

            GetWorldPosition(eventData.position, out var worldPos, out var canSendEvent);
            Debug.DrawLine(Vector3.zero, worldPos, Color.yellow, 0.5f);

            if (NoteJudgeUpdater.Instance.TryGetInputHandle(eventData.pointerId, out var handle))
            {
                handle.InValidPosition = canSendEvent;
                handle.EventType = InputEvent.EndDrag;
                handle.SetDegreeByWorldPosition(worldPos);
                handle.Dragging = false;
                NoteJudgeUpdater.Instance.InputHandleUpdated(handle);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsReadyForInput())
                return;

            GetWorldPosition(eventData.position, out var worldPos, out var canSendEvent);

            if (NoteJudgeUpdater.Instance.TryGetInputHandle(eventData.pointerId, out var handle))
            {
                handle.InValidPosition = canSendEvent;
                handle.EventType = InputEvent.PointerDown;
                handle.SetDegreeByWorldPosition(worldPos);
                handle.Holding = true;
                handle.PreviousFlickPosition = worldPos;
                NoteJudgeUpdater.Instance.InputHandleUpdated(handle);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!IsReadyForInput())
                return;

            GetWorldPosition(eventData.position, out var worldPos, out var canSendEvent);
            Debug.DrawLine(Vector3.zero, worldPos, Color.cyan * 0.5f, 0.5f);

            if (NoteJudgeUpdater.Instance.TryGetInputHandle(eventData.pointerId, out var handle))
            {
                handle.InValidPosition = canSendEvent;
                handle.EventType = InputEvent.PointerUp;
                handle.SetDegreeByWorldPosition(worldPos);
                handle.Holding = false;
                NoteJudgeUpdater.Instance.InputHandleUpdated(handle);

                handle.Reset(); //Reset the Handle
            }
        }

        private void GetWorldPosition(Vector2 pointerPosition, out Vector3 worldPosition, out bool canSendEvent)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GamePlayScreenRect, pointerPosition, MainCamera, out var localPoint);
            var gameplaySize = GamePlayScreenRect.rect.size;
            var screenPoint = localPoint + (gameplaySize * 0.5f);
            var viewport = new Vector3(
                screenPoint.x / gameplaySize.x,
                screenPoint.y / gameplaySize.y,
                Vector3.Distance(GameCamera.Transform.position, Vector3.zero));

            worldPosition = GameCamera.Cam.ViewportToWorldPoint(viewport);
            worldPosition.z = 0.0f;

            canSendEvent = worldPosition.sqrMagnitude >= 30.25f; //Input that not far about 5.5m from core
        }

        private bool IsReadyForInput()
        {
            if (NoteJudgeUpdater.Instance == null)
                return false;

            if (GameCamera.Cam == null)
                return false;

            return true;
        }
    }
}
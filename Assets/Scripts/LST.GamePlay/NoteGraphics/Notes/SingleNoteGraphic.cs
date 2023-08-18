using Lanostane.Models;
using LST.GamePlay.Judge;
using LST.GamePlay.Scrolls;
using UnityEngine;
using Utils.Maths;

namespace LST.GamePlay.Graphics
{
    internal sealed class SingleNoteGraphic : MonoBehaviour, ISingleNoteGraphic
    {
        public LST_SingleNoteType Type { get; set; }
        public LST_NoteSpecialFlags Flags { get; set; }
        public float Timing { get; set; }
        public ScrollTiming ScrollTiming { get; set; }
        public bool JudgeDone { get; set; }

        public GameObject HighlightObject;
        public GameObject FlickInObject;
        public GameObject FlickOutObject;
        public SpriteRenderer NoteGraphic;

        private Vector3 _StartPosition;
        private Vector3 _EndPosition;
        private bool _IsHidden = false;
        private bool _HasNoGraphicFlag = false;

        public void Setup(LST_SingleNoteInfo info)
        {
            Timing = info.Timing;
            ScrollTiming = GamePlays.ScrollUpdater.GetScrollTimingByTime(info.ScrollGroup, Timing);

            SetNoteType(info.Type);
            HighlightObject.SetActive(info.Highlight);

            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = new(0.0f, 0.0f, info.Degree);

            float x = Mathf.Sin(Mathf.Deg2Rad * info.Degree);
            float y = -Mathf.Cos(Mathf.Deg2Rad * info.Degree);
            Vector3 dir = new(x, y, 0.0f);
            _StartPosition = dir * GameConst.SpaceStart;
            _EndPosition = dir * GameConst.SpaceEnd;

            gameObject.SetActive(false);
            _IsHidden = true;

            Flags = info.Flags;
            _HasNoGraphicFlag = info.Flags.HasFlag(LST_NoteSpecialFlags.NoGraphic);
        }

        public void Show()
        {
            if (_HasNoGraphicFlag)
                return;

            if (!_IsHidden)
                return;

            gameObject.SetActive(true);
            _IsHidden = false;
        }

        public void Hide()
        {
            if (_HasNoGraphicFlag)
                return;

            if (_IsHidden)
                return;

            gameObject.SetActive(false);
            _IsHidden = true;
        }

        public void TriggerJudgeEffect(JudgeType type)
        {
            if (_HasNoGraphicFlag)
                return;

            switch (Type)
            {
                case LST_SingleNoteType.Click:
                    if (type == JudgeType.Perfect || type == JudgeType.PurePerfect)
                    {
                        JudgeSFX.PlayPerfectTap();
                        GameFX.TapParticles.Trigger(transform.parent, transform.position);
                    }
                    else if (type == JudgeType.Good)
                    {
                        JudgeSFX.PlayGoodTap();
                        GameFX.TapParticles.Trigger(transform.parent, transform.position);
                    }
                    break;

                case LST_SingleNoteType.Catch:
                    if (type == JudgeType.Perfect || type == JudgeType.PurePerfect)
                    {
                        JudgeSFX.PlayCatch();
                        GameFX.TapParticles.Trigger(transform.parent, transform.position);
                    }
                    break;

                case LST_SingleNoteType.FlickIn:
                case LST_SingleNoteType.FlickOut:
                    if (type == JudgeType.Perfect || type == JudgeType.PurePerfect)
                    {
                        JudgeSFX.PlayPerfectFlick();
                        GameFX.TapParticles.Trigger(transform.parent, transform.position);
                    }
                    else if (type == JudgeType.Good)
                    {
                        JudgeSFX.PlayGoodFlick();
                        GameFX.TapParticles.Trigger(transform.parent, transform.position);
                    }
                    break;
            }

        }

        public void SetNoteType(LST_SingleNoteType type)
        {
            Type = type;
            NoteGraphic.color = type switch
            {
                LST_SingleNoteType.Click => Color.white, //White
                LST_SingleNoteType.Catch => new Color(0.2704254f, 0.5609212f, 0.9716981f), //Blue
                LST_SingleNoteType.FlickIn => new Color(0.972549f, 0.2705882f, 0.3598078f), //Red
                LST_SingleNoteType.FlickOut => new Color(0.1444053f, 0.6603774f, 0.1027946f), //Green
                _ => throw new System.NotImplementedException(),
            };

            FlickInObject.SetActive(false);
            FlickOutObject.SetActive(false);

            switch (type)
            {
                case LST_SingleNoteType.FlickIn:
                    FlickInObject.SetActive(true);
                    break;

                case LST_SingleNoteType.FlickOut:
                    FlickOutObject.SetActive(true);
                    break;
            }
        }

        public void UpdateProgress(float progress01)
        {
            Show();

            transform.localScale = GameConst.LerpNoteSize(progress01);
            transform.localPosition = Vector3.Lerp(_StartPosition, _EndPosition, progress01);
        }

        public void DestroyInstance()
        {
            Destroy(this);
        }
    }
}

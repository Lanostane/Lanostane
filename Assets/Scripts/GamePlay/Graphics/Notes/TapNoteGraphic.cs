﻿using Lst.Charts;
using GamePlay.Graphics.FX;
using GamePlay.Judge;
using GamePlay.Scrolls;
using UnityEngine;
using Utils.Maths;

namespace GamePlay.Graphics
{
    internal sealed class TapNoteGraphic : MonoBehaviour, ISingleNoteGraphic
    {
        public LST_SingleNoteType Type { get; set; }
        public float Timing { get; set; }
        public Millisecond ScrollTiming { get; set; }
        public bool JudgeDone { get; set; }


        public GameObject HighlightObject;
        public GameObject FlickInObject;
        public GameObject FlickOutObject;
        public SpriteRenderer NoteGraphic;

        private Vector3 _StartPosition;
        private Vector3 _EndPosition;
        private bool _IsHidden = false;

        public void Setup(LST_SingleNoteInfo info)
        {
            Timing = info.Timing;
            ScrollTiming = ScrollManager.GetScrollTiming(Timing);

            SetNoteType(info.Type);
            HighlightObject.SetActive(info.Highlight);

            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = new(0.0f, 0.0f, info.Degree);

            var x = Mathf.Sin(Mathf.Deg2Rad * info.Degree);
            var y = -Mathf.Cos(Mathf.Deg2Rad * info.Degree);
            var dir = new Vector3(x, y, 0.0f);
            _StartPosition = dir * GameConst.SpaceStart;
            _EndPosition = dir * GameConst.SpaceEnd;

            gameObject.SetActive(false);
            _IsHidden = true;
        }

        public void Show()
        {
            if (_IsHidden)
            {
                gameObject.SetActive(true);
                _IsHidden = false;
            }
        }

        public void Hide()
        {
            if (!_IsHidden)
            {
                gameObject.SetActive(false);
                _IsHidden = true;
            }
        }

        public void TriggerJudgeEffect(JudgeType type)
        {
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

            transform.localScale = Vector3.one * GameConst.LerpNoteSize(progress01);
            transform.localPosition = Vector3.Lerp(_StartPosition, _EndPosition, progress01);
        }

        public void DestroyInstance()
        {
            Destroy(this);
        }
    }
}
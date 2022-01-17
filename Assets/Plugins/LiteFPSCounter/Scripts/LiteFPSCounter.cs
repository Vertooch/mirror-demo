//
// Lite FPS Counter
//
// Version    : 1.0.0
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using OmniSARTechnologies.Helper;

namespace OmniSARTechnologies.LiteFPSCounter {
    [SelectionBase]
    [DisallowMultipleComponent]
    public class LiteFPSCounter : MonoBehaviour {
        /// <summary>
        /// Reference to a Text component where the dynamic info will be displayed.
        /// <para></para>
        /// <para></para>
        /// Make sure the referenced UI Text component is not expensive to draw and also not 
        /// expensive to update (keep it as simple and efficient as possible).
        /// </summary>
        [Header("GUI")]
        [Tooltip(
            "Reference to a Text component where the dynamic info will be displayed.\n\r\n\r" +
            "Make sure the referenced UI Text component is not expensive to draw and also not " +
            "expensive to update (keep it as simple and efficient as possible)."
        )]
        public Text dynamicInfoText;

        /// <summary>
        /// Registered frame time within the update interval.
        /// </summary>
        public float frameTime {
            get {
                return m_FrameTime;
            }
        }
        private float m_FrameTime = 0.0f;

        /// <summary>
        /// Minimum registered frame time within the update interval.
        /// </summary>
        public float minFrameTime {
            get {
                return m_MinFrameTime;
            }
        }
        private float m_MinFrameTime = 0.0f;

        /// <summary>
        /// Maximum registered frame time within the update interval.
        /// </summary>
        public float maxFrameTime {
            get {
                return m_MaxFrameTime;
            }
        }
        private float m_MaxFrameTime = 0.0f;

        /// <summary>
        /// Fluctuation of the registered frame time within the update interval.
        /// </summary>
        public float frameTimeFlutuation {
            get {
                return m_FrameTimeFlutuation;
            }
        }
        private float m_FrameTimeFlutuation = 0.0f;

        /// <summary>
        /// Registered framerate within the update interval.
        /// </summary>
        public float frameRate {
            get {
                return m_FrameRate;
            }
        }
        private float m_FrameRate = 0.0f;

        /// <summary>
        /// Minimum registered framerate within the update interval.
        /// </summary>
        public float minFrameRate {
            get {
                return m_MinFrameRate;
            }
        }
        private float m_MinFrameRate = 0.0f;

        /// <summary>
        /// Maximum registered framerate within the update interval.
        /// </summary>
        public float maxFrameRate {
            get {
                return m_MaxFrameRate;
            }
        }
        private float m_MaxFrameRate = 0.0f;

        /// <summary>
        /// Framerate fluctuation within the update interval.
        /// </summary>
        public float frameRateFlutuation {
            get {
                return m_FrameRateFlutuation;
            }
        }
        private float m_FrameRateFlutuation = 0.0f;

        private Color m_FPSFieldsColor = ColorHelper.HexStrToColor("#80FF00FF");

        private float m_AccumulatedTime;
        private int m_AccumulatedFrames;
        private float m_LastUpdateTime;
        private string m_StaticInfoDisplay;
        private string m_DynamicConfigurationFormat;

        public static float UpdateInterval = 0.5f;
        public static float MinTime = 0.000000001f; // equivalent to 1B fps

        /// <summary>
        /// Initializes (and resets) the component.
        /// <para></para>
        /// <para></para>
        /// <remarks>
        /// The initialization only targets the component's internal data.
        /// </remarks>
        /// </summary>
        public void Initialize() {
            Reset();
            UpdateInternals();
        }

        /// <summary>
        /// Resets the framerate probing data.
        /// <para></para>
        /// <para></para>
        /// <remarks>
        /// This does not reset the component's inspector state.
        /// </remarks>
        /// </summary>
        public void Reset() {
            ResetProbingData();

            m_LastUpdateTime = Time.realtimeSinceStartup;
        }

        private void Start() {
            Initialize();
        }

        private void OnEnable() {
            Initialize();
        }

        public void UpdateInternals() {
            UpdateStaticContentAndData();
        }

        private void UpdateStaticContentAndData()
        {
            m_DynamicConfigurationFormat = string.Format(
                "{0} FPS",

                ColorHelper.ColorText("{0}", m_FPSFieldsColor)
            );
        }

        private void UpdateDynamicContent() {
            if (!dynamicInfoText) {
                return;
            }

            dynamicInfoText.text = string.Format(
                m_DynamicConfigurationFormat,
                m_FrameRate.ToString("F1")
            );
        }

        private void ResetProbingData() {
            m_MinFrameTime = float.MaxValue;
            m_MaxFrameTime = float.MinValue;
            m_AccumulatedTime = 0.0f;
            m_AccumulatedFrames = 0;
        }

        private void UpdateFPS() {
            if (!dynamicInfoText) {
                return;
            }

            float deltaTime = Time.unscaledDeltaTime;

            m_AccumulatedTime += deltaTime;
            m_AccumulatedFrames++;

            if (deltaTime < MinTime) {
                deltaTime = MinTime;
            }

            if (deltaTime < m_MinFrameTime) {
                m_MinFrameTime = deltaTime;
            }

            if (deltaTime > m_MaxFrameTime) {
                m_MaxFrameTime = deltaTime;
            }

            float nowTime = Time.realtimeSinceStartup;
            if (nowTime - m_LastUpdateTime < UpdateInterval) {
                return;
            }

            if (m_AccumulatedTime < MinTime) {
                m_AccumulatedTime = MinTime;
            }

            if (m_AccumulatedFrames < 1) {
                m_AccumulatedFrames = 1;
            }

            m_FrameTime = m_AccumulatedTime / m_AccumulatedFrames;
            m_FrameRate = 1.0f / m_FrameTime;

            m_MinFrameRate = 1.0f / m_MaxFrameTime;
            m_MaxFrameRate = 1.0f / m_MinFrameTime;

            m_FrameTimeFlutuation = Mathf.Abs(m_MaxFrameTime - m_MinFrameTime) / 2.0f;
            m_FrameRateFlutuation = Mathf.Abs(m_MaxFrameRate - m_MinFrameRate) / 2.0f;

            UpdateDynamicContent();

            ResetProbingData();
            m_LastUpdateTime = nowTime;
        }

        private void Update() {
            UpdateFPS();
        }
    }
}
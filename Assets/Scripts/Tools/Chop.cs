using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace EPL
{
    public class Chop : MonoBehaviour
    {
        /// <summary>
        /// Instance of this GameObject.
        /// </summary>
        private static Chop instance;

        [Tooltip("Draw Zone A wireframe within the editor.")]
        public bool drawZoneA;

        [Tooltip("Draw Zone B wireframe within the editor.")]
        public bool drawZoneB;

        [Tooltip("Updates the projection matrix within the editor even when it is not running. Warning, the camera cannot be edited after this is set to true.")]
        private bool renderCameraInEditor;

        /// <summary>
        /// The camera view that will be used within the editor.
        /// </summary>
        [Tooltip("The camera view that will be used within the editor. \nThis value will not be used in builds, it must be defined by the -camera command line argument.")]
        public CameraEnum editorCamera;
        private CameraEnum previousEditorCamera;
        [Range(0, 42)]
        [Tooltip("This variable will scroll through each camera view and update the editorCamera. It will only work at runtime.")]
        public int panel = 1;
        private int previousPanel;
        private Vector3 previousScreenPosition;


        /// <summary>
        /// Used to define the bottom center position of the wall screen.
        /// </summary>
        [Tooltip("The position of this transform will be used to define the bottom center position of the wall screen.")]
        public Transform screenPosition;

        /// <summary>
        /// The Projection Matrix of the camera. It is calculated and assigned to the camera at the start.
        /// </summary>
        private Matrix4x4 projectionMatrix;

        void Awake()
        {
            instance = this;
            panel = (int)editorCamera;
            previousEditorCamera = editorCamera;
            previousPanel = panel;
            previousScreenPosition = screenPosition.position;
        }

        void Start()
        {
            ChopCamera();
        }

        /// <summary>
        /// Creates the Projection matrix of the camera.
        /// Camera will be set inactive if no view is chosen.
        /// </summary>
        private void ChopCamera()
        {
            View? view = GetView();

            if (view == null || ((View)view).GamePosition == null)
            {
                Camera.main.gameObject.SetActive(false);
                Debug.LogError("There is no designated Display for your machine ");
                return;
            }

            View _view = (View)view;
            Vector3 screen = screenPosition.position;
            Vector3 upperLeft = new Vector3(((Vector3)_view.GamePosition).x + -_view.Width / 2f + screen.x, ((Vector3)_view.GamePosition).y + _view.Height / 2f + screen.y, screen.z);
            Vector3 lowerRight = new Vector3(((Vector3)_view.GamePosition).x + _view.Width / 2f + screen.x, ((Vector3)_view.GamePosition).y + -_view.Height / 2f + screen.y, screen.z);
            Vector3 lowerLeft = new Vector3(((Vector3)_view.GamePosition).x + -_view.Width / 2f + screen.x, ((Vector3)_view.GamePosition).y + -_view.Height / 2f + screen.y, screen.z);

            Camera.main.projectionMatrix = getAsymProjMatrix(lowerLeft, lowerRight, upperLeft, Camera.main.transform.position, Camera.main.nearClipPlane, Camera.main.farClipPlane);
            Camera.main.transform.rotation = Quaternion.identity;
            screenPosition.position = screen;
            projectionMatrix = Camera.main.projectionMatrix;
        }

        /// <summary>
        /// Get the camera view (position, size) of the scene.
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// Returns null if there is no CameraEnum or custom camera coordinates defined in the Command line arguments or cameraEnum editor argument.
        /// </returns>
        private View? GetView()
        {
            View display = new View();
            View? commandLineDisplay = Displays.CustomCommandLineView();
            CameraEnum cameraEnum = GetCameraEnum();


            if (cameraEnum == CameraEnum.None && commandLineDisplay == null)
            {
                return null;
            }

            display = commandLineDisplay == null ? GetViewFromCameraEnum(cameraEnum) : (View)commandLineDisplay;
            return display;
        }

        /// <summary>
        /// Get's the viewport (position, width, and height) of the camera associated with the camera enum.
        /// </summary>
        /// <param name="cameraEnum"></param>
        /// <returns></returns>
        public static View GetViewFromCameraEnum(CameraEnum cameraEnum)
        {
            View view = new View();
            int index;
            string zone = (System.Enum.GetName(typeof(CameraEnum), cameraEnum)).Split('_').First<string>();
            int.TryParse((System.Enum.GetName(typeof(CameraEnum), cameraEnum)).Split('_').Last<string>(), out index);

            switch (cameraEnum)
            {
                case CameraEnum.ZoneA_Full:
                    view.Width = Constants.PanelWidth * Constants.ZoneANumPanels;
                    view.Height = Constants.PanelHeight + Constants.ZoneAPlanarHeight;
                    view.GamePosition = new Vector3(0f, (Constants.ZoneAPlanarHeight + Constants.PanelHeight) / 2f);
                    break;
                case CameraEnum.ZoneA_Projector_0:
                    view.Width = Constants.ZoneAPlanarWidth;
                    view.Height = Constants.ZoneAPlanarHeight;
                    view.GamePosition = new Vector3(-Constants.ZoneAPlanarWidth / 2f, Constants.PanelHeight + Constants.ZoneAPlanarHeight / 2f);
                    break;
                case CameraEnum.ZoneA_Projector_1:
                    view.Width = Constants.ZoneAPlanarWidth;
                    view.Height = Constants.ZoneAPlanarHeight;
                    view.GamePosition = new Vector3(Constants.ZoneAPlanarWidth / 2f, Constants.PanelHeight + Constants.ZoneAPlanarHeight / 2f);
                    break;
                case CameraEnum.ZoneA_0_2:
                    view.Width = Constants.PanelWidth * 3f;
                    view.Height = Constants.PanelHeight;
                    view.GamePosition = PanelDisplayPosition(0f, Constants.ZoneANumPanels);
                    break;
                case CameraEnum.ZoneA_3_5:
                    view.Width = Constants.PanelWidth * 3f;
                    view.Height = Constants.PanelHeight;
                    view.GamePosition = PanelDisplayPosition(1f, Constants.ZoneANumPanels);
                    break;
                case CameraEnum.ZoneA_6_8:
                    view.Width = Constants.PanelWidth * 3f;
                    view.Height = Constants.PanelHeight;
                    view.GamePosition = PanelDisplayPosition(2f, Constants.ZoneANumPanels);
                    break;
                case CameraEnum.ZoneA_9_11:
                    view.Width = Constants.PanelWidth * 3f;
                    view.Height = Constants.PanelHeight;
                    view.GamePosition = PanelDisplayPosition(3f, Constants.ZoneANumPanels);
                    break;
                case CameraEnum.ZoneA_12_14:
                    view.Width = Constants.PanelWidth * 3f;
                    view.Height = Constants.PanelHeight;
                    view.GamePosition = PanelDisplayPosition(4f, Constants.ZoneANumPanels);
                    break;
                case CameraEnum.ZoneA_15_17:
                    view.Width = Constants.PanelWidth * 3f;
                    view.Height = Constants.PanelHeight;
                    view.GamePosition = PanelDisplayPosition(5f, Constants.ZoneANumPanels);
                    break;
                case CameraEnum.ZoneB_Full:
                    view.Width = Constants.PanelWidth * Constants.ZoneBNumPanels;
                    view.Height = Constants.PanelHeight + Constants.ZoneBPlanarHeight;
                    view.GamePosition = new Vector3(0f, (Constants.ZoneBPlanarHeight + Constants.PanelHeight) / 2f);
                    break;
                case CameraEnum.ZoneB_Projector:
                    view.Width = Constants.ZoneBPlanarWidth;
                    view.Height = Constants.ZoneBPlanarHeight;
                    view.GamePosition = new Vector3(Constants.ZoneBLedOffset, Constants.PanelHeight + Constants.ZoneBPlanarHeight / 2f);
                    break;
                case CameraEnum.ZoneB_0_2:
                    view.Width = Constants.PanelWidth * 3f;
                    view.Height = Constants.PanelHeight;
                    view.GamePosition = PanelDisplayPosition(0f, Constants.ZoneBNumPanels);
                    break;
                case CameraEnum.ZoneB_3_5:
                    view.Width = Constants.PanelWidth * 3f;
                    view.Height = Constants.PanelHeight;
                    view.GamePosition = PanelDisplayPosition(1f, Constants.ZoneBNumPanels);
                    break;
                case CameraEnum.ZoneB_4_6:
                    view.Width = Constants.PanelWidth * 3f;
                    view.Height = Constants.PanelHeight;
                    view.GamePosition = PanelDisplayPosition(1.33f, Constants.ZoneBNumPanels);
                    break;
                case CameraEnum.Development_Full:
                    view.Width = Constants.PanelWidth * Constants.DevelopmentNumPanels;
                    view.Height = Constants.PanelHeight + Constants.DevMonitorHeight;
                    view.GamePosition = new Vector3(0f, Constants.DevMonitorHeight / 2f);
                    break;
                case CameraEnum.Development_Monitors:
                    view.Width = Constants.DevMonitorWidth;
                    view.Height = Constants.DevMonitorHeight;
                    view.GamePosition = new Vector3(0f, Constants.PanelHeight + Constants.DevMonitorHeight / 2f);
                    break;
                case CameraEnum.Development_LCD:
                    view.Width = Constants.LEDWidth * Constants.DevNumLCDColumns;
                    view.Height = Constants.LEDHeight;
                    view.GamePosition = new Vector3(-2f * Constants.PanelWidth, -0.035f + Constants.PanelHeight + Constants.LEDHeight / 2f);
                    break;
                case CameraEnum.QUT_Development_Projector:
                    view.Width = Constants.QUTDevProjectorWidth;
                    view.Height = Constants.QUTDevProjectorHeight;
                    view.GamePosition = new Vector3(0f, Constants.PanelHeight + Constants.QUTDevProjectorHeight / 2f);
                    break;
                case CameraEnum.None:
                    break;
                default:
                    view.Width = Constants.PanelWidth;
                    view.Height = Constants.PanelHeight;
                    if (zone == "ZoneA")
                        view.GamePosition = PanelDisplayPosition(index, Constants.ZoneANumPanels, 1);
                    else if (zone == "ZoneB")
                        view.GamePosition = PanelDisplayPosition(index, Constants.ZoneBNumPanels, 1);
                    break;
            }
            return view;
        }

        private CameraEnum GetCameraEnum()
        {
            if (Displays.Camera() != null)
                return (CameraEnum)Displays.Camera();

#if UNITY_EDITOR
            return editorCamera;
#endif

            return CameraEnum.None;
        }

        /// <summary>
        /// Calculates the position of the touch panel groups.
        /// </summary>
        /// <param name="index">Index of executable.</param>
        /// <param name="numberOfPanels">Total number of panels in the entire zone.</param>
        /// <param name="panelsPerDisplay">Number of panels per exectuable. Will default to Constants.PanelsPerDisplay if unset. </param>
        /// <returns></returns>
        private static Vector3 PanelDisplayPosition(float index, int numberOfPanels, int panelsPerDisplay = 0)
        {
            panelsPerDisplay = panelsPerDisplay == 0 ? Constants.PanelsPerDisplay : panelsPerDisplay;
            return new Vector3(-1f * Constants.PanelWidth * ((float)(numberOfPanels - panelsPerDisplay) / 2f - (float)index * panelsPerDisplay), Constants.PanelHeight / 2f);
        }

        private void Update()
        {
            PostProcessingLayer_MatrixCheck();
#if UNITY_EDITOR
            UpdateEditorCamera();
#endif
        }

        private void UpdateEditorCamera()
        {
            if (previousEditorCamera == editorCamera && previousPanel == panel && previousScreenPosition == screenPosition.position)
                return;

            if (previousEditorCamera != editorCamera)
            {
                panel = (int)editorCamera;
            }
            if (previousPanel != panel)
            {
                editorCamera = (CameraEnum)panel;
            }

            ChopCamera();
            previousEditorCamera = editorCamera;
            previousPanel = panel;
        }

        void PostProcessingLayer_MatrixCheck()
        {
            if (projectionMatrix != Camera.main.projectionMatrix)
                Debug.LogError("The camera matrix projection has been reset! Chop.cs modifies the camera's projection matrix and requires it to stay the same through the duration of the game. " +
                    "This reset may have been called by the Post Processing package. Maybe it has been recently updated? " +
                    "Find the script called PostProcessLayer.cs and comment out any line containing ResetProjectionMatrix().");
        }

        public static Matrix4x4 getAsymProjMatrix(Vector3 pa, Vector3 pb, Vector3 pc, Vector3 pe, float ncp, float fcp)
        {
            //compute orthonormal basis for the screen - could pre-compute this...
            Vector3 vr = (pb - pa).normalized;
            Vector3 vu = (pc - pa).normalized;
            Vector3 vn = Vector3.Cross(vr, vu).normalized;

            //compute screen corner vectors
            Vector3 va = pa - pe;
            Vector3 vb = pb - pe;
            Vector3 vc = pc - pe;

            //find the distance from the eye to screen plane
            float n = ncp;
            float f = fcp;
            float d = Vector3.Dot(va, vn); // distance from eye to screen
            float nod = n / d;
            float l = Vector3.Dot(vr, va) * nod;
            float r = Vector3.Dot(vr, vb) * nod;
            float b = Vector3.Dot(vu, va) * nod;
            float t = Vector3.Dot(vu, vc) * nod;

            //put together the matrix - bout time amirite?
            Matrix4x4 m = Matrix4x4.zero;

            //from http://forum.unity3d.com/threads/using-projection-matrix-to-create-holographic-effect.291123/
            m[0, 0] = 2.0f * n / (r - l);
            m[0, 2] = (r + l) / (r - l);
            m[1, 1] = 2.0f * n / (t - b);
            m[1, 2] = (t + b) / (t - b);
            m[2, 2] = -(f + n) / (f - n);
            m[2, 3] = (-2.0f * f * n) / (f - n);
            m[3, 2] = -1.0f;

            return m;
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (renderCameraInEditor)
                UpdateEditorCamera();
#endif
            if (drawZoneA)
            {
                DrawView(CameraEnum.ZoneA_0_2, Color.white);
                DrawView(CameraEnum.ZoneA_3_5, Color.white);
                DrawView(CameraEnum.ZoneA_6_8, Color.white);
                DrawView(CameraEnum.ZoneA_9_11, Color.white);
                DrawView(CameraEnum.ZoneA_12_14, Color.white);
                DrawView(CameraEnum.ZoneA_15_17, Color.white);
                DrawView(CameraEnum.ZoneA_Projector_0, Color.white);
                DrawView(CameraEnum.ZoneA_Projector_1, Color.white);
            }
            if (drawZoneB)
            {
                DrawView(CameraEnum.ZoneB_0_2, Color.white);
                DrawView(CameraEnum.ZoneB_3_5, Color.white);
                DrawView(CameraEnum.ZoneB_6, Color.white);
                DrawView(CameraEnum.ZoneB_Projector, Color.white);
            }
            DrawView(editorCamera, Color.blue);
        }

        private void DrawView(CameraEnum camera, Color color)
        {
            Vector3 screen = screenPosition.position;

            View temp = GetViewFromCameraEnum(camera);

            Vector3 upperRight =
                new Vector3(((Vector3)temp.GamePosition).x + temp.Width / 2f + screen.x, ((Vector3)temp.GamePosition).y + temp.Height / 2f + screen.y, screen.z);
            Vector3 upperLeft =
                    new Vector3(((Vector3)temp.GamePosition).x + -temp.Width / 2f + screen.x, ((Vector3)temp.GamePosition).y + temp.Height / 2f + screen.y, screen.z); ;
            Vector3 lowerRight =
                new Vector3(((Vector3)temp.GamePosition).x + temp.Width / 2f + screen.x, ((Vector3)temp.GamePosition).y + -temp.Height / 2f + screen.y, screen.z);
            Vector3 lowerLeft =
                    new Vector3(((Vector3)temp.GamePosition).x + -temp.Width / 2f + screen.x, ((Vector3)temp.GamePosition).y + -temp.Height / 2f + screen.y, screen.z);

            Gizmos.color = color;
            Gizmos.DrawLine(upperRight, upperLeft);
            Gizmos.DrawLine(upperRight, lowerRight);
            Gizmos.DrawLine(lowerLeft, lowerRight);
            Gizmos.DrawLine(lowerLeft, upperLeft);
        }
    }
}
// Set an off-center projection, where perspective's vanishing
// point is not necessarily in the center of the screen.
//
// left/right/top/bottom define near plane size, i.e.
// how offset are corners of camera's near plane.
// Tweak the values and you can see camera's frustum change.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TODO MONDAY!!!!
 * Add in logic for Zone a Planar width offset
 * Add in comment/parameter for magic 10000 quad viewport offset in front of camera (does this work for close up objects too?)
 * Make this modular for Zone B
 * */

namespace EPL
{
    public enum Zone
    {
        A, B
    }

    [ExecuteInEditMode]
    public class CameraMatrix : MonoBehaviour
    {
        public static CameraMatrix instance;
        public Zone zone;
        public int editorDisplay;
        public bool drawZone;
        public Material drawMaterial;

        private Camera camera;
        private bool fullScreen;
        private int display;
        private int totalPanels;
        private int totalPlanars;
        private int panelsPerDisplay;
        private bool isMultitaction;

        private void Start()
        {
            if (!Application.isPlaying) return;

            instance = this;
            camera = GetComponent<Camera>();
            SetConfigurationValues();
            SplitProjectorMatrix();
#if !UNITY_EDITOR
            drawZone = false;
#endif
        }

        void LateUpdate()
        {
            if (!Application.isPlaying) return;
#if UNITY_EDITOR
            SetConfigurationValues();
            SplitProjectorMatrix();
#endif
        }

        /// <summary>
        ///  Assigns the target display, total screens based on either editor value or command line arguments.
        /// </summary>
        private void SetConfigurationValues()
        {

#if UNITY_EDITOR

            int numPlanars = (zone == Zone.A) ? Constants.ZoneANumPlanars : Constants.ZoneBNumPlanars;
            
            display = editorDisplay - 1;
            fullScreen = (display < 0);
            isMultitaction = (display >= numPlanars);
            display = (isMultitaction) ? display - numPlanars : display;
            totalPanels = (zone == Zone.A) ? Constants.ZoneANumPanels : Constants.ZoneBNumPanels;
            totalPlanars = (zone == Zone.A) ? Constants.ZoneANumPlanars : Constants.ZoneBNumPlanars;

            panelsPerDisplay = Constants.PanelsPerDisplay;
#else
            isMultitaction = !QUT.CommandLineArguments.HasArgument("isPlanar");

            if (isMultitaction) {
                display = QUT.CommandLineArguments.CommandLineArgument<int>("displayNum");
                totalPanels = QUT.CommandLineArguments.CommandLineArgument<int>("totalPanels");
                totalPlanars = QUT.CommandLineArguments.CommandLineArgument<int>("totalPlanars");
                panelsPerDisplay = QUT.CommandLineArguments.CommandLineArgument<int>("panelsPerDisplay");
            } else
            {
                display = QUT.CommandLineArguments.CommandLineArgument<int>("planar");
                totalPanels = QUT.CommandLineArguments.CommandLineArgument<int>("totalPanels");
                totalPlanars = QUT.CommandLineArguments.CommandLineArgument<int>("totalPlanars");
                panelsPerDisplay = 1;
            }
#endif
        }
        
        private void SplitProjectorMatrix()
        {
            if(fullScreen)
            {
                camera.ResetProjectionMatrix();
                return;
            }
            Matrix4x4 matrix;
            Vector3[] Corners = new Vector3[4];
            Vector2 scaledViewPortSize = ScaledViewPortSize();

            Corners = GetQuadCorners(
                TranslateViewport(FullZoneViewPortPosition(), ScaledViewPortSize(), FullZoneViewPortSize()),
                scaledViewPortSize.x,
                scaledViewPortSize.y,
                camera.transform
                );

            camera.ResetProjectionMatrix();
            matrix = GetMatrix(Corners, camera);
            camera.projectionMatrix = matrix;
        }

        /// <summary>
        /// Translates a the viewport to the specified display position.
        /// </summary>
        /// <param name="initialPosition"></param>
        /// <param name="viewportSize"></param>
        /// <param name="zoneSize"></param>
        /// <returns></returns>
        private Vector3 TranslateViewport(Vector3 initialPosition, Vector2 viewportSize, Vector2 zoneSize)
        {
            Vector3 position = initialPosition;

            position += camera.transform.right * (-zoneSize.x * 0.5f + viewportSize.x * 0.5f + viewportSize.x * display);
            position += isMultitaction ?
                        camera.transform.up * 0.5f * (-zoneSize.y + viewportSize.y):
                        camera.transform.up * 0.5f * (+zoneSize.y - viewportSize.y);

            return position;
        }   

        /// <summary>
        /// Returns the size of a Viewport, scaled down to size of one machines view port (ei, 1 planar, or 3 multitactions)
        /// </summary>
        private Vector2 ScaledViewPortSize()
        {
            Vector2 fullQuad = FullZoneViewPortSize();
            Vector2 scaledQuad; // Aspect ratio of the cross sections.
            float planarHeight = zone == Zone.A ? Constants.ZoneAPlanarHeight : Constants.ZoneBPlanarHeight;

            if (isMultitaction)
            {
                scaledQuad = new Vector2(
                    (float)panelsPerDisplay / (float)totalPanels,
                    Constants.PanelHeight / (Constants.PanelHeight + planarHeight)
                    );
            }
            else
            { // TODO add in offset amount to width here.
                scaledQuad = new Vector2(
                    1.0f / (float)totalPlanars,
                    planarHeight / (Constants.PanelHeight + planarHeight)
                    );
            }

            return scaledQuad * fullQuad;
        }

        /// <summary>
        /// Returns the four corner positions of the view port quad immediately in front of the camera.
        /// </summary>
        /// <returns></returns>
        private Vector3[] FullZoneViewPortCorners()
        {
            Vector3 quadPosition = camera.transform.position + camera.transform.forward;
            float quadHeight = 2.0f * Vector3.Distance(quadPosition, camera.transform.position) * Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2f);
            float quadWidth = FullZoneSize().x * quadHeight / FullZoneSize().y;

            return GetQuadCorners(quadPosition, quadWidth, quadHeight, camera.transform);

        }

        /// <summary>
        /// Return the size of the viewport immediately in front of the camera, scaled to the aspect ratio of the full wall. 
        /// </summary>
        /// <returns></returns>
        private Vector2 FullZoneViewPortSize()
        {
            float quadHeight = 2.0f * Vector3.Distance(FullZoneViewPortPosition(), camera.transform.position) * Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2f);
            float quadWidth = FullZoneSize().x * quadHeight / FullZoneSize().y;

            return new Vector2(quadWidth, quadHeight);
        }

        /// <summary>
        /// Returns the position immediately in front of the camera.
        /// </summary>
        /// <returns></returns>
        private Vector3 FullZoneViewPortPosition()
        {
            return camera.transform.position + camera.transform.forward *1000f;
        }

        /// <summary>
        /// Calculates the corner positions of a quad.
        /// </summary>
        /// <param name="quadPosition"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private Vector3[] GetQuadCorners(Vector3 quadPosition, float width, float height, Transform direction)
        {
            Vector3 bottomLeft = quadPosition - direction.right * width / 2f - direction.up * height / 2f;
            Vector3 bottomRight = quadPosition + direction.right * width / 2f - direction.up * height / 2f;
            Vector3 topLeft = quadPosition - direction.right * width / 2f + direction.up * height / 2f;
            Vector3 topRight = quadPosition + direction.right * width / 2f + direction.up * height / 2f;

            return new Vector3[] { bottomLeft, bottomRight, topLeft, topRight };
        }

        /// <summary>
        /// Return Width and Height of the full zone.
        /// </summary>
        /// <returns></returns>
        private Vector2 FullZoneSize()
        { 
            int numPanels = zone == Zone.A ? Constants.ZoneANumPanels : Constants.ZoneBNumPanels;
            float planarHeight = zone == Zone.A ? Constants.ZoneAPlanarHeight : Constants.ZoneBPlanarHeight;

            return new Vector2(
                Constants.PanelWidth * numPanels,
                Constants.PanelHeight + planarHeight);
        }
        
        /// <summary>
        /// Returns a cross section of the camera matrix that goes through the viewport defined by the "corners" param.
        /// </summary>
        /// <param name="corners">An Array of 4 vectors representing the corners of a square, ordered as Bottom-Left, Bottom-Right, Top-Left, Top-Right</param>
        /// <param name="theCam"></param>
        /// <returns></returns>
        private Matrix4x4 GetMatrix(Vector3[] corners, Camera theCam)
        {
            Vector3 pa, pb, pc, pd;
            pa = corners[0]; //Bottom-Left
            pb = corners[1]; //Bottom-Right
            pc = corners[2]; //Top-Left
            pd = corners[3]; //Top-Right

            Vector3 pe = theCam.transform.position;// eye position

            Vector3 vr = (pb - pa).normalized; // right axis of screen
            Vector3 vu = (pc - pa).normalized; // up axis of screen
            Vector3 vn = Vector3.Cross(vr, vu).normalized; // normal vector of screen

            Vector3 va = pa - pe; // from pe to pa
            Vector3 vb = pb - pe; // from pe to pb
            Vector3 vc = pc - pe; // from pe to pc
            Vector3 vd = pd - pe; // from pe to pd

            float n = theCam.nearClipPlane;// - Vector3.Distance(lookTarget, theCam.transform.position); // distance to the near clip plane (screen)
            float f = theCam.farClipPlane; // distance of far clipping plane
            float d = Vector3.Dot(va, vn); // distance from eye to screen
            float l = Vector3.Dot(vr, va) * n / d; // distance to left screen edge from the 'center'
            float r = Vector3.Dot(vr, vb) * n / d; // distance to right screen edge from 'center'
            float b = Vector3.Dot(vu, va) * n / d; // distance to bottom screen edge from 'center'
            float t = Vector3.Dot(vu, vc) * n / d; // distance to top screen edge from 'center'

            Matrix4x4 p = new Matrix4x4(); // Projection matrix
            p[0, 0] = 2.0f * n / (r - l);
            p[0, 2] = (r + l) / (r - l);
            p[1, 1] = 2.0f * n / (t - b);
            p[1, 2] = (t + b) / (t - b);
            p[2, 2] = (f + n) / (n - f);
            p[2, 3] = 2.0f * f * n / (n - f);
            p[3, 2] = -1.0f;

            return p;
        }

        private void OnPostRender()
        {
#if UNITY_EDITOR
            if (!drawZone) return;

            camera = GetComponent<Camera>();

            DrawZone(Color.black);
            DrawSelectedView();
#endif
        }

        private void DrawZone(Color color, int format = GL.LINES, bool discludeSelected = false)
        {
            SetConfigurationValues();
            int discludeDisplay = (isMultitaction ? display + totalPlanars : display);
            discludeDisplay = discludeSelected ? discludeDisplay : -1;

            int size = totalPanels / panelsPerDisplay + totalPlanars;
            for (int i = 0; i < size; i++)
            {
                if (i == discludeDisplay) continue;

                isMultitaction = (i >= totalPlanars);
                display = isMultitaction ? i - totalPlanars : i;

                Vector3[] Corners = new Vector3[4];
                Vector2 scaledViewPortSize = ScaledViewPortSize();

                Corners = GetQuadCorners(
                    TranslateViewport(FullZoneViewPortPosition(), ScaledViewPortSize(), FullZoneViewPortSize()),
                    scaledViewPortSize.x,
                    scaledViewPortSize.y,
                    camera.transform
                    );

                DrawViewPort(Corners, color, format);
            }
            SetConfigurationValues();
        }

        public void DrawViewPort(Vector3[] Corners, Color color, int format = GL.LINES)
        {
            if (!drawMaterial) return;

            GL.PushMatrix();
            drawMaterial.SetPass(0);
            
            GL.Begin(format);
            GL.Color(color);
            GL.Vertex(Corners[0]);
            GL.Vertex(Corners[1]);

            GL.Vertex(Corners[1]);
            GL.Vertex(Corners[3]);

            GL.Vertex(Corners[3]);
            GL.Vertex(Corners[2]);
            
            GL.Vertex(Corners[2]);
            GL.Vertex(Corners[0]);
            GL.End();

            GL.PopMatrix();
        }

        private void DrawSelectedView()
        {
            Color color = Color.black;
            color.a = 0.5f;
            if(!fullScreen)
                DrawZone(color, GL.QUADS, true);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPL
{
    public struct Display
    {
        public readonly string IPAddress;
        public readonly int Port;
        public readonly int Screen;
        public readonly string Name;

        public Display(
            string ipAddress,
            int port,
            string name,
            int screen = 0
        )
        {
            IPAddress = ipAddress;
            Port = port;
            Name = name;
            Screen = screen;
        }
    }

    public struct View
    {
        public float Width;
        public float Height;
        public Vector3? GamePosition;

        public View(
            float width = 0,
            float height = 0,
            Vector3? gamePosition = null
        )
        {
            Width = width;
            Height = height;
            GamePosition = gamePosition;
        }
    }

    public enum CameraEnum
    {
        None = -1,

        ZoneA_Full = 0,
        ZoneA_Projector_0 = 1,
        ZoneA_Projector_1 = 2,
        ZoneA_0_2 = 3,
        ZoneA_3_5 = 4,
        ZoneA_6_8 = 5,
        ZoneA_9_11 = 6,
        ZoneA_12_14 = 7,
        ZoneA_15_17 = 8,
        ZoneA_0 = 9,
        ZoneA_1 = 10,
        ZoneA_2 = 11,
        ZoneA_3 = 12,
        ZoneA_4 = 13,
        ZoneA_5 = 14,
        ZoneA_6 = 15,
        ZoneA_7 = 16,
        ZoneA_8 = 17,
        ZoneA_9 = 18,
        ZoneA_10 = 19,
        ZoneA_11 = 20,
        ZoneA_12 = 21,
        ZoneA_13 = 22,
        ZoneA_14 = 23,
        ZoneA_15 = 24,
        ZoneA_16 = 25,
        ZoneA_17 = 26,

        ZoneB_Full = 27,
        ZoneB_Projector = 28,
        ZoneB_0_2 = 29,
        ZoneB_3_5 = 30,
        ZoneB_4_6 = 31,
        ZoneB_0 = 32,
        ZoneB_1 = 33,
        ZoneB_2 = 34,
        ZoneB_3 = 35,
        ZoneB_4 = 36,
        ZoneB_5 = 37,
        ZoneB_6 = 38,

        Development_Full = 39,
        Development_LCD = 40,
        Development_Monitors = 41,

        QUT_Development_Projector = 42
    }

    public enum SpeakerEnum
    {
        None,
        ZoneA_L,
        ZoneA_R,
        ZoneA_0,
        ZoneA_1,
        ZoneA_2,
        ZoneA_3,
        ZoneA_4,
        ZoneA_5,
        ZoneB_0,
        ZoneB_1,
        ZoneB_C
    }

    public class Displays
    {
        /** TODO: When the new enivonments are added, 
         * each method must be modifed to add them.
        */

        private static int? screen;
        private static CameraEnum? camera;
        private static string ip;

        public static int Screen()
        {
            if (screen == null)
                screen = QUT.CommandLineArguments.CommandLineArgument<int>("screen");

            return (int)screen;
        }

        public static CameraEnum? Camera()
        {
            if (camera == null)
            {
                string cam = QUT.CommandLineArguments.CommandLineArgument<string>("camera");

                foreach (CameraEnum cameraEnum in Enum.GetValues(typeof(CameraEnum)))
                {
                    if (Enum.GetName(typeof(CameraEnum), cameraEnum) == cam)
                        camera = cameraEnum;
                }
            }
            return camera;
        }

        public static View? CustomCommandLineView()
        {
            float width = QUT.CommandLineArguments.CommandLineArgument<float>("camera-width");
            float height = QUT.CommandLineArguments.CommandLineArgument<float>("camera-height");
            float x = QUT.CommandLineArguments.CommandLineArgument<float>("camera-x");
            float neg_x = QUT.CommandLineArguments.CommandLineArgument<float>("camera-x-");
            float y = QUT.CommandLineArguments.CommandLineArgument<float>("camera-y");

            if (width == 0f &&
                height == 0f &&
                x == 0f &&
                neg_x == 0f &&
                y == 0f) return null;

            return new View(width, height, new Vector3(x - neg_x + width / 2f, y + height / 2f));
        }

        public static string GetIP()
        {
            if (string.IsNullOrEmpty(ip))
            {
                String strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                System.Net.IPAddress[] addr = ipEntry.AddressList;

                foreach (System.Net.IPAddress add in addr)
                {
                    if (add.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ip = add.ToString();
                        return ip;
                    }
                }

                ip = addr[addr.Length - 1].ToString();
            }
            return ip;
        }
    }
}

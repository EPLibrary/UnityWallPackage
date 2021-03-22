[User Guide](https://github.com/haleyrequa/UnityWallPackage/blob/main/Documentation/UserGuide.MD) >  Camera

# Camera Viewports

Each wall is run by several computers, so each instance of the game will only show a cropped viewport of the whole scene.



<img src="https://github.com/haleyrequa/UnityWallPackage/blob/main/Documentation/Images/ZoneA_DrawFullScreen.PNG?raw=true" alt="alt text" width=400>

The CameraMatrix.cs script will chop the camera into the selected viewport at runtime.
The user can select which display they would like to view. When the application is run, it will crop the camera matrix to only show the selected view port. 

<img src="https://github.com/haleyrequa/UnityWallPackage/blob/main/Documentation/Images/ZoneA_DrawMT2.PNG?raw=true" alt="alt text" width=400> <img src="https://github.com/haleyrequa/UnityWallPackage/blob/main/Documentation/Images/ZoneA_RunMT2.PNG?raw=true" alt="alt text" width=400>

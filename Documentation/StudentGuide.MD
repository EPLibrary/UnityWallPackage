# Student Project Guide

This is a guide for post secondary students working on the Wall. Your application will run on a single virtual machine, which spans 3 Multitaction touch screens. It will be part of a larger student exhibit, where multiple student games will run along side each other. You will not be required to do any networking or camera cropping. The size of the physical screen that your game will be displayed on is 195.9cm wide by 121.5cm.

<p align="center"><img src="https://github.com/haleyrequa/UnityWallPackage/blob/main/Documentation/Images/StudentProjects.PNG?raw=true" alt="alt text"></p>

## Installation Instructions:

1. Open a new or existing Unity project.

2. Import TUIO touchscript:
> - Download the **TouchScript.unitypackage** from https://github.com/TouchScript/TouchScript/releases/tag/9.0
> - Within unity, navigate to **Assets > Import Package > Custom Package**
> - Select the **TouchScript.unitypackage** from your downloads folder, then import all.
> - Allow Unity to update the package.
> - The **TouchScripts Settings** window should appear. Check the **Enable TUIO** box. Then you can close the settings window.

3. Import the student wall package into your project:
> - Important: This package must be installed last, other packages will be partially overwritten.
> - Download the latest **StudentWall.unitypackage** from https://github.com/EPLibrary/UnityWallPackage/releases/tag/Student_1.0
> - Within unity, navigate to **Assets > Import Package > Custom Package**
> - Select the **EPLWall_Student.unitypackage** from your downloads folder, then import all.


## Unity Game View
Within the Unity Editor, setup a custom aspect ratio to view your game. Then your workspace will match the wall. The viewport will span 3 Multitaction touch screens, each of these screens has a resolution of 1080x1920. **The total game viewport should be set to 3240x1920.**

1. Click the aspect ratio dropdown in the game view (by default it might say "Free Aspect"), then click to plus sign to add a new viewport. <br /><img src="https://github.com/haleyrequa/UnityWallPackage/blob/main/Documentation/Images/AspectRatioDropDown.png?raw=true" alt="alt text" height=150>
2. Label the new viewport **"3 Panels"**, set the type to **Fixed Resolution**, set the width to **3240** and height to **1920**. <br /><img src="https://github.com/haleyrequa/UnityWallPackage/blob/main/Documentation/Images/3PanelsSettings.PNG?raw=true" alt="alt text" height=150>


## Tips
Be flexible with your scene setup. Running the app on your computer is way different running it on the wall, so you may need to adjust things after you've tried it out in person. For instance, since it is such a large space, the player may have to take a few steps to get from one side of the view to the other. So if the UI has buttons on opposite ends, the player probably cannot interact with them both at the same time. Also children cannot reach as far up, so if the game mechanics rely on touching the top of the screen, it will not be accessable for all user.

# User Guide

## The Zones
There are actually two digitial walls that can run exhibits at the Library. Zone A is the 
Zone B is 

## The Screens
The wall is made up of large Planar Screens

## Multiple Virtual Machines
The wall consists of several seperate computers that make up the full scene, so there is actually a different instances of the game running on each machine. Each of these instances will display a viewport that is a cross-section of the full scene. Any physics objects or animations that pass between the different viewports must be networked.
Each VM spans 3 MultiTactions, or 1 Planar screen. 
Zone B is made up of 4 Virtual Machines:

![alt text](https://github.com/haleyrequa/UnityWallPackage/blob/main/Documentation/Images/VirtualMachines.PNG?raw=true)


## Build your project: 

- In the player settings, esure that "Resizeable Window" is checked.

## Testing at Home

To run your app at home, you'll want to use a custom bat file to start your game. Then you can see the perspective through a single viewport

- In windows explorer, navigate to where your project was built, and create a new **start.bat** file in the same directory as your game's exe file.
- Use Notepad++ to edit your bat file.

```
start "YourApplicationName.exe" "YourApplicationName" -popupwindow -screen-fullscreen 0 -x 0 -y 0 -screen-width 971 -screen-height 710 -server -planar 0 -isPlanar -totalPanels 18 -totalPlanars 2
```

Bat Example: Center 3 MultiTaction Panels

Bat Example: Network Testing 

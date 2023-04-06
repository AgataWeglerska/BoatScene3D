# BoatScene3D
A 3D graphic project shows a scene with a boat and three buoys around it.
The application is written in the C# language and uses the Windows Forms graphical class library.
Triangular objects generated in the Blender program are used in the project - it is boat and three different buoys.

# How does it work?
The boat moves around the green and yellow buoys.
The reflector installed on the right side of the boat is directed towards the yellow buoy or at an angle of 90 degrees to the boat.
The objects are colored using the Z-Buffer method to give depth information 
and a scan-line algorithm is used to color every triangle of the triangular surfaces of the objects.
Additionally, the color in a single picture is calculated with the Gouraud shading method. 
When an object goes out of the scene (which is limited), it is clipped. 
There are three different views:
In the first view, only the boat is moving.
In the second view, the camera follows the boat.
In the third view, the camera is situated on the right side of the boat.

# Example views
1. From the second view:
![rys2](https://user-images.githubusercontent.com/128033227/230322146-07b2c0a7-8618-43b1-bd5f-bd5c39d44047.png)
2. From the third view:
![rys1](https://user-images.githubusercontent.com/128033227/230322185-ba9ff418-dfe8-4a7a-871b-456149ac8b38.png)

# Early Physics Platformer (Sonic)

Starting in 2016 and continuing into 2017, I started work on a physics based platformer in Unity based on the physics driven gameplay found in the Sonic the Hedgehog series of games, which is something I've always wanted to make, and was a driving factor in learning Unity.

Gaining the skills in vector math, trigonometry, and quaternions was the largest hurdle in this project, as these were things I had very limited knowledge of going in, and required a lot of learning.

Contains GameController, PlayerController, and CameraController scripts to handle main behaviour, with auxiliary scripts like MeshController and InputManager to handle additional behaviour (player mesh position/rotation and input, respectively). The size that these main scripts eventually reached was the start of understanding the downsides of large scripts with self-contained systems, and the benefit of dedicated individual scripts with specific purposes that work together to form the larger system.

As the behaviour in these absolutely monstrous controller scripts is so all-encompassing, I will list the main functions that are being performed each frame, specifying which controller handles the function:

Game Controller
- Sets player's intital position to a set spawnpoint position.
- Stores the current collectable amounts, and current global gravity strength.

Input Manager
- Take user cameraX and cameraY inputs using Unity's built-in input manager and storing it in my own. This allows for easy rebinding of controls.
- Take user X and Y input and combine into an input vector.

Camera Controller
- Add camera inputs to currentX and currentY, build a quaternion out of these orbit angles using Quaternion.Euler.
- Set camera position to camera target position (player), and apply a Z offset by specified dist amount, multiplied by orbit quaternion.

Player Controller
- Multiply input vector by camera rotation to convert vector from world to camera space.
- Cast multiple rays toward ground to get the distance and normal of the closest ground collider face.
- Adjust the player's forward (horizontal) rotation to match the input vector, using the current speed as a factor for how fast the rotation will be applied.
- Adjust the player's upward (vertical) rotation to match the average ground normal acquired from any raycasts that hit a collider.

TBC

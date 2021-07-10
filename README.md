# Early Physics Platformer (Sonic)

Starting in 2016 and continuing into 2017, I started work on a 3D platformer in Unity based on the physics driven gameplay found in the Sonic the Hedgehog series of games; something I've always wanted to make, and a driving factor in learning Unity

Gaining the required skills in vector math, trigonometry, and quaternions was the largest hurdle in this project, as these were things I had very limited knowledge of going in, and demanded a lot of learning.

Contains GameController, PlayerController, and CameraController scripts to handle main behaviour, with auxiliary scripts like MeshController and InputManager to handle additional behaviour (player mesh position/rotation and input, respectively). The size that these main scripts eventually reached was the start of understanding the downsides of large scripts with self-contained systems, and the benefit of dedicated individual scripts with specific purposes that work together to form the larger system.

As the behaviour in these absolutely monstrous controller scripts is so all-encompassing, I will list the main functions that are being performed each frame, specifying which controller handles the function:


Game Controller
- Sets player's intital position vector to a specified spawnpoint vector.
- Stores the current collectable amounts, and current global gravity strength.

Input Manager
- Take user cameraX and cameraY inputs using Unity's built-in input manager and storing it in my own. This is set up to remap controls without having to use Unity's input manager.
- Take user X and Y input and combine into an input vector.

Camera Controller
- Add camera inputs to currentX and currentY, build a quaternion out of these orbit angles using Quaternion.Euler.
- Set camera position to camera target position (player), and apply a Z offset by specified dist amount, multiplied by orbit quaternion.

Player Controller
- Multiply input vector by camera rotation to convert vector from world to camera space.
- Cast multiple rays toward ground to get the distance and normal of the closest ground collider face.
- If the player pressed the jump button this frame, disable all ground detection and forces, and apply a 'jump force' in the direction of the current ground normal.
- Increment the player's forward (horizontal) rotation to match the input vector, using the current speed as a factor for how fast the rotation will be applied.
- Subtract from the player speed when turning, using the angle difference between the current forward vector and the target input vector as a factor.
- Add acceleration amount to the player speeed scaled by input axis strength. 
- If no input is detected, subtract a friction amount from current speed to slow the player until they reach a standstill.
- If grounded, apply a 'slope influence' force:
    - If the player's forward direction Y value is above 0, the player is uphill. Subtract from current speed scaled by current ground angle.
    - If the player's forward direction Y value is below 0, the player is downhill. Add to current speed scaled by current ground angle.
- Adjust the player's upward (vertical) rotation to match the average ground normal acquired from any raycasts that hit a collider.
- Construct a movement vector by multiplying the current forward direction by the player's current speed, and setting the player's rigidbody velocity directly to this vector.
- Apply a 'ground stick' force to the player velocity that uses ground distance information from the detection raycasts to keep the player grounded.

Retaining vertical momentum and redirecting it into horizontal when hitting the ground was a mechanic I couldn't work out with my understanding of vector math at the time. However, after learning about cross products and vector projection, I would eventually solve this in future projects.


This was as simple as I could break it down without going into every single function. While I was really proud of what I had accomplished at the time, I knew the structure of the code could be drastically improved, and becoming more familiar with vectors allowed me to see a lot of alternatives to the mostly float and int based calculations I was working with.

This project was the basis for a game I would eventually develop as a passion project; Sonic Islands. In 2018, I released a demo as part of an online game expo, linked here:
https://sonicfangameshq.com/forums/showcase/sonic-islands-sage-2018-demo.212/

(The released game used a project that was built from the ground up seperate to this one, but the skills and tools required were largely formed during this practise project.)


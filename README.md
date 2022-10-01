# Evolution-Simulator

This project is made in Unity 3D 2018.1.6f1
To play test the project you can download the Release version and run it.
The game has no controls. You can click on an agent with the mouse to see what parameters it has. 

![Demo](EvolutionSimulator.gif)

Characteristics of each agent type:
- Herbivores
	- Grass - a green blob that the herbivore can eat
- Cernivores
	- Meat - a red blob that the carnivore can eat
- Both
	- Speed - a measure of how fast the agent is
	- Death - when the it hits 0 the agent dies
	- Lay Egg - the time it takes for an agent to lay an egg
	- Egg Spawn - the agent will spawn an egg after this amount of time
	- Cosnume Food - each blob of food is used to either extend the survival time or spawn an egg
	- Sight - the range that the agent can see infront or to the side
	- Survive - the amount of time it takes for the agent to die if it does nothing

There are 4 game speeds and you can also Pause/Play with the buttons on the left, under the agent characteristics.

Obstacles
The black sphertical obstacles will kill the agent if it touches the area. The external area that encloses the environment space or the spining rectangles do not kill the agent when touched.

Alarming system
The Herbivores can alert other herbivores of the dangerous carnivores. They will then run away from the danger and try to escape.

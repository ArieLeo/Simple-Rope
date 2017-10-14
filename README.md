# Simple-Rope
Unity tool for generating strings of connected 3D joints quicker


This tool is not exactly polished, and only created because it's tedious to connect large numbers of configurable joints together within Unity and connect line renders to them, and for some reason no free script to do this existed before.

Instructions:
1. Create a GameObject to act as a parent to the rope you want to create.
2. Add the "SimpleRope.cs" script to the object.
3. Assign a material and choose other configuration items:
	a. It is recommended that you use all "limited" for the joints.
	b. Assign a gizmo/icon to the rope parent object. It will propogate to each joint when you generate the rope.
	c. Configure the Start and End joints after rope generation and they will propogate to the other joints when you change their number.
4. Generate the rope using the "Generate Rope" button.
5. Position the rope objects using the parent object as the start and the end position object as the end.
	a. The rope script can be removed, but the positioning of the points will not auto-update.
	b. It is recommended that you remove the rope script when you are completely happy with the rope, but not before.
	It will increase performance in the editor (doesn't become noticable unless you have quite a few ropes).
	It cannot be undone.

It's recommended you go in and play with the limit settings in the joint to get the desired effect you want.

if you want to set the end of the rope to Kinemetic, use the end position as kinematic and not the end poit.

You can change the max ranges in the scripts below pretty easily

Scripts:
SimpleRope.cs - handles generation, configuration & positioning
SimpleRopeEditor.cs - Inspector replacement script
SimpleRopeLineRenderPositioning.cs - Updates the joint line renderers every frame.

That's it!
# BezierFun
Contains a Unity project and a game build, which are my solutions for the AGAMEDEV Testat 2 exercise.

## Repo Structure
The repo contains the following directories:
- _unity_: contains the unity project.
- _build_: contains the executable "game".

## The "Game"
The "game" consists of the Demo Scene from the Unity project, with the following features:
* first-person controllable player (mouse look, WASD for walking)
* trees objects with LOD
* billboard grass
* zombies which chase you when you get close (also have LOD)
* puddles of mud which the zombies can't walk on
* an airplane flying above the scene:
  * follows a set of Bezier curves
  * engines emmit particles
  * produces trails at the wing tips

## Unity Project
The Unity project contains:
* the demo scene (game)
* a curve test scene which showcases my humble curve system
* C# types:
  * _Bezier_: Calculates points along Bezier curves.
  * _CurveGenerator_: Component which produces a number of vertices along a Bezier curve defined by control points.
  * _CurveMover_: Component which makes an object move along a list of curves at a set speed.
  * _Billboard_: Component which implements the billboard behaviour -- makes an object rotate along the Y axis to always face the camera.
  * _FirstPersonPlayerControls_: Component which processes player input and implements simple first-person controls.

# HRTF-experiment

Unity project to measure the effectiveness of an HRTF file in a VR environment.

Important scripts and files:

* Assets/Scipts/Gun.cs: Handles controller vibration and raycasting on controller button press.
* Assets/Scipts/Sound_spawner.cs: Main game logic, handles starting sound locations, plays sounds after trigger press.
* Assets/Scripts/LoadHRTF.cs: Script to set the used HRTF to index 1 instead of the default index 0.

* data_processing/remove_chars.py: Reformat data files witth python to then be used by MATLAB.
* data_processing/auditory-localisation-evaluation-toolbox/read_data.m: MATLAB post-processing of data file provided by the experiment. (Sound_spawner.cs)

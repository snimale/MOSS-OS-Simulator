# MOSS - OS Simulator
A project made by Soham S. Nimale under the name of SleepWalker games (Google Play Store). Simulation of phase 1 and phase 2 of a multiprogramming operating system. Gives users the freedom to create their own scripts and run on the simulator. Input/Output files can be created/deleted/edited within the application itself. Implementation of core OS concepts like paging, page fault, generation of error code/status, input buffering, output buffering, PCBs, and so forth.

## Configurations :
#### Unity Engine Version : 2021.3.19f1
#### Platform : Android

## Player Prefs
format used : first_second_third_part_of_name
### Information on each Player Prefs used
<b>last_used_phase_number</b> : it is used to remember the last phase that user used/set up in the SetupMenu UI. It is updated OnClick Next/Previous for phase change.It is used to reflect the phase changes using phaseImage and phaseText in the SetupMenu UI. <br>
<b>input_file_name</b> : it contains the input file name in the Inputs folder in storage that the machine will read from. String value.<br>
<b>output_file_name</b> : it contains the output file name in the Outputs folder in storage that the machine will write into. String value.<br>
<b>machine_execution_latency</b> : it contains the value corresponding to the time delay / instruction execution of the CPU/Machine. It is a float value varying between 0.1 to 5.0 (both inclusive)</br>

## privacy policy
https://sites.google.com/view/moss-os-simulator/home
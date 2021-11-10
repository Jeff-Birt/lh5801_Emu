![Prerelease UI](/Images/emu72.png) # lh5801_Emu

Sharp lh5801 Microprocessor Emulator  

Written in C# using VS 2017

The RAM_ME0 and RAM_ME1 radio buttons set which RAM bank you are viewing,
which bank code you type in will be saved to and which bank is a file
is loaded into or saved from.

To enter code:
1) Type address in 'Address' box in hex, hit Enter
2) Set RAM bank ME0/ME1 to load into
3) Type code in 'Value' box, space delimited bytes in hex, hit Enter
4) The hex dump/editor is 'READ ONLY' at this point.

To load code from a file:
1) Code must be in binary form
2) Set Address text box to starting address code should be loaded into
3) Set RAM bank ME0/ME1 to load into
4) Use file dialog to load file, if it will not fit in RAM a message pops up

Saving:
1) Select the RAM bank ME0/ME1 to save
2) Use the file dialog to save file
3) Entire bank of RAM saved to one binary file

Defaults to Single Step mode. Pressing 'RUN' in this example runs until
the HLT opcode (FD B1) is encountered, this sets it back to Single Step mode
so it halts after this opcode finished.

Most opcodes should work properly a few have not been implemented yet
as testing on actual hardware needs to be done. Opcodes not implemented
ATP, CDV, ITA, OFF. See ToDO.txt for more details.

![Prerelease UI](/Images/lh5801_Emu_V0.5.png)

Overly simplistic change log:
03/11/2021 - 1) Fixed bug where Carry Flag was carried into INC and DEC
             2) Added very simple threading to allow UI to break a running CPU.
             
5/11/2021  - 1) Added stack viewer which can be set to 8/16 bit widths
             2) Added cycle counter, maximum count of FFFF
             3) Improved validation of Value input text box to catch values > FF

8/11/2021  - 1) Added CPU execution speed display
             2) Improved UI, all text boxes should be updated properly
             3) Added menu strip for file operations and removed buttons
             4) Made a To Do list in ToDo.txt

9/11/2021  - 1) Fixed crash when canceling file load/save dialogs
             2) Refactored several opcodes listed in ToDo.txt
             3) Implemented DRL and DRR opcodes
             4) Remaining opcodes not implemented require a BUS

10/11/2021 - 1) Tweaked load/save dialog 
             2) Created an actual installable release
             3) Spiffy Emu icon :)
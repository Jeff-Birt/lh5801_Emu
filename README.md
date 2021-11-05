# lh5801_Emu

Sharp lh5801 Microprocessor Emulator  

Written in C# using VS 2017

Can find debug build exe in 'Releases'

This is still a VERY early version and is most likely to crash a lot.

To enter code:

1) Type address in 'Address' box in hex, hit Enter
2) Type code in 'Value' box, space delimited bytes in hex, hit Enter
3) The hex dump/editor is 'READ ONLY' at this point.

Defaults to Single Step mode. Pressing 'RUN' in this example runs until
the HLT opcode (FD B1) is encountered, this sets it back to Single Step mode
so it halts after this opcode finished.

The RAM_ME0 and RAM_ME1 radio buttons set which RAM bank you are viewing
and which back will be loaded into or saved from. You can use the 'Load'
button to load a binary file into selected RAM bank starting at address
given by 'Address' text box. The 'Save' button will save the entire RAM
bank to a binary file. This was done to avoid creating a custom save
dialog at this point.

Most opcodes should work properly a few have not been implemented yet
as testing on actual hardware needs to be done. Opcodes not implemented
ATP, CDV, ITA, OFF and perhaps a few more.

![Prerelease UI](/Images/lh5801_Emu_01.png)

Overly simplistic change log:
03/11/2021 - 1) Fixed bug where Carry Flag was carried into INC and DEC
             2) Added very simple threading to allow UI to break a running CPU.
             
11/5/2021  - 1) Added stack viewer which can be set to 8/16 bit widths
             2) Added cycle counter, maximum count of FFFF
             3) Improved validation of Value input text box to catch values > FF
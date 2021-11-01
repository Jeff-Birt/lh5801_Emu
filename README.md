# lh5801_Emu
Sharp lh5801 Microprocessor Emulator  

Written in C# using VS 2017 

Can find debug build exe in 'Releases'

This is still a VERY early version and is most likely to crash a lot.

To enter code:
1) Type address in 'Address' box in hex, hit Enter
2) Type code in 'Value' box, space delimted bytes in hex, hit Enter

Defaults to Single Step mode. Pressing 'RUN' in this example runs until 
the HLT opcode (FD B1) is encountered, this sets it back to Single Step mode
so it halts after this opcode finihsed.

![Prerelease UI](/Images/lh5801_Emu.png)

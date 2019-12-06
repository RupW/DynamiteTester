# Simple Dynamite bot runner

This is a simple runner for [Dynamite](https://dynamite.softwire.com/) bots written in C# that you can use to
test and debug your bots locally.

To use:
1. clone or download this code
1. copy or move the 'DynamiteTester' folder (the one with Program.cs) into your bot's directory
1. right-click the solution and 'Add', 'Existing Project', then select the solution file in the new folder
1. right-click the new 'DynamiteTester' project and
   1. add a project reference to your bot project
   1. set as start-up project
1. open Program.cs in DynamiteTester and change one of the 'Bots to test' lines at the top to be your bot

You can then run the project. It will play rock-paper-scissors-dynamite-water-baloon with the two bots selected
and show you the round-by-round moves and final score.

This was written quickly to work - please don't pay too much attention to the code!

## Troubleshooting

1. If it won't accept your bot's class name in the tester code then please check that the .NET Core versions
   match in both projects: right-click project, 'Properties', 'Application' tab, Target framework. At time of writing
   the Dynamite server requires .NET Core 2.1.

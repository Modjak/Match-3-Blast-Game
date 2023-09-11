# Good-Job-Games
Case

There are two scenes, GameScene  and LevelSelectionScene. Game should be started in LevelSelectionScene. Rules for grid creation:

1) Threshold C > Threshold B > Threshold A.
2) Number of Rows >= Number of Columns.
3) Two color must be selected atleast.

Interaction and input is closed while animations or refilling of the board happening, in order not to intervene the blast and refilling process. 
Scripts are classified into two parts: 
1) Game Scripts
2) Level Selection Scripts 
Also one static class holds data for loading the Game Scene, this static class is reset when returning to the Level Selection Scene.

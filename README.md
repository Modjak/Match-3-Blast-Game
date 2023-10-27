
## Game Mechanics

The Collapse/Blast mechanic is a gameplay feature commonly found in tile-matching games. In these types of games, players are tasked with identifying clusters of blocks of the same color, and by tapping or clicking on these clusters, they eliminate the corresponding blocks from the game board. Subsequently, the empty spaces are refilled with the blocks stacked above them, and new blocks are generated to maintain the gameplay.

## Game Screenshots


![Level Selection Scene](images/level_selection_scene.png)
*Level Scene*

![Game Scene](images/games_cene.png)
*Game Scene*

![Game Scene](images/click.png)
*Click and Blast*



Certainly, here's a revised version of your text with improved English:

---

**Game Overview:**

Our game consists of two scenes: **GameScene** and **LevelSelectionScene**. The game is initiated from the **LevelSelectionScene**. 

**Grid Creation Rules:**

1. For grid creation, we follow these rules:
   - Threshold C should be greater than Threshold B, and Threshold B should be greater than Threshold A.
   - The number of rows in the grid must be greater than or equal to the number of columns.
   - At least two colors must be selected.

**Gameplay Interaction:**

During gameplay, touch or click input is temporarily disabled when animations are in progress or when the board is being refilled. This ensures that players don't interfere with the collapsing and refilling processes.

**Script Organization:**

We've organized our scripts into two categories:
1. **Game Scripts**
2. **Level Selection Scripts**

Additionally, we utilize a static class to manage data for loading the Game Scene. This static class is reset each time players return to the Level Selection Scene, providing a seamless gaming experience.

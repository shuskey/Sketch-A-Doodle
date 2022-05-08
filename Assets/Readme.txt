Sketch A Doodle Readme
To Add/Fix: 
[ ] Add brightness/contrast settings to WebCam capture screen
[ ] Add rotate clockwise 90 degrees to image
[ ] Add crop and/or additional borders to maze - eliminate the 'sneak around'
[ ] Edit Screen lets you choose a soundtrack for your maze
[ ] cool lighting (shader graph) effect like in 10 minute maze at https://www.youtube.com/watch?v=aP9eKrnyxe4
[ ] more sound effects: hitting a wall, jumping
[ ] Do something with 'score'  add pick-up treasures
[ ] Add holes
[ ] Add enemy that will slow you down or take you out
[ ] Can I get rid of UnityEditor functions, so this runs outside the editor
[ ] Can a maze be added by giving it a URL to an online image
[ ] Options for Wall Type: See through or not, High or Low (3D only)
[ ] Can this run as a Web App

-- COMPLETE --
[X] Player is not starting at the starting block, instear it is at 0,0,0 of the maze
[X] 3D player controller is loosing its orientation - strange 'main camera' setting where to blame
[X] Add toggle for InvertforBlack lines to edit screen.
[X] Worse in 2D mode - when falling disable movement - fall in off edge, player correct their path - now it looks like they are on the platform, but they are falling
[X] When Goal Achieve stop/pause animations/sound etc.
[X] How to limit player name length - 12 Chars
[X] ON played Chooser, can 'enter' do the same as the + button ?
[X] Add a Remove Player (will also remove all highscores for this player) - perhaps only on Edit screen ?
[X] This screen loads slowly - how to fix this? Smaller images ?? (lazy load only a page at a time ?
[X] Git rid of Scriptable object for current maze level.
[X] Guest looses its default spot when it is no longer alphabetically lower than another name
[X] Snap picture screen should go right to 'edit' screen after 'Snap'
[X] Edit screen can re-initialize high scores - Add Clear all HighScores ability.
[X] Add a "Delete Maze" ability somewhere that would be protected from button mashing monkey users
[X] perhaps add a save/play button to the edit screen
[X] Create a Tool bar tool (like the Height Map or Scriptable Objects creator) that creates the Sqlite entry for a new texture/sprte - THis is how you get new mazes from another file 
[X] Add Player Chooser to Snap picture screen
[X] Looks like FULL PAth is being used to save : C:/Users/Scott/Documents/GitHub/Sketch A Doodle/Assets/Mazes/Maze-30April-07-10-16.png - This should just be Assets/Mazes/Maze-30April-07-10-16.png
[X] New High Scores need a refresh of the table on this page
[X] Allow Player chooser changes
[X] Display Personal Best on screen "now playing 2D/3D screen" while playing
[X] When Goal Achieved and timer stops, evaluate: (New Scene/screen ??)
[X] - Is this a personal best for the current player
[X]      No, Great Time, but not a personal best (let user change current player for a different evaluation)
[X]	  Yes, Excellent, New Personal Speed Record!  This puts you in 10th place (or what ever)
[X] - Button choices [Try Again (goes to playmode chooser screen)] [Home/Back/(back to play this maze)]  <If new personal record, then save New High Score>

-- CHOOSE YOUR PLAY MODE 2D 3D SCREEN
[X] High Scores should be listed on the "play 2D or 3D" screen
[X]   Display Top 5 scores/player name for 2D, Top 5 for 3D
[X] Add PlayerChooser to "play 2D or 3D" screen

[X] FIX: Initial Player position is (0,0) - falling usually fixes this
[X] convert Maze scriptable objects into serialized data save/load.
[X] Add SQLite
[X] Player chooses 2D or 3D play mode
[X] Should have a restart level button - like falling off the edge, but on purpose -> Back Button does this
[X] Need to lock 'player rotation' in 2D Maze Mode
[X] option to make mini map close/open
[X] option to clear Fog of War
[X] Option to play as just a 2D maze - Think Mini Map Only
[X] Can I get a Maze Scan from the Laptop camera - or external camera - while running Unity ?
[X] UI button and or input button to restart or end the game
[X] See how Scriptable Objects might help with Maze Data (name, creator, texture, start location, end location)
[X] Read complete list of scriptable maze objects into a list
[X] Display a scrollable list of Maze Display game objects based on list of Maze Scriptable Objects
[X] affix camera to player - Or '1st person player' - and/or fix 'look around' jerkiness
[X] Have a fog around the mini map, fog dissipates as you travel around the maze
[X] Footprints would be cool

-- B U G S --
[ ] Footprints on the walls?! Come on! Keep it clean.

-- O T H E R --
[ ] Best Practices for checking a project like this into GitHub with so many packages

Notes
FYI - Scripts attached to Player
Audio Source
First Person Move Only
Trigger Tracker
Timer (connect to Text Mesh Pro) ElapsedTimer Text & StartTimer Text
Foot Steps - has images for foot prints, location of feet, audio clip for jump landing sound, list of footsteps sound clips


	
Sketch A Doodle Readme
To Add:
[ ] Collect High Scores (perhaps top 5) for each maze one table for 2D one for 3D
[ ] Edit screen can re-initialize high scores
[ ] convert Maze scriptable objects into serialized data save/load.
[ ] Add SQLite
[ ] Options for Wall Type: See through or not, High or Low

[ ] When Cherry is triggered - remove the parent or turn it into something else - crown, rainbow, gold coin
[ ] Can I get rid of UnityEditor functions, so this runs outside the editor
[ ] Can a maze be added by giving it a URL to an online image
[ ] Can this run as a Web App
[ ] Add brightness/contrast settings to WebCam capture screen

-- COMPLETE --
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
Trigger Tracker
Timer (connect to Text Mesh Pro) ElapsedTimer Text & StartTimer Text
Foot Steps - hase images for foot prints, location of feet, audio clip for jump landing sound, list of footsteps sound clips

Data for Maze Level Serialization
    [Header("Title for this maze..")]  							public string title;
    [Header("Who created this maze?")] 							public string creator;
    [Header("The File Name for the texture for this maze.")]	public string mazeTextureFileName;
    [Header("True if you have a maze with a white background.")]public bool invertToUseBlackLines;
    [Header("Range 0.0 to 1.0 for each coordinate.")]			public Vector2 startPositionRatio;  // range from 0.0 to 1.0 for each x and y
    [Header("Range 0.0 to 1.0 for each coordinate.")]			public Vector2 endPositionRatio; 
	[Header("Date Created")]									public DateTime CreatedDate;
	[Header("Populatity based on number of completed plays")]	public int numberOfPlayThroughs; 
	[Header("High scores for 3D play")]							public List<MazeHighScore> highScores3D; 	
	[Header("High scores for 2D play")]							public List<MazeHighScore> highScores2D; 	
	
	MazeHighScore:
	[Header("Place 1st,2nd,3rd,4th,5th")]						public int place;
	[Header("Player Name")]										public int playerName;
	[Header("Date recorded")]									public DateTime dateForScore;
	// perhaps Awarded Score calculated like 1000 - time in seconds + treasure_value X quatitly - enemy_damage X quantity - jump_cost X quantity
	[Header("Awarded Score")]									public int score;
	[Header("Time in seconds")]									public int timeInSeconds;
	[Header("Number of jumps")]									public int numberOfJumps;
	
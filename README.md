# SCTicTacToe
Starcraft Classic Tic Tac Toe (Gomoku)

Code and Graphic Design by Joshua Fontany 
Email: joshua.fontany@gmail.com  
LinkedIn: https://www.linkedin.com/in/joshuafontany/ 
 
Original Starcraft 1 and 2 graphics © Blizzard Entertainment. 
 
Used without permission. Any use of Blizzard Entertainment's  
copyrighted material or trademarks in this file should not be  
viewed as a challenge to those copyrights or trademarks.

Features include:

2 Game Modes: Tic Tac Toe (3 In A Row), and Domoku (5 In a Row, played on larger boards). Choose game mode on the New Game screen.

UI fully skinned and themed on Starcraft Classic (from resources and wallpaper available on Battle.Net). Each game represents a skirmish on the never ending wars between the Terran, Zerg, and Protoss forces. Gotta tell the StarCraft story!

Fully skinned responsive MVVM based WPF GUI. Drag the window to any size without breaking the application or layout.

Max 15 character usernames that do not break the UI. Usernames are Regex scrubbed for valid characters.

Fully functional Leaderboard using the player's chosen Hero Icon (updates with new choice when starting a new-game).

Gameboard pieces use animated gifs from SC1. As WPF/XAML does not natively support gifs, it was necessary to incorporate a 3rd party animation library.

Use of Reflection and Singleton patterns to automatically handle the large lists of Background and and Hero Avatar Images. Just place any new image of the right size (1024x768 for backgrounds, 64x64 for Hero Icons) in the right folder, and the app will automatically pick it up and add it to the UI. No managing massive lists of resource URLs in the code files.

Object Oriented UI and Game logic, using DelegateCommands, and NotifyPropertyChanges event streams using proven references (which use 3rd party libraries such as "Reactive.Net").

Dynamic gameboard UI generation based on size, 3x3 (tic tac toe) / 9x9 (gomoku), using a single custom Gameboard WPF Control.

Logic to minimize the search space when checking for the win condition, as you only need to check the row, column, and diagonal connected to the last move made. I'm really happy with how this turned out, as it can apply to any sized board and any win condition and without the performance hit of traversing the whole board.

Dynamic Leaderboard sorting.

Save game logic to persist the game in progress and player stats between app session. 
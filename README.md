# BeanCell-Unity-
This is a Unity based FreeCell game written in C#

This project was born out of a love for Windows XP FreeCell. I played it a lot while growing up, and I was incredibly disappointed when I finally switched to Windows 7
and could no longer play the XP version.

My previous laptop, after years of use, lost the function of all its USB ports. Using the trackpad was tedious, so even playing online FreeCell clones became incredibly
difficult and unfun. After my first CS class, using materials from one of the projects, I made a console based FreeCell game in Java so that I could play with just my
keyboard. It was fun but quite primitive. 

Eventually, that laptop croaked completely (R.I.P.). The hard drive failed, and I had stupidly not backed up the finished version of that project. I bought a new laptop
(with functioning USB ports!) and decided to remake it. However, I could use a real mouse again, so I decided I might as well make a GUI as well. After some amount
of shopping for an engine (turns out there are quite a few options), I decided on Unity. Hence, this! It's been a very fun project that has taught me a lot.

All assets are free use as I have no interest in monetizing this. I'm grateful for all artists who put their work up for these kinds of uses.

Features thus far:
1) The game totally works. (I guess that should be implied, but I'm saying it anyway!)
2) Has a working undo button (can also be saved/loaded with the rest of the game)
3) Main menu and game over screens
4) Save/load capability for 1 game. That is, you can choose to start a new game or continue a game already started.

Points of pride:
1) There are elements of basic recursion used for the game logic
2) JSON serialization/deserialization for the save/load function

Planned/ideal features (I hope I actually get to doing these):
1) Multiplayer support (because why shouldn't FreeCell be 2 player!?)
2) High score system or a win/loss tracker
3) Game numbering and allowing user to pick a specific game (capability is already almost there)
4) Some sort of solver to tell you when you've lost (I expect this to be the most difficult of all these)

Thanks for playing/perusing!
![BeanCell Start Menu](https://user-images.githubusercontent.com/111920505/198083846-845088aa-68e4-42b9-bbd5-c5ca660107d9.png)
![BeanCellAlmostOver](https://user-images.githubusercontent.com/111920505/198083907-92d5113b-221e-4609-84f9-09f338f7946c.png)
![BeanCellOver](https://user-images.githubusercontent.com/111920505/198083921-52f8d095-24d5-4e72-9917-8ae6e74c26bc.png)

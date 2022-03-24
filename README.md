# Moondown
Moondown is a sci-fi metroidvania game made
with Unity. The game will use an AI to 
adapt to, and learn about the player. The 
game includes elements of stealth, but still 
puts a large emphasis on combat.

## Installation 
To install Moondown on your device as a clone
of this repository you have to:
* Have Unity 2019 or later
* Have an IDE (Preferably VisualStudio)
* Make sure all the packages required are installed 
* Clone this repository 

## Adding translations
Moondown uses a real-time translation system. All TMP 
text in the game will be replaced with the corresponding translation.

### Adding translations to an existing language
To add translations to an existing language you have to:
* Go to Assets/Translation
* Select the correct translation file
* Find the correct key
* Replace the temporary translation with an actual translation
* Make sure there is a space on both sides of the equals sign
* Make sure there is a semicolon at the end

### Adding new content
To add a new translated text to the game you should:
* Set the content of the text to your desired key (UPPER_SNAKE_CASE)
* Navigate to Moondown Tools/Refresh Translations
* Click the refresh button
* Follow the steps mentioned under the previous sub heading

### Adding a new language
To add a new language to the game you should:
* Go to Assets/Translation and add a new file ending in .txt
* The file should be named after a country code 
* Select and click Moondown Tools/Refresh Translations
* Follow the steps mentioned under the "Adding translations to an existing language" sub heading

## License 

It is licensed under the GPLv3.

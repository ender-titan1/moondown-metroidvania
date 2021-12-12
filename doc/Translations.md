# Translation files
Each language has its own file in the `Moondown/Assets/Translation` directory.

## The structure of a translation file
A translation file is composed of lines of keys and values, for example: `GREETING = hello;`
Notice the semicolon at the end, a semicolon has to end every line.
The equals in the center can either be seperated by spaces from both sides or not at all.

### Comments
A comment is a line of the file that is ignored when searching for key-value pairs. They look like this:
```
# This is a comment;
```
Comments begin with a hash symbol and are ended with a semicolon.

### Unknowns
You might want to include unkowns in the translations. Check `using DynamicText.md` on how to do that.

# Adding a translation
To add a translation you need to:
* First, make a text game object, set it's text as your desired key
* Secondly, click on the `Moondown Tools` tab and select `Refresh Translations`
* Thirdly, find the key in the translation files and add a value, do this for all languages you know how to translate
# Adding a language 
To add a new language you need to:
* Go to the `Moondown/Assets/Translation` directory
* Make a new file with the appropriate [ICU code](https://www.localeplanet.com/icu/) as the name 
* Add the newly created asset to the list of languages in the `Localization` game object
* Click on the `Moondown Tools` tab and select `Refresh Translations`

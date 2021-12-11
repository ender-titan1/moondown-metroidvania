# Dynamic Text
DynamicText is a component used to format translations.

## When to use it
DynamicText should be used when a translation has an unknown element.
For example:
```
EXAMPLE_KEY = Use Dynamic Text here: {0};
# The unknown here is the '{0}' which will be replaced at runtime;
```

## How to use it
Give an object the DynamicText component/script. Then put in the inputs.

### The inputs
An input is a string composed of three parts:
```
<class>.<instance>.<property>
```
There are a few rules that an input has to follow:
* It must come from a singelton 
* It must reference a public property **not field** 

For example `PlayerManager.Instance.MaxHealth` refrences the `MaxHealth` property in the instance of the singelton, PlayerManager

### What gets replaced
Each input corresponds replaces the unknow with the same index as their position in the array (starting form 0).
So `{0}` would be the input with index 0,
`{1}` woud be the input with index 1, etc...

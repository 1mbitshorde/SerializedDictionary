# Serialized Dictionary

* Serialized Dictionary for Unity with native look and feel
* Unity minimum version: **2020.1**
* Current version: **1.2.0**
* License: **MIT**

## Summary

As of 2020.1.x, Unity supports generic serialization and native support for displaying generic Lists in the inspector. 
But for a long time the community has wanted a generic Dictionary implementation that doesn't require you to add boilerplate for each concrete Dictionary type.

## Features

* Uses plain ``System.Collections.Generic`` objects in combination with Unity's generic serializer.
* Implements the ``IDictionary`` interface and can also be passed around as an ``ICollection``.
* Property drawer that displays the Dictionary as a List:

![](/Docs~/Inspector.gif)

* Displays a warning when duplicate keys are found:

![](/Docs~/InspectorWithDuplicateKeys.PNG)


## How To Use

Zero boilerplate, declare your ``SerializedDictionary`` and start using it!

```csharp
using UnityEngine;
using ActionCode.SerializedDictionaries;

namespace YourNamespace
{
    [DisallowMultipleComponent]
    public sealed class Example : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<string, GameObject> myGenericDict;
    }
}
```

## Installation

### Using the Git URL

- You can also manually modify you `Packages/manifest.json` file and add this line inside `dependencies` attribute: 

```json
"com.actioncode.serialized-dictionary":"https://github.com/1mbitshorde/SerializedDictionary.git"
```
---

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

### Using the Package Registry Server

Follow the instructions inside [here](https://cutt.ly/ukvj1c8) and the package **ActionCode-Serialized Dictionary** 
will be available for you to install using the **Package Manager** windows.

### Using the Git URL

You will need a **Git client** installed on your computer with the Path variable already set. 

- Use the **Package Manager** "Add package from git URL..." feature and paste this URL: `https://github.com/HyagoOliveira/SerializedDictionary.git`

- You can also manually modify you `Packages/manifest.json` file and add this line inside `dependencies` attribute: 

```json
"com.actioncode.serialized-dictionary":"https://github.com/HyagoOliveira/SerializedDictionary.git"
```

---

**Hyago Oliveira**

[GitHub](https://github.com/HyagoOliveira) -
[BitBucket](https://bitbucket.org/HyagoGow/) -
[LinkedIn](https://www.linkedin.com/in/hyago-oliveira/) -
<hyagogow@gmail.com>
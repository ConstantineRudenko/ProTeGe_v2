# ProTeGe v2

Graph-based procedural texture generator.

## Features
* Various noises, patterns, blending modes, filters, etc.
* Generate a full set of textures from a single graph (albedo, normals, smoothness, metallicity, AO, parallax, emission).
* Save / Load graph (save file uses json format).
* Smart texture caching - node is updated only when its input was changed.
* Multiple resolutions for preview and export (512, 1024, 2048, 4096, 8192)
* Always use best quality for export (gaussian blur, etc.)

## ToDo
Doxygen, demo video

## Unity version

**2021.3.10f1** (LTS)

## Credit

* Developed in collaboration with Ihor Los ([VectorElk](https://github.com/VectorElk))
* Uses SMAA implementation by Thomas Hourdel ([Chman](https://github.com/Chman)) [[zLib](https://github.com/KosRud/ProTeGe_v2/blob/master/Assets/SMAA-master/LICENSE.txt)]
* Uses [Wispy Skybox](https://assetstore.unity.com/packages/2d/textures-materials/sky/wispy-skybox-21737) asset by [Mundus](https://assetstore.unity.com/publishers/4555)

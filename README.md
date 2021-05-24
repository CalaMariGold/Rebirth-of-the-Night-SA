# Rebirth-of-the-Night-SA
The standalone Unity version of Rebirth of the Night.

### Installing Private Assets
This repository utilizes a submodule to store Assets which cannot be shared publicly.

If you are a member of the team and utilizing a system that supports git symlinks (e.g. unix systems): 

 * Run `git submodule init && git submodule update` in the root directory 
 * Enter your credentials to access the private assets
 * Run `cd PrivateAssets && git lfs fetch` 
 
If you are a member of the team and utilizing a windows system:

 * Run `FixSymlinks.ps1`
 * Enter your credentials when prompted to access the private assets


If you are not a member of the team, you must purchase and install the following assets yourself:

 * [Low Poly Survival Kit](https://assetstore.unity.com/packages/3d/environments/low-poly-survival-kit-161250)
 * [Rainbow Folders 2](https://assetstore.unity.com/packages/tools/utilities/rainbow-folders-2-143526)
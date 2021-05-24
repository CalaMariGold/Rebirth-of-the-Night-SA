# Initialize submodules if not done already
git submodule init
git submodule update

cd PrivateAssets
git lfs fetch
cd ..

# Run PrivateAsset Script
.\PrivateAssets\PlaceAssets.ps1
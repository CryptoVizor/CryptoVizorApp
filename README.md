<p align="center">
  <img src="readmeFiles/appLogo.png" width="256" height="256">
</p>

<p align="center"><b>CryptoVizor</b> is an app that can show NFT in AR.</p>

<p align="center">The app is iOS only for now because we only use ARKit with unity but if you wanna add ARCore support you can open a pull request.</p>

<div align="center">
  <a href="https://unity3d.com"><img src="https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white"></a>
  <a href="https://www.reddit.com/user/CryptoVizor"><img src="https://img.shields.io/badge/Reddit-FF4500?style=for-the-badge&logo=reddit&logoColor=white"></a>
  <a href="https://twitter.com/CryptoVizorNFT"><img src="https://img.shields.io/badge/CryptoVizorNFT-%231DA1F2.svg?style=for-the-badge&logo=Twitter&logoColor=white"></a>
  <a href="https://flutter.dev"><img src="https://img.shields.io/badge/Flutter-%2302569B.svg?style=for-the-badge&logo=Flutter&logoColor=white"></a>
  <a href="https://stakes.social/0x3059bD281418179A83cAE3771b0dD6C47807EA3a"><img src="https://badge.devprotocol.xyz/0x3059bD281418179A83cAE3771b0dD6C47807EA3a/descriptive"></a>
  </div>
  
---
<div align="center">
  <a href="https://github.com/CryptoVizor/CryptoVizorApp/edit/main/README.md"><img src="https://img.shields.io/badge/Maintenance%20Level-Not%20Maintained-yellow.svg"></a>
</div>

<p align="center">
  <b>The repository is not actively maintained but you still can support it on</b>
  <br>
  <br>
  <a href="https://stakes.social/0x3059bD281418179A83cAE3771b0dD6C47807EA3a"><img width="179" src="https://user-images.githubusercontent.com/17464685/129601828-fd461e84-bee7-4293-8fd6-f9cd3692f8ad.png"></a>
</p>

<p align="center">
  <b>The app is also available on the AppStore for free.</b>
  <br>
  <br>
  <a href="https://apps.apple.com/us/app/cryptovizor/id1529722418"><img src="readmeFiles/Download_on_the_App_Store_Badge_US-UK_RGB_blk_092917.svg"></a>
</p>

There is a significant difference between GitHub and AppStore version, it is the ability to take pics.  
Because we use a paid plugin for the AppStore version we don't upload it to Github. We will update this part to use an open-source library later.

<p align="center">
  <img src="readmeFiles/demo.gif">
  <p align="center">Demo gif using <a href="https://axieinfinity.com">Axie Infinity NFT</a></p>
</p>



## Supported NFTs

The app supports image and MP4 NFTs that are available on opensea.


## How to build

This project use [flutter_unity_widget](https://github.com/juicycleff/flutter-unity-view-widget) 


So in order to build this project, you first need to build the unity project as a library.
To do so you need to install all the dependencies and then click the newly appeared flutter button.

<img width="214" alt="ExportiOS" src="https://user-images.githubusercontent.com/17464685/129600274-63598f83-e851-4141-afaa-988f6e4d1b4d.png">

Then select export iOS.

After that, if there is no build error you can proceed to the flutter part and build it as a normal flutter project.

Or you can just call `flutter run` if you wanna run it on your device with debugging enabled.





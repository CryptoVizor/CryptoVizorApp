<p align="center">
  <img src="readmeFiles/appLogo.png" width="256" height="256">
</p>

<p align="center"><b>CryptoVizor</b> is an app that can show NFT in AR.</p>

<p align="center">The app is iOS only for now because we only use ARKit with unity but if you wanna add ARCore support you can open a pull request.</p>

[![Unity](https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white)](https://unity3d.com)
[![Reddit](https://img.shields.io/badge/Reddit-FF4500?style=for-the-badge&logo=reddit&logoColor=white)](https://www.reddit.com/user/CryptoVizor)
[![Twitter](https://img.shields.io/badge/CryptoVizorNFT-%231DA1F2.svg?style=for-the-badge&logo=Twitter&logoColor=white)](https://twitter.com/CryptoVizorNFT)
![Flutter](https://img.shields.io/badge/Flutter-%2302569B.svg?style=for-the-badge&logo=Flutter&logoColor=white)
[![Stake to support us](https://badge.devprotocol.xyz/0x3059bD281418179A83cAE3771b0dD6C47807EA3a/descriptive)](https://stakes.social/0x3059bD281418179A83cAE3771b0dD6C47807EA3a)
---


<p align="center">
  <b>Support US on</b>
  <br>
  <a href="https://stakes.social/0x3059bD281418179A83cAE3771b0dD6C47807EA3a"><img width="179" alt="Screen Shot 2021-08-17 at 2 01 03" src="https://user-images.githubusercontent.com/17464685/129601828-fd461e84-bee7-4293-8fd6-f9cd3692f8ad.png"></a>
</p>

<p align="center">
  <p align="center">The app is also available on AppStore for free.</p>
  <br>
  <a href="https://apps.apple.com/us/app/cryptovizor/id1529722418"><img width="179" alt="Screen Shot 2021-08-17 at 2 01 03" src="readmeFiles/Download_on_the_App_Store_Badge_US-UK_RGB_blk_092917.svg"></a>
</p>


There is a significant difference between GitHub and AppStore version, it is the ability to take pics.  
Because we use a paid plugin for the AppStore version we don't upload it to Github. We will update this part to use an open-source library later.

<p align="center">
  <img src="readmeFiles/demo.gif">
  <p align="center">Demo gif using <a href="https://axieinfinity.com">Axie Infinity NFT</a></p>
</p>



## Supported NFTs

The app supports image and MP4 NFTs that are available on opensea.

## Questions and Sugesstions

If you have any suggestions or feature requests you can post them in the issues.

If you have any questions you can ask them on Twitter or Reddit. The links are in the header.

## How to build

This project use [flutter_unity_widget](https://github.com/juicycleff/flutter-unity-view-widget) 
So in order to build this project, you first need to build the unity project as a library.
To do you need to install all the dependencies and then click the newly appeared flutter button.

<img width="214" alt="ExportiOS" src="https://user-images.githubusercontent.com/17464685/129600274-63598f83-e851-4141-afaa-988f6e4d1b4d.png">

Then select export iOS.

After that, if there is no build error you can proceed to the flutter part and build it as a normal flutter project.

Or you can just call `flutter run` if you wanna run it on your device with debugging enabled.





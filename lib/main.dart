import 'dart:convert';
import 'dart:typed_data';

import 'package:flutter/material.dart';
import 'package:dio/dio.dart';
import 'package:flutter/rendering.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:settings_ui/settings_ui.dart';
import 'package:url_launcher/url_launcher.dart';

void main() {
  runApp(MaterialApp(home: UnityScreen()));
}

class UnityScreen extends StatefulWidget {
  UnityScreen({
    Key key
  }): super(key: key);

  @override
  _UnityScreenState createState() => _UnityScreenState();
}

enum WidgetIndex {
  login,
  settings,
  collections,
  item
}

class _UnityScreenState extends State < UnityScreen > {
  var account = "";
  String background = "";
  List < Map > items = [];
  bool overlay = false;
  List < Map > slugs = [];
  TextEditingController walletController = TextEditingController();
  WidgetIndex widgetIndex = WidgetIndex.login;

  static final GlobalKey < ScaffoldState > _scaffoldKey =
  GlobalKey < ScaffoldState > ();

  final GlobalKey < FormState > _formKey = GlobalKey < FormState > ();
  UnityWidgetController _unityWidgetController;

  Widget build(BuildContext context) {
    return Scaffold(
      appBar: overlay && widgetIndex != WidgetIndex.login ?
      AppBar(
        flexibleSpace: SafeArea(child: Image(
          height: 50,
          width: 50,
          image: background != "" && background != null ?
          NetworkImage(background) :
          AssetImage("assets/images/logo.jpg"),
          fit: BoxFit.scaleDown,
          alignment: Alignment.topCenter,
        )),
        backgroundColor: Colors.white,
        leading:
        BackButton(
          color: Colors.black,
          onPressed: () {
            setState(() {
              if (widgetIndex == WidgetIndex.settings) {
                overlay = false;
              }
              if (widgetIndex == WidgetIndex.collections) {
                overlay = false;
                return;
              }
              widgetIndex = WidgetIndex.collections;
              background = "";
              items = [];
            });
          },
        ),
      ) :
      null,
      key: _scaffoldKey,
      body:
      Stack(children: [
        Container(
          color: Colors.black,
          child: UnityWidget(
            onUnityCreated: onUnityCreated,
            onUnityMessage: onUnityMessage,
          )),
        overlay ? IndexedStack(index: widgetIndex.index, children: [ //Login
          SizedBox.expand(child: Container(color: Color(0xff9930F4), child: Form(
            key: _formKey,
            child: Column(
              mainAxisAlignment: MainAxisAlignment.start,
              crossAxisAlignment: CrossAxisAlignment.center,
              children: < Widget > [
                Container(height: 150, ),
                Image.asset("assets/images/icon_white.png", height: 100, ),
                Container(height: 120, ),
                Text("Login to CryptoVizor", style: TextStyle(fontSize: 30, color: Colors.white, fontWeight: FontWeight.bold)),
                Container(height: 20, ),
                Container(decoration: new BoxDecoration(
                  color: Colors.white,
                  borderRadius: new BorderRadius.all(Radius.circular(40.0))
                ), margin: EdgeInsets.only(left: 20, right: 20), padding: EdgeInsets.only(left: 20, right: 20), child: TextFormField(
                  controller: walletController,
                  decoration: const InputDecoration(
                      hintText: 'Enter your wallet address',
                    ),
                    validator: (String value) {
                      if (value == null ||
                        value.isEmpty ||
                        value.length != 42) {
                        return 'The wallet address is incorrect';
                      }
                      return null;
                    },
                ), ),
                Padding(
                  padding: const EdgeInsets.symmetric(vertical: 16.0),
                    child: TextButton(
                      onPressed: () {
                        // Validate will return true if the form is valid, or false if
                        // the form is invalid.
                        if (_formKey.currentState.validate()) {
                          setState(() {
                            widgetIndex = WidgetIndex.collections;
                          });
                          writeLogin(walletController.text);
                        }
                      },
                      child: Container(height: 40, width: 120, decoration: new BoxDecoration(
                        color: Colors.white,
                        borderRadius: new BorderRadius.all(Radius.circular(40.0))
                      ), alignment: Alignment.center, child: Text('Continue', style: TextStyle(color: Color(0xff9930F4), fontSize: 18, fontWeight: FontWeight.bold))),
                    ),
                ),
              ],
            ),
          ))),
          //Settings
          SettingsList(
            sections: [
              SettingsSection(
                title: 'Wallet Address',
                tiles: [
                  SettingsTile(
                    title: account,
                    iosChevron: Icon(null),
                    iosChevronPadding: EdgeInsets.zero,
                    titleTextStyle: TextStyle(color: Colors.black,fontWeight: FontWeight.w900, fontSize: 12,fontFamily:'Noto Sans CJK SC' ),
                  ),

                ],
              ),
              SettingsSection(
                title: 'Bassic information',tiles: [
                  SettingsTile(
                    title: "Contact Us",
                    iosChevron: Icon(Icons.arrow_forward_ios),
                    titleTextStyle: TextStyle(color: Colors.black,fontWeight: FontWeight.w900, fontSize: 14,fontFamily:'Noto Sans CJK SC' ),
                    onPressed: (context)async => await canLaunch("https://forms.gle/T5zg1pYbRhYVbkBm7") ? await launch("https://forms.gle/T5zg1pYbRhYVbkBm7") : throw 'Could not launch "https://forms.gle/T5zg1pYbRhYVbkBm7"'
                    ,
                  ),
                  SettingsTile(
                    title: "Privacy Policy",
                    iosChevron: Icon(Icons.arrow_forward_ios),
                    titleTextStyle: TextStyle(color: Colors.black,fontWeight: FontWeight.w900, fontSize: 14,fontFamily:'Noto Sans CJK SC' ),
                    onPressed: (context)async => await canLaunch("https://www.notion.so/tart/Privacy-Policy-3fee0790959646d380ff4de8dfc19084") ? await launch("https://www.notion.so/tart/Privacy-Policy-3fee0790959646d380ff4de8dfc19084") : throw 'Could not launch "https://www.notion.so/tart/Privacy-Policy-3fee0790959646d380ff4de8dfc19084"'
                  ),
                  SettingsTile(
                    title: "About Us",
                    iosChevron: Icon(Icons.arrow_forward_ios),
                    titleTextStyle: TextStyle(color: Colors.black,fontWeight: FontWeight.w900, fontSize: 14,fontFamily:'Noto Sans CJK SC' ),
                    onPressed: (context)async => await canLaunch("https://www.notion.so/tart/Our-Team-ffc9f64068b44df397ffea9f7f33915a") ? await launch("https://www.notion.so/tart/Our-Team-ffc9f64068b44df397ffea9f7f33915a") : throw 'Could not launch "https://www.notion.so/tart/Our-Team-ffc9f64068b44df397ffea9f7f33915a"'
                  )
                  ],),
                  SettingsSection(
                title: '',tiles: [
                  SettingsTile(
                    title: "Reset the app",
                    iosChevron: Icon(null),
                    titleTextStyle: TextStyle(color: Colors.red,fontWeight: FontWeight.w900, fontSize: 14,fontFamily:'Noto Sans CJK SC' ),
                    onPressed: (context) async{
                      final storage = new FlutterSecureStorage();
                      await storage.deleteAll();
                      setState(() {
                        widgetIndex = WidgetIndex.login;
                        account = "";
                      });
                    },
                  ),
                  ],)
            ],
          ),
          //Collections
          Container(
            color: Colors.white,
            height: 1000,
            child: SafeArea(
              child: ListView.builder(
                scrollDirection: Axis.vertical,
                // shrinkWrap: true,
                itemCount: slugs.length,
                itemBuilder: (BuildContext ctx, index) {
                  return Container(
                    alignment: Alignment.center,
                    child: TextButton(
                      onPressed: () => {
                        print("categories"),
                          setState(() {
                            widgetIndex = WidgetIndex.item;
                            background = slugs[index]
                              ["image"];
                          }),
                          getItems(
                            slugs[index]["slug"]),
                      },
                      child: slugs[index]["image"] != "" ?
                      Container(
                        height: 50,
                        child: Flex(
                          direction:
                          Axis.horizontal,
                          mainAxisAlignment:
                          MainAxisAlignment
                          .start,
                          crossAxisAlignment:
                          CrossAxisAlignment
                          .center,
                          children: [
                            Spacer(flex: 1),
                            Flexible(
                              flex: 1,
                              child: Image
                              .network(slugs[
                                  index]
                                [
                                  "image"
                                ])),
                            Spacer(flex: 1),
                            Flexible(
                              flex: 7,
                              child: Text(
                                slugs[index]
                                ["slug"],
                                textAlign:
                                TextAlign
                                .center,
                                style: TextStyle(
                                  fontSize: 20,
                                  color: Colors
                                  .black),
                              ))
                          ],
                        )) :
                      Text("unsuported")));
                }))),
          Container(
            color: Colors.white,
            height: 1000,
            child: GridView.builder(
              scrollDirection: Axis.vertical,
              shrinkWrap: true,
              gridDelegate:
              SliverGridDelegateWithMaxCrossAxisExtent(
                maxCrossAxisExtent: 200,
                childAspectRatio: 4 / 5,
                crossAxisSpacing: 20,
                mainAxisSpacing: 0),
              itemCount: items.length,
              itemBuilder: (BuildContext ctx, index) {
                return Container(
                  alignment: Alignment.center,
                  child: TextButton(
                    onPressed: () => {
                      this
                        ._unityWidgetController
                        .postJsonMessage(
                          "ARPlacementInteractable",
                          "setCard", {
                            "tokenIDOverlay": items[index]
                              ["id"],
                            "URI": items[index]["image"],
                            "nameOverlay": items[index]
                              ["name"],
                            "animation": items[index]
                              ["animation"] != null ? items[index]
                              ["animation"] : "null"
                          }),
                        setState(() {
                          overlay = false;
                        }),
                        print("postJsonMessage")
                    },
                    child: Image.network(
                      items[index]["image"])));
              })),
        ]) :
        Container()

      ])


    );
  }

  void getCategories() async {
    if (account == "") {
      return;
    }
    try {
      var response = await Dio().get(
        'https://api.opensea.io/api/v1/collections?asset_owner=$account&limit=300');

      List < Map > cotegories = [];

      for (var item in response.data) {
        cotegories.add({
          "image": item["image_url"],
          "bannerImageURL": item["banner_image_url"],
          "slug": item["slug"]
        });
      }
      print(cotegories);
      setState(() {
        slugs = cotegories;
      });
    } catch (e) {
      print(e);
    }
  }

  void getItems(String slug) async {
    try {
      var response2 = await Dio().get(
        'https://api.opensea.io/api/v1/assets?owner=$account&collection=$slug&offset=0&limit=50');

      List < Map > tokens = [];

      for (var item in response2.data["assets"]) {
        tokens.add({
          "image": item["image_url"],
          "animation": item["animation_url"],
          "id": item["token_id"],
          "name": item["name"]
        });
      }
      setState(() {
        items = tokens;
      });
    } catch (e) {
      print(e);
    }
  }

  // Callback that connects the created controller to the unity controller
  void onUnityCreated(controller) {
    checkLogin();
    this._unityWidgetController = controller;
  }

  void checkLogin() async {
    final storage = new FlutterSecureStorage();
    String wallet = await storage.read(key: "wallet");
    print("wallet");
    print(wallet);
    if (wallet != null) {
      setState(() {
        widgetIndex = WidgetIndex.collections;
        account = wallet;
      });
      getCategories();
    } else {
      setState(() {
        widgetIndex = WidgetIndex.login;
        overlay = true;
      });
    }
  }

  void writeLogin(String wallet) async {
    final storage = new FlutterSecureStorage();
    await storage.write(key: "wallet", value: wallet);
    setState(() {
      widgetIndex = WidgetIndex.collections;
      account = wallet;
    });
    getCategories();
  }

  void onUnityMessage(message) {
    switch (message.toString()) {
      case "overlay":
        setState(() {
          overlay = true;
          widgetIndex = WidgetIndex.collections;
          background = "";
        });
        break;
      case "start":
        setState(() {
          if (widgetIndex == WidgetIndex.login) {
            overlay = true;
          } else {
            widgetIndex = WidgetIndex.collections;
            background = "";
          }
        });
        break;
      case "settings":
        setState(() {
          overlay = true;
          widgetIndex = WidgetIndex.settings;
        });
        break;
      default:
    }
  }
}